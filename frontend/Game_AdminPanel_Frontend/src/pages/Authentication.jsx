import { useState } from "react";
import "../App.css";
import { useAuth } from "../security/AuthContext";
import { useNavigate } from "react-router-dom";
import LoginForm from "../components/LoginForm";

function Authentication() {
  const { login, register } = useAuth();
  const navigate = useNavigate();
  const [isLogin, setIsLogin] = useState(true);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [email, setEmail] = useState("");

  const validateRegistration = () => {
    if (username.length < 5) {
      alert("username must be at least 5 characters long!");
      return false;
    } else if (confirmPassword !== password) {
      alert("passwords do not match!");
      return false;
    } else if (password.length < 8) {
      alert("password must be at least 8 caharacters long!");
      return false;
    }
    return true;
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    let success;
    if (isLogin) {
      success = await login(username, password);
      if (success) {
        navigate("/dashboard"); // Başarılı girişte kullanıcıyı yönlendir
      } else {
        alert("Login failed. Please check your username and password.");
      }
    } else {
      const valid = validateRegistration();
      if (valid) {
        success = await register(username, password);
        if (success) navigate("/auth");
      } else alert("Validation failed! Check your information!");
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-box">
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
