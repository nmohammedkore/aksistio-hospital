using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private static readonly string[] Hospials = new[]
        {
            "Bowring & Lady Curzon Hospital", "Central Leprosorium", "E.S.I Hospital", "Epidemic Diseases Hospial", "Haji Sir Ismail Sait Ghosha Hospital", "Indira Gandhi Institute of Child Health Hospital", "Jayadeva Institute of Cardiology", "K C General Hospital", "Kidwai Memorial Institute of Oncology", "Lady Willington State T.B Centre"
        };

        private readonly ILogger<PatientController> _logger;

        public PatientController(ILogger<PatientController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return Hospials.ToList();
        }
    }
}
