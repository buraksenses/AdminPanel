import { useState } from "react";
import "../App.css";
import { useAuth } from "../security/AuthContext";
import { useNavigate } from "react-router-dom";
import Spinner from "../components/Spinner.jsx";

function Authentication() {
  const { login, register } = useAuth();
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
      navigate("/dashboard");
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
    const success = await register(username, email, password);
    setIsLoading(false);
    if (success) {
      navigate("/auth");
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
          <form className="auth-form" onSubmit={handleSubmit}>
            <div className="form-group">
              <label>Username</label>
              <input
                  type="text"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  required
              />
            </div>
            {!isLogin && (
                <div className="form-group">
                  <label>Email</label>
                  <input
                      type="email"
                      value={email}
                      onChange={(e) => setEmail(e.target.value)}
                      required
                  />
                </div>
            )}
            <div className="form-group">
              <label>Password</label>
              <input
                  type="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
              />
            </div>
            {!isLogin && (
                <div className="form-group">
                  <label>Confirm Password</label>
                  <input
                      type="password"
                      value={confirmPassword}
                      onChange={(e) => setConfirmPassword(e.target.value)}
                      required
                  />
                </div>
            )}
            <div className="form-actions">
              <button type="submit">{isLogin ? "Login" : "Register"}</button>
            </div>
          </form>
        </div>
      </div>
  );
}

export default Authentication;
