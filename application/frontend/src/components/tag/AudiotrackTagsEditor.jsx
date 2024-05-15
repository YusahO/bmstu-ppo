import './TagContainer.css';
import { api, apiAuth } from '../../api/mpFetch.js';
import { useEffect, useState } from "react";
import TagSelector from "./TagSelector.jsx";

const AudiotrackTagsEditor = ({ audiotrackId }) => {

	const [tags, setTags] = useState([]);
	const [allTags, setAllTags] = useState([]);

	function handleTagUpdate(tag) {
		if (tags.some(t => t.id === tag.id)) {
			apiAuth.delete(`audiotracks/${audiotrackId}/tags`, { data: tag.id })
				.then(() => setTags(tags.filter(t => t.id !== tag.id)))
				.catch(error => console.error(error));

		} else {
			apiAuth.post(`audiotracks/${audiotrackId}/tags`, tag.id)
				.then(() => setTags([...tags, tag]))
				.catch(error => console.error(error));
		}
	}

	useEffect(() => {
		api.get(`audiotracks/${audiotrackId}/tags`)
			.then(response => setTags(response.data))
			.catch(error => console.error(error));

		api.get('tags')
			.then(response => setAllTags(response.data))
			.catch(error => console.error(error));

	}, [audiotrackId]);

	if (!tags || !allTags) {
		return <div>Loading...</div>;
	}

	return (
		<div className="tag-container">
			{allTags.map(t =>
				<TagSelector
					tag={t}
					isSelected={tags && tags.some(tt => tt.id === t.id)}
					onClick={() => handleTagUpdate(t)}
				/>)}
		</div>
	)
}

export default AudiotrackTagsEditor;