import React, { useEffect, useState } from "react";
import { Button, Container, Typography } from "@mui/material";
import StaffList from "./components/StaffList";
import { Staff } from "./types/Staff";
import AdvancedSearchForm from "./components/AdvancedSearchForm";
import { deleteStaff, getStaffList, updateStaff } from "./services/Api";
import StaffForm from "./components/StaffForm";
import ReportExport from "./components/ReportExport";
import StaffEditForm from "./components/StaffEditForm";

const App: React.FC = () => {
  const [staffs, setStaffs] = useState<Staff[]>([]);
  const [selectedStaffId, setSelectedStaffId] = useState<string | undefined>();
  const [openForm, setOpenForm] = useState(false);
   const [openEditForm, setOpenEditForm] = useState(false);

  useEffect(() => {
    fetchStaffs();
  }, []);

  const handleSearch = (results: Staff[]) => {
    setStaffs(results);
  };

  const fetchStaffs = async () => {
    const data = await getStaffList();
    setStaffs(data);
  };

  const handleFormSuccess = () => {
    setSelectedStaffId(undefined);
    // Refresh staff list
    fetchStaffs();
  };

  const handleOpenForm = (staffId?: string) => {
    setSelectedStaffId(staffId);
    setOpenForm(true);
  };

  const handleCloseForm = () => {
    setOpenForm(false);
    setSelectedStaffId(undefined);
  };

  const handleDelete = async (id: string) => {
    await deleteStaff(id);
    setStaffs(staffs.filter((staff) => staff.staffId !== id));
  };
  const handleEdit = async (id: string, staff: Staff) => {
    setSelectedStaffId(id);
    setOpenEditForm(true);
  };


  const handleCloseEditForm = () => {
    setOpenEditForm(false);
    setSelectedStaffId(undefined);
  };

  return (
    <Container>
      <Typography variant="h4" sx={{ marginBottom: "20px", marginTop: "20px" }}>
        Staff Management
      </Typography>

      <AdvancedSearchForm onSearch={handleSearch} />

      <Button variant="contained" onClick={() => handleOpenForm()}>
        Add New Staff
      </Button>

      {/* {staffs.length > 0 && <ReportExport staffs={staffs} />} */}
      <StaffList staffs={staffs} onDelete={handleDelete} onEdit={handleEdit} />
      <StaffForm
        staffId={selectedStaffId}
        open={openForm}
        onClose={handleCloseForm}
        onSuccess={handleFormSuccess}
      />
      {selectedStaffId && (
        <StaffEditForm
          staffId={selectedStaffId}
          open={openEditForm}
          onClose={handleCloseEditForm}
          onSuccess={handleFormSuccess}
        />
      )}
      
      {staffs.length > 0 && <ReportExport staffs={staffs} />}
    </Container>
  );
};

export default App;
