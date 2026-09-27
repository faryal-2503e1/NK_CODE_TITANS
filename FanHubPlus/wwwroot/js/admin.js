/* =========================================================
   FANHUB+ - ADMIN DASHBOARD
   Frontend Administration Demonstration
   ========================================================= */

"use strict";

document.addEventListener("DOMContentLoaded", () => {

    /* =====================================================
       ELEMENTS
       ===================================================== */

    const sidebar =
        document.getElementById("adminSidebar");

    const sidebarOverlay =
        document.getElementById("adminSidebarOverlay");

    const menuButton =
        document.getElementById("adminMenuButton");

    const closeSidebarButton =
        document.getElementById("adminSidebarClose");

    const navigationButtons =
        document.querySelectorAll(
            "[data-admin-section]"
        );

    const sectionTargetButtons =
        document.querySelectorAll(
            "[data-admin-section-target]"
        );

    const sections =
        document.querySelectorAll(
            ".admin-page-section"
        );

    const topbarTitle =
        document.getElementById(
            "adminTopbarTitle"
        );

    const adminSearch =
        document.getElementById(
            "adminSearch"
        );

    const notificationButton =
        document.getElementById(
            "adminNotificationButton"
        );

    const notificationPanel =
        document.getElementById(
            "adminNotificationPanel"
        );

    const closeNotificationButton =
        document.getElementById(
            "closeAdminNotifications"
        );

    const logoutButton =
        document.getElementById(
            "adminLogoutButton"
        );

    const demoActionButtons =
        document.querySelectorAll(
            "[data-demo-action]"
        );


    /* =====================================================
       SECTION TITLES
       ===================================================== */

    const sectionTitles = {

        overview:
            "Dashboard",

        content:
            "Content Management",

        multimedia:
            "Multimedia",

        characters:
            "Characters",

        articles:
            "Articles",

        users:
            "Users",

        feedback:
            "Feedback",

        chatbot:
            "Chatbot FAQs"

    };


    /* =====================================================
       TOAST
       ===================================================== */

    function showToast(message) {

        /*
         * Use common FanHub helper when available.
         */

        if (
            window.FanHub &&
            typeof window.FanHub.showToast ===
                "function"
        ) {

            window.FanHub.showToast(
                message
            );

            return;

        }


        const toast =
            document.getElementById(
                "fanHubToast"
            );

        const toastMessage =
            document.getElementById(
                "toastMessage"
            );


        if (toastMessage) {

            toastMessage.textContent =
                message;

        }


        if (
            toast &&
            window.bootstrap &&
            bootstrap.Toast
        ) {

            bootstrap.Toast
                .getOrCreateInstance(toast)
                .show();

        }

    }


    /* =====================================================
       SIDEBAR
       ===================================================== */

    function openSidebar() {

        if (!sidebar) {
            return;
        }


        sidebar.classList.add(
            "open"
        );


        sidebarOverlay?.classList.add(
            "active"
        );


        document.body.classList.add(
            "admin-menu-open"
        );

    }


    function closeSidebar() {

        sidebar?.classList.remove(
            "open"
        );


        sidebarOverlay?.classList.remove(
            "active"
        );


        document.body.classList.remove(
            "admin-menu-open"
        );

    }


    menuButton?.addEventListener(
        "click",
        openSidebar
    );


    closeSidebarButton
        ?.addEventListener(
            "click",
            closeSidebar
        );


    sidebarOverlay
        ?.addEventListener(
            "click",
            closeSidebar
        );


    /* =====================================================
       SECTION SWITCHING
       ===================================================== */

    function showSection(
        sectionName,
        updateHash = true
    ) {

        const section =
            document.getElementById(
                sectionName
            );


        if (!section) {

            return;

        }


        sections.forEach(
            (item) => {

                item.classList.remove(
                    "active"
                );

            }
        );


        navigationButtons.forEach(
            (button) => {

                button.classList.toggle(
                    "active",
                    button.dataset
                        .adminSection ===
                        sectionName
                );

            }
        );


        section.classList.add(
            "active"
        );


        if (topbarTitle) {

            topbarTitle.textContent =
                sectionTitles[
                    sectionName
                ] || "Dashboard";

        }


        if (updateHash) {

            try {

                history.replaceState(
                    null,
                    "",
                    `#${sectionName}`
                );

            } catch (error) {

                /*
                 * Ignore browser history
                 * restrictions.
                 */

            }

        }


        window.scrollTo({
            top: 0,
            behavior: "smooth"
        });


        closeSidebar();

    }


    navigationButtons.forEach(
        (button) => {

            button.addEventListener(
                "click",
                () => {

                    const sectionName =
                        button.dataset
                            .adminSection;


                    if (!sectionName) {
                        return;
                    }


                    showSection(
                        sectionName
                    );

                }
            );

        }
    );


    sectionTargetButtons.forEach(
        (button) => {

            button.addEventListener(
                "click",
                () => {

                    const sectionName =
                        button.dataset
                            .adminSectionTarget;


                    if (!sectionName) {
                        return;
                    }


                    showSection(
                        sectionName
                    );

                }
            );

        }
    );


    /* =====================================================
       LOAD SECTION FROM URL HASH
       ===================================================== */

    function loadSectionFromHash() {

        const sectionName =
            window.location.hash
                .replace("#", "")
                .trim();


        if (
            sectionName &&
            document.getElementById(
                sectionName
            )
        ) {

            showSection(
                sectionName,
                false
            );

            return;

        }


        showSection(
            "overview",
            false
        );

    }


    window.addEventListener(
        "hashchange",
        loadSectionFromHash
    );


    /* =====================================================
       ADMIN SEARCH
       ===================================================== */

    function handleAdminSearch() {

        if (!adminSearch) {
            return;
        }


        const query =
            adminSearch.value
                .trim()
                .toLowerCase();


        if (!query) {

            showToast(
                "Enter something to search."
            );

            return;

        }


        /*
         * Search known administration sections first.
         */

        const sectionKeywords = {

            content: [
                "content",
                "anime",
                "gaming",
                "movie",
                "movies",
                "tv",
                "kpop",
                "k-pop",
                "comic",
                "comics",
                "manga",
                "cosplay"
            ],

            multimedia: [
                "media",
                "multimedia",
                "video",
                "videos",
                "gallery",
                "image",
                "audio",
                "trailer"
            ],

            characters: [
                "character",
                "characters",
                "profile",
                "jin",
                "igris",
                "cha"
            ],

            articles: [
                "article",
                "articles",
                "story",
                "stories",
                "news"
            ],

            users: [
                "user",
                "users",
                "member",
                "members",
                "account"
            ],

            feedback: [
                "feedback",
                "submission",
                "submissions",
                "community"
            ],

            chatbot: [
                "chatbot",
                "bot",
                "faq",
                "faqs",
                "assistant"
            ]

        };


        let matchedSection = null;


        Object.entries(
            sectionKeywords
        ).some(
            ([sectionName, keywords]) => {

                const matched =
                    keywords.some(
                        (keyword) =>
                            query.includes(
                                keyword
                            )
                    );


                if (matched) {

                    matchedSection =
                        sectionName;

                    return true;

                }


                return false;

            }
        );


        if (matchedSection) {

            showSection(
                matchedSection
            );


            showToast(
                `Opened ${sectionTitles[matchedSection]}.`
            );

            return;

        }


        /*
         * Search visible text from each
         * admin section.
         */

        let textMatch = null;


        sections.forEach(
            (section) => {

                if (textMatch) {
                    return;
                }


                const sectionText =
                    section.textContent
                        .toLowerCase();


                if (
                    sectionText.includes(
                        query
                    )
                ) {

                    textMatch =
                        section.id;

                }

            }
        );


        if (textMatch) {

            showSection(
                textMatch
            );


            showToast(
                `Search result found in ${sectionTitles[textMatch] || "Admin"}.`
            );

        } else {

            showToast(
                `No admin result found for "${adminSearch.value.trim()}".`
            );

        }

    }


    adminSearch?.addEventListener(
        "keydown",
        (event) => {

            if (
                event.key === "Enter"
            ) {

                event.preventDefault();

                handleAdminSearch();

            }

        }
    );


    /* =====================================================
       SEARCH KEYBOARD SHORTCUT
       ===================================================== */

    document.addEventListener(
        "keydown",
        (event) => {

            const target =
                event.target;


            const typing =
                target instanceof
                    HTMLInputElement ||
                target instanceof
                    HTMLTextAreaElement ||
                target?.isContentEditable;


            if (
                event.key === "/" &&
                !typing
            ) {

                event.preventDefault();


                adminSearch?.focus();

            }

        }
    );


    /* =====================================================
       NOTIFICATIONS
       ===================================================== */

    function openNotifications() {

        notificationPanel
            ?.classList.add(
                "open"
            );

    }


    function closeNotifications() {

        notificationPanel
            ?.classList.remove(
                "open"
            );

    }


    function toggleNotifications() {

        if (!notificationPanel) {
            return;
        }


        notificationPanel.classList.toggle(
            "open"
        );

    }


    notificationButton
        ?.addEventListener(
            "click",
            (event) => {

                event.stopPropagation();

                toggleNotifications();

            }
        );


    closeNotificationButton
        ?.addEventListener(
            "click",
            closeNotifications
        );


    notificationPanel
        ?.addEventListener(
            "click",
            (event) => {

                event.stopPropagation();

            }
        );


    document.addEventListener(
        "click",
        () => {

            closeNotifications();

        }
    );


    /* =====================================================
       DEMONSTRATION ACTION BUTTONS
       ===================================================== */

    demoActionButtons.forEach(
        (button) => {

            button.addEventListener(
                "click",
                () => {

                    const action =
                        button.dataset
                            .demoAction ||
                        "Admin action";


                    showToast(
                        `${action} is available as a frontend demonstration.`
                    );

                }
            );

        }
    );


    /* =====================================================
       FAQ DETAILS
       ===================================================== */

    const faqItems =
        document.querySelectorAll(
            ".admin-faq-list details"
        );


    faqItems.forEach(
        (item) => {

            item.addEventListener(
                "toggle",
                () => {

                    /*
                     * Keep only one FAQ expanded
                     * for a cleaner admin UI.
                     */

                    if (!item.open) {
                        return;
                    }


                    faqItems.forEach(
                        (otherItem) => {

                            if (
                                otherItem !== item
                            ) {

                                otherItem.open =
                                    false;

                            }

                        }
                    );

                }
            );

        }
    );


    /* =====================================================
       LOGOUT
       ===================================================== */

    logoutButton?.addEventListener(
        "click",
        () => {

            /*
             * This is a frontend demonstration.
             * Real admin authentication and logout
             * should be handled by the backend.
             */

            try {

                sessionStorage.removeItem(
                    "fanhub-admin-session"
                );

            } catch (error) {

                /*
                 * Ignore storage restrictions.
                 */

            }


            showToast(
                "Admin session closed."
            );


            setTimeout(
                () => {

                    window.location.href =
                        "../login.html";

                },
                500
            );

        }
    );


    /* =====================================================
       RESPONSIVE CLEANUP
       ===================================================== */

    window.addEventListener(
        "resize",
        () => {

            if (
                window.innerWidth >
                991
            ) {

                closeSidebar();

            }

        }
    );


    /* =====================================================
       ESCAPE KEY
       ===================================================== */

    document.addEventListener(
        "keydown",
        (event) => {

            if (
                event.key !== "Escape"
            ) {

                return;

            }


            closeSidebar();

            closeNotifications();


            if (
                document.activeElement ===
                adminSearch
            ) {

                adminSearch.blur();

            }

        }
    );


    /* =====================================================
       INITIALIZE
       ===================================================== */

    loadSectionFromHash();

});