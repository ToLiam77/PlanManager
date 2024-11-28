
document.getElementById("clearFilters").onclick = function () {
    document.querySelector('input[name="SearchString"]').value = '';

    var daySelect = document.getElementById("daySelect");
    daySelect.selectedIndex = 0; 

    document.getElementById("priceRangeMin").value = 0;
    document.getElementById("priceRangeMax").value = 50;
    document.getElementById("minPrice").value = 0; 
    document.getElementById("maxPrice").value = 50; 

    document.getElementById("durationRangeMin").value = 0;
    document.getElementById("durationRangeMax").value = 15;
    document.getElementById("minDuration").value = 0;
    document.getElementById("maxDuration").value = 15; 

    document.getElementById("filterForm").submit();
};


window.onload = function () {
    var selectedDay = document.getElementById("selectedDay").value;
    var selectElement = document.getElementById("daySelect");

    if (selectedDay) {
        selectElement.value = selectedDay;
    }
};


function fillSlider(from, to, sliderColor, rangeColor, controlSlider) {
    const rangeDistance = to.max - to.min;
    const fromPosition = from.value - to.min;
    const toPosition = to.value - to.min;
    controlSlider.style.background = `linear-gradient(
      to right,
      ${sliderColor} 0%,
      ${sliderColor} ${(fromPosition) / (rangeDistance) * 100}%,
      ${rangeColor} ${((fromPosition) / (rangeDistance)) * 100}%,
      ${rangeColor} ${(toPosition) / (rangeDistance) * 100}%, 
      ${sliderColor} ${(toPosition) / (rangeDistance) * 100}%, 
      ${sliderColor} 100%)`;
}


var sliderColor = '#C6C6C6';
var rangeColor = '#25daa5';

const fromSliderPrice = document.querySelector('#priceRangeMin');
const toSliderPrice = document.querySelector('#priceRangeMax');
fillSlider(fromSliderPrice, toSliderPrice, sliderColor, rangeColor, toSliderPrice);

const fromSliderDuration = document.querySelector('#durationRangeMin');
const toSliderDuration = document.querySelector('#durationRangeMax');
fillSlider(fromSliderDuration, toSliderDuration, sliderColor, rangeColor, toSliderDuration);



// Price
let activeSliderPrice = null;
document.getElementById("priceRangeMin").addEventListener("focus", function () {
    activeSliderPrice = 'min';
});
document.getElementById("priceRangeMax").addEventListener("focus", function () {
    activeSliderPrice = 'max';
});

function updatePriceRange() {
    var minSlider = document.getElementById("priceRangeMin");
    var maxSlider = document.getElementById("priceRangeMax");
    var priceRangeLabel = document.getElementById("priceRangeLabel");


    if (activeSliderPrice == "max" && parseInt(maxSlider.value) < parseInt(minSlider.value)) {
        minSlider.value = maxSlider.value; // Push min slider backward
    }
    if (activeSliderPrice == "min" && parseInt(minSlider.value) > parseInt(maxSlider.value)) {
        maxSlider.value = minSlider.value; // Push max slider forward
    }


    var displayRange = "";
    if (parseInt(minSlider.value) === parseInt(maxSlider.value)) {
        if (parseInt(minSlider.value) == 0) {
            displayRange = "Free";
        }
        else {
            displayRange = "£" + minSlider.value + (parseInt(maxSlider.value) === 50 ? "+" : "");
        }
    }
    else if ((parseInt(maxSlider.value) === 50)) {
        if (parseInt(minSlider.value) == 0){
            displayRange = "Any";
        }
        else {
            displayRange = "£" + (parseInt(minSlider.value)) + "+";
        }
    }
    else {
        displayRange = "£" + minSlider.value + " - £" + maxSlider.value + (parseInt(maxSlider.value) === 50 ? "+" : "");
    }
    priceRangeLabel.innerHTML = displayRange;


    document.getElementById("minPrice").value = minSlider.value;
    document.getElementById("maxPrice").value = maxSlider.value;

    fillSlider(fromSliderPrice, toSliderPrice, sliderColor, rangeColor, toSliderPrice);
}
updatePriceRange();


// Duration
let activeSliderDuration = null;
document.getElementById("durationRangeMin").addEventListener("focus", function () {
    activeSliderDuration = 'min';
});
document.getElementById("durationRangeMax").addEventListener("focus", function () {
    activeSliderDuration = 'max';
});


function updateDurationRange() {
    var minSlider = document.getElementById("durationRangeMin");
    var maxSlider = document.getElementById("durationRangeMax");
    var durationRangeLabel = document.getElementById("durationRangeLabel");


    if (activeSliderDuration == "max" && parseInt(maxSlider.value) < parseInt(minSlider.value)) {
        minSlider.value = maxSlider.value; // Push min slider backward
    }
    if (activeSliderDuration == "min" && parseInt(minSlider.value) > parseInt(maxSlider.value)) {
        maxSlider.value = minSlider.value; // Push max slider forward
    }

    var timeRanges = [
        "15m",        // Index 0
        "30m",        // Index 1
        "45m",        // Index 2
        "1h",         // Index 3
        "1h 15m",     // Index 4
        "1h 30m",     // Index 5
        "1h 45m",     // Index 6
        "2h",         // Index 7
        "2h 15m",     // Index 8
        "2h 30m",     // Index 9
        "2h 45m",     // Index 10
        "3h",         // Index 11
        "3h 15m",     // Index 12
        "3h 30m",     // Index 13
        "3h 45m",     // Index 14
        "4h"          // Index 15
    ];

    var displayRange = null;

    if (maxSlider.value == 15) {
        displayRange = timeRanges[minSlider.value] + "+"
    }
    else if (minSlider.value == maxSlider.value) {
        displayRange = timeRanges[minSlider.value];
    }
    else {
        displayRange = timeRanges[minSlider.value] + " - " + timeRanges[maxSlider.value];
    }

    durationRangeLabel.innerHTML = displayRange;


    document.getElementById("minDuration").value = minSlider.value;
    document.getElementById("maxDuration").value = maxSlider.value;

    fillSlider(fromSliderDuration, toSliderDuration, sliderColor, rangeColor, toSliderDuration);
}
updateDurationRange();


document.addEventListener('DOMContentLoaded', function () {
    const table = document.querySelector('#activitiesTable');
    const headers = table.querySelectorAll('th[data-sort]');
    let sortOrder = 1; // 1 for ascending, -1 for descending

    headers.forEach(header => {
        header.addEventListener('click', function () {
            const sortKey = this.getAttribute('data-sort');
            sortTable(sortKey, table, sortOrder);
            sortOrder *= -1;
        });
    });

    function sortTable(key, table, order) {
        const tbody = table.querySelector('tbody');
        const rows = Array.from(tbody.querySelectorAll('tr'));

        rows.sort((rowA, rowB) => {
            const cellA = getCellValue(rowA, key);
            const cellB = getCellValue(rowB, key);

            const isNumeric = !isNaN(cellA) && !isNaN(cellB);
            return isNumeric ? (cellA - cellB) * order : cellA.localeCompare(cellB) * order;
        });

        rows.forEach(row => tbody.appendChild(row));
    }

    function getCellValue(row, key) {
        switch (key) {
            case 'duration':
                return convertDurationToMinutes(row.cells[3].innerText.trim());
            case 'price':
                let priceText = row.cells[4].innerText.trim();

                if (priceText === 'FREE') {
                    return 0;
                }

                return parseFloat(priceText.replace(/[^0-9.-]+/g, ''));
            default:
                return '';
        }
    }

    function convertDurationToMinutes(duration) {
        let totalMinutes = 0;

        const hourMatch = duration.match(/(\d+)h/);
        const minuteMatch = duration.match(/(\d+)m/);

        if (hourMatch) {
            totalMinutes += parseInt(hourMatch[1]) * 60;
        }

        if (minuteMatch) {
            totalMinutes += parseInt(minuteMatch[1]);
        }

        return totalMinutes;
    } 
});
