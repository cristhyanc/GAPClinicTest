using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GAPClinicTest.Core.Domain.Entities
{
    [Table("Patient")]
    public  class Patient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid PatientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SocialNumber { get; set; }     
        public DateTime DOB { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
