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

    document.querySelector('[data-delete-series-form]')?.addEventListener('submit', (event) => {
        if (!window.confirm('Delete this series permanently? This cannot be undone.')) {
            event.preventDefault();
        }
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

    const seriesForm = document.querySelector('[data-series-form]');
    const titleList = seriesForm?.querySelector('[data-title-list]');
    const titleTemplate = document.querySelector('[data-title-template]');
    const addTitleButton = seriesForm?.querySelector('[data-add-title]');
    const plannedLengthInput = seriesForm?.querySelector('[data-planned-length]');

    if (seriesForm && titleList && titleTemplate && addTitleButton && plannedLengthInput) {
        const setCount = (selector, value) => {
            const element = seriesForm.querySelector(selector);
            if (element) {
                element.textContent = value.toString();
            }
        };

        const updateSummary = () => {
            const rows = Array.from(titleList.querySelectorAll('[data-title-row]'));
            const planned = Math.max(0, Number.parseInt(plannedLengthInput.value, 10) || 0);
            const states = rows.map((row) => row.querySelector('input[data-field="State"]:checked')?.value);
            const released = states.filter((state) => state === 'Released' || state === 'Read').length;
            const read = states.filter((state) => state === 'Read').length;

            setCount('[data-count-planned]', planned);
            setCount('[data-count-known]', rows.length);
            setCount('[data-count-unannounced]', Math.max(0, planned - rows.length));
            setCount('[data-count-released]', released);
            setCount('[data-count-read]', read);
            seriesForm.querySelector('[data-empty-titles]')?.classList.toggle('is-hidden', rows.length > 0);
            addTitleButton.disabled = planned > 0 && rows.length >= planned;
        };

        const reindexRows = () => {
            const rows = Array.from(titleList.querySelectorAll('[data-title-row]'));
            const selectedStates = rows.map((row) => row.querySelector('input[data-field="State"]:checked')?.value || 'Upcoming');
            rows.forEach((row, index) => {
                const position = index + 1;
                row.querySelector('[data-index-token]').value = index.toString();
                row.querySelector('[data-title-position]').textContent = position.toString();

                const idInput = row.querySelector('[data-field="Id"]');
                idInput.name = `Form.Titles[${index}].Id`;

                const titleInput = row.querySelector('[data-field="Title"]');
                titleInput.name = `Form.Titles[${index}].Title`;
                titleInput.id = `title-${index}`;
                const titleLabel = row.querySelector('[data-title-label]');
                titleLabel.htmlFor = titleInput.id;
                titleLabel.textContent = `Title ${position}`;

                const legend = row.querySelector('legend');
                legend.textContent = `Status for title ${position}`;
                row.querySelectorAll('input[data-field="State"]').forEach((input) => {
                    input.name = `Form.Titles[${index}].State`;
                    input.id = `title-${index}-state-${input.value.toLowerCase()}`;
                    input.nextElementSibling.htmlFor = input.id;
                });

                row.querySelector('[data-move-up]').disabled = index === 0;
                row.querySelector('[data-move-down]').disabled = index === rows.length - 1;
            });

            rows.forEach((row, index) => {
                const selectedInput = Array.from(row.querySelectorAll('input[data-field="State"]'))
                    .find((input) => input.value === selectedStates[index]);
                selectedInput.checked = true;
            });

            updateSummary();
        };

        addTitleButton.addEventListener('click', () => {
            const index = titleList.querySelectorAll('[data-title-row]').length;
            const fragment = titleTemplate.content.cloneNode(true);
            const wrapper = document.createElement('div');
            wrapper.append(fragment);
            wrapper.innerHTML = wrapper.innerHTML
                .replaceAll('__index__', index.toString())
                .replaceAll('__position__', (index + 1).toString());
            const row = wrapper.firstElementChild;
            titleList.append(row);
            reindexRows();
            row.querySelector('[data-field="Title"]')?.focus();
        });

        titleList.addEventListener('click', (event) => {
            const button = event.target.closest('button');
            const row = button?.closest('[data-title-row]');
            if (!button || !row) {
                return;
            }

            if (button.matches('[data-remove-title]')) {
                row.remove();
            } else if (button.matches('[data-move-up]') && row.previousElementSibling) {
                titleList.insertBefore(row, row.previousElementSibling);
            } else if (button.matches('[data-move-down]') && row.nextElementSibling) {
                titleList.insertBefore(row.nextElementSibling, row);
            }

            reindexRows();
        });

        titleList.addEventListener('change', updateSummary);
        plannedLengthInput.addEventListener('input', updateSummary);
        reindexRows();
    }
});
