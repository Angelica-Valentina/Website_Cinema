using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace FacemTemaLaBDextremDeBine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RezervariController : ControllerBase
    {
        private IConfiguration _configuration;
        public RezervariController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetRezervari")]
        public JsonResult GetRezervari()
        {
            string query = "SELECT * FROM dbo.Rezervari";
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
        [Route("AddRezervare")]
        public JsonResult AddRezervare ([FromForm] int id_utilizator, [FromForm] int id_program)
        {
            string query = "INSERT INTO dbo.Rezervari (id_utilizator, id_program) " +
                           "VALUES (@id_utilizator, @id_program)";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("Cinema_BDAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id_utilizator", id_utilizator);
                    myCommand.Parameters.AddWithValue("@id_program", id_program);

                    // Use ExecuteNonQuery for an INSERT operation
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Added successfully");
        }

        [HttpDelete]
        [Route("DeleteRezervari")]

        public JsonResult DeleteRezervari(int id)
        {
            string query = "DELETE FROM dbo.Rezervari WHERE id_rezervare = @id";
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


    }
}
