/* =========================================================
   FANHUB+ - AUTHENTICATION
   Login / Register / Forgot Password
   Frontend Demonstration
   ========================================================= */

"use strict";

document.addEventListener("DOMContentLoaded", () => {

    /* =====================================================
       STORAGE KEYS
       ===================================================== */

    const USER_KEY =
        "fanhub-user";

    const DEMO_USER_KEY =
        "fanhub-demo-user";

    const FAVORITES_KEY =
        "fanhub-favorite-fandoms";


    /* =====================================================
       COMMON HELPERS
       ===================================================== */

    function getElement(id) {

        return document.getElementById(id);

    }


    function normalize(value) {

        return String(value || "")
            .trim();

    }


    function isValidEmail(email) {

        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/
            .test(email);

    }


    function safeSave(key, value) {

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


    function safeRead(key, fallback = null) {

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


    function setError(
        input,
        errorElement,
        message
    ) {

        if (errorElement) {

            errorElement.textContent =
                message || "";

        }


        if (input) {

            const wrapper =
                input.closest(
                    ".auth-input"
                );


            if (wrapper) {

                wrapper.classList.toggle(
                    "invalid",
                    Boolean(message)
                );

            }

        }

    }


    function clearError(
        input,
        errorElement
    ) {

        setError(
            input,
            errorElement,
            ""
        );

    }


    function showAuthToast(message) {

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
            getElement("fanHubToast");

        const messageElement =
            getElement("toastMessage");


        if (messageElement) {

            messageElement.textContent =
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
       PASSWORD VISIBILITY
       ===================================================== */

    const passwordButtons =
        document.querySelectorAll(
            "[data-password-target]"
        );


    passwordButtons.forEach(
        (button) => {

            button.addEventListener(
                "click",
                () => {

                    const targetId =
                        button.dataset
                            .passwordTarget;


                    const input =
                        getElement(targetId);


                    if (!input) {
                        return;
                    }


                    const showing =
                        input.type === "text";


                    input.type =
                        showing
                            ? "password"
                            : "text";


                    const icon =
                        button.querySelector(
                            "i"
                        );


                    if (icon) {

                        icon.className =
                            showing
                                ? "bi bi-eye"
                                : "bi bi-eye-slash";

                    }


                    button.setAttribute(
                        "aria-label",
                        showing
                            ? "Show password"
                            : "Hide password"
                    );

                }
            );

        }
    );


    /* =====================================================
       LOGIN
       ===================================================== */

    const loginForm =
        getElement("loginForm");


    if (loginForm) {

        const emailInput =
            getElement("loginEmail");

        const passwordInput =
            getElement("loginPassword");

        const rememberMe =
            getElement("rememberMe");

        const emailError =
            getElement("loginEmailError");

        const passwordError =
            getElement("loginPasswordError");

        const submitButton =
            getElement("loginSubmitButton");


        emailInput?.addEventListener(
            "input",
            () => {

                clearError(
                    emailInput,
                    emailError
                );

            }
        );


        passwordInput?.addEventListener(
            "input",
            () => {

                clearError(
                    passwordInput,
                    passwordError
                );

            }
        );


        loginForm.addEventListener(
            "submit",
            (event) => {

                event.preventDefault();


                const email =
                    normalize(
                        emailInput?.value
                    );

                const password =
                    passwordInput?.value || "";


                let valid = true;


                if (!email) {

                    setError(
                        emailInput,
                        emailError,
                        "Email address is required."
                    );

                    valid = false;

                } else if (
                    !isValidEmail(email)
                ) {

                    setError(
                        emailInput,
                        emailError,
                        "Enter a valid email address."
                    );

                    valid = false;

                } else {

                    clearError(
                        emailInput,
                        emailError
                    );

                }


                if (!password) {

                    setError(
                        passwordInput,
                        passwordError,
                        "Password is required."
                    );

                    valid = false;

                } else if (
                    password.length < 6
                ) {

                    setError(
                        passwordInput,
                        passwordError,
                        "Password must contain at least 6 characters."
                    );

                    valid = false;

                } else {

                    clearError(
                        passwordInput,
                        passwordError
                    );

                }


                if (!valid) {
                    return;
                }


                /*
                 * Frontend-only login demonstration.
                 * Real credential checking belongs
                 * to the backend.
                 */

                const savedUser =
                    safeRead(
                        USER_KEY,
                        null
                    );


                const displayName =
                    savedUser?.email
                        ?.toLowerCase() ===
                        email.toLowerCase()
                        ? savedUser.name
                        : "FanHub User";


                const sessionUser = {

                    name:
                        displayName ||
                        "FanHub User",

                    email: email,

                    loggedIn: true,

                    remember:
                        Boolean(
                            rememberMe?.checked
                        )

                };


                safeSave(
                    DEMO_USER_KEY,
                    sessionUser
                );


                if (submitButton) {

                    submitButton.disabled =
                        true;


                    submitButton.innerHTML =
                        `
                        <span>
                            Signing In...
                        </span>

                        <span
                            class="spinner-border spinner-border-sm"
                            aria-hidden="true">
                        </span>
                        `;

                }


                showAuthToast(
                    "Login successful."
                );


                setTimeout(
                    () => {

                        window.location.href =
                            "dashboard.html";

                    },
                    700
                );

            }
        );

    }


    /* =====================================================
       REGISTER
       ===================================================== */

    const registerForm =
        getElement("registerForm");


    if (registerForm) {

        const nameInput =
            getElement("registerName");

        const emailInput =
            getElement("registerEmail");

        const passwordInput =
            getElement("registerPassword");

        const confirmInput =
            getElement("confirmPassword");

        const termsInput =
            getElement("registerTerms");

        const avatarInput =
            getElement("registerAvatar");

        const avatarPreview =
            getElement(
                "registerAvatarPreview"
            );

        const submitButton =
            getElement(
                "registerSubmitButton"
            );


        const nameError =
            getElement(
                "registerNameError"
            );

        const emailError =
            getElement(
                "registerEmailError"
            );

        const passwordError =
            getElement(
                "registerPasswordError"
            );

        const confirmError =
            getElement(
                "confirmPasswordError"
            );

        const termsError =
            getElement(
                "registerTermsError"
            );


        let selectedFandoms = [];

        let avatarData = "";


        const fandomButtons =
            document.querySelectorAll(
                ".register-fandom-option[data-fandom]"
            );


        /* -----------------------------------------
           FANDOM SELECTION
        ------------------------------------------ */

        fandomButtons.forEach(
            (button) => {

                button.addEventListener(
                    "click",
                    () => {

                        const fandom =
                            button.dataset.fandom;


                        if (!fandom) {
                            return;
                        }


                        const alreadySelected =
                            selectedFandoms.includes(
                                fandom
                            );


                        if (alreadySelected) {

                            selectedFandoms =
                                selectedFandoms.filter(
                                    (item) =>
                                        item !== fandom
                                );

                        } else {

                            selectedFandoms.push(
                                fandom
                            );

                        }


                        button.classList.toggle(
                            "selected",
                            !alreadySelected
                        );


                        button.setAttribute(
                            "aria-pressed",
                            String(
                                !alreadySelected
                            )
                        );

                    }
                );

            }
        );


        /* -----------------------------------------
           AVATAR PREVIEW
        ------------------------------------------ */

        avatarInput?.addEventListener(
            "change",
            () => {

                const file =
                    avatarInput.files?.[0];


                if (!file) {
                    return;
                }


                if (
                    !file.type.startsWith(
                        "image/"
                    )
                ) {

                    showAuthToast(
                        "Please select an image file."
                    );

                    avatarInput.value = "";

                    return;

                }


                /*
                 * Keep demo avatar reasonably small.
                 */

                if (
                    file.size >
                    2 * 1024 * 1024
                ) {

                    showAuthToast(
                        "Avatar must be under 2 MB."
                    );

                    avatarInput.value = "";

                    return;

                }


                const reader =
                    new FileReader();


                reader.onload =
                    (readerEvent) => {

                        avatarData =
                            readerEvent
                                .target
                                ?.result || "";


                        if (
                            avatarPreview &&
                            avatarData
                        ) {

                            avatarPreview.innerHTML =
                                "";


                            const image =
                                document.createElement(
                                    "img"
                                );


                            image.src =
                                avatarData;


                            image.alt =
                                "Profile avatar preview";


                            avatarPreview.appendChild(
                                image
                            );

                        }

                    };


                reader.readAsDataURL(
                    file
                );

            }
        );


        /* -----------------------------------------
           CLEAR ERRORS
        ------------------------------------------ */

        nameInput?.addEventListener(
            "input",
            () => {

                clearError(
                    nameInput,
                    nameError
                );

            }
        );


        emailInput?.addEventListener(
            "input",
            () => {

                clearError(
                    emailInput,
                    emailError
                );

            }
        );


        passwordInput?.addEventListener(
            "input",
            () => {

                clearError(
                    passwordInput,
                    passwordError
                );


                clearError(
                    confirmInput,
                    confirmError
                );

            }
        );


        confirmInput?.addEventListener(
            "input",
            () => {

                clearError(
                    confirmInput,
                    confirmError
                );

            }
        );


        termsInput?.addEventListener(
            "change",
            () => {

                if (
                    termsInput.checked
                ) {

                    if (termsError) {

                        termsError.textContent =
                            "";

                    }

                }

            }
        );


        /* -----------------------------------------
           REGISTER SUBMIT
        ------------------------------------------ */

        registerForm.addEventListener(
            "submit",
            (event) => {

                event.preventDefault();


                const name =
                    normalize(
                        nameInput?.value
                    );

                const email =
                    normalize(
                        emailInput?.value
                    );

                const password =
                    passwordInput?.value || "";

                const confirmPassword =
                    confirmInput?.value || "";


                let valid = true;


                if (
                    name.length < 2
                ) {

                    setError(
                        nameInput,
                        nameError,
                        "Enter your display name."
                    );

                    valid = false;

                } else {

                    clearError(
                        nameInput,
                        nameError
                    );

                }


                if (!email) {

                    setError(
                        emailInput,
                        emailError,
                        "Email address is required."
                    );

                    valid = false;

                } else if (
                    !isValidEmail(email)
                ) {

                    setError(
                        emailInput,
                        emailError,
                        "Enter a valid email address."
                    );

                    valid = false;

                } else {

                    clearError(
                        emailInput,
                        emailError
                    );

                }


                if (
                    password.length < 6
                ) {

                    setError(
                        passwordInput,
                        passwordError,
                        "Use at least 6 characters."
                    );

                    valid = false;

                } else {

                    clearError(
                        passwordInput,
                        passwordError
                    );

                }


                if (
                    confirmPassword !==
                    password
                ) {

                    setError(
                        confirmInput,
                        confirmError,
                        "Passwords do not match."
                    );

                    valid = false;

                } else if (
                    !confirmPassword
                ) {

                    setError(
                        confirmInput,
                        confirmError,
                        "Confirm your password."
                    );

                    valid = false;

                } else {

                    clearError(
                        confirmInput,
                        confirmError
                    );

                }


                if (
                    !termsInput?.checked
                ) {

                    if (termsError) {

                        termsError.textContent =
                            "Please accept the community terms.";

                    }

                    valid = false;

                } else if (termsError) {

                    termsError.textContent =
                        "";

                }


                if (!valid) {
                    return;
                }


                /*
                 * Frontend demonstration account.
                 *
                 * Password is intentionally NOT stored
                 * in localStorage.
                 */

                const user = {

                    name: name,

                    email: email,

                    avatar: avatarData,

                    favoriteFandoms:
                        selectedFandoms,

                    createdAt:
                        new Date()
                            .toISOString()

                };


                safeSave(
                    USER_KEY,
                    user
                );


                safeSave(
                    FAVORITES_KEY,
                    selectedFandoms
                );


                if (submitButton) {

                    submitButton.disabled =
                        true;


                    submitButton.innerHTML =
                        `
                        <span>
                            Creating Account...
                        </span>

                        <span
                            class="spinner-border spinner-border-sm"
                            aria-hidden="true">
                        </span>
                        `;

                }


                showAuthToast(
                    "Account created successfully."
                );


                setTimeout(
                    () => {

                        window.location.href =
                            "login.html";

                    },
                    850
                );

            }
        );

    }


    /* =====================================================
       FORGOT PASSWORD
       ===================================================== */

    const forgotForm =
        getElement(
            "forgotPasswordForm"
        );


    if (forgotForm) {

        const emailInput =
            getElement("forgotEmail");

        const emailError =
            getElement(
                "forgotEmailError"
            );


        const emailStep =
            getElement(
                "forgotStepEmail"
            );

        const codeStep =
            getElement(
                "forgotStepCode"
            );

        const passwordStep =
            getElement(
                "forgotStepNewPassword"
            );

        const successStep =
            getElement(
                "forgotStepSuccess"
            );


        const resetEmailDisplay =
            getElement(
                "resetEmailDisplay"
            );


        const codeForm =
            getElement(
                "resetCodeForm"
            );

        const codeInput =
            getElement(
                "resetCode"
            );

        const codeError =
            getElement(
                "resetCodeError"
            );


        const newPasswordForm =
            getElement(
                "newPasswordForm"
            );

        const newPassword =
            getElement(
                "newPassword"
            );

        const confirmNewPassword =
            getElement(
                "confirmNewPassword"
            );

        const newPasswordError =
            getElement(
                "newPasswordError"
            );

        const confirmNewPasswordError =
            getElement(
                "confirmNewPasswordError"
            );


        const changeResetEmail =
            getElement(
                "changeResetEmail"
            );


        let resetEmail = "";


        /* -----------------------------------------
           STEP SWITCHER
        ------------------------------------------ */

        function showResetStep(step) {

            [
                emailStep,
                codeStep,
                passwordStep,
                successStep
            ].forEach(
                (item) => {

                    item?.classList.remove(
                        "active"
                    );

                }
            );


            step?.classList.add(
                "active"
            );

        }


        /* -----------------------------------------
           STEP 1 - EMAIL
        ------------------------------------------ */

        forgotForm.addEventListener(
            "submit",
            (event) => {

                event.preventDefault();


                const email =
                    normalize(
                        emailInput?.value
                    );


                if (!email) {

                    setError(
                        emailInput,
                        emailError,
                        "Email address is required."
                    );

                    return;

                }


                if (
                    !isValidEmail(email)
                ) {

                    setError(
                        emailInput,
                        emailError,
                        "Enter a valid email address."
                    );

                    return;

                }


                clearError(
                    emailInput,
                    emailError
                );


                resetEmail = email;


                if (
                    resetEmailDisplay
                ) {

                    resetEmailDisplay.textContent =
                        email;

                }


                /*
                 * Frontend demonstration only.
                 * No real email is sent.
                 */

                showResetStep(
                    codeStep
                );


                showAuthToast(
                    "Demo reset code generated."
                );

            }
        );


        emailInput?.addEventListener(
            "input",
            () => {

                clearError(
                    emailInput,
                    emailError
                );

            }
        );


        /* -----------------------------------------
           STEP 2 - CODE
        ------------------------------------------ */

        codeInput?.addEventListener(
            "input",
            () => {

                /*
                 * Keep digits only.
                 */

                codeInput.value =
                    codeInput.value
                        .replace(
                            /\D/g,
                            ""
                        )
                        .slice(
                            0,
                            6
                        );


                clearError(
                    codeInput,
                    codeError
                );

            }
        );


        codeForm?.addEventListener(
            "submit",
            (event) => {

                event.preventDefault();


                const code =
                    normalize(
                        codeInput?.value
                    );


                if (
                    code.length !== 6
                ) {

                    setError(
                        codeInput,
                        codeError,
                        "Enter the 6-digit reset code."
                    );

                    return;

                }


                /*
                 * Demo verification code.
                 */

                if (
                    code !== "123456"
                ) {

                    setError(
                        codeInput,
                        codeError,
                        "Invalid demo code. Use 123456."
                    );

                    return;

                }


                clearError(
                    codeInput,
                    codeError
                );


                showResetStep(
                    passwordStep
                );


                showAuthToast(
                    "Code verified."
                );

            }
        );


        /* -----------------------------------------
           CHANGE EMAIL
        ------------------------------------------ */

        changeResetEmail?.addEventListener(
            "click",
            () => {

                resetEmail = "";


                if (codeInput) {

                    codeInput.value = "";

                }


                clearError(
                    codeInput,
                    codeError
                );


                showResetStep(
                    emailStep
                );


                emailInput?.focus();

            }
        );


        /* -----------------------------------------
           STEP 3 - NEW PASSWORD
        ------------------------------------------ */

        newPassword?.addEventListener(
            "input",
            () => {

                clearError(
                    newPassword,
                    newPasswordError
                );

            }
        );


        confirmNewPassword
            ?.addEventListener(
                "input",
                () => {

                    clearError(
                        confirmNewPassword,
                        confirmNewPasswordError
                    );

                }
            );


        newPasswordForm?.addEventListener(
            "submit",
            (event) => {

                event.preventDefault();


                const password =
                    newPassword?.value || "";

                const confirmation =
                    confirmNewPassword?.value ||
                    "";


                let valid = true;


                if (
                    password.length < 6
                ) {

                    setError(
                        newPassword,
                        newPasswordError,
                        "Use at least 6 characters."
                    );

                    valid = false;

                } else {

                    clearError(
                        newPassword,
                        newPasswordError
                    );

                }


                if (
                    !confirmation
                ) {

                    setError(
                        confirmNewPassword,
                        confirmNewPasswordError,
                        "Confirm your new password."
                    );

                    valid = false;

                } else if (
                    confirmation !==
                    password
                ) {

                    setError(
                        confirmNewPassword,
                        confirmNewPasswordError,
                        "Passwords do not match."
                    );

                    valid = false;

                } else {

                    clearError(
                        confirmNewPassword,
                        confirmNewPasswordError
                    );

                }


                if (!valid) {
                    return;
                }


                /*
                 * Frontend demo:
                 * Password itself is not saved.
                 */

                showResetStep(
                    successStep
                );


                showAuthToast(
                    "Password reset completed."
                );

            }
        );

    }


    /* =====================================================
       MODEL COLOR SELECTOR (Green, Blue, Red, Pink)
       ===================================================== */

    function initializeModelColorSelector() {
        const colorButtons = document.querySelectorAll("[data-model-color]");
        if (!colorButtons.length) return;

        colorButtons.forEach((btn) => {
            btn.addEventListener("click", (e) => {
                e.preventDefault();
                const selectedColor = btn.dataset.modelColor;
                if (!selectedColor) return;

                // Find the character column container
                const characterColumn = document.getElementById("authCharacterColumn") ||
                                        btn.closest(".auth-split-container")?.querySelector(".auth-character-column") ||
                                        document.querySelector(".auth-character-column");

                if (characterColumn) {
                    characterColumn.dataset.activeColor = selectedColor;
                }

                // Update active button state
                const siblingButtons = btn.closest(".model-color-selector")
                    ? btn.closest(".model-color-selector").querySelectorAll("[data-model-color]")
                    : colorButtons;

                siblingButtons.forEach(b => b.classList.remove("active"));
                btn.classList.add("active");
            });
        });
    }

    initializeModelColorSelector();


    /* =====================================================
       ESCAPE KEY
       ===================================================== */

    document.addEventListener(
        "keydown",
        (event) => {

            if (
                event.key === "Escape"
            ) {

                const activeElement =
                    document.activeElement;


                if (
                    activeElement instanceof
                    HTMLElement
                ) {

                    activeElement.blur();

                }

            }

        }
    );

});