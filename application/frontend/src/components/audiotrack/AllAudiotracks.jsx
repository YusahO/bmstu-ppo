import React, { useEffect, useState } from 'react';
import AudiotrackGrid from "./AudiotrackGrid";
import { api } from '../../api/mpFetch';

function AllAudiotracks({ renderAdd }) {
  const [audiotracks, setAudiotracks] = useState([]);
  const [needUpdate, setNeedUpdate] = useState(false);

  const fetchAudiotracks = () => {
    api.get('audiotracks')
      .then(response => setAudiotracks(response.data))
      .catch(error => console.error(error));
  }

  useEffect(() => {
    fetchAudiotracks()
  }, [needUpdate]);

  return (
    <>
      <h2>Все аудиотреки</h2>
      <AudiotrackGrid
        renderAdd={renderAdd}
        audiotracks={audiotracks}
        onAudiotrackUpdate={() => setNeedUpdate(!needUpdate)}
      />
    </>
  );
}

export default AllAudiotracks;