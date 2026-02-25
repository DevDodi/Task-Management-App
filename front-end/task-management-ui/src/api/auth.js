const API_URL = import.meta.env.VITE_API_URL

export async function login(email, password) {
  const response = await fetch(`${API_URL}/auth`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ Email: email, Password: password }),
  });

  let data;
  if (response.ok) {
      data = await response.json();
  } else {
      try {
          const err = await response.json();
          throw new Error(err.Message || "Login failed");
      } catch {
          throw new Error("Login failed"); // fallback
      }
  }

  return data; // { User, AccessToken, ExpiresIn }
}