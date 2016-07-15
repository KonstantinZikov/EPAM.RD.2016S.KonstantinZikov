using System.Diagnostics;

namespace Utils
{
    public interface ILogger
    {
        void Log(TraceEventType eventType, string message);
    }
}
