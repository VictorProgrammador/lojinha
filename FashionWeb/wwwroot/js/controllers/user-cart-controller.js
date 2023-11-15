app.controller("userCartController", ["$scope", "$sce", "$location", "$window", "basicService", "$filter", "utilidadesService", function ($scope, $sce, $location, $window, basicService, $filter, utilidadesService) {

    $scope.alerts = [];
    $scope.entity = {};

    $scope.frete = 0;
    $scope.valorTotal = 0;

    $scope.endereco = {};
    $scope.card = {};

    $scope.OrderEntity = {};

    $scope.parcelamentos = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
    $scope.parcelamentoSelecionado = 0;

    $scope.addSuccessAlert = function (message) {
        $scope.alerts.push({ type: 'success', msg: message });
    };

    $scope.addErrorAlert = function (message) {
        $scope.alerts.push({ type: 'danger', msg: message });
    };

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.freteResponseList = [];

    $scope.bandeiraImg = 'img/creditCard.png';


    $scope.loadCart = function () {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.getCart().then(function (data) {
            var result = data.data;

            if (result != null && result != undefined) {
                $scope.entity = result;
                $scope.OrderEntity.cartId = $scope.entity.id;
            }
            else {
                utilidadesService.exibirMensagem('Atenção!', 'Carrinho não foi encontrado!', false);
                $scope.addErrorAlert("Carrinho não encontrado na base de dados!");
            }

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }

    $scope.excludeCartProduct = function (id) {

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.excludeCartProduct(id).then(function (data) {
            utilidadesService.exibirMensagem('Sucesso', 'O produto foi excluído do carrinho', false);
            $scope.loadCart();
        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }

    $scope.showContainerCart = function () {
        var divCart = document.querySelector('.cart-container');
        divCart.removeAttribute('hidden');

        var divPayment = document.querySelector('.payment-container');
        divPayment.setAttribute('hidden', 'true');

        var divFormaPagamento = document.querySelector('.formaPagamento-container');
        divFormaPagamento.setAttribute('hidden', 'true');

        var divMeioPagamento = document.querySelector('.meioPagamento-container');
        divMeioPagamento.setAttribute('hidden', 'true');
    }

    $scope.showFormaPagamento = function () {

        var divFormaPagamento = document.querySelector('.formaPagamento-container');
        divFormaPagamento.removeAttribute('hidden');

        var divFrete = document.querySelector('.frete-container');
        divFrete.setAttribute('hidden', 'true');

        var divPayment = document.querySelector('.payment-container');
        divPayment.setAttribute('hidden', 'true');

        var divCart = document.querySelector('.cart-container');
        divCart.setAttribute('hidden', 'true');

        var divMeioPagamento = document.querySelector('.meioPagamento-container');
        divMeioPagamento.setAttribute('hidden', 'true');

    }

    $scope.completePayment = function () {

        var allValidate = true;

        if ($scope.endereco.rua == null || $scope.endereco.rua == '') {
            document.querySelector('input[name="rua"]').style.border = '1px solid red';
            allValidate = false;
        }
        else {
            document.querySelector('input[name="rua"]').style.border = '1px solid #ccc';
        }

        if ($scope.endereco.numeroCasa == null || $scope.endereco.numeroCasa == '') {
            document.querySelector('input[name="numeroCasa"]').style.border = '1px solid red';
            allValidate = false;
        }
        else {
            document.querySelector('input[name="numeroCasa"]').style.border = '1px solid #ccc';
        }

        if ($scope.endereco.bairro == null || $scope.endereco.bairro == '') {
            document.querySelector('input[name="bairro"]').style.border = '1px solid red';
            allValidate = false;
        }
        else {
            document.querySelector('input[name="bairro"]').style.border = '1px solid #ccc';
        }

        if ($scope.endereco.cidade == null || $scope.endereco.cidade == '') {
            document.querySelector('input[name="cidade"]').style.border = '1px solid red';
            allValidate = false;
        }
        else {
            document.querySelector('input[name="cidade"]').style.border = '1px solid #ccc';
        }

        if ($scope.endereco.cep == null || $scope.endereco.cep == '') {
            document.querySelector('input[name="cep"]').style.border = '1px solid red';
            allValidate = false;
        }
        else {
            document.querySelector('input[name="cep"]').style.border = '1px solid #ccc';
        }

        if (allValidate) {
            var divCart = document.querySelector('.cart-container');
            divCart.setAttribute('hidden', 'true');

            var divFormaPagamento = document.querySelector('.formaPagamento-container');
            divFormaPagamento.setAttribute('hidden', 'true');

            var divPayment = document.querySelector('.payment-container');
            divPayment.setAttribute('hidden', 'true');

            var divMeioPagamento = document.querySelector('.meioPagamento-container');
            divMeioPagamento.setAttribute('hidden', 'true');

            var divFrete = document.querySelector('.frete-container');
            divFrete.removeAttribute('hidden');
        }
        else {
            utilidadesService.exibirMensagem('Atenção', 'Você não preencheu os campos essenciais do endereço!', false);
        }

    }

    $scope.irPagamento = function () {

        var divCart = document.querySelector('.cart-container');
        divCart.setAttribute('hidden', 'true');

        var divFormaPagamento = document.querySelector('.formaPagamento-container');
        divFormaPagamento.setAttribute('hidden', 'true');

        var divFrete = document.querySelector('.frete-container');
        divFrete.setAttribute('hidden', 'true');

        var divMeioPagamento = document.querySelector('.meioPagamento-container');
        divMeioPagamento.setAttribute('hidden', 'true');

        var divPayment = document.querySelector('.payment-container');
        divPayment.removeAttribute('hidden');
    }

    function validarCartao() {
        var numeroCartao = document.getElementById('numeroCartao').value;
        numeroCartao = numeroCartao.replace(/\s/g, ''); // remova os espaços

        var resultado = isValid(numeroCartao);

        if (resultado) {
            var bandeira = findCreditCardObjectByNumber(numeroCartao);

            if (bandeira != null && bandeira.name != '') {
                //Mastercard
                if (bandeira.name == 'Mastercard') {
                    $scope.bandeiraImg = 'img/master.png';
                }

                //Visa
                if (bandeira.name == 'Visa') {
                    $scope.bandeiraImg = 'img/visa.png';
                }

                //Amex
                if (bandeira.name == 'Amex') {
                    $scope.bandeiraImg = 'img/amex.png';
                }

                //Hipercard
                if (bandeira.name == 'Hipercard') {
                    $scope.bandeiraImg = 'img/hipercard.png';
                }

                //Elo
                if (bandeira.name == 'Elo') {
                    $scope.bandeiraImg = 'img/elo.png';
                }

            }

        } else {
            $scope.card.numeroCartao = '';
        }
    }

    $scope.captureBandeira = function () {
        validarCartao();
    }

    $scope.paid = function () {

        var allValidate = true;

        if ($scope.card.nomeTitular == null || $scope.card.nomeTitular == '') {
            document.querySelector('input[name="nomeTitular"]').style.border = '1px solid red';
            allValidate = false;
        }
        else {
            document.querySelector('input[name="nomeTitular"]').style.border = '1px solid #ccc';
        }

        if ($scope.card.cpfTitular == null || $scope.card.cpfTitular == '') {
            document.querySelector('input[name="cpfTitular"]').style.border = '1px solid red';
            allValidate = false;
        }
        else {
            document.querySelector('input[name="cpfTitular"]').style.border = '1px solid #ccc';
        }

        if (document.getElementById('numeroCartao').value == undefined ||
            document.getElementById('numeroCartao').value == '') {
            document.querySelector('input[name="numeroCartao"]').style.border = '1px solid red';
            allValidate = false;
        }
        else {
            document.querySelector('input[name="numeroCartao"]').style.border = '1px solid #ccc';
            $scope.card.numeroCartao = document.getElementById('numeroCartao').value;
        }

        if ($scope.card.validade == null || $scope.card.validade == '') {
            document.querySelector('input[name="validade"]').style.border = '1px solid red';
            allValidate = false;
        }
        else {
            document.querySelector('input[name="validade"]').style.border = '1px solid #ccc';
        }

        if ($scope.card.cvv == null || $scope.card.cvv == '') {
            document.querySelector('input[name="codigoSeguranca"]').style.border = '1px solid red';
            allValidate = false;
        }
        else {
            document.querySelector('input[name="codigoSeguranca"]').style.border = '1px solid #ccc';
        }

        if (allValidate) {

            if ($scope.parcelamentoSelecionado == 0) {
                utilidadesService.exibirMensagem('Atenção', 'Selecione o número de parcelas!', false);
            }

            var freteSelecionado = $scope.freteResponseList.find(function (objeto) {
                return objeto.id === $scope.entity.freteSelecionadoId;
            });


            $scope.OrderEntity.orderProducts = [];

            $scope.entity.cartProducts.map(function (data) {

                var orderProduct = {};
                orderProduct.productId = data.product.id;

                $scope.OrderEntity.orderProducts.push(orderProduct);
            });

            $scope.OrderEntity.freteService = freteSelecionado.name;
            $scope.OrderEntity.deliveryRangeMin = freteSelecionado.delivery_range.min;
            $scope.OrderEntity.deliveryRangeMax = freteSelecionado.delivery_range.max;
            $scope.OrderEntity.fretePrice = parseFloat(freteSelecionado.price);
            $scope.OrderEntity.Parcelamento = $scope.parcelamentoSelecionado;
            $scope.OrderEntity.valorTotal = $scope.valorTotal;
            $scope.OrderEntity.bairro = $scope.endereco.bairro;
            $scope.OrderEntity.cep = $scope.endereco.cep;
            $scope.OrderEntity.cidade = $scope.endereco.cidade;
            $scope.OrderEntity.complemento = $scope.endereco.complemento;
            $scope.OrderEntity.numeroCasa = $scope.endereco.numeroCasa;
            $scope.OrderEntity.rua = $scope.endereco.rua;

            $scope.OrderEntity.card = $scope.card;

            console.log('request', $scope.OrderEntity);

            $(".spinerStyle").addClass('centerSpinner');
            $(".spinerBackground").addClass('overlay');

            basicService.createOrder($scope.OrderEntity).then(function (data) {

                var result = data.data;

                // Obtem a URL completa
                var url = $location.absUrl();
                //https
                var https = url.split('/')[0];
                // Extrai apenas o domínio
                var domain = url.split('/')[2];

                var resultado = https + "//" + domain;

                var url = resultado + '/' + 'User/Orders';

                $window.location.href = url;

                $(".spinerStyle").removeClass('centerSpinner');
                $(".spinerBackground").removeClass('overlay');

            }, function (error) {
                $(".spinerStyle").removeClass('centerSpinner');
                $(".spinerBackground").removeClass('overlay');
            });

        }
        else {
            utilidadesService.exibirMensagem('Atenção', 'Preencha todos os dados da sua forma de pagamento!', false);
        }


    }

    $scope.calculeTotal = function (index, freteResponse) {

        $scope.valorTotal = 0;
        $scope.entity.freteSelecionadoId = freteResponse.id;

        var freteSelecionado = $scope.freteResponseList.find(function (objeto) {
            return objeto.id === $scope.entity.freteSelecionadoId;
        });

        if (freteSelecionado != null && freteSelecionado.price > 0) {
            $scope.frete = freteSelecionado.price;
        }
        else
            $scope.frete = 0;

        //Calcular valor total
        $scope.entity.cartProducts.map(function (produto) {
            $scope.valorTotal += produto.product.value;
        });

        $scope.valorTotal += parseFloat($scope.frete);

        $scope.freteResponseList.map(function (linha) {
            linha.colorida = false;
        });

        $scope.freteResponseList[index].colorida = !$scope.freteResponseList[index].colorida;
    }

    $scope.irMeioPagamento = function () {
       
        if ($scope.endereco.cep == null || $scope.endereco.cep == '') {
            document.querySelector('input[name="cep"]').style.border = '1px solid red';
            return false;
        }
        else {
            document.querySelector('input[name="cep"]').style.border = '1px solid #ccc';

            if ($scope.entity.freteSelecionadoId != null && $scope.entity.freteSelecionadoId > 0) {

                var divFrete = document.querySelector('.frete-container');
                divFrete.setAttribute('hidden', 'true');

                var divFormaPagamento = document.querySelector('.formaPagamento-container');
                divFormaPagamento.setAttribute('hidden', 'true');

                var divPayment = document.querySelector('.cart-container');
                divPayment.setAttribute('hidden', 'true');

                var divMeioPagamento = document.querySelector('.meioPagamento-container');
                divMeioPagamento.setAttribute('hidden', 'true');

                var divPaymentContainer = document.querySelector('.payment-container');
                divPaymentContainer.setAttribute('hidden', 'true');

                var divMeioPagamento = document.querySelector('.meioPagamento-container');
                divMeioPagamento.removeAttribute('hidden');

            }
            else {
                utilidadesService.exibirMensagem('Atenção', 'É necessário selecionar um serviço de frete!', false);
            }
        }
    }

    $scope.searchCEP = function () {
        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');
        basicService.searchCEP($scope.endereco).then(function (data) {

            var result = data.data;
            $scope.freteResponseList = result;

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }



}]);