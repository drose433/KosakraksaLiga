using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KosarkaskaLiga2019.Data
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        [StringLength(30)]
        public string Ime { get; set; }

        [Required]
        [StringLength(30)]
        public string Prezime { get; set; }
    }
}
