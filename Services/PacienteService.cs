using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MedicalApi.Models;
using MedicalApi.Interfaces;
using MedicalApi.DTOs;

namespace MedicalApi.Services;

public class PacienteService : IPacienteService
{
    private readonly IConfiguration _configuration;

    public PacienteService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    //obtener todos los pacientes
    public async Task<IEnumerable<Paciente>> GetAll()
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        return await connection.QueryAsync<Paciente>(
            "sp_Paciente_GetAll",
            commandType: CommandType.StoredProcedure);
    }

    //obtener un paciente por id
    public async Task<Paciente?> GetById(int id)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        return await connection.QueryFirstOrDefaultAsync<Paciente>(
            "sp_Paciente_GetById",
            new { PacienteId = id },
            commandType: CommandType.StoredProcedure);
    }

    // Crear un nuevo paciente
    public async Task Create(Paciente paciente)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        // Validar que el RUT no exista
        var pacienteExistente =
            await connection.QueryFirstOrDefaultAsync<Paciente>(
                "sp_Paciente_GetByRut",
                new
                {
                    paciente.RUT
                },
                commandType: CommandType.StoredProcedure);

        if (pacienteExistente != null)
        {
            throw new InvalidOperationException(
                "Ya existe un paciente con ese RUT");
        }

        // Crear paciente
        await connection.ExecuteAsync(
            "sp_Paciente_Insert",
            new
            {
                paciente.Nombre,
                paciente.RUT,
                paciente.FechaNacimiento
            },
            commandType: CommandType.StoredProcedure);
    }

    //Actualizar un paciente existente
    public async Task Update(int id, Paciente paciente)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        // Validar que el RUT no exista

        var rutDuplicado =
            await connection.QueryFirstOrDefaultAsync<Paciente>(
                "sp_Paciente_ExisteRut",
                new
                {
                    PacienteId = id,
                    paciente.RUT
                },
                commandType: CommandType.StoredProcedure);

        if (rutDuplicado != null)
        {
            throw new InvalidOperationException(
                "Ya existe otro paciente con ese RUT");
        }

        await connection.ExecuteAsync(
            "sp_Paciente_Update",
            new
            {
                PacienteId = id,
                paciente.Nombre,
                paciente.RUT,
                paciente.FechaNacimiento
            },
            commandType: CommandType.StoredProcedure);
    }

    //Eliminar un paciente por id
    public async Task Delete(int id)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        await connection.ExecuteAsync(
            "sp_Paciente_Delete",
            new
            {
                PacienteId = id
            },
            commandType: CommandType.StoredProcedure);
    }


    //Buscador de pacientes por rut, nombre o rango de edad
    public async Task<IEnumerable<Paciente>> Buscar(
        PacienteFiltroDto filtro)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        return await connection.QueryAsync<Paciente>(
            "sp_Paciente_Buscar",
            new
            {
                filtro.Rut,
                filtro.Nombre,
                filtro.EdadMin,
                filtro.EdadMax
            },
            commandType: CommandType.StoredProcedure);
    }
}