using System.ComponentModel.DataAnnotations;

namespace SecureAssetManager.Models
{
    public class Threat
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El origen de la amenaza es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Origen de la Amenaza")]
        public string ThreatOrigin { get; set; }

        [Required(ErrorMessage = "La descripción de la amenaza es obligatoria.")]
        [StringLength(200)]
        [Display(Name = "Descripción de la Amenaza")]
        public string ThreatDescription { get; set; }

        [Required(ErrorMessage = "La degradación es obligatoria.")]
        [Range(1, 3, ErrorMessage = "La degradación debe estar entre 1 y 3.")]
        [Display(Name = "Degradación")]
        public int Degradation { get; set; }

        [Required(ErrorMessage = "La probabilidad es obligatoria.")]
        [Range(1, 3, ErrorMessage = "La probabilidad debe estar entre 1 y 3.")]
        [Display(Name = "Probabilidad")]
        public int Probability { get; set; }
    }
}
