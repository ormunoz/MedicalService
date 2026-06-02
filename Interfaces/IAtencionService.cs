using MedicalApi.Models;
using MedicalApi.DTOs;

namespace MedicalApi.Interfaces;

public interface IAtencionService
{
    Task<List<Atencion>> GetAll();
    Task<List<PromedioEspecialidadDto>> GetAverage();
    Task<Atencion?> GetById(int atencionId);
    Task Create(Atencion atencion);
    Task Update(int atencionId, Atencion atencion);
    Task Delete(int atencionId);

    Task<IEnumerable<Atencion>> Buscar(
        AtencionFiltroDto filtro);
}