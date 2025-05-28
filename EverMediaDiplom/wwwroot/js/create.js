

document.addEventListener('DOMContentLoaded', function () {
    const dateInput = document.querySelector('input[type="datetime-local"]');
    const now = new Date();
    const minDate = new Date(now.setDate(now.getDate() + 1));

    // Установка минимального времени (завтра 8:00)
    const minDateString = minDate.toISOString().slice(0, 16);
    dateInput.min = minDateString;

    // Установка максимального времени (3 месяца от текущей даты 20:00)
    const maxDate = new Date();
    maxDate.setMonth(maxDate.getMonth() + 3);
    const maxDateString = maxDate.toISOString().slice(0, 11) + '20:00';
    dateInput.max = maxDateString;
});
