var Judgment = (function() {

    var init = function() {
        Judgment.bindPageEvents();
    };

    var bindPageEvents = function () {
       
        $("#submit").on("click",
            function() {
                var username = $("#UserName").val();
                var email = $("#UserEmail").val();

                if (!username) {
                    alert("Please enter username to submit results");
                    return;
                }

                if (!email) {
                    alert("Please enter email to submit results");
                    return;
                }

                var n = parseInt($("#stockNumbers").val());
                var styleType = parseInt($("#styleType").val());
                var stock = [];
                var amount = [];
                for (var i = 0; i < n; i++) {
                    stock.push($("#st-" + i).val().trim());
                    
                    if (styleType != 1) {
                        var amountVal = $("#am-" + i).val().trim();
                        if (!amountVal) {
                            alert("Please enter the all amounts.");
                            return;
                        } else if (amountVal > parseInt($("#totalValue").val()) && parseInt($("#totalValue").val()!=0)) {
                            alert("Amount value should be less than " + $("#totalValue").val());
                            return;
                        }
                        
                        amount.push(amountVal);
                    }
                    
                }

                var sum = 0;
                for (var i = 0; i < amount.length; i++) {
                    sum = sum + parseInt(amount[i]);
                }

                if (sum > parseInt($("#maxValue").val())) {
                    alert("Sum of all value should be less than " + $("#maxValue").val());
                    return;
                }

                var findDuplicates = arr => arr.filter((item, index) => arr.indexOf(item) != index);
                if (findDuplicates(stock).length > 0) {
                    alert("Please select different stocks");
                    return;
                }

                var result = new Object();
                result.Username = username;
                result.Email = email;
                result.Stocks = stock.toString();
                result.Amounts = amount.toString();
                result.CompanyId = $("#companyId").val();
                result.ContestId = $("#contestId").val();
                result.QuestionId = $("#id").val();

                $.ajax({
                    type: "POST",
                    url: "/Judgement/SavePickEmJudgment",
                    data: JSON.stringify(result),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.isError) {
                            alert(response.message);
                        } else {
                            alert("Results submitted successfully");
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
    }

    return {
        init: init,
        bindPageEvents: bindPageEvents
    }
})();

$(document).ready(function() {
    Judgment.init();
});