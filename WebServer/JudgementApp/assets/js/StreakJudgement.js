var Judgment = (function () {

    var init = function () {
        Judgment.bindPageEvents();
    };

    var bindPageEvents = function () {
        var typingTimer;                //timer identifier
        var doneTypingInterval = 1000;  //time in ms, 5 second for example
        var $input = $('#UserEmail');

        //on keyup, start the countdown
        $input.on('keyup', function () {
            clearTimeout(typingTimer);
            typingTimer = setTimeout(doneTyping, doneTypingInterval);
        });

        //on keydown, clear the countdown 
        $input.on('keydown', function () {
            clearTimeout(typingTimer);
        });

        //user is "finished typing," do something
        function doneTyping() {
            $.ajax({
                type: "GET",
                url: "/Judgement/GetUserStreak",
                data: {
                    email: $("#UserEmail").val(),
                    id: $("#contestId").val()
                },
                dataType: "json",
                success: function (response) {
                    $("#streakV").text(response);
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        }
       
        $("#hitbtn").on("click",
            function () {
                var email = $("#UserName").val();
                if (email) {
                    window.location =
                        "/streak-history-" + $("#companyName").val() + "-" + $("#contestName").text() + "/" + $("#id").val() + "?email=" + email;
                    return false;
                } else {
                    alert("Please enter username to see history.");
                }
                
            });


        $("#submit").on("click",
            function () {
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

                debugger;
                var stock = $("#stock").val();
                var updownValue = $("#togBtn").is(":checked") ? "Up" : "Down";

                var result = new Object();
                result.Username = username;
                result.Email = email;
                result.Stock = stock;
                result.Value = updownValue;
                result.CompanyId = $("#companyId").val();
                result.ContestId = $("#contestId").val();
                result.QuestionId = $("#id").val();

                $.ajax({
                    type: "POST",
                    url: "/Judgement/SaveStreakJudgment",
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

$(document).ready(function () {
    Judgment.init();
});