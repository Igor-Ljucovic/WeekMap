import { toast } from 'react-toastify';
import { createContext, useContext, useState } from "react";

export const toastOptions = {
  position: "top-center",
  theme: "colored",
  hideProgressBar: true,
  autoClose: 1500
};

export const notify = {
  success: (msg) => {
    toast.dismiss();
    toast.success(msg, toastOptions);
  },
  error: (msg) => {
    toast.dismiss();
    toast.error(msg, toastOptions);
  },
  info: (msg) => {
    toast.dismiss();
    toast.info(msg, toastOptions);
  },
};

export const WEEKDAYS = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

export const ThemeContext = createContext();

export function ThemeProvider({ children }) {
  const [isDarkMode, setIsDarkMode] = useState(false);

  return (
    <ThemeContext.Provider value={{ isDarkMode, setIsDarkMode }}>
      {children}
    </ThemeContext.Provider>
  );
}

export function useTheme() {
  return useContext(ThemeContext);
}