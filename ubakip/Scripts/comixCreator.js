angular.module('sortableApp', ['ui.sortable','ngTagsInput'])
.controller('sortableController', function ($scope) {   

    $scope.id; 
    $scope.tags = [];
    $scope.tagsText = [];
    $scope.tagList = [];
    $scope.dateCreate;


    $scope.Initialize = function (model) {
        $scope.tags = model;
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
                $scope.tagList=[];
                $(data).each(function (index, value) {
                    if ($scope.tagList.indexOf(value) == -1)
                    $scope.tagList.push( value );
                });
            }
        });
        return $scope.tagList;
    };


    
    $("#save").click(function () {
        var comix = {
            Id: $("#page").attr("data-id"),
            Pages: [],
            Tags:[],
            Name: $("#name").val(),
            DateCreated: $("#page").attr("data-date"),
            MPAARatingId:$( "#select" ).val()
        }
        $($scope.tagsText).each(function (index, value) {
            comix.Tags.push({ Id: 0, Count:1, Name: value.text });
        });

        $(".page-preview").each(function (index, value) {
            comix.Pages.push({ Id: $(this).attr("id") });
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
            data: { id: $(this).parent().attr("id"), comixId: $("#page").attr("data-id") },
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