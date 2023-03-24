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

        [HttpPost]
        [Route("LoadDrone")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult LoadDrone([FromBody] NewMedicationDTO medicationDTO)
        {
            Drone drone = _dbContext.Drones.Where(x => x.Id == medicationDTO.DroneId || x.SerialNumber == medicationDTO.DroneSerialNumber).FirstOrDefault();
            Medication medicationExist = _dbContext.Medications.Where(x => x.Code == medicationDTO.Code).FirstOrDefault();
            if (drone == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new JsonResult(new { status = "Failed", messge = "Drone not found for the id or serial number" });
            }
            if (drone.State != DroneState.IDLE.ToString() && drone.State != DroneState.LOADING.ToString())
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new JsonResult(new { status = "Failed", messge = "Drone is not in idle state or loading state. Current drone state is " + drone.State });
            }
            if (drone.State == DroneState.LOADING.ToString())
            {
                double currentWeight = _dbContext.Medications.Where(x => x.State == MedicationState.NOTDELIVERED.ToString() && x.DroneId == drone.Id).Sum(y => y.Weight);
                if (currentWeight + medicationDTO.Weight > drone.WeightLimit)
                {
                    Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                    return new JsonResult(new { status = "Failed", messge = "Medication weight will increase drone max weight. Drone max Weight : " + drone.WeightLimit + ". Current Weight : " + currentWeight });
                }
            }
            if (!Regex.IsMatch(medicationDTO.Name, "^[a-zA-Z0-9_-]+$"))
            {
                Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                return new JsonResult(new { status = "Failed", messge = "Medication name must be mixed of letters, number, '-' and '_'" });
            }
            if (!Regex.IsMatch(medicationDTO.Code, "^[A-Z0-9_]+$"))
            {
                Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                return new JsonResult(new { status = "Failed", messge = "Medication code must be mixed of upper case letters, number, and '_'" });
            }
            if (medicationExist != null)
            {
                Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                return new JsonResult(new { status = "Failed", messge = "There is a medication with same code" });
            }
            long id = _dbContext.Medications.Max(x => x.Id) + 1;
            Medication medication = new Medication()
            {
                Id = id,
                Name = medicationDTO.Name,
                Code = medicationDTO.Code,
                Drone = drone,
                Image = medicationDTO.Image,
                State = MedicationState.NOTDELIVERED.ToString(),
                Weight = medicationDTO.Weight
            };

            drone.State = DroneState.LOADING.ToString();
            _dbContext.Medications.Add(medication);
            _dbContext.Drones.Update(drone);
            _dbContext.SaveChanges();

            return new JsonResult(new { status = "Success", droneId = drone.Id, mediccationId = id, messge = "Drone Loaded Successfully" });
        }

        [HttpPut]
        [Route("ChangeDroneState/{id:long}")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult ChangeDroneState(long id, [FromBody] DroneStateDTO droneStateDTO)
        {
            Drone droneExist = _dbContext.Drones.Where(x => x.Id == id).FirstOrDefault();
            if (droneExist == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new JsonResult(new { status = "Failed", messge = "Drone not found for the id" });
            }
            if (droneStateDTO.State != DroneState.DELIVERED.ToString()
                && droneStateDTO.State != DroneState.RETURNING.ToString()
                && droneStateDTO.State != DroneState.LOADING.ToString()
                && droneStateDTO.State != DroneState.LOADED.ToString()
                && droneStateDTO.State != DroneState.DELIVERING.ToString()
                && droneStateDTO.State != DroneState.IDLE.ToString())
            {
                Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                return new JsonResult(new { status = "Failed", messge = "State must be one of these RETURNING, DELIVERED, LOADING, DELIVERING, IDLE" });
            }
            if (droneStateDTO.State == DroneState.DELIVERED.ToString() || droneStateDTO.State == DroneState.RETURNING.ToString())
            {
                _dbContext.Medications.Where(x => x.DroneId == droneExist.Id && x.State == MedicationState.NOTDELIVERED.ToString()).ToList()
                   .ForEach(xd => xd.State = MedicationState.DELIVERED.ToString());

            }
            droneExist.State = droneStateDTO.State;

            _dbContext.Drones.Update(droneExist);
            _dbContext.SaveChanges();

            return new JsonResult(new { status = "Success", droneId = droneExist.Id, messge = "Drone State Changed Successfully" });
        }

        [HttpGet]
        [Route("GetDronesBySerilNumber/{serialnumber}")]
        public IEnumerable<DroneDTO> GetDronesBySerilNumber(string serialnumber)
        {
            return _dbContext.Drones.Where(xd => xd.SerialNumber == serialnumber).Select(x =>
            new DroneDTO()
            {
                Id = x.Id,
                SerialNumber = x.SerialNumber,
                Model = x.Model,
                WeightLimit = x.WeightLimit,
                BatteryCapacity = x.BatteryCapacity,
                State = x.State
            }
            ).ToList();
        }

        [HttpGet]
        [Route("GetDroneById/{id:long}")]
        public IEnumerable<DroneDTO> GetDroneById(long id)
        {
            return _dbContext.Drones.Where(xd => xd.Id == id).Select(x =>
            new DroneDTO()
            {
                Id = x.Id,
                SerialNumber = x.SerialNumber,
                Model = x.Model,
                WeightLimit = x.WeightLimit,
                BatteryCapacity = x.BatteryCapacity,
                State = x.State
            }
            ).ToList();
        }

        [HttpGet]
        [Route("GetDrones")]
        public IEnumerable<DroneDTO> GetDrones()
        {
            return _dbContext.Drones.Select(x =>
            new DroneDTO()
            {
                Id = x.Id,
                SerialNumber = x.SerialNumber,
                Model = x.Model,
                WeightLimit = x.WeightLimit,
                BatteryCapacity = x.BatteryCapacity,
                State = x.State
            }
            ).ToList();
        }

        [HttpGet]
        [Route("GetLoadings/{id:long}")]
        public IEnumerable<MedicationDTO> GetLoadings(long id)
        {
            return _dbContext.Medications.Where(xd => xd.DroneId == id && xd.State == MedicationState.NOTDELIVERED.ToString()).Select(x =>
            new MedicationDTO()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Weight = x.Weight,
                Image = x.Image
            }
            ).ToList();
        }

    }
}
