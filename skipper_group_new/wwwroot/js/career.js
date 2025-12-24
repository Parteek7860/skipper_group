document.addEventListener("DOMContentLoaded", function () {

    const select = document.getElementById("careerId");
    if (!select) return;

    select.addEventListener("change", loadJobsByDivision);
});

function loadJobsByDivision() {

    const emptypeid = document.getElementById("careerId").value || "";

    const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    const token = tokenInput ? tokenInput.value : "";

    fetch(`/career/filter?emptypeid=${encodeURIComponent(emptypeid)}`, {
        method: "GET",
        headers: {
            "X-Requested-With": "XMLHttpRequest",
            "RequestVerificationToken": token
        }
    })
        .then(res => res.text())
        .then(html => {
            document.getElementById("careerJobsContainer").innerHTML = html;
        })
        .catch(err => console.error("Career filter error:", err));
}
