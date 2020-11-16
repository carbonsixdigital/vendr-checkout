$(document).ready(function () {

    window.dataLayer = window.dataLayer || [];

    var dlTestMode = false;

    function Init() {
        PageImpression();
        ProductDetail();
        ProductCheckout();
        ProductPurchase();
    }

    function PageImpression() {
        /*******************************************************************
         * Page Impressions
         ******************************************************************/
        var impDataEvent = "";
        var impType = "";
        var impCurrency = "";
        var imps = [];

        $("[data-method='ProductImpression']")
            .each(function () {
                var element = $(this);
                if (element != undefined) {
                   
                    impDataEvent = GetData(element, "method");
                    impType = GetData(element, "pagetype")
                    impCurrency = GetData(element, "currency");
                  
                    if (element.data("impressions") !== undefined) {
                        var product = GetData(element, "impressions");
                      //  if (!listContainsObjectById(product, imps))
                            imps.push(product);
                    }
                }
            });


        if (imps.length > 0) {          

            if (dlTestMode) {
                console.log("dataEvent:" + impDataEvent);
                console.log("pagetype:" + impType);
                console.log("currency:" + impCurrency);                
                console.log("products:");
                console.log(imps);
            }

            if (!dlTestMode) {
                window.dataLayer.push({
                    'event': impDataEvent,
                    'PageType': impType,
                    'ecommerce': {
                        'currencyCode': impCurrency,
                        'impressions': imps
                    }
                });
            }
        }
    }




    /*******************************************************************
     * Product Detail
     ******************************************************************/

    function ProductDetail() {

        var element = $('[data-method="ProductDetail"]:first');
        if (element.length > 0) {

            var dataEvent = GetData(element, "method");
            var pageType = GetData(element, "pagetype");
            var currency = GetData(element, "currency");
            var products = GetData(element, "products");
            var list = GetData(element, "list");

            if (dlTestMode) {
                console.log("dataEvent:" + dataEvent);
                console.log("pagetype:" + pageType);
                console.log("currency:" + currency);
                console.log("list:" + list);
                console.log("products:");
                console.log(products);
            }

            if (!dlTestMode) {
                dataLayer.push({
                    "event": dataEvent,
                    "PageType": pageType,
                    "ecommerce": {
                        "currencyCode": currency,
                        "detail": {
                            "actionField": {
                                'list': list
                            },
                            "products": products
                        }
                    }
                });
            }
        }
    }



    /*******************************************************************
     * Product Checkout 
     * *****************************************************************/
    function ProductCheckout() {

        var impDataEvent = "";
        var impType = "";
        var impCurrency = "";
        var imps = [];
        var impStep = 1;

        $("[data-method='ProductCheckout'][data-step='1']")
            .each(function () {
                var element = $(this);
                if (element.length > 0) {
                    impDataEvent = GetData(element, "method");
                    impType = GetData(element, "pagetype")
                    impCurrency = GetData(element, "currency");
                    impStep = GetData(element, "step");

                    if (element.data("impressions") !== undefined) {
                        var product = GetData(element, "impressions");
                      //  if (!listContainsObjectById(product, imps))
                            imps.push(product);
                    }
                }
            });

        if (imps.length > 0) {

             if (dlTestMode) {
                console.log("dataEvent:" + impDataEvent);
                console.log("pagetype:" + impType);
                console.log("currency:" + impCurrency);
                console.log("step:" + impStep);
                console.log("products:");
                console.log(imps);
            }

            if (!dlTestMode) {
                dataLayer.push({
                    "event": impDataEvent,
                    "PageType": impType,
                    "ecommerce": {
                        "currencyCode": impCurrency,
                        "checkout": {
                            "step": impStep,
                            "products": imps
                        }
                    }
                });
            }
        }
    }

    /*******************************************************************
     * Product Detail
     ******************************************************************/
    function ProductPurchase() {

        var element = $('[data-method="ProductPurchase"]:first');
        if (element.length > 0) {

            var dataEvent = GetData(element, "method");
            var pageType = GetData(element, "pagetype");
            var currency = GetData(element, "currency");
            var products = GetData(element, "products");
            var orderId = GetData(element, "orderid");
            var totalAmount = GetData(element, "totalamount");
            var coupon = GetData(element, "coupon");

            if (dlTestMode) {
                console.log("dataEvent:" + dataEvent);
                console.log("pagetype:" + pageType);
                console.log("currency:" + currency);
                console.log("orderId:" + orderId);
                console.log("totalAmount:" + totalAmount);
                console.log("coupon:" + coupon);
                console.log("products:");
                console.log(products);
            }

            if (!dlTestMode) {
                dataLayer.push({
                    "event": dataEvent,
                    "PageType": pageType,
                    "ecommerce": {
                        "currencyCode": currency,
                        "purchase": {
                            "actionField": {
                                "id": orderId,
                                "revenue": totalAmount,
                                "coupon": coupon
                            },
                            "products": products
                        }
                    }
                });
            }
        }
    }

    /**************************************************************************************************************************************
     * Events
     *************************************************************************************************************************************/


    /*******************************************************************
     * Product Click
     ******************************************************************/
    $("[data-method='ProductClick']")
        .on('click', function () {

            var element = $(this);
            if (element != undefined) {

                var dataEvent = GetData(element, "method");
                var pageType = GetData(element, "pagetype")
                var currency = GetData(element, "currency");
                var products = GetData(element, "products");

                if (dlTestMode) {
                    console.log("dataEvent:" + dataEvent);
                    console.log("pagetype:" + pageType);
                    console.log("currency:" + currency);
                    console.log("products:");
                    console.log(products);
                }

                if (!dlTestMode) {
                    dataLayer.push({
                        "event": dataEvent,
                        "PageType": pageType,
                        "ecommerce": {
                            "currencyCode": currency,
                            "click": {
                                "products": products
                            }
                        }
                    });
                }
            }
        });

    /*******************************************************************
     * Add To Cart
     ******************************************************************/
    $("[data-method='AddToCart']")
        .on('click', function (e) {

            var element = $(this);
            if (element != undefined) {

                var dataEvent = GetData(element, "method");
                var pageType = GetData(element, "pagetype")
                var currency = GetData(element, "currency");
                var products = GetData(element, "products");
                var list = GetData(element, "list");

                if (dlTestMode) {
                    console.log("dataEvent:" + dataEvent);
                    console.log("pagetype:" + pageType);
                    console.log("currency:" + currency);
                    console.log("list:" + list);
                    console.log("products:");
                    console.log(products);
                }

                if (!dlTestMode) {
                    dataLayer.push({
                        "event": dataEvent,
                        "PageType": pageType,
                        "ecommerce": {
                            "currencyCode": currency,
                            "add": {
                                "actionField": {
                                    'list': list
                                },
                                "products": products
                            }
                        }
                    });
                }
            }
        });


    /*******************************************************************
     * Remove To Cart
     ******************************************************************/
    $("[data-method='RemoveFromCart']")
        .on('click', function (e) {

            var element = $(this);
            if (element != undefined) {

                var dataEvent = GetData(element, "method");
                var pageType = GetData(element, "pagetype")
                var currency = GetData(element, "currency");
                var products = GetData(element, "products");
                var list = GetData(element, "list");

                if (dlTestMode) {
                    console.log("dataEvent:" + dataEvent);
                    console.log("pagetype:" + pageType);
                    console.log("currency:" + currency);
                    console.log("list:" + list);
                    console.log("products:");
                    console.log(products);
                }

                if (!dlTestMode) {
                    dataLayer.push({
                        "event": dataEvent,
                        "PageType": pageType,
                        "ecommerce": {
                            "currencyCode": currency,
                            "remove": {
                                "actionField": {
                                    'list': list
                                },
                                "products": products
                            }
                        }
                    });
                }
            }
        });

    /*******************************************************************
     * Product Checkout - Step
     *****************************************************************/
    $("[data-method='ProductCheckout']")
        .on('click', function (e) {
            var element = $(this);

            if (element !== undefined) {

                var dataEvent = GetData(element, "method");
                var pagetype = GetData(element, "pagetype");
                var currency = GetData(element, "currency");
                var step = GetData(element, "step");
                var option = GetData(element, "option");

                if (dlTestMode) {
                    console.log("dataEvent:" + dataEvent);
                    console.log("pagetype:" + pagetype);
                    console.log("currency:" + currency);
                    console.log("step:" + step);
                    console.log("option:" + option);
                }

                if (!dlTestMode) {
                    dataLayer.push({
                        "event": dataEvent,
                        "PageType": pagetype,
                        "ecommerce": {
                            "currencyCode": currency,
                            "checkout": {
                                "step": step,
                                "option": option
                            }
                        }
                    });
                }
            }
        });


    /*******************************************************************
     * Utils 
     *****************************************************************/
    function GetData(element, alias) {
        return element.is('[data-' + alias + ']') ? element.data(alias) : "";
    }

    function listContainsObjectById(product, list) {
        var x;
        for (x = 0; x < list.length; x++) {
            if (list[x].id == product.id) {
                return true;
            }
        }
        return false;
    }

    /******************************************************************/
    Init();
});