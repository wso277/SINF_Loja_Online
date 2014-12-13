(function () {
    var app = angular.module('product-app', []);

    /**
     * Controller that manages the shopping cart
     */
    app.controller('ProductController', ['$scope', '$http', '$window', 'Product', function ($scope, $http, $window, Product) {

        $scope.loading = true;

        var id = angular.element($('input[name=id]')).val();

        $scope.addToCart = function () {
            $scope.loading = true;
                Product.addToCart(id)
                    .success(function () {
                        $scope.loading = false;
                        $window.location.reload();
                        angular.element('.alert-group').append('<div class="alert alert-info"> <strong></strong>' + "Produto adicionado ao carrinho com sucesso" + '</div>');
                    })
                    .error(function (data) {
                        alert("failed");
                    });
        };
    }]);

    /**
     * Service that attempts to add the product to the shopping cart
     */
    app.factory('Product', ['$http', function ($http) {
        return{
            addToCart: function (id) {
                return $http.post('/add-to-cart', {id: id});
            }
        }
    }]);
})();