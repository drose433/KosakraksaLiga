using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KosarkaskaLiga2019.Models
{
    [Table("Tim")]
    public partial class Tim
    {
        public Tim()
        {
            UtakmicaDomacini = new HashSet<Utakmica>();
            UtakmicaGosti = new HashSet<Utakmica>();
        }

        public int TimId { get; set; }
        [Required]
        [StringLength(30)]
        public string Naziv { get; set; }
        public byte[] Slika { get; set; }
        [Column("TipSLike")]
        [StringLength(30)]
        public string TipSlike { get; set; }
        public int? GradId { get; set; }
        public int? BrojBodova { get; set; }

        [ForeignKey("GradId")]
        [InverseProperty("Timovi")]
        public virtual Grad Grad { get; set; }
        [InverseProperty("Domacin")]
        public virtual ICollection<Utakmica> UtakmicaDomacini { get; set; }
        [InverseProperty("Gost")]
        public virtual ICollection<Utakmica> UtakmicaGosti { get; set; }
    }
}