using Sfs2X.Requests;
using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(NetworkManager))]
    public class NetworkCommand : MonoBehaviour
    {
        private static NetworkManager _networkManager;

        private void Awake()
        {
            _networkManager = GetComponent<NetworkManager>();
        }

        public static void Connect()
        {
            _networkManager.Connect();
        }

        public static void SendRequest(IRequest request)
        {
            _networkManager.SendRequest(request);
        }

        ///TODO: Create a wrapper for the sfs objects that would be used by the application scripts
        public static void AddNetworkEventListener(string sfsEvent, INetworkListener listener)
        {
            _networkManager.AddNetworkEventListener(sfsEvent, listener);
        }

        //TODO: Create a wrapper for the sfs objects that would be used by the application scripts
        //Theres no need for the application scripts to know sfs exists
        public static void RemoveNetworkEventListener(string sfsEvent, INetworkListener listener)
        {
            _networkManager.RemoveNetworkEventListener(sfsEvent, listener);
        }
    }
}