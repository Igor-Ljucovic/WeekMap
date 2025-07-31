import { useState } from 'react';
import registerPhoto from "../Images/loginPreview.png";
import { Link, useNavigate } from 'react-router-dom';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function LoginPage() {
  const [userData, setUserData] = useState({
    email: "",
    password: ""
  });

  const [loginMessage, setLoginMessage] = useState("");
  const [error, setError] = useState(false);
  const navigate = useNavigate();

  function handleInput(e) {
    setUserData({ ...userData, [e.target.name]: e.target.value });
  }

  const toastOptions = {
        position: "top-center",
        theme: "colored",
        hideProgressBar: true
    };

  const handleLogin = async (e) => {
    e.preventDefault();
    setError(false);
    toast.dismiss();
    
    try {
      const response = await fetch("api/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(userData)
      });

      const result = await response.json();

      if (response.ok) {
        window.sessionStorage.setItem("auth_token", result.access_token);
        window.sessionStorage.setItem("username", result.user.username);
        window.sessionStorage.setItem("id", result.user.userID);

        toast.success("Login successful!", toastOptions);

        if (navigator.webdriver)
            setLoginMessage("Login successful!");
        else
            setTimeout(() => navigate("/"));
        } 
        else {
            throw new Error(result.message || "Login failed");
        }
    } 
    catch (err) {
        toast.error("Login failed!", toastOptions);
        setError(true);
      
      if (navigator.webdriver)
        setLoginMessage("Login failed!");
    }
  };

  return (
    <section className="vh-100 d-flex align-items-center justify-content-center" style={{ background: 'linear-gradient(to right, #7a7a7aff, #383838ff)' }}>
      <div className="card d-flex flex-row" style={{ borderRadius: "1rem", width: "50%", height: "80vh" }}>
        <div className="col-md-6 d-none d-md-block p-0" style={{ height: "100%" }}>
          <img
            src={registerPhoto}
            alt="login form"
            className="img-fluid w-100"
            style={{ objectFit: "cover", height: "100%", borderRadius: "1rem 0 0 1rem" }}
          />
        </div>
        <div className="col-md-6 d-flex align-items-center justify-content-center p-4">
          <div className="card-body text-black text-center" style={{ width: "100%" }}>
          <h1 className="mb-4" style={{ fontSize: "min(2vw, 3vh)" }}> Welcome to WeekMap! </h1>
            <form onSubmit={handleLogin}>
              <div className="form-outline mb-4">
                <input 
                  name="email" 
                  type="email" 
                  id="email" 
                  className={`form-control form-control-lg ${error ? 'border-danger' : ''}`} 
                  style={{ fontSize: "min(1.2vw, 2vh)" }} 
                  onChange={handleInput} 
                />
                <label className="form-label" htmlFor="email" style={{ fontSize: "min(1vw, 1.8vh)" }}>Email</label>
              </div>
              <div className="form-outline mb-4">
                <input 
                  name="password" 
                  type="password" 
                  id="password" 
                  className={`form-control form-control-lg ${error ? 'border-danger' : ''}`} 
                  style={{ fontSize: "min(1.2vw, 2vh)" }}  
                  onChange={handleInput} 
                />
                <label className="form-label" htmlFor="password" style={{ fontSize: "min(1vw, 1.8vh)" }}>Password</label>
              </div>
              <div className="pt-1 mb-4">
                <button className="btn btn-dark btn-lg btn-block" type="submit" style={{ fontSize: "min(1.2vw, 2vh)" }}>
                  Log in
                </button>
              </div>
              <p className="mb-5 pb-lg-2" style={{ fontSize: "min(1vw, 1.8vh)" }}>
                Don't have an account? <Link to="/register" style={{ color: "#393f81", fontSize: "min(1vw, 1.8vh)", textDecoration: "none" }}>Register here</Link>
              </p>
              {navigator.webdriver && (<p text-center content-center> This page is being accessed by a bot.
                The login message will be available in a p tag for easier frontend testing</p>)}
              {navigator.webdriver && (<p text-center id="login-message"> {loginMessage} </p>)}
            </form>
          </div>
        </div>
      </div>
      <ToastContainer />
    </section>
  );
}

export default LoginPage;
