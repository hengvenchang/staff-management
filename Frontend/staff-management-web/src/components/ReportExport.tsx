import React from 'react';
import { Button } from '@mui/material';
import { Staff } from '../types/Staff';
import * as XLSX from 'xlsx';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable'; // Explicitly import autoTable

interface ReportExportProps {
  staffs: Staff[];
}

const ReportExport: React.FC<ReportExportProps> = ({ staffs }) => {
  const exportToExcel = () => {
    const worksheet = XLSX.utils.json_to_sheet(
      staffs.map((staff) => ({
        'Staff ID': staff.staffId,
        'Full Name': staff.fullName,
        Birthday: new Date(staff.birthday).toLocaleDateString(),
        Gender: staff.gender === 1 ? 'Male' : 'Female',
      }))
    );
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Staff');
    XLSX.writeFile(workbook, 'staff_report.xlsx');
  };

  const exportToPDF = () => {
    const doc = new jsPDF();
    // Apply the autoTable plugin to the jsPDF instance
    autoTable(doc, {
      head: [['Staff ID', 'Full Name', 'Birthday', 'Gender']],
      body: staffs.map((staff) => [
        staff.staffId,
        staff.fullName,
        new Date(staff.birthday).toLocaleDateString(),
        staff.gender === 1 ? 'Male' : 'Female',
      ]),
    });
    doc.save('staff_report.pdf');
  };

  return (
    <div>
      <Button onClick={exportToExcel}>Export to Excel</Button>
      <Button onClick={exportToPDF}>Export to PDF</Button>
    </div>
  );
};

export default ReportExport;