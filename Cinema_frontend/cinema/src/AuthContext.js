import { createContext, useContext, useEffect, useState } from 'react';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [isLoggedIn, setIsLoggedIn] = useState(!!localStorage.getItem('token'));
    const [isAdmin, setIsAdmin] = useState(null);
    const [userInfo, setUserInfo] = useState([]);

    const checkAdminStatus = async (userId) => {
        try {
            if (!userId) {
                setIsAdmin(false);
                return;
            }

            const response = await fetch(`https://localhost:7079/api/Utilizatori/IsAdmin/${userId}`);
            const isAdminValueArray = await response.json();

            if (Array.isArray(isAdminValueArray) && isAdminValueArray.length > 0) {
                const isAdminValue = isAdminValueArray[0].IsAdmin;
                setIsAdmin(Boolean(isAdminValue));
            } else {
                setIsAdmin(false);
            }
        } catch (error) {
            console.error('Error checking admin status:', error);
            setIsAdmin(false);
        }
    };

    useEffect(() => {
        const storedUserInfo = JSON.parse(localStorage.getItem('userInfo'));
        if (storedUserInfo) {
            setUserInfo(storedUserInfo);
            checkAdminStatus(storedUserInfo?.UserID);
        }
    }, []); // Empty dependency array to run only on mount

    const login = (token, userInfo) => {
        localStorage.setItem('token', token);
        localStorage.setItem('userInfo', JSON.stringify(userInfo));
        setIsLoggedIn(true);
        setUserInfo(userInfo);
        checkAdminStatus(userInfo?.UserID);
    };

    const logout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('userInfo');
        setIsLoggedIn(false);
        setUserInfo([]);
        setIsAdmin(false);
    };

    return (
        <AuthContext.Provider value={{ isLoggedIn, isAdmin, userInfo, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    return useContext(AuthContext);
};
