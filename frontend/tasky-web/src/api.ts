import axios from "axios";
const api = axios.create({
  baseURL: "https://localhost:7169/api", // adjust port from your API
});
export default api;