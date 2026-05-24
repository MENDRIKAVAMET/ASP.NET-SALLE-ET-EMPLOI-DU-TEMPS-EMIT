namespace GestionSalleEmit.Filters;

public class SalleFilter
{
    public string? Type { get; set; }
    public int? Capacite { get; set; }

    public DateTime? Jour { get; set; }
    public TimeSpan? HeureDebut { get; set; }
    public TimeSpan? HeureFin { get; set; }

    public string? Semestre { get; set; }
    public int? IdNiveau { get; set; }
    public int? IdMatiere { get; set; }
    public int? IdEnseignant { get; set; }
    public int? IdEtudiant { get; set; }
}