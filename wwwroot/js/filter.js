let ARTICLE_SELECTOR = '.card, .mb-3';
let FAVOURITE_SELECTOR = '#favourites ul .todo-done';

let genres = [];

/**
 * Searches articles in currently selected genre (or all). Search is based on summary and title text
 * and matches coherent substrings
 * @param element {Element} The search input field
 */
function searchArticles(element) {
    let articles = $(ARTICLE_SELECTOR);

    let queryText = $(element).val().toLowerCase();

    articles.each(function () {
        let title = $(this).children(".row").children().children('a').children().text();
        let summary = $(this).children(".row").children().children('.card-text').text();
        let searchableText = title + " " + summary;
        searchableText = searchableText.toLowerCase();

        let genreExists;
        if (genres.length === 0) {
            genreExists = true;
        } else {
            genreExists = getGenreExists.call(this);
        }
        
        toggleElement.call(this, searchableText.includes(queryText) && genreExists);
    });
}

function searchFavourites(element) {
    let favourites = $(FAVOURITE_SELECTOR);
    
    let queryText = $(element).val().toLowerCase();

    favourites.each(function () {
        let title = $(this).children(".todo-content").children('h4').text();
        let searchableText = title.toLowerCase();

        toggleElement.call(this, searchableText.includes(queryText));
    });
}

/**
 * Shows or hides an element based on a boolean value
 * @param shouldDisplay {boolean} True if element should be displayed, false otherwise
 */
function toggleElement(shouldDisplay) {
    if (shouldDisplay) {
        if ($(this).hasClass("d-none")) {
            $(this).removeClass("d-none");
        }
    } else {
        $(this).addClass("d-none");
    }
}

/**
 * Checks if an article contains any of the filtered genres
 * @returns {boolean}
 */
function getGenreExists() {
    let articleGenres = [];

    $(this).children(".row").children().children(".badge").each(function () {
        articleGenres.push($(this).text().slice(1));
    });

    return articleGenres.some(genre => genres.includes(genre));
}

/**
 * Filters articles by a genre. If no genres are given, then all articles are displayed
 * @param genre {string} Genre to filter by
 */
function filterByGenre(genre) {
    let articles = $(ARTICLE_SELECTOR);

    // Add or remove genre from array
    if (genres.includes(genre)) {
        genres = genres.filter(function (value, index, arr) {
            return value !== genre;
        })
    } else {
        genres.push(genre);
    }

    // Display all articles if no genres are given
    if (genres.length === 0) {
        articles.each(function () {
            $(this).fadeIn();
            $(this).removeClass("d-none");
        });
        return;
    }

    // Filter articles
    articles.each(function () {
        let genreExists = getGenreExists.call(this);

        toggleElement.call(this, genreExists);
    });
}