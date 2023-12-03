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

    $scope.categorySelected = {};

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

    $scope.changeCategory = function (category) {
        $scope.viewSubCategory = true;
        $scope.viewCategories = true;

        $scope.categorySelected = category;

        $scope.list.categories.map(function (category) {
            category.isActive = false;
        });
        category.isActive = true;

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

    $scope.reorderCategories = function () {
        $scope.list.categories.map(function (category) {

            category.subcategories = [];
            category.subcategories = $filter('filter')($scope.list.subcategories, { categoryId: category.id });

            category.subcategories.map(function (subCategory) {
                subCategory.productTypes = [];
                subCategory.productTypes = $filter('filter')($scope.productTypes, { subCategoryId: subCategory.id });
            });

        });

        $scope.categorySelected = $scope.list.categories[0];
        $scope.categorySelected.isActive = true;
    }

    $scope.getCategories = function () {
        basicService.getCategories().then(function (data) {
            var result = data.data;

            if (result != null && result != undefined) {
                $scope.list.categories = result;
                $scope.loadSubCategory(0);                
            }

        }, function (error) {
            $scope.addErrorAlert("Falha ao carregar categorias. Entre em contato com o suporte!");
        });
    }

    $scope.loadSubCategory = function (categoryId) {
        
        basicService.getSubCategories(categoryId).then(function (data) {
            var result = data.data;

            $scope.list.subcategories = result;
            $scope.filter.subcategories = result;

            $scope.loadProductType(0);

        }, function (error) {
     
        });

    }

    $scope.loadProductType = function (subCategoryId) {

        basicService.getProductTypes(subCategoryId).then(function (data) {
            var result = data.data;

            $scope.productTypes = result;
            $scope.filter.productTypes = result;

            $scope.reorderCategories();

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

        var url = resultado + '/' + 'Home/Products?ProductTypeId=' + Id + '&Category=' + $scope.categorySelected.name;
        window.open(url, "_self");


    }


    $scope.init = function () {
        $scope.getCategories();
    }

    $scope.init();

}]);