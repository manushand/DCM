import axios from 'axios';

// Configure axios to use the API on localhost:5014
const api = axios.create({
  baseURL: 'http://localhost:5014',
  headers: {
    'Content-Type': 'application/json',
  },
});

export default api;
