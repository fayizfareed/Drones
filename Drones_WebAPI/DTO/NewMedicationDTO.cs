namespace Drones_WebAPI.DTO
{
    public class NewMedicationDTO
    {
        public long DroneId { get; set; }
        public string DroneSerialNumber { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
    }
}
