import { $, apiFetch, renderStatus } from "/scripts/common.js";

(async function initUserAdd() {
  const form = $("#user-form");
  const statusEl = $("#status");
  renderStatus(statusEl, "ok", "New user. Fill in the details and save.");

  form.addEventListener("submit", async (ev) => {
    ev.preventDefault();
    const username = form.username.value.trim();
    const password = form.password.value;
    const role = form.role.value;

    if (!username || !password) {
      renderStatus(statusEl, "err", "Username and password are required.");
      return;
    }

    try {
      const created = await apiFetch("/users", {
        method: "POST",
        body: JSON.stringify({ username, password, role }),
      });
      renderStatus(
        statusEl,
        "ok",
        `Created user #${created.id} "${created.username}" (${created.role}).`,
      );
      form.reset();
    } catch (err) {
      renderStatus(statusEl, "err", `Create failed: ${err.message}`);
    }
  });
})();
