
export default function AuthenticationForm({ header, value, onSubmit , error, buttonText, childElements}) {

    return(
        <div className="container">
            <form onSubmit={onSubmit} style={{ width: "300px" }}>
                <h2 className="header">{header}</h2>
                {childElements}
                {error && <div style={{ color: "red", marginBottom: "10px" }}>{error}</div>}
                <button className="actionButton" type="submit">
                    {buttonText}
                </button>
            </form>
        </div>
    );
}