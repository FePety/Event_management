using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_management.Core.Models
{
    public class BaseResponse
    {
        public List<BaseMessage> Info { get; set; }
        public List<BaseMessage> Warning { get; set; }
        public List<BaseMessage> Error { get; set; }

        public BaseResponse(List<BaseMessage> info = null, List<BaseMessage> warning = null, List<BaseMessage> error = null)
        {
            Info = info ?? new List<BaseMessage>();
            Warning = warning ?? new List<BaseMessage>();
            Error = error ?? new List<BaseMessage>();
        }
    }
}
