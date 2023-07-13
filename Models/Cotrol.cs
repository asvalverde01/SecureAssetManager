using System.ComponentModel.DataAnnotations;

namespace SecureAssetManager.Models
{
    public class Control
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "El tipo de control es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo de control debe tener máximo 50 caracteres.")]
        [Display(Name = "Tipo de Control")]
        public string TipoControl { get; set; }

        [Range(0, 100, ErrorMessage = "La efectividad del control debe estar entre 0 y 100.")]
        [Display(Name = "Efectividad")]
        public int Efectividad { get; set; }

        [Display(Name = "Brecha de Seguridad Identificada")]
        public string BrechaSeguridadIdentificada { get; set; }

        [Display(Name = "Implementación")]
        public string Implementacion { get; set; }

        [Display(Name = "Monitoreo")]
        public string Monitoreo { get; set; }
    }
}
