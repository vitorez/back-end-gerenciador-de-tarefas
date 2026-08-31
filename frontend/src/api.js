// em produção fica vazio: front e API são servidos pela mesma origem
const BASE_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5215'

async function request(path, { body, ...options } = {}) {
  const res = await fetch(`${BASE_URL}${path}`, {
    ...options,
    // só manda Content-Type quando há corpo, evitando preflight desnecessário
    ...(body === undefined
      ? {}
      : { body, headers: { 'Content-Type': 'application/json' } }),
  })

  if (!res.ok) {
    let message = `Erro ${res.status}`
    try {
      const body = await res.json()
      if (body?.message) message = body.message
    } catch {
      /* resposta sem corpo JSON */
    }
    throw new Error(message)
  }

  if (res.status === 204) return null
  return res.json()
}

export const getTasks = () => request('/api/tasks')
export const createTask = (task) =>
  request('/api/tasks', { method: 'POST', body: JSON.stringify(task) })
export const updateTask = (id, task) =>
  request(`/api/tasks/${id}`, { method: 'PUT', body: JSON.stringify(task) })
export const deleteTask = (id) =>
  request(`/api/tasks/${id}`, { method: 'DELETE' })
