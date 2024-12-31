using UnityEngine;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using System.Collections;
using System.Threading.Tasks;
using System;

namespace Network
{
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private Model.NetworkModelInfo _networkModel;
        private SmartFox _smartFox;
        public void SendRequest(IRequest request, string requestType, Action<BaseEvent> action)
        {
            if (_smartFox == null || !_smartFox.IsConnected)
                throw new System.Exception("Smartfox is not initialized");

            StartCoroutine(SendRequest_Cor(request, requestType, action));
        }
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
            StartCoroutine(EnterZone_Cor());
        }

        #region Connection And Entering Zone
        private IEnumerator EnterZone_Cor()
        {
            Task<BaseEvent> task = ConnectToTheServer();
            yield return new WaitUntil(()=>task.IsCompleted);
            if (!(bool)task.Result.Params["success"])
                yield break;

            Task<BaseEvent> loginTask = SendRequest(new LoginRequest("", "", _networkModel.ZoneName), SFSEvent.LOGIN);
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

        private IEnumerator SendRequest_Cor(IRequest request, string requestType, Action<BaseEvent> action)
        {
            Task<BaseEvent> task = SendRequest(request,requestType);
            yield return new WaitUntil(() => task.IsCompleted);
            action?.Invoke(task.Result);
        }

        private Task<BaseEvent> SendRequest(IRequest request, string requestType)
        {
            var taskCompletionSource = new TaskCompletionSource<BaseEvent>();

            _smartFox.AddEventListener(requestType, evt =>
            {
                taskCompletionSource.SetResult(evt);
                _smartFox.RemoveEventListener(requestType, null);

            });
            _smartFox.Send(request);
            return taskCompletionSource.Task;
        }

        #endregion

    }
}
