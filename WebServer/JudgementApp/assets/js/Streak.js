var Streak = (function () {

    var init = function () {
        Streak.bindPageEvents();
        
    };

    var bindPageEvents = function () {

        

        $('#btnCopy').click(function () {

            $("#linkINput").css({ "display": "block" });
            $("#linkINput").val($("#judgmentLink").attr("href"))
            var copyText = document.getElementById("linkINput");
            copyText.select();

            document.execCommand("copy");
            $("#linkINput").css({ "display": "none" });

        });

        var items = $("#selectedStocks").val();
        var selectedStocks = items.split(',');

        for (var i = 0; i < selectedStocks.length; i++) {
            if (selectedStocks[i] == "all") {
                $("#stock").attr('checked', true);
                $("#stock").prop('disabled', true);

                $("#etf").attr('checked', true);
                $("#etf").prop('disabled', true);

                $("#forex").attr('checked', true);
                $("#forex").prop('disabled', true);

                $("#crypto").attr('checked', true);
                $("#crypto").prop('disabled', true);

                $("#all").attr('checked', true);
            }

            if (selectedStocks[i] == "stock") {
                $("#stock").attr('checked', true);
            }
            if (selectedStocks[i] == "etf") {
                $("#etf").attr('checked', true);
            }
            if (selectedStocks[i] == "forex") {
                $("#forex").attr('checked', true);
            }
            if (selectedStocks[i] == "crypto") {
                $("#crypto").attr('checked', true);
            }
            
        }

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

                var selectedStocks = [];
                if ($('#all').is(":checked")) {
                    selectedStocks.push("all");
                } else {
                    if ($('#stock').is(":checked")) {
                        selectedStocks.push("stock");
                    }
                    if ($('#etf').is(":checked")) {
                        selectedStocks.push("etf");
                    }
                    if ($('#forex').is(":checked")) {
                        selectedStocks.push("forex");
                    }
                    if ($('#crypto').is(":checked")) {
                        selectedStocks.push("crypto");
                    }
                }

                var date = $("#date").val();
                

                if (!selectedStocks.length > 0) {
                    alert("Please select at least one symbol type.");
                    return;
                }
                
                var question = new Object();
                question.Id = id;
                question.CompanyId = companyId;
                question.ContestId = contestId;
                question.EndDate = date;
                question.SelectedStocks = selectedStocks.toString();

                var r = false;
                if ($("#publish1").val()) {
                    r = confirm("This contest name already exists. If you publish again it will delete the existing contest. Press OK to continue. Change the name of the contest to create a separate contest.");
                } else {
                    r = true;
                }
                if (r == true) {
                    $.ajax({
                        type: "POST",
                        url: "/Judgement/CreateAndPublishStreakQuestion",
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
                }
                
            });

        $("#all").change(function () {
            if (this.checked) {
                $("#stock").attr('checked', true);
                $("#stock").prop('disabled', true);

                $("#etf").attr('checked', true);
                $("#etf").prop('disabled', true);

                $("#forex").attr('checked', true);
                $("#forex").prop('disabled', true);

                $("#crypto").attr('checked', true);
                $("#crypto").prop('disabled', true);
            } else {
                $("#stock").attr('checked', false);
                $("#stock").prop('disabled', false);

                $("#etf").attr('checked', false);
                $("#etf").prop('disabled', false);

                $("#forex").attr('checked', false);
                $("#forex").prop('disabled', false);

                $("#crypto").attr('checked', false);
                $("#crypto").prop('disabled', false);
            }
        });


    };

   

    return {
        init: init,
        bindPageEvents: bindPageEvents,

    }

})();


$(document).ready(function () {
    Streak.init();
});

