import React, { createContext, useState, useContext } from 'react';

const AlertContext = createContext();

export const useAlertContext = () => {
  return useContext(AlertContext);
};

export const AlertTypes = {
  info: 'info',
  success: 'success',
  warn: 'warning',
  error: 'error'
}

export const AlertProvider = ({ children }) => {
  const [alerts, setAlerts] = useState([]);

  const addAlert = (type, message) => {
    const id = Math.floor(Math.random() * 1000000);
    setAlerts([...alerts, { id, type, message }]);

    setTimeout(() => {
      removeAlertFade(id);
    }, 5000);
  };

  const removeAlert = (id) => {
    setAlerts(alerts.filter(alert => alert.id !== id));
  };

  const removeAlertFade = (id) => {
    const alertElement = document.getElementById(id);
    if (alertElement) {
      alertElement.style.transition = "opacity 0.6s"; // Apply the transition effect
      alertElement.style.opacity = '0'; // Set the opacity to 0 to trigger the transition
      setTimeout(() => {
        removeAlert(id);
      }, 600);
    }
  }

  return (
    <AlertContext.Provider value={{ alerts, addAlert, removeAlert }}>
      {children}
    </AlertContext.Provider>
  );
};