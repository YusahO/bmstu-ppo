import './Authorization.css';
import React, { useState } from 'react';
import { AlertTypes, useAlertContext } from '../../context/AlertContext';
import { apiAuth } from '../../api/mpFetch';

const RegisterAdmin = ({ onChange }) => {
  const { addAlert } = useAlertContext();
  const [credentials, setCredentials] = useState({
    username: '',
    email: '',
    password: ''
  });

  const handleChange = (e) => {
    setCredentials({
      ...credentials,
      [e.target.name]: e.target.value
    });
  };

  function handleRegister(e) {
    e.preventDefault();
    apiAuth.post('auth/registration', {
      username: credentials.username,
      email: credentials.email,
      password: credentials.password,
      isAdmin: true
    })
      .then(response => {
        if (response.status !== 200) {
          throw new Error('Registration failed');
        }
        addAlert(AlertTypes.success, 'Пользователь успешно зарегистрирован')
      })
      .catch(error => addAlert(AlertTypes.error, 'Не удалось зарегистрировать пользователя'))
  }

  return (
    <div className='form-container'>
      <h3 className='form-label'>Регистрация</h3>
      <input
        type="text"
        placeholder="Имя пользователя"
        name='username'
        value={credentials.username}
        onChange={handleChange}
        required={true}
      />
      <input
        type="email"
        placeholder="Почта"
        name='email'
        value={credentials.email}
        onChange={handleChange}
        required={true}
      />
      <input
        type="password"
        placeholder="Пароль"
        name='password'
        value={credentials.password}
        onChange={handleChange}
        required={true}
      />
      <button onClick={handleRegister}>Зарегистрировать пользователя</button>
      <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
        <label style={{ alignSelf: 'center' }}>или</label>
        <label
          style={{ textDecoration: 'underline', alignSelf: 'center', cursor: 'pointer' }}
          onClick={onChange}
        >
          Назначить существующего
        </label>
      </div>
    </div>
  );
}

const AssignExistent = ({ onChange }) => {
  const [email, setEmail] = useState('');
  const { addAlert } = useAlertContext();

  function handleSubmit() {
    apiAuth.post(`users/${email}`)
      .then(() => {
        addAlert(AlertTypes.success, 'Права пользователя повышены');
      })
      .catch(error => console.error(error));
  }

  return (
    <div className='form-container'>
      <input
        type='email'
        placeholder='Введите email пользователя'
        value={email}
        onChange={e => setEmail(e.target.value)}
        required={true}
      />
      <button onClick={handleSubmit}>Назначить админстратором</button>
      <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
        <label style={{ alignSelf: 'center' }}>или</label>
        <label
          style={{ textDecoration: 'underline', alignSelf: 'center', cursor: 'pointer' }}
          onClick={onChange}
        >
          Зарегистрировать нового
        </label>
      </div>
    </div>
  )
}

const AuthorizeAdmin = () => {
  const [isAssign, setIsAssign] = useState(true);

  function handleSwitch() {
    setIsAssign(!isAssign);
  }

  return (
    <div>
      {isAssign ?
        <AssignExistent onChange={handleSwitch} /> :
        <RegisterAdmin onChange={handleSwitch} />
      }
    </div>
  );
};

export default AuthorizeAdmin;