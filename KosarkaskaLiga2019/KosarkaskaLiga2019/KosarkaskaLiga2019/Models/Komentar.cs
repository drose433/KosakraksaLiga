using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KosarkaskaLiga2019.Models
{
    [Table("Komentar")]
    public partial class Komentar
    {
        public int Id { get; set; }
        [Required]
        [StringLength(450)]
        public string ClanId { get; set; }
        [Required]
        [Column("Komentar")]
        [StringLength(350)]
        public string Komentar1 { get; set; }
        [Required]
        [StringLength(50)]
        public string Autor { get; set; }
    }
}