import { api } from '../../api/mpFetch.js';
import { useEffect, useState } from "react";
import TagElement from './TagElement.jsx';
import './TagContainer.css';

const AudiotrackTags = ({ audiotrackId }) => {

	const [tags, setTags] = useState([]);

	useEffect(() => {

		api.get(`audiotracks/${audiotrackId}/tags`)
			.then(response => setTags(response.data))
			.catch(error => console.error(error));

	}, [audiotrackId]);

	return (
		<div className="tag-container">
			{tags.map(t => <TagElement tagName={t.name} />)}
		</div>
	)
}

export default AudiotrackTags;