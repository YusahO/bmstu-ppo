import React, { useState, useEffect } from 'react';
import { useCookies } from 'react-cookie';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import Home from "./pages/Home.jsx";
import './App.css';

import AuthService from './AuthService';
import Authorization from './components/auth/Authorization.jsx';

function App() {

  const handleLogin = (username, password) => {
    AuthService.login(username, password).then((user) => {
      setCurrentUser(user);
    });
  };

  // const handleLogin = (username, password) => {
  //   AuthService.login(username, password).then((user) => {
  //     setCurrentUser(user);
  //   });
  // };

  const RequireAuth = (elem) => {
    const [cookie, getter, setter] = useCookies('RefreshToken');
    return !!cookie.RefreshToken ? elem : <Navigate to='/auth' />
  }

  return (
    <BrowserRouter>
      <div>
        <Routes>
          <Route path="/" element={RequireAuth(<Home currentUser={currentUser} />)} />
          <Route path="/auth" element={<Authorization />} />
          <Route path="/playlists" element={RequireAuth(<Authorization onLogin={handleLogin} />)} />
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
