// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Attach change event listeners to all checkboxes
document.querySelectorAll('input[name="selectedContinents"]').forEach(checkbox => {
    checkbox.addEventListener('change', function () {
        // Get the number of checked checkboxes
        var checkedCount = document.querySelectorAll('input[name="selectedContinents"]:checked').length;

        // If no checkboxes are checked, prevent unchecking and show alert
        if (checkedCount === 0) {
            // Revert the checkbox to its previous state (keep it checked)
            this.checked = true;
            alert('You must select at least one continent!');

        }
    });
});

// Trigger form submit whenever a checkbox is changed (if needed)
document.querySelectorAll('input[name="selectedContinents"]').forEach(checkbox => {
    checkbox.addEventListener('change', function () {
        // Automatically submit the form whenever the checkbox is changed
        document.getElementById('continentForm').submit();
    });
});