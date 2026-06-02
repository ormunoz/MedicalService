namespace MedicalApi.Models;

public class Paciente
{
    public int PacienteId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string RUT { get; set; } = string.Empty;

    public DateTime FechaNacimiento { get; set; }
}