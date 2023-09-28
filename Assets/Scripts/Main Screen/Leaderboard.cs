using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{

    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private TMP_InputField inputName;
    [SerializeField]
    private TMP_Text scoreText;

    private string publicKey = "7be561b002dde4319e1a95e291a01085786f4a23d758a08159d8812cc65c8452";

    private void Start()
    {
        GetLeaderboard();
        scoreText.SetText("Planta alcanzada: " + PlayerPrefs.GetInt("Score"));
    }

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicKey, ((msg) =>
        {
            int leaderboardLength = names.Count;
            if (msg.Length < leaderboardLength)
                leaderboardLength = msg.Length;
            for (int i = 0; i < leaderboardLength; i++)
            {
                names[i].text = (i + 1) + ". " + msg[i].Username + " - Planta " + msg[i].Score;
            }
        }));
    }

    public void UploadToLeaderboard(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicKey, username, score, ((msg) =>
        {
            GetLeaderboard();
        }));
    }

    public void SubmitScore()
    {
        UploadToLeaderboard(inputName.text, PlayerPrefs.GetInt("Score"));
    }

    public void LoadMainScreen()
    {
        SceneManager.LoadScene("MainScreen");
    }
}
