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

    const connectSection = document.querySelector('.connect-section');

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                connectSection.classList.remove('invisible');
                connectSection.classList.add('visible');
            }
        });
    }, {
        threshold: 0.3,
    });

    if (connectSection) {
        observer.observe(connectSection);
    }

    if (entry.isIntersecting) {
        console.log("Connect section is now visible");
        connectSection.classList.remove('invisible');
        connectSection.classList.add('visible');
    }
    
});

document.addEventListener("DOMContentLoaded", () => {
    const container = document.querySelector('.background-anim-container');
    const codeSnippets = [
        `if (user.isLoggedIn) {\n  showDashboard();\n}`,
        `const api = await fetch('/api/flights');`,
        `<div class="frosted-box">Hello World</div>`,
        `public IActionResult Index() => View();`,
        `let progress = scrollY / maxScroll;`
    ];

    const numShapes = 10;
    const shapeTypes = ['circle', 'square', 'triangle', 'pentagon'];
    const verticalSpacing = window.innerHeight / numShapes;

    for (let i = 0; i < numShapes; i++) {
        const shape = document.createElement('div');
        const isCode = Math.random() > 0.7;

        shape.classList.add('shape');

        if (isCode) {
            shape.classList.add('code-snippet');
            shape.textContent = codeSnippets[Math.floor(Math.random() * codeSnippets.length)];
        } else {
            const type = shapeTypes[Math.floor(Math.random() * shapeTypes.length)];
            shape.classList.add(type);

            // Apply size to all shapes including triangle
            const size = 100 + Math.random() * 200;
            shape.style.width = `${size}px`;
            shape.style.height = `${size}px`;
        }

        // Positioning
        const topOffset = i * verticalSpacing + Math.random() * 40;
        shape.style.top = `${topOffset}px`;
        shape.style.left = `${Math.random() * 100}vw`;

        // Animation duration
        const duration = 20 + Math.random() * 40;
        shape.style.animationDuration = `${duration}s`;

        // Random rotation
        shape.style.transform = `rotate(${Math.random() * 360}deg)`;

        container.appendChild(shape);
    }
});

document.addEventListener("DOMContentLoaded", () => {
    const items = document.querySelectorAll('.timeline-item');

    const observer = new IntersectionObserver(entries => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
            }
        });
    }, {
        threshold: 0.2
    });

    items.forEach(item => observer.observe(item));
});

let lastKnownScroll = 0;
let ticking = false;

window.addEventListener('scroll', function () {
    lastKnownScroll = window.scrollY;
    if (!ticking) {
        window.requestAnimationFrame(function () {
            const docHeight = document.documentElement.scrollHeight - window.innerHeight;
            const scrolled = (lastKnownScroll / docHeight) * 100;
            document.getElementById('progress').style.width = `${scrolled}%`;
            ticking = false;
        });
        ticking = true;
    }
});

const showUsernameForm = document.getElementById('showUsernameForm');
const showPasswordForm = document.getElementById('showPasswordForm');
const toggleButtons = document.getElementById('toggleButtons');

const currentUsername = document.getElementById('currentUsername');
const currentPassword = document.getElementById('currentPassword');
const newUsernameSection = document.getElementById('newUsernameSection');
const newPasswordSection = document.getElementById('newPasswordSection');
const submitSection = document.getElementById('submitSection');

showUsernameForm.addEventListener('click', () => {
    toggleButtons.classList.add('d-none');
    currentUsername.classList.remove('d-none');
    currentPassword.classList.remove('d-none');
    newUsernameSection.classList.remove('d-none');
    submitSection.classList.remove('d-none');
});

showPasswordForm.addEventListener('click', () => {
    toggleButtons.classList.add('d-none');
    currentUsername.classList.remove('d-none');
    currentPassword.classList.remove('d-none');
    newPasswordSection.classList.remove('d-none');
    submitSection.classList.remove('d-none');
});



