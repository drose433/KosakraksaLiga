using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KosarkaskaLiga2019.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using KosarkaskaLiga2019.Data;

namespace KosarkaskaLiga2019.Controllers
{
    public class HomeController : Controller
    {
        private readonly RoleManager<IdentityRole> rm;
        private readonly UserManager<ApplicationUser> um;

        public HomeController(RoleManager<IdentityRole> _rm, UserManager<ApplicationUser> _um)
        {
            rm = _rm;
            um = _um;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Kontakt()
        {
            return View();
        }
        private async Task<int> KreirajRolu(string rola)
        {
            bool rolaPostoji = await rm.RoleExistsAsync(rola);

            if (rolaPostoji)
            {
                return 0;
            }
            else
            {
                IdentityRole rolaAdmin = new IdentityRole(rola);
                var rezultat = await rm.CreateAsync(rolaAdmin);
                if (rezultat.Succeeded)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        public IActionResult VideoArhiva()
        {
            return View();
        }

        private async Task<ApplicationUser> KreirajAdministratora()
        {
            ApplicationUser admin = await um.FindByEmailAsync("admin@gmail.com");
            if (admin == null)
            {

                admin = new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    Ime = "Jovan",
                    Prezime = "Mitic"
                };
                string lozinka = "admin123";
                var rezultat = await um.CreateAsync(admin, lozinka);
                if (rezultat.Succeeded)
                {
                    return admin;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                //admin vec postoji
                return admin;
            }
        }
        public async Task<IActionResult> AdminSistema()
        {
            int rolaPostoji = await KreirajRolu("admin");
            ApplicationUser admin = await KreirajAdministratora();
            if (rolaPostoji == -1)
            {
                ViewBag.Poruka = "Greska pri kreiranju role";
                return View();
            }
            if (admin == null)
            {
                ViewBag.Poruka = "Greska pri kreiranju admina";
                return View();
            }
            bool rezultat1 = await um.IsInRoleAsync(admin, "admin");
            if (rezultat1)
            {
                ViewBag.Poruka = "Korisnik je vec u roli admin";
                return View();
            }

            var rezultat = await um.AddToRoleAsync(admin, "admin");
            if (rezultat.Succeeded)
            {
                ViewBag.Poruka = "Kreiran administrator sistema";
            }

            else
            {
                ViewBag.Poruka = "Greska pri dodavanju korisnika u rolu";
            }
            return View();
        }


        [HttpPost]
        public IActionResult Posalji(string ime, string prezime, string email, string poruka)
        {
            MailAddress admin = new MailAddress("miticjovan7@gmail.com");
            MailAddress posiljaoc = new MailAddress(email, ime + " " + prezime);
            MailMessage msg = new MailMessage();
            msg.To.Add(admin);
            msg.From = posiljaoc;

            msg.Subject = "Poruka sa web sajta";
            msg.IsBodyHtml = true;
            msg.Body = poruka;

            SmtpClient klijent = new SmtpClient("smtp.gmail.com");
            klijent.Credentials = new NetworkCredential("itageneracija2018@gmail.com", "link2019a");
            klijent.EnableSsl = true;
            klijent.Port = 587;

            try
            {
                klijent.Send(msg);
                return Redirect("/uspesno.html");
            }
            catch (System.Exception)
            {

                return Redirect("/greska.html");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
