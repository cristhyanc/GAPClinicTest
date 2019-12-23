using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAPClinicTest.Core.Domain.Entities;
using GAPClinicTest.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GAPClinicTest.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    
    public class PatientController : ControllerBase
    {
        IPatientUseCase patientUseCase;
        public PatientController(IPatientUseCase patient)
        {
            patientUseCase = patient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Patient>> Get()
        {
            try
            {
                return Ok(patientUseCase.GetPatients());
            }          
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Patient> GetById([FromQuery] Guid id)
        {
            try
            {
                return Ok(patientUseCase.GetPatient(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<Patient> Post([FromBody] Patient patient)
        {
            try
            {
                return Created("", patientUseCase.Save (patient));
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
                patientUseCase.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}