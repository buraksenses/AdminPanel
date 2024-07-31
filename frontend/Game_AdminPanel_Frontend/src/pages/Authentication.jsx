import { useState } from "react";
import "../App.css";
import { useAuth } from "../security/AuthContext";
import { useNavigate } from "react-router-dom";
import LoginForm from "../components/LoginForm";
import Spinner from "../components/Spinner";
import {showSuccessToast} from "../utils/notifications.js";

function Authentication() {
  const { login, register, setIsAuthenticated } = useAuth();
  const navigate = useNavigate();
  const [isLogin, setIsLogin] = useState(true);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [email, setEmail] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const validateRegistration = () => {
    if (username.length < 5) {
      alert("Username must be at least 5 characters long!");
      return false;
    } else if (confirmPassword !== password) {
      alert("Passwords do not match!");
      return false;
    } else if (password.length < 8) {
      alert("Password must be at least 8 characters long!");
      return false;
    }
    return true;
  };

  const handleLogin = async () => {
    setIsLoading(true);
    const success = await login(username, password);
    setIsLoading(false);
    if (success) {
      setIsAuthenticated(true);
      showSuccessToast(`Logged in successfully! Welcome ${username}`)
      navigate("/dashboard", {replace: true});
    } else {
      alert("Login failed. Please check your username and password.");
    }
  };

  const handleRegister = async () => {
    const valid = validateRegistration();
    if (!valid) {
      alert("Validation failed! Check your information!");
      return;
    }
    setIsLoading(true);
    const success = await register(username, password);
    setIsLoading(false);
    if (success) {
      navigate("/auth", {replace: true});
    } else {
      alert("Registration failed. Please try again.");
    }
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    if (isLogin) {
      handleLogin();
    } else {
      handleRegister();
    }
  };

  return (
      <div className="auth-container">
        {isLoading && <Spinner />}
        <div className={`auth-box ${isLoading ? 'loading' : ''}`}>
          <div className="toggle-switch">
          <span
              className={`toggle-button ${isLogin ? "active" : ""}`}
              onClick={() => setIsLogin(true)}
          >
            Login
          </span>
            <span
                className={`toggle-button ${!isLogin ? "active" : ""}`}
                onClick={() => setIsLogin(false)}
            >
            Register
          </span>
          </div>
          <LoginForm
              handleSubmit={handleSubmit}
              setConfirmPassword={setConfirmPassword}
              isLogin={isLogin}
              setEmail={setEmail}
              setUsername={setUsername}
              setPassword={setPassword}
          />
        </div>
      </div>
  );
}

export default Authentication;
