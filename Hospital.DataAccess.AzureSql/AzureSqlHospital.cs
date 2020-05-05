using System;
using System.Collections.Generic;
using Hospital.BaseClasses.Intefaces;
using Hospital.BaseClasses.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data.Common;



namespace Hospital.DataAccess.AzureSql
{
    public class AzHospital : IHospital
    {
        private readonly string connstring; 

        public AzHospital(string connectionString)
        {
            connstring = connectionString;
        }
        public List<HospitalCentre> GetHospitals()
        {
            if(string.IsNullOrEmpty(connstring))
                throw new ArgumentException("No connection string in config.json");

            List<HospitalCentre> HospitalCentreList = new List<HospitalCentre> ();
            using (var conn = new SqlConnection(connstring))
            {
                var sql = "SELECT top 10 * FROM dbo.Hospital";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            HospitalCentre hc = new HospitalCentre ();
                            hc.Id = reader.GetInt32(0);
                            hc.Name = reader.GetString(1);
                            HospitalCentreList.Add(hc);
                            Console.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                        return null;
                    }
                    reader.Close();
                    conn.Close();
                }
                return HospitalCentreList;
            }
        }

        public HospitalCentre GetHospital(int id)
        {
            if(string.IsNullOrEmpty(connstring))
                throw new ArgumentException("No connection string in config.json");

            HospitalCentre hc = new HospitalCentre();
            using (var conn = new SqlConnection(connstring))
            {
                var sql = "SELECT * FROM dbo.Hospital where id = " + id;
                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            hc.Id = reader.GetInt32(0);
                            hc.Name = reader.GetString(1);
                            Console.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                        return null;
                    }
                    reader.Close();
                    conn.Close();
                }
                return hc;
            }
        }
    }
}
