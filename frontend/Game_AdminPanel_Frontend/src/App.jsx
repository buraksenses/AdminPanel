import { BrowserRouter, Route, Routes } from "react-router-dom";
import Authentication from "./pages/Authentication";
import BuildingConfig from "./pages/BuildingConfig";
import { AuthProvider } from "./security/AuthContext";
import { ConfigurationsProvider } from "./Contexts/ConfigurationsContext";

function App() {
  return (
    <div>
      <AuthProvider>
        <ConfigurationsProvider>
          <BrowserRouter>
            <Routes>
              <Route path="/auth" element={<Authentication />} />
              <Route path="/dashboard" element={<BuildingConfig />} />
            </Routes>
          </BrowserRouter>
        </ConfigurationsProvider>
      </AuthProvider>
    </div>
  );
}

export default App;
