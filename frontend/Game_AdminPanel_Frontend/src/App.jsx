import { BrowserRouter, Route, Routes } from "react-router-dom";
import Authentication from "./pages/Authentication";
import BuildingConfig from "./pages/BuildingConfig";
import PrivateRoute from './components/PrivateRoute';
import {AuthProvider} from "./security/AuthContext";
import {ConfigurationsProvider} from "./Contexts/ConfigurationsContext";
import { NavigateProvider } from './Contexts/NavigateContext.jsx';
import PublicRoute from "./components/PublicRoute.jsx";
import {ToastContainer} from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';

function App() {

  return (
      <div>
        <BrowserRouter>
        <AuthProvider>
          <ConfigurationsProvider>
              <NavigateProvider>
                <Routes>
                  <Route path="/auth" element={<PublicRoute element={Authentication} />} />
                  <Route path="/dashboard" element={<PrivateRoute element={<BuildingConfig />} />} />
                </Routes>
                <ToastContainer />
              </NavigateProvider>
          </ConfigurationsProvider>
        </AuthProvider>
        </BrowserRouter>
      </div>
  );
}

export default App;
