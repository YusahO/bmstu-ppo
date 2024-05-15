import { useState } from "react";
import './CommentaryElement.css';
import DeletePrompt from "../common/DeletePrompt";
import { apiAuth } from "../../api/mpFetch";
import { useUserContext } from "../../context/UserContext";
import { AlertTypes, useAlertContext } from "../../context/AlertContext";

const CommentaryEditField = ({ commentary, onClose }) => {
	const { addAlert } = useAlertContext();
	const [newText, setNewText] = useState(commentary.text);

	function handleCommentaryEdited() {
		apiAuth.put('commentaries', { ...commentary, text: newText })
			.then(() => {
				addAlert(AlertTypes.success, 'Комментарий успешно изменен')
				onClose()
			})
			.catch(error => {
				addAlert(AlertTypes.error, 'Не удалось обновить комментарий');
				console.error(error);
			});
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
	const { user } = useUserContext();
	const { addAlert } = useAlertContext();
	const [showEditField, setShowEditField] = useState(false);
	const [showDeletePrompt, setShowDeletePrompt] = useState(false);

	function handleCommentaryDelete() {
		apiAuth.delete('commentaries', { data: commentary })
			.then(() => {
				addAlert(AlertTypes.success, 'Комментарий успешно удален')
				setShowDeletePrompt(false); needUpdate();
			})
			.catch(error => console.error(error));
	}

	return (
		<div className="commentary-actions">
			{user && (user.id === commentary.authorId) &&
				<div>
					<button
						className="commentary-actions button"
						onClick={() => setShowEditField(!showEditField)}
					>
						&#9998;
					</button>
					{showEditField &&
						<CommentaryEditField commentary={commentary} onClose={() => { setShowEditField(false); needUpdate(); }} />}
				</div>
			}

			<div>
				<button
					className="commentary-actions button"
					onClick={() => setShowDeletePrompt(true)}
				>
					&#128465;
				</button>
				{showDeletePrompt &&
					<DeletePrompt onAccept={handleCommentaryDelete} onClose={() => { setShowDeletePrompt(false); needUpdate(); }} />}
			</div>
		</div>
	);
}

const CommentaryElement = ({ commentary, needUpdate }) => {
	const { user } = useUserContext();
	return (
		<div className="commentary-element">
			<div>
				<h3 style={{ padding: '10px 0px' }}>{commentary.authorName}</h3>
				{user && (user.id === commentary.authorId || user.isAdmin) &&
					<CommentaryActions commentary={commentary} needUpdate={needUpdate} />}
			</div>
			<label>{commentary.text}</label>
		</div>
	)
}

export default CommentaryElement;