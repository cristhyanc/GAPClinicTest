using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GAPClinicTest.Core.Domain.Entities;
using GAPClinicTest.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GAPClinicTest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AppointmentController : ControllerBase
    {
        IAppointmentUserCase appointmentUseCase;
        public AppointmentController(IAppointmentUserCase appointmentUser )
        {
            appointmentUseCase = appointmentUser;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Appointment>> Get()
        {
            try
            {
                return Ok(appointmentUseCase.GetAppointments());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Appointment> GetById([FromQuery] Guid id)
        {
            try
            {
                return Ok(appointmentUseCase.GetAppointment(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<Appointment> Post([FromBody] Appointment appointment)
        {
            try
            {
                return Created("", appointmentUseCase.SaveAppointment(appointment));
            }           
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
        [HttpDelete("{id}")]
        public ActionResult Delete([FromQuery] Guid id)
        {
            try
            {
                appointmentUseCase.DeleteAppointment(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}