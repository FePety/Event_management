using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_management.Core.Models
{
    public class BaseMessage
    {
        public string Key { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }

        public BaseMessage(string key = default, string message = default, string description = default)
        {
            Key = key;
            Message = message;
            Description = description;
        }
    }
}
