angular.module('sortableApp', ['ui.sortable','ngTagsInput'])
.controller('sortableController', function ($scope) {   
    $scope.pages = [];
    $scope.name = "";
    $scope.createTime;
    $scope.tags = [];
    $scope.tagsText = [];
    $scope.tagList = [];

    $scope.Initialize = function (model) {
        $scope.name = model.name;
        $scope.pages = model.pages;
        $scope.tags = model.tags;
        $($scope.tags).each(function (index, value) {
            $scope.tagsText.push({ text: value.name });
        });
    };

    $scope.loadTags = function ($query) {
        $.ajax({
            type: 'POST',
            url: "/Comix/GetTag",            
            data: { quote: $query},            
            success: function (data) {
                $scope.tagList = [];
                $(data).each(function (index, value) {
                    $scope.tagList.push( value.Name );
                });
            }
        });
        return $scope.tagList;
    };

 
});