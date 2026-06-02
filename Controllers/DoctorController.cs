namespace MedicalApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using MedicalApi.Interfaces;
using MedicalApi.Models;
using MedicalApi.Helpers;


[ApiController]
[Route("api/v1/[controller]")]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _service;

    public DoctorController(IDoctorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAll();

        return Ok(ResponseHelper.Success(
            "Listado de Doctores Obtenida con éxito",
            data));
    }


    [HttpGet("{doctorId}")]
    public async Task<IActionResult> GetById(int doctorId)
    {
        var doctor = await _service.GetById(doctorId);

        if (doctor == null)
            return NotFound();

        return Ok(ResponseHelper.Success(
           "doctor encontrado con exito",
           doctor));
    }


    [HttpPost]
    public async Task<IActionResult> Create(Doctor  doctor)
    {
        try
        {
            await _service.Create(doctor);

            return Ok(ResponseHelper.Success(
                "doctor creado con éxito",
                doctor));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                ResponseHelper.Error<object>(ex.Message));
        }
    }



    [HttpPut("{doctorId}")]
    public async Task<IActionResult> Update(int doctorId, Doctor doctor)
    {
        try
        {
            await _service.Update(doctorId, doctor);

            return Ok(ResponseHelper.Success<object>(
                "Doctor actualizado con éxito",
                doctor));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                ResponseHelper.Error<object>(ex.Message));
        }
    }

    [HttpDelete("{doctorId}")]
    public async Task<IActionResult> Delete(int doctorId)
    {
        await _service.Delete(doctorId);

        return Ok(ResponseHelper.Success<object>(
            "Doctor eliminado con éxito"));
    }

}