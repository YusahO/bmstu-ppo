import './CommentaryElement.css';
import { useState } from "react";
import { apiAuth } from "../../api/mpFetch";
import { useUserContext } from "../../context/UserContext";
import { AlertTypes, useAlertContext } from "../../context/AlertContext";
import { useNavigate } from "react-router-dom";
import DeletePrompt from "../common/DeletePrompt";
import BlurComponent from "../common/BlurComponent";
import CloseButton from '../common/CloseButton';

const CommentaryEditField = ({ commentary, onClose }) => {
	const { user } = useUserContext();
	const { addAlert } = useAlertContext();
	const [text, setText] = useState(commentary.text);
	const navigate = useNavigate();

	function handleSendReport() {
		if (!user) {
			navigate('/auth');
			return;
		}
		apiAuth.put('commentaries', { ...commentary, text: text })
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
		<BlurComponent style={{
			left: '50%',
			top: '50%',
			transform: 'translate(-50%, -50%)'
		}}>
			<div id="report-window">
				<h2>Изменение комментария</h2>
				<textarea
					type='text'
					placeholder='Текст комментария'
					value={text}
					multiline={true}
					onChange={e => setText(e.target.value)}
					style={{
						width: '100%', height: '100%'
					}}
				/>
				<button onClick={handleSendReport}>Изменить</button>
				<CloseButton onClose={onClose} />
			</div>
		</BlurComponent>
	)
}

const CommentaryActions = ({ commentary, needUpdate }) => {
	const { user } = useUserContext();
	const { addAlert } = useAlertContext();
	const [showEditField, setShowEditField] = useState(false);
	const [showDeletePrompt, setShowDeletePrompt] = useState(false);

	function handleCommentaryDelete() {
		apiAuth.delete(`commentaries/${commentary.id}`)
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