using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MedicalApi.Models;
using MedicalApi.Interfaces;
using MedicalApi.DTOs;

namespace MedicalApi.Services;

public class AtencionService : IAtencionService
{
    private readonly IConfiguration _configuration;

    public AtencionService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    //obtener todas las atenciones
    public async Task<List<Atencion>> GetAll()
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        return (await connection.QueryAsync<Atencion>(
            "sp_Atencion_GetAll",
            commandType: CommandType.StoredProcedure)).ToList();
    }

    //obtener el promedio de atenciones por especialidad
    public async Task<List<PromedioEspecialidadDto>> GetAverage()
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        var result = await connection.QueryAsync<PromedioEspecialidadDto>(
            "sp_Atencion_PromedioPorEspecialidad",
            commandType: CommandType.StoredProcedure
        );

        return result.ToList();
    }


    //obtener una tencion por el id
    public async Task<Atencion?> GetById(int atencionId)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        return await connection.QueryFirstOrDefaultAsync<Atencion>(
            "sp_Atencion_GetById",
            new { atencionId = atencionId },
            commandType: CommandType.StoredProcedure);
    }

    // Crear una nueva atencion
    public async Task Create(Atencion atencion)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        // Validar solape en una atencion para el mismo Atencion 
        var solapamientoExiste =
            await connection.ExecuteScalarAsync<int?>(
                "sp_Atencion_ExisteSolapamiento",
                new
                {
                    atencion.DoctorId,
                    atencion.FechaHoraInicio,
                    atencion.FechaHoraFin
                },
                commandType: CommandType.StoredProcedure);

        if (solapamientoExiste.HasValue)
        {
            throw new InvalidOperationException(
                "ya existe una atencion con esta fecha y hora");
        }


        // Crear doctor 
         await connection.ExecuteScalarAsync<int>(
            "sp_Atencion_Insert",
            new
            {
                atencion.PacienteId,
                atencion.DoctorId,
                atencion.FechaHoraInicio,
                atencion.FechaHoraFin,
                atencion.Diagnostico
            },
            commandType: CommandType.StoredProcedure);
    }

    //Actualiza una atencion por su id
    public async Task Update(int atencionId, Atencion atencion)
    {
      
        using var connection = new SqlConnection(
        _configuration.GetConnectionString("DefaultConnection"));

        await connection.ExecuteAsync(
            "sp_Atencion_Update",
            new
            {
                AtencionId = atencionId,
                atencion.PacienteId,
                atencion.DoctorId,
                atencion.FechaHoraInicio,
                atencion.FechaHoraFin,
                atencion.Diagnostico
            },
            commandType: CommandType.StoredProcedure
        );

    }

    //Eliminar una atencion por su id
    public async Task Delete(int atencionId)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        await connection.ExecuteAsync(
            "sp_Atencion_Delete",
            new
            {
                AtencionId = atencionId
            },
            commandType: CommandType.StoredProcedure);
    }


    //Buscador de atenciones por filtro
    public async Task<IEnumerable<Atencion>> Buscar(
        AtencionFiltroDto filtro)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        return await connection.QueryAsync<Atencion>(
            "sp_Atencion_Buscar",
            new
            {
                filtro.FechaInicio,
                filtro.FechaFin,
                filtro.DoctorId,
                filtro.PacienteId,
                filtro.EspecialidadId

            },
            commandType: CommandType.StoredProcedure);
    }

}