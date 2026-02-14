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

    resetScrollPosition: function () {
        window.scrollTo(0, 1);
        window.scrollTo(0, 0);
    },

    hidePageLoadingIndicator: () => {
        const loadingElement = document.getElementById('page-loading-indicator');
        if (loadingElement) {
            loadingElement.style.display = 'none';
        }
    },

    showPageLoadingIndicator: () => {
        const loadingElement = document.getElementById('page-loading-indicator');
        if (loadingElement) {
            loadingElement.style.display = 'flex';
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

    scrollTo: (elem, position, smooth) => {
        let behavior = 'instant';
        if (smooth === true) {
            behavior = 'smooth'
        }
        elem.scrollTo({top: position, behavior: behavior});
    },

    scrollToBottom: (elem, smooth) => {
        if (!elem) return;
        window.Fog.Webapp.Common.scrollTo(elem, elem.scrollHeight, smooth);
    },

    saveFile: (filename, contentType, content) => {
        const blob = new Blob([content], {type: contentType});
        const link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = filename;
        link.click();
        URL.revokeObjectURL(link.href);
    }
};

window.Fog.Webapp.Analytics = {
    gtag: window.gtag || function () {
        dataLayer.push(arguments);
    },
}

window.Fog.Webapp.AdSense = {
    initializeAd:  (adSlotId) => {
        try {
            // Check if adsbygoogle is loaded
            if (typeof adsbygoogle === 'undefined') {
                console.error('AdSense script not loaded yet');
                return false;
            }

            // Find the ad container
            const adContainer = document.getElementById(adSlotId);
            if (!adContainer) {
                console.error('Ad container not found:', adSlotId);
                return false;
            }

            // Find the ins element inside
            const insElement = adContainer.querySelector('ins.adsbygoogle');
            if (!insElement) {
                console.error('Ad ins element not found in container:', adSlotId);
                return false;
            }

            // Check if already initialized (AdSense adds data-adsbygoogle-status when processed)
            if (insElement.dataset.adsbygoogleStatus) {
                console.log('Ad already initialized:', adSlotId);
                return true;
            }

            // Push the ad to AdSense queue
            (adsbygoogle = window.adsbygoogle || []).push({});
            console.log('Ad initialized:', adSlotId);
            return true;

        } catch (e) {
            console.error('Error initializing ad:', e);
            return false;
        }
    },

    clearAd: (adSlotId) => {
        try {
            const adContainer = document.getElementById(adSlotId);
            if (!adContainer) {
                console.log('Ad container already removed:', adSlotId);
                return;
            }

            // Find the ins element
            const insElement = adContainer.querySelector('ins.adsbygoogle');
            if (insElement) {
                // Remove the status to allow re-initialization
                delete insElement.dataset.adsbygoogleStatus;

                // Clear any iframe or ad content that Google injected
                const iframe = insElement.querySelector('iframe');
                if (iframe) {
                    iframe.remove();
                }

                // Clear any other injected content
                while (insElement.firstChild) {
                    insElement.removeChild(insElement.firstChild);
                }
            }

            console.log('Ad cleared:', adSlotId);
        } catch (e) {
            console.error('Error clearing ad:', e);
        }
    },

    // Utility function to check if AdSense script is loaded
    isAdSenseLoaded: ()  => {
        return typeof adsbygoogle !== 'undefined';
    }
}

