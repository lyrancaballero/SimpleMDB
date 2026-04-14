import {
  $,
  apiFetch,
  renderStatus,
  clearChildren,
  getQueryParam,
} from "/scripts/common.js";

(async function initAMIndex() {
  const page = Math.max(
    1,
    Number(getQueryParam("page") || localStorage.getItem("page") || "1"),
  );
  const size = Math.min(
    100,
    Math.max(
      1,
      Number(getQueryParam("size") || localStorage.getItem("size") || "9"),
    ),
  );
  localStorage.setItem("page", page);
  localStorage.setItem("size", size);

  const listEl = $("#am-list");
  const statusEl = $("#status");
  const tpl = $("#am-card");

  try {
    const payload = await apiFetch(`/actors-movies?page=${page}&size=${size}`);
    const items = Array.isArray(payload) ? payload : payload.data || [];
    clearChildren(listEl);

    if (items.length === 0) {
      renderStatus(
        statusEl,
        "warn",
        "No actor-movie links found for this page.",
      );
    } else {
      renderStatus(statusEl, "", "");
      for (const am of items) {
        const frag = tpl.content.cloneNode(true);
        frag.querySelector(".id").textContent = am.id;
        frag.querySelector(".actor-id").textContent = am.actorId;
        frag.querySelector(".movie-id").textContent = am.movieId;
        frag.querySelector(".btn-view").href =
          `/actors-movies/view.html?id=${encodeURIComponent(am.id)}`;
        frag.querySelector(".btn-edit").href =
          `/actors-movies/edit.html?id=${encodeURIComponent(am.id)}`;
        frag.querySelector(".btn-delete").dataset.id = am.id;
        listEl.appendChild(frag);
      }
    }

    listEl.addEventListener("click", async (ev) => {
      const btn = ev.target.closest("button.btn-delete[data-id]");
      if (!btn) return;
      const id = btn.dataset.id;
      if (!confirm("Delete this relationship? This cannot be undone.")) return;
      try {
        await apiFetch(`/actors-movies/${encodeURIComponent(id)}`, {
          method: "DELETE",
        });
        renderStatus(statusEl, "ok", `Link ${id} deleted.`);
        setTimeout(() => location.reload(), 2000);
      } catch (err) {
        renderStatus(statusEl, "err", `Delete failed: ${err.message}`);
      }
    });

    // Pagination controls
    const sizeSelect = document.getElementById("page-size");
    const pageSizes = [3, 6, 9, 12, 15];
    for (const s of pageSizes) {
      const opt = document.createElement("option");
      opt.value = s;
      opt.textContent = String(s);
      opt.selected = size == s;
      sizeSelect.appendChild(opt);
    }
    sizeSelect.addEventListener("change", () => {
      const params = new URLSearchParams(window.location.search);
      params.set("page", 1);
      params.set("size", sizeSelect.value);
      localStorage.setItem("page", 1);
      localStorage.setItem("size", sizeSelect.value);
      window.location.href = `${window.location.pathname}?${params.toString()}`;
    });

    $("#page-num").textContent = `Page ${page}`;
    const totalPages = payload.meta?.totalPages || 1;
    const firstPage = page <= 1;
    const lastPage = page >= totalPages;
    const firstBtn = $("#first");
    const prevBtn = $("#prev");
    const nextBtn = $("#next");
    const lastBtn = $("#last");

    firstBtn.href = `?page=1&size=${size}`;
    prevBtn.href = `?page=${page - 1}&size=${size}`;
    nextBtn.href = `?page=${page + 1}&size=${size}`;
    lastBtn.href = `?page=${totalPages}&size=${size}`;

    firstBtn.classList.toggle("disabled", firstPage);
    prevBtn.classList.toggle("disabled", firstPage);
    nextBtn.classList.toggle("disabled", lastPage);
    lastBtn.classList.toggle("disabled", lastPage);
  } catch (err) {
    renderStatus(
      statusEl,
      "err",
      `Failed to fetch actor-movie links: ${err.message}`,
    );
  }
})();
