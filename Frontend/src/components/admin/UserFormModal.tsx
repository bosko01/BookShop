import { useEffect, useState } from 'react';
import { AdminUser } from '../../services/adminService';

interface UserFormModalProps {
  open: boolean;
  onClose: () => void;
  initial: AdminUser | null;
  onSubmit: (values: {
    firstName: string;
    lastName: string;
    email: string;
    role: AdminUser['role'];
    password: string;
  }) => Promise<void>;
}

export const UserFormModal = ({ open, onClose, initial, onSubmit }: UserFormModalProps) => {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [role, setRole] = useState<AdminUser['role']>('Customer');
  const [password, setPassword] = useState('');

  useEffect(() => {
    if (!initial) return;
    setFirstName(initial.firstName);
    setLastName(initial.lastName);
    setEmail(initial.email);
    setRole(initial.role);
    setPassword('');
  }, [initial, open]);

  if (!open || !initial) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-900/50 p-4">
      <form
        className="w-full max-w-2xl rounded-2xl bg-white p-6 shadow-xl"
        onSubmit={async (event) => {
          event.preventDefault();
          await onSubmit({ firstName: firstName.trim(), lastName: lastName.trim(), email: email.trim(), role, password });
          onClose();
        }}
      >
        <h3 className="text-xl font-bold text-slate-900">Edit User</h3>
        <p className="mt-1 text-sm text-slate-500">Leave password empty if you do not want to change it.</p>
        <div className="mt-4 grid gap-3 sm:grid-cols-2">
          <input required value={firstName} onChange={(event) => setFirstName(event.target.value)} placeholder="First name" className="rounded-xl border border-slate-200 px-4 py-2" />
          <input required value={lastName} onChange={(event) => setLastName(event.target.value)} placeholder="Last name" className="rounded-xl border border-slate-200 px-4 py-2" />
          <input required type="email" value={email} onChange={(event) => setEmail(event.target.value)} placeholder="Email" className="rounded-xl border border-slate-200 px-4 py-2 sm:col-span-2" />
          <select value={role} onChange={(event) => setRole(event.target.value as AdminUser['role'])} className="rounded-xl border border-slate-200 px-4 py-2">
            <option value="Customer">Customer</option>
            <option value="Admin">Admin</option>
          </select>
          <input type="password" minLength={8} value={password} onChange={(event) => setPassword(event.target.value)} placeholder="New password (optional)" className="rounded-xl border border-slate-200 px-4 py-2" />
        </div>
        <div className="mt-5 flex justify-end gap-2">
          <button type="button" onClick={onClose} className="rounded-xl bg-slate-100 px-4 py-2">Cancel</button>
          <button type="submit" className="rounded-xl bg-brand-500 px-4 py-2 font-semibold text-white hover:bg-brand-600">Save</button>
        </div>
      </form>
    </div>
  );
};
