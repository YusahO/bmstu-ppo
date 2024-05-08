import React, { useState } from 'react';
import Score from '../../models/Score.js';
import './StarRatingInteractive.css';

const StarRatingInteractive = ({ audiotrackId, initialStars }) => {
  const [selectedStars, setSelectedStars] = useState(initialStars);

  const handleStarClick = (value) => {
    const accessToken = localStorage.getItem('accessToken');
    fetch(`http://localhost:9898/api/scores/`, {
      method: 'PUT',
      mode: 'cors',
      headers: {
        "Authorization": `Bearer ${accessToken}`,
        "Content-Type": "application/json; charset=utf-8",
      },
      body: JSON.stringify({ ...Score, value: value, audiotrackId: audiotrackId }),
    })
      .then((response) => {
        if (response.status === 401) {
          window.location = '/login';
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