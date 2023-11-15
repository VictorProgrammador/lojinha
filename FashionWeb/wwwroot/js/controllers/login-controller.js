app.controller("loginController", ["$scope", "$window", "$location", "basicService", function ($scope, $window, $location, basicService) {

    $scope.user = {};

    $scope.disableLogin = false;
    $scope.showPassword = false;

    $scope.changeShowPassword = function () {
        $scope.showPassword = !$scope.showPassword;
    }

    $scope.alerts = [];

    $scope.addSuccessAlert = function (message) {
        $scope.alerts.push({ type: 'success', msg: message });
    };

    $scope.addErrorAlert = function (message) {
        $scope.alerts.push({ type: 'danger', msg: message });
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.loginUser = function () {

        $scope.disableLogin = true;
        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        $scope.user.returnUrl = $location.absUrl();

        basicService.loginUser($scope.user).then(function (data) {

            if (data == undefined || data.data == undefined) {
                $scope.addErrorAlert("Erro! Sinto muito, falha ao realizar login!");
                $scope.disableLogin = false;
            }

            var result = data.data;

            if (result.success != undefined &&
                result.success != null &&
                result.success == true) {
                // Obtem a URL completa
                var url = $location.absUrl();

                //https
                var https = url.split('/')[0];

                // Extrai apenas o domínio
                var domain = url.split('/')[2];

                if (result.data.startsWith('http') ||
                    result.data.startsWith('https')) {
                    $window.location.href = https + "//" + domain;
                }
                else
                {
                    $window.location.href = https + "//" + domain + result.data;
                }
            }
            else
            {
                if (result.errorList != null && result.errorList.length > 0) {
                    result.errorList.map(function (item) {
                        $scope.addErrorAlert(item);
                    });
                }

                $scope.disableLogin = false;
            }

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $scope.addErrorAlert("Erro! não foi possível realizar login!");
            $scope.disableLogin = false;
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });
    }

}]);