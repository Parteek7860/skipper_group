document.addEventListener("DOMContentLoaded", function () {

    const select = document.getElementById("projectId");
    if (!select) return;

    select.addEventListener("change", loadProjectGrid);
});

function loadProjectGrid() {
    const id = document.getElementById("projectId").value;
    if (!id) return;

    fetch(getGridUrl + '/' + encodeURIComponent(id), {
        headers: { "X-Requested-With": "XMLHttpRequest" }
    })
        .then(res => res.text())
        .then(html => {
            document.getElementById('projectGridContainer').innerHTML = html;
        })
        .catch(err => console.error(err));
}
