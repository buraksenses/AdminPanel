import { BrowserRouter, Route, Routes } from "react-router-dom";
import Authentication from "./pages/Authentication";
import BuildingConfig from "./pages/BuildingConfig";
import { AuthProvider } from "./security/AuthContext";

function App() {
  return (
    <div>
      <AuthProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/auth" element={<Authentication />} />
            <Route path="/dashboard" element={<BuildingConfig />} />
          </Routes>
        </BrowserRouter>
      </AuthProvider>
    </div>
  );
}

export default App;
