import React from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Button,
} from "@mui/material";
import { Staff } from "../types/Staff";

interface StaffListProps {
  staffs: Staff[];
  onDelete: (id: string) => void; // Callback to handle deletion in the parent
  onEdit: (staffId: string, staff: Staff) => void; // Callback for edit button
}

const StaffList: React.FC<StaffListProps> = ({ staffs, onDelete, onEdit }) => {
  return (
    <Table>
      <TableHead>
        <TableRow>
          <TableCell>Staff ID</TableCell>
          <TableCell>Full Name</TableCell>
          <TableCell>Birthday</TableCell>
          <TableCell>Gender</TableCell>
          <TableCell>Actions</TableCell>
        </TableRow>
      </TableHead>
      <TableBody>
        {staffs.map((staff) => (
          <TableRow key={staff.staffId}>
            <TableCell>{staff.staffId}</TableCell>
            <TableCell>{staff.fullName}</TableCell>
            <TableCell>
              {new Date(staff.birthday).toLocaleDateString()}
            </TableCell>
            <TableCell>{staff.gender === 1 ? "Male" : "Female"}</TableCell>
            <TableCell>
              <Button onClick={() => onEdit(staff.staffId, staff)}>Edit</Button>
              <Button onClick={() => onDelete(staff.staffId)} color="error">Delete</Button>
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
};

export default StaffList;
