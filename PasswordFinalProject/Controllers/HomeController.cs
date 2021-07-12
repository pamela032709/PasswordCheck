using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PasswordFinalProject.Models;

namespace PasswordFinalProject.Controllers
{
    public class HomeController : Controller
    {       
        private readonly UserDBContext _context;      

        public HomeController( UserDBContext context)
        {
            _context = context;
        }

        public IActionResult index()
        {            
            return View();
        }
       
        [HttpPost]
        public int Register(UserViewModel vm)
        {
            if (!_context.Users.Any(p => p.UserName == vm.UserName)) {
                try
                {
                    var history = _context.PasswordHistorys.OrderBy(p => p.DateCreated).ToList();
                    if (history.Count > 5)
                    {
                        /*if password is accepted=>
                         * new passwords should be updated in the pass history table 
                         * and replaces the fifth oldest password in the passord history table
                         * with todays date */

                        _context.PasswordHistorys.Update(history.First());
                    }
                    var user = new User() { UserName = vm.UserName ,Password = Sha256EncriptPassword( vm.Password)};
                    _context.Users.Add(user);
                    _context.PasswordHistorys.Add(new PasswordHistory() { User = user, Password = Sha256EncriptPassword(vm.Password), DateCreated = DateTime.Now });
                    _context.SaveChanges();
                    return 1;
                }
                catch (Exception e)
                {
                    return -1;
                }
               
            }
            return 0;
        }
        [HttpGet]
        public string Login(UserViewModel vm)
        {
            //var u = new User() { UserName = vm.UserName, Password = Sha256EncriptPassword(vm.Password) };
            //u.PasswordHistorys = new List<PasswordHistory>();
            //u.PasswordHistorys.Add(new PasswordHistory()
            //{
            //    DateCreated = DateTime.Now,
            //    Password = u.Password
            //});
            //_context.Users.Add(u);
            //_context.SaveChanges();


            //var u = _context.Users.Include(p=> p.PasswordHistorys).FirstOrDefault(p => p.Id == 1009);
            
            //u.PasswordHistorys.Add(new PasswordHistory()
            //{
            //    DateCreated = DateTime.Now,
            //    Password = Sha256EncriptPassword("ISEC680_ss@2")
            //});
            //u.PasswordHistorys.Add(new PasswordHistory()
            //{
            //    DateCreated = DateTime.Now,
            //    Password = Sha256EncriptPassword("ISEC680_ss@3")
            //});
            //u.PasswordHistorys.Add(new PasswordHistory()
            //{
            //    DateCreated = DateTime.Now,
            //    Password = Sha256EncriptPassword("ISEC680_ss@4")
            //});
            //u.PasswordHistorys.Add(new PasswordHistory()
            //{
            //    DateCreated = DateTime.Now,
            //    Password = Sha256EncriptPassword("ISEC680_ss@5")
            //});
            //_context.Users.Update(u);
            //_context.SaveChanges();

            try
            {
                        var user = _context.Users.FirstOrDefault(p => p.UserName == vm.UserName);
                        if (user == null)
                        {                           
                           throw new Exception("The user not exist");
                        }
                        if (Sha256EncriptPassword(vm.Password ) != user.Password)
                        {
                            throw new Exception("The old password is incorrect");
                        }
                

                }
                catch (Exception e)
                {
                    return e.Message;
                }

            return "1";
        }
        private string Sha256EncriptPassword(string password) {
            // Get the bytes of the string
            
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);           

            return Convert.ToBase64String(passwordBytes);
        } 
            [HttpPost]
        public string ChangePassword(UserViewModel vm)
        {
           
                try
                {
                    var user = _context.Users.Include(p => p.PasswordHistorys).FirstOrDefault(p => p.UserName == vm.UserName);
                    var history = _context.PasswordHistorys.Where(p => p.User.Id == user.Id).OrderByDescending(p => p.DateCreated).Take(1).FirstOrDefault();

                if (user == null)
                    {
                        throw new Exception("The user not exist");
                    }
                //(Sha256EncriptPassword(vm.Password ) != user.Password)
                var oldie = Sha256EncriptPassword(vm.OldPassword);
                if (!_context.PasswordHistorys.Any(p => p.Password == oldie) )
                    {
                     throw new Exception("The old password is incorrect");
                    }
                if (user.PasswordHistorys.Any(p => p.Password == Sha256EncriptPassword(oldie)))
                    throw new Exception("You use this password the last 5 times. Please see the console log to identify in what position it was repeting ");


                if (history != null) {
                    history.Password = Sha256EncriptPassword(vm.Newpassword);
                    history.DateCreated = DateTime.Now;
                    user.Password = Sha256EncriptPassword(vm.Newpassword);

                    _context.PasswordHistorys.Update(history);
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    return "Password Changed";
                }
                
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            return "Something is wrong, contact with the administrator.";
                       
        }
        public IActionResult ChangePassword()
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
