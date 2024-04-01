using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace FacemTemaLaBDextremDeBine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriiFilmeController : ControllerBase
    {
        private IConfiguration _configuration;
        public CategoriiFilmeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetCategorii_Filme")]
        public JsonResult GetCategorii_Filme()
        {
            string query = "SELECT * FROM dbo.Categorii_Filme";
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
        [Route("AddCategorii_Filme")]
        public JsonResult AddCategorii_Filme([FromForm] string nume_categorie)
        {
            string query = "INSERT INTO dbo.Categorii_Filme (nume_categorie) " +
                           "VALUES (@nume_categorie)";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("Cinema_BDAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nume_categorie", nume_categorie);

                    // Use ExecuteNonQuery for an INSERT operation
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Added successfully");
        }



        [HttpDelete]
        [Route("DeleteCategorii_Filme")]

        public JsonResult DeleteCategorii_Filme(int id)
        {
            string query = "DELETE FROM dbo.Categorii_Filme WHERE id_categorie = @id";
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
        [Route("GetNrFilmeCategorie")]
        public JsonResult GetNrFilmeCategorie()
        {
            string query = @"SELECT CF.*, COUNT(F.id_film) AS NumarTotalFilme
                    FROM Categorii_Filme CF
                    LEFT JOIN Filme F ON CF.id_categorie = F.id_categorie
                    GROUP BY CF.id_categorie, CF.nume_categorie;";

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

