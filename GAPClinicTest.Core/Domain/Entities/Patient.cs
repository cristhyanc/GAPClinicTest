using GAPClinicTest.Core.Interfaces;
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

        public void Save(IUnitOfWork uow)
        {
            if (string.IsNullOrEmpty(this.FirstName))
            {
                throw new ValidationException("FirstName is required");
            }

            if (string.IsNullOrEmpty(this.LastName))
            {
                throw new ValidationException("LastName is required");
            }
                     

            if (this.PatientID == Guid.Empty)
            {
                uow.PatientRepository.Insert(this);
            }
            else
            {
                uow.PatientRepository.Update(this);
            }

        }


    }
}
