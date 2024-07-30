import axios from "axios";
import { createContext, useContext, useState } from "react";

const AuthContext = createContext();

const useAuth = () => useContext(AuthContext);

function AuthProvider({ children }) {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [username, setUsername] = useState(null);

  async function login(username, password) {
    try {
      const response = await axios.post(
          "http://localhost:5218/api/Auth/login",
          {
            username,
            password,
          }
      );
      alert(response.data.data.jwtToken.toString())
      if (response.data !== null) {
        setUsername(username);

        localStorage.setItem("jwtToken", response.data.data.jwtToken);
        window.location.href = "/dashboard";
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

      console.log(response.data); // Yanıtın yapısını kontrol edin

      if (response.data.data === null) {
        setIsAuthenticated(false);
        setUsername(null);
        setJwtToken(null);
        return false;
      }
    } catch (error) {
      console.error(error.response.data.errors);
      alert(error.response.data.errors);
      setIsAuthenticated(false);
      setUsername(null);
      setJwtToken(null);
      return false;
    }
  }

  function logout() {
    setIsAuthenticated(false);
    setUsername(null);
    setJwtToken(null);
    localStorage.removeItem("authToken");
  }

  return (
    <AuthContext.Provider
      value={{ isAuthenticated, login, register, logout, username }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export { AuthProvider, useAuth, AuthContext };
