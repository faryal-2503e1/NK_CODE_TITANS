/* =========================================================
   FANHUB+ - MERCHANDISE PAGE
   Search, Category Filter, Tags, Sorting, View & Modal
   ========================================================= */

"use strict";

document.addEventListener("DOMContentLoaded", () => {

    /* =====================================================
       ELEMENTS
       ===================================================== */

    const merchandiseSearch =
        document.getElementById("merchandiseSearch");

    const merchandiseSearchButton =
        document.getElementById("merchandiseSearchButton");

    const categoryTabs =
        document.querySelectorAll(
            ".merch-category-tab[data-merch-category]"
        );

    const merchandiseTagFilter =
        document.getElementById("merchandiseTagFilter");

    const merchandiseSort =
        document.getElementById("merchandiseSort");

    const merchandiseGrid =
        document.getElementById("merchandiseGrid");

    const merchandiseResultCount =
        document.getElementById("merchandiseResultCount");

    const merchandiseNoResults =
        document.getElementById("merchandiseNoResults");

    const resetMerchandiseFilters =
        document.getElementById("resetMerchandiseFilters");

    const merchGridView =
        document.getElementById("merchGridView");

    const merchListView =
        document.getElementById("merchListView");


    /* =====================================================
       MODAL ELEMENTS
       ===================================================== */

    const merchandiseDetailsModal =
        document.getElementById("merchandiseDetailsModal");

    const merchandiseModalTitle =
        document.getElementById("merchandiseModalTitle");

    const merchandiseModalDescription =
        document.getElementById("merchandiseModalDescription");

    const merchandiseModalCategory =
        document.getElementById("merchandiseModalCategory");

    const merchandiseModalRelease =
        document.getElementById("merchandiseModalRelease");

    const merchandiseModalTags =
        document.getElementById("merchandiseModalTags");

    const merchandiseModalVisual =
        document.getElementById("merchandiseModalVisual");


    /* =====================================================
       SETTINGS
       ===================================================== */

    const VIEW_STORAGE_KEY =
        "fanhub-merchandise-view";


    const filters = {

        search: "",

        category: "all",

        tag: "all"

    };


    /* =====================================================
       HELPERS
       ===================================================== */

    function normalize(value) {

        return String(value || "")
            .trim()
            .toLowerCase();

    }


    function getMerchandiseCards() {

        if (!merchandiseGrid) {
            return [];
        }


        return Array.from(
            merchandiseGrid.querySelectorAll(
                ".merchandise-card"
            )
        );

    }


    function formatCategory(category) {

        const names = {

            anime: "Anime",

            gaming: "Gaming",

            movies: "Movies",

            comics: "Comics",

            kpop: "K-Pop"

        };


        return (
            names[normalize(category)] ||
            category ||
            "Fandom"
        );

    }


    function formatReleaseDate(dateValue) {

        if (!dateValue) {
            return "Release date unavailable";
        }


        const date =
            new Date(
                `${dateValue}T00:00:00`
            );


        if (
            Number.isNaN(
                date.getTime()
            )
        ) {

            return dateValue;

        }


        return date.toLocaleDateString(
            "en-US",
            {
                month: "long",
                day: "numeric",
                year: "numeric"
            }
        );

    }


    /* =====================================================
       SEARCH
       ===================================================== */

    function updateSearch() {

        filters.search =
            merchandiseSearch
                ? normalize(
                    merchandiseSearch.value
                )
                : "";


        applyFilters();

    }


    if (merchandiseSearchButton) {

        merchandiseSearchButton.addEventListener(
            "click",
            updateSearch
        );

    }


    if (merchandiseSearch) {

        merchandiseSearch.addEventListener(
            "input",
            () => {

                filters.search =
                    normalize(
                        merchandiseSearch.value
                    );


                applyFilters();

            }
        );


        merchandiseSearch.addEventListener(
            "keydown",
            (event) => {

                if (event.key === "Enter") {

                    event.preventDefault();

                    updateSearch();

                }

            }
        );

    }


    /* =====================================================
       CATEGORY TABS
       ===================================================== */

    categoryTabs.forEach((tab) => {

        tab.addEventListener(
            "click",
            () => {

                categoryTabs.forEach(
                    (item) => {

                        item.classList.remove(
                            "active"
                        );

                    }
                );


                tab.classList.add(
                    "active"
                );


                filters.category =
                    tab.dataset
                        .merchCategory ||
                    "all";


                applyFilters();

            }
        );

    });


    /* =====================================================
       TAG FILTER
       ===================================================== */

    if (merchandiseTagFilter) {

        merchandiseTagFilter.addEventListener(
            "change",
            () => {

                filters.tag =
                    merchandiseTagFilter.value ||
                    "all";


                applyFilters();

            }
        );

    }


    /* =====================================================
       MATCH CARD
       ===================================================== */

    function cardMatchesFilters(card) {

        const title =
            normalize(
                card.dataset.title
            );


        const category =
            normalize(
                card.dataset.category
            );


        const tags =
            normalize(
                card.dataset.tag
            )
                .split(/\s+/)
                .filter(Boolean);


        const visibleText =
            normalize(
                card.textContent
            );


        const searchMatches =
            !filters.search ||
            title.includes(
                filters.search
            ) ||
            visibleText.includes(
                filters.search
            );


        const categoryMatches =
            filters.category === "all" ||
            category ===
                filters.category;


        const tagMatches =
            filters.tag === "all" ||
            tags.includes(
                filters.tag
            );


        return (
            searchMatches &&
            categoryMatches &&
            tagMatches
        );

    }


    /* =====================================================
       APPLY FILTERS
       ===================================================== */

    function applyFilters() {

        const cards =
            getMerchandiseCards();


        let visibleCount = 0;


        cards.forEach((card) => {

            const visible =
                cardMatchesFilters(card);


            card.classList.toggle(
                "merchandise-hidden",
                !visible
            );


            if (visible) {
                visibleCount++;
            }

        });


        updateResultCount(
            visibleCount
        );


        updateNoResults(
            visibleCount
        );


        sortMerchandise();

        updateURL();

    }


    /* =====================================================
       RESULT COUNT
       ===================================================== */

    function updateResultCount(count) {

        if (merchandiseResultCount) {

            merchandiseResultCount.textContent =
                count;

        }

    }


    /* =====================================================
       NO RESULTS
       ===================================================== */

    function updateNoResults(count) {

        if (!merchandiseNoResults) {
            return;
        }


        merchandiseNoResults.classList.toggle(
            "show",
            count === 0
        );


        if (merchandiseGrid) {

            merchandiseGrid.style.display =
                count === 0
                    ? "none"
                    : "";

        }

    }


    /* =====================================================
       SORT MERCHANDISE
       ===================================================== */

    function sortMerchandise() {

        if (
            !merchandiseGrid ||
            !merchandiseSort
        ) {
            return;
        }


        const cards =
            getMerchandiseCards();


        const sortValue =
            merchandiseSort.value;


        cards.sort((a, b) => {

            const titleA =
                normalize(
                    a.dataset.title
                );


            const titleB =
                normalize(
                    b.dataset.title
                );


            const releaseA =
                new Date(
                    `${a.dataset.release}T00:00:00`
                );


            const releaseB =
                new Date(
                    `${b.dataset.release}T00:00:00`
                );


            const featuredA =
                Number(
                    a.dataset.featured || 0
                );


            const featuredB =
                Number(
                    b.dataset.featured || 0
                );


            if (
                sortValue ===
                "alphabetical"
            ) {

                return titleA.localeCompare(
                    titleB
                );

            }


            if (
                sortValue ===
                "latest"
            ) {

                return releaseB - releaseA;

            }


            /*
             * Default:
             * Featured items first.
             */

            return featuredB - featuredA;

        });


        cards.forEach((card) => {

            merchandiseGrid.appendChild(
                card
            );

        });

    }


    if (merchandiseSort) {

        merchandiseSort.addEventListener(
            "change",
            sortMerchandise
        );

    }


    /* =====================================================
       RESET FILTERS
       ===================================================== */

    function resetFilters() {

        filters.search = "";

        filters.category = "all";

        filters.tag = "all";


        if (merchandiseSearch) {

            merchandiseSearch.value = "";

        }


        categoryTabs.forEach(
            (tab) => {

                tab.classList.toggle(
                    "active",
                    tab.dataset
                        .merchCategory ===
                        "all"
                );

            }
        );


        if (merchandiseTagFilter) {

            merchandiseTagFilter.value =
                "all";

        }


        if (merchandiseSort) {

            merchandiseSort.value =
                "featured";

        }


        applyFilters();


        showMerchandiseToast(
            "Merchandise filters reset."
        );

    }


    if (resetMerchandiseFilters) {

        resetMerchandiseFilters.addEventListener(
            "click",
            resetFilters
        );

    }


    /* =====================================================
       GRID / LIST VIEW
       ===================================================== */

    function setView(view) {

        if (!merchandiseGrid) {
            return;
        }


        const listView =
            view === "list";


        merchandiseGrid.classList.toggle(
            "list-view",
            listView
        );


        if (merchGridView) {

            merchGridView.classList.toggle(
                "active",
                !listView
            );

        }


        if (merchListView) {

            merchListView.classList.toggle(
                "active",
                listView
            );

        }


        try {

            localStorage.setItem(
                VIEW_STORAGE_KEY,
                listView
                    ? "list"
                    : "grid"
            );

        } catch (error) {

            // Local storage unavailable.

        }

    }


    if (merchGridView) {

        merchGridView.addEventListener(
            "click",
            () => {

                setView("grid");

            }
        );

    }


    if (merchListView) {

        merchListView.addEventListener(
            "click",
            () => {

                setView("list");

            }
        );

    }


    function restoreView() {

        let savedView =
            "grid";


        try {

            savedView =
                localStorage.getItem(
                    VIEW_STORAGE_KEY
                ) || "grid";

        } catch (error) {

            savedView = "grid";

        }


        setView(
            savedView === "list"
                ? "list"
                : "grid"
        );

    }


    /* =====================================================
       MODAL DATA
       ===================================================== */

    const merchandiseDescriptions = {

        "Shadow Hunter Collector Figure":
            "A limited-edition display collectible created for fantasy anime fans and collectors.",

        "Elemental Vision Display Set":
            "A decorative gaming-inspired collectible set designed for fandom shelves and displays.",

        "Pirate Crew Poster Collection":
            "A themed poster collection inspired by adventure, friendship and pirate anime fandoms.",

        "Web Hero Collector Mask":
            "A superhero-inspired collector display concept created for cinematic fandom collections.",

        "Sorcerer Academy Hoodie":
            "An upcoming fandom apparel concept inspired by supernatural action and sorcerer stories.",

        "Superhero Comic Display Box":
            "A decorative storage and display concept designed for comic and superhero collections.",

        "Idol Light Display Edition":
            "A limited fandom display concept created for K-Pop and music community collections.",

        "Cyber Legends Art Book":
            "An upcoming collectible art book featuring futuristic gaming-inspired concept artwork.",

        "Multiverse Hero Pin Set":
            "A collectible pin set concept inspired by cinematic heroes and multiverse adventures.",

        "Classic Comic Cover Prints":
            "An upcoming print collection celebrating classic comic-style cover artwork."

    };


    /* =====================================================
       MODAL BADGES
       ===================================================== */

    function getTagName(tag) {

        const names = {

            limited:
                "Limited Edition",

            preorder:
                "Pre-Order",

            collectible:
                "Collectible",

            new:
                "New"

        };


        return names[tag] || tag;

    }


    function createTagBadge(tag) {

        const span =
            document.createElement(
                "span"
            );


        span.className =
            `merch-badge ${tag}`;


        span.textContent =
            getTagName(tag);


        return span;

    }


    /* =====================================================
       MODAL VISUAL
       ===================================================== */

    function getVisualIcon(category) {

        const icons = {

            anime:
                "bi-stars",

            gaming:
                "bi-controller",

            movies:
                "bi-film",

            comics:
                "bi-lightning-charge",

            kpop:
                "bi-music-note-beamed"

        };


        return (
            icons[normalize(category)] ||
            "bi-box2-heart"
        );

    }


    /* =====================================================
       OPEN DETAILS MODAL
       ===================================================== */

    function openDetailsModal(card) {

        if (
            !card ||
            !merchandiseDetailsModal
        ) {
            return;
        }


        const title =
            card.dataset.title ||
            "Merchandise Item";


        const category =
            card.dataset.category ||
            "";


        const release =
            card.dataset.release ||
            "";


        const tags =
            normalize(
                card.dataset.tag
            )
                .split(/\s+/)
                .filter(Boolean);


        if (merchandiseModalTitle) {

            merchandiseModalTitle.textContent =
                title;

        }


        if (merchandiseModalDescription) {

            merchandiseModalDescription.textContent =
                merchandiseDescriptions[title] ||
                "Fan merchandise information and release details.";

        }


        if (merchandiseModalCategory) {

            merchandiseModalCategory.textContent =
                formatCategory(category);

        }


        if (merchandiseModalRelease) {

            merchandiseModalRelease.textContent =
                formatReleaseDate(release);

        }


        /*
         * Render badges safely using DOM
         * instead of injecting HTML.
         */

        if (merchandiseModalTags) {

            merchandiseModalTags.innerHTML =
                "";


            tags.forEach((tag) => {

                merchandiseModalTags.appendChild(
                    createTagBadge(tag)
                );

            });

        }


        /*
         * Change modal icon according
         * to merchandise category.
         */

        if (merchandiseModalVisual) {

            merchandiseModalVisual.innerHTML =
                "";


            const icon =
                document.createElement(
                    "i"
                );


            icon.className =
                `bi ${getVisualIcon(category)}`;


            merchandiseModalVisual.appendChild(
                icon
            );

        }


        if (
            window.bootstrap &&
            bootstrap.Modal
        ) {

            const modal =
                bootstrap.Modal
                    .getOrCreateInstance(
                        merchandiseDetailsModal
                    );


            modal.show();

        }

    }


    /* =====================================================
       DETAILS BUTTON
       ===================================================== */

    if (merchandiseGrid) {

        merchandiseGrid.addEventListener(
            "click",
            (event) => {

                const button =
                    event.target.closest(
                        ".merchandise-details-button"
                    );


                if (!button) {
                    return;
                }


                const card =
                    button.closest(
                        ".merchandise-card"
                    );


                openDetailsModal(
                    card
                );

            }
        );

    }


    /* =====================================================
       LOAD URL FILTERS
       ===================================================== */

    function loadFiltersFromURL() {

        const params =
            new URLSearchParams(
                window.location.search
            );


        const search =
            params.get("search");


        const category =
            params.get("category");


        const tag =
            params.get("tag");


        if (
            search &&
            merchandiseSearch
        ) {

            merchandiseSearch.value =
                search;


            filters.search =
                normalize(search);

        }


        if (category) {

            const normalizedCategory =
                normalize(category);


            const categoryExists =
                Array.from(
                    categoryTabs
                ).some(
                    (tab) =>
                        tab.dataset
                            .merchCategory ===
                        normalizedCategory
                );


            if (categoryExists) {

                filters.category =
                    normalizedCategory;


                categoryTabs.forEach(
                    (tab) => {

                        tab.classList.toggle(
                            "active",
                            tab.dataset
                                .merchCategory ===
                                normalizedCategory
                        );

                    }
                );

            }

        }


        if (
            tag &&
            merchandiseTagFilter
        ) {

            const allowedTags = [
                "all",
                "limited",
                "preorder",
                "collectible",
                "new"
            ];


            const normalizedTag =
                normalize(tag);


            if (
                allowedTags.includes(
                    normalizedTag
                )
            ) {

                filters.tag =
                    normalizedTag;


                merchandiseTagFilter.value =
                    normalizedTag;

            }

        }

    }


    /* =====================================================
       UPDATE URL
       ===================================================== */

    function updateURL() {

        const params =
            new URLSearchParams();


        if (filters.search) {

            params.set(
                "search",
                filters.search
            );

        }


        if (
            filters.category !==
            "all"
        ) {

            params.set(
                "category",
                filters.category
            );

        }


        if (
            filters.tag !==
            "all"
        ) {

            params.set(
                "tag",
                filters.tag
            );

        }


        const query =
            params.toString();


        const newURL =
            query
                ? `${window.location.pathname}?${query}`
                : window.location.pathname;


        try {

            window.history.replaceState(
                null,
                "",
                newURL
            );

        } catch (error) {

            // Ignore URL update restrictions.

        }

    }


    /* =====================================================
       TOAST
       ===================================================== */

    function showMerchandiseToast(message) {

        /*
         * Use shared FanHub toast
         * if app.js exposes it.
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


        const toastElement =
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
            toastElement &&
            window.bootstrap &&
            bootstrap.Toast
        ) {

            bootstrap.Toast
                .getOrCreateInstance(
                    toastElement
                )
                .show();

        }

    }


    /* =====================================================
       KEYBOARD SUPPORT
       ===================================================== */

    document.addEventListener(
        "keydown",
        (event) => {

            /*
             * Press "/" to quickly focus
             * merchandise search.
             */

            if (
                event.key === "/" &&
                document.activeElement
                    ?.tagName !== "INPUT" &&
                document.activeElement
                    ?.tagName !== "TEXTAREA" &&
                document.activeElement
                    ?.tagName !== "SELECT"
            ) {

                event.preventDefault();


                if (merchandiseSearch) {

                    merchandiseSearch.focus();

                }

            }

        }
    );


    /* =====================================================
       INITIALIZE
       ===================================================== */

    loadFiltersFromURL();

    restoreView();

    applyFilters();

});