import { Navigate } from "react-router-dom";

export default function RequireAuth({ children }) {
    const authed = localStorage.getItem("token");

    if (!authed) {
        return <Navigate to="/login" replace />;
    }

  return children;  
}