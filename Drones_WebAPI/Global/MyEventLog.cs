using System.Diagnostics;

namespace Drones_WebAPI.Global
{
    public static class MyEventLog
    {
        public static void RegisterEventLog()
        {
            try
            {
                if (!EventLog.SourceExists("DroneSource"))
                {
                    EventLog.CreateEventSource("DroneSource", "DroneBatteryLevel");
                    Console.WriteLine("CreatedEventSource");
                    Console.WriteLine("Exiting, execute the application a second time to use the source.");
                    return;
                }
            }
            catch (Exception e) 
            {
                
            }
        }
        public static void WriteLog(string message)
        {
            try
            {
                EventLog myLog = new EventLog();
                myLog.Source = "DroneSource";
                myLog.WriteEntry(message);
            }
            catch (Exception ex) { }
        }
    }
}
