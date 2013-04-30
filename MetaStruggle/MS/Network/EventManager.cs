using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class EventManager : IEventDispatcher
    {
        public delegate void EventHandler(object data);

        readonly Dictionary<string, List<EventHandler>> _registered = new Dictionary<string, List<EventHandler>>();

        public void ThrowNewEvent(string eventID, object data)
        {
            if (_registered.ContainsKey(eventID))
                foreach (var handler in _registered[eventID])
                    handler.BeginInvoke(data, null, null);
        }

        public void Register(string eventID, EventHandler handler)
        {
            if (_registered.ContainsKey(eventID))
            {
                if (_registered[eventID].All(h => !handler.Equals(h)))
                    _registered[eventID].Add(handler);
            }
            else
                _registered.Add(eventID, new List<EventHandler> {handler});
        }

        public void Unregister(string eventID, EventHandler handler)
        {
            if (_registered.ContainsKey(eventID))
                _registered[eventID].RemoveAll(handler.Equals);
        }
    }
}
