import { ReactNode } from 'react';

interface DataTableProps {
  headers: string[];
  rows: ReactNode;
}

export const DataTable = ({ headers, rows }: DataTableProps) => (
  <div className="overflow-hidden rounded-2xl bg-white shadow-md">
    <div className="overflow-x-auto">
      <table className="w-full min-w-[640px] text-left">
        <thead className="bg-slate-50 text-sm text-slate-500">
          <tr>{headers.map((header) => <th key={header} className="px-4 py-3 font-medium">{header}</th>)}</tr>
        </thead>
        <tbody className="text-sm text-slate-700">{rows}</tbody>
      </table>
    </div>
  </div>
);
