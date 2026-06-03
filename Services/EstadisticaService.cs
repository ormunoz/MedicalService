using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MedicalApi.Interfaces;
using MedicalApi.DTOs;

namespace MedicalApi.Services;

public class EstadisticaService : IEstadisticaService
{
    private readonly IConfiguration _configuration;

    public EstadisticaService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<DistribucionHorariaDto>> GetDistribucionHoraria()
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        var result = await connection.QueryAsync<DistribucionHorariaDto>(
            "sp_Estadistica_DistribucionHoraria",
            commandType: CommandType.StoredProcedure
        );

        return result.ToList();
    }

    public async Task<List<DoctorMasSolicitadoDto>> GetDoctorMasSolicitado()
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        var result = await connection.QueryAsync<DoctorMasSolicitadoDto>(
            "sp_Estadistica_DoctorMasSolicitado",
            commandType: CommandType.StoredProcedure
        );

        return result.ToList();
    }

    public async Task<List<EspecialidadMasSolicitadaDto>> GetEspecialidadMasSolicitada()
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

        var result = await connection.QueryAsync<EspecialidadMasSolicitadaDto>(
            "sp_Estadistica_EspecialidadMasSolicitada",
            commandType: CommandType.StoredProcedure
        );

        return result.ToList();
    }
}
