using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using System;

namespace Model
{
    public class NetworkResult
    {
        private BaseEvent _sfsEvent;
        private SFSObject _resultParams;
        public NetworkResult(BaseEvent sfsEvent)
        {
            this._sfsEvent = sfsEvent;
            try
            {
                _resultParams = (SFSObject)_sfsEvent.Params["params"];
            }
            catch
            {
                _resultParams = null;
            }
        }
        public bool Success()
        {
            try
            {
                return _resultParams.GetBool("success");
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException || ex is NullReferenceException)
                    return (bool)_sfsEvent.Params["success"];

                return false;
            }
        }
        public string ErrorMessage()
        {
            try
            {
                return _resultParams.GetUtfString("errorMessage");
            }
            catch (Exception ex)
            {   if(ex is ArgumentException || ex is NullReferenceException)
                    return (string)_sfsEvent.Params["errorMessage"];
             
                return "";
            }
        }
        public int GetIntResult(string name) => _resultParams.GetInt(name);
        public string GetStringResult(string name) => _resultParams.GetUtfString(name);
        public ISFSObject GetObjectResult(string name) => _resultParams.GetSFSObject(name);
        public ISFSArray GetArrayResult(string name) => _resultParams.GetSFSArray(name);
    }
}