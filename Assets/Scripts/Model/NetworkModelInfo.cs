using UnityEngine;

namespace Model
{
    [System.Serializable]
    [CreateAssetMenu(fileName ="New Network Model Info", menuName = "Models/Network/Network Model Info")]
    public class NetworkModelInfo : ScriptableObject
    {
        public string ServerIp = "127.0.0.1";
        public int ServerPort = 9933;
        public string ZoneName = "SignUp";
    }

}