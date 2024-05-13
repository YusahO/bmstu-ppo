import './TagElement.css';

const TagSelector = ({ tag, isSelected, onClick }) => {

	return (
		<div
			className="tag-element"
			style={{ display: 'flex', gap: '10px' }}
		>
			<label>
				{tag.name}
			</label>
			<label
				className='tag-select-button'
				onClick={onClick}
			>
				{isSelected ? 'x' : '+'}
			</label>
		</div>
	);
}

export default TagSelector;