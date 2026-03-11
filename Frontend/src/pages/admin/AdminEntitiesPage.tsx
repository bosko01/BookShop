import { useEffect, useState } from 'react';
import { DataTable } from '../../components/admin/DataTable';
import {
  AdminBinding,
  AdminGenre,
  AdminPublisher,
  AdminUser,
  createAdminBinding,
  createAdminGenre,
  createAdminPublisher,
  createAdminUser,
  deleteAdminBinding,
  deleteAdminGenre,
  deleteAdminPublisher,
  deleteAdminUser,
  getAdminBindings,
  getAdminGenres,
  getAdminPublishers,
  getAdminUsers,
  updateAdminBinding,
  updateAdminGenre,
  updateAdminPublisher,
  updateAdminUser,
  updateAdminUserRole,
} from '../../services/adminService';
import { useAuth } from '../../state/auth/AuthContext';

const AdminEntitiesPage = () => {
  const { accessToken } = useAuth();
  const [genres, setGenres] = useState<AdminGenre[]>([]);
  const [bindings, setBindings] = useState<AdminBinding[]>([]);
  const [publishers, setPublishers] = useState<AdminPublisher[]>([]);
  const [users, setUsers] = useState<AdminUser[]>([]);

  const [genreName, setGenreName] = useState('');
  const [bindingName, setBindingName] = useState('');
  const [publisherName, setPublisherName] = useState('');
  const [publisherCountry, setPublisherCountry] = useState('');
  const [publisherAddress, setPublisherAddress] = useState('');
  const [publisherCity, setPublisherCity] = useState('');
  const [publisherPhoneNumber, setPublisherPhoneNumber] = useState('');

  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [role, setRole] = useState<'Customer' | 'Admin'>('Customer');

  useEffect(() => {
    const loadData = async () => {
      const [loadedGenres, loadedBindings, loadedPublishers] = await Promise.all([
        getAdminGenres(),
        getAdminBindings(),
        getAdminPublishers(),
      ]);
      setGenres(loadedGenres);
      setBindings(loadedBindings);
      setPublishers(loadedPublishers);

      if (!accessToken) {
        setUsers([]);
        return;
      }

      const loadedUsers = await getAdminUsers(accessToken);
      setUsers(loadedUsers);
    };

    loadData().catch(() => {
      setGenres([]);
      setBindings([]);
      setPublishers([]);
      setUsers([]);
    });
  }, [accessToken]);

  const reloadPage = () => window.location.reload();

  return (
    <div className="space-y-8">
      <h2 className="text-2xl font-bold text-slate-900">Entities Management</h2>

      <section className="space-y-3">
        <div className="flex items-center justify-between">
          <h3 className="text-xl font-semibold text-slate-800">Genres</h3>
          <form
            className="flex gap-2"
            onSubmit={async (event) => {
              event.preventDefault();
              if (!accessToken) return;
              await createAdminGenre(accessToken, genreName);
              reloadPage();
            }}
          >
            <input required value={genreName} onChange={(event) => setGenreName(event.target.value)} placeholder="New genre" className="rounded-xl border border-slate-200 px-3 py-2 text-sm" />
            <button className="rounded-xl bg-brand-500 px-4 py-2 text-sm font-semibold text-white">Create</button>
          </form>
        </div>
        <DataTable
          headers={['ID', 'Name', 'Actions']}
          rows={genres.map((genre) => (
            <tr key={genre.id} className="border-t border-slate-100">
              <td className="px-4 py-3">{genre.id}</td>
              <td className="px-4 py-3">{genre.name}</td>
              <td className="px-4 py-3">
                <div className="flex gap-2">
                  <button
                    className="rounded-lg bg-slate-100 px-3 py-1 text-xs"
                    onClick={async () => {
                      if (!accessToken) return;
                      const nextName = window.prompt('Update genre name', genre.name);
                      if (!nextName || nextName === genre.name) return;
                      await updateAdminGenre(accessToken, genre.id, nextName);
                      reloadPage();
                    }}
                  >
                    Edit
                  </button>
                  <button
                    className="rounded-lg bg-red-50 px-3 py-1 text-xs text-red-600"
                    onClick={async () => {
                      if (!accessToken || !window.confirm('Delete this genre?')) return;
                      await deleteAdminGenre(accessToken, genre.id);
                      reloadPage();
                    }}
                  >
                    Delete
                  </button>
                </div>
              </td>
            </tr>
          ))}
        />
      </section>

      <section className="space-y-3">
        <div className="flex items-center justify-between">
          <h3 className="text-xl font-semibold text-slate-800">Bindings</h3>
          <form
            className="flex gap-2"
            onSubmit={async (event) => {
              event.preventDefault();
              if (!accessToken) return;
              await createAdminBinding(accessToken, bindingName);
              reloadPage();
            }}
          >
            <input required value={bindingName} onChange={(event) => setBindingName(event.target.value)} placeholder="New binding" className="rounded-xl border border-slate-200 px-3 py-2 text-sm" />
            <button className="rounded-xl bg-brand-500 px-4 py-2 text-sm font-semibold text-white">Create</button>
          </form>
        </div>
        <DataTable
          headers={['ID', 'Name', 'Actions']}
          rows={bindings.map((binding) => (
            <tr key={binding.id} className="border-t border-slate-100">
              <td className="px-4 py-3">{binding.id}</td>
              <td className="px-4 py-3">{binding.name}</td>
              <td className="px-4 py-3">
                <div className="flex gap-2">
                  <button
                    className="rounded-lg bg-slate-100 px-3 py-1 text-xs"
                    onClick={async () => {
                      if (!accessToken) return;
                      const nextName = window.prompt('Update binding name', binding.name);
                      if (!nextName || nextName === binding.name) return;
                      await updateAdminBinding(accessToken, binding.id, nextName);
                      reloadPage();
                    }}
                  >
                    Edit
                  </button>
                  <button
                    className="rounded-lg bg-red-50 px-3 py-1 text-xs text-red-600"
                    onClick={async () => {
                      if (!accessToken || !window.confirm('Delete this binding?')) return;
                      await deleteAdminBinding(accessToken, binding.id);
                      reloadPage();
                    }}
                  >
                    Delete
                  </button>
                </div>
              </td>
            </tr>
          ))}
        />
      </section>

      <section className="space-y-3">
        <div className="flex items-center justify-between">
          <h3 className="text-xl font-semibold text-slate-800">Publishers</h3>
          <form
            className="grid gap-2 sm:grid-cols-5"
            onSubmit={async (event) => {
              event.preventDefault();
              if (!accessToken) return;
              await createAdminPublisher(accessToken, {
                name: publisherName,
                country: publisherCountry,
                address: publisherAddress,
                city: publisherCity,
                phoneNumber: publisherPhoneNumber,
              });
              reloadPage();
            }}
          >
            <input required value={publisherName} onChange={(event) => setPublisherName(event.target.value)} placeholder="Publisher name" className="rounded-xl border border-slate-200 px-3 py-2 text-sm" />
            <input value={publisherCountry} onChange={(event) => setPublisherCountry(event.target.value)} placeholder="Country" className="rounded-xl border border-slate-200 px-3 py-2 text-sm" />
            <input value={publisherAddress} onChange={(event) => setPublisherAddress(event.target.value)} placeholder="Address" className="rounded-xl border border-slate-200 px-3 py-2 text-sm" />
            <input value={publisherCity} onChange={(event) => setPublisherCity(event.target.value)} placeholder="City" className="rounded-xl border border-slate-200 px-3 py-2 text-sm" />
            <input value={publisherPhoneNumber} onChange={(event) => setPublisherPhoneNumber(event.target.value)} placeholder="Phone number" className="rounded-xl border border-slate-200 px-3 py-2 text-sm" />
            <button className="sm:col-span-5 rounded-xl bg-brand-500 px-4 py-2 text-sm font-semibold text-white">Create</button>
          </form>
        </div>
        <DataTable
          headers={['ID', 'Name', 'Country', 'Address', 'City', 'Phone', 'Actions']}
          rows={publishers.map((publisher) => (
            <tr key={publisher.id} className="border-t border-slate-100">
              <td className="px-4 py-3">{publisher.id}</td>
              <td className="px-4 py-3">{publisher.name}</td>
              <td className="px-4 py-3">{publisher.country ?? '-'}</td>
              <td className="px-4 py-3">{publisher.address ?? '-'}</td>
              <td className="px-4 py-3">{publisher.city ?? '-'}</td>
              <td className="px-4 py-3">{publisher.phoneNumber ?? '-'}</td>
              <td className="px-4 py-3">
                <div className="flex gap-2">
                  <button
                    className="rounded-lg bg-slate-100 px-3 py-1 text-xs"
                    onClick={async () => {
                      if (!accessToken) return;
                      const nextName = window.prompt('Update publisher name', publisher.name);
                      if (!nextName) return;
                      const nextCountry = window.prompt('Update publisher country', publisher.country ?? '') ?? '';
                      const nextAddress = window.prompt('Update publisher address', publisher.address ?? '') ?? '';
                      const nextCity = window.prompt('Update publisher city', publisher.city ?? '') ?? '';
                      const nextPhoneNumber = window.prompt('Update publisher phone number', publisher.phoneNumber ?? '') ?? '';
                      await updateAdminPublisher(accessToken, publisher.id, {
                        name: nextName,
                        country: nextCountry,
                        address: nextAddress,
                        city: nextCity,
                        phoneNumber: nextPhoneNumber,
                      });
                      reloadPage();
                    }}
                  >
                    Edit
                  </button>
                  <button
                    className="rounded-lg bg-red-50 px-3 py-1 text-xs text-red-600"
                    onClick={async () => {
                      if (!accessToken || !window.confirm('Delete this publisher?')) return;
                      await deleteAdminPublisher(accessToken, publisher.id);
                      reloadPage();
                    }}
                  >
                    Delete
                  </button>
                </div>
              </td>
            </tr>
          ))}
        />
      </section>

      <section className="space-y-3">
        <div className="flex items-center justify-between">
          <h3 className="text-xl font-semibold text-slate-800">Users</h3>
          <form
            className="grid gap-2 sm:grid-cols-5"
            onSubmit={async (event) => {
              event.preventDefault();
              if (!accessToken) return;
              await createAdminUser(accessToken, { firstName, lastName, email, password, role });
              reloadPage();
            }}
          >
            <input required value={firstName} onChange={(event) => setFirstName(event.target.value)} placeholder="First name" className="rounded-xl border border-slate-200 px-3 py-2 text-sm" />
            <input required value={lastName} onChange={(event) => setLastName(event.target.value)} placeholder="Last name" className="rounded-xl border border-slate-200 px-3 py-2 text-sm" />
            <input required type="email" value={email} onChange={(event) => setEmail(event.target.value)} placeholder="Email" className="rounded-xl border border-slate-200 px-3 py-2 text-sm" />
            <input required type="password" minLength={8} value={password} onChange={(event) => setPassword(event.target.value)} placeholder="Password" className="rounded-xl border border-slate-200 px-3 py-2 text-sm" />
            <select value={role} onChange={(event) => setRole(event.target.value as 'Customer' | 'Admin')} className="rounded-xl border border-slate-200 px-3 py-2 text-sm">
              <option value="Customer">Customer</option>
              <option value="Admin">Admin</option>
            </select>
            <button className="sm:col-span-5 rounded-xl bg-brand-500 px-4 py-2 text-sm font-semibold text-white">Create User</button>
          </form>
        </div>
        <DataTable
          headers={['ID', 'Name', 'Email', 'Role', 'Actions']}
          rows={users.map((user) => (
            <tr key={user.id} className="border-t border-slate-100">
              <td className="px-4 py-3">{user.id}</td>
              <td className="px-4 py-3">{user.firstName} {user.lastName}</td>
              <td className="px-4 py-3">{user.email}</td>
              <td className="px-4 py-3">{user.role}</td>
              <td className="px-4 py-3">
                <div className="flex gap-2">
                  <button
                    className="rounded-lg bg-slate-100 px-3 py-1 text-xs"
                    onClick={async () => {
                      if (!accessToken) return;
                      const nextFirstName = window.prompt('First name', user.firstName);
                      if (!nextFirstName) return;
                      const nextLastName = window.prompt('Last name', user.lastName);
                      if (!nextLastName) return;
                      const nextEmail = window.prompt('Email', user.email);
                      if (!nextEmail) return;
                      const nextRole = window.prompt('Role (Customer/Admin)', user.role);
                      if (!nextRole || !['Customer', 'Admin'].includes(nextRole)) return;

                      await updateAdminUser(accessToken, user.id, {
                        firstName: nextFirstName,
                        lastName: nextLastName,
                        email: nextEmail,
                      });

                      if (nextRole !== user.role) {
                        await updateAdminUserRole(accessToken, user.id, nextRole as 'Customer' | 'Admin');
                      }

                      reloadPage();
                    }}
                  >
                    Edit
                  </button>
                  <button
                    className="rounded-lg bg-red-50 px-3 py-1 text-xs text-red-600"
                    onClick={async () => {
                      if (!accessToken || !window.confirm('Delete this user?')) return;
                      await deleteAdminUser(accessToken, user.id);
                      reloadPage();
                    }}
                  >
                    Delete
                  </button>
                </div>
              </td>
            </tr>
          ))}
        />
      </section>
    </div>
  );
};

export default AdminEntitiesPage;
