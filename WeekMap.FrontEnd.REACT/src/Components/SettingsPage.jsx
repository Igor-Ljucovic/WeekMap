import { useEffect, useState } from 'react';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { notify, WEEKDAYS, useTheme } from '../Utils/utils';

function SettingsPage() {
  const [userSettings, setUserSettings] = useState(null);
  const { setIsDarkMode } = useTheme();
  const [defaultMapSettings, setDefaultMapSettings] = useState(null);
  const userID = window.sessionStorage.getItem("id");

  useEffect(() => {
    if (!userID) {
      notify.error("User not logged in.");
      return;
    }

    fetch(`api/UserSettings/${userID}`, { credentials: 'include' })
      .then(res => res.json())
      .then(data => setUserSettings(data))
      .catch(() => notify.error("Failed to load user settings."));

    fetch(`api/UserDefaultWeekMapSettings/${userID}`, { credentials: 'include' })
      .then(res => res.json())
      .then(data => setDefaultMapSettings(data))
      .catch(() => notify.error("Failed to load default week map settings."));
  }, [userID]);

  useEffect(() => {
    if (userSettings?.theme) {
      const dark = userSettings.theme === "dark";
      setIsDarkMode(dark);
      document.body.style.backgroundColor = dark ? "#1e1e1e" : "#ffffff";
    }
  }, [userSettings]);

  const updateUserSettingsBackend = (updatedSettings) => {
    setUserSettings(updatedSettings);
    fetch(`api/UserSettings/${userID}`, {
      method: "PUT",
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(updatedSettings)
    })
      .then(res => {
        if (!res.ok) throw new Error();
        notify.success("User setting saved.");
      })
      .catch(() => notify.error("Failed to save user setting."));
  };

  const updateMapSettingsBackend = (updatedSettings) => {
    setDefaultMapSettings(updatedSettings);
    fetch(`api/UserDefaultWeekMapSettings/${userID}`, {
      method: "PUT",
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(updatedSettings)
    })
      .then(res => {
        if (!res.ok) throw new Error();
        notify.success("Map setting saved.");
      })
      .catch(() => notify.error("Failed to save map setting."));
  };

  const handleUserSettingsChange = (e) => {
    const { name, type, checked, value } = e.target;
    const updated = {
      ...userSettings,
      [name]: type === "checkbox" ? checked : value
    };
    updateUserSettingsBackend(updated);
  };

  const handleMapSettingsChange = (e) => {
    const { name, type, checked, value } = e.target;
    const updated = {
      ...defaultMapSettings,
      [name]: type === "checkbox" ? checked : value
    };
    updateMapSettingsBackend(updated);
  };

  const handleThemeToggle = () => {
    const updated = {
      ...userSettings,
      theme: userSettings.theme === "light" ? "dark" : "light"
    };
    updateUserSettingsBackend(updated);
  };

  const rowStyle = { marginBottom: '10px' };

  const HourSelect = ({ name, value, onChange, startingHour, totalHours }) => (
  <select
    name={name}
    value={value.slice(0, 5)}
    onChange={onChange}
    style={{
      fontSize: "14px",
      padding: "4px 6px",
      height: "32px",
      maxHeight: "32px",
      width: "90px"
    }}
  >
    {Array.from({ length: totalHours + 1 }, (_, i) => {
      const hour = (startingHour + i).toString().padStart(2, "0");
      return (
        <option key={hour} value={`${hour}:00`}>
          {hour}:00
        </option>
      );
    })}
  </select>
);

  if (!userSettings || !defaultMapSettings) return <p>Loading...</p>;

  return (
    <div style={{
      margin: '1%',
      backgroundColor: userSettings.theme === "dark" ? "#444" : "#ddd",
      color: userSettings.theme === "dark" ? "#fff" : "#000",
      padding: '20px',
      borderRadius: '8px',
      maxWidth: '650px'
    }}>
      <h2 style={rowStyle}>User Settings</h2>

      <div style={rowStyle}>
        <label>
          Theme:{" "}
          <button onClick={handleThemeToggle}>
            {userSettings.theme === "light" ? "Switch to Dark" : "Switch to Light"}
          </button>
        </label>
      </div>

      <hr style={rowStyle} />
      <h2 style={rowStyle}>Default Week Map Settings</h2>

      <div style={rowStyle}>
        <label>
          Day Start Time:{" "}
          <HourSelect
            name="dayStartTime"
            value={defaultMapSettings.dayStartTime}
            startingHour={0}
            totalHours={23}
            onChange={(e) => {
              const newTime = e.target.value + ":00";
            if (newTime >= defaultMapSettings.dayEndTime) {
              notify.error("Start time must be earlier than end time.");
              return;
            }
            const updated = {
              ...defaultMapSettings,
              dayStartTime: newTime
            };
            updateMapSettingsBackend(updated);
            } }
          />
        </label>
      </div>

      <div style={rowStyle}>
        <label>
          Day End Time:{" "}
          <HourSelect
  name="dayEndTime"
  value={(() => {
    const [h, m, s] = defaultMapSettings.dayEndTime.split(":").map(Number);

    // Special case: show 24:00 if backend gives 23:59:00
    if (h === 23 && m === 59) return "24:00";

    const date = new Date();
    date.setHours(h, m, s, 0);
    date.setMinutes(date.getMinutes() + 1);

    const hour = date.getHours();
    const minute = date.getMinutes();

    // Show 24:00 if adding 1 min rolls into next day
    //if (hour === 0 && minute === 0) return "24:00";

    return `${hour.toString().padStart(2, "0")}:${minute
      .toString()
      .padStart(2, "0")}`;
  })()}
  startingHour={1}
  totalHours={23}
  onChange={(e) => {
    const selected = e.target.value;

    const [h, m] = selected === "24:00" ? [0, 0] : selected.split(":").map(Number);
    const date = new Date();
    date.setHours(h);
    date.setMinutes(m);
    date.setSeconds(0);
    date.setMilliseconds(0);

    // Subtract 1 minute before sending to backend if the endtime is 24:00
    date.setMinutes(date.getMinutes() - 1);

    const adjusted =
      date.getHours().toString().padStart(2, "0") +
      ":" +
      date.getMinutes().toString().padStart(2, "0") +
      ":00";

    if (adjusted <= defaultMapSettings.dayStartTime) {
      notify.error("End time must be later than start time.");
      return;
    }

    const updated = {
      ...defaultMapSettings,
      dayEndTime: adjusted
    };
    updateMapSettingsBackend(updated);
  }}
/>
      </label>
      </div>

      <div style={rowStyle}>
        <label>
          Show Location In Preview:{" "}
          <input
            type="checkbox"
            name="showLocationInPreview"
            checked={defaultMapSettings.showLocationInPreview}
            onChange={handleMapSettingsChange}
          />
        </label>
      </div>

      <div style={rowStyle}>
        <label>
          Show Description In Preview:{" "}
          <input
            type="checkbox"
            name="showDescriptionInPreview"
            checked={defaultMapSettings.showDescriptionInPreview}
            onChange={handleMapSettingsChange}
          />
        </label>
      </div>
      <ToastContainer limit={1}/>
    </div>
  );
}

export default SettingsPage;
