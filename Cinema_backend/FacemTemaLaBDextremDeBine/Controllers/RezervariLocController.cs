using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace FacemTemaLaBDextremDeBine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RezervariLocController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public RezervariLocController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetRezervariLoc")]

        public JsonResult GetRezervariLoc()
        {
            string query = "SELECT * FROM dbo.RezervariLoc";
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
        [Route("AddRezervariLoc")]
        public JsonResult AddRezervariLoc ([FromForm] int numar_rand, [FromForm] int numar_loc)
         
        {
            string query = "INSERT INTO dbo.RezervariLoc (numar_rand, numar_loc)" +
                "VALUES (@numar_rand, @numar_loc)";
            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("Cinema_BDAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@numar_rand", numar_rand);
                    myCommand.Parameters.AddWithValue("@numar_loc", numar_loc);
                  
                    // Use ExecuteNonQuery for an INSERT operation
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Added successfully");
        }


        [HttpDelete]
        [Route("DeleteRezervariLoc")]

        public JsonResult DeleteRezervariLoc(int id)
        {
            string query = "DELETE FROM dbo.RezervariLoc WHERE id_rezervareloc = @id";
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
