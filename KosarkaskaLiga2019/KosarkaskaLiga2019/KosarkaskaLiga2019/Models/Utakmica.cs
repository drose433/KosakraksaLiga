using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KosarkaskaLiga2019.Models
{
    [Table("Utakmica")]
    public partial class Utakmica
    {
        public int UtakmicaId { get; set; }
        public int DomacinId { get; set; }
        public int GostId { get; set; }
        public int Kolo { get; set; }
        [StringLength(7)]
        public string Rezultat { get; set; }

        [ForeignKey("DomacinId")]
        [InverseProperty("UtakmicaDomacini")]
        public virtual Tim Domacin { get; set; }
        [ForeignKey("GostId")]
        [InverseProperty("UtakmicaGosti")]
        public virtual Tim Gost { get; set; }
    }
}