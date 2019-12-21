﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAPClinicTest.DTO
{
    public class Patient
    {
        public Guid PatientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SocialNumber { get; set; }
        public DateTime DOB { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
