import './BlurComponent.css';

const BlurComponent = ({ children, style={} }) => {
	return (
		<div id="blur-component" style={style}>
			{children}
		</div>
	)
}

export default BlurComponent;