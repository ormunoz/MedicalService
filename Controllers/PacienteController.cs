namespace MedicalApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using MedicalApi.Interfaces;
using MedicalApi.Models;
using MedicalApi.Helpers;
using MedicalApi.DTOs;


[ApiController]
[Route("api/v1/[controller]")]
public class PacienteController : ControllerBase
{
    private readonly IPacienteService _service;

    public PacienteController(IPacienteService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var data = await _service.GetAll();

        return Ok(ResponseHelper.Success(
            "Listado de pacientes",
            data));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var paciente = await _service.GetById(id);

        if (paciente == null)
            return NotFound();

        return Ok(ResponseHelper.Success(
           "Paciente encontrado con exito",
           paciente));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Paciente paciente)
    {
        try
        {
            await _service.Create(paciente);

            return Ok(ResponseHelper.Success(
                "Paciente creado con éxito",
                paciente));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                ResponseHelper.Error<object>(ex.Message));
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Paciente paciente)
    {
        try
        {
            await _service.Update(id, paciente);

            return Ok(ResponseHelper.Success<object>(
                "Paciente actualizado con éxito",
                paciente));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                ResponseHelper.Error<object>(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);

        return Ok(ResponseHelper.Success<object>(
            "Paciente eliminado con éxito"));
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar(
        [FromQuery] PacienteFiltroDto filtro)
    {
        var data = await _service.Buscar(filtro);

        return Ok(ResponseHelper.Success(
            "Búsqueda realizada con éxito",
            data));
    }

}