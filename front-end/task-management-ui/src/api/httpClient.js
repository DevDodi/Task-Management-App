import axios from 'axios';
import toast from "react-hot-toast";
const API_URL = import.meta.env.VITE_API_URL ?? "http://localhost:7114/api";

export const apiClient = axios.create({
  baseURL: API_URL,
  headers: { "Content-Type": "application/json" },
});

// request interceptor - adds JWT to every request
apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

// response interceptor - handles errors globally
apiClient.interceptors.response.use(
  (response) => response.data ?? null,
  async (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem("token");
      localStorage.removeItem("userId");
      if (window.location.pathname !== "/login"){
        window.location.href = "/login";
      }
    }

    const message = error.response?.data?.Message || "Something went wrong";
    toast.error(message);
    throw new Error(message);
  }
);