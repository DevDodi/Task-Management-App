import { apiClient } from '../httpClient.js';

export async function login(email, password){
  return apiClient.post('/auth/', email, password);
}
