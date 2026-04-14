import { $, apiFetch, renderStatus, getQueryParam } from "/scripts/common.js";

(async function initActorEdit() {
  const id = getQueryParam("id");
  const form = $("#actor-form");
  const statusEl = $("#status");

  if (!id) {
    renderStatus(statusEl, "err", "Missing ?id in URL.");
    form
      .querySelectorAll("input,textarea,button,select")
      .forEach((el) => (el.disabled = true));
    return;
  }

  try {
    const actor = await apiFetch(`/actors/${encodeURIComponent(id)}`);
    form.name.value = actor.name ?? "";
    form.birthYear.value = actor.birthYear ?? "";
    renderStatus(statusEl, "ok", "Loaded actor. You can edit and save.");
  } catch (err) {
    renderStatus(statusEl, "err", `Failed to load data: ${err.message}`);
    return;
  }

  form.addEventListener("submit", async (ev) => {
    ev.preventDefault();
    const name = form.name.value.trim();
    const birthYear = parseInt(form.birthYear.value, 10);

    if (!name) {
      renderStatus(statusEl, "err", "Name is required.");
      return;
    }

    const payload = { name, birthYear: isNaN(birthYear) ? 0 : birthYear };

    try {
      const updated = await apiFetch(`/actors/${encodeURIComponent(id)}`, {
        method: "PUT",
        body: JSON.stringify(payload),
      });
      renderStatus(
        statusEl,
        "ok",
        `Updated actor #${updated.id} "${updated.name}" (born ${updated.birthYear}).`,
      );
    } catch (err) {
      renderStatus(statusEl, "err", `Update failed: ${err.message}`);
    }
  });
})();
