import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

import './Form.css';

function SignUpForm() {
    const [user, setUser] = useState({
      nume: '',
      prenume: '',
      data_nastere: new Date().toISOString(),
      telefon: '',
      email: '', 
      parola: ''
    });

    const navigate = useNavigate();

    const handleChange = (e) => {
      const { name, value } = e.target;
      setUser((prevUser) => ({
        ...prevUser,
        [name]: value,
      }));
    };

    const handleSubmit = async (e) => {
      e.preventDefault();

      try {
        const formData = new FormData();

        Object.entries(user).forEach(([key, value]) => {
          formData.append(key, value);
        });

        const response = await axios.post('https://localhost:7079/api/Utilizatori/AddUtilizator', formData);

        if (response.status === 200) {
          console.log('User added successfully');
          // Redirect or perform any other actions upon successful addition
          navigate("/"); // TODO: change
        } else {
          console.error('Unexpected response status:', response.status);
        }
      } catch (error) {
        if (error.response && error.response.status === 400) {
          // Server returned validation errors
          console.error('Validation errors:', error.response.data);
        } else {
          console.error('Error:', error);
        }
      }
    }

    useEffect(() => {
      const storedToken = localStorage.getItem('token');
      const isLoggedIn = !!storedToken;

      if (isLoggedIn) {
        navigate('/');
      }
    }, [navigate]);

    return (
      <div className='form form-two-columns'>
        <div className='form-column'>
          <label>
            Nume:
            <input 
              name="nume"
              type="text"
              value={user.nume}
              onChange={handleChange} required
            />
          </label>

          <label>
            Prenume:
            <input
              name="prenume"
              type="text"
              value={user.prenume}
              onChange={handleChange} required
            />
          </label>

          <label>
            Email:
            <input
              name="email"
              type="text"
              value={user.email}
              onChange={handleChange} required
            />
          </label>

          <label>
            Telefon:
            <input
              name="telefon"
              type="tel"
              value={user.telefon}
              onChange={handleChange} required
            />
          </label>
        </div>

        <div className='form-column'>

          <label>
            Parolă:
            <input
              name="parola"
              type="password"
              value={user.parola}
              onChange={handleChange} required
            />
          </label>


        </div>

        <button className='submit submit-two-columns' type="submit" onClick={handleSubmit}>Sign UP</button>
      </div>
    );
}

export default SignUpForm;
