console.log("project-filter.js loaded");

document.addEventListener("DOMContentLoaded", function () {

    const ddl = document.getElementById("projectid");

    if (!ddl) {
        console.error("Dropdown not found");
        return;
    }

    ddl.addEventListener("change", function () {
        console.log("Change detected");

        const id = this.value;
        if (!id) return;

        fetch("/SkipperHome/GetGridByProject", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "X-Requested-With": "XMLHttpRequest"
            },
            body: JSON.stringify({ id: id })
        })
            .then(res => {
                console.log("Response status:", res.status);
                return res.text();
            })
            .then(html => {
                document.getElementById("projectListContainer").innerHTML = html;
            })
            .catch(err => console.error("Fetch error:", err));
    });
});
