using System;
using Sfs2X.Core;

namespace Model
{
    public struct NetworkEventSubscription
    {
        public string EventName;
        public EventListenerDelegate Action;

        public NetworkEventSubscription(string eventName, EventListenerDelegate action)
        {
            EventName = eventName;
            Action = action;
        }
    }
}
