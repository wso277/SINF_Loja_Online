var path = require('path');
var requestify = require('requestify');

exports.listen = function (app) {

    app.get('/logout', function (req, res) {
        var messages = generateMessageBlock();
        // destroy the user's session to log them out
        // will be re-created next request
        if (req.session.user) {
            req.session.destroy(function () {
                messages.success.push({title: "Logged Out", content: "You are now logged out!"});
                res.render("dashboard-private.ejs", {messages: messages, title: 'Dashboard'});
            });
        } else {
            messages.success.push({title: "Sign in first", content: "You are not logged in"});
            res.render("dashboard-private.ejs", {messages: messages, title: 'Dashboard'});
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

        requestify.request('http://localhost:49445/api/artigos', {method: 'GET'})
            .then(function (response) {
                console.log(request.getBody());
            });

        if (req.session.user) {
            res.render("products.ejs", {messages: messages, title: 'Products'});
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

        res.render("product.ejs", {messages: messages, title: "Product"});
    });

    app.get('/register', function (req, res) {
        var messages = generateMessageBlock();
        res.render("register.ejs", {messages: messages, title: "Registo"});
    });

    app.post('/register', function (req, res) {
        if (req.body.password === req.body.confirmPassword) {
            requestify.request('http://localhost:49445/api/clients', {method: 'PUT', body: {NumContribuinte: req.body.nib, Nome: req.body.nome, Email: req.body.email, Telefone: req.body.telefone, Morada: req.body.morada, Localidade: req.body.localidade, CodPostal: req.body.codPostal, Password: req.body.password}, dataType: 'form-url-encoded'})
                .then(function (response) {
                    if (response.getCode() == "201") {
                        res.status(200).send(true);
                    } else {
                        res.status(400).send(false);
                    }
                });
        } else {
            console.log("peido");
            res.status(400).send("Passwords must match!");
        }
    });


    app.post('/login', function (req, res) {
        if (req.body.email != "" && req.body.password != "") {
            requestify.request('http://localhost:49445/api/sessions', { method: 'POST', body: {
                email: req.body.email,
                password: req.body.password
            }, dataType: 'form-url-encoded'})
                .then(function (response) {
                    if (response.getCode() == "200") {
                        req.session.regenerate(function () {
                            req.session.user = response.getBody();
                            console.log(req.session.user);
                            res.status(200).send(true);
                        });
                    } else {
                        res.status(400).send(false);
                    }
                });

        }
    });

    app.get('/teste-erro', function (req, res) {
        res.render("teste-erro.ejs");
    });

    app.get('*', function (req, res) {
        if (req.session.user) {
            res.render("dashboard-private.ejs", {title: "Dashboard"});
        } else {
            res.render("dashboard-public.ejs", {title: "Dashboard"});
        }
    });
};

var generateMessageBlock = function () {
    return {
        success: [],
        info: [],
        warning: [],
        danger: []
    };
}
