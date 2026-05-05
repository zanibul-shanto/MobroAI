import axios from 'axios';
import * as SecureStore from 'expo-secure-store';

// IMPORTANT: Replace with your actual backend server IP address.
// If using Android Emulator, use 10.0.2.2. If using physical device, use your machine's IP.
const API_BASE_URL = 'https://4xcd6lhg-5000.inc1.devtunnels.ms/';

export const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use(
  async (config) => {
    const token = await SecureStore.getItemAsync('auth_token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);
