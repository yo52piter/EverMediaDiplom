document.addEventListener('DOMContentLoaded', function () {
    // Initialize tab functionality
    initTabs();

    // Initialize photo upload button
    initPhotoUpload();

    // Initialize other profile interactions
    initProfileInteractions();
});

/**
 * Initialize tab switching functionality
 */
function initTabs() {
    const tabBtns = document.querySelectorAll('.tab-btn');

    tabBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            // Remove active class from all buttons and tabs
            document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
            document.querySelectorAll('.tab-content').forEach(c => c.classList.remove('active'));

            // Add active class to current button and corresponding tab
            this.classList.add('active');
            const tabId = this.getAttribute('data-tab');
            document.getElementById(tabId).classList.add('active');
        });
    });
}

/**
 * Initialize photo upload button
 */
function initPhotoUpload() {
    const uploadBtn = document.getElementById('add-photo-btn');
    const avatarEditBtn = document.querySelector('.profile-avatar .common-btn');

    if (uploadBtn) {
        uploadBtn.addEventListener('click', function (e) {
            e.preventDefault();
            // TODO: Implement photo upload logic
            console.log('Add photo button clicked');
        });
    }

    if (avatarEditBtn) {
        avatarEditBtn.addEventListener('click', function (e) {
            e.preventDefault();
            // TODO: Implement avatar change logic
            console.log('Change avatar button clicked');
        });
    }
}

/**
 * Initialize other profile interactions
 */
function initProfileInteractions() {
    // Initialize favorite buttons
    document.querySelectorAll('.favorite-btn').forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            // TODO: Implement favorite logic
            console.log('Favorite button clicked');
        });
    });

    // Initialize delete buttons
    document.querySelectorAll('.delete-btn').forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            // TODO: Implement delete logic
            console.log('Delete button clicked');
        });
    });

    // Initialize form validation
    const settingsForm = document.querySelector('.settings-form');
    if (settingsForm) {
        settingsForm.addEventListener('submit', function (e) {
            // Add form validation if needed
            console.log('Settings form submitted');
        });
    }
}
function initTabs() {
    const tabBtns = document.querySelectorAll('.tab-btn');
    const tabsContainer = document.querySelector('.tabs-container');

    tabBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            // Remove active class from all buttons
            document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));

            // Add active class to current button
            this.classList.add('active');

            // Hide all tabs
            document.querySelectorAll('.tab-content').forEach(c => {
                c.classList.remove('active');
            });

            // Show selected tab
            const tabId = this.getAttribute('data-tab');
            const activeTab = document.getElementById(tabId);
            activeTab.classList.add('active');

            // Update container height
            tabsContainer.style.height = activeTab.scrollHeight + 'px';
        });
    });

    // Set initial height
    const activeTab = document.querySelector('.tab-content.active');
    if (activeTab) {
        tabsContainer.style.height = activeTab.scrollHeight + 'px';
    }
    document.getElementById('toggle-edit-form').addEventListener('click', function () {
        var form = document.getElementById('edit-profile-form');
        form.style.display = form.style.display === 'none' ? 'block' : 'none';
        this.textContent = form.style.display === 'none' ?
            'Редактировать профиль' : 'Скрыть форму';
    });
}