using System;
using System.Collections.Generic;
using Hospital.BaseClasses.Intefaces;
using Hospital.BaseClasses.Models;


namespace Hospital.DataAccess.SqlLite
{
    public class SqlLiteHospital : IHospitalRepo
    {
        private readonly string connstring; 

        public SqlLiteHospital(string connectionString)
        {
            connstring = connectionString;
        }

        public List<HospitalCentre> GetHospitals()
        {
            List<HospitalCentre> allCentres = new List<HospitalCentre> ();
            {
                HospitalCentre hC = new HospitalCentre();
                hC.Id = 1;
                hC.Name = "Bowring";
                allCentres.Add(hC);
            }
            {
                HospitalCentre hC = new HospitalCentre();
                hC.Id = 2;
                hC.Name = "Leprose";
                allCentres.Add(hC);
            }
            {
                HospitalCentre hC = new HospitalCentre();
                hC.Id = 3;
                hC.Name = "Cancer";
                allCentres.Add(hC);
            }
            return allCentres;
        }

        public HospitalCentre GetHospital(int id)
        {
            return new HospitalCentre();
        }

        public int AddHospital(HospitalCentre hc)
        {
            return 0;
        }

        public int DeleteHospital(int id)
        {
            return 0;
        }

        public int UpdateHospital(HospitalCentre hc)
        {
            return 0;
        }
    }
}
