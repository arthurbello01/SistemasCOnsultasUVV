using System.ComponentModel.DataAnnotations;

namespace SistemaConsultasUVV.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        // Nunca armazenamos a senha em texto puro: aqui fica o hash gerado pelo IPasswordHasher.
        [Required]
        [StringLength(300)]
        public string SenhaHash { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
    }
}
