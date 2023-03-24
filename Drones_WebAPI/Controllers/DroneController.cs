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
        [HttpPost]
        [Route("RegisterDrone")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult RegisterDrone([FromBody] NewDroneDTO droneDTO)
        {
            Drone droneExist = _dbContext.Drones.Where(x => x.SerialNumber == droneDTO.SerialNumber).FirstOrDefault();
            if (droneDTO.SerialNumber.Length > 100)
            {
                Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                return new JsonResult(new { status = "Failed", messge = "Serial number exceed max length of 100" });
            }
            if (droneDTO.Model != DroneModels.Heavyweight.ToString()
                && droneDTO.Model != DroneModels.Lightweight.ToString()
                && droneDTO.Model != DroneModels.Middleweight.ToString()
                && droneDTO.Model != DroneModels.Cruiserweight.ToString())
            {
                Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                return new JsonResult(new { status = "Failed", messge = "Model must be one of these Lightweight, Middleweight, Heavyweight, Cruiserweight" });
            }
            if (droneDTO.WeightLimit > 500)
            {
                Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                return new JsonResult(new { status = "Failed", messge = "Wheight must be less than 500" });
            }
            if (droneDTO.BatteryCapacity > 100)
            {
                Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                return new JsonResult(new { status = "Failed", messge = "Battery capacity must be less than 100" });
            }
            if (droneExist != null)
            {
                Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                return new JsonResult(new { status = "Failed", messge = "There is a drone with same serial number" });
            }
            long id = _dbContext.Drones.Max(x => x.Id) + 1;
            Drone drone = new Drone()
            {
                BatteryCapacity = droneDTO.BatteryCapacity,
                Id = id,
                Model = droneDTO.Model,
                SerialNumber = droneDTO.SerialNumber,
                State = DroneState.IDLE.ToString(),
                WeightLimit = droneDTO.WeightLimit
            };

            _dbContext.Drones.Add(drone);
            _dbContext.SaveChanges();

            return new JsonResult(new { status = "Success", droneId = id, messge = "Data Saved Successfully" });
        }

    }
}
