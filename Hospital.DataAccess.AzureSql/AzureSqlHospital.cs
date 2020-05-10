using System;
using System.Collections.Generic;
using Hospital.BaseClasses.Intefaces;
using Hospital.BaseClasses.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data.Common;

namespace Hospital.DataAccess.AzureSql
{
    public class AzureSqlHospital : IHospitalRepo
    {
        private readonly string connstring; 

        public AzureSqlHospital(string connectionString)
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
                            hc.Address = reader.GetString(2);
                            hc.City = reader.GetString(3);
                            hc.Pincode = reader.GetInt32(4);
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
                            hc.Address = reader.GetString(2);
                            hc.City = reader.GetString(3);
                            hc.Pincode = reader.GetInt32(4);                             
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

        public int AddHospital(HospitalCentre hc)
        {
            if(string.IsNullOrEmpty(connstring))
                throw new ArgumentException("No connection string in config.json");
             
            int idRowsAffected = 0;

            using (var conn = new SqlConnection(connstring))
            {
                string maxIdPlus1 = "(select max (id) + 1 from dbo.hospital)";
                
                var sql = "INSERT INTO dbo.hospital (id , hospitalname, Address, City, Pincode) " +
                                       "   VALUES ( " + maxIdPlus1 + ", '" + hc.Name + "'  , '" + hc.Address + "', '" 
                                            + hc.City + "' , "  + hc.Pincode +  " )"; 
                
                
                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    idRowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine("Added a row.");
                    conn.Close();
                }
                return idRowsAffected;
            }
        }

        public int DeleteHospital(int id)
        {
            if(string.IsNullOrEmpty(connstring))
                throw new ArgumentException("No connection string in config.json"); 
            
            int idRowsAffected = 0;
            using (var conn = new SqlConnection(connstring))
            {
                var sql = "Delete FROM dbo.Hospital where id = " + id;
                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    idRowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return idRowsAffected;
            }
        }
// hospitalname, Address, City, Pincode
        public int UpdateHospital(HospitalCentre hc)
        {
            if(string.IsNullOrEmpty(connstring))
                throw new ArgumentException("No connection string in config.json"); 
            
            int idRowsAffected = 0;
            string updateCmd = "UPDATE dbo.Hospital SET hospitalname = '" + hc.Name + "', Address = '" + hc.Address + "',  City = '" + hc.City + "', Pincode = " + hc.Pincode + " where id = " + hc.Id;
            Console.WriteLine(updateCmd);
            using (var conn = new SqlConnection(connstring))
            {
                using (var cmd = new SqlCommand(updateCmd, conn))
                {
                    conn.Open();
                    idRowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return idRowsAffected;
            }
        }
    }
}
