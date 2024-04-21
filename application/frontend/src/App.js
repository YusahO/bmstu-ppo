import React from 'react';
// import logo from './logo.svg';
import {BrowserRouter, Route, Routes} from 'react-router-dom';
import './App.css';
import Audiotracks from './components/audiotrack/Audiotracks';
import Layout from "./pages/Layout";

function App() {
  return (
    <BrowserRouter>
      <div>
        <Routes>
        <Route path="/" element={<Layout />} >
          <Route path="/audiotracks" element={<Audiotracks />} />
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
