app.controller("linesCntrl", function ($scope, $http, upload, linesService) {

    //BEGIN MASER HISTORY

    function getMasterHistoryById(masterId) {
        var getData = linesService.getMasterHistoryById(masterId);
        getData.then(function (response) {
            $scope.masterHistory = response.data;
        }, function () {
            alert('Error in getting record');
        });
    }

    $scope.showHistory = function () {
        getMasterHistoryById(angular.element("#MEETING_LINES_MTL_MT_REF").val());

        angular.element('#large').modal('show');

    }

    $scope.showHistoryDetail = function (history) {
        $scope.history = history;
                
        if ($scope.history.MEETING_MASTER_V.MT_START_TIME != null && typeof ($scope.history.MEETING_MASTER_V.MT_START_TIME) == 'object')
            $scope.history.MEETING_MASTER_V.MT_START_TIME = $scope.history.MEETING_MASTER_V.MT_START_TIME.Hours.toString() + ":" + $scope.history.MEETING_MASTER_V.MT_START_TIME.Minutes.toString();
        if ($scope.history.MEETING_MASTER_V.MT_FINISH_TIME != null && typeof ($scope.history.MEETING_MASTER_V.MT_FINISH_TIME) == 'object')
            $scope.history.MEETING_MASTER_V.MT_FINISH_TIME = $scope.history.MEETING_MASTER_V.MT_FINISH_TIME.Hours.toString() + ":" + $scope.history.MEETING_MASTER_V.MT_FINISH_TIME.Minutes.toString();;

        if ($scope.history.MEETING_MASTER_V.MT_TYPE != null && typeof($scope.history.MEETING_MASTER_V.MT_TYPE) == 'number')
            $scope.history.MEETING_MASTER_V.MT_TYPE = $scope.history.MEETING_MASTER_V.MT_TYPE.toString();

        if ($scope.history.MEETING_MASTER_V.MT_DATE != null)
            $scope.history.MEETING_MASTER_V.MT_DATE = formatDate(new Date(parseInt($scope.history.MEETING_MASTER_V.MT_DATE.substr(6))));

        angular.element('textarea').removeAttr('style');

        angular.element('#divShowHistory').slideDown('fast', 'swing', function () {
            var sectionOffset = angular.element('#divShowHistory').offset().top - 30;
            angular.element('#large').animate({ scrollTop: sectionOffset }, 'slow');


        });
       
    }

    //END MASER HISTORY
    
    
    //BEGIN Update Master

    getMasterById(angular.element("#MEETING_LINES_MTL_MT_REF").val());

    function getMasterById(masterId) {
        var getData = linesService.getMasterById(masterId);
        getData.then(function (response) {
            $scope.master = response.data;

            if ($scope.master.MEETING_MASTER.MT_START_TIME != null)
                $scope.master.MEETING_MASTER.MT_START_TIME = $scope.master.MEETING_MASTER.MT_START_TIME.Hours.toString() + ":" + $scope.master.MEETING_MASTER.MT_START_TIME.Minutes.toString();
            if ($scope.master.MEETING_MASTER.MT_FINISH_TIME != null)
                $scope.master.MEETING_MASTER.MT_FINISH_TIME = $scope.master.MEETING_MASTER.MT_FINISH_TIME.Hours.toString() + ":" + $scope.master.MEETING_MASTER.MT_FINISH_TIME.Minutes.toString();;

            if ($scope.master.MEETING_MASTER.MT_TYPE != null)
                $scope.master.MEETING_MASTER.MT_TYPE = $scope.master.MEETING_MASTER.MT_TYPE.toString();

            if ($scope.master.MEETING_MASTER.MT_MANAGER != null)
                $scope.master.MEETING_MASTER.MT_MANAGER = $scope.master.MEETING_MASTER.MT_MANAGER.toString();

            if ($scope.master.MEETING_MASTER.MT_FOLLOWER_USERID != null)
                $scope.master.MEETING_MASTER.MT_FOLLOWER_USERID = $scope.master.MEETING_MASTER.MT_FOLLOWER_USERID.toString();

            if ($scope.master.MEETING_MASTER.MT_DATE != null)
                $scope.master.MEETING_MASTER.MT_DATE = formatDate(new Date(parseInt($scope.master.MEETING_MASTER.MT_DATE.substr(6))));
        }, function () {
            alert('Error in getting record');
        });
    }

    $scope.updateMaster = function () {
            var getData = linesService.updateMaster($scope.master);
            getData.then(function (response) {
                //GetMasterById();
                //angular.element("#MeetingMasterBody").slideUp();
                angular.element('#divUpdateAlert').fadeIn(1500, function () { angular.element('#divUpdateAlert').fadeOut(1500) });
            }, function () {
                alert('Error in updating record');
            });
        $scope.refresh();
    }

    //END Update Master

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
            angular.element('textarea').removeAttr('style');
            angular.element("#MeetingLinesDiv").slideDown();
            
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

    $scope.publishLine = function (line) {
        var getData = linesService.publishLine(line);
        getData.then(function (msg) {
            GetLineAll();
        }, function () {
            alert('Error in publishing Record');
        });
    }

});