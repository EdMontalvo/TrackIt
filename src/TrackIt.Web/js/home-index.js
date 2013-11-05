// home-index.js
var homeIndexModule = angular.module("homeIndex", ['$strap.directives']);

homeIndexModule.config(["$routeProvider", function ($routeProvider) {
    $routeProvider.when("/", {
        controller: "weightsController",
        templateUrl: "/templates/weightsView.html"
    });

    $routeProvider.when("/newweight", {
        controller: "newTopicController",
        templateUrl: "/templates/newTopicView.html"
    });

    $routeProvider.when("/dailyweight/:id", {
        controller: "singleTopicController",
        templateUrl: "/templates/singleTopicView.html"
    });

    $routeProvider.otherwise({ redirectTo: "/" });
}]);

homeIndexModule.factory("dataService", ["$http", "$q", function ($http, $q) {
    var _weights = [];
    var _isInit = false;
    var _isReady = function () {
        return _isInit;
    };

    var _getweights = function () {

        var deferred = $q.defer();

        $http.get("/api/v1/dailyweights")
            .then(function (result) {
                // Successfull
                angular.copy(result.data, _weights);
                _isInit = true;
                deferred.resolve();
            },
            function () {
                // Error
                deferred.reject();
            });
        return deferred.promise;
    };

    var _addWeight = function (newTopic) {
        var deferred = $q.defer();
        $http.post("/api/v1/dailyweights", newTopic)
            .then(function (result) {
                //success
                var newlyCreatedTopic = result.data;
                _weights.splice(0, 0, newlyCreatedTopic);
                deferred.resolve(newlyCreatedTopic);
            },
            function () {
                //Error
                deferred.reject();
            }
        );

        return deferred.promise;

    };

    function _findWeight(id) {
        var found = null;

        $.each(_weights, function (i, item) {
            if (item.id == id) {
                found = item;
                return false;
            }
        });

        return found;
    }

    var _getWeightById = function (id) {
        var deferred = $q.defer();

        if (_isReady()) {
            var topic = _findWeight(id);
            if (topic) {
                deferred.resolve(topic);
            } else {
                deferred.reject();
            }
        } else {
            _getweights()
                .then(function () {
                    //success
                    var topic = _findWeight(id);
                    if (topic) {
                        deferred.resolve(topic);
                    } else {
                        deferred.reject();
                    }
                },
                function () {
                    //error
                    deferred.reject();
                });
        }
        return deferred.promise;
    };

    return {
        weights: _weights,
        getweights: _getweights,
        addWeight: _addWeight,
        isReady: _isReady,
        getWeightById: _getWeightById,

    };
}]);

var weightsController = ["$scope", "$http", "dataService", function ($scope, $http, dataService) {
    $scope.newWeight = {};
    $scope.data = dataService;
    $scope.isBusy = false;
    if (dataService.isReady() == false) {
        $scope.isBusy = true;
        dataService.getweights()
            .then(function () {
                //success
                var today = new Date();
                if (dataService.weights.length > 0) {
                    $scope.newWeight.weight = dataService.weights[0].weight;
                    $scope.newWeight.bodyFat = dataService.weights[0].bodyFat;
                } else {
                    $scope.newWeight.weight = 0;
                    $scope.newWeight.bodyFat = 0;
                }
                $scope.newWeight.weightDate = today;
            },
                function () {
                    //error
                    alert("cound not load weights");
                })
            .then(function () {
                $scope.isBusy = false;
            });
    }
}];

var newTopicController = ["$scope", "$http", "$window", "dataService", function newTopicController($scope, $http, $window, dataService) {
    $scope.newWeight = {};
    $scope.save = function () {
        dataService.addTopic($scope.newTopic)
            .then(function () {
                //success
                $window.location = "#/";
            },
                function () {
                    //Error
                    alert("cannot save the new topic");
                });
    };
}];

var singleTopicController = ["$scope", "dataService", "$window", "$routeParams", function singleTopicController($scope, dataService, $window, $routeParams) {
    $scope.topic = null;
    $scope.newReply = {};

    dataService.getTopicById($routeParams.id)
        .then(function (topic) {
            //success
            $scope.topic = topic;
        },
            function () {
                //error
                $window.location = "#/";
            });

    $scope.addReply = function () {
        dataService.saveReply($scope.topic, $scope.newReply)
            .then(function () {
                // success
                $scope.newReply.body = "";
            },
                function () {
                    //error
                    alert("Could not save the new reply");
                });
    };
}];