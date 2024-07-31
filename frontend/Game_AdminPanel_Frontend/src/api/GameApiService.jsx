import axios from 'axios';
import { getToken, isTokenExpired, removeToken } from '../utils/auth.jsx';

const apiClient = axios.create({
    baseURL: 'http://localhost:5228',
});

apiClient.interceptors.request.use(
    (config) => {
        const token = getToken();
        if (token) {
            if (isTokenExpired(token)) {
                window.dispatchEvent(new CustomEvent('token-expired'));
                removeToken();
                return Promise.reject(new Error('Session expired'));
            } else {
                config.headers['Authorization'] = `Bearer ${token}`;
            }
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

export default apiClient;
