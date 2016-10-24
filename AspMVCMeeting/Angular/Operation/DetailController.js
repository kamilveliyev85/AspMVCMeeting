operationApp.controller("DetailCntrl", function ($scope, $http, upload, operationService) {

    GetDetailAll();

    function GetDetailAll() {
        var getData = operationService.GetDetailAll(angular.element("#MEETING_LINES_ID").val());
        getData.then(function (emp) {
            $scope.details = emp.data;
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.editDetail = function (detail) {
        $scope.fileCreate = false;
        $scope.detailFiles = '';
        $scope.resultDetail = '';

        var getData = operationService.getDetail(detail.MEETING_LINES_DETAIL.ID);
        getData.then(function (emp) {
            $scope.detail = emp.data;

            $scope.Action = "Update";

            $scope.fileEdit = true;
            GetAllDetailFiles(detail.MEETING_LINES_DETAIL.ID);

            angular.element("#detailId").val(detail.MEETING_LINES_DETAIL.ID);
            angular.element('textarea').removeAttr('style');
            angular.element("#divAddOperation").slideDown();
        },
        function () {
            alert('Error in getting records');
        });
    }

    $scope.AddDetailDiv = function () {
        $scope.fileEdit = false;
        $scope.fileCreate = true;
        //$scope.lineFiles = '';
        //$scope.resultLine = '';
        $scope.detail = "";
        $scope.detail = { "MEETING_LINES_DETAIL": { "MLD_MTL_REF": angular.element("#MEETING_LINES_ID").val() } };
        $scope.Action = "Add";
        angular.element("#divAddOperation").slideDown();
        angular.element('textarea').removeAttr('style');

    }

    $scope.AddUpdateDetail = function () {
        var getAction = $scope.Action;
        if (getAction == "Update") {
            var getData = operationService.updateDetail($scope.detail);
            getData.then(function (msg) {
                GetDetailAll();
                angular.element("#divAddOperation").slideUp();
            }, function () {
                alert('Error in updating record');
            });
        }
        else {
            var getData = operationService.AddDetail($scope.detail);
            getData.then(function (msg) {
                GetDetailAll();
                angular.element("#divAddOperation").slideUp();
            }, function () {
                alert('Error in adding record');
            });
        }
        debugger;
        $scope.refresh();
    }

    $scope.deleteDetail = function (detail) {
        var getData = operationService.DeleteDetail(detail);
        getData.then(function (msg) {
            GetDetailAll();
        }, function () {
            alert('Error in Deleting Record');
        });
    }

    //BEGIN DETAIL files create

    $scope.UploadDetailFileCreate = function () {
        upload({
            url: '/api/Operation/UploadDetailFileCreate',
            method: 'POST',
            data: {
                aFile: $scope.myFile
            }
        }).then(
          function (response) {
              $scope.resultDetail = response.data;
              console.log(response.data);
          },
          function (response) {
              console.error(response);
          }
        );
    }

    $scope.removeDetailFile = function (fileName) {
        upload({
            url: '/api/Operation/RemoveDetailFile',
            method: 'POST',
            data: {
                fileName: fileName
            }
        }).then(
          function (response) {
              $scope.resultDetail = response.data;
              console.log(response.data);
          },
          function (response) {
              console.error(response);
          }
        );
    }

    //END DETAIL files create

    //BEGIN DETAIL files edit

    function GetAllDetailFiles(detailId) {
        var getData = operationService.GetAllDetailFiles(detailId);
        getData.then(function (emp) {
            $scope.detailFiles = emp.data;
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.removeDetailFileByID = function (fileId) {
        var getData = operationService.removeDetailFileByID(fileId);
        getData.then(function (emp) {
            console.log(emp.data);
            GetAllDetailFiles(angular.element("#detailId").val());
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.UploadDetailFileEdit = function () {
        upload({
            url: '/api/Operation/UploadDetailFileEdit',
            method: 'POST',
            data: {
                aFile: $scope.myFile,
                detailId: angular.element("#detailId").val()
            }
        }).then(
          function (response) {
              GetAllDetailFiles(angular.element("#detailId").val());
              console.log(response.data);
          },
          function (response) {
              console.error(response);
          }
        );
    }

    //END DETAIL files edit


});