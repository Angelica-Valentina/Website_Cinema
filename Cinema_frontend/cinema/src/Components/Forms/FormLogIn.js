import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { Link } from 'react-router-dom';


import { useAuth } from '../../AuthContext';  // Import the AuthContext hook


import './Form.css';

function FormLogIn() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    
    const { login } = useAuth();  // Use the login function from the AuthContext


    const handleLogin = async () => {
        try {
            const formData = new FormData();
            formData.append('email', email);
            formData.append('parola', password);
            const response = await axios.post('https://localhost:7079/api/Utilizatori/Login', formData);

            if (response.status === 200) {
                const { Token, UserID, Nume, Prenume } = response.data;
                login(Token, { UserID, Nume, Prenume });
                navigate('/add/movies');
                console.log('Login successful!', Token, UserID, Nume, Prenume);
            } else {
                console.error('Login failed:', response.data);
            }
        } catch (error) {
            console.error('Login failed:', error.message);
        }

    };

    return (
        <div className='form'>
            <label>
                Email:
                <input
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
            </label>

            <label>
                Parola:
                <input
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
            </label>

            <button className='submit' type="submit" onClick={handleLogin}>LOG IN</button>

            <p className='new-account'> Dacă nu ai cont, îl poți crea <Link to="/signup" className='link-signup'>aici</Link></p>
        </div>
    );
}

export default FormLogIn;
