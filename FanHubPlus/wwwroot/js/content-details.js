/* =========================================================
   FANHUB+ - CONTENT DETAILS PAGE
   Tabs, Rating, Bookmark, Notes, Share and Reviews
   ========================================================= */

"use strict";

document.addEventListener("DOMContentLoaded", () => {

    /* =====================================================
       ELEMENTS
       ===================================================== */

    const tabButtons =
        document.querySelectorAll(".details-tab");

    const tabPanels =
        document.querySelectorAll(".details-tab-panel");

    const tabTargetButtons =
        document.querySelectorAll("[data-tab-target]");

    const ratingContainer =
        document.getElementById("userRatingStars");

    const ratingButtons =
        ratingContainer
            ? ratingContainer.querySelectorAll("[data-rating]")
            : [];

    const ratingMessage =
        document.getElementById("ratingMessage");

    const bookmarkButton =
        document.querySelector(
            ".details-secondary-button[data-bookmark]"
        );

    const noteField =
        document.getElementById("contentNote");

    const saveNoteButton =
        document.getElementById("saveNoteButton");

    const shareButton =
        document.getElementById("shareContentButton");

    const reviewForm =
        document.getElementById("reviewForm");

    const reviewName =
        document.getElementById("reviewName");

    const reviewText =
        document.getElementById("reviewText");

    const reviewRatingContainer =
        document.getElementById("reviewRatingSelect");

    const reviewRatingButtons =
        reviewRatingContainer
            ? reviewRatingContainer.querySelectorAll(
                "[data-value]"
            )
            : [];

    const reviewsList =
        document.getElementById("reviewsList");

    const galleryButtons =
        document.querySelectorAll(
            ".gallery-filter button"
        );


    /* =====================================================
       STORAGE KEYS
       ===================================================== */

    const STORAGE = {
        rating: "fanhubSoloLevelingRating",
        note: "fanhubSoloLevelingNote",
        bookmarks: "fanhubBookmarks",
        reviews: "fanhubSoloLevelingReviews"
    };


    let selectedReviewRating = 0;


    /* =====================================================
       SAFE LOCAL STORAGE
       ===================================================== */

    function getStorage(key, fallback = null) {

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


    function setStorage(key, value) {

        try {

            localStorage.setItem(
                key,
                value
            );

            return true;

        } catch (error) {

            return false;

        }

    }


    function getJSONStorage(key, fallback = []) {

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


    function setJSONStorage(key, value) {

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


    /* =====================================================
       TOAST
       ===================================================== */

    function showToast(message) {

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

            return;

        }


        /*
         * Bootstrap unavailable fallback.
         */

        console.log(message);

    }


    /* =====================================================
       TABS
       ===================================================== */

    function openTab(tabName, scroll = false) {

        if (!tabName) {
            return;
        }


        const targetPanel =
            document.getElementById(tabName);


        if (!targetPanel) {
            return;
        }


        tabButtons.forEach((button) => {

            const isActive =
                button.dataset.tab === tabName;

            button.classList.toggle(
                "active",
                isActive
            );

            button.setAttribute(
                "aria-selected",
                String(isActive)
            );

        });


        tabPanels.forEach((panel) => {

            panel.classList.toggle(
                "active",
                panel.id === tabName
            );

        });


        /*
         * Store selected tab in URL hash.
         */

        try {

            history.replaceState(
                null,
                "",
                `#${tabName}`
            );

        } catch (error) {
            // No action required.
        }


        /*
         * Optional scroll when switching from
         * buttons inside a section.
         */

        if (scroll) {

            const navigation =
                document.querySelector(
                    ".details-navigation"
                );

            if (navigation) {

                const headerOffset = 80;

                const top =
                    navigation.getBoundingClientRect().top +
                    window.scrollY -
                    headerOffset;

                window.scrollTo({
                    top,
                    behavior: "smooth"
                });

            }

        }

    }


    tabButtons.forEach((button) => {

        button.addEventListener(
            "click",
            () => {

                openTab(
                    button.dataset.tab
                );

            }
        );

    });


    /*
     * Buttons such as:
     * View Gallery
     * View All Characters
     */

    tabTargetButtons.forEach((button) => {

        button.addEventListener(
            "click",
            () => {

                openTab(
                    button.dataset.tabTarget,
                    true
                );

            }
        );

    });


    /*
     * Open tab directly through URL:
     * content-details.html#characters
     */

    function loadTabFromHash() {

        const hash =
            window.location.hash
                .replace("#", "")
                .trim();


        const validTab =
            Array.from(tabPanels)
                .some(
                    (panel) =>
                        panel.id === hash
                );


        if (validTab) {

            openTab(hash);

        } else {

            openTab("overview");

        }

    }


    /* =====================================================
       USER RATING
       ===================================================== */

    function paintRating(rating) {

        ratingButtons.forEach((button) => {

            const buttonRating =
                Number(
                    button.dataset.rating
                );

            const icon =
                button.querySelector("i");


            const active =
                buttonRating <= rating;


            button.classList.toggle(
                "active",
                active
            );


            if (icon) {

                icon.className =
                    active
                        ? "bi bi-star-fill"
                        : "bi bi-star";

            }

        });

    }


    function saveUserRating(rating) {

        rating =
            Number(rating);


        if (
            !Number.isFinite(rating) ||
            rating < 1 ||
            rating > 5
        ) {
            return;
        }


        paintRating(rating);

        setStorage(
            STORAGE.rating,
            String(rating)
        );


        if (ratingMessage) {

            ratingMessage.textContent =
                `You rated this ${rating}/5`;

        }


        showToast(
            `Rating saved: ${rating}/5`
        );

    }


    ratingButtons.forEach((button) => {

        button.addEventListener(
            "click",
            () => {

                saveUserRating(
                    button.dataset.rating
                );

            }
        );


        /*
         * Preview rating on hover.
         */

        button.addEventListener(
            "mouseenter",
            () => {

                paintRating(
                    Number(
                        button.dataset.rating
                    )
                );

            }
        );

    });


    if (ratingContainer) {

        ratingContainer.addEventListener(
            "mouseleave",
            () => {

                const savedRating =
                    Number(
                        getStorage(
                            STORAGE.rating,
                            "0"
                        )
                    );

                paintRating(savedRating);

            }
        );

    }


    function restoreRating() {

        const savedRating =
            Number(
                getStorage(
                    STORAGE.rating,
                    "0"
                )
            );


        if (
            savedRating >= 1 &&
            savedRating <= 5
        ) {

            paintRating(savedRating);


            if (ratingMessage) {

                ratingMessage.textContent =
                    `You rated this ${savedRating}/5`;

            }

        }

    }


    /* =====================================================
       BOOKMARK
       ===================================================== */

    function getBookmarks() {

        const bookmarks =
            getJSONStorage(
                STORAGE.bookmarks,
                []
            );


        return Array.isArray(bookmarks)
            ? bookmarks
            : [];

    }


    function updateBookmarkUI(saved) {

        if (!bookmarkButton) {
            return;
        }


        const icon =
            bookmarkButton.querySelector("i");


        bookmarkButton.classList.toggle(
            "saved",
            saved
        );


        if (saved) {

            if (icon) {
                icon.className =
                    "bi bi-bookmark-fill";
            }

            bookmarkButton.lastChild.textContent =
                " Saved";

        } else {

            if (icon) {
                icon.className =
                    "bi bi-bookmark";
            }

            bookmarkButton.lastChild.textContent =
                " Bookmark";

        }

    }


    function restoreBookmark() {

        if (!bookmarkButton) {
            return;
        }


        const contentName =
            bookmarkButton.dataset.bookmark;


        const bookmarks =
            getBookmarks();


        updateBookmarkUI(
            bookmarks.includes(contentName)
        );

    }


    if (bookmarkButton) {

        bookmarkButton.addEventListener(
            "click",
            () => {

                const contentName =
                    bookmarkButton.dataset.bookmark;


                if (!contentName) {
                    return;
                }


                let bookmarks =
                    getBookmarks();


                const alreadySaved =
                    bookmarks.includes(
                        contentName
                    );


                if (alreadySaved) {

                    bookmarks =
                        bookmarks.filter(
                            (item) =>
                                item !== contentName
                        );

                } else {

                    bookmarks.push(
                        contentName
                    );

                }


                setJSONStorage(
                    STORAGE.bookmarks,
                    bookmarks
                );


                updateBookmarkUI(
                    !alreadySaved
                );


                showToast(
                    alreadySaved
                        ? "Removed from bookmarks."
                        : "Added to bookmarks."
                );

            }
        );

    }


    /* =====================================================
       PERSONAL NOTES
       ===================================================== */

    function restoreNote() {

        if (!noteField) {
            return;
        }


        const savedNote =
            getStorage(
                STORAGE.note,
                ""
            );


        noteField.value =
            savedNote;

    }


    if (
        saveNoteButton &&
        noteField
    ) {

        saveNoteButton.addEventListener(
            "click",
            () => {

                const note =
                    noteField.value.trim();


                setStorage(
                    STORAGE.note,
                    note
                );


                if (note) {

                    showToast(
                        "Personal note saved."
                    );

                } else {

                    showToast(
                        "Personal note cleared."
                    );

                }

            }
        );

    }


    /*
     * Ctrl + S / Cmd + S saves note
     * when user is typing in note box.
     */

    if (noteField) {

        noteField.addEventListener(
            "keydown",
            (event) => {

                const saveShortcut =
                    (event.ctrlKey || event.metaKey) &&
                    event.key.toLowerCase() === "s";


                if (!saveShortcut) {
                    return;
                }


                event.preventDefault();


                setStorage(
                    STORAGE.note,
                    noteField.value.trim()
                );


                showToast(
                    "Personal note saved."
                );

            }
        );

    }


    /* =====================================================
       SHARE
       ===================================================== */

    async function shareContent() {

        const shareData = {

            title: "Solo Leveling | FanHub+",

            text:
                "Check out Solo Leveling on FanHub+.",

            url:
                window.location.href

        };


        /*
         * Native share dialog on supported
         * browsers / mobile devices.
         */

        if (navigator.share) {

            try {

                await navigator.share(
                    shareData
                );

                return;

            } catch (error) {

                /*
                 * User closing the share dialog
                 * should not show an error.
                 */

                if (
                    error.name === "AbortError"
                ) {
                    return;
                }

            }

        }


        /*
         * Clipboard fallback.
         */

        if (
            navigator.clipboard &&
            window.isSecureContext
        ) {

            try {

                await navigator.clipboard.writeText(
                    window.location.href
                );


                showToast(
                    "Link copied to clipboard."
                );

                return;

            } catch (error) {
                // Use final fallback below.
            }

        }


        /*
         * Older browser fallback.
         */

        const temporaryInput =
            document.createElement(
                "textarea"
            );


        temporaryInput.value =
            window.location.href;

        temporaryInput.style.position =
            "fixed";

        temporaryInput.style.opacity =
            "0";


        document.body.appendChild(
            temporaryInput
        );


        temporaryInput.select();


        try {

            document.execCommand(
                "copy"
            );


            showToast(
                "Link copied to clipboard."
            );

        } catch (error) {

            showToast(
                "Unable to copy the link."
            );

        }


        temporaryInput.remove();

    }


    if (shareButton) {

        shareButton.addEventListener(
            "click",
            shareContent
        );

    }


    /* =====================================================
       REVIEW STAR SELECTION
       ===================================================== */

    function paintReviewRating(rating) {

        reviewRatingButtons.forEach(
            (button) => {

                const value =
                    Number(
                        button.dataset.value
                    );


                const active =
                    value <= rating;


                const icon =
                    button.querySelector("i");


                button.classList.toggle(
                    "active",
                    active
                );


                if (icon) {

                    icon.className =
                        active
                            ? "bi bi-star-fill"
                            : "bi bi-star";

                }

            }
        );

    }


    reviewRatingButtons.forEach(
        (button) => {

            button.addEventListener(
                "click",
                () => {

                    selectedReviewRating =
                        Number(
                            button.dataset.value
                        );


                    paintReviewRating(
                        selectedReviewRating
                    );

                }
            );


            button.addEventListener(
                "mouseenter",
                () => {

                    paintReviewRating(
                        Number(
                            button.dataset.value
                        )
                    );

                }
            );

        }
    );


    if (reviewRatingContainer) {

        reviewRatingContainer.addEventListener(
            "mouseleave",
            () => {

                paintReviewRating(
                    selectedReviewRating
                );

            }
        );

    }


    /* =====================================================
       REVIEW HELPERS
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

        const parts =
            name
                .trim()
                .split(/\s+/)
                .filter(Boolean);


        if (parts.length === 0) {
            return "FH";
        }


        if (parts.length === 1) {

            return parts[0]
                .substring(0, 2)
                .toUpperCase();

        }


        return (
            parts[0][0] +
            parts[parts.length - 1][0]
        ).toUpperCase();

    }


    function createStars(rating) {

        let stars = "";


        for (
            let index = 1;
            index <= 5;
            index++
        ) {

            stars +=
                index <= rating
                    ? '<i class="bi bi-star-fill"></i>'
                    : '<i class="bi bi-star"></i>';

        }


        return stars;

    }


    function createReviewElement(review) {

        const article =
            document.createElement(
                "article"
            );


        article.className =
            "review-card";


        article.innerHTML = `

            <div class="review-user-avatar">
                ${escapeHTML(
                    getInitials(review.name)
                )}
            </div>

            <div class="review-content">

                <div class="review-header">

                    <div>

                        <h3>
                            ${escapeHTML(review.name)}
                        </h3>

                        <span>
                            ${escapeHTML(review.date)}
                        </span>

                    </div>

                    <div class="review-stars">

                        ${createStars(
                            review.rating
                        )}

                    </div>

                </div>

                <p>
                    ${escapeHTML(review.text)}
                </p>

            </div>

        `;


        return article;

    }


    /* =====================================================
       RESTORE USER REVIEWS
       ===================================================== */

    function restoreReviews() {

        if (!reviewsList) {
            return;
        }


        const reviews =
            getJSONStorage(
                STORAGE.reviews,
                []
            );


        if (!Array.isArray(reviews)) {
            return;
        }


        reviews.forEach((review) => {

            const element =
                createReviewElement(
                    review
                );


            reviewsList.prepend(
                element
            );

        });

    }


    /* =====================================================
       SUBMIT REVIEW
       ===================================================== */

    if (reviewForm) {

        reviewForm.addEventListener(
            "submit",
            (event) => {

                event.preventDefault();


                const name =
                    reviewName
                        ? reviewName.value.trim()
                        : "";


                const text =
                    reviewText
                        ? reviewText.value.trim()
                        : "";


                if (!name || !text) {

                    showToast(
                        "Please complete the review form."
                    );

                    return;

                }


                if (
                    selectedReviewRating < 1 ||
                    selectedReviewRating > 5
                ) {

                    showToast(
                        "Please select a rating."
                    );

                    return;

                }


                const review = {

                    id: Date.now(),

                    name,

                    text,

                    rating:
                        selectedReviewRating,

                    date:
                        "Just now"

                };


                const reviews =
                    getJSONStorage(
                        STORAGE.reviews,
                        []
                    );


                const safeReviews =
                    Array.isArray(reviews)
                        ? reviews
                        : [];


                safeReviews.push(
                    review
                );


                setJSONStorage(
                    STORAGE.reviews,
                    safeReviews
                );


                if (reviewsList) {

                    const reviewElement =
                        createReviewElement(
                            review
                        );


                    reviewsList.prepend(
                        reviewElement
                    );

                }


                reviewForm.reset();


                selectedReviewRating = 0;


                paintReviewRating(0);


                showToast(
                    "Your review has been submitted."
                );

            }
        );

    }


    /* =====================================================
       GALLERY FILTER BUTTON UI
       ===================================================== */

    galleryButtons.forEach((button) => {

        button.addEventListener(
            "click",
            () => {

                galleryButtons.forEach(
                    (item) => {

                        item.classList.remove(
                            "active"
                        );

                    }
                );


                button.classList.add(
                    "active"
                );

            }
        );

    });


    /* =====================================================
       ESCAPE KEY
       ===================================================== */

    document.addEventListener(
        "keydown",
        (event) => {

            if (event.key !== "Escape") {
                return;
            }


            /*
             * Remove focus from form fields
             * when Escape is pressed.
             */

            const activeElement =
                document.activeElement;


            if (
                activeElement &&
                (
                    activeElement.tagName === "INPUT" ||
                    activeElement.tagName === "TEXTAREA"
                )
            ) {

                activeElement.blur();

            }

        }
    );


    /* =====================================================
       INITIALIZE
       ===================================================== */

    loadTabFromHash();

    restoreRating();

    restoreBookmark();

    restoreNote();

    restoreReviews();

});