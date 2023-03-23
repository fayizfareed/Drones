namespace Drones_WebAPI.Models
{
    public class Medication
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }


        public long DroneId { get; set; }
        public Drone Drone { get; set; }
    }
}
