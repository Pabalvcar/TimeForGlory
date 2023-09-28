using Dan.Main;
using Dan.Models;
using TMPro;
using UnityEngine;

namespace Dan.Demo
{
    public class LeaderboardShowcase : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerScoreText;
        [SerializeField] private TextMeshProUGUI[] _entryFields;
        
        [SerializeField] private TMP_InputField _playerUsernameInput;

        [SerializeField] private TextMeshProUGUI _personalEntryText;

        private int _playerScore;
        
        private void Start()
        {
            Load();
        }

        public void AddPlayerScore()
        {
            _playerScore++;
            _playerScoreText.text = "Your score: " + _playerScore;
        }
        
        public void Load() => Leaderboards.DemoSceneLeaderboard.GetEntries(OnLeaderboardLoaded);

        private void OnLeaderboardLoaded(Entry[] entries)
        {
            foreach (var entryField in _entryFields)
            {
                entryField.text = "";
            }
            
            for (int i = 0; i < entries.Length; i++)
            {
                _entryFields[i].text = $"{entries[i].RankSuffix()}. {entries[i].Username} : {entries[i].Score}";
            }
        }

        public void Submit()
        {
            Leaderboards.DemoSceneLeaderboard.UploadNewEntry(_playerUsernameInput.text, _playerScore, Callback, ErrorCallback);
        }
        
        public void DeleteEntry()
        {
            Leaderboards.DemoSceneLeaderboard.DeleteEntry(Callback, ErrorCallback);
        }

        public void ResetPlayer()
        {
            LeaderboardCreator.ResetPlayer();
        }

        public void GetPersonalEntry()
        {
            Leaderboards.DemoSceneLeaderboard.GetPersonalEntry(OnPersonalEntryLoaded);
        }

        private void OnPersonalEntryLoaded(Entry entry)
        {
            _personalEntryText.text = $"{entry.Rank}. {entry.Username} : {entry.Score}";
        }
        
        private void Callback(bool success)
        {
            if (success)
                Load();
        }
        
        private void ErrorCallback(string error)
        {
            Debug.LogError(error);
        }
    }
}
