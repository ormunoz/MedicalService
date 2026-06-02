namespace MedicalApi.Models;

public class Doctor
{
    public int DoctorId { get; set; }

    public required string Nombre { get; set; }

    public int EspecialidadId { get; set; }

    public string? EspecialidadNombre { get; set; }
}