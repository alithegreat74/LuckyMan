using Model;
using TMPro;
using UnityEngine;

namespace Application
{
    namespace Leaderboard
    {
        public class LeaderBoardUser : MonoBehaviour
        {
            [SerializeField] private TextMeshProUGUI _indexText;
            [SerializeField] private TextMeshProUGUI _usernameText;
            [SerializeField] private TextMeshProUGUI _scoreText;


            public void Initialize(int index, PublicUserInfo userInfo)
            {
                _indexText.text = (index+1).ToString();
                _usernameText.text = userInfo.Username;
                _scoreText.text = userInfo.Score.ToString();
            }
        }
    }
}
