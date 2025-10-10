import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTheme, notify, WEEKDAYS } from "../../Utils/utils";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { GeneralStyles } from "../../Styles/GeneralStyles";

function WeekMapsPage() {
  const [allWeekMaps, setAllWeekMaps] = useState([]);
  const [plannedMap, setPlannedMap] = useState(null);
  const [weekMapActivities, setWeekMapActivities] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [newMap, setNewMap] = useState(null);
  const [showActivityModal, setShowActivityModal] = useState(false);
  const [allActivities, setAllActivities] = useState([]);
  const [isEditingActivity, setIsEditingActivity] = useState(false);

  const [newActivity, setNewActivity] = useState({
    activityTemplateID: null, 
    startTime: "00:00", 
    endTime: "00:00", 
    activityDate: "2025-01-01",
    repeatEveryWeek: true, 
    onMonday: false, 
    onTuesday: false, 
    onWednesday: false,
    onThursday: false, 
    onFriday: false, 
    onSaturday: false, 
    onSunday: false,
  });
  const [showEditModal, setShowEditModal] = useState(false);
  const [editMapSettings, setEditMapSettings] = useState({
    showLocationInPreview: true,
    showDescriptionInPreview: true,
  });

  const handleEditActivityClick = () => {
    if (!selectedActivity) return;

    setNewActivity({
      activityTemplateID: selectedActivity.activityTemplateID,
      startTime: selectedActivity.startTime.slice(0, 5),
      endTime: selectedActivity.endTime.slice(0, 5),
      activityDate: selectedActivity.activityDate?.slice(0, 10) ?? "2025-01-01",
      repeatEveryWeek: selectedActivity.repeatEveryWeek,
      onMonday: selectedActivity.onMonday,
      onTuesday: selectedActivity.onTuesday,
      onWednesday: selectedActivity.onWednesday,
      onThursday: selectedActivity.onThursday,
      onFriday: selectedActivity.onFriday,
      onSaturday: selectedActivity.onSaturday,
      onSunday: selectedActivity.onSunday,
      weekMapActivityID: selectedActivity.weekMapActivityID,
    });

    setIsEditingActivity(true);
    setShowViewActivityModal(false);
    setShowActivityModal(true);
  };

  const [selectedActivity, setSelectedActivity] = useState(null);
  const [showViewActivityModal, setShowViewActivityModal] = useState(false);

  const navigate = useNavigate();
  const { isDarkMode } = useTheme();
  const userID = window.sessionStorage.getItem("id");
  const styles = GeneralStyles(isDarkMode);
  const STORAGE_KEY = `weekmap:lastSelected:${userID ?? "anon"}`;
  const [selectedWeekMapID, setSelectedWeekMapID] = useState(null);

  const getWeekStartDate = () => {
    if (!plannedMap) return null;

    const today = new Date();
    const weekStartDayName = plannedMap.weekStartDay || 'Monday'; 
    const startDayIndex = WEEKDAYS.indexOf(weekStartDayName);
    const todayOurIndex = (today.getDay() + 6) % 7;
    const daysToSubtract = (todayOurIndex - startDayIndex + 7) % 7;
    const startDate = new Date(today);
    startDate.setDate(today.getDate() - daysToSubtract);
    startDate.setHours(0, 0, 0, 0);
    return startDate;
  };

  const handleSelectWeekMap = (id) => {
    const selectedMap = allWeekMaps.find(m => m.weekMapID === Number(id));
    if (!selectedMap) return;
    setPlannedMap(selectedMap);
    setSelectedWeekMapID(selectedMap.weekMapID);
    window.localStorage.setItem(STORAGE_KEY, String(selectedMap.weekMapID));
  };

  useEffect(() => {
    if (!userID) return;

    fetch("api/WeekMap", { credentials: "include" })
      .then(res => res.json())
      .then(data => {
        setAllWeekMaps(data);

        const stored = window.localStorage.getItem(STORAGE_KEY);
        const storedId = stored ? Number(stored) : null;
        const storedMatch = storedId ? data.find(m => m.weekMapID === storedId) : null;

        const toUse = storedMatch ?? data[0] ?? null;
        if (toUse) {
          setPlannedMap(toUse);
          setSelectedWeekMapID(toUse.weekMapID);
          window.localStorage.setItem(STORAGE_KEY, String(toUse.weekMapID));
        } else {
          setPlannedMap(null);
          setSelectedWeekMapID(null);
        }
      })
      .catch(() => notify.error("Failed to load your week maps."));
  }, [userID]);

  useEffect(() => {
    fetch("api/ActivityTemplate", { credentials: "include" })
      .then(res => res.json())
      .then(data => setAllActivities(data))
      .catch(() => notify.error("Failed to load activities."));
  }, []);

  useEffect(() => {
    const token = window.sessionStorage.getItem("auth_token");
    if (!token) 
      navigate("/login");
    
  }, [navigate]);

  useEffect(() => {
    if (plannedMap) {
      setWeekMapActivities([]);
      fetch(`api/WeekMap/${plannedMap.weekMapID}/activityTemplates`, { credentials: "include" })
        .then(res => res.json())
        .then(data => setWeekMapActivities(data))
        .catch(() => notify.error("Failed to load activities for this map."));
    } else {
      setWeekMapActivities([]);
    }
  }, [plannedMap]);

  const loadDefaults = () => {
    fetch(`api/UserDefaultWeekMapSettings/${userID}`, { credentials: "include" })
      .then(res => {
        if (!res.ok) throw new Error("No default settings");
        return res.json();
      })
      .then(data => {
        const rawEnd = data.dayEndTime || "23:59";

        const roundUpToNextHour = (time) => {
          const [h, m] = time.split(":").map(Number);
          if (m > 0) return `${(h + 1).toString().padStart(2, "0")}:00`;
          return `${h.toString().padStart(2, "0")}:00`;
        };

        const cleanEndTime = roundUpToNextHour(rawEnd.startsWith("23:59") ? "24:00" : rawEnd.slice(0, 5));
        const cleanStartTime = roundUpToNextHour(data.dayStartTime?.slice(0, 5) || "08:00");

        setNewMap({
          ...data,
          dayStartTime: cleanStartTime,
          dayEndTime: cleanEndTime,
          showLocationInPreview: data.showLocationInPreview ?? true,
          showDescriptionInPreview: data.showDescriptionInPreview ?? true,
        });
        setShowModal(true);
      })
      .catch(() => {
        setNewMap({
          name: "map",
          dayStartTime: "08:00",
          dayEndTime: "24:00",
          showLocationInPreview: true,
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
      userID: parseInt(userID), 
      name: newMap.name,
      dayStartTime: startTime, 
      dayEndTime: endTime,
      showLocationInPreview: newMap.showLocationInPreview, 
      showDescriptionInPreview: newMap.showDescriptionInPreview
    };

    try {
      const res = await fetch("api/WeekMap", {
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

  const handleDeleteActivity = async (weekMapActivityID) => {
    if (!weekMapActivityID || !window.confirm("Are you sure you want to delete this activity from the map?")) return;
    try {
      const res = await fetch(`api/WeekMapActivity/${weekMapActivityID}`, {
        method: "DELETE",
        credentials: "include"
      });
      if (!res.ok) throw new Error();
      notify.success("Activity removed from map.");
      setShowViewActivityModal(false);

      const refreshed = await fetch(`api/WeekMap/${plannedMap.weekMapID}/activityTemplates`, { credentials: "include" });
      setWeekMapActivities(await refreshed.json());
    } catch {
      notify.error("Failed to remove activity.");
    }
  };

  const isDateDisabled = () => {
    if (newActivity.repeatEveryWeek) return true;
    return WEEKDAYS.some(day => newActivity["on" + day]);
  };

  const handleAddActivityToMap = async () => {
    const timeRegex = /^([01]\d|2[0-3]):([0-5]\d)$/;
    if (!timeRegex.test(newActivity.startTime) || !timeRegex.test(newActivity.endTime)) {
      notify.error("Start and end times must be in HH:MM 24-hour format.");
      return;
    }

    if (!newActivity.activityTemplateID || !newActivity.startTime || !newActivity.endTime || !newActivity.activityDate) {
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
      weekMapID: plannedMap.weekMapID,
      activityTemplateID: newActivity.activityTemplateID,
      ...(isEditingActivity && { weekMapActivityID: newActivity.weekMapActivityID }),
      startTime: newActivity.startTime.length === 5 ? newActivity.startTime + ":00" : newActivity.startTime,
      endTime: newActivity.endTime.length === 5 ? newActivity.endTime + ":00" : newActivity.endTime,
      activityDate: isDateDisabled() ? null : newActivity.activityDate,
      repeatEveryWeek: isDateDisabled() ? newActivity.repeatEveryWeek : false,
      onMonday: newActivity.onMonday,
      onTuesday: newActivity.onTuesday,
      onWednesday: newActivity.onWednesday,
      onThursday: newActivity.onThursday,
      onFriday: newActivity.onFriday,
      onSaturday: newActivity.onSaturday,
      onSunday: newActivity.onSunday,
    };

    try {
      const url = isEditingActivity
        ? `api/WeekMapActivity/${newActivity.weekMapActivityID}`
        : "api/WeekMapActivity";

      const method = isEditingActivity ? "PUT" : "POST";
      console.log("Editing activity payload:", payload);
      const res = await fetch(url, {
        method,
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify(payload),
      });

      if (!res.ok) {
        const error = await res.json();
        notify.error(error?.message || "Failed to save activity.");
        return;
      }

      notify.success(isEditingActivity ? "Activity updated!" : "Activity added to map!");
      setShowActivityModal(false);
      setIsEditingActivity(false);
      setSelectedActivity(null);
      setNewActivity({
        activityTemplateID: null, startTime: "00:00", endTime: "00:00", activityDate: "",
        repeatEveryWeek: true, onMonday: false, onTuesday: false, onWednesday: false,
        onThursday: false, onFriday: false, onSaturday: false, onSunday: false,
      });

      const refreshed = await fetch(`api/WeekMap/${plannedMap.weekMapID}/activityTemplates`, { credentials: "include" });
      setWeekMapActivities(await refreshed.json());

    } catch (err) {
      notify.error("Unexpected error occurred.");
    }
  };

  const handleDeleteMap = () => {
    if (!plannedMap || !window.confirm("Are you sure you want to delete this week map?")) return;

    fetch(`api/WeekMap/${plannedMap.weekMapID}`, { method: "DELETE", credentials: "include" })
      .then(res => {
        if (!res.ok) throw new Error();
        notify.success("Week map deleted.");

        // Clear stored selection if it pointed to the deleted map
        const stored = window.localStorage.getItem(STORAGE_KEY);
        if (stored && Number(stored) === plannedMap.weekMapID) {
          window.localStorage.removeItem(STORAGE_KEY);
        }

        // Refresh maps list
        return fetch("api/WeekMap", { credentials: "include" });
      })
      .then(res => res.json())
      .then(data => {
        setAllWeekMaps(data);
        if (data.length > 0) {
          setPlannedMap(data[0]);
          setSelectedWeekMapID(data[0].weekMapID);
          window.localStorage.setItem(STORAGE_KEY, String(data[0].weekMapID));
        } else {
          setPlannedMap(null);
          setSelectedWeekMapID(null);
        }
      })
      .catch(() => notify.error("Failed to delete map."));
  };


  if (!plannedMap) {
    return (
      <div style={{ padding: '20px', color: isDarkMode ? '#fff' : '#000' }}>
        
        <h2>Your Week Maps</h2>
        <button onClick={loadDefaults} style={styles.addButtonStyle}>
          Add Week Map
        </button>
        <p style={{ marginTop: '20px' }}>Loading or no week maps found. Please add one.</p>
        <ToastContainer limit={1} theme={isDarkMode ? "dark" : "light"} />
        
        {/* Still render modal even when no maps exist */}
        {showModal && newMap && (
          <div style={{ position: "fixed", top: 0, left: 0, right: 0, bottom: 0, backgroundColor: "rgba(0,0,0,0.5)", display: "flex", justifyContent: "center", alignItems: "center", zIndex: 1000 }}>
            <div style={{ backgroundColor: isDarkMode ? "#222" : "#fff", padding: "20px", borderRadius: "10px", width: "400px", color: isDarkMode ? "#fff" : "#000" }}>
              <h3>New Week Map</h3>
              <div style={{ marginBottom: "10px" }}>
                <div style={{ marginBottom: "10px" }}>
                  <label>Name:</label>
                  <input
                    type="text"
                    value={newMap.name}
                    onChange={(e) => setNewMap({ ...newMap, name: e.target.value })}
                    style={{ width: "100%", height: "32px" }}
                    maxLength={50}
                    required
                  />
                </div>
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
                  {Array.from({ length: 24 }, (_, i) => {
                    const displayHour = (i + 1) % 24;
                    return (
                      <option key={`hour-${displayHour}`} value={`${displayHour.toString().padStart(2, "0")}:00`}>
                        {`${displayHour.toString().padStart(2, "0")}:00`}
                      </option>
                    );
                  })}
                  <option value="24:00">24:00</option>
                </select>
              </div>

              {[{ label: "Show Location In Preview", key: "showLocationInPreview" }, { label: "Show Description In Preview", key: "showDescriptionInPreview" }].map(({ label, key }) => (
                <div key={`modal-checkbox-${key}`} style={{ marginBottom: "10px" }}>
                  <label>
                    <input type="checkbox" checked={newMap[key]} onChange={(e) => setNewMap({ ...newMap, [key]: e.target.checked })} /> {label}
                  </label>
                </div>
              ))}

              <button onClick={handleAddNewMap} style={isEditingActivity ? styles.editButtonStyle : styles.addButtonStyle}>
                {isEditingActivity ? "Edit" : "Add"}
              </button>
              <button onClick={() => setShowModal(false)} style={styles.cancelButtonStyle}>
                Cancel
              </button>
            </div>
          </div>
        )}
      </div>
    );
  }

  const { dayStartTime, dayEndTime } = plannedMap;
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

    const now = new Date();
    const currentDay = WEEKDAYS[now.getDay() === 0 ? 6 : now.getDay() - 1];
    const currentMinutes = now.getHours() * 60 + now.getMinutes();

    const startMinutes = timeToMinutes(dayStartTime);
    const endMinutes = dayEndTime.startsWith("23:59") ? 1440 : timeToMinutes(dayEndTime);
    const isCurrentInRange = currentMinutes >= startMinutes && currentMinutes <= endMinutes;
    const offsetTop = isCurrentInRange ? (currentMinutes - startMinutes) : null;

  return (
    <div style={{ padding: "20px", color: isDarkMode ? "#fff" : "#000" }}>
      <h2>Your Week Maps</h2>
      
      <div style={{ marginBottom: "10px" }}>
        <button onClick={loadDefaults} style={styles.addButtonStyle}>
          Add Week Map
        </button>
        <button
          onClick={() => {
            if (plannedMap) {
              setEditMapSettings({
                showLocationInPreview: plannedMap.showLocationInPreview,
                showDescriptionInPreview: plannedMap.showDescriptionInPreview
              });
              setShowEditModal(true);
            }
          }}
          style={styles.editButtonStyle}
        >
          Week Map Settings
        </button>
        <button onClick={handleDeleteMap} style={styles.deleteButtonStyle}>
          Delete Current Week Map
        </button>
        <button
          onClick={() => {
            const currentYear = new Date().getFullYear();
            setNewActivity(prev => ({ ...prev, activityDate: `${currentYear}-01-01` }));
            setShowActivityModal(true);
          }}
          style={styles.addButtonStyle}
        >
          Add Activity to Map
        </button>
      </div>
      {weekMapActivities.some(act => act.activityDate) && (
      <div style={{ marginBottom: "15px" }}>
        <label style={{ marginRight: "10px" }}>One-time activities with a date:</label>
        <select
          onChange={(e) => {
            const selectedID = parseInt(e.target.value);
            const activity = weekMapActivities.find(act => act.weekMapActivityID === selectedID);
            if (activity) {
              setSelectedActivity(activity);
              setShowViewActivityModal(true);
            }
          }}
          style={{ padding: "6px", fontSize: "14px", width: "300px" }}
          defaultValue=""
        >
          <option value="" disabled>Select an activity</option>
          {weekMapActivities
            .filter(act => act.activityDate)
            .sort((a, b) => a.activityDate.localeCompare(b.activityDate))
            .map(act => (
              <option key={act.weekMapActivityID} value={act.weekMapActivityID}>
                {`${act.activityTemplate?.name || 'Unnamed'} (${act.activityDate.slice(0, 10)})`}
              </option>
            ))}
          </select>
        </div>
      )}
      <div style={{ marginBottom: "10px" }}>
        <label>
          Current week map:{" "}
          <select
            value={selectedWeekMapID ?? ""}
            onChange={(e) => handleSelectWeekMap(e.target.value)}
          >
            {allWeekMaps.map((map) => (
              <option key={map.weekMapID} value={map.weekMapID}>
                {map.name}
              </option>
            ))}
          </select>
        </label>
      </div>

      <div style={gridContainerStyle}>
        <div style={{ gridRow: 1, gridColumn: 1, borderBottom: `1px solid ${isDarkMode ? "#555" : "#ccc"}`, borderRight: `1px solid ${isDarkMode ? "#555" : "#ccc"}` }}></div>
        {WEEKDAYS.map((day, index) => (<div key={day} style={{ gridRow: 1, gridColumn: index + 2, padding: "8px", textAlign: "center", fontWeight: "bold", borderBottom: `1px solid ${isDarkMode ? "#555" : "#ccc"}`, borderRight: index < WEEKDAYS.length - 1 ? `1px solid ${isDarkMode ? "#555" : "#ccc"}` : 'none' }}>{day}</div>))}
        {hours.map((hour, i) => (<div key={`hour-label-${hour}`} style={{ gridRow: i + 2, gridColumn: 1, padding: "4px 6px", fontSize: "12px", textAlign: 'right', borderBottom: `1px solid ${isDarkMode ? "#555" : "#ccc"}`, borderRight: `1px solid ${isDarkMode ? "#555" : "#ccc"}` }}>{hour.toString().padStart(2, "0")}:00</div>))}
        
        {WEEKDAYS.map((day, index) => (
          <div key={`${day}-column`} style={{ gridRow: `2 / span ${hours.length}`, gridColumn: index + 2, position: 'relative', borderRight: index < WEEKDAYS.length - 1 ? `1px solid ${isDarkMode ? "#555" : "#ccc"}` : 'none' }}>
            {isCurrentInRange && day === currentDay && (
              <div style={{
                position: 'absolute',
                top: `${offsetTop}px`,
                left: 0,
                width: "100%",
                height: "2px",
                backgroundColor: "yellow",
                zIndex: 20
              }}></div>
            )}
            {hours.map((_, i) => (
              <div key={`${day}-line-${i}`} style={{ position: 'absolute', top: `${i * HOUR_ROW_HEIGHT}px`, left: 0, right: 0, height: `${HOUR_ROW_HEIGHT}px`, borderBottom: `1px solid ${isDarkMode ? "#555" : "#ccc"}`, boxSizing: 'border-box' }}></div>
            ))}
            {weekMapActivities
              .filter(act => {
                const isRepeating = act.repeatEveryWeek && act["on" + day];
                if (isRepeating) return true;

                if (!act.activityDate) return false;

                const actDate = new Date(act.activityDate);
                const weekStart = getWeekStartDate();
                if (!weekStart) return false;

                const columnDate = new Date(weekStart);
                columnDate.setDate(columnDate.getDate() + WEEKDAYS.indexOf(day));

                return actDate.toDateString() === columnDate.toDateString();
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
                  <div
                    key={uniqueRenderKey}
                    onClick={() => {
                      setSelectedActivity(act);
                      setShowViewActivityModal(true);
                    }}
                    style={{
                      cursor: "pointer",
                      position: 'absolute',
                      top: `${top}px`,
                      height: `${height}px`,
                      left: '2px',
                      right: '2px',
                      backgroundColor: `#${act.activityTemplate?.activityCategory?.color || "888"}`,
                      color: "#fff",
                      padding: "6px",
                      borderRadius: "6px",
                      fontSize: "14px",
                      overflow: "hidden",
                      zIndex: 10,
                      boxSizing: 'border-box'
                    }}
                  >
                    <div style={{ fontWeight: "bold" }}>{act.activityTemplate?.name || "Unnamed"}</div>

                    {plannedMap.showLocationInPreview && act.activityTemplate?.location && (
                      <div style={{ fontSize: "11px", marginTop: "2px" }}>
                        📍 {act.activityTemplate.location}
                      </div>
                    )}

                    {plannedMap.showDescriptionInPreview && act.activityTemplate?.description && (
                      <div style={{
                        fontSize: "11px",
                        fontStyle: "italic",
                        marginTop: "2px",
                        whiteSpace: "pre-wrap",
                        overflowWrap: "break-word",
                        wordBreak: "break-word"
                      }}>
                        {act.activityTemplate.description}
                      </div>
                    )}
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
            <div style={{ marginBottom: "10px" }}>
              <label>Name:</label>
              <input
                type="text"
                value={newMap.name}
                onChange={(e) => setNewMap({ ...newMap, name: e.target.value })}
                placeholder="Enter week map name"
                style={{ width: "100%", height: "32px" }}
                maxLength={50}
                required
              />
            </div>
            <div style={{ marginBottom: "10px" }}><label>Day Start Time:</label><select value={newMap.dayStartTime} onChange={(e) => setNewMap({ ...newMap, dayStartTime: e.target.value })} style={{ width: "100%", height: "32px" }}>{Array.from({ length: 24 }, (_, i) => (<option key={`modal-start-hour-${i}`} value={`${i.toString().padStart(2, "0")}:00`}>{`${i.toString().padStart(2, "0")}:00`}</option>))}</select></div>
            <div style={{ marginBottom: "10px" }}><label>Day End Time:</label><select value={newMap.dayEndTime === "23:59" || newMap.dayEndTime === "23:59:00" ? "24:00" : newMap.dayEndTime} onChange={(e) => { const v = e.target.value; setNewMap({ ...newMap, dayEndTime: v === "24:00" ? "23:59" : v }); }} style={{ width: "100%", height: "32px" }}>{Array.from({ length: 24 }, (_, i) => (<option key={`modal-end-hour-${i}`} value={`${i.toString().padStart(2, "0")}:00`}>{`${i.toString().padStart(2, "0")}:00`}</option>))}<option value="24:00">24:00</option></select></div>
            {[{ label: "Show Location In Preview", key: "showLocationInPreview" }, { label: "Show Description In Preview", key: "showDescriptionInPreview" }].map(({ label, key }) => (<div key={`modal-checkbox-${key}`} style={{ marginBottom: "10px" }}><label><input type="checkbox" checked={newMap[key]} onChange={(e) => setNewMap({ ...newMap, [key]: e.target.checked })} /> {label}</label></div>))}
            <button onClick={handleAddNewMap} style={styles.addButtonStyle}>Add</button>
            <button onClick={() => setShowModal(false)} style={styles.cancelButtonStyle}>Cancel</button>
          </div>
        </div>
      )}
      {showEditModal && (
        <div style={{
          position: "fixed", top: 0, left: 0, right: 0, bottom: 0,
          backgroundColor: "rgba(0,0,0,0.5)", display: "flex",
          justifyContent: "center", alignItems: "center", zIndex: 1000
        }}>
          <div style={{
            backgroundColor: isDarkMode ? "#222" : "#fff",
            padding: "20px", borderRadius: "10px", width: "400px",
            color: isDarkMode ? "#fff" : "#000"
          }}>
            <h3>Edit Preview Settings</h3>

            <div style={{ marginBottom: "10px" }}>
              <label>
                <input
                  type="checkbox"
                  checked={editMapSettings.showLocationInPreview}
                  onChange={(e) =>
                    setEditMapSettings(prev => ({ ...prev, showLocationInPreview: e.target.checked }))
                  }
                />{" "}
                Show Location In Preview
              </label>
            </div>

            <div style={{ marginBottom: "10px" }}>
              <label>
                <input
                  type="checkbox"
                  checked={editMapSettings.showDescriptionInPreview}
                  onChange={(e) =>
                    setEditMapSettings(prev => ({ ...prev, showDescriptionInPreview: e.target.checked }))
                  }
                />{" "}
                Show Description In Preview
              </label>
            </div>

            <div style={{ marginTop: "15px" }}>
              <button
                onClick={async () => {
                  try {
                    const response = await fetch(`api/WeekMap/${plannedMap.weekMapID}`, {
                      method: "PUT",
                      headers: { "Content-Type": "application/json" },
                      credentials: "include",
                      body: JSON.stringify({
                        ...plannedMap,
                        showLocationInPreview: editMapSettings.showLocationInPreview,
                        showDescriptionInPreview: editMapSettings.showDescriptionInPreview
                      })
                    });

                    if (!response.ok) throw new Error("Failed to update week map.");

                    notify.success("Week map updated.");
                    const refreshed = await fetch(`api/WeekMap`, { credentials: "include" });
                    const updatedMaps = await refreshed.json();
                    setAllWeekMaps(updatedMaps);
                    const updatedSelected = updatedMaps.find(m => m.weekMapID === plannedMap.weekMapID);
                    setPlannedMap(updatedSelected);
                    setShowEditModal(false);
                  } catch {
                    notify.error("Error updating week map.");
                  }
                }}
                style={styles.editButtonStyle}
              >
                Save
              </button>
              <button onClick={() => setShowEditModal(false)} style={styles.cancelButtonStyle}>Cancel</button>
            </div>
          </div>
        </div>
      )}
      {showActivityModal && (
        <div style={{ position: "fixed", top: 0, left: 0, right: 0, bottom: 0, backgroundColor: "rgba(0,0,0,0.5)", display: "flex", justifyContent: "center", alignItems: "center", zIndex: 1000 }}>
          <div style={{ backgroundColor: isDarkMode ? "#222" : "#fff", padding: "20px", borderRadius: "10px", width: "450px", color: isDarkMode ? "#fff" : "#000" }}>
            <h3>{isEditingActivity ? "Edit Activity" : "Add Activity to This Week Map"}</h3>
            <div style={{ marginBottom: "10px" }}><label>Activity:</label>
            <select
              value={newActivity.activityTemplateID || ""}
              onChange={(e) => setNewActivity({ ...newActivity, activityTemplateID: parseInt(e.target.value) })}
              style={{ width: "100%" }}
            >
              <option value="">-- Select Activity --</option>
              {allActivities.map(a => {
                const color = a.activityCategory?.color ? `#${a.activityCategory.color}` : "inherit";
                return (
                  <option key={a.activityTemplateID} value={a.activityTemplateID} style={{ color }}>
                    {a.name}
                  </option>
                );
              })}
            </select>
            </div>
            <div style={{ marginBottom: "10px" }}>
            <label style={{ display: "block" }}>Start Time:</label>
            <input
              type="text"
              placeholder="e.g. 13:30"
              value={newActivity.startTime}
              onChange={(e) => setNewActivity({ ...newActivity, startTime: e.target.value })}
              pattern="^([01]\d|2[0-3]):([0-5]\d)$"
              title="Enter time in HH:MM 24-hour format"
              style={{ width: "100%" }}
            />
          </div>

          <div style={{ marginBottom: "10px" }}>
          <label style={{ display: "block" }}>End Time:</label>
          <input
            type="text"
            placeholder="e.g. 16:30"
            value={newActivity.endTime}
            onChange={(e) => setNewActivity({ ...newActivity, endTime: e.target.value })}
            pattern="^([01]\d|2[0-3]):([0-5]\d)$"
            title="Enter time in HH:MM 24-hour format"
            style={{ width: "100%" }}
          />
          </div>

          <div style={{ marginBottom: "10px" }}>
            <label>Date (yyyy-mm-dd):</label>
            <input
              type="date"
              value={newActivity.activityDate}
              onChange={(e) => setNewActivity({ ...newActivity, activityDate: e.target.value })}
              style={{ width: "100%" }}
              disabled={isDateDisabled()}
            />
          </div>

          <div style={{ marginBottom: "10px" }}>
            <label>
              <input
                type="checkbox"
                checked={newActivity.repeatEveryWeek}
                onChange={(e) => {
                  const repeat = e.target.checked;
                  setNewActivity({
                    ...newActivity,
                    repeatEveryWeek: repeat,
                    ...(repeat === false && Object.fromEntries(WEEKDAYS.map(day => [`on${day}`, false])))
                  });
                }}
              />
              {" "}Repeat Every Week
            </label>
          </div>

          <div style={{ display: "flex", flexWrap: "wrap", marginBottom: "10px" }}>
            {WEEKDAYS.map(day => (
              <label key={`activity-modal-day-${day}`} style={{ width: "50%" }}>
                <input
                  type="checkbox"
                  checked={newActivity["on" + day]}
                  disabled={!newActivity.repeatEveryWeek}
                  onChange={(e) => {
                    setNewActivity({ ...newActivity, ["on" + day]: e.target.checked });
                  }}
                />
                {" "}{day}
              </label>
            ))}
          </div>
          <button onClick={handleAddActivityToMap} style= {isEditingActivity ? styles.editButtonStyle : styles.addButtonStyle}>
            {isEditingActivity ? "Save" : "Add"}
          </button>
          <button
            onClick={() => {
              setShowActivityModal(false);
              setIsEditingActivity(false);
              setSelectedActivity(null);
              setNewActivity({
                activityTemplateID: null,
                startTime: "00:00",
                endTime: "00:00",
                activityDate: "2025-01-01",
                repeatEveryWeek: true,
                onMonday: false,
                onTuesday: false,
                onWednesday: false,
                onThursday: false,
                onFriday: false,
                onSaturday: false,
                onSunday: false,
              });
            }}
            style={styles.cancelButtonStyle}
          >
            Cancel
          </button>
          </div>
        </div>
      )}
      {showViewActivityModal && selectedActivity && (
        <div style={{
          position: "fixed", top: 0, left: 0, right: 0, bottom: 0,
          backgroundColor: "rgba(0,0,0,0.5)", display: "flex",
          justifyContent: "center", alignItems: "center", zIndex: 1000
        }}>
          <div style={{
            backgroundColor: isDarkMode ? "#222" : "#fff",
            padding: "20px", borderRadius: "10px", width: "400px",
            color: isDarkMode ? "#fff" : "#000"
          }}>
            <h3>Activity Details</h3>
            <p><strong>Name:</strong> {selectedActivity.activityTemplate?.name || "Unnamed"}</p>
            <p><strong>Start:</strong> {selectedActivity.startTime}</p>
            <p><strong>End:</strong> {selectedActivity.endTime}</p>
            {selectedActivity.activityTemplate.description && (
              <p style={{ wordWrap: "break-word", overflowWrap: "break-word", whiteSpace: "pre-wrap" }}>
                <strong>Description:</strong> {selectedActivity.activityTemplate.description}
              </p>
            )}
            {selectedActivity.activityTemplate.location && (
              <p><strong>Location:</strong> {selectedActivity.activityTemplate.location}</p>
            )}
            {selectedActivity.activityDate && (
              <p><strong>Date:</strong> {selectedActivity.activityDate?.slice(0, 10)}</p>
            )}
            {WEEKDAYS.some(day => selectedActivity["on" + day]) && (
              <p><strong>Repeats on:</strong> {
                WEEKDAYS.filter(day => selectedActivity["on" + day]).join(", ")
              }</p>
            )}
            <div style={{ marginTop: "15px" }}>
              <button onClick={handleEditActivityClick} style={styles.editButtonStyle}>
                Edit
              </button>
              <button onClick={() => handleDeleteActivity(selectedActivity.weekMapActivityID)} style={styles.deleteButtonStyle}>
                Delete
              </button>
              <button onClick={() => setShowViewActivityModal(false)} style={styles.cancelButtonStyle}>
                Cancel
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default WeekMapsPage;