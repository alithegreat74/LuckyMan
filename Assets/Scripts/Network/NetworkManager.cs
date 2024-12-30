using UnityEngine;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using System.Collections.Generic;

namespace Network
{
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private Model.NetworkModelInfo _networkModel;
        private SmartFox _smartFox;
        private Dictionary<string, List<INetworkListener>> _eventListeners = new();
        private Queue<IRequest> _waitQueue = new();


        public void AddNetworkEventListener(string sfsEvent, INetworkListener listener)
        {
            //TODO: Check If thread safety will hurt the proccess
            if (!_eventListeners.TryGetValue(sfsEvent, out List<INetworkListener> listeners))
            {
                var newlisteners = new List<INetworkListener>() { listener };
                _eventListeners.Add(sfsEvent, newlisteners);
                _smartFox.AddEventListener(sfsEvent, HandleEvents);
                return;
            }

            if (listeners.Contains(listener))
                return;

            listeners.Add(listener);

        }

        public void RemoveNetworkEventListener(string sfsEvent, INetworkListener listener)
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
        public void Connect()
        {
            if (_smartFox == null || _smartFox.IsConnected)
                return;
            _smartFox.Connect(_networkModel.ServerIp, _networkModel.ServerPort);
        }

        //Check for the connection upon sending requests and try to connect if you are not connected
        //Store the sent request in a queue to wait for the connection to happen
        public void SendRequest(IRequest request)
        {
            if (_smartFox == null)
                throw new System.Exception("Smartfox is not initialized");

            if (!_smartFox.IsConnected)
            {
                Connect();
                _waitQueue.Enqueue(request);
                return;
            }

            _smartFox.Send(request);
        }


        private void Awake()
        {
            _smartFox = new SmartFox();
            Application.runInBackground = true;
        }

        private void Start()
        {
            _smartFox.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        }

        private void Update()
        {
            if(_smartFox!=null)
                _smartFox.ProcessEvents();
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

        private void OnConnection(BaseEvent e)
        {
            //Try to send the requests again
            while(_waitQueue.Count>0)
            {
                SendRequest(_waitQueue.Dequeue());
            }
        }
    }
}
