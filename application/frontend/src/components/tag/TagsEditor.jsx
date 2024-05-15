import './TagElement.css';
import TagElement from "./TagElement";
import Tag from '../../models/Tag.js'
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import TagEditSelector from "./TagEditSelector.jsx";
import { apiAuth } from "../../api/mpFetch.js";
import { useUserContext } from "../../context/UserContext.js";

const TagAdd = ({ onClose }) => {
	const { user } = useUserContext();
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

		apiAuth.post('tags', { ...Tag, authorId: user.id, name: name })
			.then(() => {
				setName(''); onClose();
			})
			.catch(error => console.error(error));
	}

	return (
		<div
			onClick={() => { setShowEdit(true); onClose(); }}
			onMouseLeave={() => setShowEdit(false)}
		>
			<label className='tag-element add'>
				+
			</label>
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
		<div style={{ marginTop: '20px', display: 'flex', gap: '20px', alignItems: 'center' }}>
			<TagAdd onClose={onClose} />
			{tags.map((tag, index) =>
				<div key={index}>
					<TagEditSelector tag={tag} onClose={onClose} />
				</div>
			)}
		</div>
	)
}

export default TagsEditor;