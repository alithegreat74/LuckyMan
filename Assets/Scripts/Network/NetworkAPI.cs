using Sfs2X.Core;
using Sfs2X.Requests;
using System;
using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(NetworkManager))]
    public class NetworkAPI : MonoBehaviour
    {
        private static NetworkManager _networkManager;

        private void Awake()
        {
            _networkManager = GetComponent<NetworkManager>();
        }
        public static void SendRequest(IRequest request, string requestType, Action<BaseEvent> action)
        {
            _networkManager.SendRequest(request, requestType, action);
        }
    }
}