import React, { useState, useEffect, ReactNode } from "react";
import {
  TextField,
  Button,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  SelectChangeEvent,
  Snackbar,
  Alert,
} from "@mui/material";
import { Staff } from "../types/Staff";
import { createStaff, updateStaff, getStaffById } from "../services/Api";
import axios from "axios";

interface StaffFormProps {
  staffId?: string; // Optional for edit mode
  open: boolean; // Controls dialog visibility
  onClose: () => void; // Callback to close dialog
  onSuccess: () => void; // Callback after successful submission
}

const StaffForm: React.FC<StaffFormProps> = ({
  staffId,
  open,
  onClose,
  onSuccess,
}) => {
  const [staff, setStaff] = useState<Staff>({
    staffId: "",
    fullName: "",
    birthday: "",
    gender: 1,
  });
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (staffId) {
      const fetchStaff = async () => {
        const data = await getStaffById(staffId);
        setStaff(data);
      };
      fetchStaff();
    } else {
      // Reset form for adding new staff
      setStaff({ staffId: "", fullName: "", birthday: "", gender: 1 });
    }
  }, [staffId, open]); // Reset form when dialog opens/closes

  const handleChange = (
    e: React.ChangeEvent<
      HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement
    >
  ) => {
    setStaff({ ...staff, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    try {
      if (staffId) {
        await updateStaff(staffId, staff);
      } else {
        await createStaff(staff);
      }
      onSuccess();
      onClose();
    } catch (err) {
     if (axios.isAxiosError(err) && err.response?.data?.message) {
        setError(err.response.data.message);
      } else if (axios.isAxiosError(err) && err.response?.status === 400) {
        setError(
          staffId
            ? "Failed to update staff: Invalid data"
            : "Failed to add staff: Staff ID already exists"
        );
      } else {
        setError("An error occurred while saving staff data.");
      }
    } finally {
      setLoading(false);
    }
  };

  const handleChangeGender = (
    event: SelectChangeEvent<number | "">,
    child: ReactNode
  ) => {
    const { name, value } = event.target;
    if (name) {
      setStaff({ ...staff, [name]: value });
    }
  };

  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>{staffId ? "Edit Staff" : "Add Staff"}</DialogTitle>
      <DialogContent>
        <form onSubmit={handleSubmit}>
          <TextField
            label="Staff ID"
            name="staffId"
            value={staff.staffId}
            onChange={handleChange}
            required
            inputProps={{ maxLength: 8 }}
            fullWidth
            margin="normal"
          />
          <TextField
            label="Full Name"
            name="fullName"
            value={staff.fullName}
            onChange={handleChange}
            required
            inputProps={{ maxLength: 100 }}
            fullWidth
            margin="normal"
          />
          <TextField
            label="Birthday"
            name="birthday"
            type="date"
            value={staff.birthday}
            onChange={handleChange}
            required
            InputLabelProps={{ shrink: true }}
            fullWidth
            margin="normal"
          />
          <FormControl fullWidth margin="normal">
            <InputLabel>Gender</InputLabel>
            <Select
              name="gender"
              value={staff.gender}
              onChange={handleChangeGender}
            >
              <MenuItem value={1}>Male</MenuItem>
              <MenuItem value={2}>Female</MenuItem>
            </Select>
          </FormControl>
        </form>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button type="submit" onClick={handleSubmit}>
          {staffId ? "Update" : "Add"} Staff
        </Button>
      </DialogActions>
      <Snackbar
        open={!!error}
        autoHideDuration={6000}
        anchorOrigin={{ vertical: "top", horizontal: "center" }}
      >
        <Alert severity="error" sx={{ width: "100%" }}>
          {error}
        </Alert>
      </Snackbar>
    </Dialog>
  );
};

export default StaffForm;
