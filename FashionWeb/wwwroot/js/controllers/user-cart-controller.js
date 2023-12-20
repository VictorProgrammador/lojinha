app.controller("userCartController", ["$scope", "$sce", "$location", "$window", "basicService", "$filter", "utilidadesService", function ($scope, $sce, $location, $window, basicService, $filter, utilidadesService) {

    $scope.alerts = [];
    $scope.entity = {};

    $scope.frete = 0;
    $scope.valorTotal = 0;

    $scope.endereco = {};
    $scope.card = {};

    $scope.cupom = "";
    $scope.onlyCamisetas = false;
    $scope.cupomAplicado = false;

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


    $scope.loadCart = function (cartjson) {

        var cart = {};

        var cartTemporarioString = localStorage.getItem("cartTemporario");
        var cartTemporario = JSON.parse(cartTemporarioString);

        //Se o carrinho temporário existir, apenas adiciona o produto dentro dele.
        if (cartTemporario != null &&
            cartTemporario.CartProducts != null &&
            cartTemporario.CartProducts.length > 0) {
            if (cartjson != null && cartjson != '') {
                cart = JSON.parse(cartjson);
                if (cart != null) {
                    
                    var produtoExiste = cartTemporario.CartProducts.some(function (produto) {
                        return produto.ProductId === cart.CartProducts[0].ProductId;
                    });

                    if (!produtoExiste)
                        cartTemporario.CartProducts.push(cart.CartProducts[0]);

                }
            }

            localStorage.setItem("cartTemporario", JSON.stringify(cartTemporario));
        }
        else {
            if (cartjson != null && cartjson != '') {
                cart = JSON.parse(cartjson);
                localStorage.setItem("cartTemporario", JSON.stringify(cart));
            }
        }

        var cartTemporarioString = localStorage.getItem("cartTemporario");
        var cartTemporario = JSON.parse(cartTemporarioString);

        $(".spinerStyle").addClass('centerSpinner');
        $(".spinerBackground").addClass('overlay');

        basicService.getCart(cartTemporario).then(function (data) {
            var result = data.data;

            if (result != null && result != undefined) {
                $scope.entity = result;
                $scope.OrderEntity.cartId = $scope.entity.id;

                if ($scope.entity.cartProducts != null && $scope.entity.cartProducts.length > 0) {

                    var onlyCamisetas = $scope.entity.cartProducts.map(function (produto) {
                        return produto.product.name.includes("Camiseta");
                    });

                    if (onlyCamisetas != null &&
                        onlyCamisetas.length == 3 &&
                        $scope.entity.cartProducts.length == 3)
                        $scope.onlyCamisetas = true;

                    var totalValueCheckout = 0;

                    $scope.entity.cartProducts.map(function (produto) {
                        totalValueCheckout += produto.product.value;
                    });

                    fbq('track', 'InitiateCheckout', {
                        value: totalValueCheckout,
                        currency: 'BRL'
                    });

                }
            }

            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');

        }, function (error) {
            $(".spinerStyle").removeClass('centerSpinner');
            $(".spinerBackground").removeClass('overlay');
        });

    }
    

    $scope.excludeCartProduct = function (id, ProductId) {

        if (id == null || id == 0) {

            var cartTemporarioString = localStorage.getItem("cartTemporario");
            var cartTemporario = JSON.parse(cartTemporarioString);

            var indiceProduct = cartTemporario.CartProducts.findIndex(function (cart) {
                return cart.ProductId === ProductId;
            });

            if (indiceProduct !== -1) {
                cartTemporario.CartProducts.splice(indiceProduct, 1);
                utilidadesService.exibirMensagem('Sucesso', 'O produto foi excluído do carrinho', false);
            }

            localStorage.setItem("cartTemporario", JSON.stringify(cartTemporario));

            $scope.loadCart();         
        }
        else {
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
    }

    $scope.showContainerCart = function () {
        var divCart = document.querySelector('.contentCartItems');
        divCart.removeAttribute('hidden');

        var divPayment = document.querySelector('.payment-container');
        divPayment.setAttribute('hidden', 'true');

        var divFormaPagamento = document.querySelector('.formaPagamento-container');
        divFormaPagamento.setAttribute('hidden', 'true');

        var divRegistro = document.querySelector('.register-container');
        divRegistro.setAttribute('hidden', 'true');

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

        var divCart = document.querySelector('.contentCartItems');
        divCart.setAttribute('hidden', 'true');

        var divRegistro = document.querySelector('.register-container');
        divRegistro.setAttribute('hidden', 'true');

        var divMeioPagamento = document.querySelector('.meioPagamento-container');
        divMeioPagamento.setAttribute('hidden', 'true');

    }

    $scope.irEndereco = function (username) {

        if (username == null || username == '')
            $scope.irRegistro();
        else
            $scope.completePayment();
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
            var divCart = document.querySelector('.contentCartItems');
            divCart.setAttribute('hidden', 'true');

            var divRegistro = document.querySelector('.register-container');
            divRegistro.setAttribute('hidden', 'true');

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

        var divCart = document.querySelector('.contentCartItems');
        divCart.setAttribute('hidden', 'true');

        var divFormaPagamento = document.querySelector('.formaPagamento-container');
        divFormaPagamento.setAttribute('hidden', 'true');

        var divFrete = document.querySelector('.frete-container');
        divFrete.setAttribute('hidden', 'true');

        var divMeioPagamento = document.querySelector('.meioPagamento-container');
        divMeioPagamento.setAttribute('hidden', 'true');

        var divRegistro = document.querySelector('.register-container');
        divRegistro.setAttribute('hidden', 'true');

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

        if ($scope.parcelamentoSelecionado == 0) {
            utilidadesService.exibirMensagem('Atenção', 'Selecione o número de parcelas!', false);
            allValidate = false;
        }

        if ($scope.card.validade.length < 4) {
            utilidadesService.exibirMensagem('Atenção', 'Data de validade incorreta!', false);
            allValidate = false;
        }

        if (allValidate) {

            var freteSelecionado = $scope.freteResponseList.find(function (objeto) {
                return objeto.id === $scope.entity.freteSelecionadoId;
            });


            $scope.OrderEntity.orderProducts = [];

            $scope.entity.cartProducts.map(function (data) {

                var orderProduct = {};
                orderProduct.productId = data.product.id;
                orderProduct.color = data.color;
                orderProduct.tamanho = data.tamanho;

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


            $(".spinerStyle").addClass('centerSpinner');
            $(".spinerBackground").addClass('overlay');

            basicService.createOrder($scope.OrderEntity).then(function (data) {

                var result = data.data;

                if (result == true) {
                    fbq('track', 'Purchase', {
                        value: $scope.valorTotal,
                        currency: 'BRL'
                    });

                    // Obtem a URL completa
                    var url = $location.absUrl();
                    //https
                    var https = url.split('/')[0];
                    // Extrai apenas o domínio
                    var domain = url.split('/')[2];

                    var resultado = https + "//" + domain;

                    var url = resultado + '/' + 'User/Orders';

                    $window.location.href = url;
                }

                utilidadesService.exibirMensagem('Atenção', 'Seu cartão não foi validado pelas questões de segurança!', false);
                

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

    $scope.irRegistro = function (username) {

        if ($scope.endereco.cep == null || $scope.endereco.cep == '') {
            document.querySelector('input[name="cep"]').style.border = '1px solid red';
            return false;
        }
        else
        {
            document.querySelector('input[name="cep"]').style.border = '1px solid #ccc';

            if ($scope.entity.freteSelecionadoId != null && $scope.entity.freteSelecionadoId > 0) {
                if (username == null || username == '') {
                    var divFrete = document.querySelector('.frete-container');
                    divFrete.setAttribute('hidden', 'true');

                    var divFormaPagamento = document.querySelector('.formaPagamento-container');
                    divFormaPagamento.setAttribute('hidden', 'true');

                    var divPayment = document.querySelector('.contentCartItems');
                    divPayment.setAttribute('hidden', 'true');

                    var divMeioPagamento = document.querySelector('.meioPagamento-container');
                    divMeioPagamento.setAttribute('hidden', 'true');

                    var divPaymentContainer = document.querySelector('.payment-container');
                    divPaymentContainer.setAttribute('hidden', 'true');

                    var divMeioPagamento = document.querySelector('.meioPagamento-container');
                    divMeioPagamento.setAttribute('hidden', 'true');

                    var divRegistro = document.querySelector('.register-container');
                    divRegistro.removeAttribute('hidden');
                }
                else {
                    $scope.irMeioPagamento();
                }
            }
            else {
                utilidadesService.exibirMensagem('Atenção', 'É necessário selecionar um serviço de frete!', false);
            }
        }        
    }

    $scope.validadeUserToMeioPagamento = function () {

        var allValidate = true;

        if (document.getElementById('name').value == undefined ||
            document.getElementById('name').value == '') {
            document.querySelector('input[name="name"]').style.border = '1px solid red';
            allValidate = false;
        }
        else {
            document.querySelector('input[name="name"]').style.border = '1px solid #ccc';
        }

        if (document.getElementById('username').value == undefined ||
            document.getElementById('username').value == '') {
            document.querySelector('input[name="username"]').style.border = '1px solid red';
            allValidate = false;
        }
        else {
            document.querySelector('input[name="username"]').style.border = '1px solid #ccc';
        }

        if (allValidate)
            $scope.irMeioPagamento();

    }

    $scope.irMeioPagamento = function () {

        var divFrete = document.querySelector('.frete-container');
        divFrete.setAttribute('hidden', 'true');

        var divFormaPagamento = document.querySelector('.formaPagamento-container');
        divFormaPagamento.setAttribute('hidden', 'true');

        var divPayment = document.querySelector('.contentCartItems');
        divPayment.setAttribute('hidden', 'true');

        var divMeioPagamento = document.querySelector('.meioPagamento-container');
        divMeioPagamento.setAttribute('hidden', 'true');

        var divPaymentContainer = document.querySelector('.payment-container');
        divPaymentContainer.setAttribute('hidden', 'true');

        var divRegistro = document.querySelector('.register-container');
        divRegistro.setAttribute('hidden', 'true');

        var divMeioPagamento = document.querySelector('.meioPagamento-container');
        divMeioPagamento.removeAttribute('hidden');
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

    $scope.aplicarCupom = function () {

        if ($scope.cupomAplicado) {
            utilidadesService.exibirMensagem('Atenção', 'Cupom já aplicado à compra!', false);
        }
        else {

            if (!$scope.onlyCamisetas && $scope.cupom == "3CAMISAS") {
                utilidadesService.exibirMensagem('Atenção', 'Cupom válido se tiver exatamente 3 Camisetas no carrinho!', false);
            }
            else if ($scope.onlyCamisetas && $scope.cupom == "3CAMISAS" && $scope.cupomAplicado == false) {
                $scope.valorTotal = 120;
                $scope.cupomAplicado = true;
            }
            else {
                utilidadesService.exibirMensagem('Atenção', 'Cupom não é válido!', false);
            }

        }

    }

    async function searchViaCep(cep) {
        try {
            // Construa a URL de consulta
            const url = `https://viacep.com.br/ws/${cep}/json/`;

            // Faça a solicitação usando Fetch
            const resposta = await fetch(url);

            // Verifique se a solicitação foi bem-sucedida (status 200 OK)
            if (!resposta.ok) {
                utilidadesService.exibirMensagem('Atenção', 'Erro ao consultar CEP!', false);
            }

            // Parseie a resposta como JSON
            const dadosEndereco = await resposta.json();

            // Verifique se a resposta contém um erro
            if (dadosEndereco.erro) {
                utilidadesService.exibirMensagem('Atenção', 'Cep não encontrado!', false);
            }

            // Retorne os dados do endereço
            return dadosEndereco;
        } catch (erro) {
            console.error(erro.message);
            return null;
        }
    }

    $scope.consultaViaCep = function () {

        console.log($scope.endereco.cep);

        if ($scope.endereco.cep != null &&
            $scope.endereco.cep != '') {
            searchViaCep($scope.endereco.cep)
                .then(dados => {
                    if (dados != null && dados.cep != '') {

                        $scope.$apply(function ($scope) {
                            $scope.endereco.rua = dados.logradouro;
                            $scope.endereco.cidade = dados.localidade;
                            $scope.endereco.bairro = dados.bairro;
                            $scope.endereco.complemento = dados.complemento;
                        });

                    } else {
                        utilidadesService.exibirMensagem('Atenção', 'CEP não encontrado ou ocorreu um erro, digite manualmente.', false);
                    }
                })
                .catch(erro => console.error(erro));
        }
        else {
            utilidadesService.exibirMensagem('Atenção', 'Cep não informado!', false);
        }

    }

}]);