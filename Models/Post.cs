using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace SecureAssetManager.Models
{
    public class Post
    {

        public int ID { get; set; }
        // Linked to identity user

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        // Initialize Date


        [Required]
        [
            StringLength(
                100,
                ErrorMessage = "Descripción no puede estar vacía",
                MinimumLength = 2)
        ]
        public string? Descripcion { get; set; }

        public string? ImagenUrl { get; set; }
        // Image type
        [Display(Name = "Subir una imagen ")]
        [NotMapped]
        public IFormFile? ImagenFile { get; set; }

    }
}
