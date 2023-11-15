app.controller("userGalleryController", ["$scope", "$sce", "$window", "$http", "$filter", "basicService", "utilidadesService", function ($scope, $sce, $window, $http, $filter, basicService, utilidadesService) {

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
        archives: []
    };

    $scope.setTab = function (newTab) {
        $scope.tab = newTab;
    };

    $scope.currentNavItem = 'page1';

    $scope.isSet = function (tabNum) {
        return $scope.tab === tabNum;
    };

    $scope.addArchive = function () {
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

    saveMyArchive = function (param, headers) {
        return $http.post("/User/SaveMyPersonBusinessArchive", param, headers);
    }

    saveMyVideo = function (param, headers) {
        return $http.post("/User/SavePersonBusinesVideo", param, headers);
    }

    excluirArchive = function (param) {
        return $http.post("/User/ExcluirPersonBusinessArchive", param, headers);
    }

    $scope.save_click = function () {
        $scope.entity.personBusinessId = $scope.personBusiness.id;

        var formData = new FormData($("#formElem")[0]);

        if ($scope.isImage == true &&
            (document.getElementById('imageFile').value == null ||
                document.getElementById('imageFile').value == undefined ||
                document.getElementById('imageFile').value.endsWith('jpg') ||
                document.getElementById('imageFile').value.endsWith('jpeg') ||
                document.getElementById('imageFile').value.endsWith('png'))) {
            formData.append('file', $scope.myCroppedImage);

            if ($scope.myCroppedImage == null || $scope.myCroppedImage == '')
                return false;
        }
        else
        {
            const inputElement = document.getElementById('videoFile');
            const file = inputElement.files[0];

            if (file) {
                formData.append('videoFile', file);
            }
            else
                return false;
        }

        var archive = $scope.entity;
        formData.append('personBusinessArchive', JSON.stringify(archive));

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        var saveFunc = saveMyArchive;

        if ($scope.isImage == null || $scope.isImage == false)
            saveFunc = saveMyVideo;

        saveFunc(formData, {
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

    $scope.trustSrc = function (src) {
        return $sce.trustAsResourceUrl(src);
    }

    $scope.getMyArchives = function () {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.getMyArchives($scope.filter).then(function (data) {
            var result = data.data;

            if (result != null && result != undefined)
                $scope.personBusiness = result;

            if ($scope.personBusiness != null &&
                $scope.personBusiness.personBusinessArchive != null &&
                $scope.personBusiness.personBusinessArchive.results.length > 0) {
                $scope.list.archives = $scope.personBusiness.personBusinessArchive.results;
                $scope.paginantion.totalItems = $scope.personBusiness.personBusinessArchive.totalResults;
                $scope.filter.PageNumber = $scope.personBusiness.personBusinessArchive.pageNumber;
                $scope.paginantion.totalPages = $scope.personBusiness.personBusinessArchive.totalPages;
            }
            else {
                $scope.list.archives = [];
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

    $scope.excludeArchive = function (item) {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        excluirArchive(item).then(function (data) {
            var result = data.data;

            if (result == true) {
                utilidadesService.exibirMensagem('Sucesso!', 'Arquivo excluído com sucesso.', true);
            }
            else {
                utilidadesService.exibirMensagem('Falha', 'Falha ao excluir o arquivo.', true);
            }

            $scope.getMyArchives();

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
        $scope.getMyArchives();
    }

    $scope.init();

}]);