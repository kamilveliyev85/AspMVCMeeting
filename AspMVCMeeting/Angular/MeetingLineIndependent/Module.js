var appIndep = angular.module("appIndep", ['ngTable', 'lr.upload']);

appIndep.directive('uploadFile', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {

            var file_uploaded = $parse(attrs.uploadFile);

            element.bind('change', function () {
                scope.$apply(function () {
                    file_uploaded.assign(scope, element[0].files[0]);
                });

                switch (element.attr("id")) {
                    case 'masterFileCreate':
                        scope.UploadMasterFileCreate();
                        break;
                    case 'masterFileEdit':
                        scope.UploadMasterFileEdit();
                        break;
                    case 'lineFileCreate':
                        scope.UploadLineFileCreate();
                        break;
                    case 'lineFileEdit':
                        scope.UploadLineFileEdit();
                        break;
                }
            });
        }
    };
}]);

appIndep.filter("sanitize", ['$sce', function ($sce) {
    return function (htmlCode) {
        return $sce.trustAsHtml(htmlCode);
    }
}]);

appIndep.filter('ctime', function () {

    return function (jsonDate) {
        if (jsonDate == null)
            return "";
        var date = new Date(parseInt(jsonDate.substr(6)));
        return date;
    };

});

appIndep.directive('jqdatepicker', function () {
    return {
        restrict: "EAC",
        require: "ngModel",
        scope: {
            "ngModel": "="
        },
        link: function (scope, elem, attr) {
            $(elem).datepicker({
                format: "dd.mm.yyyy",
                autoclose: true
            }).on("changeDate", function (e) {
                return scope.$apply(function () {
                    return scope.ngModel = e.format();
                });
            });

            return scope.$watch("ngModel", function (newValue) {
                $(elem).datepicker("update", newValue);
            });
        }
    };
});

appIndep.directive("select2", function ($timeout, $parse) {
    return {
        restrict: 'C',
        link: function (scope, element, attrs) {
            $timeout(function () {
                element.select2({ width: '100%', allowClear: true, placeholder: "Seçin edin"});
                element.select2Initialized = true;
            });

            var refreshSelect = function () {
                if (!element.select2Initialized) return;
                $timeout(function () {
                    element.trigger('change.select2');
                });
            };

            scope.$watch(attrs.ngModel, refreshSelect);
        }
    };
});