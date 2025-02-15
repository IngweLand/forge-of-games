window.Fog = window.Fog || {};
window.Fog.Webapp = window.Fog.Webapp || {};
window.Fog.Webapp.Common = {

    updateContentHeight: function () {
        const navbar = document.querySelector('nav');
        const footer = document.querySelector('footer');

        if (navbar && footer) {
            const navHeight = navbar.offsetHeight;
            const footerHeight = footer.offsetHeight;

            document.documentElement.style.setProperty('--actual-nav-height', `${navHeight}px`);
            document.documentElement.style.setProperty('--actual-footer-height', `${footerHeight}px`);
        }
    },

    getDimensions: function (selector) {
        try {
            let element;

            // Handle different input types
            if (!selector) {
                console.warn('Selector is null or undefined');
                return null;
            }

            // If it's already an element
            if (selector instanceof Element) {
                element = selector;
            }
            // If it's a string (ID or class or any CSS selector)
            else if (typeof selector === 'string') {
                // Try as ID first
                element = document.getElementById(selector.replace('#', ''));

                // If not found, try as any CSS selector
                if (!element) {
                    element = document.querySelector(selector);
                }

                // If still not found, try getting all elements (for class)
                if (!element) {
                    const elements = document.querySelectorAll(selector);
                    if (elements.length > 0) {
                        // Return array of dimensions for all matching elements
                        return Array.from(elements).map(el => this.getMeasurements(el));
                    }
                }
            }

            if (!element) {
                console.warn('No element found for selector:', selector);
                return null;
            }

            return this.getMeasurements(element);

        } catch (error) {
            console.error('Error measuring element:', error, 'Selector:', selector);
            return null;
        }
    },

    getMeasurements: function (element) {
        // For SKCanvasView or similar custom components that wrap canvas
        const canvas = element.querySelector('canvas');
        if (canvas) {
            return {
                width: canvas.clientWidth,
                height: canvas.clientHeight
            };
        }

        // For direct Canvas elements
        if (element instanceof HTMLCanvasElement) {
            return {
                width: element.clientWidth,
                height: element.clientHeight
            };
        }

        // For standard HTML elements
        if (element instanceof HTMLElement) {
            const rect = element.getBoundingClientRect();
            return {
                width: rect.width,
                height: rect.height
            };
        }

        return {
            width: 0,
            height: 0
        };
    },

    calculateAndSetAvailableHeight: function (inputDivId) {
        // Get the input div element
        const inputDiv = document.getElementById(inputDivId);
        if (!inputDiv) return;

        // Get the div's offset from the top of the page
        const divOffset = inputDiv.getBoundingClientRect().top;

        // Calculate available height (viewport height - offset - buffer for padding)
        const buffer = 120; // Adjust this value as needed
        const availableHeight = window.innerHeight - divOffset - buffer;

        // Set the height
        inputDiv.style.height = `${availableHeight}px`;
    },
    
    resetScrollPosition: function (){
        window.scrollTo(0, 1);
        window.scrollTo(0, 0);
    },

    removeLoadingIndicator:  () => {
        const loadingElement = document.getElementById('fog-loading-indicator');
        if (loadingElement) {
            loadingElement.remove();
        }
    },

    copyToClipboard: async (text) => {
        try {
            await navigator.clipboard.writeText(text);
            return true;
        } catch (err) {
            console.error('Failed to copy:', err);
            return false;
        }
    },
    
    isMobile: () => {
        return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
    },
};

