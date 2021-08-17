using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebNote.Models;

namespace WebNote.Controllers
{
    public class DashboardController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        private IMemoryCache memoryCache;
        void connectionString()
        {
            con.ConnectionString = "data source=DESKTOP-VC6IM0N; database=WebNote; integrated security = SSPI";
        }
        public DashboardController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            ViewBag.Username = memoryCache.Get("Username");
            return View();
        }
        public IActionResult Adicionar()
        {
            ViewBag.IdUser = memoryCache.Get("IdUser");
            ViewBag.Username = memoryCache.Get("Username");
            return View();
        }
        [HttpPost]
        public ActionResult AdicionarNota(NotaModel note)
        {
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "INSERT INTO Notas (idUser, nota) VALUES ('" + Convert.ToInt32(memoryCache.Get("IdUser")) + "', '" + note.Nota + "')";
            dr = com.ExecuteReader();

            con.Close();
            return Redirect("/Dashboard/Index");
        }
        public IActionResult Visualizar()
        {
            ViewBag.IdUser = memoryCache.Get("IdUser");
            ViewBag.Username = memoryCache.Get("Username");

            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT nota FROM Notas WHERE idUser='" + Convert.ToInt32(memoryCache.Get("IdUser")) + "'";
            dr = com.ExecuteReader();

            ArrayList arrayNotas = new ArrayList();

            while (dr.Read())
            {
                arrayNotas.Add(dr["nota"].ToString());
            }

            ViewBag.Notas = arrayNotas;
            con.Close();
            return View();
        }
        public IActionResult Sair()
        {
            return Redirect("/");
        }
    }
}
