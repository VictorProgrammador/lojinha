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
    $scope.subcategories = [];
    $scope.productTypes = [];
    $scope.tamanhos = [];
    $scope.marcas = [];
    $scope.cores = [];


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

    $scope.productArchive = {};

    $scope.closeModal = function () {
        $scope.entity = {};
        $('#myModal').modal('hide');
    }

    $scope.editImages = function (item) {
        $scope.entity = item;
        $scope.openModalImages();
    }

    $scope.openModalImages = function () {
        $scope.myCroppedImage = '';
        $scope.myImage = '';
        $scope.file = null;
        $scope.showImageCropped = false;

        document.getElementById('productArchiveFile').value = null;
        $scope.productArchive = {};

        $scope.getProductArchives();

        $('#modalImages').modal('show');
    }

    $scope.closeModalImages = function () {
        $scope.entity = {};
        $scope.productArchive = {};
        $('#modalImages').modal('hide');
    }

    const headers = {
        'Content-Type': 'application/json'
    };

    saveMyProduct = function (param, headersProduct) {
        return $http.post("/User/SaveMyProduct", param, headersProduct);
    }

    saveMyProductFile = function (param, headersProduct) {
        return $http.post("/User/SaveMyProductFile", param, headersProduct);
    }

    saveProductArchive = function (param, headersProduct) {
        return $http.post("/User/SaveProductArchive", param, headersProduct);
    }

    saveProductVideo = function (param, headersProduct) {
        return $http.post("/User/SaveProductVideo", param, headersProduct);
    }

    excluirProductArchive = function (param) {
        return $http.post("/User/ExcluirProductArchive", param, headers);
    }

    getProductArchives = function (param) {
        return $http.post("/Home/GetProductArchives", param, headers);
    }

    excluirProduto = function (param) {
        return $http.post("/User/ExcluirProduto", param, headers);
    }

    $scope.loadCategory = function () {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        if ($scope.entity.categoryId == undefined)
            $scope.entity.categoryId = 0;

        basicService.getSubCategories($scope.entity.categoryId).then(function (data) {
            var result = data.data;

            $scope.subcategories = result;

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }

    $scope.loadSubCategory = function () {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        if ($scope.entity.subCategoryId == undefined)
            $scope.entity.subCategoryId = 0;

        basicService.getProductTypes($scope.entity.subCategoryId).then(function (data) {
            var result = data.data;

            $scope.productTypes = result;

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }


    $scope.getTamanhos = function () {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.getTamanhos().then(function (data) {
            var result = data.data;
            $scope.tamanhos = result;

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }


    $scope.getCores = function () {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.getCores().then(function (data) {
            var result = data.data;
            $scope.cores = result;

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }

    $scope.getMarcas = function () {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.getMarcas().then(function (data) {
            var result = data.data;
            $scope.marcas = result;

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }

    $scope.getProductArchives = function () {
        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        getProductArchives($scope.entity).then(function (data) {
            var result = data.data;

            $scope.entity.productArchives = result;

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });
    }

    $scope.excludeProductArchive = function (item) {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        excluirProductArchive(item).then(function (data) {
            var result = data.data;

            if (result == true) {
                utilidadesService.exibirMensagem('Sucesso!', 'Arquivo excluído com sucesso.', true);
            }
            else {
                utilidadesService.exibirMensagem('Falha', 'Falha ao excluir o arquivo.', true);
            }


            $scope.closeModalImages();

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }

    $scope.saveProductArchive = function () {

        var saveFunc = saveProductArchive;

        var formData = new FormData($("#formElem")[0]);
        if (document.getElementById('productArchiveFile').value == null ||
            document.getElementById('productArchiveFile').value == undefined ||
            document.getElementById('productArchiveFile').value.endsWith('jpg') ||
            document.getElementById('productArchiveFile').value.endsWith('jpeg') ||
            document.getElementById('productArchiveFile').value.endsWith('png')) {
            formData.append('file', $scope.myCroppedImage);
        }
        else {
            formData.append('file', $scope.file);
            saveFunc = saveProductVideo;
        }

        $scope.productArchive.productId = $scope.entity.id;

        var productArchive = $scope.productArchive;
        formData.append('productArchive', JSON.stringify(productArchive));

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        saveFunc(formData, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        }).then(function (data) {
            var result = data.data;

            $scope.closeModalImages();

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

        var saveFunc = saveMyProduct;
        $scope.entity.personBusinessId = $scope.personBusiness.id;

        var formData = new FormData($("#formElem")[0]);

        if (document.getElementById('imageFile').value == null ||
            document.getElementById('imageFile').value == undefined ||
            document.getElementById('imageFile').value.endsWith('jpg') ||
            document.getElementById('imageFile').value.endsWith('jpeg') ||
            document.getElementById('imageFile').value.endsWith('png')) {
            formData.append('file', $scope.myCroppedImage);
        }
        else {
            formData.append('file', $scope.file);
            saveFunc = saveMyProductFile;
        }

        var product = $scope.entity;

        formData.append('product', JSON.stringify(product));

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

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

            console.log('produto', $scope.list.products);

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

    $scope.addProductConfig = function () {
        $scope.entity.productConfigs.push({});
    }

    $scope.excludeProductConfig = function (productConfig) {
        var index = $scope.entity.productConfigs.indexOf(productConfig);

        if (index !== -1) {
            $scope.entity.productConfigs.splice(index, 1);
        }
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

        if ($scope.file.type != 'image/webp') {
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    $scope.myImage = evt.target.result;
                });
            };
            reader.readAsDataURL(file);
        }
    };

    angular.element(document.querySelector('#imageFile')).on('change', handleFileSelect);
    angular.element(document.querySelector('#productArchiveFile')).on('change', handleFileSelect);

    $scope.init = function () {
        $scope.getMyProducts();
        $scope.getCategories();
        $scope.loadCategory();
        $scope.loadSubCategory();
        $scope.getTamanhos();
        $scope.getMarcas();
        $scope.getCores();
    }

    $scope.init();

}]);