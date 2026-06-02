namespace MedicalApi.DTOs;

public class PacienteFiltroDto
{
    public string? Rut { get; set; }
    public string? Nombre { get; set; }
    public int? EdadMin { get; set; }
    public int? EdadMax { get; set; }
}