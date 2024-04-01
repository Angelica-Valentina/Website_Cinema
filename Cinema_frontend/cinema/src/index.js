import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import reportWebVitals from './reportWebVitals';
import { AuthProvider } from './AuthContext';
import 
{
  createBrowserRouter,
  RouterProvider,
} 
from "react-router-dom";

import App from './App';
import SignUp from './Pages/SignUp';
import LogIn from './Pages/LogIn';
import Bilete from './Pages/Bilete/Bilete';
import AddMovies from './Pages/AddMovies';
import ProgramMovie from './Pages/ProgramMovie/ProgramMovie';
import Statistici from './Pages/Statistici/Statistici';
import UpdateMoviesForm from './Components/Admin/Update';
import Home from './Pages/Home/Home';

const router = createBrowserRouter ([
  {
    path: "/",
    element: <App/>,
  },
  {
    path: "/signup",
    element: <SignUp/>,
  },
  {
    path: "/login",
    element: <LogIn/>,
  },
  {
    path: "/add/movies",
    element: <AddMovies/>,
  },
  {
    path: "/programmovie/:id_film",
    element: <ProgramMovie/>,
  },
  {
    path: "/updatefilm/:id_film",
    element: <UpdateMoviesForm/>,
  },
  {
    path: "/bilete",
    element: <Bilete/>,
  },
  {
    path: "/statistici",
    element: <Statistici/>,
  },
  {
    path: "/home",
    element: <Home/>,
  }
]);

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render (
  <React.StrictMode>
    <AuthProvider >
      <RouterProvider router={router} />
    </AuthProvider>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
