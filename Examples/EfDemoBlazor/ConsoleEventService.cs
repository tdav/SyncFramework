using System;

namespace EfDemoBlazor
{
    public class ConsoleEventService
    {
        public Action<string> WriteToEventLog { get; set; }
    }
}
