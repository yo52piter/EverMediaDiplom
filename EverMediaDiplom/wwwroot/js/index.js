document.addEventListener('DOMContentLoaded', function() {
    // Код для меню
    const openMenubarBtn = document.querySelector(".open-menubar-btn");
    const closeMenubarBtn = document.querySelector(".close-menubar-btn");
    const menuBar = document.querySelector(".menubar");

    if (openMenubarBtn && closeMenubarBtn && menuBar) {
        openMenubarBtn.addEventListener('click', function() {
            menuBar.style.top = "0";
        });

        closeMenubarBtn.addEventListener('click', function() {
            menuBar.style.top = "100%";
        });
    }

    // Исправленный код для галереи
    const galleryMenubarLi = document.querySelectorAll(".gallery-menubar ul li");
    const galleryImgs = document.querySelectorAll(".gallery-imgs-container img");

    if (galleryMenubarLi.length && galleryImgs.length) {
        galleryMenubarLi.forEach(li => {
            li.addEventListener('click', function(e) {
                // Удаляем класс active у всех кнопок
                galleryMenubarLi.forEach(item => item.classList.remove("active"));
                // Добавляем класс active к текущей кнопке
                e.currentTarget.classList.add("active");

                const filter = e.currentTarget.textContent.trim().toLowerCase();
                
                // Фильтрация изображений
                galleryImgs.forEach(img => {
                    const imgCategory = img.dataset.image.toLowerCase();
                    img.classList.remove('animate');
                    
                    if (filter === "все" || filter === "all" || imgCategory === filter) {
                        img.style.display = "block";
                        img.classList.add("animate");
                   
                    } else {
                        img.style.display = "none";
                    }
                });
            });
        });
    }
});
// Инициализация карточек сотрудников
document.querySelectorAll('.member-card').forEach(card => {
    card.addEventListener('mouseenter', function() {
        this.querySelector('.member-info').style.bottom = '0';
        this.querySelector('img').style.transform = 'scale(1.1)';
    });
    
    card.addEventListener('mouseleave', function() {
        this.querySelector('.member-info').style.bottom = '-100%';
        this.querySelector('img').style.transform = 'scale(1)';
    });
});