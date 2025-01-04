using Model;
using System.Collections.Generic;
using UnityEngine;

namespace Application
{
    namespace Leaderboard
    {
        public class LeaderboardUI : MonoBehaviour
        {
            [SerializeField] private GameObject _leaderboardUserPrefab;
            [SerializeField] private Transform _leaderboardContentTransform;

            public void LoadLeaderBoard(List<PublicUserInfo> userInfo)
            {
                for (int i = 0; i < userInfo.Count; i++)
                {
                    GameObject obj = Instantiate(_leaderboardUserPrefab, _leaderboardContentTransform);
                    obj.GetComponent<LeaderBoardUser>().Initialize(i, userInfo[i]);
                }
            }

        }

    }
}