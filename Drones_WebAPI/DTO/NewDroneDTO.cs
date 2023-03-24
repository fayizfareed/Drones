namespace Drones_WebAPI.DTO
{
    public class NewDroneDTO
    {
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public double WeightLimit { get; set; }
        public double BatteryCapacity { get; set; }
    }
}
