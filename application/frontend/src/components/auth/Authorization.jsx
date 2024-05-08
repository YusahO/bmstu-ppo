// import React, { useState } from 'react';
import UpperPanel from '../../pages/UpperPanel';
import Registration from './Registration';
import Login from './Login';

import './Authorization.css';

const Authorization = () => {

  return (
    <div>
      <UpperPanel displayFunctional={false} />

      <div>
        {true ? <Registration /> : <Login />}
      </div>

    </div>
  );
};

export default Authorization;