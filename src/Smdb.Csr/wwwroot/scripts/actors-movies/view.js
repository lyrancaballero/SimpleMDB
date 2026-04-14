import { $, apiFetch, renderStatus, getQueryParam } from "/scripts/common.js";

(async function initAMView() {
  const id = getQueryParam("id");
  const statusEl = $("#status");
  if (!id) return renderStatus(statusEl, "err", "Missing ?id in URL.");

  try {
    const am = await apiFetch(`/actors-movies/${encodeURIComponent(id)}`);
    $("#am-id").textContent = am.id;
    $("#am-actorId").textContent = am.actorId;
    $("#am-movieId").textContent = am.movieId;
    $("#edit-link").href =
      `/actors-movies/edit.html?id=${encodeURIComponent(am.id)}`;
    renderStatus(statusEl, "ok", "Link loaded successfully.");
  } catch (err) {
    renderStatus(statusEl, "err", `Failed to load link ${id}: ${err.message}`);
  }
})();
