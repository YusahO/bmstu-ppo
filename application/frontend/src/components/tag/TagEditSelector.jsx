import { useState } from "react";
import { useNavigate } from "react-router-dom";
import TagElement from "./TagElement";
import DeletePrompt from "../common/DeletePrompt";
import TagSelector from "./TagSelector";

const TagEditSelector = ({ tag, onClose }) => {
	function handleDelete() {
		fetch('http://localhost:9898/api/tags', {
			mode: 'cors',
			method: 'DELETE',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
				'Content-Type': 'application/json; charset=utf-8'
			},
			body: JSON.stringify(activeTag)
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth');
				}
				onClose();
			})
			.catch(error => console.error(error));
	}

	function handleUpdate() {
		fetch('http://localhost:9898/api/tags', {
			mode: 'cors',
			method: 'PUT',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
				'Content-Type': 'application/json; charset=utf-8'
			},
			body: JSON.stringify(activeTag)
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth');
				}
				onClose();
			})
			.catch(error => console.error(error));
	}

	const navigate = useNavigate();
	const [activeTag, setActiveTag] = useState(tag);
	const [showDelete, setShowDelete] = useState(false);
	const [showEdit, setShowEdit] = useState(false);

	return (
		<div>
			<div onDoubleClick={() => setShowEdit(true)}>
				<TagSelector tag={tag} onClick={() => setShowDelete(true)} isSelected={true} />
			</div>
			{showDelete && <DeletePrompt onAccept={handleDelete} onClose={() => setShowDelete(false)} />}
			{showEdit &&
				<input
					style={{ position: 'fixed' }}
					type="text"
					placeholder="Название тега"
					value={activeTag.name}
					onChange={e => setActiveTag({ ...activeTag, name: e.target.value })}
					onKeyDown={e => {
						if (e.key === 'Enter') {
							handleUpdate();
						}
					}}
					onMouseLeave={() => setShowEdit(false)}
				/>
			}
		</div>
	)
}

export default TagEditSelector;