angular.module('sortableApp', ['ui.sortable'])
.controller('sortableController', function () {
    var pageList = this;
   
    pageList.pages = [];

    pageList.Initialize = function (model) {    
        pageList.pages = model;
    };
});