using Model;
using Network;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Application
{
    public class SignUpManager : MonoBehaviour
    {
        public void SendRequest(UserInfo info)
        {
            NetworkAPI.SendRequest(new ExtensionRequest("$SignUp.Submit", info.ToSFSO()), new List<NetworkEventSubscription>(){ new NetworkEventSubscription(SFSEvent.EXTENSION_RESPONSE,SignUpEvent)});
            
        }
        private void SignUpEvent(BaseEvent e)
        {
            SFSObject param = (SFSObject)e.Params["params"];
            if (param.GetBool("success"))
            {
                SceneManager.LoadScene("Login");
                return;
            }

            Debug.Log(param.GetUtfString("errorMessage"));
        }

    }

}