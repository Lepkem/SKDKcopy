var acc = document.getElementsByClassName("accordion");
var filter = document.getElementById("extrafilters");
filter.style.display = "none";
var i;

acc[0].addEventListener("click", function () {
    /* Toggle between hiding and showing the panel */
    var panel = this.nextElementSibling;
    if (panel.style.display === "block") {
        panel.style.display = "none";
    } else {
        panel.style.display = "block";
    }
});

var loadFile = function (event) {
    var image = document.getElementById("imgoutput");
    image.src = URL.createObjectURL(event.target.files[0]);
};



