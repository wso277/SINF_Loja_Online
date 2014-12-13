(function () {
    var app = angular.module('product-app', []);

    /**
     * Controller that manages the shopping cart
     */
    app.controller('ProductController', ['$scope', '$http', '$window', 'Product', function ($scope, $http, $window, Product) {

        $scope.formData = {};
        $scope.loading = true;

        var id = angular.element($('input[name=id]')).val();
        alert(id);

        $scope.addToCart = function () {
            $scope.loading = true;
            if ($scope.formData != undefined) {
                Product.addToCart(id)
                    .success(function () {
                        $scope.loading = false;
                        $scope.formData = {};
                        $window.location.reload();
                    })
                    .error(function (data) {
                        alert("failed");
                    });
            }
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