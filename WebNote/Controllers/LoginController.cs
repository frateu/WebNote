using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNote.Models;
using System.Data.SqlClient;
using System.Runtime.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace WebNote.Controllers
{
    public class LoginController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        private IMemoryCache memoryCache;

        public LoginController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Login()
        {
            memoryCache.Remove("IdUser");
            memoryCache.Remove("Username");
            return View();
        }
        void connectionString()
        {
            con.ConnectionString = "data source=DESKTOP-VC6IM0N; database=WebNote; integrated security = SSPI";
        }
        [HttpPost]
        public ActionResult Verify(LoginModel log)
        {
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT * FROM Usuarios WHERE username='" + log.Username + "' AND password='" + log.Password + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                Object[] values = new Object[dr.FieldCount];
                int fieldCount = dr.GetValues(values);

                memoryCache.Set("IdUser", values[0]);
                memoryCache.Set("Username", values[1]);
                
                con.Close();
                return Redirect("/Dashboard/Index");
            }
            else
            {
                con.Close();
                return View();//não está com nada implementado
            }                       
        }
    }
}
