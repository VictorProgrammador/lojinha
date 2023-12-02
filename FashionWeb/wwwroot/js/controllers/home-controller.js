app.controller("HomeController", ["$scope", "$location", "$anchorScroll", "$filter", "basicService", function ($scope, $location, $anchorScroll, $filter, basicService) {


    $scope.filter = {
        Category: {},
        CategoryId: 0,
        SubCategoryId: 0,
        ProductTypeId: 0,
        PageNumber: 1,
        PageSize: 9
    };

    $scope.list = {
        products: [],
        categories: []
    };

    $scope.entity = {
        totalItems: 4,
        totalPages: 0
    };

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

    $scope.loadProducts = function (Id) {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        $scope.filter.ProductTypeId = Id;

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

            console.log($scope.list.products);

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }

}]);