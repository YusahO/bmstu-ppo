import { useState } from "react";

import '../search/SearchOptions.css';

const Registration = () => {

	const [username, setUsername] = useState('');
	const [email, setEmail] = useState('');
	const [password, setPassword] = useState('');

	function handleRegister(e) {
		e.preventDefault();
		fetch('http://localhost:9898/api/auth/register', {
			method: 'POST',
			mode: "cors",
			headers: {
				"Content-Type": "application/json; charset=utf-8",
			},
			body: JSON.stringify({ username: username, email: email, password: password }),
		})
			.then((response) => {
				if (!response.ok) {
					throw new Error('Registration failed');
				}
				return response.json();
			})
			.then((data) => {
				if (data.tokenDto.accessToken) {
					console.log(data.tokenDto.accessToken);
					localStorage.setItem('accessToken', data.tokenDto.accessToken);
				}
				return data;
			});
	}

	return (
		<form onSubmit={handleRegister} className='form-container'>
			<h3 className='form-label'>Регистрация</h3>
			<input
				type="text"
				placeholder="Имя пользователя"
				value={username}
				onChange={(e) => setUsername(e.target.value)}
			/>
			<input
				type="text"
				placeholder="Почта"
				value={email}
				onChange={(e) => setEmail(e.target.value)}
			/>
			<input
				type="password"
				placeholder="Пароль"
				value={password}
				onChange={(e) => setPassword(e.target.value)}
			/>
			<button type="submit">Зарегистрироваться</button>
		</form>
	);
}

export default Registration;