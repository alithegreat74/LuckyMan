using UnityEngine;
using Sfs2X;
using Sfs2X.Core;
using System.Collections.Generic;

namespace Network
{
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private Model.NetworkModelInfo _networkModel;
        public static NetworkManager Instance;
        private SmartFox _smartFox;
        private Dictionary<string, List<INetworkListener>> _eventListeners = new();
        

        private void Awake()
        {
            //TODO: Think of a better way to handle network manager than a singleton
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

            _smartFox = new SmartFox();
            Application.runInBackground = true;
        }

        private void Update()
        {
            if(_smartFox!=null)
                _smartFox.ProcessEvents();
        }

        public void AddNetworkEventListener(string sfsEvent,INetworkListener listener)
        {
            //TODO: Check If thread safety will hurt the proccess
            if(_eventListeners.TryGetValue(sfsEvent,out List<INetworkListener> listeners))
            {
                var newlisteners = new List<INetworkListener>() { listener };
                _eventListeners.Add(sfsEvent, newlisteners);
                _smartFox.AddEventListener(sfsEvent, HandleEvents);
            }

            if (listeners.Contains(listener))
                return;

            listeners.Add(listener);

        }

        public void RemoveNetworkEventListener(string sfsEvent,INetworkListener listener)
        {
            if (_eventListeners.TryGetValue(sfsEvent, out List<INetworkListener> listeners))
            {
                listeners.Remove(listener);
            }
            else
            {
                var newlisteners = new List<INetworkListener>();
                newlisteners.Add(listener);
                _eventListeners.Add(sfsEvent, newlisteners);
            }
        }
        //TODO: Think about whether or not the Core Scripts should be able to access the network manger or not
        public void Connect()
        {
            if (_smartFox == null || _smartFox.IsConnected)
                return;
            _smartFox.Connect(_networkModel.ServerIp, _networkModel.ServerPort);
        }
        private void OnApplicationQuit()
        {
            if (_smartFox == null || !_smartFox.IsConnected)
                return;

            _smartFox.Disconnect();
        }

        private void HandleEvents(BaseEvent e)
        {
            if (!_eventListeners.ContainsKey(e.Type))
                return;

            foreach(var listener in _eventListeners[e.Type])
            {
                listener.Event(e);
            }
        }
    }
}
