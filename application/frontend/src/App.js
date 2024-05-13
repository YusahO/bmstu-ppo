import './App.css';

import React, { useEffect, useContext, useState } from 'react';
import { BrowserRouter, Route, Routes, Navigate } from 'react-router-dom';
import { parseJwt } from './Globals';

import Home from "./pages/Home.jsx";
import Authorization from './components/auth/Authorization.jsx';
import SearchResults from './pages/SearchResults.jsx';
import TopBar from './pages/TopBar.jsx';
import SideBar from './pages/SideBar.jsx';
import Playlists from './pages/Playlists.jsx';
import PlaylistAudiotracks from './pages/PlaylistAudiotracks.jsx';
import TagsPage from './pages/TagsPage.jsx';

export const UserContext = React.createContext();

function retrieveUser(setUser) {
  const token = localStorage.getItem('accessToken');
  if (!token) {
    setUser(null);
    return;
  }

  const userId = parseJwt(token).id;
  fetch(`http://localhost:9898/api/users/${userId}`, {
    mode: 'cors',
    method: 'GET',
    headers: {
      'Authorization': `Bearer ${token}`
    }
  })
    .then(response => response.json())
    .then(data => {
      setUser(data);
    })
    .catch(error => console.log(error));
}

function App() {
  const [user, setUser] = useState(null);

  useEffect(() => {
    retrieveUser(setUser);
    const storageEventListener = () => {
      retrieveUser(setUser);
    };

    window.addEventListener('storage', storageEventListener);
    return () => {
      window.removeEventListener('storage', storageEventListener);
    }
  }, []);

  return (
    <UserContext.Provider value={{ user, setUser }}>
      <Main />
    </UserContext.Provider>
  );
}

function Main() {

  const { user, setUser } = useContext(UserContext);
  const [opened, setOpened] = useState(true);

  function handleClick() {
    setOpened(!opened);
    if (opened) {
      document.getElementById("sidebarComp").style.width = "250px";
    } else {
      document.getElementById("sidebarComp").style.width = "0";
    }
  }

  const RequireAuth = (requireAdmin, elem) => {
    useEffect(() => {
      retrieveUser(setUser);
    }, []);
    if (user === null) {
      return <div>Loading...</div>;
    }
    if (requireAdmin) {
      return user && user.isAdmin ? elem : <Navigate to='/auth' />;
    } else {
      return user ? elem : <Navigate to='/auth' />;
    }
  }

  return (
    <BrowserRouter>
      <div style={{
        display: 'flex',
        flexDirection: 'column',
        gap: '10px'
      }}>
        <TopBar onSidebarClick={handleClick} />
        <div className='content-pages'>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/search" element={<SearchResults />} />
            <Route path="/auth" element={<Authorization />} />
            <Route path="/playlists" element={RequireAuth(false, <Playlists />)} />
            <Route path="/audiotracks" element={RequireAuth(false, <PlaylistAudiotracks />)} />
            <Route path="/tags" element={RequireAuth(true, <TagsPage />)} />
          </Routes>
          <SideBar />
        </div>
      </div>
    </BrowserRouter>
  );
}

export default App;
