namespace MedicalApi.Models;


public class Atencion
{
    public int AtencionId { get; set; }
    public int DoctorId { get; set; }
    public int PacienteId { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public DateTime FechaHoraFin { get; set; }
    public string Diagnostico { get; set; }

    public string? DoctorNombre { get; set; }
    public string? EspecialidadNombre { get; set; }
    public string? PacienteNombre { get; set; }
}