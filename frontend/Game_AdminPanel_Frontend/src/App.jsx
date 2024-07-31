import { useState, useEffect } from 'react';
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Authentication from "./pages/Authentication";
import BuildingConfig from "./pages/BuildingConfig";
import PrivateRoute from './components/PrivateRoute';
import LogoutModal from './components/LogoutModal';
import {AuthProvider} from "./security/AuthContext";
import {ConfigurationsProvider} from "./Contexts/ConfigurationsContext";
import { NavigateProvider } from './Contexts/NavigateContext.jsx';
import PublicRoute from "./components/PublicRoute.jsx";
import {ToastContainer} from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';

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
                  <Route path="/auth" element={<PublicRoute element={Authentication} />} />
                  <Route path="/dashboard" element={<PrivateRoute element={<BuildingConfig />} />} />
                </Routes>
                <ToastContainer />
              </NavigateProvider>
            </BrowserRouter>
          </ConfigurationsProvider>
        </AuthProvider>
      </div>
  );
}

export default App;
