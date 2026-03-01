import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../api/auth";
import "./css/Login.css";
import emailIcon from "../assets/email.png"
import padlockIcon from "../assets/padlock.png"


export default function Login() {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    try {
      const data = await login(email, password);
      localStorage.setItem("token", data.AccessToken); // save JWT
      navigate("/projects"); // redirect after login
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className="container">
      <form onSubmit={handleSubmit} style={{ width: "300px" }}>
        <h2 className="header">Login</h2>
        <div className="emailInput">
          <img className = "emailImage" src={emailIcon}></img>
          <input
            type="email"
            style={{ backgroundColor: "white", color: "black" }}
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="passwordInput">
          <img className = "passwordImage" src={padlockIcon}></img>
          <input
            type="password"
            style={{ backgroundColor: "white", color: "black" }}
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <div className="register">
          <a href="/register">Create Account</a>
        </div>
        {error && <div style={{ color: "red", marginBottom: "10px" }}>{error}</div>}
        <button className = "loginButton" type="submit">
          Sign in
        </button>
      </form>
    </div>
  );
}