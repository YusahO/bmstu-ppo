import TagElement from "./TagElement";
import Tag from '../../models/Tag.js'
import { useContext, useState } from "react";
import { UserContext } from "../../App";
import { useNavigate } from "react-router-dom";
import TagEditSelector from "./TagEditSelector.jsx";

const TagAdd = ({ onClose }) => {
	const { user } = useContext(UserContext);
	const navigate = useNavigate();

	const [name, setName] = useState('');
	const [showEdit, setShowEdit] = useState(false);

	function handleTagAdd() {
		if (name === '') {
			return;
		}
		if (!user) {
			navigate('/auth');
			return;
		}

		fetch('http://localhost:9898/api/tags', {
			mode: 'cors',
			method: 'POST',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
				'Content-Type': 'application/json; charset=utf-8'
			},
			body: JSON.stringify({ ...Tag, authorId: user.id, name: name })
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth');
				}
				setName('');
				onClose();
			})
			.catch(error => console.error(error));
	}

	return (
		<div
			onClick={() => { setShowEdit(true); onClose(); }}
			onMouseLeave={() => setShowEdit(false)}
		>
			<TagElement tagName="+" />
			{showEdit &&
				<input
					style={{ position: 'fixed' }}
					type="text"
					placeholder="Имя тега"
					value={name}
					required={true}
					onChange={e => setName(e.target.value)}
					onKeyDown={e => {
						if (e.key === 'Enter') {
							handleTagAdd();
							setShowEdit(false);
						}
					}}
				/>}
		</div>
	)
}

const TagsEditor = ({ tags, onClose }) => {

	return (
		<div style={{ marginTop: '20px', display: 'flex', gap: '20px' }}>
			<TagAdd onClose={onClose} />
			{tags.map((tag, index) =>
				<TagEditSelector tag={tag} onClose={onClose} />
			)}
		</div>
	)
}

export default TagsEditor;