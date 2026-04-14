import { $, apiFetch, renderStatus, getQueryParam } from "/scripts/common.js";

(async function initUserView() {
  const id = getQueryParam("id");
  const statusEl = $("#status");
  if (!id) return renderStatus(statusEl, "err", "Missing ?id in URL.");

  try {
    const user = await apiFetch(`/users/${encodeURIComponent(id)}`);
    $("#user-id").textContent = user.id;
    $("#user-username").textContent = user.username;
    $("#user-role").textContent = user.role;
    $("#edit-link").href = `/users/edit.html?id=${encodeURIComponent(user.id)}`;
    renderStatus(statusEl, "ok", "User loaded successfully.");
  } catch (err) {
    renderStatus(statusEl, "err", `Failed to load user ${id}: ${err.message}`);
  }
})();
