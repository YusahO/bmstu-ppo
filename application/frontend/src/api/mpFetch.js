import axios from "axios";

const MEWING_PAD_API_HOST = 'http://localhost:9898/api';

const api = axios.create({
	baseURL: MEWING_PAD_API_HOST,
	headers: {
		post: { 'Content-Type': 'application/json; charset=utf-8' },
		put: { 'Content-Type': 'application/json; charset=utf-8' },
		delete: { 'Content-Type': 'application/json; charset=utf-8' },
		common: { 'Access-Control-Allow-Origin': '*' }
	}
});

const apiAuth = axios.create({
	baseURL: MEWING_PAD_API_HOST,
	headers: {
		post: { 'Content-Type': 'application/json; charset=utf-8' },
		put: { 'Content-Type': 'application/json; charset=utf-8' },
		delete: { 'Content-Type': 'application/json; charset=utf-8' },
		common: { 'Access-Control-Allow-Origin': '*' },
	},
	withCredentials: true
});

apiAuth.interceptors.response.use(
	function (response) {
		if (response.status === 401) {
			window.location = '/auth';
		}
		return response;
	}, function (error) {
		return Promise.reject(error);
	}
);

export { api, apiAuth, MEWING_PAD_API_HOST };