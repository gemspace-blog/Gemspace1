document.querySelector(".menu-trigger").addEventListener("click", function () {
    document.querySelector("nav").style.transform = "translate(0,0)";
});

document.querySelector(".menu-closer").addEventListener("click", function () {
    document.querySelector("nav").style.transform = "translate(-100%,0)";
})