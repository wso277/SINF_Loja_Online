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
                res.render("dashboard-public.ejs", {messages: messages, title: 'Dashboard'});
            });
        } else {
            messages.success.push({title: "Sign in first", content: "You are not logged in"});
            res.render("dashboard-public.ejs", {messages: messages, title: 'Dashboard'});
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
            requestify.request('http://localhost:49445/api/encomendas', {method: 'GET', params: {CodigoCliente: 'CL000001'},dataType: 'form-url-encoded'})
                .then(function (response) {
                    if (response.getCode() == "200") {
                        var orders = response.getBody();
                        var total = 0;
                        for (var i = 0; i < orders.length; i++) {
                            for (var j = 0; j < orders[i]['LinhasEncomendaExtended'].length; j++) {
                                console.log(orders[i]['LinhasEncomendaExtended'][j]['TotalLiquido']);
                                total += orders[i]['LinhasEncomendaExtended'][j]['TotalLiquido'] * (1- (orders[i]['LinhasEncomendaExtended'][j]['Desconto'] / 100));
                            }
                            orders[i]['Total'] = total;
                            total = 0;
                        }
                        console.log(orders);
                        res.render("orders.ejs", {messages: messages, title: 'Encomendas', orders: orders});
                    } else {
                        console.log("coco");
                    }
                });
        } else {
        }
    });

    app.get('/order/:id', function (req, res) {
        var messages = generateMessageBlock();
        var id = parseInt(req.params.id);
        if (req.session.user) {
            requestify.request('http://localhost:49445/api/encomendas', {method: 'GET', params: id,dataType: 'form-url-encoded'})
                .then(function (response) {
                    if (response.getCode() == "200") {
                        var order = response.getBody();
                        console.log(order);
                        res.render("order.ejs", {messages: messages, title: 'Encomenda', order: order});
                    } else {
                        console.log("coco");
                    }
                });
        } else {
        }
    });

    app.get('/products', function (req, res) {
        var messages = generateMessageBlock();
        var produtos = {};

        requestify.request('http://localhost:49445/api/artigos', {method: 'GET', body: {page: 0}, dataType: 'form-url-encoded'})
            .then(function (response) {
                if (response.getCode() == "200") {
                    produtos = response.getBody();
                    console.log(produtos);
                    if (req.session.user) {
                        res.render("products.ejs", {messages: messages, title: 'Produtos', products: produtos});
                    } else {
                        res.render("products.ejs", {messages: messages, title: 'Produtos',  products: produtos});
                    }
                } else {
                    console.log("coco");
                }
            });
    });

    app.get('/profile', function (req, res) {
        var messages = generateMessageBlock();
        if (req.session.user) {
            res.render("profile.ejs", {messages: messages, title: 'Profile'});
        } else {
        }
    });

    app.get('/product/:id', function (req, res) {
        var messages = generateMessageBlock();

        requestify.request('http://localhost:49445/api/artigos/'+req.params.id, {method: 'GET', dataType: 'form-url-encoded'})
            .then(function (response) {
                if (response.getCode() == "200") {
                    var produto = response.getBody();
                    console.log(produto);
                    console.log(response.getHeaders());
                    if (req.session.user) {
                        res.render("product.ejs", {messages: messages, title: 'Produto', product: produto});
                    } else {
                        res.render("product.ejs", {messages: messages, title: 'Produto',  product: produto});
                    }
                } else {
                    console.log("coco");
                }
            });
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

    app.get('/:val', function (req, res) {
        var messages = generateMessageBlock();
        if (req.params.val == "logged-in") {
            messages.success.push({title: "Logged In", content: "You are now logged in!"});
            res.render("dashboard-private", {title: "Dashboard", messages: messages});
        }
    });

    app.get('*', function (req, res) {
        var messages = generateMessageBlock();
        if (req.session.user) {
            res.render("dashboard-private.ejs", {title: "Dashboard", messages: messages});
        } else {
            res.render("dashboard-public.ejs", {title: "Dashboard", messages: messages});
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
