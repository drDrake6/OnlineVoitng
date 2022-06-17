using VotingApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class AccountController : Controller
    {
        Context db;
        public AccountController(Context context)
        {
            db = context;
        }
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(string login, string passwd)
        {
            AccountView account = new AccountView(login, passwd);
            db.Users.Add(new User(account));         
            db.SaveChanges();
            await Authenticate(login); // аутентификация
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Autorisation()
        {
            return View(new AccountView());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Autorisation(string login, string passwd)
        {
            AccountView account = new AccountView(login, passwd);
            AccountEnum pass = LogIn(account);

            if (pass == AccountEnum.NONE)
            {
                account.msg = "Wrong login or password";
                return View("Autorisation", account);
            }
            else
            {
                await Authenticate(login); // аутентификация
                return RedirectToAction("Index", "Home");
            }
        }

        public AccountEnum LogIn(AccountView account)
        {
            User bdel = db.Users.FirstOrDefault(el => el.Login == account.login);
            if (bdel == null || bdel.Password != account.passwd)
            {
                Admin bdad = db.Admins.FirstOrDefault(el => el.Login == account.login);
                if (bdad == null || bdad.Password != account.passwd)
                    return AccountEnum.NONE;
                else
                    return AccountEnum.ADMIN;
            }
            else
                return AccountEnum.USER;
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
