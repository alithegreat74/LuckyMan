using System.Threading.Tasks;
using Model;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(SmartFoxNetworkManager))]
    public class SmartFoxNetworkAPI : MonoBehaviour
    {
        private static SmartFoxNetworkManager _networkManager;

        private void Awake()
        {
            _networkManager = GetComponent<SmartFoxNetworkManager>();
        }

        public static async Task<BaseEvent> SendRequest(IRequest request, string sfsEvent)
        {
            return await _networkManager.SendRequest(request, sfsEvent);
        }

        public static void SubscribeToEvent(NetworkEventSubscription subscription) =>
            _networkManager.SubscribeToEvent(subscription);

        public static void UnSubscribeFromEvent(NetworkEventSubscription subscription) =>
            _networkManager.UnSubscribeToEvent(subscription);

        public static User GetMyself() => _networkManager.GetMyself();

        public static Room GetCurrentRoom() => _networkManager.GetCurrentRoom();

        public static string GetCurrentZone() => _networkManager.GetCurrentZone();
    }
}
