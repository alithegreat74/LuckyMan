using System;
using System.Collections.Generic;
using Model;
using Network;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;

namespace Application
{
    namespace Match
    {
        public class MatchManager : MonoBehaviour
        {
            private MatchUI _matchUI;
            private Dice _dice;

            private UserMatchVariables _playerVariables = new UserMatchVariables(0, 0);
            private UserMatchVariables _opponentVariables = new UserMatchVariables(0, 0);

            private void Awake()
            {
                _matchUI = GetComponent<MatchUI>();
                _dice = GetComponentInChildren<Dice>();
            }

            private void Start()
            {
                //TODO: Handle events in a different class?
                SmartFoxNetworkAPI.SubscribeToEvent(new NetworkEventSubscription(SFSEvent.USER_EXIT_ROOM, UserLeftGame));
                SmartFoxNetworkAPI.SubscribeToEvent(new NetworkEventSubscription(SFSEvent.USER_VARIABLES_UPDATE, UserVariableUpdated));
                User myself = SmartFoxNetworkAPI.GetMyself();
                Room currentRoom = SmartFoxNetworkAPI.GetCurrentRoom();
                List<User> userList = currentRoom.PlayerList;
                bool isPlayersTurn = currentRoom.GetVariable("startingUser").GetIntValue() == myself.Id;
                _matchUI.InitializeUI(myself.Name, userList[0].Id == myself.Id ? userList[1].Name : userList[0].Name, PlayButtonClicked, ReturnToMainMenu, isPlayersTurn);
            }

            private void OnDestroy()
            {
                SmartFoxNetworkAPI.UnSubscribeFromEvent(new NetworkEventSubscription(SFSEvent.USER_EXIT_ROOM, UserLeftGame));
                SmartFoxNetworkAPI.UnSubscribeFromEvent(new NetworkEventSubscription(SFSEvent.USER_VARIABLES_UPDATE, UserVariableUpdated));
            }

            private async void UserLeftGame(BaseEvent e)
            {
                var room = (Room)e.Params["room"];
                var user = (User)e.Params["user"];
                if (!room.IsGame && user.Id == SmartFoxNetworkAPI.GetMyself().Id)
                    return;

                SmartFoxNetworkAPI.UnSubscribeFromEvent(new NetworkEventSubscription(SFSEvent.USER_EXIT_ROOM, UserLeftGame));
                try
                {
                    await SmartFoxNetworkAPI.SendRequest(new LeaveRoomRequest(), SFSEvent.USER_EXIT_ROOM);
                    SceneManager.LoadScene("Main Menu");
                }
                catch (Exception exception)
                {
                    Debug.Log(exception);
                }
            }
            private async void PlayButtonClicked()
            {
                NetworkResult result = 
                    await SmartFoxNetworkAPI.SendRequest(new ExtensionRequest("rollDice", new SFSObject(), SmartFoxNetworkAPI.GetCurrentRoom()),SFSEvent.EXTENSION_RESPONSE);

                if(!result.Success())
                    return;
                _dice.RollDice(result.GetIntResult("diceNumber"));
            }
            private void UserVariableUpdated(BaseEvent e)
            {
                var user = (User)e.Params["user"];
                bool playersTurn = false;
                if (user.Id == SmartFoxNetworkAPI.GetMyself().Id)
                {
                    _playerVariables.FromSFSUserVariable(user.GetVariables());
                    playersTurn = false;
                }
                else
                {
                    _opponentVariables.FromSFSUserVariable(user.GetVariables());
                    playersTurn = true;
                }
                _matchUI.UpdateUI(_playerVariables, _opponentVariables, playersTurn);
            }

            private async void ReturnToMainMenu()
            {
                try
                {
                    NetworkResult result = await SmartFoxNetworkAPI.SendRequest(new LeaveRoomRequest(), SFSEvent.USER_EXIT_ROOM);
                    SceneManager.LoadScene("Main Menu");
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    UnityEngine.Application.Quit();
                }
            }
        }
    }
}
