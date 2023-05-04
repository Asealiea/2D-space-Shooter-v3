using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asealiea.Waves
{



    public class WavesManager : MonoBehaviour
    {

        private enum SpawnState { Spawning, Waiting, Cooldown };
        private SpawnState state = SpawnState.Cooldown;
        [System.Serializable]
        public class Wave
        {
            //names of the waves (can be used to show the wave number/name between the waves)
            [Tooltip("Names of the waves, can be used to show the player what wave they are on.")]
            public string name; 
            //the enemies that will get spawned
            [Tooltip("GameObjects that will get spawned")]
            public GameObject[] _enemy; 
            //how many of the enemy will spawn
            [Tooltip("How many of each GameObject will get spawned")]
            public int[] _count;
            //how fast the enemies will spawn
            [Tooltip("How fast the GameObjects will spawn")]
            public float spawnRate;
        }
        [SerializeField] private Wave[] waves;
        // what wave comes next (does automaticly)
        private int nextWave = 0; 
        //cooldown between the end of a wave and a new wave starts.
        [Tooltip("How long before the GameObjects will spawn")]
        [SerializeField] private float cooldownBetweenWaves = 5f;
        //variable that the time gets removed from.
        private float waveCooldown; 
        //how often the game looks to see if there are enimies alive.
        private float searchCooldown = 1f; 
        //should we be spawning enemies or not.
        private bool _spawn = true; 

        //[SerializeField] private UIManager _uiManager;
        [Header("Game Objects")]
        [SerializeField] private GameObject _enemyContainer;



        void Start()
        {
           
            NullCheck();
            waveCooldown = cooldownBetweenWaves;

          //  updateUI(waves[nextWave]);
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
                if (state != SpawnState.Spawning && _spawn)
                    StartCoroutine(SpawnWave(waves[nextWave]));
            }
            else
                waveCooldown -= Time.deltaTime;
        }

        public void StopSpawning()
        {
            _spawn = false;
        }

        public void NullCheck()
        {
            if (_enemyContainer == null)
                Debug.LogError("SpawnManager:Enemy Container is null");

        }

        #region Spawn Waves System
        void WaveCompleted()
        {
            if (_spawn)
            {
                state = SpawnState.Cooldown;
                waveCooldown = cooldownBetweenWaves;
                if (nextWave + 1 > waves.Length - 1)
                {
                    nextWave = 0; // loops waves, will add in a Congrats on winnning later.  
                  //  updateUI(waves[nextWave]);
                }
                else
                {
                    nextWave++;
                   // updateUI(waves[nextWave]);
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

        IEnumerator SpawnWave(Wave _wave)
        {
            state = SpawnState.Spawning;
            if (_spawn)
            {
                for (int q = 0; q < _wave._enemy.Length; q++)
                {
                    for (int i = 0; i < _wave._count[q]; i++)
                    {
                        SpawnEnemy(_wave._enemy[q]);
                        yield return new WaitForSeconds(1f / _wave.spawnRate);
                    }
                }
            }
            state = SpawnState.Waiting;
            yield break;
        }

        void SpawnEnemy(GameObject _enemy)
        {
            if (_spawn)
            {
                Vector3 random = new Vector3(Random.Range(-9f, 9f), 8, 0);
                GameObject newEnemy = Instantiate(_enemy, random, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;

            }
        }
        /*
        private void updateUI(Wave _wave)
        {
            _uiManager.UpdateWave(_wave.name);
        }
        */
        #endregion




    }
}