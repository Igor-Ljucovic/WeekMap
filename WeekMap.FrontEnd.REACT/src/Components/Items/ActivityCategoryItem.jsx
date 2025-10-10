
export default function ActivityCategoryItem({category, isDarkMode, onEdit, onDelete, editButtonStyle, deleteButtonStyle}) {
  return (
    <li
      style={{
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        padding: '10px',
        borderBottom: `1px solid ${isDarkMode ? '#555' : '#ccc'}`
      }}
    >
      <div style={{ display: 'flex', alignItems: 'center' }}>
        <div
          style={{
            width: '20px',
            height: '20px',
            backgroundColor: `#${category.color}`, // hexcode from the database is in "RRGGBB" format without the "#"
            borderRadius: '50%',
            marginRight: '15px'
          }}
        />
        <span>{category.name}</span>
      </div>

      <div>
        <button
          onClick={() => onEdit(category)}
          style={editButtonStyle}
        >
          Edit
        </button>
        <button
          onClick={() => onDelete(category.activityCategoryID)}
          style={deleteButtonStyle}
        >
          Delete
        </button>
      </div>
    </li>
  );
}