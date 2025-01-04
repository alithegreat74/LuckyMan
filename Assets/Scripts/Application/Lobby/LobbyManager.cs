using Network;
using Sfs2X.Requests;
using Sfs2X.Entities;
using UnityEngine;
using System.Collections.Generic;
using Model;
using Sfs2X.Core;

namespace Application
{
    namespace Lobby
    {
        public class LobbyManager : MonoBehaviour
        {
            private void Start()
            {
                JoinLobbyRequest();
                NetworkAPI.SubscribeToEvent(new NetworkEventSubscription(SFSEvent.ROOM_JOIN, UserJoinedRoom));
            }


            private void OnDestroy()
            {
                NetworkAPI.UnSubscribeFromEvent(new NetworkEventSubscription(SFSEvent.ROOM_JOIN, UserJoinedRoom));
            }
            private void JoinLobbyRequest()
            {
                NetworkAPI.SendRequest(new JoinRoomRequest(0),
                    new List<NetworkEventSubscription>());
            }
            private void UserJoinedRoom(BaseEvent e)
            {
                Room room = (Room)e.Params["room"];
                if (room.IsGame)
                    LoadGameScene();
                else
                    Debug.Log("Waiting for other people to join");

            }
            private void LoadGameScene()
            {
                SceneLoader.LoadScene("Game");
            }
            
        }

    }
}