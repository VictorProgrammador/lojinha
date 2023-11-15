app.factory("basicService", ["$http", function ($http) {

    const headers = {
        'Content-Type': 'application/json'
    };

    return {
        sendPassword: function (param) {
            return $http.post("/Home/SendPassword", param, headers);
        },
        registerUser: function (param) {
            return $http.post("/Home/Register", param, headers);
        },
        loginUser: function (param) {
            return $http.post("/Home/Login", param, headers);
        },
        getCategories: function () {
            return $http.get("/Home/GetCategories");
        },
        getMyProfile: function () {
            return $http.get("/User/GetMyProfile");
        },
        getProfile: function (param) {
            return $http.post("/Home/GetProfile", param, headers);
        },
        getPersonsBusiness: function (param) {
            return $http.post("/Home/GetPersonsBusiness", param, headers);
        },
        getProducts: function (param) {
            return $http.post("/Home/GetProducts", param, headers);
        },
        saveMyProfile: function (param) {
            return $http.post("/User/SaveMyProfile", param, headers);
        },
        updatePersonPhoto: function (param, headerProfile) {
            return $http.post("/User/UpdatePersonPhoto", param, headerProfile);
        },
        updatePersonBannerPhoto: function (param, headerProfile) {
            return $http.post("/User/UpdatePersonBannerPhoto", param, headerProfile);
        },
        saveMyBusiness: function (param) {
            return $http.post("/User/SaveMyBusiness", param, headers);
        },
        saveBusinessClassification: function (param) {
            return $http.post("/Home/SaveBusinessClassification", param, headers);
        },
        getRateProfileByPerson: function (param) {
            return $http.post("/Home/GetRateProfileByPerson", param, headers);
        },
        getMyProducts: function (param) {
            return $http.post("/User/GetMyProducts", param, headers);
        },
        getPersonBusinessProducts: function (param) {
            return $http.post("/Home/GetPersonBusinessProducts", param, headers);
        },
        getMyComplements: function (param) {
            return $http.post("/User/GetMyComplements", param, headers);
        },
        getMyRaffles: function (param) {
            return $http.post("/User/GetMyRaffles", param, headers);
        },
        getRaffleParticipants: function (param) {
            return $http.post("/User/GetRaffleParticipants", param, headers);
        },
        getProfileRaffles: function (param) {
            return $http.post("/Home/GetProfileRaffles", param, headers);
        },
        savePersonBusinessCheckin: function (param) {
            return $http.post("/Home/SavePersonBusinessCheckin", param, headers);
        },
        getMyArchives: function (param) {
            return $http.post("/User/GetMyArchives", param, headers);
        },
        getPersonBusinessArchives: function (param) {
            return $http.post("/Home/GetPersonBusinessArchives", param, headers);
        },
        createOrder: function (param) {
            return $http.post("/CheckoutApi/CreateOrder", param, headers);
        },
        getProduct: function (param) {
            return $http.post("/Home/GetProduct", param, headers);
        },
        saveProductCart: function (param) {
            return $http.post("/Home/SaveProductCart", param, headers);
        },
        getCart: function (param) {
            return $http.post("/User/GetCart", param, headers);
        },
        excludeCartProduct: function (param) {
            return $http.post("/User/ExcludeCartProduct", param, headers);
        },
        searchCEP: function (param) {
            return $http.post("/User/SearchCEP", param, headers);
        },
        createOrder: function (param) {
            return $http.post("/User/CreateOrder", param, headers);
        },
        getOrder: function (param) {
            return $http.post("/User/GetOrder", param, headers);
        },
        getOrders: function (param) {
            return $http.post("/User/GetOrders", param, headers);
        },
    };

}]);