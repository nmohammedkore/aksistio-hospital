using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
