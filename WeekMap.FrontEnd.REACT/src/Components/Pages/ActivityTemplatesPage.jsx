import { useEffect, useState } from 'react';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { notify, useTheme } from '../../Utils/utils';
import ActivityTemplateItem from '../Items/ActivityTemplateItem'; 
import { GeneralStyles } from '../../Styles/GeneralStyles';

function ActivityTemplatesPage() {
  const [activities, setActivities] = useState([]);
  const [categories, setCategories] = useState([]);
  const [currentActivity, setCurrentActivity] = useState(null);
  const [isEditing, setIsEditing] = useState(false);
  const [loading, setLoading] = useState(true);

  const { isDarkMode } = useTheme();
  const userID = window.sessionStorage.getItem("id");
  const styles = GeneralStyles(isDarkMode);

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

  return (
    <div style={styles.containerStyle}>
      <h2>Manage Activity Templates</h2>
      <hr style={{ border: `1px solid ${isDarkMode ? '#666' : '#bbb'}` }} />

      <form onSubmit={handleSubmit} style={{ marginBottom: '10px' }}>
        <h3>{isEditing ? 'Edit Activity Template' : 'Add New Activity Template'}</h3>

        <div style={{ marginBottom: '10px', display: 'flex', alignItems: 'center' }}>
          <label style={{ width: '165px' }}>Name:</label>
          <input type="text" name="name" value={currentActivity?.name || ''} onChange={handleInputChange} maxLength="50" required style={styles.inputStyle} />
        </div>

        <div style={{ marginBottom: '10px', display: 'flex', alignItems: 'center' }}>
          <label style={{ width: '165px' }}>Description (optional):</label>
          <input type="text" name="description" value={currentActivity?.description || ''} onChange={handleInputChange} maxLength="500" style={styles.inputStyle} />
        </div>

        <div style={{ marginBottom: '10px', display: 'flex', alignItems: 'center' }}>
          <label style={{ width: '165px' }}>Location (optional):</label>
          <input type="text" name="location" value={currentActivity?.location || ''} onChange={handleInputChange} maxLength="50" style={styles.inputStyle} />
        </div>

        <div style={{ marginBottom: '10px', display: 'flex', alignItems: 'center' }}>
          <label style={{ width: '165px' }}>Category (optional):</label>
          <select
            name="activityCategoryID"
            value={currentActivity?.activityCategoryID ?? "null"}
            onChange={handleInputChange}
            style={{
            ...styles.inputStyle,
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

        <button type="submit" style={styles.addButtonStyle}>
          {isEditing ? 'Save' : 'Add'}
        </button>
          {isEditing && (
            <button type="button" onClick={resetForm} style={styles.cancelButtonStyle}>Cancel</button>
          )}
      </form>

      <hr style={{ border: `1px solid ${isDarkMode ? '#666' : '#bbb'}` }} />

      <h3>Existing Activity Templates</h3>
      {activities.length > 0 ? (
        <ul style={{ listStyle: 'none', padding: 0 }}>
          {activities.map(act => (
            <ActivityTemplateItem
              key={act.activityTemplateID}
              activity={act}
              categories={categories}
              isDarkMode={isDarkMode}
              onEdit={handleEditClick}
              onDelete={handleDeleteActivity}
              editButtonStyle={styles.editButtonStyle}
              deleteButtonStyle={styles.deleteButtonStyle}
            />
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
