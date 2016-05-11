angular.module('CommentApp', [])
  .controller('CommentListController', function () {
      var commentList = this;
     
      commentList.comments = [];
          commentList.addComment = function () {
              //TODO: Send to server
           //   {text:commentList.commentText,fromUser:{nickname:"bamix",photo:"https://pp.vk.me/c630516/v630516851/17d41/3DClFMPdBSk.jpg",id:null},toUser:null,time:"2016-05-05T11:06:08.9251865+03:00"}
              commentList.comments.unshift({text:commentList.commentText,fromUser:{nickname:"bamix",photo:"https://pp.vk.me/c630516/v630516851/17d41/3DClFMPdBSk.jpg",id:null},toUser:null,time:"2016-05-05T11:06:08.9251865+03:00"});
          commentList.commentText = '';
       
      };

      commentList.Initialize = function (model) {
          commentList.comments = model;
        //  
          ////TODO: Send to server
          //commentList.comments.unshift({ text: commentList.commentText, nickname: 'bamix', photo: "https://pp.vk.me/c630516/v630516851/17d41/3DClFMPdBSk.jpg", time: "" });
          //commentList.commentText = '';
         
      };

      //todoList.remaining = function () {
      //    var count = 0;
      //    angular.forEach(commentList.comments, function (comment) {
      //        count += todo.done ? 0 : 1;
      //    });
      //    return count;
      //};

      //todoList.archive = function () {
      //    var oldTodos = todoList.todos;
      //    todoList.todos = [];
      //    angular.forEach(oldTodos, function (todo) {
      //        if (!todo.done) todoList.todos.push(todo);
      //    });
      //};
  });