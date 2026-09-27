/* =========================================================
   FANHUB+ - EXPLORE PAGE
   Search, Filters, Sorting and Layout Controls
   ========================================================= */

"use strict";

document.addEventListener("DOMContentLoaded", () => {

    const contentGrid =
        document.getElementById("exploreContentGrid");

    if (!contentGrid) {
        return;
    }


    /* =====================================================
       ELEMENTS
       ===================================================== */

    const cards =
        Array.from(
            contentGrid.querySelectorAll(
                ".explore-content-card"
            )
        );

    const searchInput =
        document.getElementById("exploreSearch");

    const searchButton =
        document.getElementById("exploreSearchButton");

    const categoryChips =
        document.querySelectorAll(
            ".category-chip"
        );

    const categoryRadios =
        document.querySelectorAll(
            'input[name="category"]'
        );

    const genreCheckboxes =
        document.querySelectorAll(
            'input[name="genre"]'
        );

    const contentTypeCheckboxes =
        document.querySelectorAll(
            'input[name="contentType"]'
        );

    const yearFilter =
        document.getElementById("yearFilter");

    const ratingFilter =
        document.getElementById("ratingFilter");

    const ratingValue =
        document.getElementById("ratingValue");

    const sortSelect =
        document.getElementById("sortContent");

    const resultCount =
        document.getElementById("resultCount");

    const activeFilters =
        document.getElementById("activeFilters");

    const resetButton =
        document.getElementById("resetFilters");

    const noResults =
        document.getElementById("noResults");

    const noResultsReset =
        document.getElementById("noResultsReset");

    const gridViewButton =
        document.getElementById("gridViewButton");

    const listViewButton =
        document.getElementById("listViewButton");

    const loadMoreButton =
        document.getElementById("loadMoreButton");

    const filterTitles =
        document.querySelectorAll(
            ".filter-group-title"
        );


    /* =====================================================
       FILTER STATE
       ===================================================== */

    const filterState = {

        search: "",

        category: "all",

        genres: [],

        year: "all",

        contentTypes: [],

        minimumRating: 0,

        sort: "popular"

    };


    /* =====================================================
       INITIAL URL FILTERS
       Example:
       explore.html?category=anime
       explore.html?search=solo
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


        if (search) {

            filterState.search =
                search.trim().toLowerCase();

            if (searchInput) {

                searchInput.value =
                    search;

            }

        }


        if (category) {

            const normalizedCategory =
                category
                    .trim()
                    .toLowerCase();


            const validCategory =
                Array.from(categoryChips)
                    .some(
                        (chip) =>
                            chip.dataset.category ===
                            normalizedCategory
                    );


            if (validCategory) {

                filterState.category =
                    normalizedCategory;


                updateCategoryControls(
                    normalizedCategory
                );

            }

        }

    }


    /* =====================================================
       SEARCH
       ===================================================== */

    function updateSearch() {

        if (!searchInput) {
            return;
        }


        filterState.search =
            searchInput.value
                .trim()
                .toLowerCase();


        applyFilters();

    }


    if (searchInput) {

        /*
         * Live search while typing.
         */

        searchInput.addEventListener(
            "input",
            updateSearch
        );


        /*
         * Enter key.
         */

        searchInput.addEventListener(
            "keydown",
            (event) => {

                if (event.key === "Enter") {

                    event.preventDefault();

                    updateSearch();

                }

            }
        );

    }


    if (searchButton) {

        searchButton.addEventListener(
            "click",
            updateSearch
        );

    }


    /* =====================================================
       CATEGORY CHIPS
       ===================================================== */

    categoryChips.forEach((chip) => {

        chip.addEventListener(
            "click",
            () => {

                const category =
                    chip.dataset.category || "all";


                filterState.category =
                    category;


                updateCategoryControls(
                    category
                );


                applyFilters();

            }
        );

    });


    /* =====================================================
       CATEGORY RADIO BUTTONS
       ===================================================== */

    categoryRadios.forEach((radio) => {

        radio.addEventListener(
            "change",
            () => {

                if (!radio.checked) {
                    return;
                }


                filterState.category =
                    radio.value;


                updateCategoryControls(
                    radio.value
                );


                applyFilters();

            }
        );

    });


    /* =====================================================
       SYNC CATEGORY CONTROLS
       ===================================================== */

    function updateCategoryControls(category) {

        categoryChips.forEach((chip) => {

            chip.classList.toggle(
                "active",
                chip.dataset.category === category
            );

        });


        categoryRadios.forEach((radio) => {

            radio.checked =
                radio.value === category;

        });

    }


    /* =====================================================
       GENRE FILTER
       ===================================================== */

    genreCheckboxes.forEach((checkbox) => {

        checkbox.addEventListener(
            "change",
            () => {

                filterState.genres =
                    getCheckedValues(
                        genreCheckboxes
                    );


                applyFilters();

            }
        );

    });


    /* =====================================================
       CONTENT TYPE FILTER
       ===================================================== */

    contentTypeCheckboxes.forEach(
        (checkbox) => {

            checkbox.addEventListener(
                "change",
                () => {

                    filterState.contentTypes =
                        getCheckedValues(
                            contentTypeCheckboxes
                        );


                    applyFilters();

                }
            );

        }
    );


    /* =====================================================
       GET CHECKED VALUES
       ===================================================== */

    function getCheckedValues(inputs) {

        return Array.from(inputs)

            .filter(
                (input) =>
                    input.checked
            )

            .map(
                (input) =>
                    input.value
                        .trim()
                        .toLowerCase()
            );

    }


    /* =====================================================
       YEAR FILTER
       ===================================================== */

    if (yearFilter) {

        yearFilter.addEventListener(
            "change",
            () => {

                filterState.year =
                    yearFilter.value;


                applyFilters();

            }
        );

    }


    /* =====================================================
       RATING FILTER
       ===================================================== */

    if (ratingFilter) {

        ratingFilter.addEventListener(
            "input",
            () => {

                filterState.minimumRating =
                    Number(
                        ratingFilter.value
                    );


                if (ratingValue) {

                    ratingValue.textContent =
                        filterState.minimumRating > 0
                            ? `${filterState.minimumRating}+`
                            : "0+";

                }


                applyFilters();

            }
        );

    }


    /* =====================================================
       SORT
       ===================================================== */

    if (sortSelect) {

        sortSelect.addEventListener(
            "change",
            () => {

                filterState.sort =
                    sortSelect.value;


                sortCards();

            }
        );

    }


    /* =====================================================
       CARD MATCHING
       ===================================================== */

    function cardMatchesFilters(card) {

        const title =
            (
                card.dataset.title || ""
            ).toLowerCase();


        const category =
            (
                card.dataset.category || ""
            ).toLowerCase();


        const genres =
            (
                card.dataset.genre || ""
            )
                .toLowerCase()
                .split(" ")
                .filter(Boolean);


        const year =
            Number(
                card.dataset.year || 0
            );


        const contentType =
            (
                card.dataset.type || ""
            ).toLowerCase();


        const rating =
            Number(
                card.dataset.rating || 0
            );


        /* -----------------------------------------
           Search
           ----------------------------------------- */

        if (filterState.search) {

            const searchableText =
                [
                    title,
                    category,
                    genres.join(" "),
                    contentType,
                    year
                ]
                    .join(" ")
                    .toLowerCase();


            if (
                !searchableText.includes(
                    filterState.search
                )
            ) {

                return false;

            }

        }


        /* -----------------------------------------
           Category
           ----------------------------------------- */

        if (
            filterState.category !== "all" &&
            category !== filterState.category
        ) {

            return false;

        }


        /* -----------------------------------------
           Genre

           If multiple genres are selected,
           content can match any selected genre.
           ----------------------------------------- */

        if (
            filterState.genres.length > 0
        ) {

            const genreMatch =
                filterState.genres.some(
                    (selectedGenre) =>
                        genres.includes(
                            selectedGenre
                        )
                );


            if (!genreMatch) {

                return false;

            }

        }


        /* -----------------------------------------
           Release Year
           ----------------------------------------- */

        if (
            filterState.year !== "all"
        ) {

            if (
                filterState.year === "older"
            ) {

                if (year > 2022) {

                    return false;

                }

            } else {

                if (
                    year !==
                    Number(filterState.year)
                ) {

                    return false;

                }

            }

        }


        /* -----------------------------------------
           Content Type
           ----------------------------------------- */

        if (
            filterState.contentTypes.length > 0 &&
            !filterState.contentTypes.includes(
                contentType
            )
        ) {

            return false;

        }


        /* -----------------------------------------
           Minimum Rating
           ----------------------------------------- */

        if (
            rating <
            filterState.minimumRating
        ) {

            return false;

        }


        return true;

    }


    /* =====================================================
       APPLY ALL FILTERS
       ===================================================== */

    function applyFilters() {

        let visibleCount = 0;


        cards.forEach((card) => {

            const matches =
                cardMatchesFilters(card);


            card.classList.toggle(
                "filtered-out",
                !matches
            );


            if (matches) {

                visibleCount++;

            }

        });


        updateResultCount(
            visibleCount
        );


        updateNoResults(
            visibleCount
        );


        updateActiveFilters();


        sortCards();


        updateURL();

    }


    /* =====================================================
       RESULT COUNT
       ===================================================== */

    function updateResultCount(count) {

        if (!resultCount) {
            return;
        }


        resultCount.textContent =
            count.toLocaleString();

    }


    /* =====================================================
       NO RESULTS
       ===================================================== */

    function updateNoResults(count) {

        if (!noResults) {
            return;
        }


        noResults.classList.toggle(
            "show",
            count === 0
        );


        if (loadMoreButton) {

            loadMoreButton.style.display =
                count === 0
                    ? "none"
                    : "";

        }

    }


    /* =====================================================
       SORT CARDS
       ===================================================== */

    function sortCards() {

        const sortedCards =
            [...cards];


        sortedCards.sort(
            (cardA, cardB) => {

                switch (
                    filterState.sort
                ) {

                    /* ---------------------------------
                       Latest
                       --------------------------------- */

                    case "latest":

                        return (
                            Number(
                                cardB.dataset.year
                            ) -
                            Number(
                                cardA.dataset.year
                            )
                        );


                    /* ---------------------------------
                       Alphabetical
                       --------------------------------- */

                    case "alphabetical":

                        return (
                            cardA.dataset.title || ""
                        ).localeCompare(
                            cardB.dataset.title || ""
                        );


                    /* ---------------------------------
                       Rating
                       --------------------------------- */

                    case "rating":

                        return (
                            Number(
                                cardB.dataset.rating
                            ) -
                            Number(
                                cardA.dataset.rating
                            )
                        );


                    /* ---------------------------------
                       Popular
                       --------------------------------- */

                    case "popular":

                    default:

                        return (
                            Number(
                                cardB.dataset.popularity
                            ) -
                            Number(
                                cardA.dataset.popularity
                            )
                        );

                }

            }
        );


        sortedCards.forEach((card) => {

            contentGrid.appendChild(card);

        });

    }


    /* =====================================================
       ACTIVE FILTER TAGS
       ===================================================== */

    function updateActiveFilters() {

        if (!activeFilters) {
            return;
        }


        activeFilters.innerHTML = "";


        /* -----------------------------------------
           Search Tag
           ----------------------------------------- */

        if (filterState.search) {

            createFilterTag(
                `Search: ${filterState.search}`,
                () => {

                    filterState.search = "";

                    if (searchInput) {

                        searchInput.value = "";

                    }

                    applyFilters();

                }
            );

        }


        /* -----------------------------------------
           Category Tag
           ----------------------------------------- */

        if (
            filterState.category !== "all"
        ) {

            createFilterTag(
                capitalize(
                    filterState.category
                ),
                () => {

                    filterState.category =
                        "all";


                    updateCategoryControls(
                        "all"
                    );


                    applyFilters();

                }
            );

        }


        /* -----------------------------------------
           Genre Tags
           ----------------------------------------- */

        filterState.genres.forEach(
            (genre) => {

                createFilterTag(
                    capitalize(genre),
                    () => {

                        const checkbox =
                            Array.from(
                                genreCheckboxes
                            ).find(
                                (input) =>
                                    input.value ===
                                    genre
                            );


                        if (checkbox) {

                            checkbox.checked =
                                false;

                        }


                        filterState.genres =
                            getCheckedValues(
                                genreCheckboxes
                            );


                        applyFilters();

                    }
                );

            }
        );


        /* -----------------------------------------
           Year Tag
           ----------------------------------------- */

        if (
            filterState.year !== "all"
        ) {

            const yearText =
                filterState.year === "older"
                    ? "2022 & Earlier"
                    : filterState.year;


            createFilterTag(
                yearText,
                () => {

                    filterState.year =
                        "all";


                    if (yearFilter) {

                        yearFilter.value =
                            "all";

                    }


                    applyFilters();

                }
            );

        }


        /* -----------------------------------------
           Content Type Tags
           ----------------------------------------- */

        filterState.contentTypes.forEach(
            (type) => {

                createFilterTag(
                    capitalize(type),
                    () => {

                        const checkbox =
                            Array.from(
                                contentTypeCheckboxes
                            ).find(
                                (input) =>
                                    input.value ===
                                    type
                            );


                        if (checkbox) {

                            checkbox.checked =
                                false;

                        }


                        filterState.contentTypes =
                            getCheckedValues(
                                contentTypeCheckboxes
                            );


                        applyFilters();

                    }
                );

            }
        );


        /* -----------------------------------------
           Rating Tag
           ----------------------------------------- */

        if (
            filterState.minimumRating > 0
        ) {

            createFilterTag(
                `${filterState.minimumRating}+ Rating`,
                () => {

                    filterState.minimumRating =
                        0;


                    if (ratingFilter) {

                        ratingFilter.value =
                            0;

                    }


                    if (ratingValue) {

                        ratingValue.textContent =
                            "0+";

                    }


                    applyFilters();

                }
            );

        }

    }


    /* =====================================================
       CREATE FILTER TAG
       ===================================================== */

    function createFilterTag(
        text,
        removeCallback
    ) {

        const tag =
            document.createElement("span");


        tag.className =
            "active-filter-tag";


        const label =
            document.createElement("span");


        label.textContent =
            text;


        const button =
            document.createElement("button");


        button.type =
            "button";


        button.setAttribute(
            "aria-label",
            `Remove ${text} filter`
        );


        button.innerHTML =
            '<i class="bi bi-x-lg"></i>';


        button.addEventListener(
            "click",
            removeCallback
        );


        tag.appendChild(label);

        tag.appendChild(button);

        activeFilters.appendChild(tag);

    }


    /* =====================================================
       RESET FILTERS
       ===================================================== */

    function resetFilters() {

        filterState.search = "";

        filterState.category = "all";

        filterState.genres = [];

        filterState.year = "all";

        filterState.contentTypes = [];

        filterState.minimumRating = 0;

        filterState.sort = "popular";


        /* Search */

        if (searchInput) {

            searchInput.value = "";

        }


        /* Categories */

        updateCategoryControls(
            "all"
        );


        /* Genres */

        genreCheckboxes.forEach(
            (checkbox) => {

                checkbox.checked = false;

            }
        );


        /* Content Types */

        contentTypeCheckboxes.forEach(
            (checkbox) => {

                checkbox.checked = false;

            }
        );


        /* Year */

        if (yearFilter) {

            yearFilter.value = "all";

        }


        /* Rating */

        if (ratingFilter) {

            ratingFilter.value = 0;

        }


        if (ratingValue) {

            ratingValue.textContent =
                "0+";

        }


        /* Sort */

        if (sortSelect) {

            sortSelect.value =
                "popular";

        }


        applyFilters();


        if (
            window.FanHub &&
            window.FanHub.showToast
        ) {

            window.FanHub.showToast(
                "Filters reset."
            );

        }

    }


    if (resetButton) {

        resetButton.addEventListener(
            "click",
            resetFilters
        );

    }


    if (noResultsReset) {

        noResultsReset.addEventListener(
            "click",
            resetFilters
        );

    }


    /* =====================================================
       GRID VIEW
       ===================================================== */

    if (gridViewButton) {

        gridViewButton.addEventListener(
            "click",
            () => {

                contentGrid.classList.remove(
                    "list-view"
                );


                gridViewButton.classList.add(
                    "active"
                );


                if (listViewButton) {

                    listViewButton.classList.remove(
                        "active"
                    );

                }


                localStorage.setItem(
                    "fanhub-explore-layout",
                    "grid"
                );

            }
        );

    }


    /* =====================================================
       LIST VIEW
       ===================================================== */

    if (listViewButton) {

        listViewButton.addEventListener(
            "click",
            () => {

                contentGrid.classList.add(
                    "list-view"
                );


                listViewButton.classList.add(
                    "active"
                );


                if (gridViewButton) {

                    gridViewButton.classList.remove(
                        "active"
                    );

                }


                localStorage.setItem(
                    "fanhub-explore-layout",
                    "list"
                );

            }
        );

    }


    /* =====================================================
       RESTORE SAVED LAYOUT
       ===================================================== */

    function restoreLayout() {

        const savedLayout =
            localStorage.getItem(
                "fanhub-explore-layout"
            );


        if (
            savedLayout === "list"
        ) {

            contentGrid.classList.add(
                "list-view"
            );


            if (listViewButton) {

                listViewButton.classList.add(
                    "active"
                );

            }


            if (gridViewButton) {

                gridViewButton.classList.remove(
                    "active"
                );

            }

        }

    }


    /* =====================================================
       COLLAPSIBLE FILTER GROUPS
       ===================================================== */

    filterTitles.forEach((button) => {

        button.addEventListener(
            "click",
            () => {

                const group =
                    button.closest(
                        ".filter-group"
                    );


                if (!group) {
                    return;
                }


                group.classList.toggle(
                    "collapsed"
                );

            }
        );

    });


    /* =====================================================
       LOAD MORE
       Frontend demo behaviour.
       ===================================================== */

    if (loadMoreButton) {

        loadMoreButton.addEventListener(
            "click",
            () => {

                /*
                 * At this stage the project is
                 * frontend-only. Later this button
                 * can request more records from
                 * the backend API.
                 */

                if (
                    window.FanHub &&
                    window.FanHub.showToast
                ) {

                    window.FanHub.showToast(
                        "All available demo content is already loaded."
                    );

                }

            }
        );

    }


    /* =====================================================
       UPDATE URL
       Keeps search/category shareable.
       ===================================================== */

    function updateURL() {

        const params =
            new URLSearchParams();


        if (filterState.search) {

            params.set(
                "search",
                filterState.search
            );

        }


        if (
            filterState.category !== "all"
        ) {

            params.set(
                "category",
                filterState.category
            );

        }


        const queryString =
            params.toString();


        const newURL =
            queryString
                ? `${window.location.pathname}?${queryString}`
                : window.location.pathname;


        /*
         * replaceState changes the URL without
         * refreshing the page.
         */

        window.history.replaceState(
            {},
            "",
            newURL
        );

    }


    /* =====================================================
       CAPITALIZE TEXT
       ===================================================== */

    function capitalize(value) {

        if (!value) {
            return "";
        }


        return (
            value.charAt(0).toUpperCase() +
            value.slice(1)
        );

    }


    /* =====================================================
       INITIALIZE
       ===================================================== */

    loadFiltersFromURL();

    restoreLayout();

    applyFilters();

});