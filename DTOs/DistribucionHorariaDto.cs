namespace MedicalApi.DTOs;

public class DistribucionHorariaDto
{
    public string RangoHora { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Porcentaje { get; set; }
}
