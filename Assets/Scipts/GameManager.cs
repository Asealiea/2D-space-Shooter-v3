using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _gameOver = false;
    [SerializeField] private GameObject _spawnManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _gameOver)
        {
            SceneManager.LoadScene(1);//First level
        }

    }

    public void GameOver()
    {
        _gameOver = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(1);//First game level
    }

    public void StartGame()
    {
        _spawnManager.SetActive(true);
    }
    
    public void Menu()
    {
        SceneManager.LoadScene(0);//menu screen
    }
}
