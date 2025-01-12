using System;
using System.Threading.Tasks;
using Model;
using Network;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Application
{
    namespace Lobby
    {
        public class LobbyManager : MonoBehaviour
        {
            private async Task Start()
            {
                SmartFoxNetworkAPI.SubscribeToEvent(
                    new NetworkEventSubscription(SFSEvent.ROOM_JOIN, UserJoinedRoom)
                );
                await JoinLobbyRequest();
            }

            private void OnDestroy()
            {
                SmartFoxNetworkAPI.UnSubscribeFromEvent(
                    new NetworkEventSubscription(SFSEvent.ROOM_JOIN, UserJoinedRoom)
                );
            }

            private async Task JoinLobbyRequest()
            {
                try
                {
                    await SmartFoxNetworkAPI.SendRequest(new JoinRoomRequest(0), SFSEvent.ROOM_JOIN);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }

            private void UserJoinedRoom(BaseEvent e)
            {
                Room room = (Room)e.Params["room"];
                if (room.IsGame)
                    SceneManager.LoadScene("Game");
                else
                    Debug.Log("Waiting for other people to join");
            }
        }
    }
}
