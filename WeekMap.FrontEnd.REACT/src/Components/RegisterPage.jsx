import { useState } from 'react';
import registerPhoto from "../Images/food-reg.jpeg";
import { Link, useNavigate } from 'react-router-dom';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function RegisterPage() {
  
    const navigate = useNavigate();
    const [registrationMessage, setRegistrationMessage] = useState("");

    const [form, setForm] = useState({ username: "", password: "", email: "" });

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const toastOptions = {
        position: "top-center",
        theme: "colored",
        hideProgressBar: true
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        toast.dismiss();
        
        if (form.password !== form.confirmPassword) {
          toast.error("Passwords do not match!", toastOptions);

            if (navigator.webdriver)
                setRegistrationMessage("Passwords do not match!");

            return;
        }
        
        const response = await fetch("api/register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(form),
        });
        
        if (response.ok) {
            toast.success("Registration successful!", toastOptions);

            if (navigator.webdriver)
                setRegistrationMessage("Registration successful!");
            else
              setTimeout(() => { navigate("/login");}, 1000);
        }
        else {
            const error = await response.json();
            
            if (error.errors) {
                const firstKey = Object.keys(error.errors)[0];
                const firstErrorMessage = error.errors[firstKey][0];
                toast.error(firstErrorMessage, toastOptions);

                if (navigator.webdriver)
                    setRegistrationMessage(firstErrorMessage);
            } 
            else {
              toast.error(error.message, toastOptions);

                if (navigator.webdriver)
                    setRegistrationMessage(error.message); 
            }
        }
    };

  return (
    <section
      className="vh-100 bg-image"
      style={{
        backgroundImage: `url(${registerPhoto})`,
        backgroundRepeat: 'no-repeat',
        backgroundSize: 'cover',
        backgroundPosition: 'center',
      }}
    >
      <div className="mask d-flex h-100 gradient-custom-3">
        <div className="container">
          <div className="row justify-content-center align-items-center h-100">
            <div className="col-12 col-md-11 col-lg-10 col-xl-9">
              <div className="card w-100" style={{ borderRadius: "15px", margin: "0 auto" }}>

                <div
                  className="card-body p-5"
                  style={{
                    backgroundColor: "#d4edda",
                    borderRadius: "15px",
                  }}
                >
                  <h2 className="text-uppercase text-center mb-5">Create an account</h2>

                  <form onSubmit={handleSubmit}>
                    <div className="row mb-4">
                      <div className="col-6">
                        <div className="form-outline">
                          <input
                            name="username"
                            type="text"
                            id="username"
                            className="form-control form-control-lg"
                            style={{ backgroundColor: "#f1f1f1", color: "black" }}
                            placeholder="Username"
                            value={form.username}
                            onChange={handleChange}
                          />
                          <label className="form-label" htmlFor="username" style={{ color: "black" }}>
                            Username
                          </label>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="form-outline">
                          <input
                            name="email"
                            type="email"
                            id="email"
                            className="form-control form-control-lg"
                            style={{ backgroundColor: "#f1f1f1", color: "black" }}
                            placeholder="Email"
                            value={form.email}
                            onChange={handleChange}
                          />
                          <label className="form-label" htmlFor="email" style={{ color: "black" }}>
                            Email
                          </label>
                        </div>
                      </div>
                    </div>
                    <div className="row mb-4">
                      <div className="col-6">
                        <div className="form-outline">
                          <input
                            name="password"
                            type="password"
                            id="password"
                            className="form-control form-control-lg"
                            style={{ backgroundColor: "#f1f1f1", color: "black" }}
                            placeholder="Password"
                            value={form.password}
                            onChange={handleChange}
                          />
                          <label className="form-label" htmlFor="password" style={{ color: "black" }}>
                            Password
                          </label>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="form-outline">
                          <input
                            name="confirmPassword"
                            type="password"
                            id="confirmPassword"
                            className="form-control form-control-lg"
                            style={{ backgroundColor: "#f1f1f1", color: "black" }}
                            placeholder="Confirm password"
                            onChange={handleChange}
                          />
                          <label className="form-label" htmlFor="confirmPassword" style={{ color: "black" }}>
                            Confirm password
                          </label>
                        </div>
                      </div>
                    </div>

                    <div className="d-flex justify-content-center mb-4">
                      <button
                        type="submit"
                        className="btn btn-lg gradient-custom-4 text-body"
                        style={{
                          padding: "15px 50px",
                          fontSize: "18px",
                          background: "linear-gradient(to right, #4caf50, #2e7d32)",
                          border: "none",
                          borderRadius: "30px",
                          color: "white",
                          boxShadow: "0 4px 10px rgba(0, 0, 0, 0.2)",
                          transition: "all 0.3s ease",
                        }}
                      >
                        Register
                      </button>
                    </div>

                    <p className="text-center text-muted mt-5">
                      Already have an account?{" "}
                      <Link to="/login">
                        <u>Log in</u>
                      </Link>
                    </p>
                  </form>
                  {navigator.webdriver && (<p text-center content-center> This page is being accessed by a bot.
                    The registration message will be available in a p tag for easier frontend testing</p>)}
                  {navigator.webdriver && (<p text-center id="registration-message"> {registrationMessage} </p>)}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <ToastContainer />
    </section>
  );
}

export default RegisterPage;