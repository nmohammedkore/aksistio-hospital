using System;
using Hospital.BaseClasses.Models;
using System.Collections.Generic;

namespace Hospital.BaseClasses.Intefaces
{
    public interface IHospitalRepo
    {
        List<HospitalCentre> GetHospitals();
        HospitalCentre GetHospital(int id);
        int AddHospital(HospitalCentre hc);
        int UpdateHospital(HospitalCentre hc);
        int DeleteHospital(int hospitalId);
    }
}
