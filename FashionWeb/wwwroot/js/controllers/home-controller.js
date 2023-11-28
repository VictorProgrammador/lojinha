app.controller("HomeController", ["$scope", "$location", "$anchorScroll", "$filter", "basicService", function ($scope, $location, $anchorScroll, $filter, basicService) {

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

    $scope.changeCategory = function () {
        $scope.filter.CategoryId = $scope.filter.Category.id;
        $scope.filter.PageNumber = 1;
        $scope.filter.PageSize = 9;
        $scope.loadSubCategory();
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

    $scope.loadSubCategory = function () {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        if ($scope.entity.categoryId == undefined)
            $scope.entity.categoryId = 0;

        basicService.getSubCategories($scope.filter.CategoryId).then(function (data) {
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

    $scope.loadProductType = function () {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        if ($scope.entity.subCategoryId == undefined)
            $scope.entity.subCategoryId = 0;

        basicService.getProductTypes($scope.filter.SubCategoryId).then(function (data) {
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

    $scope.deleteCategory = function () {
        $scope.filter.Category = {};
    }

    $scope.visitProduct = function (Id) {
        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        // Obtem a URL completa
        var url = $location.absUrl();
        //https
        var https = url.split('/')[0];
        // Extrai apenas o domínio
        var domain = url.split('/')[2];

        var resultado = https + "//" + domain;

        var url = resultado + '/' + 'Home/Product?Id=' + Id;
        window.open(url, "_blank");

        $(".spinerStyle").removeClass('centerSpinner');
        $(".spinerBackground").removeClass('overlay');
    }

    $scope.init = function () {
        $scope.getProducts();
        $scope.getCategories();
        $scope.loadSubCategory();
        $scope.loadProductType();
    }

    $scope.init();

}]);