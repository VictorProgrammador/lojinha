$(document).ready(function () {
    /**
     * Navbar links active state on scroll
     */
    let navbarlinks = $('#navbar .scrollto');
    const navbarlinksActive = () => {
        let position = $(window).scrollTop() + 200;
        navbarlinks.each(function () {
            let section = $($(this).attr('href'));
            if (section.length && position >= section.offset().top && position <= (section.offset().top + section.outerHeight())) {
                $(this).addClass('active');
            } else {
                $(this).removeClass('active');
            }
        });
    };
    $(window).on('load', navbarlinksActive);
    $(window).on('scroll', navbarlinksActive);

    /**
     * Scrolls to an element with header offset
     */
    const scrollto = (el) => {
        let header = $('#header');
        let offset = header.outerHeight() || 0;

        let elementPos = $(el).offset().top;
        $('html, body').animate({
            scrollTop: elementPos - offset
        }, 'slow');
    };

    /**
     * Toggle .header-scrolled class to #header when page is scrolled
     */
    let selectHeader = $('#header');
    if (selectHeader.length) {
        const headerScrolled = () => {
            if ($(window).scrollTop() > 100) {
                selectHeader.addClass('header-scrolled');
            } else {
                selectHeader.removeClass('header-scrolled');
            }
        };
        $(window).on('load', headerScrolled);
        $(window).on('scroll', headerScrolled);
    }

    /**
     * Back to top button
     */
    let backtotop = $('.back-to-top');
    if (backtotop.length) {
        const toggleBacktotop = () => {
            if ($(window).scrollTop() > 100) {
                backtotop.addClass('active');
            } else {
                backtotop.removeClass('active');
            }
        };
        $(window).on('load', toggleBacktotop);
        $(window).on('scroll', toggleBacktotop);
    }

    /**
     * Mobile nav toggle
     */
    $(document).on('click', '.mobile-nav-toggle', function (e) {
        $('#navbar').toggleClass('navbar-mobile');
        $('.mobile-nav-toggle').toggleClass('bi-list bi-x');
    });

    /**
     * Mobile nav dropdowns activate
     */
    $(document).on('click', '.navbar .dropdown > a', function (e) {
        if ($('#navbar').hasClass('navbar-mobile')) {
            e.preventDefault();
            $(this).next('.dropdown-menu').toggleClass('dropdown-active');
        }
    });

    $(document).on('click', '.dropdown-toggle', function (e) {
        let iconCategory = $('.iconCategory');
        if (iconCategory.length) {
            iconCategory.toggleClass('bi-filter bi-x-lg');
        }
        $('.dropdown-menu').toggleClass('dropdown-menu-active');
    });

    $(document).on('click', 'main', function (e) {
        $('.dropdown-menu').removeClass('dropdown-menu-active');
        let iconCategory = $('.iconCategory');
        iconCategory.removeClass('bi-x-lg').addClass('bi-filter');
    });

    $(document).on('click', '.headerFilter', function (e) {
        $('.contentMarca').toggleClass('openMarcas')
        // Adicione sua lógica aqui para manipular o clique em '.contentMarcas'
    });

    $(document).on('click', '.btnFilter', function (e) {
        $('.contentMarca').removeClass('openMarcas')
        // Adicione sua lógica aqui para manipular o clique em '.contentMarcas'
    });

    /**
     * Scrool with ofset on links with a class name .scrollto
     */
    $(document).on('click', '.scrollto', function (e) {
        if ($($(this).attr('href')).length) {
            e.preventDefault();

            let navbar = $('#navbar');
            if (navbar.hasClass('navbar-mobile')) {
                navbar.removeClass('navbar-mobile');
                $('.mobile-nav-toggle').toggleClass('bi-list bi-x');
            }
            scrollto($(this).attr('href'));
        }
    });

    /**
     * Scroll with offset on page load with hash links in the URL
     */
    if (window.location.hash && $($(window.location.hash)).length) {
        scrollto(window.location.hash);
    }

    /**
     * Preloader
     */
    let preloader = $('#preloader');
    if (preloader.length) {
        $(window).on('load', function () {
            preloader.remove();
        });
    }
    $(window).on('load', function () {
        Grade($('.box'));
    });

    $('#zoomable-image').on('mouseenter', function () {
        $(this).css('width', '500px'); // Largura aumentada ao passar o mouse
    });

    $('#zoomable-image').on('mouseleave', function () {
        $(this).css('width', '300px'); // Largura inicial ao retirar o mouse
    });

    function increaseQuantity() {
        var quantityInput = document.getElementById('quantityInput');
        var productValue = document.getElementById('productValue').getAttribute('data-value');

        var currentValue = parseInt(quantityInput.value);
        quantityInput.value = currentValue + 1;

        calculateTotal(productValue);
    }

    function decreaseQuantity() {
        var quantityInput = document.getElementById('quantityInput');
        var productValue = document.getElementById('productValue').getAttribute('data-value');

        var currentValue = parseInt(quantityInput.value);
        if (currentValue > 1) {
            quantityInput.value = currentValue - 1;
            calculateTotal(productValue);
        }
    }

    function calculateTotal(productValue) {
        var quantityInput = document.getElementById('quantityInput');
        var totalValue = document.getElementById('totalValue');

        var quantity = parseInt(quantityInput.value);
        var value = parseFloat(productValue);

        var total = quantity * value;
        totalValue.innerText = "Total: " + total.toFixed(2); // Exibe o total formatado com 2 casas decimais
    }

    var swiperCategories = new Swiper(".mySwiperCategories", {
        slidesPerView: 1,
        spaceBetween: 10,
        pagination: {
            el: ".swiper-pagination",
            clickable: true,
        },
        breakpoints: {
            "@0.00": {
                slidesPerView: 4,
                spaceBetween: 10,
            },
            "@0.75": {
                slidesPerView: 4,
                spaceBetween: 20,
            },
            "@1.00": {
                slidesPerView: 7,
                spaceBetween: 40,
            },
            "@1.50": {
                slidesPerView: 8,
                spaceBetween: 30,
            },
        },
    });

    var swiperSubCategories = new Swiper(".mySwiperSubCategories", {
        slidesPerView: 1,
        spaceBetween: 10,
        pagination: {
            el: ".swiper-pagination",
            clickable: true,
        },
        breakpoints: {
            "@0.00": {
                slidesPerView: 2,
                spaceBetween: 0,
            },
            "@0.75": {
                slidesPerView: 5,
                spaceBetween: 20,
            },
            "@1.00": {
                slidesPerView: 7,
                spaceBetween: 40,
            },
            "@1.50": {
                slidesPerView: 10,
                spaceBetween: 50,
            },
        },
        navigation: {
            nextEl: ".swiper-button-next",
            prevEl: ".swiper-button-prev",
          },
    });

    var swiperContainerCategories = new Swiper(".container-categorias", {
        slidesPerView: 1,
        spaceBetween: 10,
        pagination: {
            el: ".swiper-pagination",
            clickable: true,
        },
        breakpoints: {
            "@0.00": {
                slidesPerView: 4,
                spaceBetween: 10,
            },
            "@0.75": {
                slidesPerView: 4,
                spaceBetween: 20,
            }
        },
    });
});
