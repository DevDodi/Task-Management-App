import { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./css/Register.css";
import emailIcon from "../assets/email.png"
import padlockIcon from "../assets/padlock.png"
import {createUser} from "../api/users.api.js"
import AuthenticationForm from "../components/AuthentiationForm";
import Input from "../components/Input";
import toast from "react-hot-toast";


export default function Register() {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    if (password !== confirmPassword) {
      setError("Passwords do not match");
      return;
    }

    try {
      await createUser({ Email: email, Password: password });
      toast.success("Account created! Please sign in.");
      navigate("/login");
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <AuthenticationForm header="Register" onSubmit={handleSubmit} error={error} buttonText="Create Account">
        <Input
          type="email"
          style={{ backgroundColor: "white", color: "black" }}
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
        <Input
          type="password"
          style={{ backgroundColor: "#FFF4e2", color: "black" }}
          placeholder="Confirm Password"
          value={confirmPassword}
          icon = {padlockIcon}
          iconClassName = "passwordImage"
          onChange={(e) => setConfirmPassword(e.target.value)}
          required
        />
    </AuthenticationForm>
  );
}