import axios from 'axios';

axios.defaults.baseURL = `${import.meta.env.VITE_API_ENDPOINT_ORIGIN}${import.meta.env.VITE_API_ENDPOINT_PATH}`;
