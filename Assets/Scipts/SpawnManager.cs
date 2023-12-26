using System;
using System.Collections;
using System.Collections.Generic;
using RSG.Trellis.Signals;
using UnityEngine;
using Random = UnityEngine.Random;


public class SpawnManager : MonoBehaviour
{

    private enum SpawnState { Spawning, Waiting, Cooldown };
    private SpawnState state = SpawnState.Cooldown;
    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject[] _enemy;
        public int[] _count;
        public float spawnRate;
    }
    [SerializeField] private Wave[] waves;
    private int nextWave = 0;
    [SerializeField] private float cooldownBetweenWaves = 5f;
    private float waveCooldown;
    private float searchCooldown = 1f;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Player _player;
   // private bool _spawn = true;
    private float _randomWait;
    private int _powerID;
    private int _randomID;
    private int _total;
    [Header("Game Objects")]
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerUpContainer;
    [SerializeField] private BoolSignal spawnSignal;



    void Start()
    {
        //_player = GameObject.Find("Player").GetComponent<Player>();
        _player = FindObjectOfType<Player>();
        NullCheck();
        waveCooldown = cooldownBetweenWaves;
        StartCoroutine(SpawnPowerUpRoutine());
        UpdateUI(waves[nextWave]);
    }

    private void Update()
    {
        if (state == SpawnState.Waiting)
        {
            if (!EnemyIsAlive()) 
            {
                WaveCompleted();
                return;
            }
            else
                return;                        
        }

        if (waveCooldown <= 0)
        {
            if (state != SpawnState.Spawning && spawnSignal.Value)           
                StartCoroutine(SpawnWave(waves[nextWave]));
        }
        else
        {
            waveCooldown -= Time.deltaTime;
        }
    }

    private IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (spawnSignal.Value)
        {
            Vector3 randomX = new Vector3(Random.Range(-7f, 7f), 8, 0);
            _randomID = Random.Range(0, 1001);
            if (_randomID >= 850 && _total > 3 || _total == 10)// star burst
            {
                _powerID = 7;
                ObjectPool.SpawnObject(randomX,Quaternion.identity, "Super");
                
                //Instantiate(_powerUpContainer[_powerUpContainer.Length - 1], randomX, Quaternion.identity); // star powerup                 
                _total = 1;
            }
            else if (_randomID < 850 && _randomID >= 550 && _total > 1) //extra lifes and extra Missiles.
            {
                _powerID = Random.Range(4, 7);
                ObjectPool.SpawnObject(randomX,Quaternion.identity, _powerUpContainer[_powerID].tag);
                
                //Instantiate(_powerUpContainer[_powerID], randomX, Quaternion.identity);
                _total = 0;
            }
            else
            {
                _powerID = Random.Range(0, _powerUpContainer.Length - 4); // triple shot, speed, shields and Ammo;
                ObjectPool.SpawnObject(randomX,Quaternion.identity, _powerUpContainer[_powerID].tag);

                //Instantiate(_powerUpContainer[_powerID], randomX, Quaternion.identity);
                _total++;
            }
            _randomWait = (Random.Range(5f, 10f));
            yield return new WaitForSeconds(_randomWait);
        }
    }


    private void NullCheck()
    {
        if (_enemyContainer == null)
            Debug.LogError("SpawnManager:Enemy Container is null");
        if (_powerUpContainer == null)
            Debug.LogError("SpawnManager:PowerUp Container is null");
        if (_player == null)
            Debug.LogError("SpawnManager:Player is null");
        
        
    }

    #region Spawn Waves System

    private void WaveCompleted()
    {
        if (!spawnSignal.Value) return;
        
        state = SpawnState.Cooldown; 
        waveCooldown = cooldownBetweenWaves;            
        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0; // loops waves, will add in a Congrats on winnning later.  
            UpdateUI(waves[nextWave]);
        }
        else
        {
            nextWave++;
            UpdateUI(waves[nextWave]);
            _player.LongerWaitTime(nextWave / 2 + 3);                
        }
    }

    private bool EnemyIsAlive()
    {
        searchCooldown -= Time.deltaTime;
        if (!(searchCooldown <= 0f)) return true;
        searchCooldown = 1f; 
            
        return FindObjectsOfType<Enemy>().Length != 0;
    }

    private IEnumerator SpawnWave(Wave wave)
    {                
        state = SpawnState.Spawning;
        if (spawnSignal.Value)
        {
            for (int q = 0; q < wave._enemy.Length; q++)
            {
                for (int i = 0; i < wave._count[q]; i++)
                {
                    SpawnEnemy(wave._enemy[q]);
                    yield return new WaitForSeconds(1f / wave.spawnRate);
                }
            }
        }
        state = SpawnState.Waiting;                    
        yield break;
    }

    private void SpawnEnemy(GameObject enemy)
    {
        if (!spawnSignal.Value) return;
        Vector3 random = new Vector3(Random.Range(-9f, 9f), 8, 0);

        ObjectPool.SpawnObject(random,Quaternion.identity, enemy.tag);
        
        //GameObject newEnemy = Instantiate(_enemy, random , Quaternion.identity);
        //newEnemy.transform.parent = _enemyContainer.transform;
    }

    private void UpdateUI(Wave wave)
    {
        _uiManager.UpdateWave(wave.name);
    }
    #endregion
}
