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

    $scope.previewImages = [];
    $scope.previewSelected = '';
    $scope.previewSelectedImage = '';

    $scope.quantidadeDisponivel = 0;

    $scope.trustSrc = function (src) {
        return $sce.trustAsResourceUrl(src);
    }

    $scope.getProductArchives = function () {
        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.getProductArchives($scope.entity).then(function (data) {
            var result = data.data;

            result.map(function (data) {
                $scope.previewImages.push($scope.trustSrc(data.url));
            });

            $scope.$apply();

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });
    }

    $scope.encodeURI = function (url) {
        return encodeURI(url);
    };

    $scope.changePreviewImage = function (image) {
        $scope.previewSelected = image;
    }

    $scope.loadProduct = function (Id) {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.getProduct(Id).then(function (data) {
            var result = data.data;

            if (result != null && result != undefined) {
                $scope.entity = result;

                $scope.previewImages.push('/' + $scope.entity.image);
                $scope.previewSelected = '/' + $scope.entity.image;
                $scope.getProductArchives();
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

    $scope.changeCor = function () {
        $scope.entity.productTamanhos = [];
        $scope.entity.tamanhoId = null;
        $scope.quantidadeDisponivel = 0;

        $scope.entity.productConfigs.map(function (data) {

            if (data.corId == $scope.entity.colorId) {
                $scope.entity.productTamanhos.push(data.tamanho);
            }

        });

    }

    $scope.changeTamanho = function () {
        $scope.entity.productConfigs.map(function (data) {

            if (data.corId == $scope.entity.colorId &&
                data.tamanhoId == $scope.entity.tamanhoId) {
                $scope.quantidadeDisponivel = data.quantidade;
            }

        });
    }

    $scope.adicionarAoCarrinho = function() {

        if ($scope.entity.colorId == null || $scope.entity.colorId == 0) {
            utilidadesService.exibirMensagem('Atenção!', 'Selecione uma cor!', false);
            return false;
        }

        if ($scope.entity.tamanhoId == null || $scope.entity.tamanhoId == 0) {
            utilidadesService.exibirMensagem('Atenção!', 'Selecione um tamanho!', false);
            return false;
        }

        fbq('track', 'AddToCart', {
            value: $scope.entity.value,
            currency: 'BRL'
        });

        var formulario = document.getElementById("meuFormulario");
        formulario.submit();


    }

  
}]);