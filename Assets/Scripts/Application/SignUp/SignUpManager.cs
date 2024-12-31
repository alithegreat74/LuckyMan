using Model;
using Network;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;

namespace Application
{
    public class SignUpManager : MonoBehaviour
    {
        public void SendRequest(UserInfo info)
        {
            NetworkAPI.SendRequest(new ExtensionRequest("$SignUp.Submit", info.ToSFSO()), SFSEvent.EXTENSION_RESPONSE, Event);
        }
        //TODO: Create a function that handles the incoming event
        private void Event(BaseEvent e)
        {
            SFSObject param = (SFSObject)e.Params["params"];
            if (param.GetBool("success"))
            {
                Debug.Log("Sign up Successful");
                return;
            }

            Debug.Log(param.GetUtfString("errorMessage"));
        }

    }

}