import axios from "axios";
import {createContext, useContext, useEffect, useState} from "react";
import {getToken, isTokenExpired, removeToken, setToken} from "../utils/auth.jsx";

const AuthContext = createContext();

const useAuth = () => useContext(AuthContext);

function AuthProvider({ children }) {
  const token = getToken();
  const [isAuthenticated, setIsAuthenticated] = useState(token && !isTokenExpired(token));
  const [username, setUsername] = useState(null);

  useEffect(() => {
    const token = getToken();
    if (token && !isTokenExpired(token)) {
      setIsAuthenticated(true);
    } else {
      removeToken();
    }
  }, []);

  async function login(username, password) {
    try {
      const response = await axios.post(
          "http://localhost:5218/api/Auth/login",
          {
            username,
            password,
          }
      );

      if (response.data !== null) {
        setUsername(username);
        setToken(response.data.data.token);
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
        "http://localhost:5218/api/Auth/register",
        {
          username,
          password,
          roles: ["Admin"],
        }
      );

      console.log(response.data);

      if (response.data.data === null) {
        setIsAuthenticated(false);
        setUsername(null);
        return false;
      }
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
      value={{ isAuthenticated, login, register, logout, username, setIsAuthenticated }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export { AuthProvider, useAuth, AuthContext };
