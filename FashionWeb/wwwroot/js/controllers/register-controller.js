app.controller("registerController", ["$scope", "basicService", function ($scope, basicService) {

    $scope.user = {};

    $scope.disableRegister = false;

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

    $scope.registerUser = function () {

        if ($scope.user.password != $scope.user.confirmPassword) {
            $scope.addErrorAlert("As senhas devem ser idênticas!");
            return false;
        }

        $scope.disableRegister = true;
        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.registerUser($scope.user).then(function (data) {

            if (data == undefined || data.data == undefined) {
                $scope.addErrorAlert("Erro! Sinto muito, não conseguimos cadastrar sua conta!");
                $scope.disableRegister = false;
            }

            var result = data.data;

            if (result == true)
                $scope.addSuccessAlert("Sucesso! Seus dados de acesso foram enviados ao seu e-mail!");
            else {
                $scope.addErrorAlert("Erro! Sinto muito, não conseguimos cadastrar sua conta!");

                if (result.errorList != null && result.errorList.length > 0) {
                    result.errorList.map(function (item) {
                        $scope.addErrorAlert(item);
                    });
                }

                $scope.disableRegister = false;
            }

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $scope.addErrorAlert("Erro! Sinto muito, não conseguimos cadastrar sua conta!");
            $scope.disableRegister = false;
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });
    }

}]);