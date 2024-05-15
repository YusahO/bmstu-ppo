import { useState } from "react";
import DeletePrompt from "../common/DeletePrompt";
import TagSelector from "./TagSelector";
import { apiAuth } from "../../api/mpFetch";

const TagEditSelector = ({ tag, onClose }) => {
	function handleDelete() {
		apiAuth.delete('tags', { data: activeTag })
			.then(() => onClose())
			.catch(error => console.error(error));
	}

	function handleUpdate() {
		apiAuth.put('tags', activeTag)
			.then(() => onClose())
			.catch(error => console.error(error));
	}

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