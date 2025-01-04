using Network;
using Sfs2X.Core;
using UnityEngine;
using Model;
using Sfs2X.Entities;
using System.Collections.Generic;
using Sfs2X.Requests;

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
                NetworkAPI.SubscribeToEvent(new NetworkEventSubscription(SFSEvent.USER_EXIT_ROOM, UserLeftGame));
                NetworkAPI.SubscribeToEvent(new NetworkEventSubscription(SFSEvent.USER_VARIABLES_UPDATE, UserVariableUpdated));
                User myself = NetworkAPI.GetMyself();
                Room currentRoom = NetworkAPI.GetCurrentRoom();
                List<User> userList = currentRoom.PlayerList;
                bool isPlayersTurn = currentRoom.GetVariable("startingUser").GetIntValue() == myself.Id;
                _matchUI.InitializeUI(myself.Name, userList[0].Id == myself.Id ? userList[1].Name : userList[0].Name, PlayButtonClicked, isPlayersTurn);
            }
            private void OnDestroy()
            {
                NetworkAPI.UnSubscribeFromEvent(new NetworkEventSubscription(SFSEvent.USER_EXIT_ROOM, UserLeftGame));
                NetworkAPI.UnSubscribeFromEvent(new NetworkEventSubscription(SFSEvent.USER_VARIABLES_UPDATE, UserVariableUpdated));
            }

            private void UserLeftGame(BaseEvent e)
            {
                var room = (Room)e.Params["room"];
                if (room.IsGame)
                    return;
                SceneLoader.LoadScene("Main Menu");

            }
            private void PlayButtonClicked()
            {
                int diceRoll = _dice.RollDice();
                _playerVariables.DiceNumber = diceRoll;
                _playerVariables.CurrentScore += diceRoll;
                NetworkAPI.SendRequest(new SetUserVariablesRequest(_playerVariables.ToSFSUserVariable()), new List<NetworkEventSubscription>());
                _matchUI.UpdateUI(_playerVariables, _opponentVariables, false);
            }
            private void UserVariableUpdated(BaseEvent e)
            {
                var user = (User)e.Params["user"];
                if (user.Id == NetworkAPI.GetMyself().Id)
                    return;

                _opponentVariables.FromSFSUserVariable(user.GetVariables());
                _matchUI.UpdateUI(_playerVariables, _opponentVariables, true);

            }
        }

    }
}