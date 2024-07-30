import { jwtDecode } from 'jwt-decode';

const isTokenExpired = (token) => {
    try {
        const decodedToken = jwtDecode(token);
        const currentTime = Date.now() / 1000;
        return decodedToken.exp < currentTime;
    } catch (error) {
        return true;
    }
};

const getToken = () => {
    return localStorage.getItem('jwtToken');
};

const setToken = (token) => {
    localStorage.setItem('jwtToken', token);
};

const removeToken = () => {
    localStorage.removeItem('jwtToken');
};

export {isTokenExpired, getToken, setToken, removeToken};
