
import { useEffect, useState } from "react";
import api from "./api";

export default function App() {
  const [tasks, setTasks] = useState([]);
  const [title, setTitle] = useState("");

  const load = async () => {
    const res = await api.get("/tasks");
    setTasks(res.data);
  };

  useEffect(() => { load(); }, []);

  const add = async (e) => {
    e.preventDefault();
    if (!title.trim()) return;
    await api.post("/tasks", { title });
    setTitle("");
    await load();
  };

  const toggle = async (t) => {
    await api.put(`/tasks/${t.id}`, { ...t, isCompleted: !t.isCompleted });
    await load();
  };

  const del = async (id) => {
    await api.delete(`/tasks/${id}`);
    await load();
  };

  return (
    <div style={{ maxWidth: 640, margin: "3rem auto", fontFamily: "system-ui" }}>
      <h1>Tasky</h1>
      <form onSubmit={add} style={{ display: "flex", gap: 8 }}>
        <input
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          placeholder="New task title"
          style={{ flex: 1, padding: 8 }}
        />
        <button>Add</button>
      </form>

      <ul style={{ listStyle: "none", padding: 0, marginTop: 16 }}>
        {tasks.map((t) => (
          <li key={t.id} style={{
            display: "flex", alignItems: "center", gap: 8,
            padding: "8px 0", borderBottom: "1px solid #eee"
          }}>
            <input type="checkbox" checked={t.isCompleted} onChange={() => toggle(t)} />
            <div style={{ flex: 1, textDecoration: t.isCompleted ? "line-through" : "none" }}>
              {t.title}
            </div>
            <button onClick={() => del(t.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
}
