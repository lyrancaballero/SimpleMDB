import { $, apiFetch, renderStatus } from "/scripts/common.js";

const form = $("#am-form");
const statusEl = $("#status");
const actorSelect = $("#actorId");
const movieSelect = $("#movieId");

async function loadOptions() {
  try {
    const actors = await apiFetch("/actors?page=1&size=100");
    const movies = await apiFetch("/movies?page=1&size=100");
    for (const a of actors.data || []) {
      const opt = document.createElement("option");
      opt.value = a.id;
      opt.textContent = `${a.name} (${a.birthYear})`;
      actorSelect.appendChild(opt);
    }
    for (const m of movies.data || []) {
      const opt = document.createElement("option");
      opt.value = m.id;
      opt.textContent = `${m.title} (${m.year})`;
      movieSelect.appendChild(opt);
    }
    renderStatus(statusEl, "ok", "Select actor and movie.");
  } catch (err) {
    renderStatus(statusEl, "err", "Failed to load data: " + err.message);
  }
}

loadOptions();

form.addEventListener("submit", async (e) => {
  e.preventDefault();
  const actorId = parseInt(actorSelect.value, 10);
  const movieId = parseInt(movieSelect.value, 10);
  if (!actorId || !movieId) return renderStatus(statusEl, "err", "Select both");
  try {
    const created = await apiFetch("/actors-movies", {
      method: "POST",
      body: JSON.stringify({ actorId, movieId }),
    });
    renderStatus(statusEl, "ok", `Link created with ID ${created.id}`);
    form.reset();
  } catch (err) {
    renderStatus(statusEl, "err", err.message);
  }
});
