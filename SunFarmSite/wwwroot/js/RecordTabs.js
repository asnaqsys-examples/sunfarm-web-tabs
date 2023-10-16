const openTab = (tabNavButton) => {
    if (!tabNavButton) {
        return; // ignore.
    }
    openTabByName(tabNavButton.innerHTML);
}

const openTabByName = (reqPageName) => {
    const tabPage = document.getElementById(reqPageName);

    if (tabPage) {
        // Get all the elements with class="tab-content" and hide them.
        const tabcontent = document.querySelectorAll('[class~="tab-content"]');
        for (let i = 0, l = tabcontent.length; i < l; i++) {
            tabcontent[i].style.display = 'none';
        }

        // Get all elements with class="tab-links" and remove the class "active"
        const tablinks = document.querySelectorAll('[class~="tab-links"]');
        let tabNavButton = null;
        for (let i = 0, l = tablinks.length; i < l; i++) {
            tablinks[i].className = tablinks[i].className.replace(' active', '');
            if (tablinks[i].innerHTML === reqPageName) {
                tabNavButton = tablinks[i]; // Save the button that has the requested name.
            }
        }

        // Show the current tab, and add an "active" class to the button that opened the tab.
        tabPage.style.display = 'block';
        if (tabNavButton) {
            tabNavButton.className += ' active';
        }

        sessionStorage.setItem(`SunFarmLastTabForPage:${window.location.pathname}`, reqPageName);
    }
}

const getLastTabNameFromSessionStorage = () => {
    return sessionStorage.getItem(`SunFarmLastTabForPage:${window.location.pathname}`);
}