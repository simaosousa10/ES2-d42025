window.smoothScrollToStart = (element) => {
    if (element) {
        element.scrollTo({ left: 0, behavior: 'smooth' });
    }
};

window.blurElement = (element) => {
    if (element) element.blur();
};

window.loadStoreMap = (latitude, longitude, storeName) => {
    console.log("Mapa chamado com:", latitude, longitude, storeName); // DEBUG

    const map = L.map('map').setView([latitude, longitude], 15);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19
    }).addTo(map);

    L.marker([latitude, longitude]).addTo(map)
        .bindPopup(`<b>${storeName}</b>`)
        .openPopup();
};

window.initScrollObserver = (dotnetHelper) => {
    const observer = new IntersectionObserver(async entries => {
        if (entries[0].isIntersecting) {
            await dotnetHelper.invokeMethodAsync('LoadMoreProducts');
        }
    }, { threshold: 1 });

    const anchor = document.getElementById('scroll-anchor');
    if (anchor) {
        observer.observe(anchor);
    }
};








