using System;
using System.Threading.Tasks;
using Model;
using Network;
using Sfs2X.Core;
using Sfs2X.Requests;
using UnityEngine;

namespace Application
{
    public class LoginManager : MonoBehaviour
    {
        public async Task LoginSequence(UserInfo info)
        {
            try
            {
                //Since there wont be a login result (there's a seperate response for it) we just wait for it to get timed out
                NetworkResult logoutResult = await SmartFoxNetworkAPI.SendRequest(new LogoutRequest(), SFSEvent.LOGOUT);
                NetworkResult loginResult = await SmartFoxNetworkAPI.SendRequest(new LoginRequest(info.Username, info.Password, "BasicExamples"), SFSEvent.LOGIN);
                SceneLoader.LoadScene("Main Menu");
            }
            catch (Exception e)
            {
                Debug.Log("Unable to complete the login LoginSequence");
                Debug.Log(e);
                await SmartFoxNetworkAPI.SendRequest(new LoginRequest("", "", "SignUp"), SFSEvent.LOGIN);
            }
        }
    }
}
