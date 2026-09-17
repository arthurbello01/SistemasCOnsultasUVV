using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaConsultasUVV.Models
{
    public class Consulta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A especialidade é obrigatória.")]
        [StringLength(100)]
        public string Especialidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data e hora são obrigatórias.")]
        [Display(Name = "Data e Hora")]
        [DataType(DataType.DateTime)]
        public DateTime DataHora { get; set; }

        [StringLength(500)]
        public string? Descricao { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }
    }
}
