import "./css/Banner.css";
import { useNavigate } from "react-router-dom";

export default function Banner({ header, logo, name, loggedIn, onSignOut }) {

    const navigate = useNavigate();

    const handleAuthClick = () => {
        if (loggedIn) {
            onSignOut();
        }
        navigate("/login");
    };

    const authText = loggedIn === true ? "Sign Out" : "Sign In";

    return(
        <header className="banner">
            <img className="logo" src={logo} alt={name} />
            <h1 className="appName">{header}</h1>
            <h3>Projects</h3>
            <button onClick={handleAuthClick} className="authLink">
                {authText}
            </button>
        </header>
    );
}