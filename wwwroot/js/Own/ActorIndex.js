$(document).ready(function () {
    $('[data-tooltip=tooltip]').tooltip({ placement: "top", padding: 0 });
});

//WYSŁANIE LINKU PO STRONNICOWANIU
$(".pagebtn").on("click", function () {
    $(this).removeClass();
    $(this).addClass("pagebtn btn btn-dark active");
    var gradeBtns = document.getElementById("gradeBtns").children;
    var yearBtns = document.getElementById("yearBtns").children;
    var countryBtns = document.getElementById("countryBtns").children;
    var gender = document.getElementById("genderSlider").getAttribute("value");

    var grade1, grade2, year1, year2;
    var countries = [];
    var pageNumber;

    for (var g = 0; g < gradeBtns.length; g++) {
        if (gradeBtns[g].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
            if (grade1 == null) { grade1 = gradeBtns[g].getAttribute("value"); grade2 = grade1; }
            else { grade2 = gradeBtns[g].getAttribute("value"); }
        }
    }

    for (var y = 0; y < yearBtns.length; y++) {
        if (yearBtns[y].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
            if (year2 == null) { year2 = yearBtns[y].getAttribute("value"); year1 = year2; }
            else { year1 = yearBtns[y].getAttribute("value"); }
        }
    }

    for (var c = 0; c < countryBtns.length; c++) {
        if (countryBtns[c].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
            countries.push(countryBtns[c].getAttribute("value"));
        }
    }

    var urlstring = "/Actor/Index/?";
    if (year2 != null) {
        urlstring += "&yearMin=" + year1;
    }
    if (year1 != null) {
        urlstring += "&yearMax=" + year2;
    }
    if (grade1 != null) {
        urlstring += "&gradeMin=" + grade1;
    }
    if (grade2 != null) {
        urlstring += "&gradeMax=" + grade2;
    }
    for (var c = 0; c < countries.length; c++) {
        urlstring += "&countries=" + countries[c];
    }
    if ($(this).attr("id") == "next") {
        pageNumber = $("#next").attr("value");
        urlstring += "&pageNumber=" + pageNumber;
        window.location.href = urlstring;
    }
    else if ($(this).attr("id") == "previous"/* && $(this).attr("value") > 0*/) {
        pageNumber = $("#previous").attr("value");
        urlstring += "&pageNumber=" + pageNumber;
        window.location.href = urlstring;
    }
});

//ZAZNACZANIE MAX DWÓCH GUZIKÓW
$(".filterBtn").on("click", function () {
    var parent = $(this).closest(".btn-group-toggle");
    var parentId = parent.attr("id");

    var buttons = document.getElementById(parentId).children;

    var counter = 0;
    for (var i = 0; i < buttons.length; i++) {
        if (buttons[i].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
            counter++;
            if (counter == 2) {
                for (var j = 0; j < buttons.length; j++) {
                    buttons[j].setAttribute("class", "filterBtn btn btn-outline-warning");
                    buttons[j].setAttribute("aria-pressed", "true");
                }
                break;
            }
        }
    }

    if ($(this).attr("class") == "filterBtn btn btn-outline-warning focus" || $(this).attr("class") == "filterBtn btn btn-outline-warning") {
        $(this).removeClass();
        $(this).addClass("filterBtn btn btn-outline-warning active");
        $(this).attr("aria-pressed", "true");
    }
    else {
        $(this).removeClass();
        $(this).addClass("filterBtn btn btn-outline-warning");
        $(this).attr("aria-pressed", "false");
    }
    $(this).trigger("filterBtnclicked");
});

$(".btn-group-toggle").on("filterBtnclicked", function () {
    var id = $(this).attr("id");
    var button = document.getElementById(id).children;
    var ac1, ac2;
    var counter = 0;
    for (var i = 0; i < button.length; i++) {
        if (button[i].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
            counter++;
            if (counter == 1) {
                ac1 = i;
            }
            else if (counter == 2) {
                for (var j = ac1; j <= i; j++) {
                    button[j].setAttribute("class", "filterBtn btn btn-outline-warning active");
                    button[j].setAttribute("aria-pressed", "true");
                }
            }
        }
    }
});

//FILTROWANIE I WYSYŁANIE LINKU
$("#filter").on("click", function () {
    var gradeBtns = document.getElementById("gradeBtns").children;
    var yearBtns = document.getElementById("yearBtns").children;
    var countryBtns = document.getElementById("countryBtns").children;
    var gender = $("#genderSlider").val();

    var grade1, grade2, year1, year2;
    var countries = [];

    for (var g = 0; g < gradeBtns.length; g++) {
        if (gradeBtns[g].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
            if (grade1 == null) { grade1 = gradeBtns[g].getAttribute("value"); grade2 = grade1; }
            else { grade2 = gradeBtns[g].getAttribute("value"); }
        }
    }

    for (var y = 0; y < yearBtns.length; y++) {
        if (yearBtns[y].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
            if (year2 == null) { year2 = yearBtns[y].getAttribute("value"); year1 = year2;}
            else { year1 = yearBtns[y].getAttribute("value"); }
        }
    }

    for (var c = 0; c < countryBtns.length; c++) {
        if (countryBtns[c].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
            countries.push(countryBtns[c].getAttribute("value"));
        }
    }

    var urlstring = "/Actor/Index/?gender=" + gender;
    if (year2 != null) {
        urlstring += "&yearMin=" + year1;
    }
    if (year1 != null) {
        urlstring += "&yearMax=" + year2;
    }
    if (grade1 != null) {
        urlstring += "&gradeMin=" + grade1;
    }
    if (grade2 != null) {
        urlstring += "&gradeMax=" + grade2;
    }
    for (var c = 0; c < countries.length; c++) {
        urlstring += "&countries=" + countries[c];
    }

    window.location.href = urlstring;
});

// SORTOWANIE I WYSYŁANIE LINKU
$(".dropdown-item").on("click", function () {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    var urlstring = "/Actor/Index/?";

    var sorting = false, paging = false;

    var dropDownItemParent = $(this).parent();

    if (dropDownItemParent.attr("class") == "col-auto") {
        urlstring += "&sortOrder=" + $(this).attr("value");
        sorting = true;
    }
    else if (dropDownItemParent.attr("id") == "pageSize") {
        urlstring += "&pageSize=" + $(this).attr("value");
        paging = true;
    }

    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] == "countries") {
            urlstring += "&countries=" + sParameterName[1];
        }
        else if (sParameterName[0] == "gradeMin") {
            urlstring += "&gradeMin=" + sParameterName[1];
        }
        else if (sParameterName[0] == "gradeMax") {
            urlstring += "&gradeMax=" + sParameterName[1];
        }
        else if (sParameterName[0] == "yearMin") {
            urlstring += "&yearMin=" + sParameterName[1];
        }
        else if (sParameterName[0] == "yearMax") {
            urlstring += "&yearMax=" + sParameterName[1];
        }
        else if (sParameterName[0] == "gender") {
            urlstring += "&gender=" + sParameterName[1];
        }
        else if (sParameterName[0] == "sortOrder" && sorting == false) {
            urlstring += "&sortOrder=" + sParameterName[1];
        }
        else if (sParameterName[0] == "pageSize" && paging == false) {
            urlstring += "&pageSize=" + sParameterName[1];
        }
    }
    window.location.href = urlstring;
});

//KOLOROWANIE GUZIKÓW PO PRZEŁADOWANIU STRONY
$("html").ready(function () {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    var countries = [];
    var gradeMin, gradeMax, yearMin, yearMax, gender;
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == "countries") {
            countries.push(sParameterName[1]);
        }
        else if (sParameterName[0] == "gradeMin") {
            gradeMin = sParameterName[1];
            gradeMax = gradeMin;
        }
        else if (sParameterName[0] == "gradeMax") {
            gradeMax = sParameterName[1];
        }
        else if (sParameterName[0] == "yearMin") {
            yearMin = sParameterName[1];
            yearMax = yearMin;
        }
        else if (sParameterName[0] == "yearMax") {
            yearMax = sParameterName[1];
        }
        else if (sParameterName[0] == "gender") {
            gender = sParameterName[1];
        }
    }

    var gradeBtns = document.getElementById("gradeBtns").children;
    var yearBtns = document.getElementById("yearBtns").children;
    var countryBtns = document.getElementById("countryBtns").children;
    var genderSlider = document.getElementById("genderSlider");

    genderSlider.setAttribute("value", gender);

    for (var i = 0; i < gradeBtns.length; i++) {
        if (gradeBtns[i].getAttribute("value") >= gradeMin && gradeBtns[i].getAttribute("value") <= gradeMax) {
            gradeBtns[i].setAttribute("class","filterBtn btn btn-outline-warning active");
            gradeBtns[i].setAttribute("aria-pressed", "true");
        }
    }

    for (var i = 0; i < yearBtns.length; i++) {
        if (yearBtns[i].getAttribute("value") >= yearMin && yearBtns[i].getAttribute("value") <= yearMax) {
            yearBtns[i].setAttribute("class", "filterBtn btn btn-outline-warning active");
            yearBtns[i].setAttribute("aria-pressed", "true");
        }
    }

    for (var i = 0; i < countryBtns.length; i++) {
        for (var j = 0; j < countries.length; j++) {
            if (countryBtns[i].getAttribute("value") == countries[j]) {
                countryBtns[i].setAttribute("class", "filterBtn btn btn-outline-warning active");
                countryBtns[i].setAttribute("aria-pressed", "true");
                break;
            }
        }
    }
});

$(".clearModalBtn").on("click", function () {
    var clearBtn = $(this).parent().attr("id");
    if (clearBtn == "gradeModal") {
        var gradeBtns = document.getElementById("gradeBtns").children;
        for (var i = 0; i < gradeBtns.length; i++) {
            if (gradeBtns[i].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
                gradeBtns[i].setAttribute("class", "filterBtn btn btn-outline-warning");
                gradeBtns[i].setAttribute("aria-pressed", "false");
            }
        }
    }
    else if (clearBtn == "yearModal") {
        var yearBtns = document.getElementById("yearBtns").children;
        for (var i = 0; i < yearBtns.length; i++) {
            if (yearBtns[i].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
                yearBtns[i].setAttribute("class", "filterBtn btn btn-outline-warning");
                yearBtns[i].setAttribute("aria-pressed", "false");
            }
        }
    }
    else if (clearBtn == "countryModal") {
        var countryBtns = document.getElementById("countryBtns").children;
        for (var i = 0; i < countryBtns.length; i++) {
            if (countryBtns[i].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
                countryBtns[i].setAttribute("class", "filterBtn btn btn-outline-warning");
                countryBtns[i].setAttribute("aria-pressed", "false");
            }
        }
    }
});


