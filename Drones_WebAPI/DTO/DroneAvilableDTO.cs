namespace Drones_WebAPI.DTO
{
    public class DroneAvilableDTO
    {
        public long Id { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public double WeightLimit { get; set; }
        public double BatteryCapacity { get; set; }
        public string State { get; set; }
        public double CurrentWeight { get; set; }
    }
}
