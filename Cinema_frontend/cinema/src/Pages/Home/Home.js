import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../../AuthContext';
import axios from 'axios';
import './Home.css';

function Home() {
  const { isLoggedIn, logout } = useAuth();
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedMovieIndex, setSelectedMovieIndex] = useState(0);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get('https://localhost:7079/api/Filme/GetFilme');
        setMovies(response.data);
      } catch (error) {
        setError(error.message || 'An error occurred while fetching movies.');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  const handleMovieClick = (index) => {
    setSelectedMovieIndex(index);
  };

  const handlePrevButtonClick = () => {
    const newIndex = (selectedMovieIndex - 1 + movies.length) % movies.length;
    setSelectedMovieIndex(newIndex);
  };

  const handleNextButtonClick = () => {
    const newIndex = (selectedMovieIndex + 1) % movies.length;
    setSelectedMovieIndex(newIndex);
  };

  if (loading) {
    return <p>Loading...</p>;
  }

  if (error) {
    return <p>Error: {error}</p>;
  }

  // Obținem cele trei filme din lista de filme, folosind o rotație circulară în funcție de indexul selectat
  const visibleMovies = [
    movies[selectedMovieIndex % movies.length],
    movies[(selectedMovieIndex + 1) % movies.length],
    movies[(selectedMovieIndex + 2) % movies.length]
  ];

  return (
    <div className="Home">
      <div className="navbar">
        <ul className='bar-list'>
          <li className='bar-text'><Link to="/home">Home</Link></li>
          {isLoggedIn ? null : <li className='bar-text'><Link to="/login">Log In</Link></li>}
          {isLoggedIn ? <li className='bar-text'><Link to="/add/movies">Add</Link></li> : null}
          {isLoggedIn ? <li className='bar-text'><Link to="/bilete">Bilete</Link></li> : null}
          {isLoggedIn ? <li className='bar-text'><Link to="/statistici">Statistici</Link></li> : null}
          {isLoggedIn ? <li className='bar-text' onClick={logout}>Logout</li> : null}
        </ul>
      </div>
      <div
        className="background"
        style={{ backgroundImage: `url(${visibleMovies[0].imagine})` }}
      />
      <div className="gallery">
        {visibleMovies.map((movie, index) => (
          <div
            key={movie.id_film}
            className={`card ${index === 0 ? 'selected' : ''}`}
            onClick={() => handleMovieClick(selectedMovieIndex + index)}
            style={{ backgroundImage: `url(${movie.imagine})` }}
          />
        ))}
      </div>
      <div className="movie-details">
      <p className='nume-film'>{visibleMovies[0].nume_film}</p>
      <p className='descriere-film'>{visibleMovies[0].descriere_film}</p>
      <p className='durata'>Durata: {visibleMovies[0].durata} minute</p>
      <p className='producator'>Producător: {visibleMovies[0].nume_producator}</p>
      <p className='pret-bilet'>Preț bilet: {visibleMovies[0].pret_bilet} RON</p>
      <button className='cumpara-bilet'>Cumpără Bilet</button>
      </div>
      <div className='navigation-buttons'>
        <button onClick={handlePrevButtonClick}>Previous</button>
        <button onClick={handleNextButtonClick}>Next</button>
      </div>
    </div>
  );
}

export default Home;
