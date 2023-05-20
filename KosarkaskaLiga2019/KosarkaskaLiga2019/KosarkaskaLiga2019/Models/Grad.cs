using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KosarkaskaLiga2019.Models
{
    [Table("Grad")]
    public partial class Grad
    {
        public Grad()
        {
            Timovi = new HashSet<Tim>();
        }

        public int GradId { get; set; }
        [Required]
        [StringLength(30)]
        public string NazivGrada { get; set; }

        [InverseProperty("Grad")]
        public virtual ICollection<Tim> Timovi { get; set; }
    }
}