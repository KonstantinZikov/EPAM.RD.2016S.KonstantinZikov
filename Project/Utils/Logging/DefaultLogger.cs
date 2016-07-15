using System;
using System.Diagnostics;

namespace Utils
{
    public class DefaultLogger : ILogger
    {
        private readonly TraceSource source;
        private int nextId;

        public DefaultLogger()
        {
            try
            {
                source = new TraceSource("Log");
            }
            catch(ArgumentException ex)
            {
                throw new LoggerException("Can't find trace source with name \"Log\"." +
                    "Check App.config file.", ex);
            }
        }

        public void Log(TraceEventType eventType, string message)
        {           
            source.TraceData(eventType, nextId, message);
            nextId++;
            source.Flush();
            source.Close();

        }
    }
}
