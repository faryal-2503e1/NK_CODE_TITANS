/* =========================================================
   FANHUB+ - USER DASHBOARD
   ========================================================= */

"use strict";

document.addEventListener("DOMContentLoaded", () => {

    /* =====================================================
       ELEMENTS
       ===================================================== */

    const sidebar =
        document.getElementById("dashboardSidebar");

    const sidebarOverlay =
        document.getElementById("dashboardSidebarOverlay");

    const menuButton =
        document.getElementById("dashboardMenuButton");

    const sidebarClose =
        document.getElementById("dashboardSidebarClose");


    const navigationLinks =
        document.querySelectorAll(
            "[data-dashboard-section]"
        );

    const dashboardSections =
        document.querySelectorAll(
            ".dashboard-page-section"
        );

    const sectionOpenButtons =
        document.querySelectorAll(
            "[data-open-dashboard-section]"
        );


    const favoriteCards =
        document.querySelectorAll(
            ".favorite-selection-card[data-fandom]"
        );

    const favoriteCount =
        document.getElementById("favoriteCount");

    const favoritePreview =
        document.getElementById(
            "dashboardFavoritePreview"
        );


    const bookmarkCount =
        document.getElementById("bookmarkCount");

    const sidebarBookmarkCount =
        document.getElementById(
            "sidebarBookmarkCount"
        );

    const bookmarksGrid =
        document.getElementById(
            "dashboardBookmarksGrid"
        );

    const bookmarkEmptyState =
        document.getElementById(
            "bookmarkEmptyState"
        );


    const ratingCount =
        document.getElementById("ratingCount");

    const noteCount =
        document.getElementById("noteCount");

    const notesGrid =
        document.getElementById(
            "dashboardNotesGrid"
        );

    const notesEmptyState =
        document.getElementById(
            "notesEmptyState"
        );


    const dashboardSearch =
        document.getElementById(
            "dashboardSearch"
        );


    const profileSettingsButton =
        document.getElementById(
            "profileSettingsButton"
        );

    const topProfileButton =
        document.getElementById(
            "topProfileButton"
        );

    const profileForm =
        document.getElementById("profileForm");

    const profileName =
        document.getElementById("profileName");

    const profileEmail =
        document.getElementById("profileEmail");

    const profileBio =
        document.getElementById("profileBio");

    const dashboardUserName =
        document.getElementById(
            "dashboardUserName"
        );

    const welcomeUserName =
        document.getElementById(
            "welcomeUserName"
        );


    const displaySettingsButton =
        document.getElementById(
            "displaySettingsButton"
        );

    const modalThemeButton =
        document.getElementById(
            "modalThemeButton"
        );

    const modalFontButton =
        document.getElementById(
            "modalFontButton"
        );


    const notificationButton =
        document.getElementById(
            "notificationButton"
        );

    const notificationPanel =
        document.getElementById(
            "notificationPanel"
        );


    const logoutButton =
        document.getElementById(
            "dashboardLogoutButton"
        );


    /* =====================================================
       STORAGE KEYS
       ===================================================== */

    const STORAGE = {

        favorites:
            "fanhub-favorite-fandoms",

        bookmarks:
            "fanhub-bookmarks",

        profile:
            "fanhub-user-profile",

        soloRating:
            "fanhubSoloLevelingRating",

        soloNote:
            "fanhubSoloLevelingNote"

    };


    /* =====================================================
       SAFE LOCAL STORAGE
       ===================================================== */

    function getJSON(key, fallback) {

        try {

            const value =
                localStorage.getItem(key);

            if (!value) {
                return fallback;
            }

            return JSON.parse(value);

        } catch (error) {

            return fallback;

        }

    }


    function setJSON(key, value) {

        try {

            localStorage.setItem(
                key,
                JSON.stringify(value)
            );

            return true;

        } catch (error) {

            return false;

        }

    }


    function getText(key, fallback = "") {

        try {

            const value =
                localStorage.getItem(key);

            return value !== null
                ? value
                : fallback;

        } catch (error) {

            return fallback;

        }

    }


    /* =====================================================
       TOAST
       ===================================================== */

    function showDashboardToast(message) {

        /*
         * Use shared app.js toast when available.
         */

        if (
            window.FanHub &&
            typeof window.FanHub.showToast === "function"
        ) {

            window.FanHub.showToast(message);

            return;

        }


        const toastElement =
            document.getElementById("fanHubToast");

        const toastMessage =
            document.getElementById("toastMessage");


        if (toastMessage) {
            toastMessage.textContent = message;
        }


        if (
            toastElement &&
            window.bootstrap &&
            bootstrap.Toast
        ) {

            const toast =
                bootstrap.Toast.getOrCreateInstance(
                    toastElement
                );

            toast.show();

        }

    }


    /* =====================================================
       MOBILE SIDEBAR
       ===================================================== */

    function openSidebar() {

        if (sidebar) {
            sidebar.classList.add("open");
        }

        if (sidebarOverlay) {
            sidebarOverlay.classList.add("show");
        }

        document.body.classList.add(
            "dashboard-menu-open"
        );

    }


    function closeSidebar() {

        if (sidebar) {
            sidebar.classList.remove("open");
        }

        if (sidebarOverlay) {
            sidebarOverlay.classList.remove("show");
        }

        document.body.classList.remove(
            "dashboard-menu-open"
        );

    }


    if (menuButton) {

        menuButton.addEventListener(
            "click",
            openSidebar
        );

    }


    if (sidebarClose) {

        sidebarClose.addEventListener(
            "click",
            closeSidebar
        );

    }


    if (sidebarOverlay) {

        sidebarOverlay.addEventListener(
            "click",
            closeSidebar
        );

    }


    /* =====================================================
       DASHBOARD SECTION SWITCHING
       ===================================================== */

    function openDashboardSection(
        sectionName,
        updateHash = true
    ) {

        if (!sectionName) {
            return;
        }


        const target =
            document.querySelector(
                `.dashboard-page-section[data-section="${sectionName}"]`
            );


        if (!target) {
            return;
        }


        dashboardSections.forEach(
            (section) => {

                section.classList.toggle(
                    "active",
                    section === target
                );

            }
        );


        navigationLinks.forEach(
            (link) => {

                link.classList.toggle(
                    "active",
                    link.dataset.dashboardSection ===
                        sectionName
                );

            }
        );


        if (updateHash) {

            try {

                history.replaceState(
                    null,
                    "",
                    `#${sectionName}`
                );

            } catch (error) {
                // Browser fallback.
            }

        }


        closeSidebar();


        window.scrollTo({
            top: 0,
            behavior: "smooth"
        });

    }


    navigationLinks.forEach((link) => {

        link.addEventListener(
            "click",
            (event) => {

                event.preventDefault();

                openDashboardSection(
                    link.dataset.dashboardSection
                );

            }
        );

    });


    sectionOpenButtons.forEach(
        (button) => {

            button.addEventListener(
                "click",
                () => {

                    openDashboardSection(
                        button.dataset
                            .openDashboardSection
                    );

                }
            );

        }
    );


    function loadSectionFromHash() {

        const hash =
            window.location.hash
                .replace("#", "")
                .trim();


        const validSections = [
            "overview",
            "favorites",
            "bookmarks",
            "activity",
            "notes"
        ];


        if (validSections.includes(hash)) {

            openDashboardSection(
                hash,
                false
            );

        } else {

            openDashboardSection(
                "overview",
                false
            );

        }

    }


    /* =====================================================
       FAVORITE FANDOMS
       ===================================================== */

    const defaultFavorites = [
        "Anime",
        "Gaming",
        "Movies",
        "Manga"
    ];


    const fandomInformation = {

        Anime: {
            icon: "bi-stars",
            className: "anime",
            description: "Anime series & movies"
        },

        Gaming: {
            icon: "bi-controller",
            className: "gaming",
            description: "Games & virtual worlds"
        },

        Movies: {
            icon: "bi-film",
            className: "movies",
            description: "Movies & cinematic universes"
        },

        "TV Shows": {
            icon: "bi-tv",
            className: "tv",
            description: "Series & streaming worlds"
        },

        "K-Pop": {
            icon: "bi-music-note-beamed",
            className: "kpop",
            description: "Artists, groups & releases"
        },

        Comics: {
            icon: "bi-lightning-charge",
            className: "comics",
            description: "Heroes & comic universes"
        },

        Manga: {
            icon: "bi-book",
            className: "manga",
            description: "Manga stories & collections"
        },

        Cosplay: {
            icon: "bi-person-arms-up",
            className: "cosplay",
            description: "Costumes & fan creations"
        }

    };


    function getFavorites() {

        const favorites =
            getJSON(
                STORAGE.favorites,
                defaultFavorites
            );


        return Array.isArray(favorites)
            ? favorites
            : defaultFavorites;

    }


    function saveFavorites(favorites) {

        setJSON(
            STORAGE.favorites,
            favorites
        );

    }


    function syncFavoriteCards() {

        const favorites =
            getFavorites();


        favoriteCards.forEach((card) => {

            const fandom =
                card.dataset.fandom;


            card.classList.toggle(
                "selected",
                favorites.includes(fandom)
            );

        });


        if (favoriteCount) {

            favoriteCount.textContent =
                favorites.length;

        }


        renderFavoritePreview(
            favorites
        );

    }


    function renderFavoritePreview(favorites) {

        if (!favoritePreview) {
            return;
        }


        favoritePreview.innerHTML = "";


        if (favorites.length === 0) {

            favoritePreview.innerHTML = `

                <div class="dashboard-fandom-empty">

                    <i class="bi bi-heart"></i>

                    <span>
                        No favorite fandoms selected.
                    </span>

                </div>

            `;

            return;

        }


        favorites
            .slice(0, 4)
            .forEach((fandom) => {

                const info =
                    fandomInformation[fandom];


                if (!info) {
                    return;
                }


                const item =
                    document.createElement("div");


                item.className =
                    "dashboard-fandom-item";


                item.innerHTML = `

                    <div class="dashboard-fandom-icon ${info.className}">

                        <i class="bi ${info.icon}"></i>

                    </div>

                    <div>

                        <strong>
                            ${escapeHTML(fandom)}
                        </strong>

                        <span>
                            ${escapeHTML(
                                info.description
                            )}
                        </span>

                    </div>

                `;


                favoritePreview.appendChild(
                    item
                );

            });

    }


    favoriteCards.forEach((card) => {

        card.addEventListener(
            "click",
            () => {

                const fandom =
                    card.dataset.fandom;


                let favorites =
                    getFavorites();


                if (
                    favorites.includes(fandom)
                ) {

                    favorites =
                        favorites.filter(
                            (item) =>
                                item !== fandom
                        );

                    showDashboardToast(
                        `${fandom} removed from favorites.`
                    );

                } else {

                    favorites.push(fandom);

                    showDashboardToast(
                        `${fandom} added to favorites.`
                    );

                }


                saveFavorites(favorites);

                syncFavoriteCards();

            }
        );

    });


    /* =====================================================
       BOOKMARKS
       ===================================================== */

    const contentInformation = {

        "Solo Leveling": {
            category: "ANIME",
            icon: "bi-stars",
            link: "content-details.html"
        },

        "Genshin Impact": {
            category: "GAMING",
            icon: "bi-controller",
            link: "explore.html"
        },

        "One Piece": {
            category: "ANIME",
            icon: "bi-stars",
            link: "explore.html"
        },

        "Demon Slayer": {
            category: "ANIME",
            icon: "bi-stars",
            link: "explore.html"
        },

        "Jujutsu Kaisen": {
            category: "ANIME",
            icon: "bi-stars",
            link: "explore.html"
        },

        "Spider-Man": {
            category: "MOVIES",
            icon: "bi-film",
            link: "explore.html"
        },

        "Stranger Things": {
            category: "TV SHOWS",
            icon: "bi-tv",
            link: "explore.html"
        },

        "BLACKPINK": {
            category: "K-POP",
            icon: "bi-music-note-beamed",
            link: "explore.html"
        },

        "Chainsaw Man": {
            category: "MANGA",
            icon: "bi-book",
            link: "explore.html"
        },

        "Marvel Universe": {
            category: "COMICS",
            icon: "bi-lightning-charge",
            link: "explore.html"
        },

        "Cosplay Masters": {
            category: "COSPLAY",
            icon: "bi-person-arms-up",
            link: "explore.html"
        },

        "Cyber Legends": {
            category: "GAMING",
            icon: "bi-controller",
            link: "explore.html"
        }

    };


    function getBookmarks() {

        let bookmarks =
            getJSON(
                STORAGE.bookmarks,
                []
            );


        /*
         * Compatibility with older localStorage
         * bookmark format.
         */

        if (!Array.isArray(bookmarks)) {

            bookmarks = [];

        }


        return bookmarks;

    }


    function normalizeBookmark(bookmark) {

        /*
         * Current bookmark format:
         * ["Solo Leveling", "Genshin Impact"]
         */

        if (
            typeof bookmark === "string"
        ) {

            return bookmark;

        }


        /*
         * Compatibility if an object was stored.
         */

        if (
            bookmark &&
            typeof bookmark === "object"
        ) {

            return (
                bookmark.title ||
                bookmark.name ||
                bookmark.id ||
                ""
            );

        }


        return "";

    }


    function renderBookmarks() {

        if (!bookmarksGrid) {
            return;
        }


        const rawBookmarks =
            getBookmarks();


        const bookmarks =
            rawBookmarks
                .map(normalizeBookmark)
                .filter(Boolean);


        bookmarksGrid.innerHTML = "";


        if (bookmarkCount) {

            bookmarkCount.textContent =
                bookmarks.length;

        }


        if (sidebarBookmarkCount) {

            sidebarBookmarkCount.textContent =
                bookmarks.length;

        }


        if (bookmarks.length === 0) {

            if (bookmarkEmptyState) {

                bookmarkEmptyState.classList.add(
                    "show"
                );

            }

            bookmarksGrid.style.display =
                "none";

            return;

        }


        if (bookmarkEmptyState) {

            bookmarkEmptyState.classList.remove(
                "show"
            );

        }


        bookmarksGrid.style.display =
            "";


        bookmarks.forEach((title) => {

            const info =
                contentInformation[title] || {
                    category: "FANDOM",
                    icon: "bi-stars",
                    link: "explore.html"
                };


            const article =
                document.createElement(
                    "article"
                );


            article.className =
                "dashboard-bookmark-card";


            article.innerHTML = `

                <div class="dashboard-bookmark-cover">

                    <i class="bi ${info.icon}"></i>

                    <button
                        type="button"
                        class="dashboard-bookmark-remove"
                        data-remove-bookmark="${escapeHTML(title)}"
                        aria-label="Remove ${escapeHTML(title)}"
                        title="Remove bookmark"
                    >

                        <i class="bi bi-bookmark-x"></i>

                    </button>

                </div>

                <div class="dashboard-bookmark-content">

                    <span>
                        ${escapeHTML(info.category)}
                    </span>

                    <h3>
                        ${escapeHTML(title)}
                    </h3>

                    <a href="${info.link}">
                        View Content
                        <i class="bi bi-arrow-right"></i>
                    </a>

                </div>

            `;


            bookmarksGrid.appendChild(
                article
            );

        });

    }


    if (bookmarksGrid) {

        bookmarksGrid.addEventListener(
            "click",
            (event) => {

                const removeButton =
                    event.target.closest(
                        "[data-remove-bookmark]"
                    );


                if (!removeButton) {
                    return;
                }


                const title =
                    removeButton.dataset
                        .removeBookmark;


                const rawBookmarks =
                    getBookmarks();


                const updated =
                    rawBookmarks.filter(
                        (bookmark) =>
                            normalizeBookmark(
                                bookmark
                            ) !== title
                    );


                setJSON(
                    STORAGE.bookmarks,
                    updated
                );


                renderBookmarks();


                showDashboardToast(
                    `${title} removed from bookmarks.`
                );

            }
        );

    }


    /* =====================================================
       RATINGS
       ===================================================== */

    function updateRatingCount() {

        if (!ratingCount) {
            return;
        }


        const soloRating =
            Number(
                getText(
                    STORAGE.soloRating,
                    "0"
                )
            );


        ratingCount.textContent =
            soloRating >= 1 &&
            soloRating <= 5
                ? "1"
                : "0";

    }


    /* =====================================================
       NOTES
       ===================================================== */

    function renderNotes() {

        if (!notesGrid) {
            return;
        }


        notesGrid.innerHTML = "";


        const soloNote =
            getText(
                STORAGE.soloNote,
                ""
            ).trim();


        const notes = [];


        if (soloNote) {

            notes.push({
                category: "ANIME",
                title: "Solo Leveling",
                text: soloNote,
                link: "content-details.html"
            });

        }


        if (noteCount) {

            noteCount.textContent =
                notes.length;

        }


        if (notes.length === 0) {

            notesGrid.style.display =
                "none";


            if (notesEmptyState) {

                notesEmptyState.classList.add(
                    "show"
                );

            }

            return;

        }


        notesGrid.style.display = "";


        if (notesEmptyState) {

            notesEmptyState.classList.remove(
                "show"
            );

        }


        notes.forEach((note) => {

            const article =
                document.createElement(
                    "article"
                );


            article.className =
                "dashboard-note-card";


            article.innerHTML = `

                <span>
                    ${escapeHTML(note.category)}
                </span>

                <h3>
                    ${escapeHTML(note.title)}
                </h3>

                <p>
                    ${escapeHTML(note.text)}
                </p>

                <a href="${note.link}">
                    Open Content
                    <i class="bi bi-arrow-right"></i>
                </a>

            `;


            notesGrid.appendChild(
                article
            );

        });

    }


    /* =====================================================
       PROFILE
       ===================================================== */

    const defaultProfile = {

        name: "Haris Qureshi",

        email: "fan@example.com",

        bio: ""

    };


    function getProfile() {

        const profile =
            getJSON(
                STORAGE.profile,
                defaultProfile
            );


        return {
            ...defaultProfile,
            ...profile
        };

    }


    function updateProfileUI() {

        const profile =
            getProfile();


        if (dashboardUserName) {

            dashboardUserName.textContent =
                profile.name;

        }


        if (welcomeUserName) {

            const firstName =
                profile.name
                    .trim()
                    .split(/\s+/)[0];


            welcomeUserName.textContent =
                firstName || "Fan";

        }


        if (profileName) {

            profileName.value =
                profile.name;

        }


        if (profileEmail) {

            profileEmail.value =
                profile.email;

        }


        if (profileBio) {

            profileBio.value =
                profile.bio || "";

        }


        updateAvatarInitials(
            profile.name
        );

    }


    function updateAvatarInitials(name) {

        const initials =
            getInitials(name);


        const avatars = [
            document.querySelector(
                ".dashboard-user-avatar"
            ),
            document.getElementById(
                "topProfileButton"
            ),
            document.querySelector(
                ".dashboard-profile-avatar"
            )
        ];


        avatars.forEach((avatar) => {

            if (!avatar) {
                return;
            }


            /*
             * Sidebar avatar contains
             * online status span.
             */

            const online =
                avatar.querySelector(
                    ".online-indicator"
                );


            if (online) {

                avatar.childNodes.forEach(
                    (node) => {

                        if (
                            node.nodeType ===
                            Node.TEXT_NODE
                        ) {

                            node.textContent =
                                "";

                        }

                    }
                );


                avatar.insertBefore(
                    document.createTextNode(
                        initials
                    ),
                    online
                );

            } else {

                avatar.textContent =
                    initials;

            }

        });

    }


    function openProfileModal() {

        const modalElement =
            document.getElementById(
                "profileModal"
            );


        if (
            !modalElement ||
            !window.bootstrap
        ) {
            return;
        }


        updateProfileUI();


        const modal =
            bootstrap.Modal.getOrCreateInstance(
                modalElement
            );


        modal.show();

    }


    if (profileSettingsButton) {

        profileSettingsButton.addEventListener(
            "click",
            openProfileModal
        );

    }


    if (topProfileButton) {

        topProfileButton.addEventListener(
            "click",
            openProfileModal
        );

    }


    if (profileForm) {

        profileForm.addEventListener(
            "submit",
            (event) => {

                event.preventDefault();


                const name =
                    profileName
                        ? profileName.value.trim()
                        : "";


                const email =
                    profileEmail
                        ? profileEmail.value.trim()
                        : "";


                const bio =
                    profileBio
                        ? profileBio.value.trim()
                        : "";


                if (!name || !email) {

                    showDashboardToast(
                        "Name and email are required."
                    );

                    return;

                }


                if (!isValidEmail(email)) {

                    showDashboardToast(
                        "Please enter a valid email address."
                    );

                    return;

                }


                const profile = {
                    name,
                    email,
                    bio
                };


                setJSON(
                    STORAGE.profile,
                    profile
                );


                updateProfileUI();


                const modalElement =
                    document.getElementById(
                        "profileModal"
                    );


                if (
                    modalElement &&
                    window.bootstrap
                ) {

                    const modal =
                        bootstrap.Modal.getInstance(
                            modalElement
                        );


                    if (modal) {
                        modal.hide();
                    }

                }


                showDashboardToast(
                    "Profile updated successfully."
                );

            }
        );

    }


    /* =====================================================
       DISPLAY PREFERENCES
       ===================================================== */

    function openPreferencesModal() {

        const modalElement =
            document.getElementById(
                "preferencesModal"
            );


        if (
            !modalElement ||
            !window.bootstrap
        ) {
            return;
        }


        const modal =
            bootstrap.Modal.getOrCreateInstance(
                modalElement
            );


        modal.show();

    }


    if (displaySettingsButton) {

        displaySettingsButton.addEventListener(
            "click",
            openPreferencesModal
        );

    }


    /*
     * These trigger the same shared
     * app.js controls already used
     * throughout FanHub+.
     */

    if (modalThemeButton) {

        modalThemeButton.addEventListener(
            "click",
            () => {

                const mainThemeButton =
                    document.getElementById(
                        "themeButton"
                    );


                if (mainThemeButton) {

                    mainThemeButton.click();

                }

            }
        );

    }


    if (modalFontButton) {

        modalFontButton.addEventListener(
            "click",
            () => {

                const mainFontButton =
                    document.getElementById(
                        "fontSizeButton"
                    );


                if (mainFontButton) {

                    mainFontButton.click();

                }

            }
        );

    }


    /* =====================================================
       NOTIFICATIONS
       ===================================================== */

    function closeNotifications() {

        if (notificationPanel) {

            notificationPanel.classList.remove(
                "show"
            );

        }

    }


    if (
        notificationButton &&
        notificationPanel
    ) {

        notificationButton.addEventListener(
            "click",
            (event) => {

                event.stopPropagation();


                notificationPanel.classList.toggle(
                    "show"
                );

            }
        );


        notificationPanel.addEventListener(
            "click",
            (event) => {

                event.stopPropagation();

            }
        );


        document.addEventListener(
            "click",
            closeNotifications
        );

    }


    /* =====================================================
       DASHBOARD SEARCH
       ===================================================== */

    function handleDashboardSearch() {

        if (!dashboardSearch) {
            return;
        }


        const query =
            dashboardSearch.value.trim();


        if (!query) {

            showDashboardToast(
                "Type something to search."
            );

            dashboardSearch.focus();

            return;

        }


        window.location.href =
            `explore.html?search=${encodeURIComponent(
                query
            )}`;

    }


    if (dashboardSearch) {

        dashboardSearch.addEventListener(
            "keydown",
            (event) => {

                if (event.key === "Enter") {

                    event.preventDefault();

                    handleDashboardSearch();

                }

            }
        );

    }


    /* =====================================================
       LOGOUT
       ===================================================== */

    if (logoutButton) {

        logoutButton.addEventListener(
            "click",
            () => {

                /*
                 * Frontend demo logout.
                 * User preferences/bookmarks remain saved.
                 */

                try {

                    localStorage.removeItem(
                        "fanhub-demo-user"
                    );

                    localStorage.removeItem(
                        "fanhub-user"
                    );

                    sessionStorage.clear();

                } catch (error) {
                    // Storage may be unavailable.
                }


                showDashboardToast(
                    "Logged out successfully."
                );


                setTimeout(
                    () => {

                        window.location.href =
                            "login.html";

                    },
                    700
                );

            }
        );

    }


    /* =====================================================
       HELPER FUNCTIONS
       ===================================================== */

    function escapeHTML(value) {

        return String(value)
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#039;");

    }


    function getInitials(name) {

        const words =
            String(name)
                .trim()
                .split(/\s+/)
                .filter(Boolean);


        if (words.length === 0) {
            return "FH";
        }


        if (words.length === 1) {

            return words[0]
                .substring(0, 2)
                .toUpperCase();

        }


        return (
            words[0][0] +
            words[words.length - 1][0]
        ).toUpperCase();

    }


    function isValidEmail(email) {

        /*
         * Use shared helper if available.
         */

        if (
            window.FanHub &&
            typeof window.FanHub.isValidEmail ===
                "function"
        ) {

            return window.FanHub.isValidEmail(
                email
            );

        }


        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/
            .test(email);

    }


    /* =====================================================
       STORAGE SYNC
       ===================================================== */

    window.addEventListener(
        "storage",
        (event) => {

            if (
                event.key === STORAGE.bookmarks
            ) {

                renderBookmarks();

            }


            if (
                event.key === STORAGE.favorites
            ) {

                syncFavoriteCards();

            }


            if (
                event.key === STORAGE.soloRating
            ) {

                updateRatingCount();

            }


            if (
                event.key === STORAGE.soloNote
            ) {

                renderNotes();

            }


            if (
                event.key === STORAGE.profile
            ) {

                updateProfileUI();

            }

        }
    );


    /* =====================================================
       ESCAPE KEY
       ===================================================== */

    document.addEventListener(
        "keydown",
        (event) => {

            if (event.key !== "Escape") {
                return;
            }


            closeSidebar();

            closeNotifications();

        }
    );


    /* =====================================================
       INITIALIZE DASHBOARD
       ===================================================== */

    loadSectionFromHash();

    syncFavoriteCards();

    renderBookmarks();

    updateRatingCount();

    renderNotes();

    updateProfileUI();

});