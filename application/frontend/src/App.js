import './App.css';

import React, { useEffect, useState } from 'react';
import { BrowserRouter, Route, Routes, Navigate } from 'react-router-dom';
import { getCookie, parseJwt } from './Globals';
import { apiAuth } from './api/mpFetch.js';
import { Cookies } from 'react-cookie';
import { AlertProvider } from './context/AlertContext.js';
import { UserProvider, useUserContext } from './context/UserContext.js';

import Home from "./pages/Home.jsx";
import Authorization from './components/auth/Authorization.jsx';
import SearchResults from './pages/SearchResults.jsx';
import TopBar from './pages/TopBar.jsx';
import SideBar from './pages/SideBar.jsx';
import Playlists from './pages/Playlists.jsx';
import PlaylistAudiotracks from './pages/PlaylistAudiotracks.jsx';
import TagsPage from './pages/TagsPage.jsx';
import AlertNotifies from './components/common/AlertNotify.jsx';
import ReportsPage from './pages/ReportsPage.jsx';
import AuthAdminPage from './pages/AuthAdminPage.jsx'

function App() {
  return (
    <AlertProvider>
      <UserProvider>
        <Main />
      </UserProvider>
    </AlertProvider>
  );
}

function Main() {

  const { user, setUser } = useUserContext();
  const [opened, setOpened] = useState(true);

  const cookies = new Cookies();
  const token = cookies.get('token');

  useEffect(() => {
    if (!token) {
      setUser(null);
      return;
    }
    const userId = parseJwt(token).id;
    apiAuth.get(`users/${userId}`)
      .then(response => setUser(response.data))
      .catch(error => console.log(error));

    const cookieChangeListener = (name, value) => {
      if (name === 'token' && value === 'undefined') {
        setUser(null);
      }
    }

    cookies.addChangeListener(cookieChangeListener);
    return () => {
      cookies.removeChangeListener(cookieChangeListener);
    }
  }, []);

  if (!user && token) {
    return <div>Loading...</div>
  }

  function handleClick() {
    setOpened(!opened);
    if (opened) {
      document.getElementById("sidebarComp").style.width = "350px";
    } else {
      document.getElementById("sidebarComp").style.width = "0";
    }
  }

  const RequireAuth = (requireAdmin, elem) => {
    if (!getCookie('token')) {
      setUser(null);
    }

    if (requireAdmin) {
      return user && user.isAdmin ? elem : <Navigate to='/auth' />;
    } else {
      return user ? elem : <Navigate to='/auth' />;
    }
  }

  return (
    <BrowserRouter>
      <TopBar onSidebarClick={handleClick} />
      <div className='content-pages'>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/search" element={<SearchResults />} />
          <Route path="/auth" element={<Authorization />} />
          <Route path="/playlists" element={RequireAuth(false, <Playlists />)} />
          <Route path="/audiotracks" element={RequireAuth(false, <PlaylistAudiotracks />)} />
          <Route path="/tags" element={RequireAuth(true, <TagsPage />)} />
          <Route path="/reports" element={RequireAuth(true, <ReportsPage />)} />
          <Route path="/auth_admin" element={RequireAuth(true, <AuthAdminPage />)} />
        </Routes>
        {user && <SideBar />}
      </div>
      <AlertNotifies />
    </BrowserRouter>
  );
}

export default App;
