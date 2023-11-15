app.controller("checkinController", ["$scope", "$location", "$window", "basicService", "$filter", "utilidadesService", function ($scope, $location, $window, basicService, $filter, utilidadesService) {

    $scope.filter = {
        PageNumber: 1,
        PageSize: 5,
        PersonId: 0
    };

    $scope.paginantion = {
        totalItems: 4,
        totalPages: 0
    };

    $scope.list = {
        raffles: [],
        sortedNumber: []
    };

    $scope.personBusiness = {};
    $scope.alerts = [];

    $scope.entity = {};
    $scope.disableProsseguir = false;

    $scope.tagsConcatened = '';

    $scope.progressValue = 0;

    $scope.addSuccessAlert = function (message) {
        $scope.alerts.push({ type: 'primary', msg: message });
    };

    $scope.addErrorAlert = function (message) {
        $scope.alerts.push({ type: 'danger', msg: message });
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.currentStep = 0;

    $scope.nextPage = function () {
        if ($scope.currentStep < 2) {
            $scope.currentStep++;
        }
    }

    $scope.firstPage = function () {
        $scope.currentStep = 0;
    }

    $scope.savePersonBusinessCheckin = function () {

        if ($scope.entity.instagram == null || $scope.entity.instagram == '') {
            $scope.addErrorAlert("Necessário por o user do Instagram!");
            return false;
        }

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        $scope.entity.personBusinessId = $scope.personBusiness.id;

        basicService.savePersonBusinessCheckin($scope.entity).then(function (data) {
            var result = data.data;
            if (result.success == true) {
                $scope.progressValue += 1;
                $scope.addSuccessAlert(result.message);

                 var lista = Object.entries(result.pedrasSorteadas);
                lista.forEach(function (elemento) {
                    var chave = elemento[0];
                    var valor = elemento[1];
                    $scope.list.sortedNumber.push({ Name: chave, Value: valor});
                });

                $scope.currentStep++;
            }
            else {
                $scope.addErrorAlert(result.message);
            }

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
            $scope.addErrorAlert("Erro ao fazer seu checkin!");
        });
    }

    $scope.loadProfile = function (personId) {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        $scope.filter.PersonId = personId;

        basicService.getProfileRaffles($scope.filter).then(function (data) {
            var result = data.data;

            if (result != null && result != undefined) {
                $scope.personBusiness = result;
                $scope.secondRate = $scope.personBusiness.mediaAvaliacoes;

                if ($scope.personBusiness.person.photoUrl != null &&
                    $scope.personBusiness.person.photoUrl != '') {
                    if ($scope.personBusiness.person.photoUrl != '/gravatar.png') {
                        $scope.personBusiness.person.photoUrl = "/" + $scope.personBusiness.person.photoUrl;
                    }
                }

                if ($scope.personBusiness.person.hasBanner == true &&
                    $scope.personBusiness.person.bannerUrl != null &&
                    $scope.personBusiness.person.bannerUrl != '') {
                    var contentImage = document.querySelector('.content-img');
                    var imgPath = "/" + $scope.personBusiness.person.bannerUrl.replace(/\\/g, '/');
                    contentImage.style.backgroundImage = 'url(' + imgPath + ')';
                }

                if ($scope.personBusiness.category != null && $scope.personBusiness.category.tags != null) {
                    let tags = $scope.personBusiness.category.tags.map(item => item.name);
                    $scope.tagsConcatened = tags.join(', ');
                }


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

                console.log('resultado:', $scope.personBusiness);

            }
            else {
                utilidadesService.exibirMensagem('Atenção!', 'Pessoa não foi encontrada!', false);
                $scope.addErrorAlert("Pessoa buscada não encontrada na base de dados!");
            }

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }

}]);