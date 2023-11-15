app.controller("RecoverPasswordController", ["$scope", "basicService", function ($scope, basicService) {

    $scope.email = "";
    $scope.disableRecover = false;

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

    $scope.recoverPassword = function () {

        if ($scope.email == undefined || $scope.email == '') {
            $(".emailInput").addClass('border');
            $(".emailInput").addClass('border-danger');
            return false;
        }

        $(".emailInput").removeClass('border');
        $(".emailInput").removeClass('border-danger');

        $scope.disableRecover = true;
        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');


        let user = {
            username: $scope.email
        };

        basicService.sendPassword(user).then(function (data) {

            if (data == undefined || data.data == undefined) {
                $scope.addErrorAlert("Erro! Sinto muito, não conseguimos enviar sua senha!.");
                $scope.disableRecover = false;
            }

            if (data.data == true) {
                $scope.addSuccessAlert("Sucesso! A sua senha foi enviada no seu email.");
            }
            else {
                $scope.addErrorAlert("Erro! Sinto muito, não conseguimos enviar sua senha!.");
                $scope.disableRecover = false;
            }

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $scope.addErrorAlert("Erro! Sinto muito, não conseguimos enviar sua senha!.");
            $scope.disableRecover = false;
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });
    }

}]);