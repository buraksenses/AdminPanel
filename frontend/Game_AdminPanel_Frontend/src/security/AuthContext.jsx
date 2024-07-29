import axios from "axios";
import { createContext, useContext, useState } from "react";

const AuthContext = createContext();

const useAuth = () => useContext(AuthContext);

function AuthProvider({ children }) {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [username, setUsername] = useState(null);
  const [jwtToken, setJwtToken] = useState(null);

  async function login(username, password) {
    try {
      const response = await axios.post(
        "http://localhost:5218/api/Auth/login",
        {
          username,
          password,
        }
      );

      console.log(response.data);

      if (response.data !== null) {
        setIsAuthenticated(true);
        setUsername(username);
        setJwtToken(response.data.data.jwtToken); // Token'ı state'e kaydet

        // Token'ı localStorage'a kaydet
        localStorage.setItem("authToken", jwtToken);

        return true;
      } else {
        setIsAuthenticated(false);
        setUsername(null);
        setJwtToken(null); // Başarısız olduğunda token'ı sıfırla
        return false;
      }
    } catch (error) {
      console.error("Login failed", error);
      setIsAuthenticated(false);
      setUsername(null);
      setJwtToken(null); // Hata durumunda token'ı sıfırla
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
