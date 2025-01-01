using Sfs2X.Entities.Data;
using UnityEngine;

namespace Model
{
    public struct PublicUserInfo
    {
        public string Username;
        public int Score;
        public PublicUserInfo(string username, int score)
        {
            Username = username;
            Score = score;
        }
        public SFSObject ToSFSO()
        {
            var sfso = new SFSObject();
            sfso.PutUtfString("username", Username);
            sfso.PutInt("score", Score);
            return sfso;
        }
        public void FromSFSO(SFSObject sfso)
        {
            Username = sfso.GetUtfString("username");
            Score = sfso.GetInt("score");
        }
    }

}