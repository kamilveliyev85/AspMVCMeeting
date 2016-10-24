app.controller("masterCntrl", function ($scope, $http, upload, linesService) {
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