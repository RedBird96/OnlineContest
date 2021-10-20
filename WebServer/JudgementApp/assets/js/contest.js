var Contest = (function() {
    var init = function() {
        Contest.bindPageEvents();
    };

    var bindPageEvents = function() {
        $("#fileUpload").on("change",
            function() {
                var fileUpload = $("#fileUpload").get(0);
                var files = fileUpload.files;

                // Create FormData object  
                var fileData = new FormData();

                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    fileData.append(files[i].name, files[i]);
                }


                $.ajax({
                    url: '/Judgement/UploadFile',
                    type: "POST",
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: fileData,
                    success: function(result) {
                        $("#logo").val(result);
                        $("#CompanyLogo").attr("src", "/assets/CompanyLogos/"+result);
                    },
                    error: function(err) {
                        alert(err.statusText);
                    }
                });
            });

        $("#contentList").on("click", function () {
            var companyName = $("#companyName").text();
            var companyId = $("#companyId").val();
            if (!companyId) {
                alert("Please select valid company");
                return;
            } else if (!companyName) {
                alert("Please select valid company");
                return;
            }

            window.location.href = "/" + companyName + "-content-list/" + companyId;
        });

        $("#createContest").on("click",
            function() {
                var companyId = $("#companyId").val();
                var companyName = $("#companyName").text();
                var logo = $("#logo").val();
                var contestname = $("#Contestname").val();
                var contestType = $("#contestType").val();

                if (!companyId) {
                    alert("Please select valid company");
                    return;
                } else if (!companyName) {
                    alert("Please select valid company");
                    return;
                } else if (!contestname) {
                    alert("Please select contest name");
                    return;
                }

                var contest = new Object();
                contest.CompanyId = companyId;
                contest.CompanyName = companyName;
                contest.LogoPath = logo;
                contest.ContestName = contestname;
                contest.ContestType = contestType;

                $.ajax({
                    type: "POST",
                    url: "/Judgement/CreateContest",
                    data: JSON.stringify(contest),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.isError) {
                            alert(response.message);
                        } else {
                            if (response.data == 1) {
                                window.location.href = "/contest-admin/" + response.id;
                            }else if (response.data == 2) {
                                window.location.href = "/contest-pickem-" + $("#companyName").text() + "-" + contestname+"/" + response.id;
                            } else if (response.data == 3) {
                                window.location.href = "/contest-streak-" + $("#companyName").text() + "-" + contestname + "/" + response.id;
                            }
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

    var fillCompanyData = function () {
        var id = $("#companyId").val();
        $.ajax({
            type: "GET",
            url: '/Judgement/SearchCompany',
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.CompanyName) {
                    $("#companyName").text(data.CompanyName);
                    $("#CompanyLogo").attr('src', "/assets/companyLogos/" + data.Logo);
                    $("#logo").val(data.Logo);
                } else {
                    alert("No company exist with Id:" + id);
                    $("#companyId").val('');
                    $("#companyName").text('');
                }

            },
            error: function (e) {
                alert(e);
            }


        });
    };

    return {
        init: init,
        bindPageEvents: bindPageEvents,
        fillCompanyData: fillCompanyData
    }
})();


$(document).ready(function() {
    Contest.init();
});