import axios from "axios";
import {getToken, isTokenExpired, removeToken} from "../utils/auth.jsx";

const apiClient = axios.create({
  baseURL: "http://localhost:5218",
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

export default apiClient;