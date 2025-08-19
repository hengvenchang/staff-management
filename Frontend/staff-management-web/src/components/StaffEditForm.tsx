// src/components/StaffEditForm.tsx
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
} from "@mui/material";
import { Staff } from "../types/Staff";
import { updateStaff, getStaffById } from "../services/Api";

interface StaffEditFormProps {
  staffId: string; // Required for edit mode
  open: boolean; // Controls dialog visibility
  onClose: () => void; // Callback to close dialog
  onSuccess: () => void; // Callback after successful submission
}

const StaffEditForm: React.FC<StaffEditFormProps> = ({
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
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchStaff = async () => {
      try {
        const data = await getStaffById(staffId);
        setStaff(data);
        setLoading(false);
      } catch (error) {
        console.error("Error fetching staff:", error);
        setLoading(false);
      }
    };
    if (open) {
      fetchStaff();
    }
  }, [staffId, open]);

  const handleChange = (
    e: React.ChangeEvent<
      HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement
    >
  ) => {
    setStaff({ ...staff, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await updateStaff(staffId, staff);
      onSuccess();
      onClose();
    } catch (error) {
      console.error("Error updating staff:", error);
    }
  };

  if (loading) {
    return (
      <Dialog open={open} onClose={onClose}>
        <DialogContent>Loading...</DialogContent>
      </Dialog>
    );
  }

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
      <DialogTitle>Edit Staff</DialogTitle>
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
            disabled // Staff ID is typically non-editable
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
          Update Staff
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default StaffEditForm;
