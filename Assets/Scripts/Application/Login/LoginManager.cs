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
                BaseEvent logoutResult = await NetworkAPI.SendRequest(
                    new LogoutRequest(),
                    SFSEvent.LOGOUT
                );

                BaseEvent loginResult = await NetworkAPI.SendRequest(
                    new LoginRequest(info.Username, info.Password, "BasicExamples"),
                    SFSEvent.LOGIN
                );
                SceneLoader.LoadScene("Main Menu");
            }
            catch (Exception e)
            {
                Debug.Log("Unable to complete the login LoginSequence");
                Debug.Log(e);
                await NetworkAPI.SendRequest(new LoginRequest("", "", "SignUp"), SFSEvent.LOGIN);
            }
        }
    }
}
