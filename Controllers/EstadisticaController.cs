using Microsoft.AspNetCore.Mvc;
using MedicalApi.Interfaces;
using MedicalApi.Helpers;

namespace MedicalApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EstadisticaController : ControllerBase
{
    private readonly IEstadisticaService _service;

    public EstadisticaController(IEstadisticaService service)
    {
        _service = service;
    }

    [HttpGet("distribucion-horaria")]
    public async Task<IActionResult> GetDistribucionHoraria()
    {
        var data = await _service.GetDistribucionHoraria();
        return Ok(ResponseHelper.Success(
            "Distribución horaria obtenida con éxito",
            data));
    }

    [HttpGet("doctor-mas-solicitado")]
    public async Task<IActionResult> GetDoctorMasSolicitado()
    {
        var data = await _service.GetDoctorMasSolicitado();
        return Ok(ResponseHelper.Success(
            "Estadísticas del doctor más solicitado obtenidas con éxito",
            data));
    }

    [HttpGet("especialidad-mas-solicitada")]
    public async Task<IActionResult> GetEspecialidadMasSolicitada()
    {
        var data = await _service.GetEspecialidadMasSolicitada();
        return Ok(ResponseHelper.Success(
            "Estadísticas de la especialidad más solicitada obtenidas con éxito",
            data));
    }
}
