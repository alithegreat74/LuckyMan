using UnityEngine;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;

namespace Core
{
    public class Connector : MonoBehaviour
    {
        //TODO: Manage server info like ip and port in a scriptable object
        private readonly string _serverIP = "127.0.0.1";
        private readonly int _serverPort = 9933;

        public static SmartFox SmartFox;
        public static bool IsConnected;
        private void Start()
        {
            SmartFox = new SmartFox();
            SmartFox.ThreadSafeMode = true;

            SmartFox.AddEventListener(SFSEvent.CONNECTION, OnConnection);

            SmartFox.Connect(_serverIP, _serverPort);
        }

        private void Update()
        {
            SmartFox.ProcessEvents();
        }

        private void OnConnection(BaseEvent e)
        {
            if (!(bool)e.Params["success"])
                return;

            Debug.Log("Connected");
            IsConnected = true;
        }
    }

}