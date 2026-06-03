using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MedicalApi.Models;
using MedicalApi.Interfaces;

namespace MedicalApi.Services;

public class DoctorService : IDoctorService
{
    private readonly IConfiguration _configuration;

    public DoctorService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    //obtener todos las Doctores
    public async Task<List<Doctor>> GetAll()
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        return (await connection.QueryAsync<Doctor>(
            "sp_Doctor_GetAll",
            commandType: CommandType.StoredProcedure)).ToList();
    }

    

    //obtener un doctor por el id
    public async Task<Doctor?> GetById(int doctorId)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        return await connection.QueryFirstOrDefaultAsync<Doctor>(
            "sp_Doctor_GetById",
            new { doctorId = doctorId },
            commandType: CommandType.StoredProcedure);
    }


    // Crear un nueva doctor
    public async Task Create(Doctor doctor)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        // Validar que no exista otro doctor con el mismo nombre
        var doctorExiste =
            await connection.QueryFirstOrDefaultAsync<Doctor>(
                "sp_Doctor_ExisteName_Create",
                new
                {
                    doctor.Nombre
                },
                commandType: CommandType.StoredProcedure);

        if (doctorExiste != null)
        {
            throw new InvalidOperationException(
                "Ya existe un doctor con este nombre");
        }


    // Crear doctor 
    try
    {
        await connection.ExecuteScalarAsync<int>(
            "sp_Doctor_Insert",
            new
            {
                doctor.Nombre,
                doctor.EspecialidadId
            },
            commandType: CommandType.StoredProcedure);
    }
    catch (SqlException ex)
    {
        // Construir mensaje detallado para ayudar al diagnóstico (no exponer demasiada info en producción)
        var msg = $"Error SQL al crear doctor: Number={ex.Number}; Message={ex.Message}";
        foreach (SqlError err in ex.Errors)
        {
            msg += $" | Proc={err.Procedure}; Line={err.LineNumber}; ErrMessage={err.Message}";
        }

        throw new InvalidOperationException(msg);
    }
    }




    //Actualizar un Doctor existente
    public async Task Update(int doctorId, Doctor doctor)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        // Validar que el nombre no exista
        var nombreDuplicado =
            await connection.QueryFirstOrDefaultAsync<Doctor>(
                "sp_Doctor_ExisteName",
                new
                {
                    DoctorId = doctorId,
                    doctor.Nombre
                },
                commandType: CommandType.StoredProcedure);

        if (nombreDuplicado != null)
        {
            throw new InvalidOperationException(
                "Ya existe un doctor con este nombre");
        }
        await connection.ExecuteAsync(
            "sp_Doctor_Update",
            new
            {
                DoctorId = doctorId,
                doctor.Nombre,
                doctor.EspecialidadId

            },
            commandType: CommandType.StoredProcedure);
    }

    //Eliminar un doctor por su id
    public async Task Delete(int doctorId)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        await connection.ExecuteAsync(
            "sp_Doctor_Delete",
            new
            {
                DoctorId = doctorId
            },
            commandType: CommandType.StoredProcedure);
    }

}