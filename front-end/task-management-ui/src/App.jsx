import { useState } from 'react'
import Login from "./pages/Login";
import Projects from "./pages/Projects";
import Register from "./pages/Register";
import './App.css'
import { BrowserRouter, Routes, Route } from "react-router-dom";

function App() {
  const [count, setCount] = useState(0)

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/projects" element={<Projects />} />
        <Route path="*" element={<Login />} />
      </Routes>
    </BrowserRouter>    
  )
}

export default App
