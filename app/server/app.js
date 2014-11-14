var express = require('express');
var session = require('express-session');
var cookieParser = require('cookie-parser');
var path = require('path');

var app = express();

app.set("views", __dirname + '/../views');
app.set('view engine', 'ejs');

app.listen(8080, "127.0.0.1", function () {
        console.log("Listening");

    }
);
