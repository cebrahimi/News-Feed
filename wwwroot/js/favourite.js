let FAVOURITES_SELECTOR = "#favourites";
let FAVOURITES_TOGGLE_SELECTOR = "#toggleFav";

let favourites = [];
let degrees = 0; // Keeps track of list-icon rotation

function getUserFavourites(userId) {
    $.ajax({
        type: 'POST',
        url: 'User/GetFavourites',
        data: { userId: userId },
        success: function(articleIds) {
            if (articleIds && articleIds.length !== 0) {
                $.ajax({
                    type: 'POST',
                    url: 'Filter/GetArticles',
                    data: {articleIds: articleIds},
                    success: function(articles) {
                        for (let i in articles) {
                            let article = articles[i];
                            if (!favourites.includes(article.id)) {
                                favourites.push(article.id);
                                addFavouriteElement(article.id, article.title, article.websiteName, article.websiteUrl);
                            }
                        }
                    },
                    error: function() {
                        alert("Cannot get favourite articles");
                    }
                })
               
            }
        },
        error:function(){
            alert("Cannot find favourites");
        }
    });
}

function toggleFavourites(icon) {
    let container = $(FAVOURITES_SELECTOR + " ul");
    degrees = degrees === 0 ? 90 : 0;
    $(icon).css({'transform' : 'rotate('+ degrees +'deg)'});
    container.slideToggle();
}

function markFavourites() {
    favourites.forEach(function (id) {
        toggleIconSelected(id);
    });
}

function toggleIconSelected(id) {
    let icon = $('#' + id).children('.row').children().children('.fav-icon');
    if (icon.hasClass('icon-selected')) {
        icon.removeClass('icon-selected');
    } else {
        icon.addClass('icon-selected');
    }
}

function toggleFavourite(id) {
    if (!userLoggedIn) {
        alert('You need to be logged in to add favourites');
        return;
    }
    
    let icon = $('#' + id).children('.row').children().children('.fav-icon');
    if (icon.hasClass('icon-selected')) {
        removeFavourite(id);
    } else {
        addFavourite(id);
    }
}

function addFavourite(id) {
    if (!favourites.includes(id)) {
        favourites.push(id);
    } else {
        return;
    }

    $.ajax({
        type: 'POST',
        url: 'User/AddFavourite',
        data: { userId: userId, articleId: id },
        success: function(data) {
            if (data) {
                let [title, website, url] = getArticleData(id);
                addFavouriteElement(id, title, website, url);
            } else {
                alert("Cannot add favourite");
            }
        },
        error:function(){
            alert("Cannot add favourite");
        }
    });

}

function removeFavourite(id) {
    $.ajax({
        type: 'POST',
        url: 'User/RemoveFavourite',
        data: { userId: userId, articleId: id },
        success:function(data) {
            if (data) {
                if (favourites.includes(id)) {
                    favourites = favourites.filter(function (value, index, arr) {
                        return value !== id;
                    })
                }

                toggleIconSelected(id);

                let item = $(`[data-id="${id}"]`);
                item.fadeOut(250, function () {
                    this.remove();
                });
            } else {
                alert("Cannot remove favourite");
            }
        },
        error:function(){
            alert("Cannot remove favourite");
        }
    });

}   

function getArticleData(id) {
    let body = $('#' + id).children('.row').children();
    let title = body.children('a').children().text();
    let website = body.children('.card-subtitle').text();
    let url = body.children('a').attr('href');
    return [title, website, url];
}

function addFavouriteElement(id, title, website, url) {
    toggleIconSelected(id);

    let li = $('<li></li>', {
        'class': 'todo-done',
        'data-id': id
    });

    let icon = $('<div></div>', {
        'class': 'todo-icon fui-heart',
        'onclick': 'removeFavourite(' + id + ')'
    });

    let content = $('<a></a>', {
        'class': 'todo-content',
        'href': url,
        'target': '_blank'
    });

    let name = $('<h4></h4>',
        {'class': 'todo-name text-ellipsis'}
    );
    
    let tooltip = $('<a></a>',
        {'class': '',
        // 'href': '#',
        'data-toggle' : 'tooltip',
        'title': title}
    );

    tooltip.append(title);
    name.append(tooltip);
    content.append(name);
    content.append(website);
    li.append(icon);
    li.append(content);

    li.hide();
    li.fadeIn(150);
    $(FAVOURITES_SELECTOR + ' ul').append(li);
}
