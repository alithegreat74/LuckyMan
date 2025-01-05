using Model;
using Network;
using Sfs2X.Core;
using Sfs2X.Requests;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Application
{
    public class LoginManager : MonoBehaviour
    {
        private UserInfo _userInfo;
        private List<NetworkEventSubscription> _subcriptions;
        private void Start()
        {
            _subcriptions = new List<NetworkEventSubscription>
            {
                new NetworkEventSubscription(SFSEvent.LOGIN,LoginSuccess),
                new NetworkEventSubscription(SFSEvent.LOGIN_ERROR,LoginError)
            };
        }
        public void SendLoginRequest(UserInfo info)
        {
            _userInfo = info;
            //Logout first from signup
            NetworkAPI.SendRequest(new LogoutRequest(), new List<NetworkEventSubscription> { new NetworkEventSubscription(SFSEvent.LOGOUT, LogoutEvent) });
        }
        private void LogoutEvent(BaseEvent e)
        {
            NetworkAPI.SendRequest(new LoginRequest(_userInfo.Username, _userInfo.Password, "BasicExamples"), _subcriptions);
        }
        private void LoginSuccess(BaseEvent e)
        {
            if (NetworkAPI.GetCurrentZone() != "BasicExamples")
                return;
            Debug.Log("Logged in successfully");
            SceneLoader.LoadScene("Main Menu");
        }

        private void LoginError(BaseEvent e)
        {
            //Re-Enter guest zone
            Debug.Log((string)e.Params["errorMessage"]);
            NetworkAPI.SendRequest(new LoginRequest("", "", "SignUp"), new());
        }
    }
}