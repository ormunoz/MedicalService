using MedicalApi.DTOs;
using MedicalApi.Models;

namespace MedicalApi.Interfaces;

public interface IPacienteService
{
    Task<IEnumerable<Paciente>> GetAll();
    Task<Paciente?> GetById(int id);
    Task Create(Paciente paciente);
    Task Update(int id, Paciente paciente);
    Task Delete(int id);

    Task<IEnumerable<Paciente>> Buscar(
        PacienteFiltroDto filtro);
}