using Prism.Events;
using Prism.Logging;

namespace MV.Core.Events
{
    public class UserMessage
    {
        public string Source { get; set; }
        public Category Level { get; set; }
        public string Content { get; set; }
    }


    public class UserMessageEvent : PubSubEvent<UserMessage>
    {

    }
}
