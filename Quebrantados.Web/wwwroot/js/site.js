window.quebrantados = window.quebrantados || {};

window.quebrantados.scrollToElement = function (elementId) {
    document.getElementById(elementId)?.scrollIntoView({
        behavior: "smooth",
        block: "start"
    });
};
