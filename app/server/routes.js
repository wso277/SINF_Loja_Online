var path = require('path');
var requestify = require('requestify');

exports.listen = function (app) {

    app.get('/logout', function (req, res) {
        var messages = generateMessageBlock();
        // destroy the user's session to log them out
        // will be re-created next request
        if (req.session.user) {
            req.session.destroy(function () {
                messages.success.push({title: "Autentique-se primeiro", content: "Não está autenticado"});
                res.render("dashboard-public.ejs", {messages: messages, title: 'Dashboard'});
            });
        } else {
            messages.success.push({title: "Autentique-se primeiro", content: "Não está autenticado"});
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
            requestify.request('http://localhost:49445/api/encomendas', {
                method: 'GET',
                params: {CodigoCliente: req.session.user.CodigoCliente},
                dataType: 'form-url-encoded'
            })
                .then(function (response) {
                    if (response.getCode() == "200") {
                        var orders = response.getBody();
                        var total = 0;
                        for (var i = 0; i < orders.length; i++) {
                            for (var j = 0; j < orders[i]['LinhasEncomendaExtended'].length; j++) {
                                console.log(orders[i]['LinhasEncomendaExtended'][j]['TotalLiquido']);
                                total += orders[i]['LinhasEncomendaExtended'][j]['TotalLiquido'] * (1 - (orders[i]['LinhasEncomendaExtended'][j]['Desconto'] / 100));
                            }
                            orders[i]['Total'] = total;
                            total = 0;
                        }
                        console.log(orders);
                        res.render("orders.ejs", {messages: messages, title: 'Encomendas', orders: orders, user: req.session.user, cart: req.session.shoppingCart});
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
            requestify.request('http://localhost:49445/api/encomendas/' + id, {
                method: 'GET',
                dataType: 'form-url-encoded'
            })
                .then(function (response) {
                    if (response.getCode() == "200") {
                        var order = response.getBody();
                        console.log(order);
                        res.render("order.ejs", {messages: messages, title: 'Encomenda', order: order, user: req.session.user, cart: req.session.shoppingCart});
                    } else {
                        console.log("coco");
                    }
                });
        } else {
        }
    });

    app.get('/sales', function (req, res) {
        var messages = generateMessageBlock();
        var produtos = {};

        requestify.request('http://localhost:49445/api/artigos', {
            method: 'GET',
            params: {page: 0, promocao: true},
            dataType: 'form-url-encoded'
        })
            .then(function (response) {
                if (response.getCode() == "200") {
                    produtos = response.getBody();
                    if (req.session.user) {
                        res.render("sales.ejs", {messages: messages, title: 'Produtos', products: produtos, user: req.session.user, cart: req.session.shoppingCart});
                    } else {
                        res.render("sales.ejs", {messages: messages, title: 'Produtos', products: produtos, user: null, cart: null});
                    }
                } else {
                    console.log("coco");
                }
            });
    });

    app.get('/products', function (req, res) {
        var messages = generateMessageBlock();
        var produtos = {};

        requestify.request('http://localhost:49445/api/artigos', {
            method: 'GET',
            params: {page: 0},
            dataType: 'form-url-encoded'
        })
            .then(function (response) {
                if (response.getCode() == "200") {
                    produtos = response.getBody();
                    if (req.session.user) {
                        res.render("products.ejs", {messages: messages, title: 'Produtos', products: produtos, user: req.session.user, cart: req.session.shoppingCart});
                    } else {
                        res.render("products.ejs", {messages: messages, title: 'Produtos', products: produtos, user: null, cart: null});
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

    app.get('/finalize-order', function(req,res) {
        var messages = generateMessageBlock();
        if (req.session.user) {
            var orderLines = [];
            for (var i = 0; i < req.session.shoppingCart['products'].length; i++) {
                orderLines.push({CodigoArtigo: req.session.shoppingCart['products'][i]['CodigoArtigo'], Quantidade: req.session.shoppingCart['products'][i]['quantidade']});
            }
            var order = {Entidade: req.session.user.CodigoCliente, LinhasEncomenda: orderLines};
            console.log(order);
            requestify.request('http://localhost:49445/api/encomendas', {
                method: 'PUT',
                body: order,
                dataType: 'json'
            })
                .then(function (response) {
                    if (response.getCode() == "201") {
                        messages.success.push({title: "Sucesso", content: "Encomenda criada com sucesso"});
                        res.redirect('/orders');
                    } else {
                        console.log("error");
                    }
                });
        } else {

        }
    });

    app.post('/get-page', function (req, res) {
        console.log(req.body.page);
        if (req.body.promocao) {
            requestify.request('http://localhost:49445/api/artigos', {
                method: 'GET',
                params: {page: req.body.page, promocao: req.body.promocao},
                dataType: 'form-url-encoded'
            })
                .then(function (response) {
                    if (response.getCode() == "200") {
                        produtos = response.getBody();
                        console.log(produtos);
                        res.status(200).send(produtos);
                    } else {
                        console.log("coco");
                    }
                });
        } else {
            requestify.request('http://localhost:49445/api/artigos', {
                method: 'GET',
                params: {page: req.body.page},
                dataType: 'form-url-encoded'
            })
                .then(function (response) {
                    if (response.getCode() == "200") {
                        produtos = response.getBody();
                        console.log(produtos);
                        res.status(200).send(produtos);
                    } else {
                        console.log("coco");
                    }
                });
        }

    });

    app.get('/product/:id', function (req, res) {
        var messages = generateMessageBlock();

        requestify.request('http://localhost:49445/api/artigo/' + req.params.id, {
            method: 'GET',
            dataType: 'form-url-encoded'
        })
            .then(function (response) {
                if (response.getCode() == "200") {
                    var produto = response.getBody();
                    if (req.session.user) {
                        res.render("product.ejs", {messages: messages, title: 'Produto', product: produto, user: req.session.user, cart: req.session.shoppingCart});
                    } else {
                        res.render("product.ejs", {messages: messages, title: 'Produto', product: produto, user: null, cart: null});
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
            requestify.request('http://localhost:49445/api/clients', {
                method: 'PUT',
                body: {
                    NumContribuinte: req.body.nib,
                    Nome: req.body.nome,
                    Email: req.body.email,
                    Telefone: req.body.telefone,
                    Morada: req.body.morada,
                    Localidade: req.body.localidade,
                    CodPostal: req.body.codPostal,
                    Password: req.body.password
                },
                dataType: 'form-url-encoded'
            })
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

    app.get('/remove-order/:id', function (req,res) {
        var messages = generateMessageBlock();
        if (req.session.user) {
            for (var i = 0; i < req.session.shoppingCart['products'].length; i++) {
                if (req.params.id == req.session.shoppingCart['products'][i]['CodigoArtigo']) {
                    var products = req.session.shoppingCart['products'].splice(i, 1);
                    req.session.shoppingCart['products'] = products;
                    console.log(req.session.shoppingCart);
                    break;
                }
            }
            messages.success.push({title: "Sucesso", content: "Produto removido do carrinho com sucesso"});
            res.redirect('/products');

        } else {
            messages.success.push({title: "Autentique-se primeiro", content: "Não está autenticado"});
            res.render("dashboard-public.ejs", {messages: messages, title: 'Dashboard'});
        }

    });

    app.post('/add-to-cart', function (req, res) {
        var messages = generateMessageBlock();
        var hasAmount = false;
        if (req.session.user) {
            requestify.request('http://localhost:49445/api/artigo/' + req.body.id, {method: 'GET', dataType: 'form-url-encoded'})
                .then(function (response) {
                    if (response.getCode() == "200") {
                        for (var i = 0; i < req.session.shoppingCart['products'].length; i++) {
                            if (response.getBody().CodigoArtigo == req.session.shoppingCart['products'][i].CodigoArtigo) {
                                req.session.shoppingCart['products'][i]['quantidade'] += parseInt(req.body.nUnits);
                                hasAmount = true;
                                break;
                            }
                        }
                        if (!hasAmount) {
                            var product = {CodigoArtigo: response.getBody()['CodigoArtigo'], Nome: response.getBody()['Nome'], Marca: response.getBody()['Marca'], PVP:
                                response.getBody()['PVP'], Desconto: response.getBody()['Desconto'], fotoURL: response.getBody()['fotoURL']};
                            product['quantidade'] = parseInt(req.body.nUnits);
                            req.session.shoppingCart['products'].push(product);
                            console.log(req.session.shoppingCart);
                        }
                        for (var i = 0; i < req.session.shoppingCart['products'].length; i++) {
                            req.session.shoppingCart['totalItems'] += req.session.shoppingCart['products'][i]['quantidade'];
                            req.session.shoppingCart['total'] += (req.session.shoppingCart['products'][i]['PVP'] * (1 - (req.session.shoppingCart['products'][i]['Desconto'] / 100)));
                        }
                        req.session.shoppingCart['total'].toFixed(2);
                        console.log(req.session.shoppingCart);
                        hasAmount = false;
                        res.status(200).send(true);
                    } else {
                        res.status(400).send(false);
                    }
                });
        } else {
            messages.success.push({title: "Autentique-se primeiro", content: "Não está autenticado"});
            res.render("dashboard-public.ejs", {messages: messages, title: 'Dashboard'});
        }
    });

    app.post('/login', function (req, res) {
        if (req.body.email != "" && req.body.password != "") {
            requestify.request('http://localhost:49445/api/sessions', {
                method: 'POST', body: {
                    email: req.body.email,
                    password: req.body.password
                }, dataType: 'form-url-encoded'
            })
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

    app.get('/:val', function (req, res) {
        var messages = generateMessageBlock();
        if (req.params.val == "logged-in") {
            if (req.session.user) {
                messages.success.push({title: "Sucesso", content: "Está agora auntenticado!"});
                messages.success.push({title: "Bem-vindo", content: ""});
                requestify.request('http://localhost:49445/api/clients/' + req.session.user.CodigoCliente, {
                    method: 'GET',
                    dataType: 'form-url-encoded'
                })
                    .then(function (response) {
                        if (response.getCode() == "200") {
                            req.session.user = response.getBody();
                            req.session.shoppingCart = {products: [], totalItems: 0, total: 0};
                            console.log(req.session.shoppingCart);
                            console.log(req.session.shoppingCart['products']);
                            res.render("dashboard-private", {title: "Dashboard", messages: messages, user: req.session.user, cart: req.session.shoppingCart});
                        } else {
                        }
                    });
            } else {
                messages.success.push({title: "Autentique-se primeiro", content: "Não está autenticado"});
                res.render("dashboard-public.ejs", {messages: messages, title: 'Dashboard'});
            }
        }
    });

    app.get('*', function (req, res) {
        var messages = generateMessageBlock();
        if (req.session.user) {
            res.render("dashboard-private.ejs", {title: "Dashboard", messages: messages, user: req.session.user, cart: req.session.shoppingCart});
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
