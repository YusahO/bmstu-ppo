import React, { useState } from 'react';
import './Login.css'
import UpperPanel from '../../pages/UpperPanel';

const Login = ({ onLogin }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = (e) => {
    e.preventDefault();
    onLogin(email, password);
  };

  return (
    <div>
      <UpperPanel displayFunctional={false} />
      <form onSubmit={handleLogin} className='form-container'>
        <h3 className='form-label'>Авторизация</h3>
        <input className='form-input'
          type="text"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
        <input className='form-input'
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        <button type="submit">Login</button>
      </form>
    </div>
  );
};

export default Login;