// document variables
const btn = document.getElementById("submit");
const clientSideCheckbox = document.getElementById("action-type");
const results = document.getElementById("results");
const tableBody = document.getElementById("table-body");
const entry = document.getElementById("entry");
const wordHeader = document.getElementById("wordHeader");
const countHeader = document.getElementById("countHeader"); 
const tableResults = document.getElementById("resultsTable");
const textRadioButton = document.getElementById("input-text");
const urlRadioButton = document.getElementById("input-url");
const textSubmissionDiv = document.getElementById("text-submission");
const urlSubmissionDiv = document.getElementById("url-submission");
let tableContent = null;



// If btn exists, user is on submission page, else is on results page from server
if (btn !== null) {
    // Add form event listeners
    btn.addEventListener('click', onClick);
    textRadioButton.addEventListener("change", handleRadioButtonChange);
    urlRadioButton.addEventListener("change", handleRadioButtonChange);
} else {
    tableContent = Array.from(tableResults.tBodies[0].rows);
    addSortingListeners();
}



// Event handler function
function handleRadioButtonChange(event) {
    // Check which radio button is selected, show/hide corresponsing inputs
    if (textRadioButton.checked) {
        textSubmissionDiv.style.display = "block";
        urlSubmissionDiv.style.display = "none";
        
    } else if (urlRadioButton.checked) {
        textSubmissionDiv.style.display = "none";
        urlSubmissionDiv.style.display = "block";
    }
}

// If client side and Text submission checked, process text and display results using index.js
function onClick(event) {
    if (clientSideCheckbox.checked && textRadioButton.checked) {
        event.preventDefault(); // Prevent default form submission

        // Process text and obtain results
        const results = processText();

        // Sort results by value (frequency) in descending order
        const sortedData = Object.entries(results).sort(([, value1], [, value2]) => value2 - value1);

        // Display sorted results
        displayResults(sortedData);

        // Save table rows from table body
        tableContent = Array.from(tableResults.tBodies[0].rows);

        // Add event listeners to sort table
        addSortingListeners();
    }
}

/**
* Counts occurrences of words in textbox value
* @returns (object) - key/value pairs of words and number of occurrences
*
*/
function processText() {
    // Create word list
    const delimiters = /[ \n—]+/
    const wordList = document.getElementById("sample-text").value.split(delimiters);

    // Do not count these words
    const ignoreList = ["a", "an", "at", "and", "for", "in", "is", "of", "on", "the"];

    const data = {};
    const cleanedWordList = [];

    // Clean each word of non-alphanumeric characters and increment occurrence count
    wordList.forEach(function (word) {
        // Trim and convert the word to lowercase
        const trimmedWord = trimNonAlphanumericReg(word).toLowerCase();

        // Skip if the trimmed word is in the ignore list or is empty
        if (ignoreList.includes(trimmedWord) || trimmedWord.trim() === '') {
            return;
        }

        if (data.hasOwnProperty(trimmedWord)) {
            // If word existsm increment the count for the existing word
            data[trimmedWord] += 1;
        } else {
            // Else, add the new word to the data object with a count of 1
            data[trimmedWord] = 1;
        }
    });

    return data;

}

/**
* Adds rows to table based on input
* @param (array) input - array of key/value pairs to be displayed
*/
function displayResults(input) {
    
    input.forEach((entry) => {
        const row = document.createElement("tr");
        const td1 = document.createElement("td");
        const td2 = document.createElement("td");

        td1.textContent = entry[0];
        td2.textContent = entry[1];

        row.appendChild(td1);
        row.appendChild(td2);
        tableBody.appendChild(row);
    });

    // Display results and hide entry form
    results.style.display = "block";
    entry.style.display = "none";

}

/**
* Removes non-alphanumeric characters from beginning and end of string
* @param (string) str - string to be trimmed
* @returns (string) - trimmed string
*
*/
function trimNonAlphanumericReg(str) {
    return str.replace(/^[^a-zA-Z0-9]+|[^a-zA-Z0-9]+$/g, '');
}

// Variables to manage sort direction
let wordSort = null;
let countSort = null;
const wordIndex = 0;
const countIndex = 1;

// Sorts results table aphabetically by word
function sortByWord() {
    if (wordSort === 1 || wordSort === null) {
        // Sort in ascending order
        tableContent.sort((a, b) => {
            wordSort = 0;
            let x = a.cells[0].textContent;
            let y = b.cells[0].textContent;
            return x.localeCompare(y);
        });
    } else {
        // Sort in descending order
        tableContent.sort((a, b) => {
            wordSort = 1;
            let x = a.cells[0].textContent;
            let y = b.cells[0].textContent;
            return y.localeCompare(x);
        });
    }
    countSort = null;
    refreshSortedList();
}

// Sorts results table numerically by frequency
function sortByFrequency() {
    if (countSort === 1 || countSort === null) {
        // Sort in ascending order
        tableContent.sort((a, b) => {
            countSort = 0;
            let x = a.cells[1].textContent;
            let y = b.cells[1].textContent;
            return parseInt(x) - parseInt(y);
        });
    } else {
        // Sort in descending order
        tableContent.sort((a, b) => {
            countSort = 1;
            let x = a.cells[1].textContent;
            let y = b.cells[1].textContent;
            return parseInt(y) - parseInt(x);
        });
    }
    wordSort = null;
    refreshSortedList();
}

// Refreshes the table display post sort
function refreshSortedList() {
    tableResults.tBodies[0].innerHTML = '';
    tableContent.forEach(row => tableResults.tBodies[0].appendChild(row));

}

// Adds click listeners to results table headers
function addSortingListeners() {
    countHeader.addEventListener('click', sortByFrequency);
    wordHeader.addEventListener('click', sortByWord);
}
