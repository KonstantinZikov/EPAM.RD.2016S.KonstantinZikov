using System;
using System.Diagnostics;

namespace Utils
{
    /// <summary>
    /// Simple logger. Search trace source with name "log" in app.config and use it as an output.
    /// </summary>
    public class DefaultLogger : MarshalByRefObject, ILogger
    {
        private readonly TraceSource source;
        private int nextId;

        public DefaultLogger()
        {
            try
            {
                this.source = new TraceSource("Log");
            }
            catch (ArgumentException ex)
            {
                throw new LoggerException(
                    "Can't find trace source with name \"Log\"." +
                    "Check App.config file.", 
                    ex);
            }
        }

        public void Log(TraceEventType eventType, string message)
        {
            this.source.TraceData(eventType, this.nextId, message);
            this.nextId++;
            this.source.Flush();
            this.source.Close();
        }
    }
}
