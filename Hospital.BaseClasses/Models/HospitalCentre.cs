using System;

namespace Hospital.BaseClasses.Models
{
    public class HospitalCentre  
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Pincode { get; set; }
    }
}
