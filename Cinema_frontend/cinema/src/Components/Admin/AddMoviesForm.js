import { useState } from 'react';
import './AddMoviesForm.css'

import axios from 'axios';

const AddMoviesForm = () => {

    const API = 'https://localhost:7079/api/';

    const [movie, setMovie] = useState ({
        nume_film: '',
        descriere_film: '',
        imagine: '',
        id_categorie: 1,
        nume_producator: '',
        durata: 0,
        pret_bilet: 0
    })

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

            const response = await axios.post(API + 'Filme/PostFilme', formData);

            if (response.status === 200) {
                console.log("Succes!");

                setMovie({
                    nume_film: '',
                    descriere_film: '',
                    imagine: '',
                    id_categorie: 1,
                    nume_producator: '',
                    durata: 0,
                    pret_bilet: 0
                });
            } else {
                console.log("Eroare!");
            }
        } catch(error) {
            console.error('Eroare: ', error);
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
            <input type="text" name="nume_producator" value={movie.nume_prodcator} onChange={handleChange}/>

            <label>
                Durata
            </label>
            <input type="number" name="durata" value={movie.durata} onChange={handleChange}/>

            <label>
                Pret
            </label>
            <input type="number" name="pret_bilet" value={movie.pret_bilet} onChange={handleChange}/>

            <button type='submit'>Adaugă film</button>
        </form>
    )
}

export default AddMoviesForm; 