var spanEl = document.getElementsByClassName("caret");

for (var i = 0; i < spanEl.length; i++) {
    spanEl[i].addEventListener("click", function () {
        this.parentElement.querySelector(".nested").classList.toggle("expanded");
        this.classList.toggle("rotated90");
    });
}