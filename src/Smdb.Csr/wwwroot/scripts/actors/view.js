import { $, apiFetch, renderStatus, getQueryParam } from "/scripts/common.js";

(async function initActorView() {
  const id = getQueryParam("id");
  const statusEl = $("#status");
  if (!id) return renderStatus(statusEl, "err", "Missing ?id in URL.");

  try {
    const actor = await apiFetch(`/actors/${encodeURIComponent(id)}`);
    $("#actor-id").textContent = actor.id;
    $("#actor-name").textContent = actor.name;
    $("#actor-birthYear").textContent = actor.birthYear ?? "—";
    $("#edit-link").href =
      `/actors/edit.html?id=${encodeURIComponent(actor.id)}`;
    renderStatus(statusEl, "ok", "Actor loaded successfully.");
  } catch (err) {
    renderStatus(statusEl, "err", `Failed to load actor ${id}: ${err.message}`);
  }
})();
