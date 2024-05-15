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

export function setCookie(name, value, lifetimeSecs) {
	let cookie = name + "=" + encodeURIComponent(value);

	if (typeof lifetimeSecs === "number") {
		cookie += "; max-age=" + (lifetimeSecs);
		document.cookie = cookie;
	}
}

export function getCookie(name) {
  let cookie = document.cookie.split('; ').find(row => row.startsWith(name + '='));
  return cookie ? cookie.split('=')[1] : null;
}