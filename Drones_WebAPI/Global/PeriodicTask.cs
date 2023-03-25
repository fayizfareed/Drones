using Drones_WebAPI.DataAccess;
using Drones_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Drones_WebAPI.Global
{
    public class PeriodicTask : BackgroundService
    {
        private const int generalDelay = 1 * 10 * 1000; // 10 seconds

        IDbContextFactory<DronesDbContext> myDbContextFactory;
        public PeriodicTask(IDbContextFactory<DronesDbContext> mydbcontext) 
        { 
            myDbContextFactory = mydbcontext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(generalDelay, stoppingToken);
                await CheckBatteryLevel();
            }
        }

        private Task CheckBatteryLevel()
        {
            var _dbContext = myDbContextFactory.CreateDbContext();
            List<Drone> drones = _dbContext.Drones.ToList();
            Console.WriteLine("");
            foreach (Drone drone in drones)
            {
                string message = "Battery Level For Drone " + drone.SerialNumber + " Is " + drone.BatteryCapacity + "% | Drone Id : " + drone.Id;
                MyEventLog.WriteLog(message);
                Console.WriteLine(message);
            }
            
            return Task.FromResult("Done");
        }
    }
}
