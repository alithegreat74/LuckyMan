using System;
using Sfs2X.Core;
using System.Threading.Tasks;
using Sfs2X;
using Sfs2X.Requests;
using Model;

namespace Network
{
    public class SmartFoxTasks
    {
        private static SmartFox _smartFox;
        public SmartFoxTasks(SmartFox smartfox)
        {
            _smartFox = smartfox;
        }
        public async Task<bool> ConnectToServer
            (string serverIp, int serverPort, int timeout)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            //Listen for the events response
            EventListenerDelegate lambda = null;
            lambda = (evt) =>
            {
                _smartFox.RemoveEventListener(SFSEvent.CONNECTION, lambda);
                taskCompletionSource.TrySetResult(true);
            };
            _smartFox.AddEventListener(SFSEvent.CONNECTION, lambda);
            _smartFox.Connect(serverIp, serverPort);
            //Set a timeout for the CONNECTION
            var timeoutTask = Task.Delay(timeout).ContinueWith(t =>
                    {
                        _smartFox.RemoveEventListener(SFSEvent.CONNECTION, lambda);
                    });
            //Wait for one task either the timeout or the response be over
            return await Task.WhenAny(taskCompletionSource.Task, timeoutTask) == taskCompletionSource.Task ?
                await taskCompletionSource.Task :
                throw new TimeoutException("Connection Time out");
        }
        public async Task<NetworkResult> SendRequest
            (IRequest request, string sfsEvent, int timeout)
        {
            TaskCompletionSource<NetworkResult> taskCompletionSource = new TaskCompletionSource<NetworkResult>();
            EventListenerDelegate lambda = null;
            lambda = (evt) =>
            {
                taskCompletionSource.TrySetResult(new NetworkResult(evt));
                _smartFox.RemoveEventListener(sfsEvent, lambda);
            };
            _smartFox.AddEventListener(sfsEvent, lambda);
            _smartFox.Send(request);
            var timeoutTask = Task.Delay(timeout).ContinueWith(t =>
            {
                _smartFox.RemoveEventListener(sfsEvent, lambda);
            });
            return await Task.WhenAny(taskCompletionSource.Task, timeoutTask) == taskCompletionSource.Task ?
                await taskCompletionSource.Task :
                throw new TimeoutException("Request Time out");

        }
    }
}
