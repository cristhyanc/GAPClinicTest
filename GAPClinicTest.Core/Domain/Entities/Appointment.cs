using GAPClinicTest.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

namespace GAPClinicTest.Core.Domain.Entities
{
    [Table("Appointment")]
    public class Appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Guid PatientID { get; set; }
        public AppointmentType AppointmentType { get; set; }
        public Patient Patient { get; set; }



        public void Save(IUnitOfWork uow)
        {
            if (this.PatientID == Guid.Empty)
            {
                throw new ValidationException("PatiendId is required");
            }

            if (this.AppointmentDate < DateTime.Now)
            {
                throw new ValidationException("Date can't be in the Past");
            }

            var appointments = uow.AppointmentRepository.Get(x => x.PatientID == this.PatientID);
            var newAppoDate = DateTime.Parse(this.AppointmentDate.ToShortDateString());


            foreach (Appointment item in appointments)
            {
                var appoDate = DateTime.Parse(item.AppointmentDate.ToShortDateString());
                if (appoDate == newAppoDate && item.AppointmentID != this.AppointmentID)
                {
                    throw new ValidationException("A Patient can't have more than one Appointment in the same day");
                }
            }

            if (this.AppointmentID == Guid.Empty)
            {
                uow.AppointmentRepository.Insert(this);
            }
            else
            {
                uow.AppointmentRepository.Update(this);
            }
          
        }

        public void Delete(IUnitOfWork uow)
        {
            if (this.AppointmentDate < DateTime.Now.AddDays(1))
            {
                throw new ValidationException("Appointments must be canceled at least 24 hours in advance.");
            }

            uow.AppointmentRepository.Delete(this);

           
        }
    }
}
