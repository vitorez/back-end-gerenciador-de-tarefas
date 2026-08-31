import { useEffect, useState } from 'react'
import { getTasks, createTask, updateTask, deleteTask } from './api'

const SECTIONS = ['today', 'upcoming', 'done']

const emptyForm = {
  title: '',
  description: '',
  category: '',
  date: '',
  time: '',
  section: 'today',
  color: '#4f8cff',
  completed: false,
}

export default function App() {
  const [tasks, setTasks] = useState([])
  const [form, setForm] = useState(emptyForm)
  const [editingId, setEditingId] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  async function load() {
    try {
      setError(null)
      setTasks(await getTasks())
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    load()
  }, [])

  function setField(field, value) {
    setForm((prev) => ({ ...prev, [field]: value }))
  }

  function cancelEdit() {
    setEditingId(null)
    setForm(emptyForm)
  }

  async function handleSubmit(event) {
    event.preventDefault()
    if (!form.title.trim()) return

    try {
      setError(null)
      if (editingId === null) {
        await createTask(form)
      } else {
        await updateTask(editingId, form)
      }
      cancelEdit()
      await load()
    } catch (err) {
      setError(err.message)
    }
  }

  function startEdit(task) {
    setEditingId(task.id)
    setForm({
      title: task.title ?? '',
      description: task.description ?? '',
      category: task.category ?? '',
      // a API devolve a data já formatada ("31 ago"), então o campo volta vazio;
      // deixando vazio o backend mantém a data original da tarefa.
      date: '',
      time: '',
      section: task.section ?? 'today',
      color: task.color || '#4f8cff',
      completed: task.completed,
    })
  }

  async function toggleCompleted(task) {
    try {
      setError(null)
      await updateTask(task.id, {
        title: task.title,
        description: task.description,
        category: task.category,
        date: '',
        time: '',
        section: task.section,
        color: task.color,
        completed: !task.completed,
      })
      await load()
    } catch (err) {
      setError(err.message)
    }
  }

  async function handleDelete(id) {
    if (!confirm('Excluir esta tarefa?')) return
    try {
      setError(null)
      await deleteTask(id)
      if (editingId === id) cancelEdit()
      await load()
    } catch (err) {
      setError(err.message)
    }
  }

  return (
    <main className="app">
      <h1>Gerenciador de Tarefas</h1>

      <form className="card form" onSubmit={handleSubmit}>
        <h2>{editingId === null ? 'Nova tarefa' : `Editando tarefa #${editingId}`}</h2>

        <label>
          Título
          <input
            value={form.title}
            onChange={(e) => setField('title', e.target.value)}
            placeholder="Ex.: Estudar Vertical Slice"
            required
          />
        </label>

        <label>
          Descrição
          <textarea
            value={form.description}
            onChange={(e) => setField('description', e.target.value)}
            rows={2}
          />
        </label>

        <div className="row">
          <label>
            Categoria
            <input
              value={form.category}
              onChange={(e) => setField('category', e.target.value)}
            />
          </label>
          <label>
            Seção
            <select
              value={form.section}
              onChange={(e) => setField('section', e.target.value)}
            >
              {SECTIONS.map((s) => (
                <option key={s} value={s}>
                  {s}
                </option>
              ))}
            </select>
          </label>
        </div>

        <div className="row">
          <label>
            Data
            <input
              type="date"
              value={form.date}
              onChange={(e) => setField('date', e.target.value)}
            />
          </label>
          <label>
            Hora
            <input
              type="time"
              value={form.time}
              onChange={(e) => setField('time', e.target.value)}
            />
          </label>
          <label className="color">
            Cor
            <input
              type="color"
              value={form.color}
              onChange={(e) => setField('color', e.target.value)}
            />
          </label>
        </div>

        <label className="checkbox">
          <input
            type="checkbox"
            checked={form.completed}
            onChange={(e) => setField('completed', e.target.checked)}
          />
          Concluída
        </label>

        <div className="actions">
          <button type="submit">
            {editingId === null ? 'Adicionar' : 'Salvar'}
          </button>
          {editingId !== null && (
            <button type="button" className="ghost" onClick={cancelEdit}>
              Cancelar
            </button>
          )}
        </div>
      </form>

      {error && <p className="error">{error}</p>}

      {loading ? (
        <p className="muted">Carregando…</p>
      ) : tasks.length === 0 ? (
        <p className="muted">Nenhuma tarefa cadastrada.</p>
      ) : (
        <ul className="list">
          {tasks.map((task) => (
            <li
              key={task.id}
              className={`card task ${task.completed ? 'completed' : ''}`}
              style={{ borderLeftColor: task.color || '#4f8cff' }}
            >
              <input
                type="checkbox"
                checked={task.completed}
                onChange={() => toggleCompleted(task)}
                title="Marcar como concluída"
              />

              <div className="task-body">
                <strong>{task.title}</strong>
                {task.description && <p>{task.description}</p>}
                <small className="muted">
                  {[task.category, task.section, task.date, task.time]
                    .filter(Boolean)
                    .join(' · ')}
                </small>
              </div>

              <div className="actions">
                <button className="ghost" onClick={() => startEdit(task)}>
                  Editar
                </button>
                <button className="danger" onClick={() => handleDelete(task.id)}>
                  Excluir
                </button>
              </div>
            </li>
          ))}
        </ul>
      )}
    </main>
  )
}
