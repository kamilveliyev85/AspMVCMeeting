var app = angular.module("app", ['lr.upload']);

app.directive('uploadFile', ['$parse', function ($parse) {
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

app.filter("sanitize", ['$sce', function ($sce) {
    return function (htmlCode) {
        return $sce.trustAsHtml(htmlCode);
    }
}]);

app.filter('ctime', function () {

    return function (jsonDate) {
        if (jsonDate == null)
            return "";
        var date = new Date(parseInt(jsonDate.substr(6)));
        return date;
    };

});

app.directive('jqdatepicker', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {

            element.datepicker({
                dateFormat: 'dd.MM.yyyy',
                onSelect: function (date) {
                    scope.date = date;
                    scope.$apply();
                }
            });

            scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                //element.datepicker({
                //    dateFormat: 'dd.MM.yyyy',
                //    onSelect: function (date) {
                //        scope.date = date;
                //        scope.$apply();
                //    }
                //}).val('10.11.2011');

            });
           
        }
    };
});

app.directive("select2", function ($timeout, $parse) {
    return {
        restrict: 'C',
        link: function (scope, element, attrs) {
            $timeout(function () {
                element.select2({ width: '100%' });
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