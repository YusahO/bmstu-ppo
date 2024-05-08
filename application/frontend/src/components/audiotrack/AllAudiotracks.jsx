import React, { useEffect, useState } from 'react';
import Audiotrack from "../../models/Audiotrack";
import AudiotrackGrid from "../../pages/AudiotrackGrid";
import './AllAudiotracks.css';

function AllAudiotracks() {
  const [audiotracks, setAudiotracks] = useState([]);

  const fetchAudiotracks = () => {
    fetch('http://localhost:9898/api/audiotracks', { mode: 'cors' })
      .then((response) => response.json())
      .then((data) => {
        let audiosList = [];
        data.map((audiotrack) => {
          audiosList.push({
            ...Audiotrack,
            id: audiotrack.id,
            title: audiotrack.title,
            filepath: audiotrack.filepath
          });
        });
        setAudiotracks(audiosList);
      })
      .catch(error => console.error('Error fetching audiotracks:', error));
  }

  useEffect(() => {
    fetchAudiotracks()
  }, []);

  return (
    <>
      <h2>Все аудиотреки</h2>
      <div className='audiotracks-container'>
        <AudiotrackGrid audiotracks={audiotracks} />
      </div>
    </>
  );
}

export default AllAudiotracks;