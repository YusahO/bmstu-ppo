import './StarRatingInteractive.css';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Score from '../../models/Score.js';

const StarRatingInteractive = ({ audiotrackId, initialStars }) => {

  const navigate = useNavigate();
  const [selectedStars, setSelectedStars] = useState(initialStars);

  const handleStarClick = (value) => {
    // await api.put('/scores', {
    //   ...Score,
    //   value: value,
    //   audiotrackId: audiotrackId
    // }, {
    //   headers: `Bearer ${localStorage.getItem('accessToken')}`
    // })
    //   .then(response => {
    //     console.log('AAAAAAAA');
    //     if (response.status === 401) {
    //       navigate('/auth');
    //     }
    //     console.log(value);
    //     setSelectedStars(value);
    //   })
    //   .catch(error => console.warn(error));

    fetch(`http://localhost:9898/api/scores/`, {
      method: 'PUT',
      mode: 'cors',
      headers: {
        "Authorization": `Bearer ${localStorage.getItem('accessToken')}`,
        "Content-Type": "application/json; charset=utf-8",
      },
      body: JSON.stringify({ ...Score, value: value, audiotrackId: audiotrackId }),
    })
      .then((response) => {
        if (response.status === 401) {
          navigate('/auth');
        }
        else {
          setSelectedStars(value);
        }
      });
  };


  return (
    <div className="rate">
      {[5, 4, 3, 2, 1].map((starValue) => (
        <React.Fragment key={starValue}>
          <input
            type="radio"
            id={`star${starValue}`}
            name="rate"
            value={starValue}
            checked={selectedStars === starValue}
            onChange={() => handleStarClick(starValue)}
          />
          <label htmlFor={`star${starValue}`} title={`${starValue} stars`}>
            {starValue} stars
          </label>
        </React.Fragment>
      ))}
    </div>
  );
};

export default StarRatingInteractive;