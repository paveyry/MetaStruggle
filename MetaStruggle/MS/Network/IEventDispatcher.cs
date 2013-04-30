using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public interface IEventDispatcher
    {
        void ThrowNewEvent(string eventID, object data);
    }
}
