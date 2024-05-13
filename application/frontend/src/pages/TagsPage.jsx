import { useContext, useEffect, useState } from "react";
import TagsEditor from "../components/tag/TagsEditor";
import { UserContext } from "../App";
import { useNavigate } from "react-router-dom";

const TagsPage = () => {
	// const { user } = useContext(UserContext);
	const navigate = useNavigate();
	const [tags, setTags] = useState(null);
	const [needUpdate, setNeedUpdate] = useState(false);

	useEffect(() => {
		fetch(`http://localhost:9898/api/tags`, {
			mode: 'cors',
			method: 'GET'
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth');
				}
				return response.json();
			})
			.then(data => setTags(data))
			.catch(error => console.error(error));
	}, [needUpdate]);

	if (!tags) {
		return <div>Loading...</div>
	}

	return (
		<div>
			<h2>Все теги</h2>
			<TagsEditor tags={tags} onClose={() => setNeedUpdate(!needUpdate)} />
		</div>
	);
}

export default TagsPage;