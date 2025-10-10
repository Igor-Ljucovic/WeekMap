export const GeneralStyles = (isDarkMode) => {
  const containerStyle = {
    margin: "1%",
    backgroundColor: isDarkMode ? "#444" : "#ddd",
    color: isDarkMode ? "#fff" : "#000",
    padding: "20px",
    borderRadius: "8px",
    maxWidth: "650px",
  };

  const inputStyle = {
    padding: "8px",
    margin: "0 10px 0 5px",
    borderRadius: "4px",
    border: "1px solid #ccc",
    backgroundColor: isDarkMode ? "#555" : "#fff",
    color: isDarkMode ? "#fff" : "#000",
  };

  const buttonStyle = {
    padding: "6px 12px",
    border: "none",
    borderRadius: "5px",
    fontSize: "14px",
    marginRight: "10px",
  };

  const addButtonStyle = {
    ...buttonStyle,
    backgroundColor: "#5cb85c",
    color: "white",
  };

  const editButtonStyle = {
    ...buttonStyle,
    backgroundColor: "#0275d8",
    color: "white",
  };

  const deleteButtonStyle = {
    ...buttonStyle,
    backgroundColor: "#d9534f",
    color: "white",
  };

  const cancelButtonStyle = {
    ...buttonStyle,
    backgroundColor: "#6c757d",
    color: "white",
  };

  return {
    containerStyle,
    inputStyle,
    addButtonStyle,
    editButtonStyle,
    deleteButtonStyle,
    cancelButtonStyle,
  };
};
