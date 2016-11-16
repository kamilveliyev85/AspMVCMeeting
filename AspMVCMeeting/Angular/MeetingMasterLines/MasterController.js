app.controller("masterCntrl", function ($scope, $http, upload, linesService) {

    $scope.selected = undefined;
    $scope.scope1 = undefined;
    $scope.scope2 = undefined;
    $scope.scope3 = undefined;
    $scope.scope4 = undefined;

    
    var getData = linesService.GetAllByColumnName("MT_TITLE", "MEETING_MASTER");
    getData.then(function (emp) {
        $scope.titles = emp.data;
    });
    
    var getData = linesService.GetAllByColumnName("MT_SCODE1", "MEETING_MASTER");
    getData.then(function (emp) {
        $scope.scopes1 = emp.data;
    });

    var getData = linesService.GetAllByColumnName("MT_SCODE2", "MEETING_MASTER");
    getData.then(function (emp) {
        $scope.scopes2 = emp.data;
    });

    var getData = linesService.GetAllByColumnName("MT_SCODE3", "MEETING_MASTER");
    getData.then(function (emp) {
        $scope.scopes3 = emp.data;
    });

    var getData = linesService.GetAllByColumnName("MT_SCODE4", "MEETING_MASTER");
    getData.then(function (emp) {
        $scope.scopes4 = emp.data;
    });

    //BEGIN Master meeting files create

    $scope.UploadMasterFileCreate = function () {
        upload({
            url: '/api/MeetingMaster/upload',
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
            url: '/api/MeetingMaster/RemoveFile',
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
});