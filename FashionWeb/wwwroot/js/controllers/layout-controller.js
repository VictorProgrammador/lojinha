app.controller("LayoutController", ["$scope", "$location", "$anchorScroll", "$filter", "basicService", function ($scope, $location, $anchorScroll, $filter, basicService) {

    $scope.filter = {
        Category: {},
        CategoryId: 0,
        SubCategoryId: 0,
        PageNumber: 1,
        PageSize: 9
    };

    $scope.tags = [];
    $scope.tagSelecionada = {};

    $scope.list = {
        products: [],
        categories: []
    };

    $scope.viewSubCategory = false;
    $scope.viewProductType = false;
    $scope.viewCategories = false;

    $scope.entity = {
        totalItems: 4,
        totalPages: 0
    };

    $scope.scrollTo = function (id) {
        $location.hash(id);
        $anchorScroll();
    };

    $scope.clearFilter = function () {
        $scope.filter.Category = {};
        $scope.filter.CategoryId = 0;
        $scope.getProducts();
    }

    $scope.changeCategory = function (categoryId) {
        $scope.viewSubCategory = true;
        $scope.viewCategories = true;
        $scope.loadSubCategory(categoryId);
    }

    $scope.changeSubCategory = function (subCategoryId) {
        $scope.viewProductType = true;
        $scope.viewSubCategory = false;
        $scope.loadProductType(subCategoryId);
    }

    $scope.voltarCategorias = function () {
        $scope.viewProductType = false;
        $scope.viewSubCategory = false;
        $scope.viewCategories = true;
    }

    $scope.voltarSubCategorias = function () {
        $scope.viewProductType = false;
        $scope.viewSubCategory = true;
    }

    $scope.expandAllCategories = function () {
        $scope.viewSubCategory = true;
        $scope.viewProductType = false;
        $scope.viewCategories = true;
        $scope.subcategories = [];
    }

    $scope.getProducts = function () {
        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.getProducts($scope.filter).then(function (data) {
            var result = data.data;

            if (result != null && result != undefined && result.results.length > 0) {
                $scope.list.products = result.results;
                $scope.entity.totalItems = result.totalResults;
                $scope.filter.PageNumber = result.pageNumber;
                $scope.entity.totalPages = result.totalPages;
            }
            else {
                $scope.list.products = [];
                $scope.entity.totalItems = 0;
                $scope.filter.PageNumber = 1;
                $scope.entity.totalPages = 0;
            }

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });
    }

    $scope.getCategories = function () {
        basicService.getCategories().then(function (data) {
            var result = data.data;

            if (result != null && result != undefined)
                $scope.list.categories = result;

            console.log('list.categories', $scope.list.categories);

        }, function (error) {
            $scope.addErrorAlert("Falha ao carregar categorias. Entre em contato com o suporte!");
        });
    }

    $scope.loadSubCategory = function (categoryId) {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        
        basicService.getSubCategories(categoryId).then(function (data) {
            var result = data.data;

            $scope.subcategories = result;
            console.log('subcategories', $scope.subcategories);

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }

    $scope.loadProductType = function (subCategoryId) {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');


        basicService.getProductTypes(subCategoryId).then(function (data) {
            var result = data.data;

            $scope.productTypes = result;
            console.log('productTypes', $scope.productTypes);

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }

    $scope.pesquisarProdutos = function (Id) {
        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        // Obtem a URL completa
        var url = $location.absUrl();
        //https
        var https = url.split('/')[0];
        // Extrai apenas o domínio
        var domain = url.split('/')[2];

        var resultado = https + "//" + domain;

        var url = resultado + '/' + 'Home/Products?Id=' + Id;
        window.open(url, "_blank");

        $(".spinerStyle").removeClass('centerSpinner');
        $(".spinerBackground").removeClass('overlay');
    }


    $scope.init = function () {
        $scope.getCategories();
    }

    $scope.init();

}]);