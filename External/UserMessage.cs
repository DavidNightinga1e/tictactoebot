using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External
{
    struct UserMessage
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string Payload { get; set; }

        public UserMessage(long id, string text, string payload)
        {
            Id = id;
            Text = text;
            Payload = payload;
        }
    }
}
