using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {            
            return View();
        }
        [HttpPost]
        public int Save(UserViewModel vm)
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
                    var user = new User() { UserName = vm.UserName };
                    _context.Users.Add(user);
                    _context.PasswordHistorys.Add(new PasswordHistory() { User = user, Password = vm.Password, DateCreated = DateTime.Now });
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
