import { useState } from "react";

export default function RegisterPage() {
    const [form, setForm] = useState({ username: "", password: "", email: "" });
    const [message, setMessage] = useState("");

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const response = await fetch("https://localhost:7141/api/account/register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(form),
        });

        if (response.ok) {
            setMessage("Registration successful!");
        } else {
            const error = await response.json();
            setMessage("Error: " + JSON.stringify(error));
        }
    };

    return (
        <div>
            <h2>Register</h2>
            <form onSubmit={handleSubmit}>
                <input
                    type="text"
                    name="username"
                    placeholder="Username"
                    value={form.username}
                    onChange={handleChange}
                />
                <br />

                <input
                    type="password"
                    name="password"
                    placeholder="Password"
                    value={form.password}
                    onChange={handleChange}
                />
                <br />

                <input
                    type="email"
                    name="email"
                    placeholder="Email"
                    value={form.email}
                    onChange={handleChange}
                />
                <br />

                <button type="submit">Register</button>
            </form>
            <p>{message}</p>
        </div>
    );
}
