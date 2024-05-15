import './TagElement.css';

const TagSelector = ({ tag, isSelected, onClick }) => {

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
		</div>
	);
}

export default TagSelector;