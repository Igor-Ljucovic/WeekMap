import { useEffect, useState } from 'react';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { notify, WEEKDAYS } from '../Utils/utils';

function SettingsPage() {
  const [userSettings, setUserSettings] = useState(null);
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
      document.body.style.backgroundColor = userSettings.theme === "dark" ? "#1e1e1e" : "#ffffff";
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

  const HourSelect = ({ name, value, onChange }) => (
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
    {Array.from({ length: 24 }, (_, i) => {
      const hour = i.toString().padStart(2, "0");
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

      <div style={rowStyle}>
        <label>
          Email Updates:{" "}
          <input
            type="checkbox"
            name="emailUpdates"
            checked={userSettings.emailUpdates}
            onChange={handleUserSettingsChange}
          />
        </label>
      </div>

      <div style={rowStyle}>
        <label>
          Notifications Enabled:{" "}
          <input
            type="checkbox"
            name="notification"
            checked={userSettings.notification}
            onChange={handleUserSettingsChange}
          />
        </label>
      </div>

      <div style={rowStyle}>
        <label>
          Notification Time:{" "}
          <HourSelect
            name="notificationTime"
            value={userSettings.notificationTime}
            onChange={(e) => {
            const updated = {
              ...userSettings,
              notificationTime: e.target.value + ":00"
            };
            updateUserSettingsBackend(updated);
            } }
          />
        </label>
      </div>

      <hr style={rowStyle} />
      <h2 style={rowStyle}>Default Week Map Settings</h2>

      <div style={rowStyle}>
        <label>
          Skip Saturday:{" "}
          <input
            type="checkbox"
            name="skipSaturday"
            checked={defaultMapSettings.skipSaturday}
            onChange={handleMapSettingsChange}
          />
        </label>
      </div>

      <div style={rowStyle}>
        <label>
          Skip Sunday:{" "}
          <input
            type="checkbox"
            name="skipSunday"
            checked={defaultMapSettings.skipSunday}
            onChange={handleMapSettingsChange}
          />
        </label>
      </div>

      <div style={rowStyle}>
        <label>
          Week Start Day:{" "}
          <select name="weekStartDay" value={defaultMapSettings.weekStartDay} onChange={handleMapSettingsChange}>
            {WEEKDAYS.map(day => (
              <option key={day} value={day}>{day}</option>
            ))}
          </select>
        </label>
      </div>

      <div style={rowStyle}>
        <label>
          Day Start Time:{" "}
          <HourSelect
            name="dayStartTime"
            value={defaultMapSettings.dayStartTime}
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
            value={defaultMapSettings.dayEndTime}
            onChange={(e) => {
            const newTime = e.target.value + ":00";
            if (newTime <= defaultMapSettings.dayStartTime) {
              notify.error("End time must be later than start time.");
              return;
            }
            const updated = {
              ...defaultMapSettings,
              dayEndTime: newTime
            };
            updateMapSettingsBackend(updated);
          } }
          />
        </label>
      </div>

      <div style={rowStyle}>
        <label>
          Show Place In Preview:{" "}
          <input
            type="checkbox"
            name="showPlaceInPreview"
            checked={defaultMapSettings.showPlaceInPreview}
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
