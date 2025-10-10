export default function ActivityTemplateItem({activity, categories, isDarkMode, onEdit, onDelete, editButtonStyle, deleteButtonStyle, }) {
  
    const cat = activity.activityCategoryID != null ? categories.find(c => c.activityCategoryID === activity.activityCategoryID) : null;

    return (
    <li
      style={{
        display: 'flex',
        alignItems: 'flex-start',
        justifyContent: 'space-between',
        gap: '10px',
        flexWrap: 'wrap',
        padding: '10px',
        borderBottom: `1px solid ${isDarkMode ? '#555' : '#ccc'}`
      }}
    >
        <div style={{ flex: 1, minWidth: 0, wordWrap: 'break-word', whiteSpace: 'normal' }}>
        <div style={{ fontWeight: 'bold', marginBottom: '3px' }}>
        {activity.name}
        </div>

        {activity.description && (
        <div style={{ marginBottom: '3px' }}>
            📄 {activity.description}
        </div>
        )}

        {activity.location && (
        <div style={{ fontSize: '0.85em', opacity: 0.8, marginBottom: '3px' }}>
            📍 {activity.location}
        </div>
        )}

        {cat && (
        <div style={{ fontSize: '0.85em', color: `#${cat.color}` }}>
            🏷️ {cat.name}
        </div>
        )}
        </div>

        <div>
        <button onClick={() => onEdit(activity)} style={editButtonStyle}>Edit</button>
        <button onClick={() => onDelete(activity.activityTemplateID)} style={deleteButtonStyle}>Delete</button>
        </div>
    </li>
  );
}