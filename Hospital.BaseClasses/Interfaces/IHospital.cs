using System;
using Hospital.BaseClasses.Models;
using System.Collections.Generic;

namespace Hospital.BaseClasses.Intefaces
{
    public interface IHospital
    {
        List<HospitalCentre> GetHospitals();
        HospitalCentre GetHospital(int id);
    }
}
