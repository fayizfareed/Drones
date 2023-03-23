namespace Drones_WebAPI.Models
{
    public class Medications
    {
        public int Id { get; set; }
        public Drone DroneId { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
    }
}
