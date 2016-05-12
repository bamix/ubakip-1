angular.module('sortableApp', ['ui.sortable','ngTagsInput'])
.controller('sortableController', function ($scope) {   
    $scope.pages = [];
    $scope.name = "";
    $scope.createTime;
    $scope.tags = [];
    $scope.tagsText = [];
    $scope.tagList = [];
    $scope.coverPageId;
    $scope.publish = true;

    $scope.Initialize = function (model) {
        $scope.name = model.name;
        $scope.pages = model.pages;       
        $scope.tags = model.tags;
        $scope.coverPageId = model.coverPageId;
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

    $("#sortable").sortable({
        items: "div:not(.ui-state-disabled)",
        cursor: 'pointer',
        connectWith: ".pages-container"
    }).disableSelection();

    $("#sortable").on("dblclick", ".list-group-item", function () {
        if ($(this).attr("id") == null) return;
        $scope.coverPageId = parseInt($(this).attr("id"));
        $scope.$apply();
    });
});