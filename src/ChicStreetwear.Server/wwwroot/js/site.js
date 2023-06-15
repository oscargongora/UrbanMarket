window.addLandingCarouselIntersectionObserver = () => {
    const appHeader = document.getElementById("app-header");

    const landingCarousel = document.getElementById("landing-carousel");

    const options = {
        rootMargin: "0px",
        threshold: 1.0
    };

    const observer = new IntersectionObserver((entries, observer) => {
        entries.forEach((entry) => {
            if (!entry.isIntersecting) {
                appHeader.classList.add('scrolled');
            }
            else {
                appHeader.classList.remove('scrolled');
            }
        });
    }, options);

    observer.observe(landingCarousel);
}