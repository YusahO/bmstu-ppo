import React, { useState } from 'react';

const Login = () => {
	const [email, setEmail] = useState('');
	const [password, setPassword] = useState('');

	const handleLogin = (e) => {
		e.preventDefault();
		fetch('http://localhost:9898/api/auth/login', {
			method: 'POST',
			mode: "cors",
			headers: {
				"Content-Type": "application/json; charset=utf-8",
			},
			body: JSON.stringify({ email: email, password: password }),
		})
			.then((response) => {
				if (!response.ok) {
					throw new Error('Login failed');
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
	};

	return (
		<form onSubmit={handleLogin} className='form-container'>
			<h3 className='form-label'>Вход</h3>
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
			<button type="submit">Войти</button>
		</form>
	);
}

export default Login;