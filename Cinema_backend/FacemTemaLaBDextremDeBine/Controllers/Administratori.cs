using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FacemTemaLaBDextremDeBine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratoriController : ControllerBase
    {
        private IConfiguration _configuration;
        public AdministratoriController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAdministratori")]
        public JsonResult GetAdministratori()
        {
            string query = "SELECT * FROM dbo.Administratori";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("Cinema_BDAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        [Route("AddAdministratori")]
        public JsonResult AddAdministratori([FromForm] int id_utilizator, [FromForm] string functie)
        {
            string query = "INSERT INTO dbo.Administratori (id_utilizator, functie) " +
                           "VALUES (@id_utilizator, @functie)";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("Cinema_BDAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id_utilizator", id_utilizator);
                    myCommand.Parameters.AddWithValue("@functie", functie);

                    // Use ExecuteNonQuery for an INSERT operation
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Added successfully");
        }



        [HttpDelete]
        [Route("DeleteAdministratori")]

        public JsonResult DeleteAdministratori(int id)
        {
            string query = "DELETE FROM dbo.Administratori WHERE id_administrator = @id";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("Cinema_BDAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted successfully");
        }

        [HttpGet]
        [Route("GetAdministratoriUtilizatori")]
        public JsonResult GetAdministratoriUtilizatori()
        {
            string query = @"SELECT U.*, A.functie AS FunctieAdministrator
                    FROM Utilizatori U
                    JOIN Administratori A ON U.id_utilizator = A.id_utilizator
                    WHERE U.id_utilizator IN (SELECT id_utilizator FROM Utilizatori);";

            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("Cinema_BDAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }

                myCon.Close();
            }

            return new JsonResult(table);
        }

    }
}

