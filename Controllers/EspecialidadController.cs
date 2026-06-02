namespace MedicalApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using MedicalApi.Interfaces;
using MedicalApi.Models;
using MedicalApi.Helpers;


[ApiController]
[Route("api/v1/[controller]")]
public class EspecialidadController : ControllerBase
{
    private readonly IEspecialidadService _service;

    public EspecialidadController(IEspecialidadService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAll();

        return Ok(ResponseHelper.Success(
            "Listado de Especialidad Obtenido con éxito",
            data));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var especialidad = await _service.GetById(id);

        if (especialidad == null)
            return NotFound();

        return Ok(ResponseHelper.Success(
           "especialidad encontrada con exito",
           especialidad));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Especialidad especialidad)
    {
        try
        {
            await _service.Create(especialidad);

            return Ok(ResponseHelper.Success(
                "Especialidad creada con éxito",
                especialidad));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                ResponseHelper.Error<object>(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Especialidad especialidad)
    {
        try
        {
            await _service.Update(id, especialidad);

            return Ok(ResponseHelper.Success<object>(
                "Especialidad actualizada con éxito",
                especialidad));
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
            "Especialidad eliminada con éxito"));
    }

}