var path = require('path');

exports.listen = function (app) {

    app.get('/logout', function (req, res) {
        var messages = generateMessageBlock();
        // destroy the user's session to log them out
        // will be re-created next request
        if (req.session.user) {
            req.session.destroy(function () {
                messages.success.push({title: "Logged Out", content: "You are now logged out!"});
                res.render("teste.ejs", {messages: messages, title: 'Landing'});
            });
        } else {
            messages.success.push({title: "Sign in first", content: "You are not logged in"});
            res.render("teste.ejs", {messages: messages, title: 'Landing'});
        }
    });

    app.get('*', function (req, res) {
       res.render("teste.ejs");
    });
};
