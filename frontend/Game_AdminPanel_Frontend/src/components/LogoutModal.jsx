import "../LogoutModal.css";
const LogoutModal = ({ show, onClose }) => {
    if (!show) {
        return null;
    }

    return (
        <div className="logout-modal-overlay">
            <div className="logout-modal">
                <h2>Session Expired</h2>
                <p>Your session has expired. You will be redirected to the login page.</p>
                <button onClick={onClose}>OK</button>
            </div>
        </div>
    );
};

export default LogoutModal;
