using System;
using System.Collections;
using System.Collections.Generic;
using RSG.Trellis.Signals;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    [Header("Thruster Bar")] 
    [SerializeField] private FloatSignal thruster;
    [SerializeField] private Slider _slider;
    [Header("Score Text")]
    [SerializeField] private Text _scoreText;
    [SerializeField] private IntSignal playerScore;
    [Header("Ammo Count Text")]
    [SerializeField] private Text _ammoCount;
    [SerializeField] private IntSignal ammoCount;
    [SerializeField] private Text _missileCount;
    [SerializeField] private IntSignal missileCount;
    [Header("Lives")] 
    [SerializeField] private IntSignal playerLives;
    [SerializeField] private Sprite[] _livesSprite;
    [SerializeField] private Image _livesImage;
    [Header("Waves")]
    [SerializeField] private Text _waveText;
    [Header("Pause Menu")]
    [SerializeField] private bool _paused;
    [SerializeField] private GameObject _pauseMenu;
    [Header("Options Menu")]
    [SerializeField] private GameObject _optionMenu;
    [SerializeField] private CameraShake _camera;
    [SerializeField] private Toggle _cameraShakeToggle;
    [Header("Game Over")]
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private BoolSignal gameOverSignal;
    //magnet.
    [Header("Magnet")]
    [SerializeField] private BoolSignal magnetSignal;
    [SerializeField] private GameObject _magnet;
    [SerializeField] private GameObject _tutorialMenu;

    private void OnEnable()
    {
        thruster.AddListener(ThrusterUpdate);
        playerLives.AddListener(UpdateLives);
        playerScore.AddListener(UpdateScore);
        missileCount.AddListener(UpdateMissile);
        ammoCount.AddListener(UpdateAmmo);
        gameOverSignal.AddListener(GameOver);
        magnetSignal.AddListener(MagnetOn);
    }

    private void OnDisable()
    {
        
        thruster.RemoveListener(ThrusterUpdate);
        playerLives.RemoveListener(UpdateLives);
        playerScore.RemoveListener(UpdateScore);
        missileCount.RemoveListener(UpdateMissile);
        ammoCount.RemoveListener(UpdateAmmo);
        gameOverSignal.RemoveListener(GameOver);
        magnetSignal.RemoveListener(MagnetOn);
    }


    // Start is called before the first frame update
    private void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        //UpdateLives(3);
        _scoreText.text = "Score: 0";
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
            Debug.LogError("UIManager: GameManger is null");
    
    }

    // Update is called once per frame
    private void Update()
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

    private void UpdateMissile()
    {
        _missileCount.text = missileCount.Value + "/5";
    }

    private void UpdateAmmo()
    {
        _ammoCount.text =  ammoCount.Value + "/30";
    }

    private void UpdateScore()
    {
        _scoreText.text = "Score: " + playerScore.Value;
    }

    private void UpdateLives()
    {
        _livesImage.sprite = _livesSprite[playerLives.Value];
    }

    private void GameOver()
    {
        if (!gameOverSignal.Value) return;

        StartCoroutine(FlashyEnd());
        _restartText.gameObject.SetActive(true);
    }

    private IEnumerator FlashyEnd()
    {
        while (gameOverSignal.Value)
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
        _paused = false;
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

    public void OptionsButton()
    {
        _pauseMenu.SetActive(false);
        _optionMenu.SetActive(true);
        _tutorialMenu.SetActive(false);
    }

    public void CameraShakeToggle(bool camera)
    {
        _camera.CameraShakeOn(_cameraShakeToggle.isOn);
    }

    public void BackButtonOptions()
    {
        _pauseMenu.SetActive(true);
        _optionMenu.SetActive(false);
    }

    private void ThrusterUpdate()
    {
        _slider.value = thruster.Value;
    }

    public void UpdateWave(string Name)
    {
        // update text

        _waveText.text = Name;
        StartCoroutine(WaveTextCoolDown());
    }

    private IEnumerator WaveTextCoolDown()
    {
        yield return new WaitForSeconds(5f);
        _waveText.text = "";
    }

    private void MagnetOn()
    {
        _magnet.SetActive(!magnetSignal.Value);
    }


    public void TutorialButton()
    {
        _pauseMenu.SetActive(false);
        _optionMenu.SetActive(false);
        _tutorialMenu.SetActive(true);
    }

    



}
