using System;
using System.Threading.Tasks;
using Model;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using UnityEngine;

namespace Network
{
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField]
        private Model.NetworkModelInfo _networkModel;
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
            UnSubscribeToEvent(
                new NetworkEventSubscription(SFSEvent.CONNECTION_LOST, ConnectionLost));
            _smartFox.Disconnect();
        }

        private void Start()
        {
            SubscribeToEvent(
                new NetworkEventSubscription(SFSEvent.CONNECTION_LOST, ConnectionLost));
            InitializeSession();
        }

        public async Task<BaseEvent> SendRequest(IRequest request, string sfsEvent)
        {
            var taskCompletionSource = new TaskCompletionSource<BaseEvent>();
            if (_smartFox == null)
            {
                taskCompletionSource.SetException(
                    new Exception("Smartfox is not initialized"));
                return await taskCompletionSource.Task;
            }
            // creating a lambda that sets the result
            // we need to Unsubscribe from it in order to not raise the thing
            EventListenerDelegate lambda = null;
            lambda = (BaseEvent e) =>
            {
                taskCompletionSource.SetResult(e);
                _smartFox.RemoveEventListener(sfsEvent, lambda);
            };
            _smartFox.AddEventListener(sfsEvent, lambda);
            _smartFox.Send(request);
            // What happens when your request times out
            var timeoutTask = Task.Delay(_networkModel.Timeout).ContinueWith(t =>
            {
                taskCompletionSource.TrySetException(new Exception("Request Timed Out"));
                _smartFox.RemoveEventListener(sfsEvent, lambda);
            });

            return await Task.WhenAny(timeoutTask, taskCompletionSource.Task) ==
                           taskCompletionSource.Task
                       ? await taskCompletionSource.Task
                       : throw new TimeoutException("Request Timed Out");
        }

        public void SubscribeToEvent(NetworkEventSubscription subscription) =>
            _smartFox.AddEventListener(subscription.EventName, subscription.Action);

        public void UnSubscribeToEvent(NetworkEventSubscription subscription) =>
            _smartFox.RemoveEventListener(subscription.EventName,
                                          subscription.Action);

        public User GetMyself() => _smartFox.MySelf;

        public Room GetCurrentRoom() => _smartFox.LastJoinedRoom;

        public string GetCurrentZone() => _smartFox.CurrentZone;

        private void ConnectionLost(BaseEvent e) => InitializeSession();

        private async void InitializeSession()
        {
            await ConnectToServer();
            try
            {
                await SendRequest(new LoginRequest("", "", _networkModel.ZoneName),
                                  SFSEvent.LOGIN);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private async Task<bool> ConnectToServer()
        {
            TaskCompletionSource<bool> taskCompletionSource =
                new TaskCompletionSource<bool>();
            EventListenerDelegate lambda = null;
            lambda = evt =>
            {
                taskCompletionSource.SetResult(true);
                _smartFox.RemoveEventListener(SFSEvent.CONNECTION, lambda);
            };
            _smartFox.AddEventListener(SFSEvent.CONNECTION, lambda);
            _smartFox.Connect();
            var timeoutTask = Task.Delay(_networkModel.Timeout).ContinueWith(t =>
            {
                taskCompletionSource.SetException(
                    new TimeoutException("Request Timed out"));
                _smartFox.RemoveEventListener(SFSEvent.CONNECTION, lambda);
            });

            return await Task.WhenAny(timeoutTask, taskCompletionSource.Task) ==
                           taskCompletionSource.Task
                       ? await taskCompletionSource.Task
                       : throw new TimeoutException("Request Timed out");
        }
    }
}
