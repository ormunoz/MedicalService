namespace MedicalApi.DTOs;

public class DoctorMasSolicitadoDto
{
    public int DoctorId { get; set; }
    public string DoctorNombre { get; set; } = string.Empty;
    public string EspecialidadNombre { get; set; } = string.Empty;
    public int TotalAtenciones { get; set; }
    public decimal Porcentaje { get; set; }
}
