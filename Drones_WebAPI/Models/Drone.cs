namespace Drones_WebAPI.Models
{
    public class Drone
    {
        public long Id { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public double WeightLimit { get; set; }
        public double BatteryCapacity { get; set; }
        public string State { get; set; }

        public ICollection<Medication> Medications { get; set; }
    }
}
