import { Navigate } from 'react-router-dom';
import { useAuth } from '../Contexts/AuthContext.jsx';

const PublicRoute = ({ element: Component, ...rest }) => {
    const { isAuthenticated } = useAuth();

    return isAuthenticated ? (
        <Navigate to="/dashboard" />
    ) : (
        <Component {...rest} />
    );
};

export default PublicRoute;

