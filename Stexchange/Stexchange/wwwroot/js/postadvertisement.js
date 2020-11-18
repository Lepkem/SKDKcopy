var acc = document.getElementsByClassName("accordion");
var filter = document.getElementById("extrafilters");

/*hides extrafilter field*/
filter.style.display = "none";

/*click function for hiding/showing panel*/
acc[0].addEventListener("click", function () {
    var panel = this.nextElementSibling;

    if (panel.style.display === "block") {
        panel.style.display = "none";
    } else {
        panel.style.display = "block";
    }
});
/*message*/
var message = document.getElementById("messagepicture");

/*previous button*/
var previous = document.getElementsByClassName("previous");

/*next button*/
var next = document.getElementsByClassName("next");
previous[0].style.display = "none";
next[0].style.display = "none";

/*image field*/
var image = document.getElementById("imgoutput");

/*image files*/
var files = document.getElementById("imginput").files;

/*span text*/
var imagecounter = document.getElementById("imagecounter")

var currentImage = 0;

/*inserts first image in list into img field*/
var loadFile = function (event) {
    currentImage = 0;
    image.src = URL.createObjectURL(files[currentImage]);
    imagecounter.textContent = (currentImage + 1) + "/" + files.length;
    message.style.display = "none";
    previous[0].style.display = "inline";
    next[0].style.display = "inline";
};

/*inserts next file into img field*/
function nextImg(n) {
    if (currentImage == files.length - 1) {
        currentImage = 0;
        image.src = URL.createObjectURL(files[currentImage]);
        imagecounter.textContent = (currentImage + 1) + "/" + files.length;
    }
    else {
        currentImage += n;
        image.src = URL.createObjectURL(files[currentImage]);
        imagecounter.textContent = (currentImage + 1) + "/" + files.length;
    }
}

/*inserts previous file into img field*/
function previousImg(n) {
    if (currentImage > 0) {
        currentImage -= n;
        image.src = URL.createObjectURL(files[currentImage]);
        imagecounter.textContent = (currentImage + 1) + "/" + files.length;
    } else {
        currentImage = files.length - 1;
        image.src = URL.createObjectURL(files[currentImage]);
        imagecounter.textContent = (currentImage + 1) + "/" + files.length;
    }
}





