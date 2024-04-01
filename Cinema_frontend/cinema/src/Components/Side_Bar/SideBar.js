import { Link } from 'react-router-dom';


import { useAuth } from '../../AuthContext';  // Import the AuthContext hook

import './SideBar.css';

function SideBar() 
{
    const { isLoggedIn, logout } = useAuth();  // Use the authentication state and functions from the AuthContext

    console.log("loggedin: " + isLoggedIn);
    return (
        <div className='sidebar'>
            <ul className='bar-list'>
                <li className='bar-text'><Link to="/">Home</Link></li>
                { isLoggedIn ? null : <li className='bar-text'><Link to="/login">Log In</Link></li> }
                { isLoggedIn ? <li className='bar-text'><Link to="/home">Home</Link></li> : null }

                { isLoggedIn ? <li className='bar-text'><Link to="/add/movies">Add</Link></li> : null }
                { isLoggedIn ? <li className='bar-text'><Link to="/bilete">Bilete</Link></li> : null }
                { isLoggedIn ? <li className='bar-text'><Link to="/statistici">Statistici</Link></li> : null }
                { isLoggedIn ? <li className='bar-text' onClick={logout}>Logout</li> : null }
            </ul>
        </div>
    )
}

export default SideBar;