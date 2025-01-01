using Model;
using Network;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Collections.Generic;
using UnityEngine;

namespace Application
{
    namespace Leaderboard
    {
        public class LeaderboardManager : MonoBehaviour
        {
            private LeaderboardUI _leaderboardUI => GetComponent<LeaderboardUI>();

            private void Start()
            {
                SendLeaderboardRequest();
            }

            public void SendLeaderboardRequest()
            {
                NetworkAPI.SendRequest(new ExtensionRequest("getLeaderboard", SFSObject.NewInstance()), new List<NetworkEventSubscription> { new NetworkEventSubscription(SFSEvent.EXTENSION_RESPONSE, LeaderBoardLoaded) });
            }
            private void LeaderBoardLoaded(BaseEvent e)
            {
                SFSObject param = (SFSObject)e.Params["params"];
                ISFSArray userArray = param.GetSFSArray("users");
                var userInfoList = new List<PublicUserInfo>();
                foreach (var user in userArray)
                {
                    SFSObject userObject = (SFSObject)user;
                    var userInfo = new PublicUserInfo(userObject.GetUtfString("username"), userObject.GetInt("score"));
                    userInfoList.Add(userInfo);
                }
                _leaderboardUI.LoadLeaderBoard(userInfoList);
            }

        }

    }
}