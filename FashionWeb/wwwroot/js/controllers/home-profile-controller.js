app.controller("homeProfileController", ["$scope", "$sce", "$location", "$window", "basicService", "$filter", "utilidadesService", function ($scope, $sce, $location, $window, basicService, $filter, utilidadesService) {

    $scope.alerts = [];

    $scope.entity = {};

    $scope.addSuccessAlert = function (message) {
        $scope.alerts.push({ type: 'success', msg: message });
    };

    $scope.addErrorAlert = function (message) {
        $scope.alerts.push({ type: 'danger', msg: message });
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };


    $scope.loadProduct = function (Id) {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.getProduct(Id).then(function (data) {
            var result = data.data;

            if (result != null && result != undefined) {
                $scope.entity = result;
            }
            else {
                utilidadesService.exibirMensagem('Atenção!', 'Produto não foi encontrado!', false);
                $scope.addErrorAlert("Produto buscado não encontrada na base de dados!");
            }

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }

  
}]);