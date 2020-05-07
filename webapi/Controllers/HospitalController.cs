using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hospital.BaseClasses.Intefaces;
using Hospital.BaseClasses.Models;


namespace Hospital.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HospitalController : ControllerBase
    {
        private static readonly string[] Hospials = new[]
        {
            "Bowring & Lady Curzon Hospital", "Central Leprosorium", "E.S.I Hospital", "Epidemic Diseases Hospial", "Haji Sir Ismail Sait Ghosha Hospital", "Indira Gandhi Institute of Child Health Hospital", "Jayadeva Institute of Cardiology", "K C General Hospital", "Kidwai Memorial Institute of Oncology", "Lady Willington State T.B Centre"
        };

        private readonly ILogger<HospitalController> _logger;
        private readonly IHospital _hospital;

        public HospitalController(IHospital iHospital, ILogger<HospitalController> logger)
        {
            _hospital = iHospital;
            _logger = logger;
        }

        [HttpGet]
        public List<HospitalCentre> Get()
        {
            /*return Hospials.ToList();*/
            _logger.LogInformation("Inside cotroller get");
            return _hospital.GetHospitals();
        }

        /// <summary>
        /// Find hospital by ID
        /// </summary>
        /// <remarks>Returns a single hospital</remarks>
        /// <param name="hospitalId">ID of hospital to return</param>
        /// <response code="200">successful operation</response>
        /// <response code="400">Invalid ID supplied</response>
        /// <response code="404">Hospital not found</response>
        /// <response code="500">Server Error</response>
        [HttpGet]
        [Route("/hospital/{hospitalId}")]               
        public virtual IActionResult GetHospitalById([FromRoute]int? hospitalId)
        { 
            if ((hospitalId < 1) || (hospitalId > 100))
            {
                return StatusCode(400, hospitalId);      
            }
            try
            {    
                HospitalCentre hc = _hospital.GetHospital((int)hospitalId);
                if(hc == null)
                {
                    return this.NotFound("No hospital found for id : " + hospitalId);                
                }
                return StatusCode(200, hc);            
            } 
            catch(Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        /// <summary>
        /// Add a new hospital to the list
        /// </summary>
        
        /// <param name="body">Hospital object that needs to be added to the list</param>
        /// <response code="405">Invalid input</response>
        [HttpPost]
        [Route("/hospital")] 
        public virtual IActionResult AddHospital([FromBody]HospitalCentre hc)
        { 
            //TODO: Uncomment the next line to return response 405 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(405);
            _hospital.AddHospital(hc);
            return StatusCode(201, hc);  
        }

        /// <summary>
        /// Deletes a hospital
        /// </summary>        
        /// <param name="hospitalId">Hospital id to delete</param>        
        /// <response code="400">Invalid ID supplied</response>
        /// <response code="404">Hospital not found</response>
        [HttpDelete]
        [Route("/hospital/{hospitalId}")] 
        public virtual IActionResult DeleteHospital([FromRoute]int hospitalId)
        { 
            Console.WriteLine("Delete hospital with id: " + hospitalId);
            int rowsAffected =_hospital.DeleteHospital(hospitalId);
            if(rowsAffected == 0)
            {
                return this.NotFound("No hospital found to delete");
            }
            else
            {
                return Ok("Deleted Hospital with id: " + hospitalId);
            }
        }

        /// <summary>
        /// Updates a hospital in the list
        /// </summary>
        /// <remarks>Updates a hospital to the list</remarks>
        /// <param name="body">Updates Hospital in the list</param>
        /// <response code="200">hospital updated</response>
        /// <response code="400">invalid input, object invalid</response>
        /// <response code="404">hospital does not exists</response>
        [HttpPatch]
        [Route("/hospital")]  
        public virtual IActionResult UpdateHospital([FromBody]HospitalCentre hc)
        { 
            Console.WriteLine("Update hospital Controller");
            int rowsAffected =_hospital.UpdateHospital(hc);
            if(rowsAffected > 0)
            {
                return Ok(hc);
            }
            else
            {
                return StatusCode(404, hc);
            } 
        }
    }
}
