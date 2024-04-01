import MainLayout from "../../Components/Layout/MainLayout";
import { useParams, useNavigate } from 'react-router-dom';
import { useEffect, useState, useCallback } from 'react';
import axios from 'axios';
import { useAuth } from '../../AuthContext'; // Asigură-te că importul este corect

import './ProgramMovie.css';

const ProgramMovie = () => {
    const { isLoggedIn, id_utilizator } = useAuth();
    const navigate = useNavigate();
    const { id_film } = useParams();
    const [program, setProgram] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const fetchData = useCallback(async () => {
        try {
            const response = await axios.get(`https://localhost:7079/api/Program/GetProgramByFilm/${id_film}`);
            setProgram(response.data);
        } catch (error) {
            setError(error.message || 'An error occurred while fetching programs.');
        } finally {
            setLoading(false);
        }
    }, [id_film]);

    const reloadPrograms = () => fetchData();

    const handleBuyButtonClick = async (id_program) => {
        try {
            if (!isLoggedIn) {
                console.error('User not authenticated.');
                return;
            }
    
            const selectedProgram = program.find(programItem => programItem.id_sala === id_program);
    
            if (!selectedProgram) {
                console.error('Program not found in state.');
                return;
            }
    
            const response = await axios.post('https://localhost:7079/api/Rezervari/AddRezervare', {
                id_utilizator: id_utilizator,
                id_program,
            });
    
            console.log('Server response:', response.data);
            navigate('/bilete');
        } catch (error) {
            console.error('Error:', error);
            console.error('Error details:', error.response?.data);
            // Aici poți afișa un mesaj de eroare în interfața utilizatorului sau să gestionezi altfel situația de eroare.
        }
    };    

    useEffect(() => {
        fetchData();
    }, [id_film, fetchData]);

    if (loading) return <p>Loading...</p>;
    if (error) return <div><p>Error: {error}</p><button onClick={reloadPrograms}>Reload Programs</button></div>;
    if (program.length === 0) return <p>No programs available for this film.</p>;

    return (
        <MainLayout>
            <div className="container-program">
                <table className="program-table">
                    <thead>
                        <tr>
                            <th>ID Sala</th>
                            <th>Date and Time</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        {program.map(({ id_sala, data_si_ora }) => (
                            <tr key={`${id_sala}_${data_si_ora}`}>
                                <td>{id_sala}</td>
                                <td>{data_si_ora}</td>
                                <td>
                                    <button type="button" onClick={() => handleBuyButtonClick(id_sala)}>
                                        Buy
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </MainLayout>
    );
};

export default ProgramMovie;
