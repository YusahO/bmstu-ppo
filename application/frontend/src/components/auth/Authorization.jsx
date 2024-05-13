import React, { useState } from 'react';
import Registration from './Registration';
import Login from './Login';

import './Authorization.css';
import { useNavigate } from 'react-router-dom';

const Authorization = () => {

  const navigate = useNavigate();
  const [isLogin, setIsLogin] = useState(true);

  function handleSwitch() {
    setIsLogin(!isLogin);
  }

  function handleNavigate() {
    navigate('/');
  }

  return (
    <div>
      <div>
        {isLogin ?
          <Registration onChange={handleSwitch} onSuccess={handleNavigate} /> :
          <Login onChange={handleSwitch} onSuccess={handleNavigate} />
        }
      </div>

    </div>
  );
};

export default Authorization;