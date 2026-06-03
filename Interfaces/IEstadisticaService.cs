using MedicalApi.DTOs;

namespace MedicalApi.Interfaces;

public interface IEstadisticaService
{
    Task<List<DistribucionHorariaDto>> GetDistribucionHoraria();
    Task<List<DoctorMasSolicitadoDto>> GetDoctorMasSolicitado();
    Task<List<EspecialidadMasSolicitadaDto>> GetEspecialidadMasSolicitada();
}
