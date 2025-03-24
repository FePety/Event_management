using System.Collections.Generic;

namespace Event_management.Core.Models
{
    public class BaseResponse
    {
        public List<BaseMessage> Info { get; set; } = new List<BaseMessage>();
        public List<BaseMessage> Warning { get; set; } = new List<BaseMessage>();
        public List<BaseMessage> Error { get; set; } = new List<BaseMessage>();

        public void AddError(string key, string msg, string desc)
        {
            Error.Add(new BaseMessage(key, msg, desc));
        }

        public void AddInfo(string key, string msg, string desc)
        {
            Info.Add(new BaseMessage(key, msg, desc));
        }
        /* 
         public List<BaseMessage> Info { get; set; }
         public List<BaseMessage> Warning { get; set; }
         public List<BaseMessage> Error { get; set; }

         public BaseResponse(List<BaseMessage> info = null, List<BaseMessage> warning = null, List<BaseMessage> error = null)
         {
             Info = info ?? new List<BaseMessage>();
             Warning = warning ?? new List<BaseMessage>();
             Error = error ?? new List<BaseMessage>();
         }

         public void AddError(BaseResponse response, string key, string msg, string desc)
         {
             response.Error.Add(new BaseMessage(key, msg, desc));
         }

         public void AddInfo(BaseResponse response, string key, string msg, string desc)
         {
             response.Info.Add(new BaseMessage(key, msg, desc));
         }
        */
    }
}
