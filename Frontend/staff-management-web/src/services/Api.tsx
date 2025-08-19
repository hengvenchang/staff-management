import axios from "axios";
import { Staff } from "../types/Staff";
import { SearchCriteria } from "../types/SearchCriteria";

const API_URL = "http://localhost:5080/api/staff"; // Adjust to your backend URL

export const getStaffList = async (): Promise<Staff[]> => {
  const response = await axios.get(API_URL);
  return response.data;
};

export const getStaffById = async (id: string): Promise<Staff> => {
  const response = await axios.get(`${API_URL}/${id}`);
  return response.data;
};

export const createStaff = async (staff: Staff): Promise<Staff> => {
  const response = await axios.post(API_URL, staff);
  return response.data;
};

export const updateStaff = async (id: string, staff: Staff): Promise<Staff> => {
  const response = await axios.put(`${API_URL}/${id}`, staff);
  return response.data;
};

export const deleteStaff = async (id: string): Promise<void> => {
  await axios.delete(`${API_URL}/${id}`);
};

export const searchStaff = async (
  criteria: SearchCriteria
): Promise<Staff[]> => {
  const response = await axios.get(`${API_URL}/search`, {params: criteria});
  return response.data;
};
