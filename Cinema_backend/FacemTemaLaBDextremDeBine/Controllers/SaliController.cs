using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace FacemTemaLaBDextremDeBine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaliController : ControllerBase
    {
        private IConfiguration _configuration;
        public SaliController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetSali")]
        public JsonResult GetSali()
        {
            string query = "SELECT * FROM dbo.Sali";
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
        [Route("AddSali")]
        public JsonResult AddSali([FromForm] int numar_randuri, [FromForm] int numar_locuri, [FromForm] string tip_sala)
        {
            string query = "INSERT INTO dbo.Sali (numar_randuri, numar_locuri, tip_sala) " +
                           "VALUES (@numar_randuri, @numar_locuri, @tip_sala)";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("Cinema_BDAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@numar_randuri", numar_randuri);
                    myCommand.Parameters.AddWithValue("@numar_locuri", numar_locuri);
                    myCommand.Parameters.AddWithValue("@tip_sala", tip_sala);

                    // Use ExecuteNonQuery for an INSERT operation
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Added successfully");
        }



        [HttpDelete]
        [Route("DeleteSali")]

        public JsonResult DeleteProgram(int id)
        {
            string query = "DELETE FROM dbo.Sali WHERE id_sala = @id";
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
        [Route("GetSali100")]
        public JsonResult GetSali100()
        {
            string query = @"
                SELECT S.*
                FROM Sali S
                JOIN Program P ON S.id_sala = P.id_sala
                WHERE P.data_si_ora BETWEEN DATEADD(DAY, -100, GETDATE()) AND GETDATE();";

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

