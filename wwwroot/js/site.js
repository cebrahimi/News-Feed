let ARTICLE_CONTAINER_SELECTOR = '#articles';
let GENRE_CONTAINER_SELECTOR = '#genres';
let WEBSITE_CONTAINER_SELECTOR = ".website-container";
let FOCUS_SWITCH_SELECTOR = "#focus-switch";
let FOCUS_CONTAINER_SELECTOR = '#focus-container';
let THEME_SWITCH_SELECTOR = '#theme-switch';
let TITLE_SELECTOR = '.title';

let App = window.App;
let FormHandler = App.FormHandler;
let LOGIN_FORM = new FormHandler(LOGIN_FORM_SELECTOR);
let SIGNUP_FORM = new FormHandler(SIGNUP_FORM_SELECTOR);

let loginModal = $('#login-modal');

$('#settings_dropdown').click(function (e) {
    e.stopPropagation();
});

$(function () {
    checkCookie();
    
    $(FAVOURITES_TOGGLE_SELECTOR).click(function () {
        toggleFavourites();
    });
  
    LOGIN_FORM.addSubmitHandler(authUser);
    SIGNUP_FORM.addSubmitHandler(registerUser);

    loginModal.on('show.bs.modal', function (e) {
        console.log('test');
        responseText("");
    });
    
    $(FOCUS_SWITCH_SELECTOR).bootstrapSwitch();
    $(FOCUS_SWITCH_SELECTOR).bootstrapSwitch('onSwitchChange', function (e, data) {
        toggleFocusMode(data);
    });
    
    $(THEME_SWITCH_SELECTOR).bootstrapSwitch();
    $(THEME_SWITCH_SELECTOR).bootstrapSwitch('onText', 'Dark');
    $(THEME_SWITCH_SELECTOR).bootstrapSwitch('offText', 'Light');
    $(THEME_SWITCH_SELECTOR).bootstrapSwitch('onSwitchChange', function (e, data) {
        toggleTheme(data);
    });
    
    $(ARTICLE_CONTAINER_SELECTOR).scroll(handleScroll);
    
});

handleScroll = (e) => {
    const bottom = e.target.scrollHeight - e.target.scrollTop <= e.target.clientHeight;
    if (bottom) {
        console.log('header bottom reached');
    }
};

function checkCookie() {
    $.ajax({
        type: 'POST',
        url: 'Cookie/CheckCookie',
        success:function(data) {
            if (data === -1) {
                return;
            }
            userId = data;
            
            $.ajax({
                type: 'POST',
                url: 'User/GetUser',
                data: {userId: userId},
                success:function(data) {
                    let email = data.email;
                    toggleLogin();
                    getUserFavourites(userId);
                    $(EMAIL_SELECTOR).text("Welcome, " + email);
                }
            });
        },
        error:function(){
            responseText('An error occured');
        }
    })
}

function toggleFocusMode(data) {
    if (data) {
        $(FOCUS_CONTAINER_SELECTOR).fadeOut();
    } else {
        $(FOCUS_CONTAINER_SELECTOR).fadeIn();
    }
}

function toggleTheme(data) {
    if (data) {
        document.documentElement.style.setProperty('--color-wet-asphalt', '#34495E');
        document.documentElement.style.setProperty('--color-midnight-blue', '#2C3E50');
        document.documentElement.style.setProperty('--color-turquoise', '#1ABC9C');
        document.documentElement.style.setProperty('--color-green-sea', '#16A085');
        document.documentElement.style.setProperty('--color-peter-river', '#3498DB');
        document.documentElement.style.setProperty('--color-belize-hole', '#2980B9');
        document.documentElement.style.setProperty('--color-dark-gray', '#6c757d');
        $(EMAIL_SELECTOR).attr('style', 'color: lightgray;');
        $(TITLE_SELECTOR).attr('style', 'color: var(--color-light-gray);');

    } else {
        document.documentElement.style.setProperty('--color-wet-asphalt', '#ECF0F1');
        document.documentElement.style.setProperty('--color-midnight-blue', '#BDC3C7');
        document.documentElement.style.setProperty('--color-turquoise', '#F1C40F');
        document.documentElement.style.setProperty('--color-green-sea', '#F39C12');
        document.documentElement.style.setProperty('--color-peter-river', '#E67E22');
        document.documentElement.style.setProperty('--color-belize-hole', '#D35400');
        document.documentElement.style.setProperty('--color-dark-gray', '#a3a7ab');
        $(EMAIL_SELECTOR).attr('style', 'color: var(--color-wet-asphalt);');
        $(TITLE_SELECTOR).attr('style', 'color: var(--color-wet-asphalt);');

    }
}

/**
 * Handles clicks on genres. Adds visual cue of selected state and filters genres.
 * @param element {Element} The clicked genre element
 */
var genreClick = function (element) {
    var genre = $(element).children()[0].id;
    var shouldRemove = $(element).hasClass("selected");

    if (shouldRemove) {
        $(element).removeClass("selected");
    } else {
        $(element).addClass("selected");
    }

    filterByGenre(genre);
    $("#article-search-field").val("");
};

/**
 * Refreshes articles and genres with new data from clicked website.
 * @param element {Element} The clicked website
 */
function websiteClick(element) {
    let target = $(element);
    var id = target.attr('value');

    $(WEBSITE_CONTAINER_SELECTOR).removeClass("selected");
    target.addClass("selected");

    $.ajax({
        url: 'Filter/GetFilteredArticles',
        data: {websiteId: id},
        success: (function (data) {
            refreshCards(data);
            markFavourites();
        })
    });
    $.ajax({
        url: 'Filter/GetFilteredGenres',
        data: {websiteId: id},
        success: (data => refreshGenres(data))
    });
    $("#article-search-field").val("");
}

/**
 * Resets articles with new data
 * @param data {string} HTML formatted article data
 */
function refreshCards(data) {
    let cardview = $(ARTICLE_CONTAINER_SELECTOR);
    cardview.empty();
    cardview.append(data);
}

/**
 * Resets genres with new data
 * @param data {string} HTML formatted genre data
 */
function refreshGenres(data) {
    let genreView = $(GENRE_CONTAINER_SELECTOR);
    genreView.empty();
    genreView.append(data);
    genres = []; // Reset genre filter
}

loginModal.on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    var recipient = button.data('type');

    loginModal.find('.modal-title').text(recipient);

    if (recipient === "Signup") {
        loginModal.find('[id=login-form]').hide();
        loginModal.find('[id=signup-form]').show();
    } else {
        loginModal.find('[id=signup-form]').hide();
        loginModal.find('[id=login-form]').show();
    }
});
