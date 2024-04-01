import React, { useState, useEffect } from 'react';
import axios from 'axios';

import MovieCard from '../MovieCard/MovieCard';
import './Movies.css';

const Movies = () => {
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentMovieIndex, setCurrentMovieIndex] = useState(0);

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

  useEffect(() => {
    fetchData();
  }, []); // Efectul se va executa doar o singură dată la montarea componentei

  const showNextMovie = () => {
    setCurrentMovieIndex((prevIndex) => (prevIndex + 1) % movies.length);
  };

  const showPrevMovie = () => {
    setCurrentMovieIndex((prevIndex) =>
      prevIndex === 0 ? movies.length - 1 : prevIndex - 1
    );
  };

  const reloadMovies = () => {
    fetchData();
  };

  if (loading) {
    return <p>Loading...</p>;
  }

  if (error) {
    return (
      <div>
        <p>Error: {error}</p>
        <button onClick={reloadMovies}>Reload Movies</button>
      </div>
    );
  }

  return (
    <div className="container-movies">
      <div className='prev' onClick={showPrevMovie}>
        <div className='prev-triangle'></div>
      </div>
      <div className='about'>
        <MovieCard
          movie = {movies[currentMovieIndex]}
        />

      </div>
      <div className='next' onClick={showNextMovie}>
        <div className='next-triangle'></div>
      </div>
    </div>
  );
};

export default Movies;
