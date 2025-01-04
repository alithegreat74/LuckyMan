using UnityEngine;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using System.Collections;
using System.Threading.Tasks;
using Model;
using System.Collections.Generic;
using Sfs2X.Entities;

namespace Network
{
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private Model.NetworkModelInfo _networkModel;
        private SmartFox _smartFox;
        private List<NetworkEventSubscription> _permenantEventSubscribers = new();
        public void SendRequest(IRequest request, List<NetworkEventSubscription> subscriptions)
        {
            if (_smartFox == null)
                throw new System.Exception("Smartfox is not initialized");
            StartCoroutine(SendRequest_Cor(request, subscriptions));
        }
        public void SubscribeToEvent(NetworkEventSubscription subscription) => _smartFox.AddEventListener(subscription.EventName,subscription.Action);
        public void UnSubscribeToEvent(NetworkEventSubscription subscription) => _smartFox.RemoveEventListener(subscription.EventName,subscription.Action);
        public User GetMyself() => _smartFox.MySelf;
        public Room GetCurrentRoom() => _smartFox.LastJoinedRoom;
        private void Awake()
        {
            _smartFox = new SmartFox();
            Application.runInBackground = true;
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
        private void Start()
        {
            StartCoroutine(InitializeServer_Cor());
        }
        #region Connection And Entering Zone
        private IEnumerator InitializeServer_Cor()
        {
            Task<BaseEvent> task = ConnectToTheServer();
            yield return new WaitUntil(() => task.IsCompleted);
            if (!(bool)task.Result.Params["success"])
                yield break;

            Task<BaseEvent> loginTask = SendRequest_Task(new LoginRequest("", "", _networkModel.ZoneName), new List<NetworkEventSubscription>());
            yield return new WaitUntil(() => loginTask.IsCompleted);

        }
        private Task<BaseEvent> ConnectToTheServer()
        {
            var taskCompletionSource = new TaskCompletionSource<BaseEvent>();

            _smartFox.AddEventListener(SFSEvent.CONNECTION, evt =>
            {
                taskCompletionSource.SetResult(evt);
                _smartFox.RemoveEventListener(SFSEvent.CONNECTION, null);

            });
            _smartFox.Connect(_networkModel.ServerIp, _networkModel.ServerPort);
            return taskCompletionSource.Task;
        }
        #endregion
        #region RequestSending
        private IEnumerator SendRequest_Cor(IRequest request, List<NetworkEventSubscription> subscriptions)
        {
            if(!_smartFox.IsConnected)
            {
                Task<BaseEvent> connection = ConnectToTheServer();
                yield return new WaitUntil(() => connection.IsCompleted);
            }

            Task<BaseEvent> task = SendRequest_Task(request,subscriptions);
            yield return new WaitUntil(() => task.IsCompleted);

            EventListenerCleanup(subscriptions);
        }
        private Task<BaseEvent> SendRequest_Task(IRequest request, List<NetworkEventSubscription> subscriptions)
        {
            var taskCompletionSource = new TaskCompletionSource<BaseEvent>();
            foreach (var subscription in subscriptions)
            {
                _smartFox.AddEventListener(subscription.EventName, e =>
                {
                    subscription.Action?.Invoke(e);
                });
            }
            _smartFox.Send(request);
            return taskCompletionSource.Task;
        }
        //In order to make the unecessary events not trigger we un register them from sfs
        private void EventListenerCleanup(List<NetworkEventSubscription> subscriptions)
        {
            foreach (var subscription in subscriptions)
            {
                _smartFox.RemoveEventListener(subscription.EventName, subscription.Action);
            }
        }
        #endregion
    }
}
