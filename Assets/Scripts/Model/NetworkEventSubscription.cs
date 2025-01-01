using Sfs2X.Core;
using System;

namespace Model
{
    public struct NetworkEventSubscription
    {
        public string EventName;
        public Action<BaseEvent> Action;
        public NetworkEventSubscription(string eventName, Action<BaseEvent> action)
        {
            EventName = eventName;
            Action = action;
        }
    }

}