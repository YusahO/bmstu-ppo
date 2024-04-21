import React, { useEffect, useState } from 'react';
import Audiotrack from "../../models/Audiotrack";

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
            <h2>All Audiotracks</h2>
            <ul>
                {audiotracks.map(audiotrack => (
                    <li key={audiotrack.id}>
                        <strong>Title</strong> {audiotrack.title}<br />
                        {/* <audio controls src="http://178.140.95.215:9877/audiofiles/${audiotrack.title}"></audio> */}
                        <audio controls src={`http://178.140.95.215:9877/audiofiles/${encodeURIComponent(audiotrack.filepath)}`}></audio>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default Audiotracks;