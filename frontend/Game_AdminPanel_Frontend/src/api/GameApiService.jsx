import axios from 'axios';
import {getToken, isTokenExpired, removeToken, setToken} from '../utils/auth.jsx';

const apiClient = axios.create({
    baseURL: 'http://localhost:5228',
    withCredentials: true
});

apiClient.interceptors.request.use(
    (config) => {
        const token = getToken();
        if (token) {
            if (isTokenExpired(token)) {
                window.dispatchEvent(new CustomEvent('token-expired'));
                removeToken();
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

apiClient.interceptors.response.use(
    response => response,
    async error => {
        if (!error.message) {
            console.error('Network error or server not responding');
            return Promise.reject(new Error('Network error'));
        }

        const originalRequest = error.config;
        if (error.response.status === 401) {
            try {
                const response = await apiClient.post('http://localhost:5218/api/Auth/refresh-token', {});
                const newToken = response.data.data.accessToken;

                const inOneMinute = new Date(new Date().getTime() + 60 * 1000);
                setToken(newToken, 'accessToken', {secure: true, sameSite: 'Strict', expires: inOneMinute});
                apiClient.defaults.headers.common['Authorization'] = `Bearer ${newToken}`;
                error.config.headers['Authorization'] = `Bearer ${newToken}`;

                return apiClient.request(originalRequest);
            } catch (refreshError) {
                removeToken();
                return Promise.reject(refreshError);
            }
        }

        else if(error.response.status === 400){
            return Promise.reject(new Error('Session expired!'));
        }
        return Promise.reject(error);
    }
);

export default apiClient;
