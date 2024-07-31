import { useState, useEffect } from 'react';
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Authentication from "./pages/Authentication";
import BuildingConfig from "./pages/BuildingConfig";
import PrivateRoute from './components/PrivateRoute';
import LogoutModal from './components/LogoutModal';
import { AuthProvider } from "./security/AuthContext";
import { ConfigurationsProvider } from "./Contexts/ConfigurationsContext";
import { NavigateProvider } from './Contexts/NavigateContext.jsx';

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

  return (
      <div>
        <AuthProvider>
          <ConfigurationsProvider>
            <BrowserRouter>
              <NavigateProvider>
                <LogoutModal show={showLogoutModal} onClose={() => setShowLogoutModal(false)} />
                <Routes>
                  <Route path="/" element={<Authentication />} />
                  <Route path="/auth" element={<Authentication />} />
                  <Route path="/dashboard" element={<PrivateRoute element={<BuildingConfig />} />} />
                </Routes>
              </NavigateProvider>
            </BrowserRouter>
          </ConfigurationsProvider>
        </AuthProvider>
      </div>
  );
}

export default App;
