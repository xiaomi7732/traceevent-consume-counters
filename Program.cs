using System;
using System.Collections.Generic;
using Microsoft.Diagnostics.Tracing;

namespace CounterConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            using EventPipeEventSource source = new EventPipeEventSource("./counters.nettrace");
            source.Dynamic.All += (TraceEvent obj) =>
            {
                if (obj.EventName.Equals("EventCounters"))
                {
                    System.Console.WriteLine("Got something:");
                    var payloadVal = (IDictionary<string, object>)(obj.PayloadValue(0));
                    IDictionary<string, object> payloadFields = (IDictionary<string, object>)(payloadVal["Payload"]);

                    if (payloadFields != null)
                    {
                        foreach (var item in payloadFields)
                        {
                            System.Console.WriteLine($"{item.Key}: {item.Value}");
                        }
                    }
                    System.Console.WriteLine(obj.ToString());
                }
            };
            source.Process();
        }
    }
}
