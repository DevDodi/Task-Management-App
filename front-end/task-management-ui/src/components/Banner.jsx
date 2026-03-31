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

    const handleProjectsClick = () => {
        navigate("/projects");
    };

    return(
        <header className="banner">
            <div className="bannerLeft">
                <img className="logo" src={logo} alt={name} />
                <h1 className="appName" onClick={handleProjectsClick}>{header}</h1>
                <h3 className="projectsLink" onClick={handleProjectsClick}>Projects</h3>
            </div>
            <button onClick={handleAuthClick} className="authLink">
                {authText}
            </button>
        </header>
    );
}