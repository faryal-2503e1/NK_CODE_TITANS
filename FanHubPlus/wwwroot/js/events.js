/* =========================================================
   FANHUB+ - EVENTS PAGE
   Search, Filters, Calendar, Sorting, View & Event Modal
   ========================================================= */

"use strict";

document.addEventListener("DOMContentLoaded", () => {

    /* =====================================================
       ELEMENTS
       ===================================================== */

    const eventSearch =
        document.getElementById("eventSearch");

    const eventSearchButton =
        document.getElementById("eventSearchButton");

    const eventCityFilter =
        document.getElementById("eventCityFilter");

    const eventSidebarCity =
        document.getElementById("eventSidebarCity");

    const categoryChips =
        document.querySelectorAll(
            ".event-category-chip[data-event-category]"
        );

    const eventTypeInputs =
        document.querySelectorAll(
            'input[name="eventType"]'
        );

    const calendarDays =
        document.querySelectorAll(
            ".calendar-day[data-event-date]"
        );

    const resetEventFilters =
        document.getElementById("resetEventFilters");

    const eventNoResultsReset =
        document.getElementById("eventNoResultsReset");

    const eventSort =
        document.getElementById("eventSort");

    const eventsGrid =
        document.getElementById("eventsGrid");

    const eventResultCount =
        document.getElementById("eventResultCount");

    const eventsNoResults =
        document.getElementById("eventsNoResults");

    const eventGridView =
        document.getElementById("eventGridView");

    const eventListView =
        document.getElementById("eventListView");


    /* =====================================================
       MODAL ELEMENTS
       ===================================================== */

    const eventDetailsModal =
        document.getElementById("eventDetailsModal");

    const eventModalTitle =
        document.getElementById("eventModalTitle");

    const eventModalDescription =
        document.getElementById("eventModalDescription");

    const eventModalDate =
        document.getElementById("eventModalDate");

    const eventModalLocation =
        document.getElementById("eventModalLocation");

    const eventModalType =
        document.getElementById("eventModalType");


    /* =====================================================
       STORAGE
       ===================================================== */

    const VIEW_STORAGE_KEY =
        "fanhub-events-view";


    /* =====================================================
       FILTER STATE
       ===================================================== */

    const filters = {

        search: "",

        city: "all",

        category: "all",

        eventTypes: [],

        date: ""

    };


    /* =====================================================
       GET EVENT CARDS
       ===================================================== */

    function getEventCards() {

        if (!eventsGrid) {
            return [];
        }

        return Array.from(
            eventsGrid.querySelectorAll(
                ".fan-event-card"
            )
        );

    }


    /* =====================================================
       NORMALIZE TEXT
       ===================================================== */

    function normalize(value) {

        return String(value || "")
            .trim()
            .toLowerCase();

    }


    /* =====================================================
       SEARCH
       ===================================================== */

    function updateSearchFilter() {

        filters.search =
            eventSearch
                ? normalize(eventSearch.value)
                : "";

        applyEventFilters();

    }


    if (eventSearchButton) {

        eventSearchButton.addEventListener(
            "click",
            updateSearchFilter
        );

    }


    if (eventSearch) {

        eventSearch.addEventListener(
            "keydown",
            (event) => {

                if (event.key === "Enter") {

                    event.preventDefault();

                    updateSearchFilter();

                }

            }
        );


        /*
         * Live filtering after user clears
         * or changes the search.
         */

        eventSearch.addEventListener(
            "input",
            () => {

                filters.search =
                    normalize(
                        eventSearch.value
                    );

                applyEventFilters();

            }
        );

    }


    /* =====================================================
       CITY FILTER
       ===================================================== */

    function setCity(city, source) {

        filters.city =
            city || "all";


        /*
         * Keep hero and sidebar
         * city selectors synchronized.
         */

        if (
            eventCityFilter &&
            source !== eventCityFilter
        ) {

            eventCityFilter.value =
                filters.city;

        }


        if (
            eventSidebarCity &&
            source !== eventSidebarCity
        ) {

            eventSidebarCity.value =
                filters.city;

        }


        applyEventFilters();

    }


    if (eventCityFilter) {

        eventCityFilter.addEventListener(
            "change",
            () => {

                setCity(
                    eventCityFilter.value,
                    eventCityFilter
                );

            }
        );

    }


    if (eventSidebarCity) {

        eventSidebarCity.addEventListener(
            "change",
            () => {

                setCity(
                    eventSidebarCity.value,
                    eventSidebarCity
                );

            }
        );

    }


    /* =====================================================
       CATEGORY FILTER
       ===================================================== */

    categoryChips.forEach((chip) => {

        chip.addEventListener(
            "click",
            () => {

                categoryChips.forEach(
                    (item) => {

                        item.classList.remove(
                            "active"
                        );

                    }
                );


                chip.classList.add(
                    "active"
                );


                filters.category =
                    chip.dataset
                        .eventCategory ||
                    "all";


                applyEventFilters();

            }
        );

    });


    /* =====================================================
       EVENT TYPE FILTER
       ===================================================== */

    function updateEventTypes() {

        filters.eventTypes =
            Array.from(eventTypeInputs)
                .filter(
                    (input) =>
                        input.checked
                )
                .map(
                    (input) =>
                        input.value
                );


        applyEventFilters();

    }


    eventTypeInputs.forEach((input) => {

        input.addEventListener(
            "change",
            updateEventTypes
        );

    });


    /* =====================================================
       CALENDAR DATE FILTER
       ===================================================== */

    calendarDays.forEach((day) => {

        day.addEventListener(
            "click",
            () => {

                const selectedDate =
                    day.dataset.eventDate;


                /*
                 * Clicking selected date again
                 * removes the date filter.
                 */

                if (
                    filters.date ===
                    selectedDate
                ) {

                    filters.date = "";

                    day.classList.remove(
                        "active"
                    );

                } else {

                    filters.date =
                        selectedDate;


                    calendarDays.forEach(
                        (item) => {

                            item.classList.remove(
                                "active"
                            );

                        }
                    );


                    day.classList.add(
                        "active"
                    );

                }


                applyEventFilters();

            }
        );

    });


    /* =====================================================
       EVENT MATCHING
       ===================================================== */

    function eventMatchesFilters(card) {

        const title =
            normalize(
                card.dataset.title
            );

        const category =
            normalize(
                card.dataset.category
            );

        const city =
            normalize(
                card.dataset.city
            );

        const type =
            normalize(
                card.dataset.type
            );

        const date =
            card.dataset.date || "";


        /*
         * Also include visible card text
         * in search matching.
         */

        const cardText =
            normalize(
                card.textContent
            );


        const searchMatches =
            !filters.search ||
            title.includes(
                filters.search
            ) ||
            cardText.includes(
                filters.search
            );


        const cityMatches =
            filters.city === "all" ||
            city === filters.city;


        const categoryMatches =
            filters.category === "all" ||
            category ===
                filters.category;


        const typeMatches =
            filters.eventTypes.length === 0 ||
            filters.eventTypes.includes(
                type
            );


        const dateMatches =
            !filters.date ||
            date === filters.date;


        return (
            searchMatches &&
            cityMatches &&
            categoryMatches &&
            typeMatches &&
            dateMatches
        );

    }


    /* =====================================================
       APPLY FILTERS
       ===================================================== */

    function applyEventFilters() {

        const cards =
            getEventCards();


        let visibleCount = 0;


        cards.forEach((card) => {

            const visible =
                eventMatchesFilters(
                    card
                );


            card.classList.toggle(
                "event-hidden",
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


        sortEvents();

        updateURL();

    }


    /* =====================================================
       RESULT COUNT
       ===================================================== */

    function updateResultCount(count) {

        if (eventResultCount) {

            eventResultCount.textContent =
                count;

        }

    }


    /* =====================================================
       NO RESULTS
       ===================================================== */

    function updateNoResults(count) {

        if (!eventsNoResults) {
            return;
        }


        eventsNoResults.classList.toggle(
            "show",
            count === 0
        );


        if (eventsGrid) {

            eventsGrid.style.display =
                count === 0
                    ? "none"
                    : "";

        }

    }


    /* =====================================================
       RESET FILTERS
       ===================================================== */

    function resetFilters() {

        filters.search = "";

        filters.city = "all";

        filters.category = "all";

        filters.eventTypes = [];

        filters.date = "";


        if (eventSearch) {

            eventSearch.value = "";

        }


        if (eventCityFilter) {

            eventCityFilter.value =
                "all";

        }


        if (eventSidebarCity) {

            eventSidebarCity.value =
                "all";

        }


        categoryChips.forEach(
            (chip) => {

                chip.classList.toggle(
                    "active",
                    chip.dataset
                        .eventCategory ===
                        "all"
                );

            }
        );


        eventTypeInputs.forEach(
            (input) => {

                input.checked = false;

            }
        );


        calendarDays.forEach(
            (day) => {

                day.classList.remove(
                    "active"
                );

            }
        );


        if (eventSort) {

            eventSort.value =
                "soonest";

        }


        applyEventFilters();


        showEventsToast(
            "Event filters reset."
        );

    }


    if (resetEventFilters) {

        resetEventFilters.addEventListener(
            "click",
            resetFilters
        );

    }


    if (eventNoResultsReset) {

        eventNoResultsReset.addEventListener(
            "click",
            resetFilters
        );

    }


    /* =====================================================
       SORT EVENTS
       ===================================================== */

    function sortEvents() {

        if (
            !eventsGrid ||
            !eventSort
        ) {
            return;
        }


        const cards =
            getEventCards();


        const sortValue =
            eventSort.value;


        cards.sort((a, b) => {

            const titleA =
                normalize(
                    a.dataset.title
                );

            const titleB =
                normalize(
                    b.dataset.title
                );


            const dateA =
                new Date(
                    `${a.dataset.date}T00:00:00`
                );


            const dateB =
                new Date(
                    `${b.dataset.date}T00:00:00`
                );


            if (
                sortValue ===
                "latest"
            ) {

                return dateB - dateA;

            }


            if (
                sortValue ===
                "alphabetical"
            ) {

                return titleA.localeCompare(
                    titleB
                );

            }


            /*
             * Default: soonest.
             */

            return dateA - dateB;

        });


        cards.forEach((card) => {

            eventsGrid.appendChild(card);

        });

    }


    if (eventSort) {

        eventSort.addEventListener(
            "change",
            () => {

                sortEvents();

            }
        );

    }


    /* =====================================================
       GRID / LIST VIEW
       ===================================================== */

    function setEventView(view) {

        if (!eventsGrid) {
            return;
        }


        const isList =
            view === "list";


        eventsGrid.classList.toggle(
            "list-view",
            isList
        );


        if (eventGridView) {

            eventGridView.classList.toggle(
                "active",
                !isList
            );

        }


        if (eventListView) {

            eventListView.classList.toggle(
                "active",
                isList
            );

        }


        try {

            localStorage.setItem(
                VIEW_STORAGE_KEY,
                isList
                    ? "list"
                    : "grid"
            );

        } catch (error) {
            // Storage unavailable.
        }

    }


    if (eventGridView) {

        eventGridView.addEventListener(
            "click",
            () => {

                setEventView("grid");

            }
        );

    }


    if (eventListView) {

        eventListView.addEventListener(
            "click",
            () => {

                setEventView("list");

            }
        );

    }


    function restoreEventView() {

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


        setEventView(
            savedView === "list"
                ? "list"
                : "grid"
        );

    }


    /* =====================================================
       EVENT DETAILS MODAL
       ===================================================== */

    const eventDescriptions = {

        "Karachi Anime Fan Festival":
            "Anime fan gathering featuring cosplay, fan art and community activities.",

        "Gamer Arena Championship":
            "Competitive gaming tournament for local gaming communities.",

        "Cosplay Creators Meetup":
            "A community meetup for cosplayers, photographers and costume creators.",

        "Superhero Movie Night":
            "A fan-focused superhero movie screening and discussion evening.",

        "Anime Art Community Meetup":
            "Meet anime artists and share illustrations, ideas and fan work.",

        "Online Esports Community Cup":
            "Online gaming tournament connecting competitive players and fans.",

        "Cosplay Photography Day":
            "Cosplayers and photographers come together for a creative fan meetup.",

        "Fantasy Film Fan Screening":
            "Online fantasy film screening followed by a community discussion."

    };


    function formatEventDate(dateString) {

        if (!dateString) {
            return "Date not available";
        }


        const date =
            new Date(
                `${dateString}T00:00:00`
            );


        if (
            Number.isNaN(
                date.getTime()
            )
        ) {

            return dateString;

        }


        return date.toLocaleDateString(
            "en-US",
            {
                weekday: "long",
                month: "long",
                day: "numeric",
                year: "numeric"
            }
        );

    }


    function formatEventType(type) {

        const normalizedType =
            normalize(type);


        const names = {

            convention:
                "Convention",

            tournament:
                "Tournament",

            meetup:
                "Meetup",

            screening:
                "Screening"

        };


        return (
            names[normalizedType] ||
            type ||
            "Fan Event"
        );

    }


    function formatLocation(city) {

        const normalizedCity =
            normalize(city);


        const locations = {

            karachi:
                "Karachi, Pakistan",

            lahore:
                "Lahore, Pakistan",

            islamabad:
                "Islamabad, Pakistan",

            online:
                "Online Event"

        };


        return (
            locations[normalizedCity] ||
            city ||
            "Location unavailable"
        );

    }


    function openEventModal(card) {

        if (
            !card ||
            !eventDetailsModal
        ) {
            return;
        }


        const title =
            card.dataset.title ||
            "Fan Event";


        const date =
            card.dataset.date ||
            "";


        const city =
            card.dataset.city ||
            "";


        const type =
            card.dataset.type ||
            "";


        if (eventModalTitle) {

            eventModalTitle.textContent =
                title;

        }


        if (eventModalDescription) {

            eventModalDescription.textContent =
                eventDescriptions[title] ||
                "Discover this fan event and connect with the FanHub+ community.";

        }


        if (eventModalDate) {

            eventModalDate.textContent =
                formatEventDate(date);

        }


        if (eventModalLocation) {

            eventModalLocation.textContent =
                formatLocation(city);

        }


        if (eventModalType) {

            eventModalType.textContent =
                formatEventType(type);

        }


        if (
            window.bootstrap &&
            bootstrap.Modal
        ) {

            const modal =
                bootstrap.Modal
                    .getOrCreateInstance(
                        eventDetailsModal
                    );


            modal.show();

        }

    }


    if (eventsGrid) {

        eventsGrid.addEventListener(
            "click",
            (event) => {

                const detailsButton =
                    event.target.closest(
                        ".event-details-button"
                    );


                if (!detailsButton) {
                    return;
                }


                const card =
                    detailsButton.closest(
                        ".fan-event-card"
                    );


                openEventModal(card);

            }
        );

    }


    /* =====================================================
       URL PARAMETERS
       ===================================================== */

    function loadFiltersFromURL() {

        const params =
            new URLSearchParams(
                window.location.search
            );


        const search =
            params.get("search");


        const city =
            params.get("city");


        const category =
            params.get("category");


        const date =
            params.get("date");


        if (search && eventSearch) {

            eventSearch.value =
                search;

            filters.search =
                normalize(search);

        }


        if (
            city &&
            [
                "karachi",
                "lahore",
                "islamabad",
                "online"
            ].includes(
                normalize(city)
            )
        ) {

            filters.city =
                normalize(city);


            if (eventCityFilter) {

                eventCityFilter.value =
                    filters.city;

            }


            if (eventSidebarCity) {

                eventSidebarCity.value =
                    filters.city;

            }

        }


        if (category) {

            const normalizedCategory =
                normalize(category);


            const categoryExists =
                Array.from(
                    categoryChips
                ).some(
                    (chip) =>
                        chip.dataset
                            .eventCategory ===
                        normalizedCategory
                );


            if (categoryExists) {

                filters.category =
                    normalizedCategory;


                categoryChips.forEach(
                    (chip) => {

                        chip.classList.toggle(
                            "active",
                            chip.dataset
                                .eventCategory ===
                                normalizedCategory
                        );

                    }
                );

            }

        }


        if (date) {

            const matchingDay =
                Array.from(
                    calendarDays
                ).find(
                    (day) =>
                        day.dataset
                            .eventDate ===
                        date
                );


            if (matchingDay) {

                filters.date = date;

                matchingDay.classList.add(
                    "active"
                );

            }

        }

    }


    function updateURL() {

        const params =
            new URLSearchParams();


        if (filters.search) {

            params.set(
                "search",
                filters.search
            );

        }


        if (filters.city !== "all") {

            params.set(
                "city",
                filters.city
            );

        }


        if (
            filters.category !== "all"
        ) {

            params.set(
                "category",
                filters.category
            );

        }


        if (filters.date) {

            params.set(
                "date",
                filters.date
            );

        }


        const query =
            params.toString();


        const newURL =
            query
                ? `${window.location.pathname}?${query}`
                : window.location.pathname;


        try {

            history.replaceState(
                null,
                "",
                newURL
            );

        } catch (error) {
            // Ignore browser restriction.
        }

    }


    /* =====================================================
       TOAST
       ===================================================== */

    function showEventsToast(message) {

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
             * "/" focuses event search,
             * except while typing.
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

                if (eventSearch) {

                    eventSearch.focus();

                }

            }


            /*
             * Escape clears selected
             * calendar date.
             */

            if (
                event.key === "Escape" &&
                filters.date
            ) {

                filters.date = "";


                calendarDays.forEach(
                    (day) => {

                        day.classList.remove(
                            "active"
                        );

                    }
                );


                applyEventFilters();

            }

        }
    );


    /* =====================================================
       INITIALIZE
       ===================================================== */

    loadFiltersFromURL();

    restoreEventView();

    applyEventFilters();

});