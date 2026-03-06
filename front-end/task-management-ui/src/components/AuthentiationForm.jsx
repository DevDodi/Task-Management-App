import "../pages/css/Login.css";
import "../pages/css/Register.css";

export default function AuthenticationForm({ header, value, onSubmit , error, buttonText, children}) {

    return(
        <div className="authWrapper">
            <div className="container">
                <form onSubmit={onSubmit} style={{ width: "300px" }}>
                    <h2 className="header">{header}</h2>
                    {children}
                    {error && <div style={{ color: "red", marginBottom: "10px", textAlign: "center" }}>{error}</div>}
                    <button className="actionButton" type="submit">
                        {buttonText}
                    </button>
                </form>
            </div>
        </div>
    );
}