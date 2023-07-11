using System.ComponentModel.DataAnnotations;

namespace SecureAssetManager.Models
{
	public class Threat
	{
		[Key]
		public string Code { get; set; }

		[Required]
		[StringLength(100)]
		public string ThreatOrigin { get; set; }

		[Required]
		[StringLength(200)]
		public string ThreatDescription { get; set; }

		[Required]
		[Range(1, 5)]
		public int Degradation { get; set; }

		[Required]
		[Range(1, 3)]
		public int Probability { get; set; }
	}
}