(function () {
    var app = angular.module('products-app', []);

    /**
     * Controller that manages the login information of an user
     */
    app.controller('ProductsController', ['$http', '$scope', function ($http, $scope) {

        var page = 0;
        $scope.getPage = function () {
            page = page + 1;
            //alert(page);
            angular.element("#add").hide();
            $http.post("/get-page", {page: page})
                .success(function (data) {
                    //console.log(data);

                    for (i = 0; i < data.length;i++) {
                        var elem = angular.element('<article class="search-result row" style="background-color: #222222"> <div class="col-xs-12 col-sm-12 col-md-3"> <a href="/product/' + data[i].CodigoArtigo + '" class="thumbnail"><img src="http://lorempixel.com/400/200/" alt="Lorem ipsum"/></a> </div> <div class="col-xs-12 col-sm-12 col-md-2"> <ul class="meta-search"> <li><i class="fa fa-android"></i> <span>' + data[i].NomeSistemaOperativo + '</span> </li> <li><i class="fa fa-mobile"></i> <span>' + data[i].TamanhoEcra + ' polegadas</span> </li> <li><i class="fa fa-mobile"></i> <span>' + data[i].StockAtual + ' in stock</span> </li> <li id="preco"><i class="fa fa-euro"></i> <span>' + data[i].PVP * (1 - (data[i].Desconto / 100)) + '</span> </li> </ul> </div> <div class="col-xs-12 col-sm-12 col-md-7 excerpet"> <h3><a href="/product/' + data[i].CodigoArtigo + '" title="">' + data[i].Nome + '</a></h3> <p>Processador: ' + data[i].CPU + '</p> <p>Armazenamento: ' + data[i].Armazenamento + '</p> <p>Data de Lançamento: ' + data[i].Lancamento + '</p> </div> <span class="clearfix borda"></span> </article>')
                    angular.element('#section').append(elem);
                    };
                    angular.element("#add").show();
                })
        };

    }]);

})();