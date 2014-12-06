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

        res.render("product.ejs", {messages: messages, title: "Product"});
    });

    app.get('/register', function (req, res) {
        var messages = generateMessageBlock();
        res.render("register.ejs", {messages: messages, title: "Registo"});
    });

    app.post('/register', function (req, res) {
        console.log("começou");
        if (req.body.password === req.body.confirmPassword) {
            console.log("entrou");
            requestify.put('http://localhost:49445/api/clients', {NumContribuinte: req.body.nib, Nome: req.body.nome, Email: req.body.email, Telefone: req.body.telefone, Morada: req.body.morada, Localidade: req.body.localidade, CodPostal: req.body.codPostal, Password: req.body.password})
                .then(function (response) {
                    console.log("pedido");
                    console.log(response.getBody());
                });
        } else {
            console.log("peido");
            res.status(400).send("Passwords must match!");
        }
    });


    app.post('/login', function (req, res) {
        if (req.body.email != "" && req.body.password != "") {

            requestify.post('http://localhost:49445/api/sessions', {
                email   : req.body.email,
                password: req.body.password
            })
                .then(function (response) {
                    if (response.getCode() == 200) {
                        console.log(response.getBody());
                        req.session.user = response.getBody();
                    } else {
                        console.log("Status: " + response.getCode() + " Bad request");
                    }
                });

        }
        res.send(200);
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
        info   : [],
        warning: [],
        danger : []
    };
}
