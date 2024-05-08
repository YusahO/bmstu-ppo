import { useEffect, useState } from "react";
import TagElement from './TagElement.jsx';
import './TagContainer.css';

const AudiotrackTags = ({ audiotrackId }) => {

	const [tags, setTags] = useState([]);

	useEffect(() => {
		let isMounted = true;
		fetch(`http://localhost:9898/api/tags/${audiotrackId}`, {
			mode: 'cors',
			method: 'GET'
		})
			.then((response) => response.json())
			.then((data) => {
				if (isMounted) {
					setTags(data);
				}
			})
	}, [audiotrackId]);

	return (
		<>
			{tags.length === 0 ?
				<label>Ничего нет</label>
				:
				<div className="tag-container">
					{tags.map(t => <TagElement tagName={t.name} />)}
				</div>
			}
		</>
	)
}

export default AudiotrackTags;