using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GAPClinicTest.Core.Domain.Entities
{
    [Table("Appointment")]
    public  class Appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Guid PatientID { get; set; }
        public AppointmentType AppointmentType { get; set; }
    }
}
