using Network;
using UnityEngine;
using Sfs2X.Core;
namespace Core
{
    public class ConnectTest : MonoBehaviour, INetworkListener
    {
        //
        public void Event(BaseEvent e)
        {
        }

        private void Start()
        {
            NetworkManager.Instance.AddNetworkEventListener(Sfs2X.Core.SFSEvent.CONNECTION, this);
            NetworkManager.Instance.Connect();
        }


    }
}
