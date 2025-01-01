using Model;
using Sfs2X.Core;
using Sfs2X.Requests;
using System;
using System.Collections.Generic;
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
        public static void SendRequest(IRequest request, List<NetworkEventSubscription> subscriptions)
        {
            _networkManager.SendRequest(request, subscriptions);
        }
        public static bool IsLoggedIn() => _networkManager.IsLoggedIn();
        public static bool IsConnected() => _networkManager.IsConnected();
    }
}