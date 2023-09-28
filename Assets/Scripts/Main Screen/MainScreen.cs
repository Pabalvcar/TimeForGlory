using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{

    public void LoadGameScene()
    {
        SceneManager.LoadScene("LevelGen");
    }

    public void LoadLeaderboardsScene()
    {
        PlayerPrefs.SetInt("Score", 0);
        SceneManager.LoadScene("LeaderboardsUI");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
