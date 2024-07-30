import { Navigate } from 'react-router-dom';
import { isTokenExpired, getToken } from '../utils/auth.jsx';

const PrivateRoute = ({ element }) => {
    const token = getToken();
    const isAuthenticated = token && !isTokenExpired(token);

    return isAuthenticated ? element : <Navigate to="/auth" />;
};

export default PrivateRoute;
