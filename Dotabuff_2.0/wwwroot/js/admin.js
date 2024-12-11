document.addEventListener("DOMContentLoaded", () => {
    const selectElement = document.getElementById("dateFilter");

    if (selectElement) {
        selectElement.addEventListener("change", () => {
            document.forms["filterForm"].submit();
        });
    }
});
