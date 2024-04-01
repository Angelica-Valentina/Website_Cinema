import React from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

import './MovieCard.css';

const MovieCard = ({ movie }) => {
  const navigate = useNavigate();

  function program() {
    navigate(`/programmovie/${movie.id_film}`);
  }

  function upd() {
    navigate(`/updatefilm/${movie.id_film}`);
  }

  async function deleteMovie() {
    try {
      await axios.delete(`https://localhost:7079/api/Filme/DeleteFilm/${movie.id_film}`);
      // Puteți adăuga și o reîmprospătare a paginii sau gestionare a listei de filme după ștergere
      console.log("Film deleted successfully");
    } catch (error) {
      console.error('Error deleting movie:', error);
    }
  }

  return (
    <div className="container-movie">
      <img src={movie.imagine} alt={movie.nume_film} />
      <h2>{movie.nume_film}</h2>
      <p>{movie.descriere_film}</p>
      <p>{movie.nume_categorie}</p>
      <p>{`Price: $${movie.pret_film}`}</p>
      <button onClick={program} className='rezervare'> Rezervare </button>
      <button onClick={upd}> Update </button>
      <button onClick={deleteMovie}> Delete </button>
      {console.log(movie.imagine)}
    </div>
  );
};

export default MovieCard;
