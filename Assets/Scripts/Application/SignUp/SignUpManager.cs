using System.Threading.Tasks;
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
        public async Task SendRequest(UserInfo info)
        {
            BaseEvent result = await SmartFoxNetworkAPI.SendRequest(
                    new ExtensionRequest("signUp", info.ToSFSO()), SFSEvent.EXTENSION_RESPONSE);
            SFSObject param = (SFSObject)result.Params["params"];
            if (param.GetBool("success"))
            {
                SceneLoader.LoadScene("Login");
                return;
            }

            Debug.Log(param.GetUtfString("errorMessage"));
        }
    }
}
