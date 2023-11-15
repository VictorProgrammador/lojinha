app.service('utilidadesService', function ($mdDialog) {
    this.exibirMensagem = function (titulo, mensagem, strong) {
        $mdDialog.show({
            templateUrl: '/js/components/customAlert.html',
            parent: angular.element(document.body),
            clickOutsideToClose: true,
            fullscreen: false,
            locals: {
                titulo: titulo,
                mensagem: mensagem,
                strong: strong
            },
            controller: function ($scope, titulo, mensagem, strong) {
                $scope.titulo = titulo;
                $scope.mensagem = mensagem;
                $scope.strong = strong;
                $scope.closeDialog = function () {
                    $mdDialog.hide();
                }
            }
        });
    };
    this.showAlert = function (titulo, message) {
        $mdDialog.show(
            $mdDialog.alert()
                .parent(angular.element(document.querySelector('#popupContainer')))
                .clickOutsideToClose(true)
                .title(titulo)
                .textContent(message)
                .ariaLabel('Alert Dialog Demo')
                .ok('Fechar')
        );
    };
});
