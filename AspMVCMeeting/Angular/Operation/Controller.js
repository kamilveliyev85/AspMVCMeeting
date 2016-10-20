operationApp.controller("operationCntrl", function ($scope, $http, upload, operationService) {
    
    GetLineAll();

    function GetLineAll() {
        var getData = operationService.GetLinesAll(null);
        getData.then(function (emp) {
            $scope.lines = emp.data;
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.viewLine = function (line) {
        $scope.fileCreate = false;
        $scope.lineFiles = '';
        $scope.resultLine = '';
        //var getData = operationService.getLine(line.MEETING_LINES.ID);
        //getData.then(function (emp) {
            //$scope.line = emp.data;
        //$scope.Action = "Update";

        $scope.line = line;

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
            //GetAllLineFiles(line.MEETING_LINES.ID);

            angular.element("#lineId").val(line.MEETING_LINES.ID);
            angular.element('textarea').removeAttr('style');
            angular.element("#MeetingLinesDiv").slideDown();

        //},
        //function () {
        //    alert('Error in getting records');
        //});
    }

});