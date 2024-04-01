import { useState, useEffect } from 'react';
import axios from 'axios';

const UpdateMoviesForm = ({ filmId }) => {
    const API = 'https://localhost:7079/api/';
    
    const [movie, setMovie] = useState({
        nume_film: '',
        descriere_film: '',
        imagine: '',
        id_categorie: 1,
        nume_producator: '',
        durata: 0,
        pret_bilet: 0
    });

    useEffect(() => {
        // Fetch the current movie details
        if (!filmId) {
            console.error("filmId is undefined");
            return;
        }
        else {
            console.log("Film ID:", filmId); // Check the filmId value
        }

        const fetchMovieDetails = async () => {
            try {
                const response = await axios.get(`${API}Filme/GetFilmById/${filmId}`);
                setMovie(response.data);
            } catch (error) {
                console.error('Error fetching movie details:', error);
            }
        };

        fetchMovieDetails();
    }, [filmId, API]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setMovie((prevMovie) => ({
            ...prevMovie,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const formData = new FormData();
            Object.entries(movie).forEach(([key, value]) => {
                formData.append(key, value);
            });

            // Update the movie using a PUT request
            await axios.put(`${API}UpdateFilm/${filmId}`, formData);

            console.log("Film updated successfully!");
        } catch (error) {
            console.error('Error updating movie:', error);
        }
    };

    return (
        <form className='form' onSubmit={handleSubmit}>
            <label>
                Nume
            </label>
            <input type="text" name="nume_film" value={movie.nume_film} onChange={handleChange}/>

            <label>
                Descriere
            </label>
            <input type="text" name="descriere_film" value={movie.descriere_film} onChange={handleChange}/>

            <label>
                Imagine
            </label>
            <input type="text" name="imagine" value={movie.imagine} onChange={handleChange}/>

            <label>
                Producator
            </label>
            <input type="text" name="nume_producator" value={movie.nume_producator} onChange={handleChange}/>

            <label>
                Durata
            </label>
            <input type="number" name="durata" value={movie.durata} onChange={handleChange}/>

            <label>
                Pret
            </label>
            <input type="number" name="pret_bilet" value={movie.pret_bilet} onChange={handleChange}/>
            <button type='submit'>Actualizează film</button>
        </form>
    );
};

export default UpdateMoviesForm;