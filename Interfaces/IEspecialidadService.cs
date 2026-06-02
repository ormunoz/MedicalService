using MedicalApi.Models;

namespace MedicalApi.Interfaces;

public interface IEspecialidadService
{
    Task<List<Especialidad>> GetAll();
    Task Create(Especialidad especialidad);
    Task<Especialidad?> GetById(int id);
    Task Update(int id, Especialidad especialidad);
    Task Delete(int id);
}