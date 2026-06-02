using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MedicalApi.Models;
using MedicalApi.Interfaces;

namespace MedicalApi.Services;

public class EspecialidadService : IEspecialidadService
{
    private readonly IConfiguration _configuration;

    public EspecialidadService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    //obtener todos las especialidades
    public async Task<List<Especialidad>> GetAll()
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        return (await connection.QueryAsync<Especialidad>(
            "sp_Especialidad_GetAll",
            commandType: CommandType.StoredProcedure)).ToList();
    }

    //obtener una especialidad por id
    public async Task<Especialidad?> GetById(int doctorId)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        return await connection.QueryFirstOrDefaultAsync<Especialidad>(
            "sp_Especialidad_GetById",
            new { especialidadId = doctorId },
            commandType: CommandType.StoredProcedure);
    }

    // Crear un nueva Especialidad
    public async Task Create(Especialidad especialidad)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        // Validar que el nombre no exista
        var especialidadExiste =
            await connection.QueryFirstOrDefaultAsync<Especialidad>(
                "sp_Especialidad_GetByName",
                new
                {
                    especialidad.Nombre
                },
                commandType: CommandType.StoredProcedure);

        if (especialidadExiste != null)
        {
            throw new InvalidOperationException(
                "Ya existe esta especialidad");
        }

        // Crear especialidad
        await connection.ExecuteAsync(
            "sp_Especialidad_Insert",
            new
            {
                especialidad.Nombre,
            },
            commandType: CommandType.StoredProcedure);
    }


    //Actualizar un paciente existente
    public async Task Update(int id, Especialidad especialidad)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        // Validar que el nombre no exista
        var nombreDuplicado =
            await connection.QueryFirstOrDefaultAsync<Especialidad>(
                "sp_Especialidad_ExisteName",
                new
                {
                    EspecialidadId = id,
                    especialidad.Nombre
                },
                commandType: CommandType.StoredProcedure);

        if (nombreDuplicado != null)
        {
            throw new InvalidOperationException(
                "Ya existe una especialidad con este nombre");
        }
        await connection.ExecuteAsync(
            "sp_Especialidad_Update",
            new
            {
                EspecialidadId = id,
                especialidad.Nombre,
            },
            commandType: CommandType.StoredProcedure);
    }

    //Eliminar un paciente por id
    public async Task Delete(int id)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        await connection.ExecuteAsync(
            "sp_Especialidad_Delete",
            new
            {
                EspecialidadId = id
            },
            commandType: CommandType.StoredProcedure);
    }

}