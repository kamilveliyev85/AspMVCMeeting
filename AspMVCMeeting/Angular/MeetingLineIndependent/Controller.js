﻿appIndep.controller("IndepCntrl", function ($scope, $document, $http, $filter, $q, NgTableParams, upload, IndepService) {

    $scope.isAfter = function (jsonDate) {
        if (jsonDate !== null)
            return new Date(parseInt(jsonDate.substr(6))).setHours(0, 0, 0, 0) < new Date().setHours(0, 0, 0, 0);
        else
            return null;
    }

    var clearDepWatch = null;

    var getData = IndepService.GetAllByColumnName("MTL_SCODE1", "MEETING_LINES");
    getData.then(function (emp) {
        $scope.lineScopes1 = emp.data;
    });

    var getData = IndepService.GetAllByColumnName("MTL_SCODE2", "MEETING_LINES");
    getData.then(function (emp) {
        $scope.lineScopes2 = emp.data;
    });

    var getData = IndepService.GetAllByColumnName("MTL_SCODE3", "MEETING_LINES");
    getData.then(function (emp) {
        $scope.lineScopes3 = emp.data;
    });

    var getData = IndepService.GetAllByColumnName("MTL_SCODE4", "MEETING_LINES");
    getData.then(function (emp) {
        $scope.lineScopes4 = emp.data;
    });

    //BEGIN MASER HISTORY

    function getMasterHistoryById(masterId) {
        var getData = IndepService.getMasterHistoryById(masterId);
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

        if ($scope.history.MEETING_MASTER_V.MT_TYPE != null && typeof ($scope.history.MEETING_MASTER_V.MT_TYPE) == 'number')
            $scope.history.MEETING_MASTER_V.MT_TYPE = $scope.history.MEETING_MASTER_V.MT_TYPE.toString();

        if ($scope.history.MEETING_MASTER_V.MT_PLACE != null && typeof ($scope.history.MEETING_MASTER_V.MT_PLACE) == 'number')
            $scope.history.MEETING_MASTER_V.MT_PLACE = $scope.history.MEETING_MASTER_V.MT_PLACE.toString();

        if ($scope.history.MEETING_MASTER_V.MT_DATE != null)
            $scope.history.MEETING_MASTER_V.MT_DATE = formatDate(new Date(parseInt($scope.history.MEETING_MASTER_V.MT_DATE.substr(6))));

        angular.element('textarea').removeAttr('style');

        angular.element('#MEETING_MASTER_V_MT_TAGS').tagsinput('removeAll');
        angular.element('#MEETING_MASTER_V_MT_TAGS').tagsinput('add', history.MEETING_MASTER_V.MT_TAGS);


        angular.element('#divShowHistory').slideDown('fast', 'swing', function () {
            var sectionOffset = angular.element('#divShowHistory').offset().top - 30;
            angular.element('#large').animate({ scrollTop: sectionOffset }, 'slow');


        });

    }

    //END MASER HISTORY


    //BEGIN Update Master

    getMasterById(angular.element("#MEETING_LINES_MTL_MT_REF").val());

    function getMasterById(masterId) {
        var getData = IndepService.getMasterById(masterId);
        getData.then(function (response) {
            $scope.master = response.data;

            if ($scope.master.MEETING_MASTER.MT_START_TIME != null)
                $scope.master.MEETING_MASTER.MT_START_TIME = $scope.master.MEETING_MASTER.MT_START_TIME.Hours.toString() + ":" + $scope.master.MEETING_MASTER.MT_START_TIME.Minutes.toString();
            if ($scope.master.MEETING_MASTER.MT_FINISH_TIME != null)
                $scope.master.MEETING_MASTER.MT_FINISH_TIME = $scope.master.MEETING_MASTER.MT_FINISH_TIME.Hours.toString() + ":" + $scope.master.MEETING_MASTER.MT_FINISH_TIME.Minutes.toString();;

            if ($scope.master.MEETING_MASTER.MT_TYPE != null)
                $scope.master.MEETING_MASTER.MT_TYPE = $scope.master.MEETING_MASTER.MT_TYPE.toString();

            if ($scope.master.MEETING_MASTER.MT_PLACE != null)
                $scope.master.MEETING_MASTER.MT_PLACE = $scope.master.MEETING_MASTER.MT_PLACE.toString();

            if ($scope.master.MEETING_MASTER.MT_MANAGER != null)
                $scope.master.MEETING_MASTER.MT_MANAGER = $scope.master.MEETING_MASTER.MT_MANAGER.toString();

            if ($scope.master.MEETING_MASTER.MT_FOLLOWER_USERID != null)
                $scope.master.MEETING_MASTER.MT_FOLLOWER_USERID = $scope.master.MEETING_MASTER.MT_FOLLOWER_USERID.toString();

            if ($scope.master.MEETING_MASTER.MT_DATE != null)
                $scope.master.MEETING_MASTER.MT_DATE = formatDate(new Date(parseInt($scope.master.MEETING_MASTER.MT_DATE.substr(6))));

            angular.element('#MEETING_MASTER_MT_TAGS').tagsinput('removeAll');
            angular.element('#MEETING_MASTER_MT_TAGS').tagsinput('add', $scope.master.MEETING_MASTER.MT_TAGS);


        }, function () {
            alert('Error in getting record');
        });
    }

    $scope.updateMaster = function () {
        if ($scope.master.MEETING_MASTER.MT_DATE != null)
            $scope.master.MEETING_MASTER.MT_DATE = convertDate($scope.master.MEETING_MASTER.MT_DATE);
        var getData = IndepService.updateMaster($scope.master);
        getData.then(function (response) {
            //GetMasterById();
            angular.element("#MeetingMasterBody").slideUp();
            angular.element('#divUpdateAlert').fadeIn(1500, function () { angular.element('#divUpdateAlert').fadeOut(1500) });
        }, function () {
            alert('Error in updating record');
        });
        $scope.refresh();
    }

    //END Update Master

    //BEGIN Master meeting files edit
    GetAllFiles();

    function GetAllFiles() {
        var getData = IndepService.GetAllFiles(angular.element("#MEETING_LINES_MTL_MT_REF").val(), 1);
        getData.then(function (emp) {
            $scope.files = emp.data;
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.removeFileByID = function (fileId) {
        var getData = IndepService.removeFileByID(fileId);
        getData.then(function (emp) {
            console.log(emp.data);
            GetAllFiles();
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.UploadMasterFileEdit = function () {
        upload({
            url: '/api/MeetingMaster/UploadFile',
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
            url: '/api/MeetingMaster/UploadLineFileCreate',
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
            url: '/api/MeetingMaster/RemoveLineFile',
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
        var getData = IndepService.GetAllLineFiles(lineId);
        getData.then(function (emp) {
            $scope.lineFiles = emp.data;
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.removeLineFileByID = function (fileId) {
        var getData = IndepService.removeLineFileByID(fileId);
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

    GetLineAll();

    function GetLineAll(notClose) {
        var getData = IndepService.GetLinesAll(angular.element("#MEETING_LINES_MTL_MT_REF").val());
        getData.then(function (emp) {
            $scope.data = emp.data;

            $scope.tableParams = new NgTableParams({
                page: 1, // show first page
                total: 1, // value less than count hide pagination
                count: 12 // count per page
            }, { counts: [], dataset: $scope.data });


            $scope.checkboxes = { 'checked': false, items: {} };
            angular.forEach($scope.data, function (item) {
                if (item.MEETING_LINES.MTL_STS === 4)
                    $scope.checkboxes.items[item.MEETING_LINES.ID] = false;
            });

            // watch for check all checkbox
            $scope.$watch('checkboxes.checked', function (value) {
                angular.forEach($scope.data, function (item) {
                    if (angular.isDefined(item.MEETING_LINES.ID) && item.MEETING_LINES.MTL_STS === 4) {
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
                    if (item.MEETING_LINES.MTL_STS === 4) total += 1;
                });

                angular.forEach($scope.data, function (item) {
                    if (item.MEETING_LINES.MTL_STS === 4) {
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

            if (!angular.isDefined(notClose))
                $scope.CancelAddEdit();

            if (emp.data.length === 0)
                $scope.AddLineDiv();

        }, function (response) {
            alert('Error in getting records');
        });

    }

    $scope.publishSelected = function (items) {
        var getData = IndepService.publishSelected(items);
        getData.then(function (emp) {
            GetLineAll();
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

    function convertDate(str) {
        var from = str.split(".");
        return new Date(from[2], from[1] - 1, from[0]);
    }

    $scope.editLine = function (line, isEdit) {
        if (clearDepWatch != null) clearDepWatch();
        $scope.fileCreate = false;
        $scope.lineFiles = '';
        $scope.resultLine = '';
        $scope.$watch('line.MEETING_LINES.MTL_RESPONSIBLE', function () { });
        var getData = IndepService.getLine(line.MEETING_LINES.ID);
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

            clearDepWatch = $scope.$watch('line.MEETING_LINES.MTL_RESPONSIBLE', function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    //alert(newValue + " " + oldValue);
                    findDepByAccount(newValue);
                }
            });

            angular.element("#AddCopyLine").css("display", "none");
            $scope.editClass = {};
            $scope.editClass[line.MEETING_LINES.ID] = 'active';
            angular.element("#MeetingLinesDiv").slideDown();

        },
        function () {
            alert('Error in getting records');
        });
    }

    $scope.AddLineDiv = function () {
        if (clearDepWatch !== null) clearDepWatch();
        angular.element('#MeetingLinesDiv').find('input, textarea, select').removeAttr('disabled');
        angular.element('#btnAddLine').show();
        $scope.fileEdit = false;
        $scope.lineFiles = '';
        $scope.resultLine = '';
        $scope.line = "";
        $scope.line = {
            "MEETING_LINES": {
                "MTL_MT_REF": angular.element("#MEETING_LINES_MTL_MT_REF").val(), "MTL_START_DATE": new Date(),
                "MTL_DECISION_TYPE": angular.element("#MEETING_MASTER_MT_TYPE").val()
            }
        };
        $scope.Action = "Add";
        $scope.fileCreate = true;
        angular.element('textarea').removeAttr('style');
        angular.element('#MEETING_LINES_MTL_TAGS').tagsinput('removeAll');
        //$scope.line.MEETING_LINES.MTL_START_DATE = convertDate($scope.line.MEETING_LINES.MTL_START_DATE);

        clearDepWatch = $scope.$watch('line.MEETING_LINES.MTL_RESPONSIBLE', function (newValue) {
            //alert(newValue);
            findDepByAccount(newValue);
        });

        angular.element("#AddCopyLine").css("display", "");
        angular.element("#MeetingLinesDiv").slideDown();

    }

    $scope.CancelAddEdit = function () {
        $scope.editClass = {};
        $scope.Action = null;
        angular.element("#AddCopyLine").css("display", "none");
        angular.element("#MeetingLinesDiv").slideUp('fast', function () {
            $scope.line = null;
        });
    }

    $scope.AddUpdateLine = function () {
        if ($scope.line.MEETING_LINES.MTL_START_DATE != null)
            $scope.line.MEETING_LINES.MTL_START_DATE = convertDate($scope.line.MEETING_LINES.MTL_START_DATE);
        if ($scope.line.MEETING_LINES.MTL_FINISH_DATE != null)
            $scope.line.MEETING_LINES.MTL_FINISH_DATE = convertDate($scope.line.MEETING_LINES.MTL_FINISH_DATE);

        if ($scope.Action === "Update") {
            var getData = IndepService.updateLine($scope.line);
            getData.then(function (msg) {
                GetLineAll();
            }, function () {
                alert('Error in updating record');
            });
        }
        else if ($scope.Action === "Add") {
            var getData = IndepService.AddLine($scope.line);
            getData.then(function (msg) {
                GetLineAll();
                getMasterById(angular.element("#MEETING_LINES_MTL_MT_REF").val());
            }, function () {
                alert('Error in adding record');
            });
        }
    }

    $scope.deleteLine = function (line) {
        if (confirm('Silmek istediyinizden eminmisiniz?')) {
            var getData = IndepService.DeleteLine(line);
            getData.then(function (msg) {
                GetLineAll();
            }, function () {
                alert('Error in Deleting Record');
            });
        };
    }

    $scope.copyLine = function (line) {
        var getData = IndepService.CopyLine(line);
        getData.then(function (msg) {
            GetLineAll(true);
            $scope.editLine(msg.data, true);
        }, function () {
            alert('Error in Coping Record');
        });
    }

    $scope.AddCopyLine = function () {
        var copiedLine = null;
        if ($scope.line.MEETING_LINES.MTL_START_DATE != null)
            $scope.line.MEETING_LINES.MTL_START_DATE = convertDate($scope.line.MEETING_LINES.MTL_START_DATE);
        if ($scope.line.MEETING_LINES.MTL_FINISH_DATE != null)
            $scope.line.MEETING_LINES.MTL_FINISH_DATE = convertDate($scope.line.MEETING_LINES.MTL_FINISH_DATE);

        if ($scope.Action === "Add") {
            var getData = IndepService.AddLine($scope.line);
            getData.then(function (msg) {
                GetLineAll(true);
                $scope.editClass = {};

            }, function () {
                alert('Error in adding record');
            });
        }
    }

    $scope.publishLine = function (line) {
        var getData = IndepService.publishLine(line);
        getData.then(function (msg) {
            GetLineAll();
        }, function () {
            alert('Error in publishing Record');
        });
    }

    function findDepByAccount(accountName) {
        var getData = IndepService.findDepByAccount(accountName);
        getData.then(function (emp) {
            $scope.line.MEETING_LINES.MTL_DEPARTMENT = emp.data;
        },
        function () {
            //alert('Error in getting records');
        });

    }

    $document.bind("keydown", function (event) {
        if (event.ctrlKey || event.metaKey) {
            switch (String.fromCharCode(event.which).toLowerCase()) {
                case 's':
                    event.preventDefault();
                    if ($scope.Action != null) {
                        $scope.AddUpdateLine();
                        $scope.$apply();
                    }
                    break;
                case 'y':
                    event.preventDefault();
                    $scope.AddLineDiv();
                    $scope.$apply();
                    break;
            }
        }
    });

});