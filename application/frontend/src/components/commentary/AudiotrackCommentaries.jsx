import { useEffect, useState } from "react";
import CommentaryElement from "./CommentaryElement.jsx";

const AudiotrackCommentaries = ({ audiotrackId }) => {
	const [comms, setComms] = useState([]);

	useEffect(() => {
		console.log('hello');
		fetch(`http://localhost:9898/api/commentaries/${audiotrackId}`, {
			mode: 'cors',
			method: 'GET'
		})
			.then((response) => response.json())
			.then((data) => {
				setComms(data);
			});
	}, [audiotrackId]);

	return (
		<div style={{ overflowY: 'scroll' }}>
			{comms.map(c => <CommentaryElement commentary={c} />)}
		</div>
	);
}

export default AudiotrackCommentaries;