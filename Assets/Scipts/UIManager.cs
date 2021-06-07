using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    [Header("Score Text")]
    [SerializeField] private Text _scoreText;
    [Header("Lives")]
    [SerializeField] private Sprite[] _livesSprite;
    [SerializeField] private Image _livesImage;
    [Header("Pause Menu")]
    [SerializeField] private bool _paused;
    [SerializeField] private GameObject _pauseMenu;
    [Header("Game Over")]
    private bool _gameOver = false;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private GameManager _gameManager;



    // Start is called before the first frame update
    void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        UpdateLives(3);
        _scoreText.text = "Score: 0";
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
            Debug.Log("UIManager: GameManger is null");      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_paused)
        {
            Time.timeScale = 0;
            _paused = true;
            _pauseMenu.SetActive(true);
            //open the pause menu
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _paused)
        {
            Time.timeScale = 1;
            _paused = false;
            _pauseMenu.SetActive(false);
        }
    }

    public void UpdateScore(int PlayerScore)
    {
        _scoreText.text = "Score: " + PlayerScore.ToString();
    }

    public void UpdateLives(int CurrentLives)
    {
        _livesImage.sprite = _livesSprite[CurrentLives];
    }

    public void GameOver()
    {
        _gameOver = true;
        StartCoroutine(FlashyEnd());
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }
        
    IEnumerator FlashyEnd()
    {
        while (_gameOver == true)
        {
            _gameOverText.color = Color.yellow;
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f); 
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            _gameOverText.color = Color.red;
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
    }

    public void RestartGameButton()
    {
        _gameManager.RestartLevel();
        Time.timeScale = 1;
    }

    public void MenuButton()
    {
        _gameManager.Menu();
    }

    public void ExitGameButton()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // makes it stop while in test play
#else
		    Application.Quit();// quits the game (only works after it's been built)
#endif
    }



}
