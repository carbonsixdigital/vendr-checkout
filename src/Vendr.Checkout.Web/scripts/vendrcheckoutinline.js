(function () {
    window.dataLayer = window.dataLayer || [];

    var dlTestMode = false;

    // Initialization
    function initSm() {

        $("body").on("click.sm", "#order-summary-toggle", function (e) {
            e.preventDefault();
            var toShow = $("#order-summary").hasClass("hidden");
            if (toShow) {
                showMobileOrderSummary();
            } else {
                hideMobileOrderSummary();
            }
        });

        hideMobileOrderSummary();

    }

    function initLg() {
        $("body").off(".sm");
    }

    function GetData(element, alias) {
        return element.is('[data-' + alias + ']') ? element.data(alias) : "";
    }

    function ProductCheckoutStep(element) {
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
    }

    function initCommon() {

        initAjaxForms($("body"))

        $("body").on("click", "[data-ajax-pay]", function (e) {
            e.preventDefault();
            ProductCheckoutStep($(this));
            showOverlays();
            $.get("?altTemplate=VendrCheckoutAjaxPartial&partial=VendrCheckoutPaymentForm", function (markup) {
                $("#ajax-payment-form").empty();
                $("#ajax-payment-form").append($(markup));
                $("#ajax-payment-form").find("form").submit();
            });
        });

        // Load ajax links via ajax
        $("body").on("click", "[data-ajax-link]", function (e) {
            e.preventDefault();

            ProductCheckoutStep($(this));

            var self = $(this);

            if (self.attr("href")) {

                showOverlays();

                $.get(self.attr("href"), function (data) {
                    if (data.success) {
                        loadAjaxView(self.data("ajaxLink"));
                    } else {
                        handleAjaxErrors(data.errors);
                    }
                });

            } else {
                loadAjaxView(self.data("ajaxLink"));
            }

        });

        // Auto submit ajax radio buttons
        $("body").on("change", "[data-ajax-radio]", function (e) {
            ProductCheckoutStep($(this));
            $(this).closest("form").find("button[type=submit]").trigger("click");
        });

        // Toggle shipping address display
        $("body").on("click", "input[name=shippingSameAsBilling]", function () {
            showHideShippingInfo(true);
        });

        // Display billing address regions if any
        $("body").on("change", "select[name='billingAddress.Country']", function () {
            showRegions("billing", $(this).children("option:selected").data("regions"));
        }).triggerHandler("change");

        // Display shipping address regions if any
        $("body").on("change", "select[name='shippingAddress.Country']", function () {
            showRegions("shipping", $(this).children("option:selected").data("regions"));
        }).triggerHandler("change");

        // Handle accept terms
        $("body").on("click", "#acceptTerms", function () {
            enableDisablePaymentButton();
        });

        //var errors = $(".validation-summary-errors");
        //if (errors.length > 0) {
        //    Toastify({
        //        text: errors.find("ul li:first-child").text(),
        //        duration: 3000,
        //        gravity: "bottom", // `top` or `bottom`
        //        position: 'center', // `left`, `center` or `right`
        //        backgroundColor: "#f56565",
        //        className: "",
        //        stopOnFocus: true, // Prevents dismissing of toast on hover
        //    }).showToast();
        //}

        showHideShippingInfo(false);
    }

    function initAjaxForms(el) {

        el.find("[data-ajax-form]").each(function () {
            var self = $(this);
            self.ajaxForm({
                beforeSubmit: function () {
                    showOverlays();
                },
                success: function (data) {
                    if (data.success) {
                        loadAjaxView(self.data("ajaxView"));
                    } else {
                        handleAjaxErrors(data.errors);
                    }
                }
            });
        });

    }

    function handleAjaxErrors(errors) {
        Toastify({
            text: errors[0],
            duration: 3000,
            gravity: "bottom", // `top` or `bottom`
            position: 'center', // `left`, `center` or `right`
            backgroundColor: "#f56565",
            className: "",
            stopOnFocus: true, // Prevents dismissing of toast on hover
        }).showToast();
        hideOverlays();
    }

    function loadAjaxView(view) {

        // Show the overlay
        showOverlays();

        // Fetch the view
        $.get(view, function (markup) {
            var $markup = $(markup);

            // Replace the order summary toggle
            var orderSummaryToggle = $markup.find("#order-summary-toggle");
            if (!isEmpty(orderSummaryToggle)) {
                $(document).find("#order-summary-toggle").replaceWith(orderSummaryToggle);
                initAjaxForms($(document).find("#order-summary-toggle"));
            }

            // Replace the order summary
            var orderSummary = $markup.find("#order-summary");
            if (!isEmpty(orderSummary)) {
                $(document).find("#order-summary").replaceWith(orderSummary);
                initAjaxForms($(document).find("#order-summary"));
            }

            // Replace the body
            var body = $markup.find("#body");
            if (!isEmpty(body)) {
                $(document).find("#body").replaceWith(body);
                initAjaxForms($(document).find("#body"));
            }

            // Show hide shipping info
            showHideShippingInfo(false);

            // Enable / disable continue button
            enableDisablePaymentButton();

            // Hide the overlay
            hideOverlays();
        });
    }

    function isEmpty(el) {
        return !$.trim(el.html())
    }

    function showOverlays() {
        $("[data-overlay]").removeClass("hidden");
    }

    function hideOverlays() {
        $("[data-overlay]").addClass("hidden");
    }

    function showMobileOrderSummary() {
        $("#order-summary").removeClass("hidden");
        $("#order-summary-toggle__text-open").removeClass("hidden");
        $("#order-summary-toggle__text-closed").addClass("hidden");
    }

    function hideMobileOrderSummary() {
        $("#order-summary").addClass("hidden");
        $("#order-summary-toggle__text-open").addClass("hidden");
        $("#order-summary-toggle__text-closed").removeClass("hidden");
    }

    function showHideShippingInfo(clearValues) {
        var hideShippingInfo = $("input[name=shippingSameAsBilling]").is(":checked");
        if (hideShippingInfo) {
            $("#shipping-info").hide();
        } else {
            if (clearValues) {
                //$("input[type=text][name^=shipping]").val("");
            }
            $("#shipping-info").show();
        }
    }

    function showRegions(addressType, regions) {

        var sl = $("select[name='" + addressType + "Address.Region']");
        var slVal = sl.data("value");

        sl.empty();

        var containsValue = false;

        regions.forEach(function (itm, idx) {
            sl.append($('<option>', {
                value: itm.id,
                text: itm.name
            }));
            if (slVal && itm.id === slVal)
                containsValue = true;
        });

        if (containsValue) {
            sl.val(slVal);
        }

        if (regions.length > 0) {
            sl.removeClass("hidden")
                .addClass("block");
        } else {
            sl.removeClass("block")
                .addClass("hidden");
        }

    };

    function enableDisablePaymentButton() {

        var enableContinueButton = $("#acceptTerms").is(":checked");
        if (enableContinueButton) {
            $("[data-ajax-pay]").attr("disabled", false)
                .css("backgroundColor", "")
                .css("color", "")
                .css("cursor", "");
        } else {
            $("[data-ajax-pay]").attr("disabled", true)
                .css("backgroundColor", "#f7fafc")
                .css("color", "#ddd")
                .css("cursor", "no-drop");
        }

    }

    // Setup responsive states
    ssm.addState({
        id: 'sm',
        query: '(max-width: 1023px)',
        onEnter: initSm
    });

    ssm.addState({
        id: 'lg',
        query: '(min-width: 1024px)',
        onEnter: initLg
    });

    initCommon();

})();