
export default function Input({ label, icon, iconClassName, type, placeholder, value, onChange }) {

    const containerClass = type === "email" ? "emailInput" : "passwordInput";
    const imageClass = iconClassName ?? (type === "email" ? "emailImage" : "passwordImage");

    return(
        <div className={containerClass}>
            <img className={imageClass} src={icon} alt={label} />
            <input
                type={type}
                style={{ backgroundColor: "white", color: "black" }}
                placeholder={placeholder}
                value={value}
                onChange={onChange}
                required
            />
        </div>
    );
}