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

        public async Task<BaseEvent> SendRequest(IRequest request, string sfsEvent)
        {
            var taskCompletionSource = new TaskCompletionSource<BaseEvent>();
            if (_smartFox == null)
            {
                taskCompletionSource.SetException(new Exception("Smartfox is not initialized"));
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
            var timeoutTask = Task.Delay(_networkModel.Timeout)
                .ContinueWith(t =>
                {
                    taskCompletionSource.TrySetException(new Exception("Request Timed Out"));
                    _smartFox.RemoveEventListener(sfsEvent, lambda);
                });

            return
                await Task.WhenAny(timeoutTask, taskCompletionSource.Task)
                == taskCompletionSource.Task
                ? await taskCompletionSource.Task
                : throw new TimeoutException("Request Timed Out");
        }

        public void SubscribeToEvent(NetworkEventSubscription subscription) =>
            _smartFox.AddEventListener(subscription.EventName, subscription.Action);

        public void UnSubscribeToEvent(NetworkEventSubscription subscription) =>
            _smartFox.RemoveEventListener(subscription.EventName, subscription.Action);

        public User GetMyself() => _smartFox.MySelf;

        public Room GetCurrentRoom() => _smartFox.LastJoinedRoom;

        public string GetCurrentZone() => _smartFox.CurrentZone;

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
            _smartFox.Disconnect();
        }

        private void Start()
        {
            _smartFox.AddEventListener(SFSEvent.CONNECTION, OnConnect);
            _smartFox.Connect(_networkModel.ServerIp, _networkModel.ServerPort);
        }

        // TODO: Handle zone login in a seperate async sequence
        private void OnConnect(BaseEvent e)
        {
            _smartFox.Send(new LoginRequest("", "", _networkModel.ZoneName));
        }
    }
}
