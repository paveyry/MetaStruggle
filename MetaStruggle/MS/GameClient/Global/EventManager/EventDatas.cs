using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClient.Global.EventManager
{
    public class EventDatas
    {
        public string Sender { get; set; }
        public string ID { get; set; }
        public object Datas { get; set; }

        public EventDatas(string sender, string id, object datas)
        {
            Sender = sender;
            ID = id;
            Datas = datas;
        }
    }
}
