using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace FacemTemaLaBDextremDeBine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramController : ControllerBase
    {
        private IConfiguration _configuration;
        public ProgramController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetProgram")]
        public JsonResult GetProgram()
        {
            string query = "SELECT * FROM dbo.Program";
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
        [Route("AddProgram")]
        public JsonResult AddProgram([FromForm] int id_film, [FromForm] int id_sala, [FromForm] DateTime data_si_ora)
        {
            string query = "INSERT INTO dbo.Program (id_film, id_sala, data_si_ora) " +
                           "VALUES (@id_film, @id_sala, @data_si_ora)";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("Cinema_BDAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id_film", id_film);
                    myCommand.Parameters.AddWithValue("@id_sala", id_sala);
                    myCommand.Parameters.AddWithValue("@data_si_ora", data_si_ora);

                    // Use ExecuteNonQuery for an INSERT operation
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Added successfully");
        }



        [HttpDelete]
        [Route("DeleteProgram")]

        public JsonResult DeleteProgram(int id)
        {
            string query = "DELETE FROM dbo.Program WHERE id_program = @id";
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
        [Route("GetProgramByFilm/{id}")]
        public JsonResult GetProgramByFilm(int id)
        {
            string query = $"SELECT P.data_si_ora, P.id_sala " +
                $"FROM Program P INNER JOIN Filme F ON P.id_film = F.id_film " +
                $"WHERE F.id_film = {id} " +
                $"ORDER BY P.data_si_ora";
            ;

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
    }
}

