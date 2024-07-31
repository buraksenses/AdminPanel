import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
export const showSuccessToast = (message) => {
    toast.success(message, {
        position: "top-right",
        closeButton: true,
        hideProgressBar: false,
        autoClose: 5000,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
    });
};

export const showErrorToast = (message) => {
    toast.error(message, {
        position: "top-right",
        closeButton: true,
        hideProgressBar: false,
        autoClose: 5000,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
    });
};

export const showWarningToast = (message) => {
    toast.warning(message, {
        position: "top-right",
        closeButton: true,
        hideProgressBar: false,
        autoClose: 5000,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
    });
};

export const showInfoToast = (message) => {
    toast.info(message, {
        position: "top-right",
        closeButton: true,
        hideProgressBar: false,
        autoClose: 5000,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
    });
};
