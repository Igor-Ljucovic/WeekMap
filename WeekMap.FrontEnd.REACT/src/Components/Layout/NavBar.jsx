import { Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";

function NavBar() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [username, setUsername] = useState("");
  const [user, setUser] = useState("");
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const token = window.sessionStorage.getItem("auth_token");
    const UserID = window.sessionStorage.getItem("id");

    if (token) {
      setIsLoggedIn(true);
      fetch(`/api/users/${UserID}`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      })
        .then(async (response) => {
          if (response.ok) {
            const data = await response.json();
            setUser(data.user);
          } else {
            throw new Error("Encountered an error while logging out.");
          }
        })
        .catch((error) => {
          console.error(error);
        });

      const storedUsername = window.sessionStorage.getItem("username");
      setUsername(storedUsername || "User");
    } else {
      setIsLoggedIn(false);
    }
  }, []);

  const handleLogout = async () => {
    try {
      const response = await fetch("api/logout", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${window.sessionStorage.getItem("auth_token")}`,
        },
        body: JSON.stringify({})
      });

      if (response.ok) {
        window.sessionStorage.removeItem("auth_token");
        window.sessionStorage.removeItem("username");
        setIsLoggedIn(false);
        navigate("/login");
      } else {
        const error = await response.json();
        alert(error.Poruka || "Encountered an error while logging out.");
      }
    } catch (error) {
      console.error(error);
      alert(error);
    }
  };

  const toggleDropdown = () => {
    setDropdownOpen((prevState) => !prevState);
  };

  return (
    <nav className="navbar navbar-expand-lg" style={{
      backgroundColor: "rgb(194, 194, 194)",
      zIndex: "9999"
    }}>
      <div className="container-fluid">
        <Link className="navbar-brand d-flex align-items-center" to="/">
          <span>WeekMap</span>
        </Link>

        <div className="collapse navbar-collapse show" id="navbarNavAltMarkup">
          <div className="navbar-nav">
            <Link className="nav-link" to="/weekmaps">Week Maps</Link>
            <Link className="nav-link" to="/activity-categories">Activity Categories</Link>
            <Link className="nav-link" to="/activity-templates">Activity Templates</Link>
          </div>
          <div className="navbar-nav ms-auto">
            
              <div className="nav-item dropdown">
                <button
                  className="nav-link dropdown-toggle"
                  type="button"
                  id="navbarDropdown"
                  aria-expanded={dropdownOpen ? "true" : "false"}
                  onClick={toggleDropdown}
                  style={{ color: "#000000" }}
                >
                  {username}
                </button>
                <ul className={`dropdown-menu ${dropdownOpen ? "show" : ""}`} aria-labelledby="navbarDropdown" style={{
                  position: "absolute",
                  top: "100%",
                  right: "-12px",
                  zIndex: "9999",
                  backgroundColor: "rgb(182, 182, 182)",
                  minWidth: "130px",
                  padding: "8px 0",
                }}>
                  <li><Link className="dropdown-item" to="/settings" onClick={() => setDropdownOpen(false)}>Settings</Link></li>
                  <li><hr className="dropdown-divider" /></li>
                  <li><button className="dropdown-item" onClick={handleLogout}>Log out</button></li>
                </ul>
              </div>
          </div>
        </div>
      </div>
    </nav>
  );
}

export default NavBar;
