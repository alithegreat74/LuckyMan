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
            SmartFoxTasks.Init(_smartFox);
            await InitializeSession();
        }
        public async Task<BaseEvent> SendRequest(IRequest request, string sfsEvent)
        {
            return await SmartFoxTasks.SendRequest
                (request, sfsEvent, _networkModel.RequestTimeout);
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
        private async Task InitializeSession()
        {
            try
            {
                await SmartFoxTasks.ConnectToServer(_networkModel.ServerIp, _networkModel.ServerPort, _networkModel.ConnectionTimeout);
                await SendRequest(new LoginRequest("", "", _networkModel.ZoneName), SFSEvent.LOGIN);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        private void ConnectionLost(BaseEvent e)
        {
            Debug.Log("Connection lost");
            _smartFox.RemoveAllEventListeners();
            SceneManager.LoadScene("Father");
        }
    }
}
