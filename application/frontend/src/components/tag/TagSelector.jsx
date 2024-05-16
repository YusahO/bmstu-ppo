import './TagElement.css';

const TagSelector = ({ tag, isSelected, onClick, onEditClick = null }) => {

	return (
		<div
			className="tag-element"
			style={{ display: 'flex', justifyContent: 'space-between' }}
		>
			<label>
				{tag.name}
			</label>
			<label
				className='tag-element button'
				onClick={onClick}
			>
				{isSelected ? <span>&times;</span> : '+'}
			</label>
			{onEditClick &&
				<label className='tag-element button' onClick={onEditClick}> &#9998; </label>
			}
		</div>
	);
}

export default TagSelector;