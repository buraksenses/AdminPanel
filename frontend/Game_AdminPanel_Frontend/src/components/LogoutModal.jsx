import '../LogoutModal.css';
import { useNavigateContext } from '../Contexts/NavigateContext.jsx';
import {useConfig} from "../Contexts/ConfigurationsContext.jsx";

const LogoutModal = ({ show, onClose }) => {
    const navigate = useNavigateContext();
    const { logout } = useConfig();

    const handleLogout = () => {
        localStorage.removeItem('jwtToken');
        onClose();
        logout();
        navigate("/auth");
    };

    if (!show) {
        return null;
    }

    return (
        <div className="logout-modal-overlay">
            <div className="logout-modal">
                <h2>Session Expired</h2>
                <p>Your session has expired. You will be redirected to the login page.</p>
                <button onClick={handleLogout}>OK</button>
            </div>
        </div>
    );
};

export default LogoutModal;
