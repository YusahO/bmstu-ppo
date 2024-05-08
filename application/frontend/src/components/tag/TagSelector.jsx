import { useState } from 'react';
import './TagElement.css';

const TagSelector = ({ tag, onClick }) => {
	const [isChecked, setIsChecked] = useState(false);

	function handleClick() {
		document.querySelector(':root')
			.style
			.setProperty('--tag-elem-selection', isChecked ? '100%' : '130%');

		setIsChecked(!isChecked);
	}

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
				onClick={() => { onClick(); handleClick() }}
			>
				{isChecked ? 'x' : '+'}
			</label>
		</div>
	);
}

export default TagSelector;