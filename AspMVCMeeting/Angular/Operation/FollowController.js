operationApp.controller("followCntrl", function ($scope, $filter, $http, $q, NgTableParams, upload, operationService) {

    GetFollowAll();

    function GetFollowAll() {
        var getData = operationService.GetFollowAll();
        getData.then(function (emp) {
            $scope.data = emp.data;
            $scope.tableParams = new NgTableParams({
                page: 1, // show first page
                total: 1, // value less than count hide pagination
                count: 5 // count per page
            }, { counts: [], dataset: $scope.data });
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.viewLine = function (line) {
        $scope.fileCreate = false;
        $scope.lineFiles = '';
        $scope.resultLine = '';

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

        $scope.fileEdit = false;
        $scope.fileCreate = false;
        angular.element('#MeetingLinesDiv').find('input, textarea, select').attr('disabled', 'disabled');
        angular.element('#btnAddLine').hide();

        angular.element("#lineId").val(line.MEETING_LINES.ID);
        angular.element('textarea').removeAttr('style');
        angular.element("#MeetingLinesDiv").slideDown();

        //},
        //function () {
        //    alert('Error in getting records');
        //});
    }

    $scope.CancelAddEdit = function () {
        $scope.line = null;
        angular.element("#MeetingLinesDiv").slideUp();
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

    $scope.GetDetailAll = function (line) {
        var getData = operationService.GetDetailAll(line.MEETING_LINES.ID);
        getData.then(function (emp) {
            $scope.details = emp.data;
            angular.element("#divLineDetail").slideDown();
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.closeDetail = function () {
        $scope.details = null;
        angular.element("#divLineDetail").slideUp();
    }

    $scope.viewFiles = function (id) {
        GetAllFiles(id);
        angular.element('#fileModal').modal('show');
    }

    function GetAllFiles(id) {
        var getData = operationService.GetAllFiles(id);
        getData.then(function (emp) {
            $scope.files = emp.data;
        }, function (response) {
            alert('Error in getting records');
        });
    }
});