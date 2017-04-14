function showValidationErrors($scope, errorResponse, defaultError) {

    $scope.validationErrors = [];

    if (errorResponse.data && angular.isObject(errorResponse.data)) {
        if (errorResponse.data.ModelState && angular.isObject(errorResponse.data.ModelState)) {

            var modelState = errorResponse.data.ModelState;

            for (var invalidField in modelState) {
                for (var i = 0; i < modelState[invalidField].length; i++) {
                    var error = modelState[invalidField][i];
                    $scope.validationErrors.push(error);
                }
            }
        }
    }

    if ($scope.validationErrors.length == 0) {
        $scope.validationErrors.push(defaultError);
    }
};

function showLoginError($scope, errorResponse, defaultError) {
    $scope.validationErrors = [];

    if (errorResponse.data && angular.isObject(errorResponse.data)) {
        if (errorResponse.data.error) {
            $scope.validationErrors.push(errorResponse.data.error);
        }
    }

    if ($scope.validationErrors.length == 0) {
        $scope.validationErrors.push(defaultError);
    }
}