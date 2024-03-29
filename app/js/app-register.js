(function () {
    var app = angular.module('register-app', []);

    /**
     * Controller that manages the register information of an user
     */
    app.controller('RegisterController', ['$scope', '$http', '$window', 'Register', function ($scope, $http, $window, Register) {

        $scope.formData = {};
        $scope.loading = true;

        $scope.register = function () {
            $scope.loading = true;
            if ($scope.formData != undefined) {
                console.log($scope.formData);
                Register.create($scope.formData)
                    .success(function () {
                        $scope.loading = false;
                        $scope.formData = {};
                        $window.location.href = '/login';
                    })
                    .error(function (data) {
                        alert("failed");
                    });
            }
        };
    }]);

    /**
     * Service that attempts to register with the provided information
     */
    app.factory('Register', ['$http', function ($http) {
        return{
            create: function (user) {
                return $http.post('/register', user);
            }
        }
    }]);
})();
