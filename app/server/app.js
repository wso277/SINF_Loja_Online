var express = require('express');
var session = require('express-session');
var cookieParser = require('cookie-parser');
var path = require('path');
var morgan = require('morgan');
var routes = require('./routes.js');

var app = express();
var port = 8080;
var ip = "127.0.0.1";

app.set("views", __dirname + '/../views');
app.set('view engine', 'ejs');
app.use("/css", express.static(path.join(__dirname, '../css')));
app.use("/images", express.static(path.join(__dirname, '../images')));
app.use("/js", express.static(path.join(__dirname, '/../js')));
app.use("/fonts", express.static(path.join(__dirname, '../fonts')));
app.use(morgan('dev'));

app.use(cookieParser('shhhh, very very very secretzzzzz'));
app.use(session({
    secret: 'shhhh, very very very secretzzzzz',
    resave: true,
    saveUninitialized: true
}));

routes.listen(app);

app.listen(port, ip, function () {
        console.log("Listening");
    }
);
