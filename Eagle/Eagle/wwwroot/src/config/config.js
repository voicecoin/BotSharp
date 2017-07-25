import Env from './env';

let config = {
    env: Env,
	baseURL: (Env == 'development' ? `http://localhost:9000` : `http://api.yaya.ai`),
	testAccount: {username: (Env == 'development' ? `info@yaya.ai` : ``), password: (Env == 'development' ? `Yayabot123` : ``)}
};
export default config;