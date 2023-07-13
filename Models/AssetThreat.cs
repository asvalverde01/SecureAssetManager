using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureAssetManager.Models
{
    public class AssetThreat
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Asset")]
        public int AssetId { get; set; }

        [ForeignKey("Threat")]
        public int ThreatId { get; set; }

        public virtual Asset Asset { get; set; }

        public virtual Threat Threat { get; set; }
    }
}
