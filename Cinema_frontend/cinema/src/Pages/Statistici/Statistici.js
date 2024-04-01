import { useEffect, useState } from 'react';
import { useAuth } from '../../AuthContext';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import SideBar from '../../Components/Side_Bar/SideBar';

import './Statistici.css';

const Statistici = () => {
    const [loading, setLoading] = useState(true);
    const { isLoggedIn, isAdmin } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (!isLoggedIn || !isAdmin) {
            navigate("/");
        }
    }, [isLoggedIn, isAdmin, navigate]);

    const API_URL = "https://localhost:7079/";

    const [topReservedMovies, setTopReservedMovies] = useState([]);
    const [recentReservedMovies, setRecentReservedMovies] = useState([]);
    const [topMovies, setTopMovies] = useState([]);
    const [topUtilizatori, setTopUtilizatori] = useState([]);
    const [vipUtilizatori, setVipUtilizatori] = useState([]);
    const [administratoriUtilizatori, setAdministratoriUtilizatori] = useState([]);
    const [proiectiiAcum100zile, setProiectiiAcum100zile] = useState([]);
    const [filmeRezervatePeste18, setFilmeRezervatePeste18] = useState([]);
    const [sali100, setSali100] = useState([]);
    const [nrFilmeCategorie, setNrFilmeCategorie] = useState([]);


    useEffect(() => {
        const fetchData = async () => {
            try {
                const topReservedMoviesResponse = await axios.get(`${API_URL}api/Filme/GetTopReservedMovies`);
                setTopReservedMovies(topReservedMoviesResponse.data);

                const recentReservedMoviesResponse = await axios.get(`${API_URL}api/Filme/GetRecentReservedMovies`);
                setRecentReservedMovies(recentReservedMoviesResponse.data);

                const topMoviesResponse = await axios.get(`${API_URL}api/Filme/GetTopMovies`);
                setTopMovies(topMoviesResponse.data);

                const topUtilizatoriResponse = await axios.get(`${API_URL}api/Utilizatori/GetTopUtilizatori`);
                setTopUtilizatori(topUtilizatoriResponse.data);


                const vipUtilizatoriResponse = await axios.get(`${API_URL}api/Utilizatori/GetVIPUtilizatori`);
                setVipUtilizatori(vipUtilizatoriResponse.data);

                const administratoriUtilizatoriResponse = await axios.get(`${API_URL}api/Administratori/GetAdministratoriUtilizatori`);
                setAdministratoriUtilizatori(administratoriUtilizatoriResponse.data);

                const proiectiiAcum100zileResponse = await axios.get(`${API_URL}api/Filme/GetProiectiiAcum100zile`);
                setProiectiiAcum100zile(proiectiiAcum100zileResponse.data);

                const filmeRezervatePeste18Response = await axios.get(`${API_URL}api/Filme/GetFilmeRezervateUtilizatoriPeste18`);
                setFilmeRezervatePeste18(filmeRezervatePeste18Response.data);

                const sali100Response = await axios.get(`${API_URL}api/Sali/GetSali100`);
                setSali100(sali100Response.data);

                const nrFilmeCategorieResponse = await axios.get(`${API_URL}api/CategoriiFilme/GetNrFilmeCategorie`);
                setNrFilmeCategorie(nrFilmeCategorieResponse.data);


                setLoading(false);
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        };

        fetchData();
    }, [API_URL]);

    if (loading) {
        return <div>Loading...</div>;
    }

    return (
        <div className="statistici">
            <SideBar />
            <h2>Statistici</h2>
            <h3>Top filme rezervate:</h3>
            {topReservedMovies.map((movie, index) => (
                <div key={index} className='result'>
                    <p><span>Film:</span> {movie.nume_film}</p>
                    <p><span>Total locuri rezervate:</span> {movie.TotalLocuriRezervate}</p>
                </div>
            ))}

            <h3>Filme rezervate recent:</h3>
            {recentReservedMovies.map((movie, index) => (
                <div key={index} className='result'>
                    <p><span>Film:</span> {movie.nume_film}</p>
                    <p><span>Data programare:</span> {movie.DataProgramare}</p>
                </div>
            ))}

            <h3>Top filme:</h3>
            {topMovies.map((movie, index) => (
                <div key={index} className='result'>
                    <p><span>Film:</span> {movie.nume_film}</p>
                    <p><span>Total locuri rezervate:</span> {movie.TotalLocuriRezervate}</p>
                </div>
            ))}

            <h3>Top utilizatori:</h3>
            {topUtilizatori.map((utilizator, index) => (
                <div key={index} className='result'>
                    <p><span>Utilizator:</span> {utilizator.Nume} {utilizator.Prenume}</p>
                    <p><span>Total rezervări:</span> {utilizator.TotalRezervari}</p>
                </div>
            ))}


// Afișează tabel pentru Utilizatori VIP
            <h3>Utilizatori VIP:</h3>
            <table>
                <thead>
                    <tr>
                        <th>ID Utilizator</th>
                        <th>Data Nastere</th>
                        {/* Adaugă aici altele coloane specifice, dacă există */}
                    </tr>
                </thead>
                <tbody>
                    {vipUtilizatori.map((utilizator, index) => (
                        <tr key={index}>
                            <td>{utilizator.id_utilizator}</td>
                            <td>{utilizator.data_nastere}</td>
                            {/* Adaugă aici altele valori specifice, dacă există */}
                        </tr>
                    ))}
                </tbody>
            </table>

// Afișează tabel pentru Administratori Utilizatori
            <h3>Administratori Utilizatori:</h3>
            <table>
                <thead>
                    <tr>
                        <th>ID Utilizator</th>
                        <th>Data Nastere</th>
                        <th>Functie Administrator</th>
                    </tr>
                </thead>
                <tbody>
                    {administratoriUtilizatori.map((utilizator, index) => (
                        <tr key={index}>
                            <td>{utilizator.id_utilizator}</td>
                            <td>{utilizator.data_nastere}</td>
                            <td>{utilizator.FunctieAdministrator}</td>
                        </tr>
                    ))}
                </tbody>
            </table>

// Afișează tabel pentru Categorii Filme și Numărul Total de Filme
            <h3>Categorii Filme și Numărul Total de Filme:</h3>
            <table>
                <thead>
                    <tr>
                        <th>ID Categorie</th>
                        <th>Nume Categorie</th>
                        <th>Număr Total Filme</th>
                    </tr>
                </thead>
                <tbody>
                    {nrFilmeCategorie.map((categorie, index) => (
                        <tr key={index}>
                            <td>{categorie.id_categorie}</td>
                            <td>{categorie.nume_categorie}</td>
                            <td>{categorie.NumarTotalFilme}</td>
                        </tr>
                    ))}
                </tbody>
            </table>

// Afișează tabel pentru Filmele Programate în Sala cu Cele Mai Multe Locuri
            <h3>Filme Programate în Sala cu Cele Mai Multe Locuri:</h3>
            <table>
                <thead>
                    <tr>
                        <th>ID Film</th>
                        <th>Nume Film</th>
                        <th>Data Programare</th>
                        {/* Adaugă aici altele coloane specifice, dacă există */}
                    </tr>
                </thead>
                <tbody>
                    {proiectiiAcum100zile.map((proiectie, index) => (
                        <tr key={index}>
                            <td>{proiectie.id_film}</td>
                            <td>{proiectie.nume_film}</td>
                            <td>{proiectie.DataProgramare}</td>
                            {/* Adaugă aici altele valori specifice, dacă există */}
                        </tr>
                    ))}
                </tbody>
            </table>

// Afișează tabel pentru Filme Rezervate de Utilizatori Peste 18 Ani
            <h3>Filme Rezervate de Utilizatori Peste 18 Ani:</h3>
            <table>
                <thead>
                    <tr>
                        <th>ID Film</th>
                        <th>Nume Film</th>
                        {/* Adaugă aici altele coloane specifice, dacă există */}
                    </tr>
                </thead>
                <tbody>
                    {filmeRezervatePeste18.map((film, index) => (
                        <tr key={index}>
                            <td>{film.id_film}</td>
                            <td>{film.nume_film}</td>
                            {/* Adaugă aici altele valori specifice, dacă există */}
                        </tr>
                    ))}
                </tbody>
            </table>

// Afișează tabel pentru Săli Utilizate în Ultimele 100 de Zile
            <h3>Săli Utilizate în Ultimele 100 de Zile:</h3>
            <table>
                <thead>
                    <tr>
                        <th>ID Sală</th>
                        {/* Adaugă aici altele coloane specifice, dacă există */}
                    </tr>
                </thead>
                <tbody>
                    {sali100.map((sala, index) => (
                        <tr key={index}>
                            <td>{sala.id_sala}</td>
                            {/* Adaugă aici altele valori specifice, dacă există */}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default Statistici;
