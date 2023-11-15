app.controller("userManagementProductsController", ["$scope", "$window", "$http", "$filter", "basicService", "utilidadesService", function ($scope, $window, $http, $filter, basicService, utilidadesService) {

    $scope.filter = {
        PageNumber: 1,
        PageSize: 5
    };

    $scope.paginantion = {
        totalItems: 4,
        totalPages: 0
    };

    $scope.myCroppedImage = '';
    $scope.showImageCropped = false;
    $scope.file = null;

    $scope.entity = {};

    $scope.personBusiness = {

    };

    $scope.list = {
        products: []       
    };

    $scope.categories = [];

    $scope.complements = [];

    $scope.setTab = function (newTab) {
        $scope.tab = newTab;
    };

    $scope.currentNavItem = 'page1';

    $scope.isSet = function (tabNum) {
        return $scope.tab === tabNum;
    };

    $scope.addProduct = function () {
        $scope.openModal();
    }

    $scope.openModal = function () {
        $scope.entity = {};
        $scope.myImage = '';
        $scope.myCroppedImage = '';
        $scope.file = null;
        $scope.showImageCropped = false;
        document.getElementById('imageFile').value = null;

        console.log('complementos:', $scope.complements);

        $('#myModal').modal('show');
    }

    $scope.closeModal = function () {
        $scope.entity = {};
        $('#myModal').modal('hide');
    }

    const headers = {
        'Content-Type': 'application/json'
    };

    saveMyProduct = function (param, headersProduct) {
        return $http.post("/User/SaveMyProduct", param, headersProduct);
    }

    excluirProduto = function (param) {
        return $http.post("/User/ExcluirProduto", param, headers);
    }

    $scope.getCategories = function () {
        basicService.getCategories().then(function (data) {
            var result = data.data;

            if (result != null && result != undefined)
                $scope.categories = result;

        }, function (error) {
            $scope.addErrorAlert("Falha ao carregar categorias. Entre em contato com o suporte!");
        });
    }

    $scope.save_click = function () {
        if ($scope.entity.value != null && $scope.entity.value != undefined)
        {
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

        var product = $scope.entity;

        formData.append('product', JSON.stringify(product));

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        saveMyProduct(formData, {
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

            //$scope.getMyProducts();

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });
    }

    $scope.getComplements = function () {
        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.getMyComplements().then(function (data) {
            var result = data.data;

            if (result != null &&
                result.length > 0) {
                $scope.complements = result;
            }
            else {
                $scope.complements = [];
            }

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });
    }

    $scope.getMyProducts = function () {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.getMyProducts($scope.filter).then(function (data) {
            var result = data.data;

            if (result != null && result != undefined)
                $scope.personBusiness = result;

            if ($scope.personBusiness != null &&
                $scope.personBusiness.products != null &&
                $scope.personBusiness.products.results.length > 0)
            {
                $scope.list.products = $scope.personBusiness.products.results;
                $scope.paginantion.totalItems = $scope.personBusiness.products.totalResults;
                $scope.filter.PageNumber = $scope.personBusiness.products.pageNumber;
                $scope.paginantion.totalPages = $scope.personBusiness.products.totalPages;
            }
            else {
                $scope.list.products = [];
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

    $scope.editProduct = function (item) {
        $scope.openModal();
        $scope.entity = item;
    }

    $scope.excludeProduct = function (item) {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        excluirProduto(item).then(function (data) {
            var result = data.data;

            if (result == true) {
                utilidadesService.exibirMensagem('Sucesso!', 'Produto excluído com sucesso.', true);
            }
            else {
                utilidadesService.exibirMensagem('Falha', 'Falha ao excluir o produto.', true);
            }

            $scope.getMyProducts();

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
        $scope.getMyProducts();
        $scope.getComplements();
        $scope.getCategories();
    }

    $scope.init();

}]);