using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Score Text")]
    [SerializeField] private Text _scoreText;
    [Header("Lives")]
    [SerializeField] private Sprite[] _livesSprite;
    [SerializeField] private Image _livesImage;
    [Header("Pause Menu")]
    [SerializeField] private bool _paused;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_paused)
        {
            Time.timeScale = 0;
            _paused = true;
            //open the pause menu
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _paused)
        {
            Time.timeScale = 1;
            _paused = false;
        }

    }

    public void UpdateScore(int PlayerScore)
    {
        _scoreText.text = "Score: " + PlayerScore;
       // _scoreText.text = "Score: " + PlayerScore.ToString();
    }

    public void UpdateLives(int CurrentLives)
    {
        _livesImage.sprite = _livesSprite[CurrentLives];
    }
}
