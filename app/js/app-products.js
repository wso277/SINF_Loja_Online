(function () {
    var app = angular.module('products-app', []);

    /**
     * Controller that manages the login information of an user
     */
    app.controller('ProductsController', ['$http', '$scope', function ($http, $scope) {

        var page = 0;
        $scope.getPage = function () {
            alert("cenas");
            page = page + 1;

            $http.post("/get-page", {page: page})
                .success(function (data) {
                    alert("success");
                    alert(data);
                    data.produtos.foreach(function (product) {
                        angular.element('<article class="search-result row" style="background-color: #222222"> <div class="col-xs-12 col-sm-12 col-md-3"> <a href="/product/' + product.CodigoArtigo + '" class="thumbnail"><img src="http://lorempixel.com/400/200/" alt="Lorem ipsum"/></a> </div> <div class="col-xs-12 col-sm-12 col-md-2"> <ul class="meta-search"> <li><i class="fa fa-android"></i> <span>' + product.NomeSistemaOperativo + '</span> </li> <li><i class="fa fa-mobile"></i> <span>' + product.TamanhoEcra + ' polegadas</span> </li> <li><i class="fa fa-mobile"></i> <span>' + product.StockAtual + ' in stock</span> </li> <li id="preco"><i class="fa fa-euro"></i> <span>' + product.PVP * (1 - (product.Desconto / 100)) + '</span> </li> </ul> </div> <div class="col-xs-12 col-sm-12 col-md-7 excerpet"> <h3><a href="/product/' + product.CodigoArtigo + '" title="">' + product.Nome + '</a></h3> <p>Processador: ' + product.CPU + '</p> <p>Armazenamento: ' + product.Armazenamento + '</p> <p>Data de Lançamento: ' + product.Lancamento + '</p> </div> <span class="clearfix borda"></span> </article>')
                    });
                })
        };

    }]);

})();