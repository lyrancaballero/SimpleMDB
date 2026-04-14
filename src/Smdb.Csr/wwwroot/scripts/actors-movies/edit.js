import { $, apiFetch, renderStatus, getQueryParam } from "/scripts/common.js";

(async function initAMEdit() {
  const id = getQueryParam("id");
  const form = $("#am-form");
  const statusEl = $("#status");

  if (!id) {
    renderStatus(statusEl, "err", "Missing ?id in URL.");
    form
      .querySelectorAll("input,textarea,button,select")
      .forEach((el) => (el.disabled = true));
    return;
  }

  try {
    const am = await apiFetch(`/actors-movies/${encodeURIComponent(id)}`);
    form.actorId.value = am.actorId ?? "";
    form.movieId.value = am.movieId ?? "";
    renderStatus(statusEl, "ok", "Loaded link. You can edit and save.");
  } catch (err) {
    renderStatus(statusEl, "err", `Failed to load data: ${err.message}`);
    return;
  }

  form.addEventListener("submit", async (ev) => {
    ev.preventDefault();
    const actorId = parseInt(form.actorId.value, 10);
    const movieId = parseInt(form.movieId.value, 10);

    if (isNaN(actorId) || isNaN(movieId) || actorId < 1 || movieId < 1) {
      renderStatus(
        statusEl,
        "err",
        "Valid Actor ID and Movie ID are required.",
      );
      return;
    }

    try {
      const updated = await apiFetch(
        `/actors-movies/${encodeURIComponent(id)}`,
        {
          method: "PUT",
          body: JSON.stringify({ actorId, movieId }),
        },
      );
      renderStatus(
        statusEl,
        "ok",
        `Updated link #${updated.id} (actor ${updated.actorId}, movie ${updated.movieId}).`,
      );
    } catch (err) {
      renderStatus(statusEl, "err", `Update failed: ${err.message}`);
    }
  });
})();
