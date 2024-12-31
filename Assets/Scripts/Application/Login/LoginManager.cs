using Model;
using UnityEngine;

namespace Application
{
    public class LoginManager : MonoBehaviour
    {
        private readonly string _zoneName = "BasicExamples";
        
        public void SendRequest(UserInfo info)
        {
            //NetworkAPI.SendRequest(new LoginRequest(info.Username, info.Password, _zoneName));
        }
    }

}