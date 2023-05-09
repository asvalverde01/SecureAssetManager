using System.ComponentModel.DataAnnotations;

namespace SecureAssetManager.Models
{
    public class Asset
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "El código de activo es obligatorio.")]
        [StringLength(10, ErrorMessage = "El código de activo debe tener máximo 10 caracteres.")]
        [Display(Name = "Código Activo")]
        public string CodigoActivo { get; set; }

        [Required(ErrorMessage = "El nombre del activo es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre del activo debe tener máximo 50 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El responsable del activo es obligatorio.")]
        [StringLength(50, ErrorMessage = "El responsable del activo debe tener máximo 50 caracteres.")]
        [Display(Name = "Responsable")]
        public string Responsable { get; set; }

        [Required(ErrorMessage = "La ubicación del activo es obligatoria.")]
        [StringLength(20, ErrorMessage = "La ubicación del activo debe tener máximo 20 caracteres.")]
        [Display(Name = "Ubicación")]
        public string Ubicacion { get; set; }

        [StringLength(250, ErrorMessage = "La descripción del activo debe tener máximo 250 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El tipo de activo es obligatorio.")]
        [StringLength(20, ErrorMessage = "El tipo de activo debe tener máximo 20 caracteres.")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "La categoría del activo es obligatoria.")]
        [StringLength(20, ErrorMessage = "La categoría del activo debe tener máximo 20 caracteres.")]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "La clasificación del activo es obligatoria.")]
        [StringLength(20, ErrorMessage = "La clasificación del activo debe tener máximo 20 caracteres.")]
        [Display(Name = "Clasificación")]
        public string Clasificacion { get; set; }

        [Required(ErrorMessage = "La etiqueta principal del activo es obligatoria.")]
        [StringLength(50, ErrorMessage = "La etiqueta principal del activo debe tener máximo 50 caracteres.")]
        [Display(Name = "Etiqueta Principal")]
        public string EtiquetaPrincipal { get; set; }

        [Required(ErrorMessage = "El tipo de valoración del activo es obligatorio.")]
        [StringLength(20, ErrorMessage = "El tipo de valoración del activo debe tener máximo 20 caracteres.")]
        [Display(Name = "Tipo de Valoración")]
        public string TipoValoracion { get; set; }

        [Required(ErrorMessage = "La valoración del activo es obligatoria.")]
        [Range(0, 4, ErrorMessage = "La valoración del activo debe estar entre 0 y 4.")]
        [Display(Name = "Valoración")]
        public int Valoracion { get; set; }
    }
}
