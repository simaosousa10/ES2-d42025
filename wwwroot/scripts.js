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

window.updatePriceChart = (labels, avgPrices, minPrices) => {
    const canvas = document.getElementById("priceChart");
    if (!canvas) {
        console.warn("Canvas 'priceChart' não encontrado. O gráfico não foi desenhado.");
        return;
    }

    const ctx = canvas.getContext("2d");

    // Apaga gráfico anterior se existir
    if (window.myChart) {
        window.myChart.destroy();
    }

    window.myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Preço Médio',
                    data: avgPrices,
                    borderColor: '#000',
                    backgroundColor: '#000',
                    fill: false,
                    tension: 0.1,
                    pointRadius: 0,
                    pointHoverRadius: 4
                },
                {
                    label: 'Preço Mínimo',
                    data: minPrices,
                    borderColor: 'red',
                    backgroundColor: 'red',
                    fill: false,
                    tension: 0.1,
                    pointRadius: 0,
                    pointHoverRadius: 4
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        font: {
                            size: 14
                        },
                        usePointStyle: true,
                        pointStyle: 'circle'
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: false,
                    ticks: {
                        stepSize: 50,
                        callback: function(value) {
                            return value.toLocaleString('pt-PT', { style: 'currency', currency: 'EUR' });
                        }
                    },
                    title: {
                        display: true,
                        text: 'Preço (€)',
                        font: {
                            size: 14
                        },
                        color: '#666',
                        padding: {
                            top: 10,
                            bottom: 0
                        },
                        rotation: -90
                    }
                }
            }
        }
    });
};

window.setupRightClickToClearSuggestions = function (selector, dotNetHelper) {
    const input = document.querySelector(selector);
    if (input) {
        input.addEventListener("contextmenu", function (e) {
            e.preventDefault();
            dotNetHelper.invokeMethodAsync("ClearSuggestionsFromJS");
        });
    }
};