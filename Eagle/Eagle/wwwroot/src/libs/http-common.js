import axios from 'axios';
import storage from 'localStorage';
import config from '../config/config';

export const HTTP = axios.create({
  baseURL: config.baseURL,
  withCredentials: true
})

HTTP.interceptors.request.use(function (config) {
    // Do something before request is sent
	config.headers.common['Authorization'] = 'Bearer ' + localStorage.getItem('access_token');
    return config;
  }, function (error) {
    // Do something with request error
    return Promise.reject(error);
  });