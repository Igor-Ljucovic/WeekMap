import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTheme, notify, WEEKDAYS } from "../Utils/utils";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function WeekMapsPage() {
  // --- State declarations ---
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [username, setUsername] = useState("");
  const [allWeekMaps, setAllWeekMaps] = useState([]);
  const [plannedMap, setPlannedMap] = useState(null);
  const [weekMapActivities, setWeekMapActivities] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [newMap, setNewMap] = useState(null);
  const [showActivityModal, setShowActivityModal] = useState(false);
  const [allActivities, setAllActivities] = useState([]);
  const [newActivity, setNewActivity] = useState({
    activityID: null, startTime: "00:00", endTime: "00:00", activityDate: "2025-01-01",
    repeatEveryWeek: true, onMonday: false, onTuesday: false, onWednesday: false,
    onThursday: false, onFriday: false, onSaturday: false, onSunday: false,
  });

  const navigate = useNavigate();
  const { isDarkMode, setIsDarkMode } = useTheme();
  const userID = window.sessionStorage.getItem("id");

  // --- Data Fetching and Effects ---
  useEffect(() => {
    fetch("api/Activity", { credentials: "include" })
      .then(res => res.json())
      .then(data => setAllActivities(data))
      .catch(() => notify.error("Failed to load activities."));
  }, []);

  useEffect(() => {
    const token = window.sessionStorage.getItem("auth_token");
    const username = window.sessionStorage.getItem("username");
    if (token) {
      setIsLoggedIn(true);
      setUsername(username);
    } else {
      navigate("/login");
    }
  }, [navigate]);

  useEffect(() => {
    if (!userID) return;
    fetch("api/PlannedWeekMap", { credentials: "include" })
      .then(res => res.json())
      .then(data => {
        setAllWeekMaps(data);
        if (data.length > 0) {
          setPlannedMap(data[0]);
        }
      })
      .catch(() => notify.error("Failed to load your week maps."));
  }, [userID]);

  useEffect(() => {
    if (plannedMap) {
      setWeekMapActivities([]);
      fetch(`api/PlannedWeekMap/${plannedMap.plannedWeekMapID}/activities`, { credentials: "include" })
        .then(res => res.json())
        .then(data => setWeekMapActivities(data))
        .catch(() => notify.error("Failed to load activities for this map."));
    } else {
      setWeekMapActivities([]);
    }
  }, [plannedMap]);

  useEffect(() => {
    if (plannedMap?.user?.theme) {
      const dark = plannedMap.user.theme === "dark";
      setIsDarkMode(dark);
      document.body.style.backgroundColor = dark ? "#1e1e1e" : "#ffffff";
    }
  }, [plannedMap, setIsDarkMode]);

  // --- Handler Functions ---
  const loadDefaults = () => {
    fetch(`api/UserDefaultWeekMapSettings/${userID}`, { credentials: "include" })
      .then(res => {
        if (!res.ok) throw new Error("No default settings");
        return res.json();
      })
      .then(data => {
        const rawEnd = data.dayEndTime || "23:59";
        const cleanEndTime = rawEnd.startsWith("23:59") ? "24:00" : rawEnd.slice(0, 5);
        const cleanStartTime = data.dayStartTime?.slice(0, 5) || "08:00";

        setNewMap({
          ...data,
          showSaturday: !data.skipSaturday,
          showSunday: !data.skipSunday,
          dayStartTime: cleanStartTime,
          dayEndTime: cleanEndTime,
          showPlaceInPreview: data.showPlaceInPreview ?? true,
          showDescriptionInPreview: data.showDescriptionInPreview ?? true,
        });
        setShowModal(true);
      })
      .catch(() => {
        // Fallback if defaults fail to load
        setNewMap({
          weekStartDay: "Monday",
          dayStartTime: "08:00",
          dayEndTime: "24:00",
          showSaturday: true,
          showSunday: true,
          showPlaceInPreview: true,
          showDescriptionInPreview: true,
        });
        setShowModal(true);
        notify.info("Default settings not found, using fallback.");
      });
  };

  const handleAddNewMap = async () => {
    const timeFormat = /^([01]\d|2[0-3]):([0-5]\d):([0-5]\d)$/;
    const normalizeTime = (t) => (t === "24:00" ? "23:59:00" : t.length === 5 ? t + ":00" : t);
    
    const startTime = normalizeTime(newMap.dayStartTime);
    const endTime = normalizeTime(newMap.dayEndTime);

    if (!timeFormat.test(startTime) || !timeFormat.test(endTime)) {
        notify.error("Time must be in a valid format.");
        return;
    }

    const postMap = {
      userID: parseInt(userID), showSaturday: newMap.showSaturday, showSunday: newMap.showSunday,
      weekStartDay: newMap.weekStartDay, dayStartTime: startTime, dayEndTime: endTime,
      showPlaceInPreview: newMap.showPlaceInPreview, showDescriptionInPreview: newMap.showDescriptionInPreview
    };

    try {
      const res = await fetch("api/PlannedWeekMap", {
        method: "POST", headers: { "Content-Type": "application/json" },
        credentials: "include", body: JSON.stringify(postMap)
      });
      if (!res.ok) { throw new Error("Failed to add map"); }
      notify.success("Week map added!");
      setShowModal(false);
      window.location.reload();
    } catch (err) {
      notify.error("An error occurred while adding the map.");
    }
  };

  const isDateDisabled = () => {
    if (newActivity.repeatEveryWeek) return true;
    return WEEKDAYS.some(day => newActivity["on" + day]);
  };

  const isRepeatForced = () => WEEKDAYS.some(day => newActivity["on" + day]);

  const handleAddActivityToMap = async () => {

    const timeRegex = /^([01]\d|2[0-3]):([0-5]\d)$/;
    if (!timeRegex.test(newActivity.startTime) || !timeRegex.test(newActivity.endTime)) {
      notify.error("Start and end times must be in HH:MM 24-hour format.");
      return;
    }

    if (!newActivity.activityID || !newActivity.startTime || !newActivity.endTime || !newActivity.activityDate) {
      notify.error("You haven't filled out all of the required fields.");
      return;
    }

    if (newActivity.repeatEveryWeek) {
      const hasAtLeastOneDay = WEEKDAYS.some(day => newActivity["on" + day]);
      if (!hasAtLeastOneDay) {
        notify.error("At least one weekday must be selected when 'Repeat Every Week' is checked.");
        return;
      }
    }

    const payload = {
      plannedWeekMapID: plannedMap.plannedWeekMapID,
      activityID: newActivity.activityID,
      startTime: newActivity.startTime.length === 5 ? newActivity.startTime + ":00" : newActivity.startTime,
      endTime: (() => {
        const raw = newActivity.endTime;
        if (raw === "12:00" || raw === "12:00:00") return "23:59:00";
        return raw.length === 5 ? raw + ":00" : raw;
      })(),
      activityDate: newActivity.activityDate,
      repeatEveryWeek: newActivity.repeatEveryWeek,
      onMonday: newActivity.onMonday,
      onTuesday: newActivity.onTuesday,
      onWednesday: newActivity.onWednesday,
      onThursday: newActivity.onThursday,
      onFriday: newActivity.onFriday,
      onSaturday: newActivity.onSaturday,
      onSunday: newActivity.onSunday,
    };

    try {
      const res = await fetch("api/PlannedWeekMapActivity", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify(payload),
      });

      if (!res.ok) {
        const error = await res.json();
        notify.error(error?.message || "Failed to add activity.");
        return;
      }

      notify.success("Activity added to map!");
      setShowActivityModal(false);
      setNewActivity({
        activityID: null, startTime: "00:00", endTime: "00:00", activityDate: "",
        repeatEveryWeek: true, onMonday: false, onTuesday: false, onWednesday: false,
        onThursday: false, onFriday: false, onSaturday: false, onSunday: false,
      });
      const refreshed = await fetch(`api/PlannedWeekMap/${plannedMap.plannedWeekMapID}/activities`, { credentials: "include" });
      setWeekMapActivities(await refreshed.json());
    } catch (err) {
      notify.error("Unexpected error occurred.");
    }
  };

  const handleDeleteMap = () => {
    if (!plannedMap || !window.confirm("Are you sure you want to delete this week map?")) return;
    fetch(`api/PlannedWeekMap/${plannedMap.plannedWeekMapID}`, { method: "DELETE", credentials: "include" })
      .then(res => {
        if (!res.ok) throw new Error();
        notify.success("Week map deleted.");
        window.location.reload();
      })
      .catch(() => notify.error("Failed to delete map."));
  };

  if (!plannedMap) {
    return (
      <div style={{ padding: '20px', color: isDarkMode ? '#fff' : '#000' }}>
        <h2>Your Week Maps</h2>
        <button onClick={loadDefaults}>+ Add Week Map</button>
        <p style={{ marginTop: '20px' }}>Loading or no week maps found. Please add one.</p>
        <ToastContainer limit={1} theme={isDarkMode ? "dark" : "light"} />

        {/* ✅ Still render modal even when no maps exist */}
        {showModal && newMap && (
          <div style={{ position: "fixed", top: 0, left: 0, right: 0, bottom: 0, backgroundColor: "rgba(0,0,0,0.5)", display: "flex", justifyContent: "center", alignItems: "center", zIndex: 1000 }}>
            <div style={{ backgroundColor: isDarkMode ? "#222" : "#fff", padding: "20px", borderRadius: "10px", width: "400px", color: isDarkMode ? "#fff" : "#000" }}>
              <h3>New Week Map</h3>
              <div style={{ marginBottom: "10px" }}>
                <label>Week Start Day:</label>
                <select value={newMap.weekStartDay} onChange={(e) => setNewMap({ ...newMap, weekStartDay: e.target.value })} style={{ width: "100%", height: "32px" }}>
                  {WEEKDAYS.map(day => (
                    <option key={`modal-weekday-${day}`} value={day}>{day}</option>
                  ))}
                </select>
              </div>

              <div style={{ marginBottom: "10px" }}>
                <label>Day Start Time:</label>
                <select value={newMap.dayStartTime} onChange={(e) => setNewMap({ ...newMap, dayStartTime: e.target.value })} style={{ width: "100%", height: "32px" }}>
                  {Array.from({ length: 24 }, (_, i) => (
                    <option key={`modal-start-hour-${i}`} value={`${i.toString().padStart(2, "0")}:00`}>
                      {`${i.toString().padStart(2, "0")}:00`}
                    </option>
                  ))}
                </select>
              </div>

              <div style={{ marginBottom: "10px" }}>
                <label>Day End Time:</label>
                <select value={newMap.dayEndTime === "23:59" || newMap.dayEndTime === "23:59:00" ? "24:00" : newMap.dayEndTime} onChange={(e) => {
                  const v = e.target.value;
                  setNewMap({ ...newMap, dayEndTime: v === "24:00" ? "23:59" : v });
                }} style={{ width: "100%", height: "32px" }}>
                  {Array.from({ length: 24 }, (_, i) => (
                    <option key={`modal-end-hour-${i}`} value={`${i.toString().padStart(2, "0")}:00`}>
                      {`${i.toString().padStart(2, "0")}:00`}
                    </option>
                  ))}
                  <option value="24:00">24:00</option>
                </select>
              </div>

              {[{ label: "Show Saturday", key: "showSaturday" }, { label: "Show Sunday", key: "showSunday" }, { label: "Show Place In Preview", key: "showPlaceInPreview" }, { label: "Show Description In Preview", key: "showDescriptionInPreview" }].map(({ label, key }) => (
                <div key={`modal-checkbox-${key}`} style={{ marginBottom: "10px" }}>
                  <label>
                    <input type="checkbox" checked={newMap[key]} onChange={(e) => setNewMap({ ...newMap, [key]: e.target.checked })} /> {label}
                  </label>
                </div>
              ))}

              <button onClick={handleAddNewMap} style={{ marginRight: "10px" }}>Add</button>
              <button onClick={() => setShowModal(false)}>Cancel</button>
            </div>
          </div>
        )}
      </div>
    );
  }

  const { showSaturday, showSunday, dayStartTime, dayEndTime, ShowPlaceInPreview, ShowDescriptionInPreview } = plannedMap;
  const parseHour = (time) => parseInt(time.split(":")[0], 10);
  const timeToMinutes = (timeString) => {
    if(!timeString) return 0;
    const [h, m] = timeString.split(':').map(Number);
    return h * 60 + m;
  };

  const startHour = parseHour(dayStartTime);
  const isEndNearlyMidnight = dayEndTime.startsWith("23:59");
  const effectiveEndHour = isEndNearlyMidnight ? 24 : parseHour(dayEndTime);
  const hours = Array.from({ length: effectiveEndHour - startHour }, (_, i) => startHour + i);
  
  const HOUR_ROW_HEIGHT = 60;
  const HEADER_HEIGHT = 40;

  const gridContainerStyle = {
    display: "grid",
    gridTemplateColumns: `100px repeat(${WEEKDAYS.length}, 1fr)`,
    gridTemplateRows: `${HEADER_HEIGHT}px`,
    gridAutoRows: `${HOUR_ROW_HEIGHT}px`,
    color: isDarkMode ? "#fff" : "#000",
    backgroundColor: isDarkMode ? "#333" : "#f9f9f9",
    position: "relative",
  };

  return (
    <div style={{ padding: "20px", color: isDarkMode ? "#fff" : "#000" }}>
      <h2>Your Week Maps</h2>
      <div style={{ marginBottom: "10px" }}>
        <button onClick={loadDefaults} style={{ marginRight: "10px" }}>+Add Week Map</button>
        <button onClick={handleDeleteMap} style={{ backgroundColor: "#d9534f", color: "white" }}>Delete Current</button>
        <button
          onClick={() => { 
            const currentYear = new Date().getFullYear(); 
            setNewActivity({activityDate: `${currentYear}-01-01`});
            setShowActivityModal(true);
          }}
          style={{ marginRight: "10px" }}
        >
          +Add Activity to Map
        </button>
      </div>
      <div style={{ marginBottom: "10px" }}>
        <label>Choose a week map:{" "}<select value={plannedMap.plannedWeekMapID} onChange={(e) => { const selectedMap = allWeekMaps.find(m => m.plannedWeekMapID === parseInt(e.target.value)); setPlannedMap(selectedMap); }}>{allWeekMaps.map((map) => (<option key={map.plannedWeekMapID} value={map.plannedWeekMapID}>{map.dayStartTime} to {map.dayEndTime.startsWith("23:59") ? "24:00:00" : map.dayEndTime}</option>))}</select></label>
      </div>

      <div style={gridContainerStyle}>
        <div style={{ gridRow: 1, gridColumn: 1, borderBottom: `1px solid ${isDarkMode ? "#555" : "#ccc"}`, borderRight: `1px solid ${isDarkMode ? "#555" : "#ccc"}` }}></div>
        {WEEKDAYS.map((day, index) => (<div key={day} style={{ gridRow: 1, gridColumn: index + 2, padding: "8px", textAlign: "center", fontWeight: "bold", borderBottom: `1px solid ${isDarkMode ? "#555" : "#ccc"}`, borderRight: index < WEEKDAYS.length - 1 ? `1px solid ${isDarkMode ? "#555" : "#ccc"}` : 'none' }}>{day}</div>))}
        {hours.map((hour, i) => (<div key={`hour-label-${hour}`} style={{ gridRow: i + 2, gridColumn: 1, padding: "4px 6px", fontSize: "12px", textAlign: 'right', borderBottom: `1px solid ${isDarkMode ? "#555" : "#ccc"}`, borderRight: `1px solid ${isDarkMode ? "#555" : "#ccc"}` }}>{hour.toString().padStart(2, "0")}:00</div>))}
        
        {WEEKDAYS.map((day, index) => (
          <div key={`${day}-column`} style={{ gridRow: `2 / span ${hours.length}`, gridColumn: index + 2, position: 'relative', borderRight: index < WEEKDAYS.length - 1 ? `1px solid ${isDarkMode ? "#555" : "#ccc"}` : 'none' }}>
            {hours.map((_, i) => (
              <div key={`${day}-line-${i}`} style={{ position: 'absolute', top: `${i * HOUR_ROW_HEIGHT}px`, left: 0, right: 0, height: `${HOUR_ROW_HEIGHT}px`, borderBottom: `1px solid ${isDarkMode ? "#555" : "#ccc"}`, boxSizing: 'border-box' }}></div>
            ))}
            {weekMapActivities
              .filter(act => {
                if (!act["on" + day]) return false;
                if (day === 'Saturday' && !showSaturday) return false;
                if (day === 'Sunday' && !showSunday) return false;
                return true;
              })
              .map((act, actIndex) => {
                const startMinutesTotal = timeToMinutes(act.startTime);
                const endMinutesTotal = act.endTime.startsWith("23:59") ? 24 * 60 : timeToMinutes(act.endTime);
                const gridStartMinutes = startHour * 60;
                const durationMinutes = endMinutesTotal - startMinutesTotal;
                if (durationMinutes <= 0) return null;
                const top = startMinutesTotal - gridStartMinutes;
                const height = durationMinutes;
                
                const uniqueRenderKey = `${day}-${actIndex}-${act.startTime || ''}`;

                return (
                  <div key={uniqueRenderKey} style={{ position: 'absolute', top: `${top}px`, height: `${height}px`, left: '2px', right: '2px', backgroundColor: `#${act.activityCategory?.color || "888"}`, color: "#fff", padding: "6px", borderRadius: "6px", fontSize: "12px", overflow: "hidden", zIndex: 10, boxSizing: 'border-box' }}>
                    <b>{act.name}</b>
                    {ShowPlaceInPreview && act.place && <div style={{ fontSize: "11px" }}>📍 {act.place}</div>}
                    {ShowDescriptionInPreview && act.description && <div style={{ fontSize: "11px", marginTop: "2px", fontStyle: 'italic' }}>{act.description}</div>}
                    <div style={{ position: 'absolute', bottom: '4px', fontSize: '11px' }}>{act.startTime} - {act.endTime}</div>
                  </div>
                );
              })}
          </div>
        ))}
      </div>
      <ToastContainer limit={1} theme={isDarkMode ? "dark" : "light"} />
      
      {showModal && newMap && (
        <div style={{ position: "fixed", top: 0, left: 0, right: 0, bottom: 0, backgroundColor: "rgba(0,0,0,0.5)", display: "flex", justifyContent: "center", alignItems: "center", zIndex: 1000 }}>
          <div style={{ backgroundColor: isDarkMode ? "#222" : "#fff", padding: "20px", borderRadius: "10px", width: "400px", color: isDarkMode ? "#fff" : "#000" }}>
            <h3>New Week Map</h3>
            <div style={{ marginBottom: "10px" }}><label>Week Start Day:</label><select value={newMap.weekStartDay} onChange={(e) => setNewMap({ ...newMap, weekStartDay: e.target.value })} style={{ width: "100%", height: "32px" }}>{WEEKDAYS.map(day => (<option key={`modal-weekday-${day}`} value={day}>{day}</option>))}</select></div>
            <div style={{ marginBottom: "10px" }}><label>Day Start Time:</label><select value={newMap.dayStartTime} onChange={(e) => setNewMap({ ...newMap, dayStartTime: e.target.value })} style={{ width: "100%", height: "32px" }}>{Array.from({ length: 24 }, (_, i) => (<option key={`modal-start-hour-${i}`} value={`${i.toString().padStart(2, "0")}:00`}>{`${i.toString().padStart(2, "0")}:00`}</option>))}</select></div>
            <div style={{ marginBottom: "10px" }}><label>Day End Time:</label><select value={newMap.dayEndTime === "23:59" || newMap.dayEndTime === "23:59:00" ? "24:00" : newMap.dayEndTime} onChange={(e) => { const v = e.target.value; setNewMap({ ...newMap, dayEndTime: v === "24:00" ? "23:59" : v }); }} style={{ width: "100%", height: "32px" }}>{Array.from({ length: 24 }, (_, i) => (<option key={`modal-end-hour-${i}`} value={`${i.toString().padStart(2, "0")}:00`}>{`${i.toString().padStart(2, "0")}:00`}</option>))}<option value="24:00">24:00</option></select></div>
            {[{ label: "Show Saturday", key: "showSaturday" }, { label: "Show Sunday", key: "showSunday" }, { label: "Show Place In Preview", key: "showPlaceInPreview" }, { label: "Show Description In Preview", key: "showDescriptionInPreview" }].map(({ label, key }) => (<div key={`modal-checkbox-${key}`} style={{ marginBottom: "10px" }}><label><input type="checkbox" checked={newMap[key]} onChange={(e) => setNewMap({ ...newMap, [key]: e.target.checked })} /> {label}</label></div>))}
            <button onClick={handleAddNewMap} style={{ marginRight: "10px" }}>Add</button>
            <button onClick={() => setShowModal(false)}>Cancel</button>
          </div>
        </div>
      )}
      {showActivityModal && (
        <div style={{ position: "fixed", top: 0, left: 0, right: 0, bottom: 0, backgroundColor: "rgba(0,0,0,0.5)", display: "flex", justifyContent: "center", alignItems: "center", zIndex: 1000 }}>
          <div style={{ backgroundColor: isDarkMode ? "#222" : "#fff", padding: "20px", borderRadius: "10px", width: "450px", color: isDarkMode ? "#fff" : "#000" }}>
            <h3>Add Activity to This Week Map</h3>
            <div style={{ marginBottom: "10px" }}><label>Activity:</label><select value={newActivity.activityID || ""} onChange={(e) => setNewActivity({ ...newActivity, activityID: parseInt(e.target.value) })} style={{ width: "100%" }}><option value="">-- Select Activity --</option>{allActivities.map(a => (<option key={a.activityID} value={a.activityID}>{a.name}</option>))}</select></div>
            <div style={{ marginBottom: "10px" }}><label>Start Time:</label><input type="text"  placeholder="e.g. 13:30" value={newActivity.startTime} onChange={(e) => setNewActivity({ ...newActivity, startTime: e.target.value })} pattern="^([01]\d|2[0-3]):([0-5]\d)$" title="Enter time in HH:MM 24-hour format"/></div>
            <div style={{ marginBottom: "10px" }}><label>End Time:</label><input type="text"  placeholder="e.g. 16:30" value={newActivity.endTime} onChange={(e) => setNewActivity({ ...newActivity, endTime: e.target.value })} pattern="^([01]\d|2[0-3]):([0-5]\d)$" title="Enter time in HH:MM 24-hour format"/></div>
            <div style={{ marginBottom: "10px" }}><label>Date (yyyy-mm-dd):</label><input type="date" value={newActivity.activityDate} onChange={(e) => setNewActivity({ ...newActivity, activityDate: e.target.value })} style={{ width: "100%" }} disabled={isDateDisabled()}/></div>
            <div style={{ marginBottom: "10px" }}><label><input type="checkbox" checked={isRepeatForced() || newActivity.repeatEveryWeek} onChange={(e) => setNewActivity({ ...newActivity, repeatEveryWeek: e.target.checked })} disabled={isRepeatForced()}/> Repeat Every Week</label></div>
            <div style={{ display: "flex", flexWrap: "wrap", marginBottom: "10px" }}>{WEEKDAYS.map(day => (<label key={`activity-modal-day-${day}`} style={{ width: "50%" }}><input type="checkbox" checked={newActivity["on" + day]} onChange={(e) => setNewActivity({ ...newActivity, ["on" + day]: e.target.checked })}/> {day}</label>))}</div>
            <button onClick={handleAddActivityToMap} style={{ marginRight: "10px" }}>Add</button>
            <button onClick={() => setShowActivityModal(false)}>Cancel</button>
          </div>
        </div>
      )}
    </div>
  );
}

export default WeekMapsPage;