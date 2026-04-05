import { useState } from 'react'
import Login from "./pages/Login";
import Projects from "./pages/Projects";
import Register from "./pages/Register";
import ProjectDetails from "./pages/ProjectDetails";
import Banner from "./components/Banner";
import RequireAuth from "./components/RequireAuth";
import Logo from "./assets/task-list-logo.svg";
import './App.css'
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Toaster } from "react-hot-toast";

function App() {
  const [token, setToken] = useState(() => {
    const token = localStorage.getItem("token");
    return token;
  });

  const handleSignOut = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
    setToken(null);
  };

  return (
    <BrowserRouter> 
      <Banner header="Task Management App" logo={Logo} name="Task Management App" loggedIn={!!token} onSignOut={handleSignOut} />
      <Toaster position="top-center" />
      <Routes>
        <Route path="/login" element={<Login onLogin={setToken} /> } />
        <Route path="/register" element={<Register />} />
        <Route path="/projects" element={
          <RequireAuth>
            <Projects />
          </RequireAuth> } 
        />
        <Route path="/projects/:id" element={
          <RequireAuth>
            <ProjectDetails />
          </RequireAuth> } 
        />
        <Route path="*" element={<Login />} />
      </Routes>
    </BrowserRouter>    
  )
}

export default App
