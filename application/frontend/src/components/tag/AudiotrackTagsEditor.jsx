import { useEffect, useState } from "react";
import './TagContainer.css';
import TagSelector from "./TagSelector.jsx";

const AudiotrackTagsEditor = ({ audiotrackId }) => {

	const [tags, setTags] = useState([]);
	const [allTags, setAllTags] = useState([]);

	function handleTagUpdate(tag) {
		if (tags.some(t => t.id === tag.id)) {
			fetch(`http://localhost:9898/api/audiotracks/${audiotrackId}/tags`, {
				mode: 'cors',
				method: 'DELETE',
				headers: {
					'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
					'Content-Type': 'application/json; charset=utf-8'
				},
				body: JSON.stringify(tag.id)
			})
				.then((response) => {
					setTags(tags.filter(t => t.id != tag.id));
				})
				.catch(error => console.error(error))
		} else {
			fetch(`http://localhost:9898/api/audiotracks/${audiotrackId}/tags`, {
				mode: 'cors',
				method: 'POST',
				headers: {
					'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
					'Content-Type': 'application/json; charset=utf-8'
				},
				body: JSON.stringify(tag.id)
			})
				.then((response) => {
					setTags([...tags, tag]);
				})
				.catch(error => console.error(error))
		}
	}

	useEffect(() => {
		fetch(`http://localhost:9898/api/audiotracks/${audiotrackId}/tags`, {
			mode: 'cors',
			method: 'GET'
		})
			.then((response) => response.json())
			.then((data) => setTags(data))

		fetch(`http://localhost:9898/api/tags`, {
			mode: 'cors',
			method: 'GET'
		})
			.then((response) => response.json())
			.then((data) => setAllTags(data))
			.catch(error => console.error(error))

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