import '../search/SearchOptions.css';

import { useState } from "react";
import { parseJwt, setCookie } from "../../Globals";
import { api } from '../../api/mpFetch';
import { useUserContext } from '../../context/UserContext';
import { AlertTypes, useAlertContext } from '../../context/AlertContext';

const Login = ({ onChange, onSuccess }) => {
	const { setUser } = useUserContext();
	const { addAlert } = useAlertContext();

	const [credentials, setCredentials] = useState({
		username: '',
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
		api.post('auth/login', {
			email: credentials.email,
			password: credentials.password
		})
			.then(response => {
				const token = response.data.token;
				if (token) {
					const tokenParsed = parseJwt(token);
					const cookieLifetime = (new Date(tokenParsed.exp).getTime() - Math.floor(new Date().getTime() / 1000));
					setUser(response.data.userDto);
					setCookie('token', token, cookieLifetime);
				}
				onSuccess();
			})
			.catch(() => addAlert(AlertTypes.info, 'Не удалось авторизоваться'))
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
					style={{ textDecoration: 'underline', alignSelf: 'center', cursor: 'pointer' }}
					onClick={onChange}
				>
					Зарегистрироваться
				</label>
			</div>
		</form>
	);
}

export default Login;