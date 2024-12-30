using Model;
using Network;
using Sfs2X.Core;
using Sfs2X.Requests;
using UnityEngine;

namespace Application
{
    public class SignUpManager : MonoBehaviour, INetworkListener
    {
        private readonly string _extension = "$SignUp.Submit";
        public void Event(BaseEvent e)
        {
            Debug.Log($"Signup was {(bool)e.Params["success"]}");
        }

        private void Start()
        {
            NetworkCommand.AddNetworkEventListener(SFSEvent.EXTENSION_RESPONSE, this);
        }

        public void SendRequest(UserInfo info)
        {
            NetworkCommand.SendRequest(new ExtensionRequest(_extension, info.ToSFS()));
        }

    }

}