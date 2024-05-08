export function parseJwt(token = "") {
	try {
		const decodedToken = JSON.parse(atob(token.split('.')[1]));
		return {
			iss: decodedToken.iss,
			aud: decodedToken.aud,
			exp: decodedToken.exp,
			email: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
			name: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
			id: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
		}
	} catch (e) {
		return null;
	}
};