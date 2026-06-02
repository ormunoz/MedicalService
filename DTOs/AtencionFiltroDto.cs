namespace MedicalApi.DTOs;

public class AtencionFiltroDto
{
    public int? DoctorId { get; set; }
    public int? PacienteId { get; set; }
    public int? EspecialidadId { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
}