const CommentaryElement = ({ commentary }) => {
	console.log(commentary);
	return (
		<div style={{
			display: 'flex',
			flexDirection: 'column',
			
			border: 'none',
			borderTop: '1px solid #ffffff',
			marginTop: '30px'
		}}>
			<h3 style={{padding: '10px 0px'}}>{commentary.authorName}</h3>
			<label>{commentary.text}</label>
		</div>
	)
}

export default CommentaryElement;