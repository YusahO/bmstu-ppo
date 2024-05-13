import React, { useContext, useState } from 'react';
import { UserContext } from '../../App.js';

const Login = ({ onChange, onSuccess }) => {

	const { setUser } = useContext(UserContext);
	const [credentials, setCredentials] = useState({
		email: '',
		password: ''
	});

	const handleChange = (e) => {
		setCredentials({
			...credentials,
			[e.target.name]: e.target.value
		});
	};

	const handleSubmit = (e) => {
		e.preventDefault();
		fetch('http://localhost:9898/api/auth/login', {
			method: 'POST',
			mode: "cors",
			headers: {
				"Content-Type": "application/json; charset=utf-8",
			},
			body: JSON.stringify({
				email: credentials.email,
				password: credentials.password
			}),
		})
			.then((response) => {
				if (!response.ok) {
					throw new Error('Login failed');
				}
				return response.json();
			})
			.then((data) => {
				if (data.tokenDto.accessToken) {
					setUser(data.userDto);
					console.log(data.tokenDto.accessToken);
					localStorage.setItem('accessToken', data.tokenDto.accessToken);
				}
				return data;
			})
			.then(() => {
				onSuccess();
			});
	};

	return (
		<form onSubmit={handleSubmit} className='form-container'>
			<h3 className='form-label'>Вход</h3>
			<input
				type="text"
				placeholder="Почта"
				name='email'
				value={credentials.email}
				onChange={handleChange}
				required={true}
			/>
			<input
				type="password"
				placeholder="Пароль"
				name='password'
				value={credentials.password}
				onChange={handleChange}
				required={true}
			/>
			<button type="submit">Войти</button>
			<div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
				<label style={{ alignSelf: 'center' }}>или</label>
				<label
					style={{ textDecoration: 'underline', alignSelf: 'center' }}
					onClick={onChange}
				>
					Зарегистрироваться
				</label>
			</div>
		</form>
	);
}

export default Login;