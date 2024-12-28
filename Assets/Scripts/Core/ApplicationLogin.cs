using UnityEngine;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X;
using Sfs2X.Entities;

namespace Core
{
    public class ApplicationLogin : MonoBehaviour
    {
        //TODO: Handle zone info in a scriptable object
        private static readonly string s_ZoneName = "LuckyMan";


        public static void TryLogin(string userName)
        {
            if (!Connector.IsConnected)
                throw new System.Exception("The program is not connected to the server");

            Connector.SmartFox.AddEventListener(SFSEvent.LOGIN,OnLogin);
            Connector.SmartFox.Send(new LoginRequest(userName, "", s_ZoneName));
        }



        private static void OnLogin(BaseEvent e)
        {
    
            Debug.Log($"User: {(User)e.Params["user"]} Just Logged in");
        }
    }

}