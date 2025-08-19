import React, { ReactNode, useState } from "react";
import {
  TextField,
  Button,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  SelectChangeEvent,
  Box,
} from "@mui/material";
import { SearchCriteria } from "../types/SearchCriteria";
import { searchStaff } from "../services/Api";
import { Staff } from "../types/Staff";

interface AdvancedSearchFormProps {
  onSearch: (results: Staff[]) => void;
}

const AdvancedSearchForm: React.FC<AdvancedSearchFormProps> = ({
  onSearch,
}) => {
  const [criteria, setCriteria] = useState<SearchCriteria>({});

  const handleChange = (
    e: React.ChangeEvent<
      HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement
    >
  ) => {
    setCriteria({ ...criteria, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const results = await searchStaff(criteria);
    onSearch(results);
  };

  const handleChangeGender = (
    event: SelectChangeEvent<number | "">,
    child: ReactNode
  ) => {
    const { name, value } = event.target;
    if (name) {
      // Convert value to number or keep as empty string
      const numericValue = value === "" ? "" : Number(value);
      setCriteria((prev) => ({
        ...prev,
        [name]: numericValue,
      }));
    }
  };

  return (
    <Box sx={{ marginBottom: "20px" }}>
      <form onSubmit={handleSubmit}>
        <TextField
          label="Staff ID"
          name="staffId"
          value={criteria.staffId || ""}
          onChange={handleChange}
        />
        <TextField
          label="Staff Name"
          name="name"
          value={criteria.name || ""}
          onChange={handleChange}
        />
        <FormControl>
          <InputLabel>Gender</InputLabel>
          <Select
            name="gender"
            value={criteria.gender || ""}
            onChange={handleChangeGender}
          >
            <MenuItem value="">Any</MenuItem>
            <MenuItem value={1}>Male</MenuItem>
            <MenuItem value={0}>Female</MenuItem>
          </Select>
        </FormControl>
        <TextField
          label="Birthday From"
          name="birthdayFrom"
          type="date"
          value={criteria.birthdayFrom || ""}
          onChange={handleChange}
          InputLabelProps={{ shrink: true }}
        />
        <TextField
          label="Birthday To"
          name="birthdayTo"
          type="date"
          value={criteria.birthdayTo || ""}
          onChange={handleChange}
          InputLabelProps={{ shrink: true }}
        />
        <Button type="submit">Search</Button>
      </form>
    </Box>
  );
};

export default AdvancedSearchForm;
