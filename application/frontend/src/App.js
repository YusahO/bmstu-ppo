import React, { useState, useEffect } from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './App.css';
import Audiotracks from './components/audiotrack/Audiotracks';
import Layout from "./pages/Layout";

import AuthService from './AuthService';
import Login from './components/auth/Login.jsx';

function App() {

  const [currentUser, setCurrentUser] = useState(undefined);

  useEffect(() => {
    const user = AuthService.getCurrentUser();
    if (user) {
      setCurrentUser(user);
    }
  }, []);

  const handleLogin = (username, password) => {
    AuthService.login(username, password).then((user) => {
      setCurrentUser(user);
    });
  };

  // const handleLogout = () => {
  //   AuthService.logout();
  //   setCurrentUser(undefined);
  // };

  return (
    <BrowserRouter>
      <div>
        <Routes>
          <Route path="/" element={<Layout />} >
            <Route path="/audiotracks" element={<Audiotracks />} />
            <Route path="/login" element={<Login onLogin={handleLogin} />} />
          </Route>
        </Routes>
      </div>
    </BrowserRouter>
  );
  // return (
  //   <div className="App">
  //     <header className="App-header">
  //       <img src={logo} className="App-logo" alt="logo" />
  //       <p>
  //         Edit <code>src/App.js</code> and save to reload.
  //       </p>
  //       <a
  //         className="App-link"
  //         href="https://reactjs.org"
  //         target="_blank"
  //         rel="noopener noreferrer"
  //       >
  //         Learn React
  //       </a>
  //     </header>
  //   </div>
  // );
}

export default App;
