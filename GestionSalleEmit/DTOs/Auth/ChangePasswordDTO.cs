using System.ComponentModel.DataAnnotations;

public class ChangePasswordDTO
{
    public int IdUtilisateur { get; set; }

    [Required]
    public string AncienMotDePasse { get; set; }

    [Required]
    [MinLength(8)]
    public string NouveauMotDePasse { get; set; }
}