using System.ComponentModel.DataAnnotations;

namespace SecureAssetManager.Models
{
    public class Risk
    {
        [Key]
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Display(Name = "CID")]
        public double CID { get; set; }

        [Display(Name = "Nivel de Amenaza")]
        public int ThreatLevel { get; set; }

        [Display(Name = "Nivel de Vulnerabilidad")]
        public int VulnerabilityLevel { get; set; }

        [Display(Name = "Nivel de Riesgo")]
        public double RiskLevel { get; set; }

        [Display(Name = "Resultado")]
        public string Result { get; set; }

        [StringLength(100, ErrorMessage = "El control existente debe tener máximo 100 caracteres.")]
        [Display(Name = "Control Existente")]
        public string ExistingControl { get; set; }
    }
}
