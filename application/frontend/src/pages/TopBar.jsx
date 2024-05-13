import { useNavigate } from "react-router-dom";
import { useState } from "react";
import SearchBar from "../components/search/SearchBar";
import UserMinimized from "../components/user/UserMinimized";

import './TopBar.css';

const TopBar = ({ onSidebarClick }) => {

	const navigate = useNavigate();
	function handleClick() {
		navigate("/");
	}

	return (
		<div className='topbar'>

			<button style={{
				transform: 'scale(1.3)',
				backgroundColor: 'transparent',
				borderRadius: 0,
				position: "fixed",
				right: 5,
				float: 'right'
			}} onClick={onSidebarClick}>☰</button>
			<div style={{ padding: '50px' }}>
				<UserMinimized />
			</div>
			<SearchBar />
			<label style={{
				fontSize: '30px',
				fontStyle: 'italic',
				cursor: 'pointer'
			}} onClick={handleClick}>
				MewingPad
			</label>
		</div>
	)
}

export default TopBar;