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
        categories: [],
        subcategories: []
    };

    $scope.subcategories = [];
    $scope.productTypes = [];

    $scope.filter = {
        categories: [],
        subcategories: [],
        productTypes: []
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
        $scope.viewCategories = false;

        $scope.filter.subcategories = $filter('filter')($scope.list.subcategories, { categoryId: categoryId });
    }

    $scope.changeSubCategory = function (subCategoryId) {
        $scope.viewProductType = true;
        $scope.viewSubCategory = false;

        $scope.filter.productTypes = $filter('filter')($scope.productTypes, { subCategoryId: subCategoryId });
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

    $scope.getCategories = function () {
        basicService.getCategories().then(function (data) {
            var result = data.data;

            if (result != null && result != undefined)
                $scope.list.categories = result;

        }, function (error) {
            $scope.addErrorAlert("Falha ao carregar categorias. Entre em contato com o suporte!");
        });
    }

    $scope.loadSubCategory = function (categoryId) {
        
        basicService.getSubCategories(categoryId).then(function (data) {
            var result = data.data;

            $scope.list.subcategories = result;
            $scope.filter.subcategories = result;

        }, function (error) {
     
        });

    }

    $scope.loadProductType = function (subCategoryId) {

        basicService.getProductTypes(subCategoryId).then(function (data) {
            var result = data.data;

            $scope.productTypes = result;
            $scope.filter.productTypes = result;

        }, function (error) {

        });

    }

    $scope.pesquisarProdutos = function (Id) {

        // Obtem a URL completa
        var url = $location.absUrl();
        //https
        var https = url.split('/')[0];
        // Extrai apenas o domínio
        var domain = url.split('/')[2];

        var resultado = https + "//" + domain;

        var url = resultado + '/' + 'Home/Products?Id=' + Id;
        window.open(url, "_blank");


    }


    $scope.init = function () {
        $scope.getCategories();
        $scope.loadSubCategory(0);
        $scope.loadProductType(0);
    }

    $scope.init();

}]);