using Sfs2X.Core;
using System;

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