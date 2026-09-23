document.addEventListener('DOMContentLoaded', () => {
    const themeToggle = document.querySelector('[data-theme-toggle]');
    const themeIcon = document.querySelector('[data-theme-icon]');
    const themeLabel = document.querySelector('[data-theme-label]');

    const updateThemeToggle = () => {
        const isDark = document.documentElement.dataset.theme === 'dark';
        themeToggle?.setAttribute('aria-label', isDark ? 'Switch to light theme' : 'Switch to dark theme');
        themeToggle?.setAttribute('aria-pressed', isDark.toString());
        if (themeIcon) {
            themeIcon.textContent = isDark ? '\u263c' : '\u263e';
        }
        if (themeLabel) {
            themeLabel.textContent = isDark ? 'Light theme' : 'Dark theme';
        }
    };

    themeToggle?.addEventListener('click', () => {
        const nextTheme = document.documentElement.dataset.theme === 'dark' ? 'light' : 'dark';
        document.documentElement.dataset.theme = nextTheme;
        localStorage.setItem('series-tracker-theme', nextTheme);
        updateThemeToggle();
    });

    updateThemeToggle();

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
