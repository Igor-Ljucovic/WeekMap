import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTheme, notify } from '../../Utils/utils';
import WeekMapSample from "../../Images/WeekMapSample.png";

function HomePage() {
  const navigate = useNavigate();
  const { isDarkMode, setIsDarkMode } = useTheme();
  const [userSettings, setUserSettings] = useState(null);
  const userID = window.sessionStorage.getItem("id");

  useEffect(() => {
    const token = window.sessionStorage.getItem("auth_token");
    
    if (!token)
      navigate("/login"); // redirect if not logged in
    
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
    <div
      style={{
        padding: "40px",
        backgroundColor: isDarkMode ? "#121212" : "#f4f4f4",
        minHeight: "100vh",
      }}
    >
      <div
        style={{
          width: "70%",
          maxWidth: 1000,
          margin: "0 auto",
          padding: "28px 32px",
          borderRadius: "16px",
          boxShadow: "0 10px 30px rgba(0,0,0,0.15)",
          backgroundColor: isDarkMode ? "#1e1e1e" : "#ffffff",
          color: isDarkMode ? "#ffffff" : "#1a1a1a",
          lineHeight: 1.6,
        }}
      >
        <h1 style={{ margin: 0, fontSize: 28 }}>Welcome to WeekMap!</h1>
        <p style={{ marginTop: 12 }}>
          WeekMap is a simple and intuitive app for organizing time on a weekly basis.
        </p>

        <h2 style={{ marginTop: 24, fontSize: 22 }}>Categories</h2>
        <p>
          You can (but don't have to) use categories (e.g. <em>work</em>, <em>school</em>, <em>break</em>, <em>sleep</em>, <em>other</em>) to <strong>group multiple Activity templates.</strong>
        </p>

        <h2 style={{ marginTop: 24, fontSize: 22 }}>Activity Templates</h2>
        <p>
          Activity templates are used so that <strong>you don't have to enter all of the activity info every time</strong> you want to add its occurrence in the planned time for it in the WeekMap.
          They make it possible to only be required to add the info that can be different in multiple occurrences of an activity, such as the start and end time.
        </p>

        <h2 style={{ marginTop: 24, fontSize: 22 }}>WeekMap</h2>
        <p>
          WeekMap is a table consisting of all of <strong>your repetitive weekly obligations and activities.</strong>
        </p>

        <h2 style={{ marginTop: 24, fontSize: 22 }}>Activities</h2>
        <p>
          Activities are <strong>concrete occurrences of ActivityTemplates</strong> that you can add in a WeekMap. They:
        </p>
        <ul style={{ paddingLeft: 20, marginTop: 8 }}>
          <li>occur at a certain time and last for a certain period,</li>
          <li>can occur on one or more weekdays,</li>
          <li>or can occur only once (set a specific date).</li>
        </ul>
        <p style={{ marginTop: 8 }}>
          The app ensures you either fill the date field <em>or</em> select weekdays (never both) so you don’t have to worry about entering invalid combinations.
        </p>

        <h2 style={{ marginTop: 24, fontSize: 22 }}>Settings</h2>
        <p>
          You can change the <strong>theme</strong> of the app by clicking the Settings tab after clicking your username in the top-right corner.
          You can also change default WeekMap settings there so these load automatically when creating a new WeekMap without having to adjust them every time.
        </p>

        <hr style={{ margin: "28px 0", border: "none", height: 1, background: isDarkMode ? "#2b2b2b" : "#e9e9e9" }} />

        <h2 style={{ marginTop: 0, fontSize: 22 }}>Why should I use WeekMap?</h2>
        <p style={{ marginTop: 8 }}>
          <strong>A picture is worth a thousand words.</strong> Instead of writing down all your responsibilities or trying to memorize them,
          you can create a simple table in just a few minutes. With one glance, you immediately see everything you need to know.
        </p>

        <div style={{ marginTop: 16 }}>
          <div
            style={{
              padding: "16px",
              borderRadius: 12,
              background: isDarkMode ? "#242424" : "#f7f7f7",
              border: `1px solid ${isDarkMode ? "#2c2c2c" : "#e6e6e6"}`,
              marginBottom: 12,
            }}
          >
            <div style={{ fontWeight: 700, marginBottom: 6 }}>Without WeekMap</div>
            <div style={{ paddingLeft: 8 }}>
              <p style={{ margin: "6px 0" }}>
                <strong>Them:</strong> “Hey, when will you be free this week?”
              </p>
              <p style={{ margin: "6px 0" }}>
                <strong>You:</strong> “Well, it’s complicated… On Monday, as long as I get home early enough… On Tuesday and Wednesday I’m busy all day, as usual… but on Thursday…”
              </p>
              <p style={{ margin: "6px 0" }}>
                <strong>Them:</strong> “Oh, never mind then. I guess we’ll meet up next week, or some other time.”
              </p>
            </div>
          </div>

          <div
            style={{
              padding: "16px",
              borderRadius: 12,
              background: isDarkMode ? "#1f2a1f" : "#eef8ee",
              border: `1px solid ${isDarkMode ? "#2c3a2c" : "#d7ead7"}`,
            }}
          >
            <div style={{ fontWeight: 700, marginBottom: 6 }}>With WeekMap</div>
            <div style={{ paddingLeft: 8 }}>
              <p style={{ margin: "6px 0" }}>
                <strong>Them:</strong> “Hey, when will you be free this week?”
              </p>
              <p style={{ margin: "6px 0" }}>
                <strong>You:</strong>{" "}
                <span style={{ fontStyle: "italic" }}>sends WeekMap picture</span>
              </p>
              <div style={{ marginTop: 8 }}>
              <img
                src={WeekMapSample}
                alt="WeekMap sample"
                style={{
                  width: "100%",
                  maxWidth: 600,
                  display: "block",
                  borderRadius: 12,
                  border: `1px solid ${isDarkMode ? "#2c3a2c" : "#d7ead7"}`,
                  boxShadow: "0 6px 16px rgba(0,0,0,0.12)",
                }}
              />
              </div>
              <p style={{ margin: "6px 0" }}>
                <strong>Them:</strong> “Oh, perfect. So we can meet at…”
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default HomePage;
