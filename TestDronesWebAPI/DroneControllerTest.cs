using Drones_WebAPI.Controllers;
using Drones_WebAPI.DataAccess;
using Drones_WebAPI.DTO;
using Drones_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Nodes;
using Xunit;

namespace TestDronesWebAPI
{
    public class DroneControllerTest
    {
        private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("https://localhost:7145") };
        [Fact]
        public async Task Test_Drone_Serial_Number_Limit_Exceed()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.UnprocessableEntity;
            var drone = new NewDroneDTO()
            {
                BatteryCapacity = 100,
                Model = "Middleweight",
                SerialNumber = "asdfadkfj adkf adlfk ad;lsfk ad;flak dfalkd faldfk alsdkf adslkf adlfa dlkf ajd;fl ajdfl adkf aasdfadkfj adkf adlfk ad;lsfk ad;flak dfalkd faldfk alsdkf adslkf adlfa dlkf ajd;fl ajdfl adkf aasdfadkfj adkf adlfk ad;lsfk ad;flak dfalkd faldfk alsdkf adslkf adlfa dlkf ajd;fl ajdfl adkf aasdfadkfj adkf adlfk ad;lsfk ad;flak dfalkd faldfk alsdkf adslkf adlfa dlkf ajd;fl ajdfl adkf aasdfadkfj adkf adlfk ad;lsfk ad;flak dfalkd faldfk alsdkf adslkf adlfa dlkf ajd;fl ajdfl adkf aasdfadkfj adkf adlfk ad;lsfk ad;flak dfalkd faldfk alsdkf adslkf adlfa dlkf ajd;fl ajdfl adkf aasdfadkfj adkf adlfk ad;lsfk ad;flak dfalkd faldfk alsdkf adslkf adlfa dlkf ajd;fl ajdfl adkf aasdfadkfj adkf adlfk ad;lsfk ad;flak dfalkd faldfk alsdkf adslkf adlfa dlkf ajd;fl ajdfl adkf aasdfadkfj adkf adlfk ad;lsfk ad;flak dfalkd faldfk alsdkf adslkf adlfa dlkf ajd;fl ajdfl adkf aasdfadkfj adkf adlfk ad;lsfk ad;flak dfalkd faldfk alsdkf adslkf adlfa dlkf ajd;fl ajdfl adkf a",
                WeightLimit = 100,
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PostAsync("/api/Drone/RegisterDrone", Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "Serial number exceed max length of 100" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Drone_Model_Is_Not_In_Predefined()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.UnprocessableEntity;
            var drone = new NewDroneDTO()
            {
                BatteryCapacity = 100,
                Model = "abcd",
                SerialNumber = "ABC3948N",
                WeightLimit = 100,
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PostAsync("/api/Drone/RegisterDrone", Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "Model must be one of these Lightweight, Middleweight, Heavyweight, Cruiserweight" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);

        }

        [Fact]
        public async Task Test_Drone_Weight_Limit_Exceed_500()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.UnprocessableEntity;
            var drone = new NewDroneDTO()
            {
                BatteryCapacity = 100,
                Model = "Middleweight",
                SerialNumber = "ABC3948N",
                WeightLimit = 600,
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PostAsync("/api/Drone/RegisterDrone", Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "Wheight must be less than 500" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Drone_Battery_Capacity_Exceed_100()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.UnprocessableEntity;
            var drone = new NewDroneDTO()
            {
                BatteryCapacity = 300,
                Model = "Middleweight",
                SerialNumber = "ABC3948N",
                WeightLimit = 100,
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PostAsync("/api/Drone/RegisterDrone", Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "Battery capacity must be less than 100" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Drone_Is_AllreadyExist()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.UnprocessableEntity;
            var drone = new NewDroneDTO()
            {
                BatteryCapacity = 100,
                Model = "Middleweight",
                SerialNumber = "XDSB12S4XS56",
                WeightLimit = 100,
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PostAsync("/api/Drone/RegisterDrone", Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "There is a drone with same serial number" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Drone_Registration_Successfull()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var drone = new NewDroneDTO()
            {
                BatteryCapacity = 100,
                Model = "Middleweight",
                SerialNumber = "ABC3948N",
                WeightLimit = 100,
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PostAsync("/api/Drone/RegisterDrone", Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Success", droneId = 14, messge = "Data Saved Successfully" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Load_Drone_GivenDrone_Not_Exist()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.NotFound;
            var drone = new NewMedicationDTO()
            {
                Code = "ABCD",
                DroneId = 100,
                DroneSerialNumber = "",
                Image = "",
                Name = "ABCD",
                Weight = 50
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PostAsync("/api/Drone/LoadDrone", Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "Drone not found for the id or serial number" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Load_Drone_Given_Drone_Is_Not_In_Idle_or_Loading_State()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.NotFound;
            var drone = new NewMedicationDTO()
            {
                Code = "ABCD",
                DroneId = 4,
                DroneSerialNumber = "",
                Image = "",
                Name = "ABCD",
                Weight = 50
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PostAsync("/api/Drone/LoadDrone", Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "Drone is not in idle state or loading state. Current drone state is DELIVERED" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Load_Drone_Invalid_Medication_Name()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.UnprocessableEntity;
            var drone = new NewMedicationDTO()
            {
                Code = "ABCD",
                DroneId = 1,
                DroneSerialNumber = "",
                Image = "",
                Name = "ABCD$",
                Weight = 50
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PostAsync("/api/Drone/LoadDrone", Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "Medication name must be mixed of letters, number, '-' and '_'" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Load_Drone_Invalid_Medication_Code()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.UnprocessableEntity;
            var drone = new NewMedicationDTO()
            {
                Code = "ABCD$",
                DroneId = 1,
                DroneSerialNumber = "",
                Image = "",
                Name = "ABCD",
                Weight = 50
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PostAsync("/api/Drone/LoadDrone", Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "Medication code must be mixed of upper case letters, number, and '_'" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Load_Drone_Medication_Code_Exist()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.UnprocessableEntity;
            var drone = new NewMedicationDTO()
            {
                Code = "ABCD_1",
                DroneId = 1,
                DroneSerialNumber = "",
                Image = "",
                Name = "ABCD",
                Weight = 50
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PostAsync("/api/Drone/LoadDrone", Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "There is a medication with same code" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Load_Drone_Drone_Battery_Capacity_Less_Than_25_Percentage()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.UnprocessableEntity;
            var drone = new NewMedicationDTO()
            {
                Code = "ABCDFG",
                DroneId = 13,
                DroneSerialNumber = "",
                Image = "",
                Name = "ABCDFG",
                Weight = 50
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PostAsync("/api/Drone/LoadDrone", Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "Drone's battery level is below 25%" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Load_Drone_Success()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var drone = new NewMedicationDTO()
            {
                Code = "ABCD",
                DroneId = 1,
                DroneSerialNumber = "",
                Image = "",
                Name = "ABCD",
                Weight = 50
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PostAsync("/api/Drone/LoadDrone", Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Success", droneId = 1, mediccationId = 11, messge = "Drone Loaded Successfully" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Change_Drone_State_Drone_Not_Exist()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.NotFound;
            var drone = new DroneStateDTO()
            {
                State = "DELIVNothingERED"
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PutAsync("/api/Drone/ChangeDroneState/200", Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "Drone not found for the id" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Change_Drone_State_Invalid_State()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.UnprocessableEntity;
            var drone = new DroneStateDTO()
            {
                State = "DELIVNothingERED"
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PutAsync("/api/Drone/ChangeDroneState/" + 1, Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "State must be one of these RETURNING, DELIVERED, LOADING, DELIVERING, IDLE" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Change_Drone_State_To_Loading_To_Battery_Level_Less_Than_25_Percentage()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.UnprocessableEntity;
            var drone = new DroneStateDTO()
            {
                State = "LOADING"
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PutAsync("/api/Drone/ChangeDroneState/" + 13, Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "Drone's battery level is below 25%" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Change_Drone_State_Success()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var drone = new DroneStateDTO()
            {
                State = "LOADING"
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PutAsync("/api/Drone/ChangeDroneState/" + 1, Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Success", droneId = 1, messge = "Drone State Changed Successfully" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Change_Battery_Level_GivenDrone_Not_Exist()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.NotFound;
            var drone = new DroneBatteryDTO()
            {
                BatteryCapacity = 75
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PutAsync("/api/Drone/ChangeBatteryLevel/" + 200, Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "Drone not found for the id" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Change_Battery_Level_Battery_Level_Exceed_100_Percentage()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.UnprocessableEntity;
            var drone = new DroneBatteryDTO()
            {
                BatteryCapacity = 200
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PutAsync("/api/Drone/ChangeBatteryLevel/" + 1, Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Failed", messge = "Battery level must be less than 100%" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Change_Battery_Level_Success()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var drone = new DroneBatteryDTO()
            {
                BatteryCapacity = 100
            };
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PutAsync("/api/Drone/ChangeBatteryLevel/" + 1, Helper.GetJsonStringContent(drone));
            var expectedContent = new { status = "Success", droneId = 1, messge = "Drone Battery Level Changed Successfully" };
            await Helper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task Test_Get_Drones_By_Invalid_Serial_Number()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.NoContent;
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.GetAsync("/api/Drone/GetDronesBySerilNumber/ADFB1234XS56SDF");
            Helper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }

        [Fact]
        public async Task Test_Get_Drones_By_Serial_Number()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.GetAsync("/api/Drone/GetDronesBySerilNumber/ADFB1234XS56");
            Helper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }

        [Fact]
        public async Task Test_Get_Drones_By_Invalid_Id_Type()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.NotFound;
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.GetAsync("/api/Drone/GetDroneById/sdf" + 1456);
            Helper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }

        [Fact]
        public async Task Test_Get_Drones_By_Invalid_Id()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.NoContent;
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.GetAsync("/api/Drone/GetDroneById/" + 1456);
            Helper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }

        [Fact]
        public async Task Test_Get_Drones_By_Id()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.GetAsync("/api/Drone/GetDroneById/" + 1);
            Helper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }

        [Fact]
        public async Task Test_GetDrones()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.GetAsync("/api/Drone/GetDrones"); 
            Helper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }

        [Fact]
        public async Task Test_Get_Loadings_Invalid_ID_DataType()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.NotFound;
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.GetAsync("/api/Drone/GetLoadings/adsfad");
            Helper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }

        [Fact]
        public async Task Test_Get_Loadings()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.GetAsync("/api/Drone/GetLoadings/2");
            Helper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }

        [Fact]
        public async Task Test_Get_Loading_History_Invalid_Id_Type()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.NotFound;
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.GetAsync("/api/Drone/GetLoadingHistory/adf");
            Helper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }

        [Fact]
        public async Task Test_Get_Loading_History()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.GetAsync("/api/Drone/GetLoadingHistory/2");
            Helper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }

        [Fact]
        public async Task Test_Get_Available_Drones()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.GetAsync("/api/Drone/AvailableDrones");
            Helper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }

        [Fact]
        public async Task Test_GetBatteryLevel_Invalid_Id_Type()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.NotFound;
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.GetAsync("/api/Drone/GetBatteryLevel/adf");
            Helper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }

        [Fact]
        public async Task Test_GetBatteryLevel_Invalid_Id()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.NoContent;
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.GetAsync("/api/Drone/GetBatteryLevel/1342");
            Helper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }

        [Fact]
        public async Task Test_GetBatteryLevel()
        {
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.GetAsync("/api/Drone/GetBatteryLevel/1"); 
            Helper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }
    }
}