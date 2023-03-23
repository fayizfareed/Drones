using Drones_WebAPI.DataAccess;
using Drones_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Drones_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DroneController : ControllerBase
    {
        private readonly DronesDbContext _dbContext;
        public DroneController(DronesDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public IEnumerable<Medication> GetDrones()
        {
            return _dbContext.Medications.ToList();
        }
    }
}
