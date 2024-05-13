import { useEffect, useState } from "react";
import TagElement from './TagElement.jsx';
import './TagContainer.css';

const AudiotrackTags = ({ audiotrackId }) => {

	const [tags, setTags] = useState([]);

	useEffect(() => {
		fetch(`http://localhost:9898/api/audiotracks/${audiotrackId}/tags`, {
			mode: 'cors',
			method: 'GET'
		})
			.then((response) => response.json())
			.then((data) => setTags(data))
			.catch(error => console.error(error))
	}, [audiotrackId]);

	return (
		<div className="tag-container">
			{tags.map(t => <TagElement tagName={t.name} />)}
		</div>
	)
}

export default AudiotrackTags;