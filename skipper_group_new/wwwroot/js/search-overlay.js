document.addEventListener("DOMContentLoaded", function () {

    document.addEventListener("click", function (e) {
        if (e.target.closest(".header-search")) {
            e.preventDefault();
            const overlay = document.getElementById("searchOverlay");
            overlay.classList.add("active");
            overlay.querySelector("input").focus();
        }

        if (e.target.closest(".search-close")) {
            document.getElementById("searchOverlay").classList.remove("active");
        }

    });

    document.addEventListener("keydown", function (e) {
        if (e.key === "Escape") {
            document.getElementById("searchOverlay").classList.remove("active");
        }
    });

});
