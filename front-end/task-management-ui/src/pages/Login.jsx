import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../api/auth";
import "./css/Login.css";
import emailIcon from "../assets/email.png"
import padlockIcon from "../assets/padlock.png"
import AuthenticationForm from "../components/AuthentiationForm";
import Input from "../components/Input";


export default function Login() {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  
  useEffect(() => {
    const isLoggedIn = localStorage.getItem("token");
    if (isLoggedIn) {
      navigate("/projects");
    }
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    try {
      const data = await login(email, password);
      localStorage.setItem("token", data.accessToken); // save JWT
      localStorage.setItem("userId", data.userId); // save user ID
      navigate("/projects"); // redirect after login
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <AuthenticationForm header="Login" onSubmit={handleSubmit} error={error} buttonText="Sign in">
      <Input
        type="email"
        style={{ backgroundColor: "#FFF4e2", color: "black" }}
        placeholder="Email"
        value={email}
        icon = {emailIcon}
        iconClassName = "emailImage"
        onChange={(e) => setEmail(e.target.value)}
        required
      />
      <Input
        type="password"
        style={{ backgroundColor: "#FFF4e2", color: "black" }}
        placeholder="Password"
        value={password}
        icon = {padlockIcon}
        iconClassName = "passwordImage"
        onChange={(e) => setPassword(e.target.value)}
        required
      />
      <div className="register">
        <a href="/register">Create Account?</a>
      </div>
    </AuthenticationForm>
  );
}