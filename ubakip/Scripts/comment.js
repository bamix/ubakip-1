angular.module('CommentApp', ['masonry'])
  .controller('CommentListController', function ($scope) {    
      $scope.comments = [];
      $scope.userId;
      $scope.addComment = function () {
             
              $.ajax({
                  type: 'POST',
                  url: "/UserInfo/SendComment",
                  data: { text: $scope.commentText, toUserId: $scope.userId },
                  success: function (data) {
                      var comment = {
                         id:data.Id, text: data.Text, fromUser: { photo: data.FromUser.Photo, id: data.FromUser.Id, name: data.FromUser.Name },
                          dateCreated: data.DateCreated
                      };
                      $scope.comments.unshift(comment);
                      $scope.$apply();                    
                  }
              });
              $scope.commentText = '';
      };

      $scope.Initialize = function (model, userId) {
          $scope.comments = model;
          $scope.userId = userId;
      };
  });