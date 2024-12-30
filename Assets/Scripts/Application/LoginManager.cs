using Model;
using Network;
using Sfs2X.Core;
using Sfs2X.Requests;
using UnityEngine;

namespace Application
{
    public class LoginManager : MonoBehaviour, INetworkListener
    {
        private readonly string _zoneName = "BasicExamples";
        public void Event(BaseEvent e)
        {
            switch(e.Type)
            {
            case "login":
                LoginSuccess(e);
                break;
            case "loginError":
                LoginFailed(e);
                break;
            }
        }
        public void SendRequest(LoginInfo info)
        {
            NetworkCommand.SendRequest(new LoginRequest(info.Username, info.Password, _zoneName));
        }

        private void Start()
        {
            NetworkCommand.AddNetworkEventListener(SFSEvent.LOGIN, this);
            NetworkCommand.AddNetworkEventListener(SFSEvent.LOGIN_ERROR, this);
        }

        private void LoginSuccess(BaseEvent e)
        {
            Debug.Log("Login Was Successfull");
        }
        private void LoginFailed(BaseEvent e)
        {
            Debug.Log("Login Failed");
            Debug.Log(e.Params["errorMessage"]);
        }

    }

}