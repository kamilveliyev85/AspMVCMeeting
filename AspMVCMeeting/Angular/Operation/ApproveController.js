operationApp.controller("approveCntrl", function ($scope, $http, $filter, $q, NgTableParams, upload, operationService) {

    $scope.names = [{ "id": "Karar", "title": "Karar" }]

    var getData = operationService.GetColumnValues("MLN_NAME", "MEETING_LINE_TYPE");
    getData.then(function (emp) {
        $scope.names = emp.data;

    });


    //BEGIN Master meeting LINES edit

    function GetAllLineFiles(lineId) {
        var getData = operationService.GetAllLineFiles(lineId);
        getData.then(function (emp) {
            $scope.lineFiles = emp.data;
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

    $scope.removeLineFileByID = function (fileId) {
        var getData = operationService.removeLineFileByID(fileId);
        getData.then(function (emp) {
            console.log(emp.data);
            GetAllLineFiles(angular.element("#lineId").val());
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.UploadLineFileEdit = function () {
        upload({
            url: '/api/MeetingMaster/UploadLineFileEdit',
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

    GetApproveAll();

    function GetApproveAll() {
        var getData = operationService.GetApproveAll(null);
        getData.then(function (emp) {
            $scope.data = emp.data;
            angular.element(".spn_Approve_Count").text($scope.data.length);
            $scope.tableParams = new NgTableParams({
                page: 1, // show first page
                total: 1, // value less than count hide pagination
                count: 10 // count per page
            }, { counts: [], dataset: $scope.data });

            $scope.checkboxes = { 'checked': false, items: {} };
            angular.forEach($scope.data, function (item) {
                if (item.MEETING_LINES.MTL_STS !== 9)
                    $scope.checkboxes.items[item.MEETING_LINES.ID] = false;
            });

            // watch for check all checkbox
            $scope.$watch('checkboxes.checked', function (value) {
                angular.forEach($scope.data, function (item) {
                    if (angular.isDefined(item.MEETING_LINES.ID) && item.MEETING_LINES.MTL_STS !== 9) {
                        $scope.checkboxes.items[item.MEETING_LINES.ID] = value;
                    }
                });
            });

            // watch for data checkboxes
            $scope.$watch('checkboxes.items', function (values) {
                if (!$scope.data) {
                    return;
                }
                var checked = 0, unchecked = 0, total = 0;

                angular.forEach($scope.data, function (item) {
                    if (item.MEETING_LINES.MTL_STS !== 9) total += 1;
                });

                angular.forEach($scope.data, function (item) {
                    if (item.MEETING_LINES.MTL_STS !== 9) {
                        checked += ($scope.checkboxes.items[item.MEETING_LINES.ID]) || 0;
                        unchecked += (!$scope.checkboxes.items[item.MEETING_LINES.ID]) || 0;
                    }
                });

                if ((unchecked == 0) || (checked == 0)) {
                    $scope.checkboxes.checked = (checked == total);
                    if (total == 0) $scope.checkboxes.checked = false;
                }
                // grayed checkbox
                angular.element($("#select_all")).prop("indeterminate", (checked != 0 && unchecked != 0));

            }, true);

        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.CancelAddEdit = function () {
        $scope.line = null;
        angular.element("#MeetingLinesDiv").slideUp();
    }

    $scope.editLine = function (line, isEdit) {
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

            $scope.fileEdit = true;
            GetAllLineFiles(line.MEETING_LINES.ID);

            angular.element("#lineId").val(line.MEETING_LINES.ID);
            angular.element('textarea').removeAttr('style');
            angular.element('#MEETING_LINES_MTL_TAGS').tagsinput('removeAll');
            angular.element('#MEETING_LINES_MTL_TAGS').tagsinput('add', line.MEETING_LINES.MTL_TAGS);

            if (isEdit) {
                angular.element('#MeetingLinesDiv').find('input, textarea, select').removeAttr('disabled');
                angular.element('#btnAddLine').show();
            } else {
                $scope.fileEdit = false;
                $scope.fileCreate = false;
                angular.element('#MeetingLinesDiv').find('input, textarea, select').attr('disabled', 'disabled');
                angular.element('#btnAddLine').hide();
            }

            angular.element("#MeetingLinesDiv").slideDown();

        },
        function () {
            alert('Error in getting records');
        });
    }

    $scope.AddUpdateLine = function () {

        if ($scope.line.MEETING_LINES.MTL_START_DATE != null)
            $scope.line.MEETING_LINES.MTL_START_DATE = convertDate($scope.line.MEETING_LINES.MTL_START_DATE);
        if ($scope.line.MEETING_LINES.MTL_FINISH_DATE != null)
            $scope.line.MEETING_LINES.MTL_FINISH_DATE = convertDate($scope.line.MEETING_LINES.MTL_FINISH_DATE);

        var getData = operationService.updateLine($scope.line);
        getData.then(function (msg) {
            GetApproveAll();
            angular.element("#MeetingLinesDiv").slideUp();
        }, function () {
            alert('Error in updating record');
        });

        debugger;
        $scope.refresh();
    }

    $scope.changeStatusLine = function (line) {
        $scope.isSelectedApprove = false;
        $scope.modal_type = 1;
        angular.element('#statusUpdate').modal('show');
        $scope.line = angular.copy(line);
        $scope.line.MTL_ACTION_DESC = null;
        $scope.line.MTL_ACTION_TYPE = null;
    }

    $scope.updateStatus = function (line) {
        var getData = operationService.updateStatusApprove($scope.line);
        getData.then(function (msg) {
            GetApproveAll();
            angular.element('#statusUpdate').modal('hide');
        }, function () {
            alert('Error in updating status');
        });
    }

    $scope.publishLine = function (line) {
        line.MTL_ACTION_TYPE = 1;
        var getData = operationService.updateStatusApprove(line);
        getData.then(function (msg) {
            GetApproveAll();
            angular.element('#statusUpdate').modal('hide');
        }, function () {
            alert('Error in updating publishing');
        });
    }

    $scope.approveSelected = function () {
        $scope.isSelectedApprove = true;
        $scope.line = [{ "MTL_ACTION_DESC": null, "MTL_ACTION_TYPE": null }];
        $scope.modal_type = 2;
        angular.element('#statusUpdate').modal('show');
    }

    $scope.deleteLine = function (line) {
        var getData = operationService.DeleteLine(line);
        getData.then(function (msg) {
            GetApproveAll();
        }, function () {
            alert('Error in Deleting Record');
        });
    }

    $scope.UpdateSelectedStatus = function (items, status, desc) {
        var getData = operationService.UpdateSelectedStatusApprove(items, status, desc);
        getData.then(function (msg) {
            GetApproveAll();
            angular.element('#statusUpdate').modal('hide');
        }, function () {
            alert('Error in approving selected');
        });
    }

    function convertDate(str) {
        var from = str.split(".");
        return new Date(from[2], from[1] - 1, from[0]);
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