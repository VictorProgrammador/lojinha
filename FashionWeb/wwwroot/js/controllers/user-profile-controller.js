app.controller("userProfileController", ["$scope", "$filter", "$window", "basicService", "utilidadesService", function ($scope, $filter, $window, basicService, utilidadesService) {

    //Declaração de váriavies

    $scope.myCroppedImage = '';
    $scope.urlProfile = '';
    $scope.showImageCropped = false;
    $scope.tabSelected = 0;

    $scope.user = {};
    $scope.file = null;

    $scope.categories = [];

    $scope.categoryTags = [];

    $scope.multipleDemo = {};
    $scope.multipleDemo.colors = [];

    //Funções de setar as tabs ativas


    $scope.tab = 1;

    $scope.currentNavItem = 'page1';


    //Funções sobre os alertas

    $scope.alerts = [];

    $scope.addSuccessAlert = function (message) {
        $scope.alerts.push({ type: 'success', msg: message });
    };

    $scope.addErrorAlert = function (message) {
        $scope.alerts.push({ type: 'danger', msg: message });
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };


    $scope.onTabSelected = function () {
        $scope.tabSelected = $scope.selectedIndex;
    };

    //Funções da foto de perfil.

    $scope.showImageToCropped = function () {
        $scope.showImageCropped = true;
        utilidadesService.exibirMensagem('Imagem carregada', 'Se sua imagem tiver aparecido em branco, entre em contato com o suporte. Caso tenha carregado, após cortar, clique no botão Atualizar foto de Perfil.', true);
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

    angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);


    //Busca e salva o perfil

    $scope.getMyProfile = function () {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.getMyProfile().then(function (data) {
            var result = data.data;

            if (result != null && result != undefined)
                $scope.user = result;

            if ($scope.user.profile.personBusiness != undefined &&
                $scope.user.profile.personBusiness != null &&
                ($scope.user.profile.personBusiness.approved == undefined ||
                $scope.user.profile.personBusiness.approved == false)) {
                $scope.addErrorAlert("Aprovação pendente, você será notificado por e-mail e via WhatsApp(caso tenha preenchido esse campo)");
            }

            $scope.urlProfile = $scope.user.profile.photoUrl + '?v=' + new Date().getTime();

            if ($scope.user.profile.hasBanner == true &&
                $scope.user.profile.bannerUrl != null &&
                $scope.user.profile.bannerUrl != '')
            {
                var contentImage = document.querySelector('.content-img');
                var imgPath = "/" + $scope.user.profile.bannerUrl.replace(/\\/g, '/');
                contentImage.style.backgroundImage  = 'url(' + imgPath  + ')';
            }

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

            $scope.getCategories();

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });
    }

    $scope.saveMyProfile = function () {

        var usuario = {
            PersonId: $scope.user.personId,
            AspNetUserId: $scope.user.aspNetUserId,
            typeUser: $scope.user.typeUser,
            Id: $scope.user.id,
            UniqueId: $scope.user.uniqueId,
            Profile: $scope.user.profile
        };


        if ($scope.user.profile.name == '' || $scope.user.profile.name == null) {
            utilidadesService.exibirMensagem('Atenção!', 'Necessário preencher o Nome!', false);
            return false;
        }

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.saveMyProfile(usuario).then(function (data) {

            var resultado = data.data;
            if (resultado != undefined && resultado.success == true) {
                utilidadesService.exibirMensagem('Atenção!', 'Sucesso ao atualizar seu perfil!', false);
            }
            else {

                if (resultado.errorList != null && resultado.errorList.length > 0) {
                    resultado.errorList.map(function (item) {
                        utilidadesService.exibirMensagem('Atenção!', item, false);
                    });
                }
            }


            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

            utilidadesService.exibirMensagem('Erro no processamento', 'Falha ao atualizar seu perfil, entre em contato com o suporte!', false);
        });

    }

    var fileChanged = function () {
        $scope.$apply(function () {
            $scope.bannerArchive = document.querySelector('input[name=bannerArchive]').files[0];
        });
    };

    angular.element(document.querySelector('#bannerArchiveId')).on('change', fileChanged);

    $scope.updatePersonBannerPhoto = function () {
        var formData = new FormData($("#formElem")[0]);

        if ($scope.bannerArchive == null || $scope.bannerArchive == undefined) {
            utilidadesService.exibirMensagem('Atenção!', 'Necessário anexar uma imagem! Aqui mesmo na aba Banner, no campo de subir arquivo.', false);
            return false;
        }

        formData.append('file', $scope.bannerArchive);

        var usuario = {
            PersonId: $scope.user.personId,
            AspNetUserId: $scope.user.aspNetUserId,
            typeUser: $scope.user.typeUser,
            Id: $scope.user.id,
            UniqueId: $scope.user.uniqueId,
            Profile: $scope.user.profile
        };
        formData.append('user', JSON.stringify(usuario));

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.updatePersonBannerPhoto(formData, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        }).then(function (data) {

            var resultado = data.data;
            if (resultado != undefined && resultado.success == true) {
                utilidadesService.exibirMensagem('Atenção!', 'Sucesso ao atualizar seu banner!', false);
                $window.location.reload();
            }
            else {

                if (resultado.errorList != null && resultado.errorList.length > 0) {
                    resultado.errorList.map(function (item) {
                        utilidadesService.exibirMensagem('Atenção!', item, false);
                    });
                }
            }


            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

            utilidadesService.exibirMensagem('Erro no processamento', 'Falha ao atualizar seu banner, entre em contato com o suporte!', false);
        });

    }

    $scope.updatePersonPhoto = function () {
        var formData = new FormData($("#formElem")[0]);

        if ($scope.myImage == null || $scope.myImage == '') {
            utilidadesService.exibirMensagem('Atenção!', 'Necessário anexar uma imagem. Para isso clique na camêra e carregue um arquivo de sua preferência.', false);
            return false;
        }

        formData.append('file', $scope.myCroppedImage);

        var usuario = {
            PersonId: $scope.user.personId,
            AspNetUserId: $scope.user.aspNetUserId,
            typeUser: $scope.user.typeUser,
            Id: $scope.user.id,
            UniqueId: $scope.user.uniqueId,
            Profile: $scope.user.profile
        };
        formData.append('user', JSON.stringify(usuario));

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.updatePersonPhoto(formData, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        }).then(function (data) {

            var resultado = data.data;
            if (resultado != undefined && resultado.success == true) {
                utilidadesService.exibirMensagem('Atenção!', 'Sucesso ao atualizar seu perfil!', false);
                $window.location.reload();
            }
            else {

                if (resultado.errorList != null && resultado.errorList.length > 0) {
                    resultado.errorList.map(function (item) {
                        utilidadesService.exibirMensagem('Atenção!', item, false);
                    });
                }
            }


            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

            utilidadesService.exibirMensagem('Erro no processamento', 'Falha ao atualizar seu perfil, entre em contato com o suporte!', false);
        });
    }

    //Busca as categorias

    $scope.getCategories = function () {
        basicService.getCategories().then(function (data) {
            var result = data.data;

            if (result != null && result != undefined)
                $scope.categories = result;

            if ($scope.categories != null && $scope.categories != undefined) {
                let category = $scope.categories.filter(function (item) {
                    return item.id === $scope.user.profile.personBusiness.categoryId;
                })[0];

                if (category != null && category != undefined) {
                    $scope.categoryTags = category.tags;
                }
            }

        }, function (error) {
            $scope.addErrorAlert("Falha ao carregar categorias. Entre em contato com o suporte!");
        });
    }

    $scope.changeCategory = function () {
        let category = $scope.categories.filter(function (item) {
            return item.id === $scope.user.profile.personBusiness.categoryId;
        })[0];

        if ($scope.user.profile.personBusiness.category != null &&
            $scope.user.profile.personBusiness.category != undefined) {
            $scope.user.profile.personBusiness.category.tags = [];
        }


        $scope.categoryTags = category.tags;
    }

    //Salva as informações do negócio
    $scope.saveMyBusiness = function () {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        if ($scope.user.profile.personBusiness == undefined || $scope.user.profile.personBusiness == null)
            $scope.user.profile.personBusiness = {};

        $scope.user.profile.personBusiness.PersonId = $scope.user.profile.id;

        basicService.saveMyBusiness($scope.user.profile.personBusiness).then(function (data) {
            
            var resultado = data.data;
            if (resultado != undefined && resultado.success == true) {
                utilidadesService.exibirMensagem('Atenção!', 'Sucesso ao atualizar seu negócio!', false);
            }
            else {

                if (resultado.errorList != null && resultado.errorList.length > 0) {
                    resultado.errorList.map(function (item) {
                        utilidadesService.exibirMensagem('Atenção!', item, false);
                    });
                }
            }


            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

            utilidadesService.exibirMensagem('Erro no processamento', 'Falha ao atualizar seu negócio. Entre em contato com o suporte!', false);
        });


    }

    //Inicialização

    $scope.init = function () {
        $scope.addSuccessAlert("Algumas imagens podem não funcionar devido seu formato ou tamanho, nesse caso faça upload da foto pelo computador ou entre em contato com nosso suporte.");
        $scope.getMyProfile();
    }

    $scope.init();

}]);