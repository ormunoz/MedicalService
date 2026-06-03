namespace MedicalApi.DTOs;

public class EspecialidadMasSolicitadaDto
{
    public int EspecialidadId { get; set; }
    public string EspecialidadNombre { get; set; } = string.Empty;
    public int TotalAtenciones { get; set; }
    public decimal Porcentaje { get; set; }
}
