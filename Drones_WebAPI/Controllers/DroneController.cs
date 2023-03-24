using Drones_WebAPI.DataAccess;
using Drones_WebAPI.DTO;
using Drones_WebAPI.Global;
using Drones_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.RegularExpressions;

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

    }
}
