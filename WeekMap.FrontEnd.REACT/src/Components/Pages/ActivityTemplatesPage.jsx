import { useEffect, useState } from 'react';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { notify, useTheme } from '../../Utils/utils';

function ActivityTemplatesPage() {
  const [activities, setActivities] = useState([]);
  const [categories, setCategories] = useState([]);
  const [currentActivity, setCurrentActivity] = useState(null);
  const [isEditing, setIsEditing] = useState(false);
  const [loading, setLoading] = useState(true);

  const { isDarkMode } = useTheme();
  const userID = window.sessionStorage.getItem("id");

  const getBlankActivity = () => ({
    activityTemplateID: 0,
    name: '',
    description: '',
    location: '',
    activityCategoryID: null,
    userID: parseInt(userID, 10),
  });

  const getSelectedCategoryColor = () => {
    const selected = categories.find(c => c.activityCategoryID === currentActivity?.activityCategoryID);
    return selected ? `#${selected.color}` : (isDarkMode ? '#fff' : '#000');
  };

  useEffect(() => {
    if (!userID) {
      notify.error("User not logged in.");
      setLoading(false);
      return;
    }

    setCurrentActivity(getBlankActivity());
    fetchActivities();
    fetchCategories();
  }, [userID]);

  const fetchActivities = async () => {
    try {
      const response = await fetch(`api/ActivityTemplate`, { credentials: 'include' });
      if (!response.ok) throw new Error("Failed to fetch activity templates.");
      const data = await response.json();
      setActivities(data);
    } catch (error) {
      notify.error(error.message);
    } finally {
      setLoading(false);
    }
  };

  const fetchCategories = async () => {
    try {
      const response = await fetch(`api/ActivityCategory`, { credentials: 'include' });
      if (!response.ok) throw new Error("Failed to fetch activity categories.");
      const data = await response.json();
      setCategories(data);
    } catch (error) {
      notify.error(error.message);
    }
  };

  const handleAddActivity = async (activity) => {
    try {
      const response = await fetch('api/ActivityTemplate', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify(activity)
      });
      if (!response.ok) throw new Error('Failed to add activity template.');
      notify.success("Activity template added successfully!");
      resetForm();
      await fetchActivities();
    } catch (error) {
      notify.error(error.message);
    }
  };

  const handleUpdateActivity = async (activity) => {
    try {
      const response = await fetch(`api/ActivityTemplate/${activity.activityTemplateID}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify(activity)
      });
      if (!response.ok) throw new Error('Failed to update activity template.');
      notify.success("Activity template updated successfully!");
      resetForm();
      await fetchActivities();
    } catch (error) {
      notify.error(error.message);
    }
  };

  const handleDeleteActivity = async (id) => {
    if (!window.confirm("Are you sure you want to delete this activity template?")) return;
    try {
      const response = await fetch(`api/ActivityTemplate/${id}`, {
        method: 'DELETE',
        credentials: 'include'
      });
      if (!response.ok) throw new Error("Failed to delete activity template.");
      notify.success("Activity template deleted.");
      await fetchActivities();
    } catch (error) {
      notify.error(error.message);
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setCurrentActivity(prev => ({
      ...prev,
      [name]: name === "activityCategoryID"
        ? (value === "null" ? null : parseInt(value, 10))
        : value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!currentActivity.name) {
      notify.error("Name is required.");
      return;
    }

    if (isEditing) {
      handleUpdateActivity(currentActivity);
    } else {
      handleAddActivity(currentActivity);
    }
  };

  const handleEditClick = (activity) => {
    setIsEditing(true);
    setCurrentActivity({ ...activity });
    window.scrollTo(0, 0);
  };

  const resetForm = () => {
    setIsEditing(false);
    setCurrentActivity(getBlankActivity());
  };

  const containerStyle = {
    margin: '1%',
    backgroundColor: isDarkMode ? "#444" : "#ddd",
    color: isDarkMode ? "#fff" : "#000",
    padding: '20px',
    borderRadius: '8px',
    maxWidth: '650px'
  };

  const inputStyle = {
    padding: '8px',
    margin: '0 10px 0 5px',
    borderRadius: '4px',
    border: '1px solid #ccc',
    backgroundColor: isDarkMode ? '#555' : '#fff',
    color: isDarkMode ? '#fff' : '#000',
  };

  const buttonStyle = {
    padding: "6px 12px",
    border: "none",
    borderRadius: "5px",
    fontSize: "14px",
    marginRight: "10px"
  };

  const addButtonStyle = {
    ...buttonStyle,
    backgroundColor: "#5cb85c",
    color: "white"
  };

  const editButtonStyle = {
    ...buttonStyle,
    backgroundColor: "#0275d8",
    color: "white"
  };

  const deleteButtonStyle = {
    ...buttonStyle,
    backgroundColor: "#d9534f",
    color: "white"
  };

  const cancelButtonStyle = {
    ...buttonStyle,
    backgroundColor: "#6c757d",
    color: "white"
  };

  return (
    <div style={containerStyle}>
      <h2>Manage Activity Templates</h2>
      <hr style={{ border: `1px solid ${isDarkMode ? '#666' : '#bbb'}` }} />

      <form onSubmit={handleSubmit} style={{ marginBottom: '10px' }}>
        <h3>{isEditing ? 'Edit Activity Template' : 'Add New Activity Template'}</h3>

        <div style={{ marginBottom: '10px', display: 'flex', alignItems: 'center' }}>
          <label style={{ width: '165px' }}>Name:</label>
          <input type="text" name="name" value={currentActivity?.name || ''} onChange={handleInputChange} maxLength="50" required style={inputStyle} />
        </div>

        <div style={{ marginBottom: '10px', display: 'flex', alignItems: 'center' }}>
          <label style={{ width: '165px' }}>Description (optional):</label>
          <input type="text" name="description" value={currentActivity?.description || ''} onChange={handleInputChange} maxLength="500" style={inputStyle} />
        </div>

        <div style={{ marginBottom: '10px', display: 'flex', alignItems: 'center' }}>
          <label style={{ width: '165px' }}>Location (optional):</label>
          <input type="text" name="location" value={currentActivity?.location || ''} onChange={handleInputChange} maxLength="50" style={inputStyle} />
        </div>

        <div style={{ marginBottom: '10px', display: 'flex', alignItems: 'center' }}>
          <label style={{ width: '165px' }}>Category (optional):</label>
          <select
            name="activityCategoryID"
            value={currentActivity?.activityCategoryID ?? "null"}
            onChange={handleInputChange}
            style={{
            ...inputStyle,
            color: getSelectedCategoryColor()
            }}
            >
            <option value="null">-- No Category --</option>
            {categories.map(cat => (
            <option
              key={cat.activityCategoryID}
              value={cat.activityCategoryID}
              style={{ color: `#${cat.color}` }}
            >
            {cat.name}
            </option>
          ))}
        </select>
        </div>

        <button type="submit" style={addButtonStyle}>
          {isEditing ? 'Save' : 'Add'}
        </button>
        {isEditing && (
          <button type="button" onClick={resetForm} style={cancelButtonStyle}>Cancel</button>
        )}
      </form>

      <hr style={{ border: `1px solid ${isDarkMode ? '#666' : '#bbb'}` }} />

      <h3>Existing Activity Templates</h3>
      {activities.length > 0 ? (
        <ul style={{ listStyle: 'none', padding: 0 }}>
          {activities.map(act => (
            <li key={act.activityTemplateID} style={{
              display: 'flex',
              alignItems: 'flex-start',
              justifyContent: 'space-between',
              gap: '10px',
              flexWrap: 'wrap',
              padding: '10px',
              borderBottom: `1px solid ${isDarkMode ? '#555' : '#ccc'}`
            }}>
              <div style={{ flex: 1, minWidth: 0, wordWrap: 'break-word', whiteSpace: 'normal' }}>
              <div style={{ fontWeight: 'bold', marginBottom: '3px' }}>
                {act.name}
              </div>
                {act.description && (
                  <div style={{ marginBottom: '3px' }}>
                    📄 {act.description}
                  </div>
                )}
                {act.location && (
                  <div style={{ fontSize: '0.85em', opacity: 0.8, marginBottom: '3px' }}>
                    📍 {act.location}
                  </div>
                )}
                {act.activityCategoryID !== null && (() => {
                  const cat = categories.find(c => c.activityCategoryID === act.activityCategoryID);
                  return cat ? (
                    <div style={{ fontSize: '0.85em', color: `#${cat.color}` }}>
                      🏷️ {cat.name}
                    </div>
                  ) : null;
                })()}
              </div>
              <div>
                <button onClick={() => handleEditClick(act)} style={editButtonStyle}>Edit</button>
                <button onClick={() => handleDeleteActivity(act.activityTemplateID)} style={deleteButtonStyle}>Delete</button>
              </div>
            </li>
          ))}
        </ul>
      ) : (
        <p>No activity categories found. Add one using the form above!</p>
      )}

      <ToastContainer limit={1} theme={isDarkMode ? 'dark' : 'light'} />
    </div>
  );
}

export default ActivityTemplatesPage;
