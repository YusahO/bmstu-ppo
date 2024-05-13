import { useContext, useState } from "react";
import { UserContext } from "../../App";
import './CommentaryElement.css';
import { useNavigate } from "react-router-dom";
import DeletePrompt from "../common/DeletePrompt";

const CommentaryEditField = ({ commentary, onClose }) => {
	const [newText, setNewText] = useState('');
	const navigate = useNavigate();

	function handleCommentaryEdited() {
		fetch(`http://localhost:9898/api/commentaries`, {
			mode: 'cors',
			method: 'PUT',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
				'Content-Type': 'application/json; charset=utf-8'
			},
			body: JSON.stringify({ ...commentary, text: newText }),
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth');
				}
				onClose();
			})
			.catch(error => console.error(error));
	}

	return (
		<div style={{
			position: 'fixed',
			display: 'flex',
			flexDirection: 'column',
			width: 300
		}}>
			<input
				placeholder="Новый текст (Enter для сохранения)"
				type="text"
				value={newText}
				onChange={e => setNewText(e.target.value)}
				onKeyDown={e => {
					if (e.key === 'Enter') {
						handleCommentaryEdited();
					}
				}}
			/>
		</div>
	);
}

const CommentaryActions = ({ commentary, needUpdate }) => {
	const {user} = useContext(UserContext);
	const navigate = useNavigate();
	const [showEditField, setShowEditField] = useState(false);
	const [showDeletePrompt, setShowDeletePrompt] = useState(false);

	function handleCommentaryDelete() {
		fetch(`http://localhost:9898/api/commentaries`, {
			mode: 'cors',
			method: 'DELETE',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
				'Content-Type': 'application/json; charset=utf-8'
			},
			body: JSON.stringify(commentary),
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth');
				}
				setShowDeletePrompt(false); needUpdate();
			})
			.catch(error => console.error(error));
	}

	return (
		<div className="commentary-actions">
			{user && (user.id === commentary.authorId) &&
				<div>
					<button onClick={() => setShowEditField(!showEditField)}>
						&#9998;
					</button>
					{showEditField &&
						<CommentaryEditField commentary={commentary} onClose={() => { setShowEditField(false); needUpdate(); }} />}
				</div>
			}

			<div>
				<button onClick={() => setShowDeletePrompt(true)}>
					&#128465;
				</button>
				{showDeletePrompt &&
					<DeletePrompt onAccept={handleCommentaryDelete} onClose={() => { setShowDeletePrompt(false); needUpdate(); }} />}
			</div>
		</div>
	);
}

const CommentaryElement = ({ commentary, needUpdate }) => {
	const { user } = useContext(UserContext);
	return (
		<div style={{
			display: 'flex',
			flexDirection: 'column',

			border: 'none',
			borderTop: '1px solid #ffffff',
			marginTop: '30px'
		}}>
			<div style={{
				display: 'flex',
				alignItems: 'center',
				justifyContent: 'space-between'
			}}>
				<h3 style={{ padding: '10px 0px' }}>{commentary.authorName}</h3>
				{user && (user.id === commentary.authorId || user.isAdmin) &&
					<CommentaryActions commentary={commentary} needUpdate={needUpdate} />}
			</div>
			<label>{commentary.text}</label>
		</div>
	)
}

export default CommentaryElement;