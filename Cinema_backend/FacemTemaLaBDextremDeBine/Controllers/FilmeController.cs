using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace FacemTemaLaBDextremDeBine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public FilmeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetFilme")]

        public JsonResult GetFilme()
        {
            string query = "SELECT F.*, CF.nume_categorie " +
                "FROM Filme F " +
                "INNER JOIN Categorii_Filme CF ON F.id_categorie = CF.id_categorie";
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
        [Route("GetFilmById/{id}")]
        public JsonResult GetProdusById(int id)
        {
            string query = $"SELECT * FROM Filme WHERE id_film = {id}";
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
        [Route("PostFilme")]

        public JsonResult PostFilme([FromForm] string nume_film, [FromForm] string descriere_film, [FromForm] string imagine, [FromForm] int id_categorie, [FromForm] string nume_producator, [FromForm] int durata, [FromForm] int pret_bilet)
        {
            string query = "INSERT INTO dbo.Filme" +
                "(nume_film, descriere_film, imagine, id_categorie, nume_producator, durata, pret_bilet)" +
                "values (@nume_film, @descriere_film, @imagine, @id_categorie, @nume_producator, @durata, @pret_bilet)";

            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("Cinema_BDAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nume_film", nume_film);
                    myCommand.Parameters.AddWithValue("@descriere_film", descriere_film);
                    myCommand.Parameters.AddWithValue("@imagine", imagine);
                    myCommand.Parameters.AddWithValue("@id_categorie", id_categorie);
                    myCommand.Parameters.AddWithValue("@nume_producator", nume_producator);
                    myCommand.Parameters.AddWithValue("@durata", durata);
                    myCommand.Parameters.AddWithValue("@pret_bilet", pret_bilet);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }


        [HttpPut]
        [Route("UpdateFilm/{id}")]
        public JsonResult UpdateFilm(int id, [FromForm] string nume_film, [FromForm] string descriere_film, [FromForm] string imagine, [FromForm] int id_categorie, [FromForm] string nume_producator, [FromForm] int durata, [FromForm] int pret_bilet)
        {
            string query = "UPDATE dbo.Filme SET " +
                "nume_film = @nume_film, " +
                "descriere_film = @descriere_film, " +
                "imagine = @imagine, " +
                "id_categorie = @id_categorie, " +
                "nume_producator = @nume_producator, " +
                "durata = @durata, " +
                "pret_bilet = @pret_bilet " +
                "WHERE id_film = @id";

            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("Cinema_BDAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myCommand.Parameters.AddWithValue("@nume_film", nume_film);
                    myCommand.Parameters.AddWithValue("@descriere_film", descriere_film);
                    myCommand.Parameters.AddWithValue("@imagine", imagine);
                    myCommand.Parameters.AddWithValue("@id_categorie", id_categorie);
                    myCommand.Parameters.AddWithValue("@nume_producator", nume_producator);
                    myCommand.Parameters.AddWithValue("@durata", durata);
                    myCommand.Parameters.AddWithValue("@pret_bilet", pret_bilet);

                    myCommand.ExecuteNonQuery();
                    myCon.Close();
                }
            }

            return new JsonResult("Update Successful");
        }

        [HttpDelete]
        [Route("DeleteFilm/{id}")]

        public JsonResult DeleteProdus(int id)
        {
            string query = "DELETE FROM dbo.Filme WHERE id_film = @id";
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

        [HttpDelete]
        [Route("GetDelete")]

        public JsonResult DeleteFilme()
        {
            string query = "DELETE FROM dbo.Filme" +
                "WHERE id_film = @id_film";

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
        [Route("GetTopMovies")]
        public JsonResult GetTopMovies()
        {
            string query = $"SELECT F.*, R1.TotalLocuriRezervate " +
                           $"FROM Filme F " +
                           $"JOIN Program P ON F.id_film = P.id_film " +
                           $"JOIN ( " +
                           $"    SELECT P1.id_film, COUNT(R2.id_rezervare) AS TotalLocuriRezervate " +
                           $"    FROM Program P1 " +
                           $"    LEFT JOIN Rezervari R2 ON P1.id_program = R2.id_program " +
                           $"    GROUP BY P1.id_film " +
                           $"    HAVING COUNT(R2.id_rezervare) >= 2) R1 ON F.id_film = R1.id_film " +
                           $"ORDER BY R1.TotalLocuriRezervate DESC;";

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
        [Route("GetRecentReservedMovies")]
        public JsonResult GetRecentReservedMovies()
        {
            string query = $"SELECT F.nume_film, P.data_si_ora AS DataProgramare " +
                           $"FROM Filme F " +
                           $"JOIN Program P ON F.id_film = P.id_film " +
                           $"WHERE P.data_si_ora BETWEEN DATEADD(DAY, -100, GETDATE()) AND GETDATE() " +
                           $"AND EXISTS ( " +
                           $"    SELECT 1 " +
                           $"    FROM Rezervari R " +
                           $"    WHERE R.id_program = P.id_program " +
                           $");";

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
        [Route("GetTopReservedMovies")]
        public JsonResult GetTopReservedMovies()
        {
            string query = $"SELECT F.*, R1.TotalLocuriRezervate " +
                           $"FROM Filme F " +
                           $"JOIN Program P ON F.id_film = P.id_film " +
                           $"JOIN ( " +
                           $"    SELECT P1.id_film, COUNT(R2.id_rezervare) AS TotalLocuriRezervate " +
                           $"    FROM Program P1 " +
                           $"    LEFT JOIN Rezervari R2 ON P1.id_program = R2.id_program " +
                           $"    GROUP BY P1.id_film " +
                           $"    HAVING COUNT(R2.id_rezervare) >= 2 " +
                           $") R1 ON F.id_film = R1.id_film " +
                           $"ORDER BY R1.TotalLocuriRezervate DESC;";

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
        [Route("GetProiectiiAcum100zile")]
        public JsonResult GetProiectiiAcum100zile()
        {
            string query = @"
                SELECT F.*, P.data_si_ora AS DataProgramare
                FROM Filme F
                JOIN Program P ON F.id_film = P.id_film
                JOIN Sali S ON P.id_sala = S.id_sala
                WHERE S.numar_locuri = (SELECT MAX(numar_locuri) FROM Sali);";

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
        [Route("GetFilmeRezervateUtilizatoriPeste18")]
        public JsonResult GetFilmeRezervateUtilizatoriPeste18()
        {
            string query = @"
                SELECT F.*
                FROM Filme F
                JOIN Program P ON F.id_film = P.id_film
                JOIN Rezervari R ON P.id_program = R.id_program
                JOIN Utilizatori U ON R.id_utilizator = U.id_utilizator
                WHERE DATEDIFF(YEAR, U.data_nastere, GETDATE()) > 18;";

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
