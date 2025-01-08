using System;
using System.Threading.Tasks;
using Model;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class SmartFoxNetworkManager : MonoBehaviour
    {
        [SerializeField] private NetworkModelInfo _networkModel;
        private SmartFox _smartFox;
        private SmartFoxTasks _smartFoxTasks;
        private void Awake()
        {
            _smartFox = new SmartFox();
            Application.runInBackground = true;
        }
        private void Update()
        {
            if (_smartFox != null)
                _smartFox.ProcessEvents();
        }
        private void OnApplicationQuit()
        {
            if (_smartFox == null || !_smartFox.IsConnected)
                return;
            UnSubscribeToEvent(new NetworkEventSubscription(SFSEvent.CONNECTION_LOST, ConnectionLost));
            _smartFox.Disconnect();
        }
        private async void Start()
        {
            SubscribeToEvent(new NetworkEventSubscription(SFSEvent.CONNECTION_LOST, ConnectionLost));
            //Initialize the smartfox tasks class by giving it a reference of the smartfox instance
            _smartFoxTasks = new SmartFoxTasks(_smartFox);
            await InitializeSession();
        }
        
        public async Task<NetworkResult> SendRequest(IRequest request, string sfsEvent)
        {
            
            return await _smartFoxTasks.SendRequest
                (request, sfsEvent, _networkModel.RequestTimeout);
        }
        private async Task InitializeSession()
        {
            try
            {
                await _smartFoxTasks.ConnectToServer(_networkModel.ServerIp, _networkModel.ServerPort, _networkModel.ConnectionTimeout);
                await SendRequest(new LoginRequest("", "", _networkModel.ZoneName), SFSEvent.LOGIN);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        #region Event Handling
        public void SubscribeToEvent(NetworkEventSubscription subscription) => _smartFox.AddEventListener(subscription.EventName, subscription.Action);
        public void UnSubscribeToEvent(NetworkEventSubscription subscription) => _smartFox.RemoveEventListener(subscription.EventName, subscription.Action);
        #endregion
        #region Get Session Info
        public User GetMyself() => _smartFox.MySelf;
        public Room GetCurrentRoom() => _smartFox.LastJoinedRoom;
        public string GetCurrentZone() => _smartFox.CurrentZone;
        #endregion
        #region Events
        private void ConnectionLost(BaseEvent e)
        {
            Debug.Log("Connection lost");
            _smartFox.RemoveAllEventListeners();
            SceneManager.LoadScene("Father");
        }
        #endregion
    }
}
