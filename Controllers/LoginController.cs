using e_TillRemote.Models;
using e_TillRemote.Provider;
using e_TillRemote.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace e_TillRemote.Controllers
{
    public class LoginController : Controller
    {

        SqlConnection primarycon = new SqlConnection(DbConnections.ConnectionString);
        LoginRepository _loginrepository = new LoginRepository();
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(PasswordModel pwd)
        {
            var passwordModel = _loginrepository.getUserLogin(pwd);
            if (passwordModel != null)
            {
                if (passwordModel.WEB_YN == "Y")
                {
                    var claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.Name, passwordModel.P_USER));
                    claims.Add(new Claim(ClaimTypes.Sid, passwordModel.P_NO.ToString()));


                    var identity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.
            AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    var props = new AuthenticationProperties();
                    props.IsPersistent = false;// model.RememberMe;

                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.
            AuthenticationScheme,
                        principal, props).Wait();
                    return RedirectToAction("Dashboard");
                }
            }

            return View();
        }



        [Authorize]
        public ActionResult Dashboard()
        {
            ViewBag.UserName = HttpContext.User.Identity.Name;
            ViewBag.IPAddress = new SelectList(GetIpAddress(), "IPAddress", "Location");
            ViewBag.Pass = "initial";
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Dashboard(IPAddressModel Ipdet)
        {
            ViewBag.IPAddress = new SelectList(GetIpAddress(), "IPAddress", "Location");

            //Get Ipaddressdet will have all Ip 
            var Ipaddressdet = _loginrepository.GetAllIpAddress(Ipdet);

            //Initializing Remote Connection Call only 1st time
            var IsConnected = LoginRepository.GetRemoteConnected(Ipaddressdet);

            if ((IsConnected != null))
            {
                var resultSection1 = _loginrepository.GetSection1();

                SessionLogModel sessionLog = new SessionLogModel();
                sessionLog.IpAddressTo = Ipdet.IPAddress;
                sessionLog.IpAddressFrom = Ipdet.IPAddress;
                sessionLog.UserId = Convert.ToString(GetPno());
                sessionLog.UserName = HttpContext.User.Identity.Name;
                sessionLog.Location = "Imtiaz House";
                sessionLog.DeviceId = "3xdr24567";

                ViewBag.Pass = "Show";
                var SessionLogIn = _loginrepository.SaveSessionLogIn(sessionLog);

            }
            else
            {
                ViewBag.Pass = "Error";
                ViewBag.ConnectingUser = Ipdet.UserName;
            }
            return View();
        }

        public IActionResult SignOut()
        {

            HttpContext.SignOutAsync(
             CookieAuthenticationDefaults.AuthenticationScheme);
            var SessionLogOut = _loginrepository.SaveSessionLogOut(GetPno());

            return RedirectToAction("Login");
        }

       
        public List<IPAddressModel> GetIpAddress()
        {
            int pno = 0;
            foreach (var item in HttpContext.User.Claims)
            {
                if (item.Type.Substring(54) == "sid")
                {
                    pno = Convert.ToInt32(item.Value);
                }
            }

            var Ipaddress = _loginrepository.GetIpAddressByPno(pno);

            return Ipaddress;
        }





        public int GetPno()
        {
            int pno = 0;
            foreach (var item in HttpContext.User.Claims)
            {
                if (item.Type.Substring(54) == "sid")
                {
                    pno = Convert.ToInt32(item.Value);
                }
            }

            return pno;
        }
    }
}
