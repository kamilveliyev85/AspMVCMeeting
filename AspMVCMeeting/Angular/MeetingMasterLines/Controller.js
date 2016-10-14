app.controller("linesCntrl", function ($scope, $http, upload, linesService) {

    //BEGIN Master meeting files create

    $scope.UploadMasterFileCreate = function () {
        upload({
            url: '/tr/MeetingMaster/upload',
            method: 'POST',
            data: {
                aFile: $scope.myFile
            }
        }).then(
          function (response) {
              $scope.result = response.data;
              console.log(response.data);
          },
          function (response) {
              console.error(response);
          }
        );
    }

    $scope.removeFile = function (fileName) {
        upload({
            url: '/tr/MeetingMaster/RemoveFile',
            method: 'POST',
            data: {
                fileName: fileName
            }
        }).then(
          function (response) {
              $scope.result = response.data;
              console.log(response.data);
          },
          function (response) {
              console.error(response);
          }
        );
    }

    //END Master meeting files create

    //BEGIN Master meeting files edit
    GetAllFiles();

    function GetAllFiles() {
        var getData = linesService.GetAllFiles(angular.element("#MEETING_LINES_MTL_MT_REF").val());
        getData.then(function (emp) {
            $scope.files = emp.data;
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.removeFileByID = function (fileId) {
        var getData = linesService.removeFileByID(fileId);
        getData.then(function (emp) {
            console.log(emp.data);
            GetAllFiles();
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.UploadMasterFileEdit = function () {
        upload({
            url: '/tr/MeetingMaster/UploadFile',
            method: 'POST',
            data: {
                aFile: $scope.myFile,
                MT_REF: angular.element("#MEETING_LINES_MTL_MT_REF").val()
            }
        }).then(
          function (response) {
              GetAllFiles();
              console.log(response.data);
          },
          function (response) {
              console.error(response);
          }
        );
    }

    //END Master meeting files edit


    //BEGIN Master LINES files create

    $scope.UploadLineFileCreate = function () {
        upload({
            url: '/tr/MeetingMaster/UploadLineFileCreate',
            method: 'POST',
            data: {
                aFile: $scope.myFile
            }
        }).then(
          function (response) {
              $scope.resultLine = response.data;
              console.log(response.data);
          },
          function (response) {
              console.error(response);
          }
        );
    }

    $scope.removeLineFile = function (fileName) {
        upload({
            url: '/tr/MeetingMaster/RemoveLineFile',
            method: 'POST',
            data: {
                fileName: fileName
            }
        }).then(
          function (response) {
              $scope.resultLine = response.data;
              console.log(response.data);
          },
          function (response) {
              console.error(response);
          }
        );
    }

    //END Master meeting LINES create

    //BEGIN Master meeting LINES edit

    function GetAllLineFiles(lineId) {
        var getData = linesService.GetAllLineFiles(lineId);
        getData.then(function (emp) {
            $scope.lineFiles = emp.data;
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.removeLineFileByID = function (fileId) {
        var getData = linesService.removeLineFileByID(fileId);
        getData.then(function (emp) {
            console.log(emp.data);
            GetAllLineFiles(angular.element("#lineId").val());
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.UploadLineFileEdit = function () {
        upload({
            url: '/tr/MeetingMaster/UploadLineFileEdit',
            method: 'POST',
            data: {
                aFile: $scope.myFile,
                lineId: angular.element("#lineId").val()
            }
        }).then(
          function (response) {
              GetAllLineFiles(angular.element("#lineId").val());
              console.log(response.data);
          },
          function (response) {
              console.error(response);
          }
        );
    }

    //END Master meeting LINES edit

    GetLineAll();

    function GetLineAll() {
        var getData = linesService.GetLinesAll(angular.element("#MEETING_LINES_MTL_MT_REF").val());
        getData.then(function (emp) {
            $scope.lines = emp.data;
        }, function (response) {
            alert('Error in getting records');
        });
    }

    function formatDate(date) {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [day, month, year].join('.');
    }


    $scope.editLine = function (line) {
        $scope.fileCreate = false;
        $scope.lineFiles = '';
        $scope.resultLine = '';
        var getData = linesService.getLine(line.MEETING_LINES.ID);
        getData.then(function (emp) {
            $scope.line = emp.data;
            $scope.Action = "Update";

            for (var keyName in $scope.line.MEETING_LINES) {
                var key = keyName;
                var value = $scope.line.MEETING_LINES[keyName];
                if (!isNaN(value) && angular.isNumber(value))
                    $scope.line.MEETING_LINES[keyName] = value.toString();
            }

            if ($scope.line.MEETING_LINES.MTL_OFFER_OK != null)
                $scope.line.MEETING_LINES.MTL_OFFER_OK = $scope.line.MEETING_LINES.MTL_OFFER_OK.toString();

            if ($scope.line.MEETING_LINES.MTL_START_DATE != null)
                $scope.line.MEETING_LINES.MTL_START_DATE = formatDate(new Date(parseInt($scope.line.MEETING_LINES.MTL_START_DATE.substr(6))));
            if ($scope.line.MEETING_LINES.MTL_FINISH_DATE != null)
                $scope.line.MEETING_LINES.MTL_FINISH_DATE = formatDate(new Date(parseInt($scope.line.MEETING_LINES.MTL_FINISH_DATE.substr(6))));

            $scope.fileEdit = true;
            GetAllLineFiles(line.MEETING_LINES.ID);

            angular.element("#lineId").val(line.MEETING_LINES.ID);

            angular.element("#MeetingLinesDiv").slideDown();
            angular.element('textarea').removeAttr('style');
        },
        function () {
            alert('Error in getting records');
        });
    }

    $scope.AddLineDiv = function () {
        $scope.fileEdit = false;
        $scope.lineFiles = '';
        $scope.resultLine = '';
        $scope.line = "";
        $scope.line = { "MEETING_LINES": { "MTL_MT_REF": angular.element("#MEETING_LINES_MTL_MT_REF").val() } };
        $scope.Action = "Add";
        $scope.fileCreate = true;
        angular.element("#MeetingLinesDiv").slideDown();
        angular.element('textarea').removeAttr('style');
    }

    $scope.AddUpdateLine = function () {
        var getAction = $scope.Action;
        if (getAction == "Update") {
            var getData = linesService.updateLine($scope.line);
            getData.then(function (msg) {
                GetLineAll();
                angular.element("#MeetingLinesDiv").slideUp();
            }, function () {
                alert('Error in updating record');
            });
        }
        else {
            var getData = linesService.AddLine($scope.line);
            getData.then(function (msg) {
                GetLineAll();
                angular.element("#MeetingLinesDiv").slideUp();
            }, function () {
                alert('Error in adding record');
            });
        }
        debugger;
        $scope.refresh();
    }


    $scope.deleteLine = function (line) {
        var getData = linesService.DeleteLine(line);
        getData.then(function (msg) {
            GetLineAll();
        }, function () {
            alert('Error in Deleting Record');
        });
    }

});