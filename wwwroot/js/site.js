document.addEventListener('DOMContentLoaded', () => {
    const editor = document.querySelector('.series-editor');
    const editorCloseLink = document.querySelector('.editor-close');

    if (editor) {
        document.body.classList.add('editor-open');

        const focusableSelector = 'a[href], button:not([disabled]), input:not([disabled]), select:not([disabled]), textarea:not([disabled])';
        const focusableElements = () => Array.from(editor.querySelectorAll(focusableSelector));

        window.requestAnimationFrame(() => {
            const firstInput = editor.querySelector('input[autofocus]') || focusableElements()[0];
            firstInput?.focus();
        });

        editor.addEventListener('keydown', (event) => {
            if (event.key === 'Escape') {
                editorCloseLink?.click();
                return;
            }

            if (event.key !== 'Tab') {
                return;
            }

            const elements = focusableElements();
            if (elements.length === 0) {
                return;
            }

            const firstElement = elements[0];
            const lastElement = elements[elements.length - 1];
            if (event.shiftKey && document.activeElement === firstElement) {
                event.preventDefault();
                lastElement.focus();
            } else if (!event.shiftKey && document.activeElement === lastElement) {
                event.preventDefault();
                firstElement.focus();
            }
        });
    }

    document.querySelector('[data-editor-close]')?.addEventListener('click', () => {
        editorCloseLink?.click();
    });

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
