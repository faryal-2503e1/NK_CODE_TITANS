/* =========================================================
   FANHUB+ - MAIN JAVASCRIPT
   ========================================================= */

"use strict";


/* =========================================================
   DOM READY
   ========================================================= */

document.addEventListener("DOMContentLoaded", () => {

    initializeLoader();
    initializeNavbar();
    initializeRevealAnimations();
    initializeCounters();
    initializeTheme();
    initializeFontSize();
    initializeBookmarks();
    initializeGlobalSearch();
    initializeParticles();
    initializeMouseGlow();
    initializeKeyboardShortcuts();
    initializeCharacterHeroSlider();
    initializeModelColorSelector();

});


/* =========================================================
   PAGE LOADER
   ========================================================= */

function initializeLoader() {

    const loader = document.getElementById("pageLoader");

    if (!loader) {
        return;
    }

    window.addEventListener("load", () => {

        setTimeout(() => {

            loader.classList.add("hidden");

        }, 500);

    });

}


/* =========================================================
   NAVBAR SCROLL EFFECT
   ========================================================= */

function initializeNavbar() {

    const header = document.getElementById("mainHeader");

    if (!header) {
        return;
    }


    const updateNavbar = () => {

        if (window.scrollY > 30) {

            header.classList.add("scrolled");

        } else {

            header.classList.remove("scrolled");

        }

    };


    updateNavbar();

    window.addEventListener(
        "scroll",
        updateNavbar,
        {
            passive: true
        }
    );

}


/* =========================================================
   SCROLL REVEAL ANIMATION
   ========================================================= */

function initializeRevealAnimations() {

    const elements = document.querySelectorAll(".reveal");

    if (!elements.length) {
        return;
    }


    if (!("IntersectionObserver" in window)) {

        elements.forEach((element) => {

            element.classList.add("visible");

        });

        return;

    }


    const observer = new IntersectionObserver(

        (entries, currentObserver) => {

            entries.forEach((entry) => {

                if (!entry.isIntersecting) {
                    return;
                }


                entry.target.classList.add("visible");

                currentObserver.unobserve(entry.target);

            });

        },

        {
            threshold: 0.12,
            rootMargin: "0px 0px -40px 0px"
        }

    );


    elements.forEach((element, index) => {

        /*
         * Small delay makes card groups appear naturally
         * instead of every item appearing at exactly
         * the same time.
         */

        element.style.transitionDelay =
            `${Math.min(index % 6, 5) * 70}ms`;

        observer.observe(element);

    });

}


/* =========================================================
   HERO COUNTERS
   ========================================================= */

function initializeCounters() {

    const counters = document.querySelectorAll(".counter");

    if (!counters.length) {
        return;
    }


    let countersStarted = false;


    const startCounters = () => {

        if (countersStarted) {
            return;
        }

        countersStarted = true;


        counters.forEach((counter) => {

            const target =
                Number(counter.dataset.target) || 0;

            const duration = 1300;

            const startTime = performance.now();


            const updateCounter = (currentTime) => {

                const elapsed =
                    currentTime - startTime;

                const progress =
                    Math.min(elapsed / duration, 1);


                /*
                 * Ease-out animation.
                 */

                const easedProgress =
                    1 - Math.pow(1 - progress, 3);


                const currentValue =
                    Math.floor(target * easedProgress);


                counter.textContent =
                    currentValue.toLocaleString();


                if (progress < 1) {

                    requestAnimationFrame(updateCounter);

                } else {

                    counter.textContent =
                        target.toLocaleString();

                }

            };


            requestAnimationFrame(updateCounter);

        });

    };


    const statsSection =
        document.querySelector(".hero-stats");


    if (!statsSection) {

        startCounters();

        return;

    }


    if (!("IntersectionObserver" in window)) {

        startCounters();

        return;

    }


    const counterObserver =
        new IntersectionObserver(

            (entries) => {

                entries.forEach((entry) => {

                    if (entry.isIntersecting) {

                        startCounters();

                        counterObserver.disconnect();

                    }

                });

            },

            {
                threshold: 0.4
            }

        );


    counterObserver.observe(statsSection);

}


/* =========================================================
   DARK / LIGHT THEME
   ========================================================= */

function initializeTheme() {

    const themeButton =
        document.getElementById("themeButton");

    if (!themeButton) {
        return;
    }


    const savedTheme =
        localStorage.getItem("fanhub-theme");


    if (savedTheme === "light") {

        document.body.classList.add("light-theme");

    }


    updateThemeIcon(themeButton);


    themeButton.addEventListener("click", () => {

        document.body.classList.toggle("light-theme");


        const isLight =
            document.body.classList.contains(
                "light-theme"
            );


        document.documentElement.setAttribute(
            "data-bs-theme",
            isLight ? "light" : "dark"
        );


        localStorage.setItem(
            "fanhub-theme",
            isLight ? "light" : "dark"
        );


        updateThemeIcon(themeButton);


        showToast(

            isLight
                ? "Light mode enabled."
                : "Dark mode enabled."

        );

    });

}


/* =========================================================
   UPDATE THEME ICON
   ========================================================= */

function updateThemeIcon(button) {

    const icon = button.querySelector("i");

    if (!icon) {
        return;
    }


    const isLight =
        document.body.classList.contains(
            "light-theme"
        );


    if (isLight) {

        icon.className = "bi bi-sun";

        button.title = "Switch to dark mode";

    } else {

        icon.className = "bi bi-moon-stars";

        button.title = "Switch to light mode";

    }

}


/* =========================================================
   FONT SIZE ACCESSIBILITY
   ========================================================= */

function initializeFontSize() {

    const fontButton =
        document.getElementById("fontSizeButton");

    if (!fontButton) {
        return;
    }


    const savedFontSize =
        localStorage.getItem("fanhub-font-size");


    if (savedFontSize === "large") {

        document.body.classList.add("large-text");

    }


    fontButton.addEventListener("click", () => {

        document.body.classList.toggle(
            "large-text"
        );


        const largeTextEnabled =
            document.body.classList.contains(
                "large-text"
            );


        localStorage.setItem(

            "fanhub-font-size",

            largeTextEnabled
                ? "large"
                : "normal"

        );


        showToast(

            largeTextEnabled
                ? "Larger text enabled."
                : "Normal text restored."

        );

    });

}


/* =========================================================
   BOOKMARKS
   ========================================================= */

function initializeBookmarks() {

    const bookmarkButtons =
        document.querySelectorAll(
            "[data-bookmark]"
        );


    if (!bookmarkButtons.length) {
        return;
    }


    const savedBookmarks =
        getBookmarks();


    bookmarkButtons.forEach((button) => {

        const itemName =
            button.dataset.bookmark;


        if (savedBookmarks.includes(itemName)) {

            setBookmarkState(
                button,
                true
            );

        }


        button.addEventListener(
            "click",
            (event) => {

                /*
                 * Some bookmark buttons are placed
                 * inside clickable content cards.
                 */

                event.preventDefault();

                event.stopPropagation();


                toggleBookmark(
                    button,
                    itemName
                );

            }
        );

    });

}


/* =========================================================
   GET BOOKMARKS
   ========================================================= */

function getBookmarks() {

    try {

        const storedBookmarks =
            localStorage.getItem(
                "fanhub-bookmarks"
            );


        if (!storedBookmarks) {
            return [];
        }


        const parsedBookmarks =
            JSON.parse(storedBookmarks);


        return Array.isArray(parsedBookmarks)
            ? parsedBookmarks
            : [];

    } catch (error) {

        console.warn(
            "FanHub+ bookmarks could not be loaded.",
            error
        );

        return [];

    }

}


/* =========================================================
   SAVE BOOKMARKS
   ========================================================= */

function saveBookmarks(bookmarks) {

    localStorage.setItem(

        "fanhub-bookmarks",

        JSON.stringify(bookmarks)

    );

}


/* =========================================================
   TOGGLE BOOKMARK
   ========================================================= */

function toggleBookmark(button, itemName) {

    let bookmarks = getBookmarks();


    const alreadySaved =
        bookmarks.includes(itemName);


    if (alreadySaved) {

        bookmarks =
            bookmarks.filter(
                (bookmark) =>
                    bookmark !== itemName
            );


        setBookmarkState(
            button,
            false
        );


        showToast(
            `${itemName} removed from bookmarks.`
        );

    } else {

        bookmarks.push(itemName);


        setBookmarkState(
            button,
            true
        );


        showToast(
            `${itemName} added to bookmarks.`
        );

    }


    saveBookmarks(bookmarks);

}


/* =========================================================
   BOOKMARK BUTTON STATE
   ========================================================= */

function setBookmarkState(
    button,
    saved
) {

    const icon =
        button.querySelector("i");


    button.classList.toggle(
        "saved",
        saved
    );


    if (!icon) {
        return;
    }


    /*
     * Home cards currently use hearts.
     * Other pages can use bookmark icons.
     */

    if (
        icon.classList.contains("bi-heart") ||
        icon.classList.contains("bi-heart-fill")
    ) {

        icon.className =
            saved
                ? "bi bi-heart-fill"
                : "bi bi-heart";

    } else {

        icon.className =
            saved
                ? "bi bi-bookmark-fill"
                : "bi bi-bookmark";

    }

}


/* =========================================================
   GLOBAL SEARCH
   ========================================================= */

function initializeGlobalSearch() {

    const searchInput =
        document.getElementById("globalSearch");


    if (!searchInput) {
        return;
    }


    searchInput.addEventListener(
        "keydown",
        (event) => {

            if (event.key !== "Enter") {
                return;
            }


            const searchTerm =
                searchInput.value.trim();


            if (!searchTerm) {

                showToast(
                    "Type something to search."
                );

                return;

            }


            /*
             * Explore page will read this
             * query string and filter results.
             */

            window.location.href =
                `explore.html?search=${encodeURIComponent(
                    searchTerm
                )}`;

        }
    );

}


/* =========================================================
   KEYBOARD SHORTCUTS
   ========================================================= */

function initializeKeyboardShortcuts() {

    document.addEventListener(
        "keydown",
        (event) => {

            /*
             * Ctrl + K / Cmd + K
             * focuses the global search box.
             */

            const searchInput =
                document.getElementById(
                    "globalSearch"
                );


            if (
                (event.ctrlKey || event.metaKey) &&
                event.key.toLowerCase() === "k"
            ) {

                if (!searchInput) {
                    return;
                }


                event.preventDefault();

                searchInput.focus();

                searchInput.select();

            }


            /*
             * Escape removes focus from search.
             */

            if (
                event.key === "Escape" &&
                document.activeElement === searchInput
            ) {

                searchInput.blur();

            }

        }
    );

}


/* =========================================================
   PARTICLES
   ========================================================= */

function initializeParticles() {

    const particleContainer =
        document.getElementById("particles");


    if (!particleContainer) {
        return;
    }


    /*
     * Avoid unnecessary visual motion for
     * users who prefer reduced motion.
     */

    const reducedMotion =
        window.matchMedia(
            "(prefers-reduced-motion: reduce)"
        ).matches;


    if (reducedMotion) {
        return;
    }


    const totalParticles = 28;


    for (
        let index = 0;
        index < totalParticles;
        index++
    ) {

        const particle =
            document.createElement("span");


        particle.className =
            "particle";


        particle.style.left =
            `${Math.random() * 100}%`;


        particle.style.top =
            `${Math.random() * 100}%`;


        particle.style.opacity =
            `${0.08 + Math.random() * 0.25}`;


        const size =
            1 + Math.random() * 2;


        particle.style.width =
            `${size}px`;

        particle.style.height =
            `${size}px`;


        particle.style.animationDuration =
            `${12 + Math.random() * 20}s`;


        particle.style.animationDelay =
            `${Math.random() * -20}s`;


        particleContainer.appendChild(
            particle
        );

    }

}


/* =========================================================
   MOUSE GLOW
   ========================================================= */

function initializeMouseGlow() {

    const glow =
        document.getElementById("mouseGlow");


    if (!glow) {
        return;
    }


    const isTouchDevice =
        window.matchMedia(
            "(pointer: coarse)"
        ).matches;


    if (isTouchDevice) {

        glow.style.display = "none";

        return;

    }


    let mouseX = 0;
    let mouseY = 0;

    let currentX = 0;
    let currentY = 0;


    document.addEventListener(
        "mousemove",
        (event) => {

            mouseX = event.clientX;

            mouseY = event.clientY;

        }
    );


    const animateGlow = () => {

        currentX +=
            (mouseX - currentX) * 0.08;

        currentY +=
            (mouseY - currentY) * 0.08;


        glow.style.left =
            `${currentX}px`;

        glow.style.top =
            `${currentY}px`;


        requestAnimationFrame(
            animateGlow
        );

    };


    animateGlow();

}


/* =========================================================
   TOAST
   ========================================================= */

function showToast(message) {

    const toastElement =
        document.getElementById(
            "fanHubToast"
        );


    const toastMessage =
        document.getElementById(
            "toastMessage"
        );


    if (!toastElement) {

        console.log(message);

        return;

    }


    if (toastMessage) {

        toastMessage.textContent =
            message;

    }


    /*
     * Bootstrap is loaded in index.html.
     */

    if (
        typeof bootstrap === "undefined" ||
        !bootstrap.Toast
    ) {

        console.log(message);

        return;

    }


    const toast =
        bootstrap.Toast.getOrCreateInstance(
            toastElement,
            {
                delay: 2500
            }
        );


    toast.show();

}


/* =========================================================
   GENERAL CONTENT SEARCH
   Used by Explore and other pages later.
   ========================================================= */

function filterCardsBySearch(
    searchValue,
    selector = ".content-card"
) {

    const cards =
        document.querySelectorAll(
            selector
        );


    const normalizedSearch =
        searchValue
            .trim()
            .toLowerCase();


    let visibleItems = 0;


    cards.forEach((card) => {

        const cardText =
            card.textContent
                .toLowerCase();


        const shouldShow =
            !normalizedSearch ||
            cardText.includes(
                normalizedSearch
            );


        card.style.display =
            shouldShow
                ? ""
                : "none";


        if (shouldShow) {

            visibleItems++;

        }

    });


    return visibleItems;

}


/* =========================================================
   CATEGORY FILTER
   Used by Explore page.
   ========================================================= */

function filterCardsByCategory(
    category,
    selector = ".content-card"
) {

    const cards =
        document.querySelectorAll(
            selector
        );


    const normalizedCategory =
        category
            .trim()
            .toLowerCase();


    let visibleItems = 0;


    cards.forEach((card) => {

        const cardCategory =
            (
                card.dataset.category ||
                ""
            ).toLowerCase();


        const shouldShow =
            normalizedCategory === "all" ||
            !normalizedCategory ||
            cardCategory === normalizedCategory;


        card.style.display =
            shouldShow
                ? ""
                : "none";


        if (shouldShow) {

            visibleItems++;

        }

    });


    return visibleItems;

}


/* =========================================================
   URL PARAMETERS
   ========================================================= */

function getQueryParameter(parameterName) {

    const parameters =
        new URLSearchParams(
            window.location.search
        );


    return parameters.get(
        parameterName
    );

}


/* =========================================================
   SIMPLE FORM VALIDATION HELPER
   Login / Register pages can reuse this.
   ========================================================= */

function validateRequiredFields(form) {

    if (!form) {
        return false;
    }


    const requiredFields =
        form.querySelectorAll(
            "[required]"
        );


    let valid = true;


    requiredFields.forEach((field) => {

        const value =
            field.value.trim();


        field.classList.remove(
            "field-error"
        );


        if (!value) {

            field.classList.add(
                "field-error"
            );

            valid = false;

        }

    });


    return valid;

}


/* =========================================================
   EMAIL VALIDATION HELPER
   ========================================================= */

function isValidEmail(email) {

    const emailPattern =
        /^[^\s@]+@[^\s@]+\.[^\s@]+$/;


    return emailPattern.test(
        email.trim()
    );

}


/* =========================================================
   DEMO USER SESSION
   Frontend only.
   Backend authentication will replace this later.
   ========================================================= */

function saveDemoUser(user) {

    localStorage.setItem(

        "fanhub-user",

        JSON.stringify(user)

    );

}


function getDemoUser() {

    try {

        const storedUser =
            localStorage.getItem(
                "fanhub-user"
            );


        if (!storedUser) {
            return null;
        }


        return JSON.parse(
            storedUser
        );

    } catch (error) {

        return null;

    }

}


function logoutDemoUser() {

    localStorage.removeItem(
        "fanhub-user"
    );


    window.location.href =
        "login.html";

}


/* =========================================================
   FAVORITE FANDOMS
   Dashboard / profile can reuse this.
   ========================================================= */

function getFavoriteFandoms() {

    try {

        const stored =
            localStorage.getItem(
                "fanhub-favorite-fandoms"
            );


        return stored
            ? JSON.parse(stored)
            : [];

    } catch (error) {

        return [];

    }

}


function saveFavoriteFandoms(fandoms) {

    localStorage.setItem(

        "fanhub-favorite-fandoms",

        JSON.stringify(fandoms)

    );

}


/* =========================================================
   NOTES
   Content Details page can reuse this.
   ========================================================= */

function saveContentNote(
    contentId,
    note
) {

    const notes =
        getContentNotes();


    notes[contentId] =
        note;


    localStorage.setItem(

        "fanhub-content-notes",

        JSON.stringify(notes)

    );

}


function getContentNotes() {

    try {

        const stored =
            localStorage.getItem(
                "fanhub-content-notes"
            );


        return stored
            ? JSON.parse(stored)
            : {};

    } catch (error) {

        return {};

    }

}


/* =========================================================
   SHARE CONTENT
   ========================================================= */

async function shareContent(
    title,
    url = window.location.href
) {

    /*
     * Use native share sheet where available.
     */

    if (navigator.share) {

        try {

            await navigator.share({
                title,
                url
            });

            return;

        } catch (error) {

            /*
             * User closing the share dialog
             * should not be treated as an error.
             */

            if (
                error.name === "AbortError"
            ) {

                return;

            }

        }

    }


    /*
     * Desktop fallback.
     */

    try {

        await navigator.clipboard.writeText(
            url
        );


        showToast(
            "Link copied to clipboard."
        );

    } catch (error) {

        showToast(
            "Unable to copy the link."
        );

    }

}


/* =========================================================
   CHARACTER HERO SLIDER (5 CATEGORIES)
   Comics, Cosplay, Badminton, Anime, TV Shows
   ========================================================= */

function initializeCharacterHeroSlider() {

    const sliderSection = document.getElementById("characterHeroSlider");
    if (!sliderSection) {
        return;
    }

    const slides = sliderSection.querySelectorAll(".character-slide");
    const tabs = sliderSection.querySelectorAll(".slider-tab");
    const prevBtn = document.getElementById("heroPrevBtn");
    const nextBtn = document.getElementById("heroNextBtn");
    const ctaContainer = document.getElementById("sliderCompletionCta");

    if (!slides.length) {
        return;
    }

    const totalSlides = slides.length;
    let currentSlide = 0;
    let slideTimer = null;
    let cycleCount = 0;
    const SLIDE_DURATION = 4200; // ms per slide
    const visitedSlides = new Set([0]);

    function showSlide(index, userTriggered = false) {
        // Normalize index
        if (index >= totalSlides) {
            index = 0;
            cycleCount++;
        } else if (index < 0) {
            index = totalSlides - 1;
        }

        currentSlide = index;
        visitedSlides.add(index);

        // Update slides
        slides.forEach((slide, i) => {
            if (i === currentSlide) {
                slide.classList.add("active");
                slide.setAttribute("aria-hidden", "false");
            } else {
                slide.classList.remove("active");
                slide.setAttribute("aria-hidden", "true");
            }
        });

        // Update tabs & progress bars
        tabs.forEach((tab, i) => {
            const progressBar = tab.querySelector(".tab-progress-bar");
            if (i === currentSlide) {
                tab.classList.add("active");
                tab.setAttribute("aria-selected", "true");
                if (progressBar) {
                    progressBar.style.animation = "none";
                    void progressBar.offsetWidth; // Trigger reflow
                    progressBar.style.animation = `tabProgressFill ${SLIDE_DURATION}ms linear forwards`;
                }
            } else {
                tab.classList.remove("active");
                tab.setAttribute("aria-selected", "false");
                if (progressBar) {
                    progressBar.style.animation = "none";
                    progressBar.style.width = i < currentSlide ? "100%" : "0%";
                }
            }
        });

        // Check if the complete 5-slide sequence has finished
        if (cycleCount >= 1 || visitedSlides.size >= totalSlides) {
            revealCompletionCTA();
        }
    }

    function revealCompletionCTA() {
        if (ctaContainer && !ctaContainer.classList.contains("revealed")) {
            ctaContainer.classList.add("revealed");
            ctaContainer.setAttribute("aria-hidden", "false");
        }
    }

    function startAutoSlide() {
        stopAutoSlide();
        slideTimer = setInterval(() => {
            showSlide(currentSlide + 1);
        }, SLIDE_DURATION);
    }

    function stopAutoSlide() {
        if (slideTimer) {
            clearInterval(slideTimer);
            slideTimer = null;
        }
    }

    // Tabs click handler
    tabs.forEach((tab) => {
        tab.addEventListener("click", () => {
            const targetIndex = parseInt(tab.dataset.slideTo, 10);
            if (!isNaN(targetIndex)) {
                showSlide(targetIndex, true);
                startAutoSlide(); // Reset timer
            }
        });
    });

    // Arrow buttons
    if (prevBtn) {
        prevBtn.addEventListener("click", () => {
            showSlide(currentSlide - 1, true);
            startAutoSlide();
        });
    }

    if (nextBtn) {
        nextBtn.addEventListener("click", () => {
            showSlide(currentSlide + 1, true);
            startAutoSlide();
        });
    }

    // Pause on hover
    sliderSection.addEventListener("mouseenter", stopAutoSlide);
    sliderSection.addEventListener("mouseleave", startAutoSlide);

    // Touch swipe support
    let touchStartX = 0;
    let touchEndX = 0;

    sliderSection.addEventListener("touchstart", (e) => {
        touchStartX = e.changedTouches[0].screenX;
    }, { passive: true });

    sliderSection.addEventListener("touchend", (e) => {
        touchEndX = e.changedTouches[0].screenX;
        handleSwipe();
    }, { passive: true });

    function handleSwipe() {
        const swipeDiff = touchEndX - touchStartX;
        if (Math.abs(swipeDiff) > 45) {
            if (swipeDiff < 0) {
                showSlide(currentSlide + 1, true);
            } else {
                showSlide(currentSlide - 1, true);
            }
            startAutoSlide();
        }
    }

    // Keyboard navigation (Left / Right arrows when slider in viewport)
    document.addEventListener("keydown", (e) => {
        const rect = sliderSection.getBoundingClientRect();
        const isInView = rect.top < window.innerHeight && rect.bottom > 0;
        if (isInView && (e.key === "ArrowLeft" || e.key === "ArrowRight")) {
            if (e.key === "ArrowLeft") {
                showSlide(currentSlide - 1, true);
            } else {
                showSlide(currentSlide + 1, true);
            }
            startAutoSlide();
        }
    });

    // Initialize first slide and start auto rotation
    showSlide(0);
    startAutoSlide();
}


/* =========================================================
   MODEL COLOR SELECTOR (Green, Blue, Red, Pink)
   ========================================================= */

function initializeModelColorSelector() {
    const colorButtons = document.querySelectorAll("[data-model-color]");
    if (!colorButtons.length) return;

    colorButtons.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();
            const selectedColor = btn.dataset.modelColor;
            if (!selectedColor) return;

            const characterColumn = document.getElementById("authCharacterColumn") ||
                                    btn.closest(".auth-split-container")?.querySelector(".auth-character-column") ||
                                    document.querySelector(".auth-character-column");

            if (characterColumn) {
                characterColumn.dataset.activeColor = selectedColor;
            }

            const siblingButtons = btn.closest(".model-color-selector")
                ? btn.closest(".model-color-selector").querySelectorAll("[data-model-color]")
                : colorButtons;

            siblingButtons.forEach(b => b.classList.remove("active"));
            btn.classList.add("active");
        });
    });
}


/* =========================================================
   EXPOSE SHARED HELPERS
   Other FanHub+ pages can use them.
   ========================================================= */

window.FanHub = {

    showToast,

    getBookmarks,

    filterCardsBySearch,

    filterCardsByCategory,

    getQueryParameter,

    validateRequiredFields,

    isValidEmail,

    saveDemoUser,

    getDemoUser,

    logoutDemoUser,

    getFavoriteFandoms,

    saveFavoriteFandoms,

    saveContentNote,

    getContentNotes,

    shareContent

};