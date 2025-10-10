import { useEffect, useState } from 'react';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { notify, useTheme } from '../../Utils/utils';

function ActivityCategoriesPage() {
  
  const [categories, setCategories] = useState([]);
  const [currentCategory, setCurrentCategory] = useState(null);
  const [isEditing, setIsEditing] = useState(false);
  const [loading, setLoading] = useState(true);
  const [categoryMessage, setCategoryMessage] = useState("");
  
  const { isDarkMode } = useTheme();
  const userID = window.sessionStorage.getItem("id");

  const getBlankCategory = () => ({
    activityCategoryID: 0,
    name: '',
    color: 'FFFFFF',
    userID: parseInt(userID, 10)
  });

  useEffect(() => {
    if (!userID) {
      notify.error("User not logged in.");
      setLoading(false);
      return;
    }
    
    setCurrentCategory(getBlankCategory());

    fetchCategories();
  }, [userID]);

  const fetchCategories = async () => {
    try {
      const response = await fetch(`api/ActivityCategory`, { credentials: 'include' });
      if (!response.ok) {
        throw new Error('Failed to fetch categories.');
      }
      const data = await response.json();
      setCategories(data);
    } catch (error) {
      notify.error(error.message || "Could not load categories.");
    } finally {
      setLoading(false);
    }
  };

  const handleAddCategory = async (category) => {
    try {
      const response = await fetch('api/ActivityCategory', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify(category)
      });

      if (!response.ok) {
        throw new Error('Failed to add category.');
      }

      notify.success("Category added successfully!");
      if (navigator.webdriver) setCategoryMessage("Success");

      resetForm();
      await fetchCategories();
    } catch (error) {
      notify.error(error.message || "Failed to save the new category.");
      if (navigator.webdriver) setCategoryMessage("Failure");
    }
  };

  const handleUpdateCategory = async (category) => {
    try {
      const response = await fetch(`api/ActivityCategory/${category.activityCategoryID}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify(category)
      });
      if (!response.ok) {
        throw new Error('Failed to update category.');
      }
      notify.success("Category updated successfully!");
      resetForm();
      await fetchCategories(); // Refresh the list
    } catch (error) {
      notify.error(error.message || "Failed to save changes.");
    }
  };

  const handleDeleteCategory = async (id) => {
    if (!window.confirm("Are you sure you want to delete this category? This action cannot be undone.")) {
        return;
    }

    try {
      const response = await fetch(`api/ActivityCategory/${id}`, {
        method: 'DELETE',
        credentials: 'include'
      });
      if (!response.ok) {
        throw new Error('Failed to delete category.');
      }
      notify.success("Category deleted.");
      await fetchCategories(); // Refresh the list
    } catch (error) {
        notify.error(error.message || "Failed to delete the category.");
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setCurrentCategory(prev => ({ ...prev, [name]: value }));
  };

  const handleColorChange = (e) => {
    setCurrentCategory(prev => ({ ...prev, color: e.target.value.substring(1) }));
  };
  
  const handleSubmit = (e) => {
    e.preventDefault();
    if (!currentCategory.name || !currentCategory.color) {
      notify.error("Name and color are required.");
      return;
    }
    if (isEditing) {
      handleUpdateCategory(currentCategory);
    } else {
      handleAddCategory(currentCategory);
    }
  };

  const handleEditClick = (category) => {
    setIsEditing(true);
    // The color from the DB is 'RRGGBB', but the input needs '#RRGGBB'
    setCurrentCategory({ ...category }); 
    window.scrollTo(0, 0);
  };

  const resetForm = () => {
    setIsEditing(false);
    setCurrentCategory(getBlankCategory());
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
      <h2>Manage Activity Categories</h2>
      <hr style={{ border: `1px solid ${isDarkMode ? '#666' : '#bbb'}` }} />
      <form onSubmit={handleSubmit} style={{ marginBottom: '10px' }}>
        <h3>{isEditing ? 'Edit Category' : 'Add New Category'}</h3>
        <div style={{ marginBottom: '15px' }}>
          <label>
            Name:
            <input
              type="text"
              name="name"
              value={currentCategory?.name || ''}
              onChange={handleInputChange}
              maxLength="50"
              required
              style={inputStyle}
            />
          </label>
          <label>
            Color:
            <input
              type="color"
              name="color"
              value={`#${currentCategory?.color || 'FFFFFF'}`} // Color needs '#' prefix because its not in the database
              onChange={handleColorChange}
              style={{...inputStyle, padding: '0px', height: '35px', verticalAlign: 'middle'}}
            />
          </label>
        </div>
        <button type="submit" style={addButtonStyle}>{isEditing ? 'Save' : 'Add'}</button>
        {isEditing && (
          <button type="button" onClick={resetForm} style={cancelButtonStyle}>Cancel</button>
        )}
      </form>

      <hr style={{ border: `1px solid ${isDarkMode ? '#666' : '#bbb'}` }} />

      <h3>Existing Categories</h3>
      {categories.length > 0 ? (
        <ul style={{ listStyle: 'none', padding: 0 }}>
          {categories.map(cat => (
            <li key={cat.activityCategoryID} style={{
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'space-between',
              padding: '10px',
              borderBottom: `1px solid ${isDarkMode ? '#555' : '#ccc'}`
            }}>
              <div style={{ display: 'flex', alignItems: 'center' }}>
                <div style={{
                  width: '20px',
                  height: '20px',
                  backgroundColor: `#${cat.color}`,
                  borderRadius: '50%',
                  marginRight: '15px'
                }}></div>
                <span>{cat.name}</span>
              </div>
              <div>
                <button onClick={() => handleEditClick(cat)} style={editButtonStyle}>Edit</button>
                <button onClick={() => handleDeleteCategory(cat.activityCategoryID)} style={deleteButtonStyle}>Delete</button>
              </div>
            </li>
          ))}
        </ul>
      ) : (
        <p>No categories found. Add one using the form above!</p>
      )}

      <ToastContainer limit={1} theme={isDarkMode ? 'dark' : 'light'}/>
      {navigator.webdriver && (<p id="category-message" style={{ textAlign: 'center', marginTop: '20px' }}> {categoryMessage} </p> )}
    </div>
  );
}

export default ActivityCategoriesPage;