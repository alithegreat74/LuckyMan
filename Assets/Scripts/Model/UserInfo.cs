using Sfs2X.Entities.Data;

namespace Model
{
    public struct UserInfo
    {
        public string Username;
        public string Password;
        public SFSObject ToSFSO()
        {
            var sfso = new SFSObject();

            sfso.PutUtfString("username", Username);
            sfso.PutUtfString("password", Password);
            return sfso;
        }
    }

}