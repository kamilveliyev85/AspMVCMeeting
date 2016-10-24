operationApp.controller("approveCntrl", function ($scope, $http, $filter, $q, NgTableParams, upload, operationService) {
    
    GetApproveAll();

    function GetApproveAll() {
        var getData = operationService.GetApproveAll(null);
        getData.then(function (emp) {
            $scope.data = emp.data;
            $scope.tableParams = new NgTableParams({
                page: 1, // show first page
                total: 1, // value less than count hide pagination
                count: 10 // count per page
            }, { counts: [], dataset: $scope.data });



            $scope.checkboxes = { 'checked': false, items: {} };
            angular.forEach($scope.data, function (item) {
                $scope.checkboxes.items[item.MEETING_LINES.ID] = false;
            });

            // watch for check all checkbox
            $scope.$watch('checkboxes.checked', function (value) {
                angular.forEach($scope.data, function (item) {
                    if (angular.isDefined(item.MEETING_LINES.ID)) {
                        $scope.checkboxes.items[item.MEETING_LINES.ID] = value;
                    }
                });
            });

            // watch for data checkboxes
            $scope.$watch('checkboxes.items', function (values) {
                if (!$scope.data) {
                    return;
                }
                var checked = 0, unchecked = 0,
                    total = $scope.data.length;
                angular.forEach($scope.data, function (item) {
                    checked += ($scope.checkboxes.items[item.MEETING_LINES.ID]) || 0;
                    unchecked += (!$scope.checkboxes.items[item.MEETING_LINES.ID]) || 0;
                });
                if ((unchecked == 0) || (checked == 0)) {
                    $scope.checkboxes.checked = (checked == total);
                }
                // grayed checkbox
                angular.element($("#select_all")).prop("indeterminate", (checked != 0 && unchecked != 0));
            }, true);

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

    $scope.changeStatusLine = function (line) {
        $scope.isSelectedApprove = false;
        angular.element('#statusUpdate').modal('show');
        $scope.line = angular.copy(line);
        $scope.line.MEETING_LINES.MTL_STS = $scope.line.MEETING_LINES.MTL_STS.toString();
    }

    $scope.updateStatus = function (line) {
            var getData = operationService.updateStatus($scope.line);
            getData.then(function (msg) {
                GetApproveAll();
                angular.element('#statusUpdate').modal('hide');
            }, function () {
                alert('Error in updating record');
            });
    }

    $scope.approveSelected = function () {
        $scope.isSelectedApprove = true;
        angular.element('#selectedStatus').modal('show');
    }

    $scope.UpdateSelectedStatus = function (items, status) {
        var getData = operationService.UpdateSelectedStatus(items, status);
        getData.then(function (msg) {
            GetApproveAll();
            angular.element('#selectedStatus').modal('hide');
        }, function () {
            alert('Error in approving selected');
        });
    }

});