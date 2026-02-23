const API_URL = import.meta.env.VITE_API_URL || "http://localhost:5000";

export async function login(email, password) {
  const response = await fetch(`${API_URL}/auth/`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password }),
  });

  if (!response.ok) {
    const err = await response.json();
    throw new Error(err.message || "Login failed");
  }

  return response.json(); // { token, user }
}