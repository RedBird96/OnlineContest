var PickEm = (function() {

    var init = function() {
        PickEm.bindPageEvents();
        if ($("#stockNumber").val() == 0) {
            $("#stockNumber").val('');
        }

        if ($("#preDateValue").val() != 0) {
            $('#expiryDate').val($("#preDateValue").val());
        }

        if ($("#preStyleValue").val() != 0) {
            $('#style').val($("#preStyleValue").val());
        }

        var val = $("#style").val();
        if (val == 2) {
            $("#totalDollars").show();
            $("#maxDollars").show();
        } else {
            $("#totalDollars").hide();
            $("#maxDollars").hide();
        }

        var val = $("#expiryDate").val();
        if (val == 4) {
            $("#customeDate").show();
        } else {
            $("#customeDate").hide();
        }
    };

    var bindPageEvents = function() {
        $('#btnCopy').click(function () {
            
            $("#linkINput").css({ "display": "block" });
            $("#linkINput").val($("#judgmentLink").attr("href"))
            var copyText = document.getElementById("linkINput");
            copyText.select();

            document.execCommand("copy");
            $("#linkINput").css({ "display": "none" });

        });

        $("#style").on("change",
            function () {
                var val = $("#style").val();
                if (val == 2) {
                    $("#totalDollars").show();
                    $("#maxDollars").show();
                } else {
                    $("#totalDollars").hide();
                    $("#maxDollars").hide();
                }
            });

        $("#expiryDate").on("change",
            function () {
                var val = $("#expiryDate").val();
                if (val == 4) {
                    $("#customeDate").show();
                } else {
                    $("#customeDate").hide();
                }
            });

        var now = new Date(),
            // minimum date the user can choose, in this case now and in the future
            minDate = now.toISOString().substring(0, 10);

        $('#date').prop('min', minDate);


        $("#publish").on("click",
            function () {
               
                var id = $("#id").val();
                var companyId = $("#companyId").val();
                var contestId = $("#contestId").val();
                var stockNumber = $("#stockNumber").val();
                var expirationType = $("#expiryDate").val();
                var expirayDate = $("#date").val();
                var style = $("#style").val();
                var totalDollars = $("#totalDollarsVal").val();
                var maxDollars = $("#maxDollar").val();
                var style = $("#style").val();
                
                if (!stockNumber) {
                    alert("Please select stock number");
                    return;
                } else if (expirationType == 4 && !expirayDate) {
                    alert("Please select custom date");
                    return;
                } else if (style == 2 && !totalDollars) {
                    alert("Please select total dollars");
                    return;
                } else if (style == 2 && !maxDollars) {
                    alert("Please select max dollars");
                    return;
                } else if (style == 2 && parseInt(maxDollars) > parseInt(totalDollars)) {
                    alert("Max dollars should be less than total dollars");
                    return;
                }

                var question = new Object();
                question.Id = id;
                question.CompanyId = companyId;
                question.ContestId = contestId;
                question.StockNumber = stockNumber;
                question.ExpirationType = expirationType;
                question.ExpirationDate = expirayDate;
                question.DollarsPerPoint = totalDollars;
                question.MaxDollars = maxDollars;
                question.StyleType = style;

                $.ajax({
                    type: "POST",
                    url: "/Judgement/CreateAndPublishPickEmQuestion",
                    data: JSON.stringify(question),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.isError) {
                            alert(response.message);
                        } else {
                            location.reload();
                        }
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    },
                    error: function (response) {
                        alert(response.responseText);
                    }
                });
            });

    };

    var StockChange = function() {
        var val = parseInt($("#stockNumber").val());
        if (val < 1 || val > 10) {
            $("#stockNumber").val('');
            alert("Stock value can not be less than 1 or greater than 10");
        }
    };

    return {
        init: init,
        bindPageEvents: bindPageEvents,
        StockChange: StockChange
    }

})();


$(document).ready(function() {
    PickEm.init();
});

