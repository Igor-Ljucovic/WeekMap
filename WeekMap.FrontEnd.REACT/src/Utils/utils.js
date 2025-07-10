import { toast } from 'react-toastify';

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

export const DARKMODE = false;