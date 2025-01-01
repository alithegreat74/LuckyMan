using Sfs2X.Entities.Data;
using Sfs2X.Util;
namespace Model
{
    public struct UserInfo
    {
        public string Username;
        public string Password {
            get 
            { 
                return _password; 
            }
            set 
            {
                _password = value;
            }
        }

        private string _password;
        public SFSObject ToSFSO()
        {
            var sfso = new SFSObject();
            UnityEngine.Debug.Log(Password);
            sfso.PutUtfString("username", Username);
            sfso.PutUtfString("password", Password);
            return sfso;
        }
    }
}