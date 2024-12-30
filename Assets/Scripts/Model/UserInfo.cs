using Sfs2X.Entities.Data;

namespace Model
{
    public struct UserInfo
    {
        public string Username;
        public string Password;

        public SFSObject ToSFS()
        {
            var sfso = new SFSObject();

            sfso.PutUtfString("password", Username);
            sfso.PutUtfString("password", Password);
            return sfso;
        }
    }

}