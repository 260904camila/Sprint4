using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiConnect.Models
{
    [Table("INTERACOES_AI")]
    public class Interacao
    {
        [Key]
        [Column("id_interacao")]
        public int Id { get; set; }

        [Required]
        [Column("tipo_interacao")]
        public string TipoInteracao { get; set; }

        [Required]
        [Column("data_interacao")]
        public DateTime DataInteracao { get; set; }

        [Column("detalhes_interacao")]
        public string Detalhes { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }
    }
}