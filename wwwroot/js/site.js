document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.status-row[data-edit-url]').forEach((row) => {
        const editUrl = row.getAttribute('data-edit-url');
        if (!editUrl) {
            return;
        }

        const openEdit = (event) => {
            if (event.target.closest('a, button, input, select, textarea, label')) {
                return;
            }

            window.location.href = editUrl;
        };

        row.addEventListener('click', openEdit);
        row.addEventListener('keydown', (event) => {
            if (event.key === 'Enter' || event.key === ' ') {
                event.preventDefault();
                window.location.href = editUrl;
            }
        });
    });
});
