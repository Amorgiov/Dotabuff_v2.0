document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.getElementById('hero-search');
    const heroCards = document.querySelectorAll('.hero-card');

    searchInput.addEventListener('input', function (e) {
        const query = e.target.value.toLowerCase();
        heroCards.forEach(card => {
            const heroName = card.dataset.name.toLowerCase();
            card.style.display = heroName.includes(query) ? 'block' : 'none';

        });
    });
});
