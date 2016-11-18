operationApp.controller("operationCntrl", function ($scope, $filter, $http, $q, NgTableParams, upload, operationService) {

    GetLineAll();

    function filterBy(obj) {
        if ($scope.employees.toString().indexOf(obj.MEETING_LINES.MTL_RESPONSIBLE) !== -1) {
            return true;
        } else {
            return false;
        }
    }

    function GetLineAll() {
        var getData = operationService.GetLinesAll(null);

        getData.then(function (emp) {
            $scope.fullData = emp.data;
            $scope.data = emp.data;
            angular.element(".spn_Task_Count").text($scope.data.length);
            $scope.employees = null;
            $scope.$watch('employees', function () {
                if ($scope.employees != null)
                    $scope.data = $scope.fullData.filter(filterBy);
                else
                    $scope.data = $scope.fullData;

                $scope.tableParams = new NgTableParams({
                    page: 1, // show first page
                    total: 1, // value less than count hide pagination
                    count: 12 // count per page
                }, { counts: [], dataset: $scope.data });

            });

        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.isAfter = function (jsonDate) {
        if (jsonDate !== null)
            return new Date(parseInt(jsonDate.substr(6))).setHours(0, 0, 0, 0) < new Date().setHours(0, 0, 0, 0);
        else
            return null;
    }

    $scope.viewLine = function (line) {
        $scope.fileCreate = false;
        $scope.lineFiles = '';
        $scope.resultLine = '';
        var getData = operationService.getLine(line.MEETING_LINES.ID);
        getData.then(function (emp) {
            $scope.line = emp.data;

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

        },
            function () {
                alert('Error in getting records');
            });
    }

    $scope.filterEmployee = function (employees) {

        $scope.employees = employees;
    }

    $scope.changeStatusLine = function (line) {
        angular.element('#statusUpdate').modal('show');
        $scope.line = angular.copy(line);
        $scope.line.MEETING_LINES.MTL_STS = $scope.line.MEETING_LINES.MTL_STS.toString();
    }

    $scope.updateStatus = function (line) {
        var getData = operationService.updateStatus($scope.line);
        getData.then(function (msg) {
            GetLineAll();
            angular.element('#statusUpdate').modal('hide');
        }, function () {
            alert('Error in updating record');
        });
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

});