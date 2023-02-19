using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snitch.Core.Models
{
    public class Message
    {
        public string Sender { get; set; }
        public string MessageText { get; set; }   
        public bool IsMeSender { get; set; }
    }
}
