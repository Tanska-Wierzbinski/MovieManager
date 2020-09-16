$(document).ready(function () {
    $('[data-tooltip=tooltip]').tooltip({ placement: "top", padding: 0 });
});
$(".pagebtn").on("click", function () {
    $(this).removeClass();
    $(this).addClass("pagebtn btn btn-dark active");
    var gradeBtns = document.getElementById("gradeBtns").children;
    var yearBtns = document.getElementById("yearBtns").children;
    var catBtns = document.getElementById("catBtns").children;

    var grade1, grade2, year1, year2;
    var cats = [];
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

    for (var c = 0; c < catBtns.length; c++) {
        if (catBtns[c].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
            cats.push(catBtns[c].getAttribute("value"));
        }
    }


    var urlstring = "/Movie/Index/?";
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
    for (var c = 0; c < cats.length; c++) {
        urlstring += "&categories=" + cats[c];
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
    var catBtns = document.getElementById("catBtns").children;

    var grade1, grade2, year1, year2;
    var cats = [];
    

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

    for (var c = 0; c < catBtns.length; c++) {
        if (catBtns[c].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
            cats.push(catBtns[c].getAttribute("value"));
        }
    }
   
    var urlstring = "/Movie/Index/?";
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
    for (var c = 0; c < cats.length; c++) {
        urlstring += "&categories=" + cats[c];
    }

    window.location.href = urlstring;
});

// SORTOWANIE I WYSYŁANIE LINKU
$(".dropdown-item").on("click", function () {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    var urlstring = "/Movie/Index/?";

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

   // var categories = [];
   // var gradeMin, gradeMax, yearMin, yearMax, sortOrder, pageSize;
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] == "categories") {
            urlstring += "&categories=" + sParameterName[1];
          //  categories.push(sParameterName[1]);
        }
        else if (sParameterName[0] == "gradeMin") {
            urlstring += "&gradeMin=" + sParameterName[1];
          //  gradeMin = sParameterName[1];
          //  gradeMax = gradeMin;
        }
        else if (sParameterName[0] == "gradeMax") {
            urlstring += "&gradeMax=" + sParameterName[1];
          //  gradeMax = sParameterName[1];
        }
        else if (sParameterName[0] == "yearMin") {
            urlstring += "&yearMin=" + sParameterName[1];
          //  yearMin = sParameterName[1];
          //  yearMax = yearMin;
        }
        else if (sParameterName[0] == "yearMax") {
            urlstring += "&yearMax=" + sParameterName[1];
          //  yearMax = sParameterName[1];
        }
        else if (sParameterName[0] == "sortOrder" && sorting == false) {
            urlstring += "&sortOrder=" + sParameterName[1];
          //  sortOrder = sParameterName[1];
        }
        else if (sParameterName[0] == "pageSize" && paging == false) {
            urlstring += "&pageSize=" + sParameterName[1];
          //  pageSize = sParameterName[1];
        }
    }
    window.location.href = urlstring;
});

//$(".dropdown-item").on("click", function () {
//    var gradeBtns = document.getElementById("gradeBtns").children;
//    var yearBtns = document.getElementById("yearBtns").children;
//    var catBtns = document.getElementById("catBtns").children;

//    var grade1, grade2, year1, year2;
//    var cats = [];
//    var dropDown = $(this).attr("value");
//    var dropDownParentId = $(this).parent().attr("id");// .attr("id");
//    //var dropDownParentId = dropDownParent.attr("id");

//    for (var g = 0; g < gradeBtns.length; g++) {
//        if (gradeBtns[g].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
//            if (grade1 == null) { grade1 = gradeBtns[g].getAttribute("value"); }
//            else { grade2 = gradeBtns[g].getAttribute("value"); }
//        }
//    }

//    for (var y = 0; y < yearBtns.length; y++) {
//        if (yearBtns[y].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
//            if (year2 == null) { year2 = yearBtns[y].getAttribute("value"); }
//            else { year1 = yearBtns[y].getAttribute("value"); }
//        }
//    }

//    for (var c = 0; c < catBtns.length; c++) {
//        if (catBtns[c].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
//            cats.push(catBtns[c].getAttribute("value"));
//        }
//    }

//    var urlstring = "/Movie/Index/?";//yearMin=" + year2 + "&yearMax=" + year1 + "&gradeMin=" + grade1 + "&gradeMax=" + grade2 + "&sortOrder=" + sort;
//    if (year2 != null) {
//        urlstring += "&yearMin=" + year1;
//    }
//    if (year1 != null) {
//        urlstring += "&yearMax=" + year2;
//    }
//    if (grade1 != null) {
//        urlstring += "&gradeMin=" + grade1;
//    }
//    if (grade2 != null) {
//        urlstring += "&gradeMax=" + grade2;
//    }
//    for (var c = 0; c < cats.length; c++) {
//        urlstring += "&categories=" + cats[c];
//    }

//    if (dropDownParentId == "pageSize") {
//        urlstring += "&pageSize=" + dropDown;
//    }
//    else {
//        urlstring += "&sortOrder=" + dropDown;
//    } 
//    window.location.href = urlstring;
//});

//KOLOROWANIE GUZIKÓW PO PRZEŁADOWANIU STRONY
$("html").ready(function () {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    var categories = [];
    var gradeMin, gradeMax, yearMin, yearMax;
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == "categories") {
            categories.push(sParameterName[1]);
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
    }

    var gradeBtns = document.getElementById("gradeBtns").children;
    var yearBtns = document.getElementById("yearBtns").children;
    var catBtns = document.getElementById("catBtns").children;

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

    for (var i = 0; i < catBtns.length; i++) {
        for (var j = 0; j < categories.length; j++) {
            if (catBtns[i].getAttribute("value") == categories[j]) {
                catBtns[i].setAttribute("class", "filterBtn btn btn-outline-warning active");
                catBtns[i].setAttribute("aria-pressed", "true");
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
                //gradeBtns[i].removeClass();
                //gradeBtns[i].addClass("filterBtn btn btn-outline-warning");
                //gradeBtns[i].attr("aria-pressed", "false");
                gradeBtns[i].setAttribute("class", "filterBtn btn btn-outline-warning");
                gradeBtns[i].setAttribute("aria-pressed", "false");
            }
        }
        
    }
    else if (clearBtn == "yearModal") {
        var yearBtns = document.getElementById("yearBtns").children;
        for (var i = 0; i < yearBtns.length; i++) {
            if (yearBtns[i].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
                //gradeBtns[i].removeClass();
                //gradeBtns[i].addClass("filterBtn btn btn-outline-warning");
                //gradeBtns[i].attr("aria-pressed", "false");
                yearBtns[i].setAttribute("class", "filterBtn btn btn-outline-warning");
                yearBtns[i].setAttribute("aria-pressed", "false");
            }
        }
    }
    else if (clearBtn == "categoryModal") {
        var catBtns = document.getElementById("catBtns").children;
        for (var i = 0; i < catBtns.length; i++) {
            if (catBtns[i].getAttribute("class") == "filterBtn btn btn-outline-warning active") {
                //gradeBtns[i].removeClass();
                //gradeBtns[i].addClass("filterBtn btn btn-outline-warning");
                //gradeBtns[i].attr("aria-pressed", "false");
                catBtns[i].setAttribute("class", "filterBtn btn btn-outline-warning");
                catBtns[i].setAttribute("aria-pressed", "false");
            }
        }
    }
});


