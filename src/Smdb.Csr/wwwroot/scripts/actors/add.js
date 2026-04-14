import { $, apiFetch, renderStatus } from "/scripts/common.js";

const form = $("#actor-form");
const statusEl = $("#status");

form.addEventListener("submit", async (e) => {
  e.preventDefault();
  const name = form.name.value.trim();
  const birthYear = parseInt(form.birthYear.value, 10);
  if (!name) return renderStatus(statusEl, "err", "Name required");
  try {
    const created = await apiFetch("/actors", {
      method: "POST",
      body: JSON.stringify({
        name,
        birthYear: isNaN(birthYear) ? 0 : birthYear,
      }),
    });
    renderStatus(
      statusEl,
      "ok",
      `Created actor #${created.id} ${created.name}`,
    );
    form.reset();
  } catch (err) {
    renderStatus(statusEl, "err", err.message);
  }
});
