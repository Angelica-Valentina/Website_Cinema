using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace FacemTemaLaBDextremDeBine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizatoriController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UtilizatoriController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetUtilizatori")]

        public JsonResult GetUtilizatori()
        {
            string query = "SELECT * FROM dbo.Utilizatori";
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
        [Route("Login")]
        public IActionResult Login([FromForm] string email, [FromForm] string parola)
        {
            string query = "SELECT id_utilizator, nume, prenume FROM dbo.Utilizatori WHERE email = @email AND parola = @parola";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("Cinema_BDAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@email", email);
                    myCommand.Parameters.AddWithValue("@parola", parola);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            // Check if login was successful
            if (table.Rows.Count == 1)
            {
                // Generate a unique token (you can use a library for this)
                string token = Guid.NewGuid().ToString();

                // Update the user's Token in the database
                int userId = Convert.ToInt32(table.Rows[0]["id_utilizator"]);
                UpdateUserToken(userId, token);

                // Return the token and user information with a success status code (200 OK)
                 return Ok(new { Token = token, UserID = userId, Nume = table.Rows[0]["nume"], Prenume = table.Rows[0]["prenume"] });
                //return Ok(new { Token = token });
            }
            else
            {
                // Return an unauthorized status code (401 Unauthorized) with an error message
                return Unauthorized("Login failed. Invalid username or password.");
            }
        }

        // Helper method to update the user's Token in the database
        private void UpdateUserToken(int userId, string token)
        {
            string updateQuery = "UPDATE dbo.Utilizatori SET Token = @Token WHERE id_utilizator = @UserID";
            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("Cinema_BDAppCon")))
            {
                myCon.Open();
                using (SqlCommand updateCommand = new SqlCommand(updateQuery, myCon))
                {
                    updateCommand.Parameters.AddWithValue("@UserID", userId);
                    updateCommand.Parameters.AddWithValue("@Token", token);
                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        [HttpPost]
        [Route("AddUtilizator")]
        public JsonResult AddUtilizator (
        [FromForm] string nume,
        [FromForm] string prenume,
        [FromForm] string data_nastere,
        [FromForm] string telefon,
        [FromForm] string email,
        [FromForm] string parola
)
        {
            string query = "INSERT INTO dbo.Utilizatori (nume, prenume, data_nastere, telefon, email, parola) " +
                           "VALUES (@nume, @prenume, @data_nastere, @telefon, @email, @parola)";

            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("Cinema_BDAppCon")))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nume", nume);
                    myCommand.Parameters.AddWithValue("@prenume", prenume);
                    myCommand.Parameters.AddWithValue("@data_nastere", data_nastere);
                    myCommand.Parameters.AddWithValue("@telefon", telefon);
                    myCommand.Parameters.AddWithValue("@email", email);
                    myCommand.Parameters.AddWithValue("@parola", parola);
            
                    // Use ExecuteNonQuery for an INSERT operation
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Added successfully");
        }

        [HttpGet]
        [Route("IsAdmin/{id}")]
        public JsonResult IsAdmin(int id)
        {
            string query = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM Administratori WHERE id_utilizator = {id}) THEN 1 ELSE 0 END AS IsAdmin;";

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

        [HttpGet]
        [Route("GetTopUtilizatori")]
        public JsonResult GetTopUtilizatori()
        {
            string query = $"SELECT U.* " +
                           $"FROM Utilizatori U " +
                           $"WHERE U.id_utilizator IN (   " +
                           $"SELECT R1.id_utilizator    " +
                           $"FROM Rezervari R1    " +
                           $"GROUP BY R1.id_utilizator    " +
                           $"HAVING COUNT(R1.id_rezervare) >= 3) AND U.id_utilizator NOT IN (" +
                           $"SELECT DISTINCT R2.id_utilizator    " +
                           $"FROM Rezervari R2   " +
                           $"JOIN Program P ON R2.id_program = P.id_program    " +
                           $"JOIN Filme F ON P.id_film = F.id_film    WHERE F.durata < 120);";

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

        [HttpGet]
        [Route("GetVIPUtilizatori")]
        public JsonResult GetVIPUtilizatori()
        {
            string query = @"SELECT U.*
                    FROM Utilizatori U
                    JOIN Rezervari R ON U.id_utilizator = R.id_utilizator
                    JOIN Program P ON R.id_program = P.id_program
                    JOIN Sali S ON P.id_sala = S.id_sala
                    WHERE S.tip_sala = 'VIP';";

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
