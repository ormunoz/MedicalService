namespace MedicalApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using MedicalApi.Interfaces;
using MedicalApi.Models;
using MedicalApi.Helpers;
using Dapper;
using System.Data;
using MedicalApi.DTOs;
using Microsoft.Data.SqlClient;

[ApiController]
[Route("api/v1/[controller]")]
public class AtencionController : ControllerBase
{
    private readonly IAtencionService _service;

    public AtencionController(IAtencionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAll();

        return Ok(ResponseHelper.Success(
            "Listado de atencion obtenida con éxito",
            data));
    }

    [HttpGet("average")]
    public async Task<IActionResult> GetAverage()
    {
        var data = await _service.GetAverage();

        return Ok(ResponseHelper.Success(
            "promedio realizado con exito",
            data));
    }


    [HttpGet("{atencionId}")]
    public async Task<IActionResult> GetById(int atencionId)
    {
        var atencion = await _service.GetById(atencionId);

        if (atencion == null)
            return NotFound();

        return Ok(ResponseHelper.Success(
           "atencion encontrado con exito",
           atencion));
    }


    [HttpPost]
    public async Task<IActionResult> Create(Atencion atencion)
    {
        try
        {
            await _service.Create(atencion);

            return Ok(ResponseHelper.Success(
                "atencion creado con éxito",
                atencion));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                ResponseHelper.Error<object>(ex.Message));
        }
    }

    [HttpPut("{atencionId}")]
    public async Task<IActionResult> Update(int atencionId, Atencion atencion)
    {
        try
        {
            await _service.Update(atencionId, atencion);

            return Ok(ResponseHelper.Success<object>(
                "Atencion actualizada con éxito",
                atencion));
        }
        catch (SqlException ex)
        {
            return BadRequest(ResponseHelper.Error<object>(ex.Message, 400));
        }


    }

    [HttpDelete("{atencionId}")]
    public async Task<IActionResult> Delete(int atencionId)
    {
        await _service.Delete(atencionId);

        return Ok(ResponseHelper.Success<object>(
            "atencion eliminada con éxito"));
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar(
    [FromQuery] AtencionFiltroDto filtro)
    {
        var data = await _service.Buscar(filtro);

        return Ok(ResponseHelper.Success(
            "Búsqueda realizada con éxito",
            data));
    }

}