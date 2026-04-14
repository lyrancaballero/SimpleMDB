import { $, apiFetch, renderStatus, getQueryParam } from "/scripts/common.js";

(async function initUserEdit() {
  const id = getQueryParam("id");
  const form = $("#user-form");
  const statusEl = $("#status");

  if (!id) {
    renderStatus(statusEl, "err", "Missing ?id in URL.");
    form
      .querySelectorAll("input,textarea,button,select")
      .forEach((el) => (el.disabled = true));
    return;
  }

  try {
    const user = await apiFetch(`/users/${encodeURIComponent(id)}`);
    form.username.value = user.username ?? "";
    form.role.value = user.role ?? "user";
    renderStatus(statusEl, "ok", "Loaded user. You can edit and save.");
  } catch (err) {
    renderStatus(statusEl, "err", `Failed to load data: ${err.message}`);
    return;
  }

  form.addEventListener("submit", async (ev) => {
    ev.preventDefault();
    const username = form.username.value.trim();
    const password = form.password.value.trim();
    const role = form.role.value;

    if (!username) {
      renderStatus(statusEl, "err", "Username is required.");
      return;
    }

    const payload = { username, role };
    if (password) payload.password = password;

    try {
      const updated = await apiFetch(`/users/${encodeURIComponent(id)}`, {
        method: "PUT",
        body: JSON.stringify(payload),
      });
      renderStatus(
        statusEl,
        "ok",
        `Updated user #${updated.id} "${updated.username}" (${updated.role}).`,
      );
    } catch (err) {
      renderStatus(statusEl, "err", `Update failed: ${err.message}`);
    }
  });
})();
