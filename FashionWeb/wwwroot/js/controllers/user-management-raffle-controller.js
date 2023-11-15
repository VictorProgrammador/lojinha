app.controller("userManagementRaffleController", ["$scope", "$window", "$http", "$filter", "basicService", "utilidadesService", function ($scope, $window, $http, $filter, basicService, utilidadesService) {

    $scope.filter = {
        PageNumber: 1,
        PageSize: 5
    };

    $scope.paginantion = {
        totalItems: 4,
        totalPages: 0
    };

    $scope.participants = [];

    $scope.myCroppedImage = '';
    $scope.showImageCropped = false;
    $scope.file = null;

    $scope.entity = {};

    $scope.personBusiness = {

    };

    $scope.list = {
        raffles: []
    };

    $scope.setTab = function (newTab) {
        $scope.tab = newTab;
    };

    $scope.currentNavItem = 'page1';

    $scope.isSet = function (tabNum) {
        return $scope.tab === tabNum;
    };

    $scope.addRaffle = function () {
        $scope.openModal();
    }

    $scope.openModal = function () {
        $scope.entity = {};
        $scope.myImage = '';
        $scope.myCroppedImage = '';
        $scope.file = null;
        $scope.showImageCropped = false;
        document.getElementById('imageFile').value = null;

        $('#myModal').modal('show');
    }

    $scope.closeModal = function () {
        $scope.entity = {};
        $('#myModal').modal('hide');
    }

    const headers = {
        'Content-Type': 'application/json'
    };

    saveMyRaffle = function (param, headersProduct) {
        return $http.post("/User/SaveMyRaffle", param, headersProduct);
    }

    excluirSorteio = function (param) {
        return $http.post("/User/ExcluirSorteio", param, headers);
    }

    $scope.save_click = function () {
        if ($scope.entity.value != null && $scope.entity.value != undefined) {
            if (typeof $scope.entity.value === 'string') {
                var valorDecimal = $scope.entity.value.replace('.', '').replace(',', '.');
                valorDecimal = parseFloat(valorDecimal);
                $scope.entity.value = valorDecimal;
            }
        }

        $scope.entity.personBusinessId = $scope.personBusiness.id;

        var formData = new FormData($("#formElem")[0]);

        if (document.getElementById('imageFile').value == null ||
            document.getElementById('imageFile').value == undefined ||
            document.getElementById('imageFile').value.endsWith('jpg') ||
            document.getElementById('imageFile').value.endsWith('jpeg') ||
            document.getElementById('imageFile').value.endsWith('png')) {
            formData.append('file', $scope.myCroppedImage);
        }

        var raffle = $scope.entity;

        formData.append('raffle', JSON.stringify(raffle));

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        saveMyRaffle(formData, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        }).then(function (data) {
            var result = data.data;

            $scope.closeModal();

            if (result == true) {
                utilidadesService.exibirMensagem('Sucesso!', 'Ação realizada com sucesso. Se tiver subido imagem, atualize sua tela para ver ela no produto!', true);
            }
            else {
                utilidadesService.exibirMensagem('Falha', 'Falha ao realizar essa ação.', true);
            }

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

            $window.location.reload();
        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });
    }

    $scope.getMyRaffles = function () {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.getMyRaffles($scope.filter).then(function (data) {
            var result = data.data;

            if (result != null && result != undefined)
                $scope.personBusiness = result;

            if ($scope.personBusiness != null &&
                $scope.personBusiness.raffles != null &&
                $scope.personBusiness.raffles.results.length > 0) {
                $scope.list.raffles = $scope.personBusiness.raffles.results;
                $scope.paginantion.totalItems = $scope.personBusiness.raffles.totalResults;
                $scope.filter.PageNumber = $scope.personBusiness.raffles.pageNumber;
                $scope.paginantion.totalPages = $scope.personBusiness.raffles.totalPages;
            }
            else {
                $scope.list.raffles = [];
                $scope.filter.PageNumber = 1;
                $scope.paginantion.totalItems = 0;
                $scope.paginantion.totalPages = 0;
            }


            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });
    }

    $scope.editRaffle = function (item) {
        $scope.openModal();
        $scope.entity = item;
    }

    $scope.getRaffleParticipants = function (raffle) {

        $scope.openParticipants();
        $scope.entity = raffle;

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.getRaffleParticipants($scope.entity).then(function (data) {
            var result = data.data;
            console.log('resultado', result);

            $scope.participants = result;

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });
    }

    $scope.viewParticipants = function (raffle) {
        $scope.getRaffleParticipants(raffle);
    }

    $scope.openParticipants = function () {
        $scope.participants = [];
        $('#modalParticipants').modal('show');
    }

    $scope.closeModalParticipants = function () {
        $scope.participants = [];
        $scope.entity = {};
        $('#modalParticipants').modal('hide');
    }

    $scope.excludeRaffle = function (item) {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        excluirSorteio(item).then(function (data) {
            var result = data.data;

            if (result == true) {
                utilidadesService.exibirMensagem('Sucesso!', 'Sorteio excluído com sucesso.', true);
            }
            else {
                utilidadesService.exibirMensagem('Falha', 'Falha ao excluir o sorteio.', true);
            }

            $scope.getMyRaffles();

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }

    $scope.showImageToCropped = function () {
        $scope.showImageCropped = true;
        utilidadesService.exibirMensagem('Imagem carregada', 'Se sua imagem tiver aparecido em branco, entre em contato com o suporte.', true);
    }

    $scope.imageLoadError = function () {
        utilidadesService.exibirMensagem('Erro no processamento', 'Essa imagem não está em um formato permitido. Entre em contato com o suporte ou tente uma imagem diferente.', true);
    }

    var handleFileSelect = function (evt) {
        var file = evt.currentTarget.files[0];
        $scope.file = file;
        var reader = new FileReader();
        reader.onload = function (evt) {
            $scope.$apply(function ($scope) {
                $scope.myImage = evt.target.result;
            });
        };
        reader.readAsDataURL(file);
    };

    angular.element(document.querySelector('#imageFile')).on('change', handleFileSelect);

    $scope.init = function () {
        $scope.getMyRaffles();
    }

    $scope.init();

}]);