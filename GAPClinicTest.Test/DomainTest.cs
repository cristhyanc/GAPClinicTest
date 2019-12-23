using GAPClinicTest.Core.Domain.Entities;
using GAPClinicTest.Core.UseCases;
using GAPClinicTest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SQLite;
using System.Text;
using Xunit;

namespace GAPClinicTest.Test
{
  public  class DomainTest
    {
       
        [Theory ]
        [InlineData("John", "DOe")]
        [InlineData("Jim", "Smit")]
        public void Test_Create_Patient(string name,string lastname)
        {
            var options = new DbContextOptionsBuilder<ClinicContext>()
              .UseInMemoryDatabase(databaseName: "Test1")
              .Options;
            ClinicContext context = new ClinicContext(options);
                     

            UnitOfWork uow = new UnitOfWork(context);
            PatientUseCase  patientUse  = new PatientUseCase(uow);

            Patient patient = new Patient();
            patient.FirstName = name;
            patient.LastName = lastname;
            patient.DOB = DateTime.Now.AddYears(-30);

            patientUse.Save(patient);

            Assert.NotEqual(patient.PatientID, Guid.Empty);



        }

        [Theory]
        [InlineData("", "DOe")]
        [InlineData("", "Smit")]
        public void Test_Create_Patient_Name_Required(string name, string lastname)
        {
            var options = new DbContextOptionsBuilder<ClinicContext>()
              .UseInMemoryDatabase(databaseName: "Test1")
              .Options;
            ClinicContext context = new ClinicContext(options);


            UnitOfWork uow = new UnitOfWork(context);
            PatientUseCase patientUse = new PatientUseCase(uow);

            Patient patient = new Patient();
            patient.FirstName = name;
            patient.LastName = lastname;
            patient.DOB = DateTime.Now.AddYears(-30);

            try
            {
                patientUse.Save(patient);
            }
            catch (ValidationException ex)
            {
                Assert.Equal("FirstName is required",ex.Message);
            }
            
            
        }

        [Theory]
        [InlineData("John", "")]
        [InlineData("Jim", "")]
        public void Test_Create_Patient_LastName_Required(string name, string lastname)
        {
            var options = new DbContextOptionsBuilder<ClinicContext>()
              .UseInMemoryDatabase(databaseName: "Test1")
              .Options;
            ClinicContext context = new ClinicContext(options);


            UnitOfWork uow = new UnitOfWork(context);
            PatientUseCase patientUse = new PatientUseCase(uow);

            Patient patient = new Patient();
            patient.FirstName = name;
            patient.LastName = lastname;
            patient.DOB = DateTime.Now.AddYears(-30);

            try
            {
                patientUse.Save(patient);
            }
            catch (ValidationException ex)
            {
                Assert.Equal("LastName is required", ex.Message);
            }


        }

        [Fact]

        public void Test_Create_Appointment()
        {
           
                var options = new DbContextOptionsBuilder<ClinicContext>()
              .UseInMemoryDatabase(databaseName: "Test1")
              .Options;
                ClinicContext context = new ClinicContext(options);


                UnitOfWork uow = new UnitOfWork(context);
                PatientUseCase patientUse = new PatientUseCase(uow);
                AppointmentUserCase appointmentUser = new AppointmentUserCase(uow);

                Patient patient = new Patient();
                patient.FirstName = "John";
                patient.LastName = "Doe";
                patient.DOB = DateTime.Now.AddYears(-30);
                patientUse.Save(patient);


                Appointment appointment = new Appointment();
                appointment.AppointmentDate = DateTime.Now.AddHours(2);
                appointment.PatientID = patient.PatientID;
                appointment.AppointmentType = Core.AppointmentType.Neurology;
                appointmentUser.SaveAppointment(appointment);

                Assert.NotEqual(appointment.AppointmentID, Guid.Empty);

           


        }

        public void Test_Create_2_Appointments_SameDay()
        {
            try
            {
                var options = new DbContextOptionsBuilder<ClinicContext>()
              .UseInMemoryDatabase(databaseName: "Test1")
              .Options;
                ClinicContext context = new ClinicContext(options);


                UnitOfWork uow = new UnitOfWork(context);
                PatientUseCase patientUse = new PatientUseCase(uow);
                AppointmentUserCase appointmentUser = new AppointmentUserCase(uow);

                Patient patient = new Patient();
                patient.FirstName = "John";
                patient.LastName = "Doe";
                patient.DOB = DateTime.Now.AddYears(-30);
                patientUse.Save(patient);


                Appointment appointment = new Appointment();
                appointment.AppointmentDate = DateTime.Now.AddHours(2);
                appointment.PatientID = patient.PatientID;
                appointment.AppointmentType = Core.AppointmentType.Neurology;
                appointmentUser.SaveAppointment(appointment);

                appointment = new Appointment();
                appointment.AppointmentDate = DateTime.Now.AddHours(2);
                appointment.PatientID = patient.PatientID;
                appointment.AppointmentType = Core.AppointmentType.Odontology;
                appointmentUser.SaveAppointment(appointment);

               

            }
            catch (ValidationException ex)
            {
                Assert.Equal("A Patient can't have more than one Appointment in the same day", ex.Message);
            }


        }


        public void Test_Delete_Appointment()
        {
            try
            {
                var options = new DbContextOptionsBuilder<ClinicContext>()
              .UseInMemoryDatabase(databaseName: "Test1")
              .Options;
                ClinicContext context = new ClinicContext(options);


                UnitOfWork uow = new UnitOfWork(context);
                PatientUseCase patientUse = new PatientUseCase(uow);
                AppointmentUserCase appointmentUser = new AppointmentUserCase(uow);

                Patient patient = new Patient();
                patient.FirstName = "John";
                patient.LastName = "Doe";
                patient.DOB = DateTime.Now.AddYears(-30);
                patientUse.Save(patient);


                Appointment appointment = new Appointment();
                appointment.AppointmentDate = DateTime.Now;
                appointment.PatientID = patient.PatientID;
                appointment.AppointmentType = Core.AppointmentType.Neurology;
                appointmentUser.SaveAppointment(appointment);

                appointmentUser.DeleteAppointment(appointment.AppointmentID);
                Assert.True(true);


            }
            catch (ValidationException ex)
            {
                Assert.Equal("A Patient can't have more than one Appointment in the same day", ex.Message);
            }


        }

        public void Test_Delete_Appointment_24Hours()
        {
            try
            {
                var options = new DbContextOptionsBuilder<ClinicContext>()
              .UseInMemoryDatabase(databaseName: "Test1")
              .Options;
                ClinicContext context = new ClinicContext(options);


                UnitOfWork uow = new UnitOfWork(context);
                PatientUseCase patientUse = new PatientUseCase(uow);
                AppointmentUserCase appointmentUser = new AppointmentUserCase(uow);

                Patient patient = new Patient();
                patient.FirstName = "John";
                patient.LastName = "Doe";
                patient.DOB = DateTime.Now.AddYears(-30);
                patientUse.Save(patient);


                Appointment appointment = new Appointment();
                appointment.AppointmentDate = DateTime.Now;
                appointment.PatientID = patient.PatientID;
                appointment.AppointmentType = Core.AppointmentType.Neurology;
                appointmentUser.SaveAppointment(appointment);

                appointmentUser.DeleteAppointment(appointment.AppointmentID );



            }
            catch (ValidationException ex)
            {
                Assert.Equal("Appointments must be canceled at least 24 hours in advance.", ex.Message);
            }


        }
    }
}
