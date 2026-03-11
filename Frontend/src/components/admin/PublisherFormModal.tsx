import { useEffect, useState } from 'react';
import { AdminPublisher } from '../../services/adminService';

interface PublisherFormModalProps {
  open: boolean;
  onClose: () => void;
  initial: AdminPublisher | null;
  onSubmit: (values: Pick<AdminPublisher, 'name' | 'country' | 'address' | 'city' | 'phoneNumber'>) => Promise<void>;
}

export const PublisherFormModal = ({ open, onClose, initial, onSubmit }: PublisherFormModalProps) => {
  const [name, setName] = useState('');
  const [country, setCountry] = useState('');
  const [address, setAddress] = useState('');
  const [city, setCity] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');

  useEffect(() => {
    if (!initial) return;
    setName(initial.name);
    setCountry(initial.country ?? '');
    setAddress(initial.address ?? '');
    setCity(initial.city ?? '');
    setPhoneNumber(initial.phoneNumber ?? '');
  }, [initial, open]);

  if (!open || !initial) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-900/50 p-4">
      <form
        className="w-full max-w-2xl rounded-2xl bg-white p-6 shadow-xl"
        onSubmit={async (event) => {
          event.preventDefault();
          await onSubmit({ name: name.trim(), country, address, city, phoneNumber });
          onClose();
        }}
      >
        <h3 className="text-xl font-bold text-slate-900">Edit Publisher</h3>
        <div className="mt-4 grid gap-3 sm:grid-cols-2">
          <input required value={name} onChange={(event) => setName(event.target.value)} placeholder="Name" className="rounded-xl border border-slate-200 px-4 py-2 sm:col-span-2" />
          <input value={country} onChange={(event) => setCountry(event.target.value)} placeholder="Country" className="rounded-xl border border-slate-200 px-4 py-2" />
          <input value={city} onChange={(event) => setCity(event.target.value)} placeholder="City" className="rounded-xl border border-slate-200 px-4 py-2" />
          <input value={address} onChange={(event) => setAddress(event.target.value)} placeholder="Address" className="rounded-xl border border-slate-200 px-4 py-2 sm:col-span-2" />
          <input value={phoneNumber} onChange={(event) => setPhoneNumber(event.target.value)} placeholder="Phone number" className="rounded-xl border border-slate-200 px-4 py-2 sm:col-span-2" />
        </div>
        <div className="mt-5 flex justify-end gap-2">
          <button type="button" onClick={onClose} className="rounded-xl bg-slate-100 px-4 py-2">Cancel</button>
          <button type="submit" className="rounded-xl bg-brand-500 px-4 py-2 font-semibold text-white hover:bg-brand-600">Save</button>
        </div>
      </form>
    </div>
  );
};
