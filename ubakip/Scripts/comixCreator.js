angular.module('sortableApp', ['ui.sortable','ngTagsInput'])
.controller('sortableController', function ($scope) {   
    $scope.pages = [];
    $scope.id;
    $scope.name = "";
    $scope.createTime;
    $scope.tags = [];
    $scope.tagsText = [];
    $scope.tagList = [];
    $scope.publish = true;
    $scope.ratingId;

    $scope.Initialize = function (model) {
        $scope.name = model.comix.name;
        $scope.id = model.comix.id;
        $scope.pages = model.comix.pages;
        $scope.tags = model.comix.tags;
        $scope.ratingId = model.comix.mpaaRatingId.toString();;
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

    window.onbeforeunload = function (e) {
  
    };
    
    $("#save").click(function () {
        var comix = {
            Id: $scope.id,
            Pages: $scope.pages,
            Tags:[],
            Name: $scope.name,
            MPAARatingId: $scope.ratingId
        }
        $($scope.tagsText).each(function (index, value) {
            comix.Tags.push({ Id: 0, Count:1, Name: value.text });
        });

        $.ajax({
            type: 'POST',
            url: "/Comix/SaveComix",
            dataType: 'json',
            data: JSON.stringify(comix),
            contentType: "application/json; charset=utf-8",
            traditional: true,
            success: function (data) { }
        });
    });

    $("#sortable").sortable({   
        cursor: 'pointer',
        connectWith: ".pages-container"
    }).disableSelection();

    $("#sortable").delegate(".delete", "click", function () {
        $.ajax({
            type: 'POST',
            url: "/Comix/DeletePage",
            data: { id: $(this).parent().attr("id"), comixId: $scope.id },
            success: function (data) { }
        });
        var id = $(this).parent().attr("id");
        $($scope.pages).each(function (index, value) {            
            if (value.id == id)
            {
                $scope.pages.splice(index, 1);
            }
        });       
        $(this).parent().remove();
    });
});