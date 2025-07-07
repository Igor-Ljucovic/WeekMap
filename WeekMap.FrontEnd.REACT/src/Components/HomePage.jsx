import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom"; 

function HomePage() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [username, setUsername] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    const token = window.sessionStorage.getItem("auth_token");
    const username = window.sessionStorage.getItem("username");

    if (token) {
      setIsLoggedIn(true);
      setUsername(username);
    } else {
      navigate("/login"); // redirect if not logged in
    }
  }, [navigate]);

  return (
    <div>
    </div>
  );
}

export default HomePage;
