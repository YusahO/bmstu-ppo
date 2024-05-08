import './TagElement.css';

const TagElement = ({ tagName }) => {
	return (
		<label className='tag-element'>
			{tagName}
		</label>
	)
}

export default TagElement;