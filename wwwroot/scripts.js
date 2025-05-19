window.smoothScrollToStart = (element) => {
    if (element) {
        element.scrollTo({ left: 0, behavior: 'smooth' });
    }
};
