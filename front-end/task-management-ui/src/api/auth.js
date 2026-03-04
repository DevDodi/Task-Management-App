import { apiClient } from './httpClient.js';

export async function login(email, password){
  return apiClient.post('/auth/', { Email: email, Password: password });
}
