import { BrowserRouter, Route, Routes } from "react-router-dom";
import Authentication from "./pages/Authentication";
import BuildingConfig from "./pages/BuildingConfig";
import { AuthProvider } from "./security/AuthContext";
import { ConfigurationsProvider } from "./Contexts/ConfigurationsContext";
import PrivateRoute from "./components/PrivateRoute.jsx";
import {useEffect, useState} from "react";
import LogoutModal from "./components/LogoutModal.jsx";

function App() {
  const [showLogoutModal, setShowLogoutModal] = useState(false);

  useEffect(() => {
    const handleTokenExpired = () => {
      setShowLogoutModal(true);
    };

    window.addEventListener('token-expired', handleTokenExpired);

    return () => {
      window.removeEventListener('token-expired', handleTokenExpired);
    };
  }, []);

  const handleLogout = () => {
    setShowLogoutModal(false);
    localStorage.removeItem('jwtToken');
    window.location.href = "/auth";
  };

  return (
    <div>
      <AuthProvider>
        <ConfigurationsProvider>
          <BrowserRouter>
            <LogoutModal show={showLogoutModal} onClose={handleLogout} />
            <Routes>
              <Route path="/" element={<Authentication />} />
              <Route path="/auth" element={<Authentication />} />
              <Route path="/dashboard" element={<PrivateRoute element={<BuildingConfig />} />} />
            </Routes>
          </BrowserRouter>
        </ConfigurationsProvider>
      </AuthProvider>
    </div>
  );
}

export default App;
