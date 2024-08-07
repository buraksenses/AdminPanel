import { Navigate } from 'react-router-dom';
import {useAuth} from "../Contexts/AuthContext.jsx";

const PrivateRoute = ({ element }) => {
    const {isAuthenticated} = useAuth();
    return isAuthenticated ? element : <Navigate to="/auth" replace />;
};

export default PrivateRoute;
