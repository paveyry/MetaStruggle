using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClient.Global.EventManager
{
    public class EventManager
    {
        public delegate void EventHandler(EventDatas eventDatas);
        private readonly Dictionary<string, EventHandler> _eventHandlers;

        public EventManager()
        {
            _eventHandlers = new Dictionary<string, EventHandler>();
        }

        public void ThrowEvent(EventDatas eventDatas)
        {
            lock (_eventHandlers)
            {
                foreach (var eventHandler in _eventHandlers.Where(current => current.Key == eventDatas.ID || current.Key == "*"))
                    eventHandler.Value.BeginInvoke(eventDatas, null, null);
            }
        }

        public void AddListener(string id, EventHandler eventHandler)
        {
            _eventHandlers.Add(id, eventHandler);
        }
    }
}
