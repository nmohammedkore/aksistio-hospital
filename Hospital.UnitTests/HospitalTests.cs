using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Xunit;
using Hospital.BaseClasses.Intefaces;
using Hospital.BaseClasses.Models;
using Hospital.Controllers;
using NSubstitute;



namespace Hospital.UnitTests
{
    public class HospitalTests
    {

        [Fact]
        public void GetAllHospitals()
        {
            //Arrange
            var hospitalRepoSub = Substitute.For<IHospitalRepo>();
            hospitalRepoSub.GetHospitals().Returns(new List<HospitalCentre>());
            var logger = Substitute.For<ILogger<HospitalController>>();
            var controller = new HospitalController(hospitalRepoSub, logger);
            
            //Act
            List<HospitalCentre> hList = controller.Get();

            //Assert
            Assert.Empty(hList);
        }
    }
}
