import './Authorization.css';
import React, { useState } from 'react';
import Registration from './Registration';
import Login from './Login';
import { useNavigate } from 'react-router-dom';
import { AlertTypes, useAlertContext } from '../../context/AlertContext';

const Authorization = () => {
  const navigate = useNavigate();
  const { addAlert } = useAlertContext()
  const [isLogin, setIsLogin] = useState(true);

  function handleSwitch() {
    setIsLogin(!isLogin);
  }

  function handleNavigate() {
    addAlert(AlertTypes.success, 'Авторизация успешна')
    navigate('/');
  }

  return (
    <div>
      {isLogin ?
        <Login onChange={handleSwitch} onSuccess={handleNavigate} /> :
        <Registration onChange={handleSwitch} onSuccess={handleNavigate} />
      }
    </div>
  );
};

export default Authorization;