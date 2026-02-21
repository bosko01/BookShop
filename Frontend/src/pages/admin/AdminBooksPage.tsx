import { useEffect, useState } from 'react';
import { BookFormModal } from '../../components/admin/BookFormModal';
import { DataTable } from '../../components/admin/DataTable';
import { createAdminBook, deleteAdminBook, getAdminBooks, updateAdminBook } from '../../services/adminService';
import { useAuth } from '../../state/auth/AuthContext';
import { Book } from '../../types/book';

const AdminBooksPage = () => {
  const { accessToken } = useAuth();
  const [books, setBooks] = useState<Book[]>([]);
  const [open, setOpen] = useState(false);
  const [editing, setEditing] = useState<Book | null>(null);

  const loadBooks = async () => {
    const result = await getAdminBooks();
    setBooks(result);
  };

  useEffect(() => {
    loadBooks().catch(() => setBooks([]));
  }, []);

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h2 className="text-2xl font-bold text-slate-900">Books Management</h2>
        <button onClick={() => { setEditing(null); setOpen(true); }} className="rounded-xl bg-brand-500 px-4 py-2 font-semibold text-white hover:bg-brand-600">Create Book</button>
      </div>
      <DataTable
        headers={['Title', 'Author', 'Price', 'Stock', 'Category', 'Actions']}
        rows={books.map((book) => (
          <tr key={book.id} className="border-t border-slate-100">
            <td className="px-4 py-3 font-medium">{book.title}</td>
            <td className="px-4 py-3">{book.author}</td>
            <td className="px-4 py-3">${book.price.toFixed(2)}</td>
            <td className="px-4 py-3">{book.stock}</td>
            <td className="px-4 py-3">{book.category}</td>
            <td className="px-4 py-3">
              <div className="flex gap-2">
                <button onClick={() => { setEditing(book); setOpen(true); }} className="rounded-lg bg-slate-100 px-3 py-1 text-xs">Edit</button>
                <button onClick={async () => { if (accessToken) { await deleteAdminBook(accessToken, book.id); await loadBooks(); } }} className="rounded-lg bg-red-50 px-3 py-1 text-xs text-red-600">Delete</button>
              </div>
            </td>
          </tr>
        ))}
      />
      <BookFormModal
        open={open}
        onClose={() => setOpen(false)}
        initial={editing}
        onSubmit={async (values) => {
          if (!accessToken) return;
          if (editing) {
            await updateAdminBook(accessToken, editing.id, values);
          } else {
            await createAdminBook(accessToken, values);
          }
          await loadBooks();
        }}
      />
    </div>
  );
};

export default AdminBooksPage;
