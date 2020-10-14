var spanEl = document.getElementsByClassName("caret");

for (var i = 0; i < spanEl.length; i++) {
    spanEl[i].addEventListener("click", function () {
        this.parentElement.querySelector(".nested").classList.toggle("expanded");
        this.classList.toggle("rotated");
    });
}

var sortRangeTogglers = document.getElementsByClassName("sort-range-toggle");
var sortRange = document.getElementsByClassName("sort-range");
var displays = document.getElementsByClassName("sort-range-display");

for (var i = 0; i < sortRange.length; i++) {
    sortRangeTogglers[i].addEventListener("click", togglerDecorator(i));
    sortRange[i].addEventListener("input", (event) => {
        let paragraph = document.getElementById(event.target.getAttribute("id") + "_amount");
        let span = paragraph.childNodes[0];
        span.innerText = event.target.value;
    });
}

function togglerDecorator(j) {
    return function () {
        sortRange[j].disabled = !sortRange[j].disabled;
        displays[j].hidden = !displays[j].hidden;
    };
}