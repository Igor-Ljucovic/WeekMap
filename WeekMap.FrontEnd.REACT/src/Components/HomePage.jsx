import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom"; 
import { useTheme, notify } from '../Utils/utils';

function HomePage() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [username, setUsername] = useState("");
  const navigate = useNavigate();
  const { isDarkMode, setIsDarkMode } = useTheme();
  const [userSettings, setUserSettings] = useState(null);
  const userID = window.sessionStorage.getItem("id");

  useEffect(() => {
    const token = window.sessionStorage.getItem("auth_token");
    const username = window.sessionStorage.getItem("username");
    
    if (token) {
      setIsLoggedIn(true);
      setUsername(username);
      navigate("/weekmaps");
    } else {
      navigate("/login"); // redirect if the user isn't logged in
    }
  }, [navigate]);

  useEffect(() => {
    if (!userID) {
      notify.error("User not logged in.");
      return;
    }

    fetch(`api/UserSettings/${userID}`, { credentials: 'include' })
      .then(res => res.json())
      .then(data => setUserSettings(data))
      .catch(() => notify.error("Failed to load user settings."));
  }, [userID]);

  useEffect(() => {
    if (userSettings?.theme) {
      const dark = userSettings.theme === "dark";
      setIsDarkMode(dark);
      document.body.style.backgroundColor = dark ? "#1e1e1e" : "#ffffff";
    }
  }, [userSettings]);

  return (
    <div style={{ padding: "20px", color: isDarkMode ? "#fff" : "#000" }}>
    </div>
  );
}

export default HomePage;
