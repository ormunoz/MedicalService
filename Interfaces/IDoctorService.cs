using MedicalApi.Models;

namespace MedicalApi.Interfaces;

public interface IDoctorService
{
    Task<List<Doctor>> GetAll();
    Task Create(Doctor doctor);
    Task<Doctor?> GetById(int doctorId);
    Task Update(int doctorId, Doctor doctor);
    Task Delete(int doctorId);
}