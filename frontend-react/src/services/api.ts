import axios, { type AxiosInstance } from 'axios';
import { getConfig } from '../config/env';

// In test environment, use a default URL
const API_BASE_URL = getConfig().API_URL;

interface GenerateContentResponse {
    content: string;
    credits: number;
}

const api: AxiosInstance = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

// Intercepteur pour ajouter le token JWT
api.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers = config.headers || {};
        config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
});

export const contentService = {
    async generateContent(prompt: string, type: string): Promise<GenerateContentResponse> {
        try {
            const response = await api.post('/content/generate', { prompt, type });
            return response.data;
        } catch (error) {
            throw new Error('API Error');
        }
    },

    async getCreditsBalance(): Promise<number> {
        try {
            const response = await api.get('/content/credits');
            return response.data;
        } catch (error) {
            throw new Error('Failed to fetch credits');
        }
    }
};

export const authService = {
    async login(email: string, password: string): Promise<void> {
        try {
            const response = await api.post('/auth/login', { email, password });
            localStorage.setItem('token', response.data.token);
        } catch (error) {
            throw new Error('Login failed');
        }
    },

    logout(): void {
        localStorage.removeItem('token');
    },

    isAuthenticated(): boolean {
        return !!localStorage.getItem('token');
    }
};
