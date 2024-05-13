import React, { useEffect, useState } from 'react';
import Audiotrack from "../../models/Audiotrack";
import AudiotrackGrid from "./AudiotrackGrid";

function AllAudiotracks({ renderAdd }) {
  const [audiotracks, setAudiotracks] = useState([]);
  const [needUpdate, setNeedUpdate] = useState(false);

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
  }, [needUpdate]);

  return (
    <>
      <h2>Все аудиотреки</h2>
      <div style={{
        overflow: 'hidden',
        margin: '10px 10px',
        padding: '10px 10px',
        borderRadius: '10px'
      }}>
        <AudiotrackGrid
          renderAdd={renderAdd}
          audiotracks={audiotracks}
          onAudiotrackUpdate={() => setNeedUpdate(!needUpdate)}
        />
      </div>
    </>
  );
}

export default AllAudiotracks;