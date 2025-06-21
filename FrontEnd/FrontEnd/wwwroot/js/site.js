// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", () => {
    const introText = document.getElementById('introText');
    const navbar = document.querySelector('.navbar');

    let lastScrollY = window.scrollY;

    window.addEventListener('scroll', () => {
        const scrollY = window.scrollY;
        const maxScroll = 300;

        const progress = Math.min(scrollY / maxScroll, 1);
        const scale = 1 + progress * 3;
        const opacity = 1 - progress;

        introText.style.transform = `translate(-50%, -50%) scale(${scale})`;
        introText.style.opacity = opacity;

        if (progress >= 1) {
            navbar.classList.add('visible');
        } else {
            navbar.classList.remove('visible');
        }

        if (scrollY < lastScrollY) {
            // Scrolling up → hide navbar
            navbar.classList.add('hide');
        } else {
            // Scrolling down → show navbar
            if (progress >= 1) navbar.classList.remove('hide');
        }


        lastScrollY = scrollY;
    });
});
