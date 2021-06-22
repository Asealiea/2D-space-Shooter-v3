using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnManager : MonoBehaviour
{

    public enum SpawnState { Spawning, Waiting, Cooldown };
    public SpawnState state = SpawnState.Cooldown;

    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject enemy;
        public int count;
        public float spawnRate;

        public GameObject[] _enemy;
        public int[] _count;
    }
    public Wave[] waves;
    private int nextWave = 0;
    [SerializeField] private float cooldownBetweenWaves = 5f;
    private float waveCooldown;
    private float searchCooldown = 1f;

    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Player _player;




  
    private bool _spawn = true;
    private float _randomWait;
    private int _powerID;
    private int _randomID;
    private int _total;



    [Header("Game Objects")]
  //  [SerializeField] private GameObject _enemyPreFab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerUpContainer;



    /*
    [Header ("Background Objects")]
    [SerializeField] private float delaytimebg = 4f;
    private WaitForSeconds  _spawnDelaybg;
    [SerializeField] private float delaytimebgSmall = 2f;
    private WaitForSeconds  _spawnDelaybgSmall;
    [SerializeField] private GameObject[] _backgroundScene;
    [SerializeField] private GameObject[] _smallBackGroundScene;
    */


    // Start is called before the first frame update
    void Start()
    {
        NullCheck();
        waveCooldown = cooldownBetweenWaves;
        StartCoroutine(SpawnPowerUpRoutine());
        updateUI(waves[nextWave]);
        _player = GameObject.Find("Player").GetComponent<Player>();
       // Time.timeScale = 100;

    }
    /*
            _spawnDelay = new WaitForSeconds(_delaytime);
            StartCoroutine(SpawnRoutine());
            _spawnDelaybg = new WaitForSeconds(delaytimebg);
            _spawnDelaybgSmall = new WaitForSeconds(delaytimebgSmall);
             StartCoroutine(BackGroundObjects());
            StartCoroutine(SmallBackgroundObjects());
    */

    private void Update()
    {
        if (state == SpawnState.Waiting)
        {
            if (!EnemyIsAlive()) // same as writing EnemyIsAlive() == false
            {
                WaveCompleted();
                return;
            }
            else
            {
                return;
            }
        }

        if (waveCooldown <= 0)
        {
            if (state != SpawnState.Spawning && _spawn)           
                StartCoroutine(SpawnWave(waves[nextWave]));
        }
        else
        {
            waveCooldown -= Time.deltaTime;
        }
      

    
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P)) // spawn a random power up, only works in editor
        {
            Vector3 randomX = new Vector3(Random.Range(-7f, 7f), 8, 0);
            //_powerID = Random.Range(0, _powerUpContainer.Length);
            Instantiate(_powerUpContainer[5], randomX, Quaternion.identity);
        }
#endif

    }
    /* background objects to add in later
        IEnumerator BackGroundObjects()
        {
            while (_spawn == true)
            {
                yield return _spawnDelaybg;
                int randomBg = Random.Range(0, _backgroundScene.Length);
                Instantiate(_backgroundScene[randomBg], transform.position, Quaternion.identity);
            }
        }

        IEnumerator SmallBackgroundObjects()
        {
            while (_spawn == true)
            {
                yield return _spawnDelaybg;
                int randomBg = Random.Range(0, _smallBackGroundScene.Length);
                Instantiate(_smallBackGroundScene[randomBg], transform.position, Quaternion.identity);
            }
        } */

    /* Spawn Rotine.
        IEnumerator SpawnRoutine()
        {
            while (_spawn == true)
            {
                yield return _spawnDelay;
                Transform newEnemy = Instantiate(_enemy, transform.position, Quaternion.identity);
                newEnemy.parent = _enemyContainer.transform;

            }
        }
    */

      IEnumerator SpawnPowerUpRoutine()
      {
          yield return new WaitForSeconds(3f);
          while (_spawn == true)
          {
              //Debug.Log("Spawns");
              Vector3 randomX = new Vector3(Random.Range(-7f, 7f), 8, 0);

              _randomID = Random.Range(0, 1001);
              if (_randomID >= 850 && _total > 3 || _total == 10)// star burst
              {
                  _powerID = 6;
                  Instantiate(_powerUpContainer[6], randomX, Quaternion.identity); // star powerup
                  //Debug.Log(_powerID + " Above 850  :   " + _powerUpContainer[6].name);
                  _total = 5;
              }
              else if (_randomID < 850 && _randomID >= 550 && _total > 1) //extra lifes and extra Missiles.
              {
                  _powerID = Random.Range(4, 6);
                  Instantiate(_powerUpContainer[_powerID], randomX, Quaternion.identity);
                  //Debug.Log(_powerID + " between 550 and 849  :  " + _powerUpContainer[_powerID].name);
                  _total = 0;
              }
              else
              {
                  _powerID = Random.Range(0, _powerUpContainer.Length - 3); // triple shot, speed, shields and Ammo;
                  Instantiate(_powerUpContainer[_powerID], randomX, Quaternion.identity);
                  //Debug.Log(_powerID + " between 0 and 749  :   " + _powerUpContainer[_powerID].name);
                  _total++;
              }
              _randomWait = (Random.Range(5f, 10f));
              yield return new WaitForSeconds(_randomWait);      
          }
      } 



    public void StopSpawning()
    {
        _spawn = false;
    }

    public void NullCheck()
    {
//        if (_enemyPreFab == null)
//            Debug.LogError("SpawnManager:Enemy PreFab is null");
        if (_enemyContainer == null)
            Debug.LogError("SpawnManager:Enemy Container is null");
        if (_powerUpContainer == null)
            Debug.LogError("SpawnManager:PowerUp Container is null");

    }

    

    //SpawnSystem

 

    void WaveCompleted()
    {
        if (_spawn)
        {
            
            //Debug.Log("Wave Completed!");
            state = SpawnState.Cooldown; 
            waveCooldown = cooldownBetweenWaves;            
            if (nextWave + 1 > waves.Length - 1)
            {
                nextWave = 0; // loops waves, will add in a Congrats on winnning later.  
                updateUI(waves[nextWave]);
            }
            else
            {
                nextWave++;
                updateUI(waves[nextWave]);
                _player.LongerWaitTime(nextWave / 2 + 3);
                
            } 
        }
    }

    bool EnemyIsAlive()
    {
        searchCooldown -= Time.deltaTime; 
        if (searchCooldown <= 0f) 
        {
            searchCooldown = 1f; 
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0) 
            {
                return false; 
            }
        }
        return true; 
    }
    IEnumerator TestEnum(Wave _wave)
    {
        state = SpawnState.Spawning;
        for (int q = 0; q < _wave._enemy.Length; q++)
        {
            for (int i = 0; i < _wave._count[q]; i++)
            {
                SpawnEnemy(_wave._enemy[q]);
                yield return new WaitForSeconds(1f);
            }

        }
        state = SpawnState.Waiting;
        yield break;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
       
//        Debug.Log("Spawning Wave: " + _wave.name);             
        state = SpawnState.Spawning;
        //Spawn
        for (int q = 0; q < _wave._enemy.Length; q++)
        {
            for (int i = 0; i < _wave._count[q]; i++)
            {
                SpawnEnemy(_wave._enemy[q]);
                yield return new WaitForSeconds(1f / _wave.spawnRate);
            }
        }
      /*  for (int i = 0; i < _wave.count; i++)
        {
            if (_spawn)
            {
                SpawnEnemy(_wave.enemy); 
                yield return new WaitForSeconds(1f / _wave.spawnRate); 
            }
        } */
       /* for (int i = 0; i < _wave.count[i]; i++)
        {
            if (_spawn)
            {
                SpawnEnemy(_wave.enemy);
                yield return new WaitForSeconds(1f / _wave.spawnRate);
            }
        } */
        state = SpawnState.Waiting;                    
        yield break;

    }

    void SpawnEnemy(GameObject _enemy)
    {
        //         Spawn Enemy
        //        Debug.Log("Spawning Enemy: " + _enemy.name);

        GameObject newEnemy = Instantiate(_enemy, new Vector3(0,8,0), Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
    }

    private void updateUI(Wave _wave)
    {
        _uiManager.UpdateWave(_wave.name);
    }
    



}
