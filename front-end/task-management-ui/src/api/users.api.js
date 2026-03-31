import { apiClient } from './httpClient.js';

export async function getUsers() {
  return apiClient.get(`/users/`);
}

export async function getUser(id) {
  return apiClient.get(`/users/${id}`);
}

export async function createUser(user) {
  return apiClient.post('/users', user);
}

export async function updateUser(id, user){
  return apiClient.put(`/users/${id}`, user)
}

export async function deleteUser(id){
  return apiClient.delete(`/users/${id}`)
}

