using System;
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
        public async Task<Tuple<bool,string>> SendRequest(UserInfo info)
        {
            NetworkResult result = await SmartFoxNetworkAPI.SendRequest(
                    new ExtensionRequest("signUp", info.ToSFSO()), SFSEvent.EXTENSION_RESPONSE);
            if (result.Success())
                return new Tuple<bool, string>(true,"");

            return new Tuple<bool, string>(false,result.ErrorMessage());
        }
    }
}
