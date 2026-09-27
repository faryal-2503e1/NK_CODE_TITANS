// Shared by the public site and the member dashboard:
// dark mode, text size, mobile sidebar toggle and the page-loading spinner.
//
// Uses the SAME localStorage keys and body classes as wwwroot/js/app.js
// (initializeTheme / initializeFontSize) and Views/Shared/_ThemeInit.cshtml,
// so the preference stays in sync no matter which layout/page you're on.
(function () {
    var body = document.body;
    var loader = document.getElementById('page-loader');

    function save(key, value) {
        try {
            if (value === null) localStorage.removeItem(key);
            else localStorage.setItem(key, value);
        } catch (e) { }
    }

    function setTheme(theme) {
        document.documentElement.setAttribute('data-bs-theme', theme === 'light' ? 'light' : 'dark');
        if (theme === 'light') {
            body.classList.add('light-theme');
        } else {
            body.classList.remove('light-theme');
        }
        save('fanhub-theme', theme === 'light' ? 'light' : 'dark');
        document.querySelectorAll('[data-ui="theme-icon"]').forEach(function (icon) {
            icon.classList.toggle('bi-sun', theme === 'light');
            icon.classList.toggle('bi-moon-stars', theme !== 'light');
        });
    }

    function setFont(large) {
        if (large) {
            body.classList.add('large-text');
        } else {
            body.classList.remove('large-text');
        }
        save('fanhub-font-size', large ? 'large' : 'normal');
    }

    function on(name, handler) {
        document.querySelectorAll('[data-ui="' + name + '"]').forEach(function (el) {
            el.addEventListener('click', handler);
        });
    }

    on('theme-toggle', function () {
        setTheme(body.classList.contains('light-theme') ? 'dark' : 'light');
    });
    on('font-smaller', function () { setFont(false); });
    on('font-larger', function () { setFont(true); });
    on('font-reset', function () { setFont(false); });

    var sidebar = document.getElementById('sidebar');
    var sidebarToggle = document.getElementById('sidebar-toggle');
    if (sidebar && sidebarToggle) {
        sidebarToggle.addEventListener('click', function () {
            sidebar.classList.toggle('active');
        });
    }

    if (!loader) return;

    function showLoader() { loader.classList.remove('d-none'); }

    document.addEventListener('click', function (e) {
        var link = e.target.closest('a[href]');
        if (!link || e.defaultPrevented || e.ctrlKey || e.metaKey || e.shiftKey || e.button !== 0) return;
        if (link.target === '_blank' || link.hasAttribute('download') || link.hasAttribute('data-bs-toggle')) return;
        var href = link.getAttribute('href');
        if (!href || href.charAt(0) === '#' || href.indexOf('javascript:') === 0) return;
        if (link.origin !== location.origin) return;
        showLoader();
    });

    document.addEventListener('submit', function (e) {
        // Runs after validation, so an invalid form (default prevented) does not show the spinner
        setTimeout(function () { if (!e.defaultPrevented) showLoader(); }, 0);
    });

    window.addEventListener('pageshow', function () { loader.classList.add('d-none'); });
})();
