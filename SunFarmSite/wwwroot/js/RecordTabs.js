const openTab = (tabNavButton) => {
    const tabPage = document.getElementById(tabNavButton.innerHTML);

    if (tabPage) {
        // Get all the elements with class="tabcontent" and hide them
        const tabcontent = document.querySelectorAll('[class~="tab-content"]');
        for (let i = 0, l = tabcontent.length; i < l; i++) {
            tabcontent[i].style.display = 'none';
        }

        // Get all elements with class="tablinks" and remove the class "active"
        const tablinks = document.querySelectorAll('[class~="tab-links"]');
        for (let i = 0, l = tablinks.length; i < l; i++) {
            tablinks[i].className = tablinks[i].className.replace(' active', '');
        }

        // Show the current tab, and add an "active" class to the button that opened the tab.
        tabPage.style.display = 'block';
        tabNavButton.className += ' active';
    }
}