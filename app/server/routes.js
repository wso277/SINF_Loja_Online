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

    app.get('/login', function (req, res) {
        var messages = generateMessageBlock();
        if (req.session.user) {

        } else {
            res.render("login.ejs", {messages: messages, title: 'Login'});
        }
    });

    app.get('/orders', function (req, res) {
        var messages = generateMessageBlock();
        if (req.session.user) {

        } else {
            res.render("orders.ejs", {messages: messages, title: 'Orders'});
        }
    });

    app.get('/products', function (req, res) {
        var messages = generateMessageBlock();
        if (req.session.user) {

        } else {
            res.render("products.ejs", {messages: messages, title: 'Products'});
        }
    });

    app.get('/profile', function (req, res) {
        var messages = generateMessageBlock();
        if (req.session.user) {

        } else {
            res.render("profile.ejs", {messages: messages, title: 'Profile'});
        }
    });

    app.get('/product/:id', function (req, res) {
        var messages = generateMessageBlock();
        var id = parseInt(req.params.id);

        res.render("product.ejs", {messages: messages, title:"Product"});
    });

    app.post('/login', function (req,res) {
        res.send(200);
    });

    app.get('/teste-erro', function (req, res) {
        res.render("teste-erro.ejs");
    });

    app.get('*', function (req, res) {
        if (req.session.user) {
            res.render("dashboard-private.ejs", {title:"Dashboard"});
        } else {
            res.render("dashboard-public.ejs", {title: "Dashboard"});
        }
    });
};

var generateMessageBlock = function() {
    return {
        success: [],
        info  : [],
        warning: [],
        danger: []
    };
}
