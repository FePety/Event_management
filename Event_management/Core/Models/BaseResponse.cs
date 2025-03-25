using System.Collections.Generic;

namespace Event_management.Core.Models
{
    // It allows adding messages to the response to give the client more context.
    public class BaseResponse
    {
        public List<BaseMessage> Info { get; set; } = new List<BaseMessage>();
        public List<BaseMessage> Warning { get; set; } = new List<BaseMessage>();
        public List<BaseMessage> Error { get; set; } = new List<BaseMessage>();

        // Adds an error message to the Error list.
        // Parameters:
        // - key: A unique identifier for the error.
        // - msg: The error message.
        // - desc: A detailed description of the error.
        public void AddError(string key, string msg, string desc)
        {
            Error.Add(new BaseMessage(key, msg, desc));
        }

        // Adds an informational message to the Info list.
        // Parameters:
        // - key: A unique identifier for the information.
        // - msg: The information message.
        // - desc: A detailed description of the information.
        public void AddInfo(string key, string msg, string desc)
        {
            Info.Add(new BaseMessage(key, msg, desc));
        }
    }
}
