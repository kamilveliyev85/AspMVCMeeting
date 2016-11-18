operationApp.controller("NotificationCntrl", function ($scope, $document, $http, upload, operationService) {
    
    GetNotificationAll();
        
    function GetNotificationAll() {
        var getData = operationService.GetNotificationAll();
        getData.then(function (emp) {
            $scope.notifications = emp.data;
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.showDetail = function (item) {
        $scope.getLineById(item.MEETING_NOTIFICATIONS.NTF_REFID);
        $scope.GetDetailAllByDetailId(item.MEETING_NOTIFICATIONS.ID, item.MEETING_NOTIFICATIONS.NTF_REFID);
        item.MEETING_NOTIFICATIONS.NTF_SEEN = true;
        item.MEETING_NOTIFICATIONS.NTF_READ = true;
        //angular.element(".spn_Notification_Count").text($scope.data.length);
    }

    $scope.removeNotification = function (item) {
        var getData = operationService.removeNotification(item.MEETING_NOTIFICATIONS.ID);
        getData.then(function (emp) {
            GetNotificationAll();
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.replyNotification = function (item) {
        var getData = operationService.replyNotification(item.MEETING_NOTIFICATIONS.ID);
        getData.then(function (emp) {
            angular.element('#replyModal').modal('show');
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.getLineById = function (NTF_REFID) {
        var getData = operationService.getLineById(NTF_REFID);
        getData.then(function (emp) {
            $scope.line = emp.data;
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.GetDetailAllByDetailId = function (id, NTF_REFID) {
        var getData = operationService.GetDetailAllByDetailId(id, NTF_REFID);
        getData.then(function (emp) {
            $scope.details = emp.data;
            angular.element("#divLineDetail").slideDown("fast", function () { });
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.closeDetail = function () {
        angular.element("#divLineDetail").slideUp("fast", function () { });
        $scope.details = null;
    }

    function GetAllFiles(id, type) {
        var getData = operationService.GetAllFiles(id, type);
        getData.then(function (emp) {
            $scope.files = emp.data;
        }, function (response) {
            alert('Error in getting records');
        });
    }

    $scope.viewFiles = function (id, type) {
        GetAllFiles(id, type);
        angular.element('#fileModal').modal('show');
    }

});