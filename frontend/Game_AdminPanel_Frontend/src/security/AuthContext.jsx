import axios from "axios";
import {createContext, useContext, useEffect, useState} from "react";
import {getToken, isTokenExpired, removeToken, setToken} from "../utils/auth.jsx";
import {showWarningToast} from "../utils/notifications.js";
import {useLocation} from "react-router-dom";
import Cookies from "js-cookie";
import apiClient from "../api/GameApiService.jsx";

const AuthContext = createContext();

const useAuth = () => useContext(AuthContext);

function AuthProvider({ children }) {
  const token = getToken();
  const location = useLocation();
  const [isAuthenticated, setIsAuthenticated] = useState(token && !isTokenExpired(token));
  const [username, setUsername] = useState(null);
  const [isLogin, setIsLogin] = useState(true);

  /*useEffect(() => {
    const token = getToken();
    if (token && !isTokenExpired(token)) {
      setIsAuthenticated(true);
    } else {
      removeToken();
    }
  }, []);*/

  useEffect(() => {
    const refreshAccessToken = async () => {
      try {
        const token = getToken();
        if (token && !isTokenExpired(token)) {
          setIsAuthenticated(true);
          return;
        }

        const refreshToken = Cookies.get('refreshToken');
        if (!refreshToken) {
          if (location.pathname === '/dashboard') {
            removeToken();
            setIsAuthenticated(false);
            throw new Error("Session expired!");
          } else {
            removeToken();
            setIsAuthenticated(false);
            return;
          }
        }

        const response = await apiClient.post('https://cbewzfrmej.eu-central-1.awsapprunner.com/api/Auth/refresh-token', {});

        const { accessToken, cookieOptions } = response.data.data;
        setToken(accessToken, 'accessToken', {
          secure: true,
          sameSite: 'Strict',
          expires: new Date(cookieOptions.expires)
        });
        setIsAuthenticated(true);

      } catch (error) {
        console.error("Authentication refresh failed", error);
        removeToken();
        setIsAuthenticated(false);
        if (location.pathname === '/dashboard') {
          showWarningToast("Session expired!")
        }
      }
    };

    refreshAccessToken();
  }, []);


  async function login(username, password) {
    try {
      const response = await axios.post(
          "https://cbewzfrmej.eu-central-1.awsapprunner.com/api/Auth/login",
          {
            username,
            password,
          }
      );

      if (response.data !== null) {
        setUsername(username);
        setToken(response.data.data.accessToken, 'accessToken', {secure: true, sameSite: 'Strict', expires: new Date(response.data.data.cookieOptions.expires)});
        setToken(response.data.data.refreshToken.token, 'refreshToken', {secure: true, sameSite: 'Strict', expires: new Date(response.data.data.refreshToken.expires)});
        setIsAuthenticated(true);
        return true;
      } else {
        setUsername(null);
        alert("Login failed. Please try again.");
        return false;
      }
    } catch (error) {
      alert("Login failed. Please try again.");
      setUsername(null);
      return false;
    }
  }

  async function register(username, password) {
    try {
      const response = await axios.post(
        "https://cbewzfrmej.eu-central-1.awsapprunner.com/api/Auth/register",
        {
          username,
          password
        }
      );

      console.log(response.data);

      if (response.data === null) {
        setIsAuthenticated(false);
        return false;
      }
      setUsername(null);
      return true;
    } catch (error) {
      console.error(error.response.data.errors);
      alert(error.response.data.errors);
      setIsAuthenticated(false);
      setUsername(null);
      return false;
    }
  }

  function logout() {
    setIsAuthenticated(false);
    setUsername(null);
    removeToken();
  }

  return (
    <AuthContext.Provider
      value={{ isLogin, setIsLogin, isAuthenticated, login, register, logout, username, setIsAuthenticated, setUsername}}
    >
      {children}
    </AuthContext.Provider>
  );
}

export { AuthProvider, useAuth, AuthContext };
