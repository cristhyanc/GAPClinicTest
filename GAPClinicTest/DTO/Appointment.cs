using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAPClinicTest.DTO
{
    public enum AppointmentType
    {
        GeneralPractice,
        Odontology,
        Pediatrics,
        Neurology
    }

    public class Appointment
    {
       

        public Guid AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Guid PatientID { get; set; }
        public AppointmentType AppointmentType { get; set; }
    }
}
