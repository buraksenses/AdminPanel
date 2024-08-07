import Cookies from 'js-cookie';
import {jwtDecode} from 'jwt-decode';

const COOKIE_NAME = 'accessToken';

const isTokenExpired = () => {
    const token = Cookies.get(COOKIE_NAME);
    if (!token) return true;

    try {
        const decodedToken = jwtDecode(token);
        const currentTime = Date.now() / 1000;
        return decodedToken.exp < currentTime;
    } catch (error) {
        return true;
    }
};


const getToken = () => {
    return Cookies.get(COOKIE_NAME);
};

const setToken = (token, cookieName, options) => {
    Cookies.set(cookieName, token, { secure: options.secure, sameSite: options.sameSite, expires: options.expires });
};

const removeToken = () => {
    Cookies.remove(COOKIE_NAME);
};

export { isTokenExpired, getToken, setToken, removeToken };
