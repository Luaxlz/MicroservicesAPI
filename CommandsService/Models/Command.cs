using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommandsService.Models
{
    public class Command
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // ✅ Maiúsculo para seguir convenção

        [Required]
        public required string HowTo { get; set; }

        [Required]
        public required string CommandLine { get; set; }

        [Required]
        public int PlatformId { get; set; }

        [Required]
        public Platform Platform { get; set; } = null!;
    }
}