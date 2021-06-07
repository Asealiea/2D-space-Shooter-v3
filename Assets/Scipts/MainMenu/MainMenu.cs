using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
 
    public void ExitGameButton()

    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // makes it stop while in test play
#else
		    Application.Quit();// quits the game (only works after it's been built)
#endif
    }

    public void StartGameButton()
    {
        SceneManager.LoadScene(1); //First level
        Time.timeScale = 1;
    }
}
