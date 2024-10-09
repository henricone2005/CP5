using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PacientesePlanos.Model;

public class Paciente
{
      [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // ID auto incremento

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(11)]
        public string CPF { get; set; }

        [Required]
        [StringLength(15)]
        public string Telefone { get; set; }
    
        public ICollection<Plano> Planos { get; set; } = new List<Plano>();
}
