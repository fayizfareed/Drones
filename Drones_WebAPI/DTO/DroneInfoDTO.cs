namespace Drones_WebAPI.DTO
{
    public class DroneInfoDTO
    {
        public long Id { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public double WeightLimit { get; set; }
        public double BatteryCapacity { get; set; }
        public string State { get; set; }
        public List<NewMedicationDTO> NewMedication { get; set; } = new List<NewMedicationDTO>();
    }
}
