import React, { useEffect, useState } from 'react';
import Audiotrack from "../../models/Audiotrack";
import AllAudiotrackGrid from "../../pages/AllAudiotrackGrid";

function Audiotracks() {
  const [audiotracks, setAudiotracks] = useState([]);

  const fetchAudiotracks = () => {
    fetch('http://localhost:9898/api/audiotracks', { mode: 'cors' })
      .then((response) => response.json())
      .then((data) => {
        let audiosList = [];
        data.map((audiotrack) => {
          audiosList.push({
            ...Audiotrack,
            title: audiotrack.title,
            filepath: audiotrack.filepath
          });
        });
        setAudiotracks(audiosList);
      })
      .catch(error => console.error('Error fetching users:', error));
  }

  useEffect(() => {
    fetchAudiotracks()
  }, []);

  return (
    <div>
      <h2>Все аудиотреки</h2>
      <AllAudiotrackGrid audiotracks={audiotracks} />
    </div>
  );
}

export default Audiotracks;