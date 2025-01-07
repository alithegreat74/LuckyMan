using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using Network;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;

namespace Application
{
    namespace Leaderboard
    {
        public class LeaderboardManager : MonoBehaviour
        {
            private LeaderboardUI _leaderboardUI;

            private async Task Start()
            {
                _leaderboardUI = GetComponent<LeaderboardUI>();
                await SendLeaderboardRequest();
            }

            public async Task SendLeaderboardRequest()
            {
                try
                {
                    BaseEvent result = await SmartFoxNetworkAPI.SendRequest(
                        new ExtensionRequest("getLeaderboard", SFSObject.NewInstance()),
                        SFSEvent.EXTENSION_RESPONSE
                    );
                    LeaderBoardLoaded(result);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }

            private void LeaderBoardLoaded(BaseEvent e)
            {
                SFSObject param = (SFSObject)e.Params["params"];
                ISFSArray userArray = param.GetSFSArray("users");
                var userInfoList = new List<PublicUserInfo>();
                foreach (var user in userArray)
                {
                    SFSObject userObject = (SFSObject)user;
                    var userInfo = new PublicUserInfo(
                        userObject.GetUtfString("username"),
                        userObject.GetInt("xp")
                    );
                    userInfoList.Add(userInfo);
                }
                _leaderboardUI.LoadLeaderBoard(userInfoList);
            }
        }
    }
}
