'use strict';
commentApp.controller('commentCtrl', ['$scope', 'commentSvc', function ($scope, commentSvc) {

    $scope.id = $scope.value ? $scope.value.entityId : "";

    $scope.newComment = {
        message: '',
    };

    var commentLimit = {
        Firstlevel: 10,
        SecondLevel: 2,
        ThirdLevel: 0
    }

    $scope.baseCommentThread = {};

    var actionType = {
        Init: 1,
        Add: 2,
        Reply: 3,
        Delete: 4,
        LoadMore: 5,
        LoadAll: 6
    };

    // Conver the plain comment to thread
    function getCommentThread(_level, _comment, _replies, _totalReplies) {

        if (!_comment) _comment = {};
        if (!_replies) _replies = [];
        if (!_totalReplies) _totalReplies = 0;
        if (!_level) _level = 0;

        return {
            comment: _comment,
            replies: _replies,
            pageSize: _level === 0 ? commentLimit.Firstlevel : (_level === 1 ? commentLimit.SecondLevel : commentLimit.ThirdLevel),
            currentPage: 1,
            limit: 5,
            totalReplies: _totalReplies
        };

    };

    // Server returns result as plain json. Lets change it inorder to render the result recursivly.
    function appendResult(_level, result) {

        var repliesCount = result ? result.length : 0;
        if (repliesCount > 0) {
            for (var index = 0; index < repliesCount; index++) {
                var reply = result[index];
                if (reply.replies) {
                    result[index] = getCommentThread(_level, reply.comment, reply.replies.replies, reply.replies.totalReplies);
                    _level = _level + 1;
                    appendResult(_level, reply.replies.replies);
                }
                else
                    result[index] = getCommentThread(_level, reply.comment);
            }
        }
        return result;
    }

    var pushItemToArray = function (items, itemTobeAdded) {
        var array = [];
        array = array.concat(itemTobeAdded);
        array = array.concat(items);
        return array;
    }

    // Apply the result in DOM
    function updateDOM(item, result, action) {
        switch (action) {
            case actionType.Init:
                item.totalReplies = result.totalReplies;
                item.replies = item.replies.concat(appendResult(0, result.replies));
                item.limit = item.limit + item.pageSize;
                break;
            case actionType.Add:
                item.replies = pushItemToArray(item.replies, getCommentThread(0, result.comment));
                item.limit = item.limit + 1;
                item.totalReplies = item.totalReplies + 1;
                break;
            case actionType.Reply:
                debugger;
                item.comment.isReply = false;
                var reply = getCommentThread(item.level, result.comment);
                item.replies = pushItemToArray(item.replies, reply);
                break;
            case actionType.Delete:

                break;
            case actionType.LoadMore:
                item.replies = item.replies.concat(appendResult(0, result.replies));
                item.totalReplies = result.totalReplies;
                item.limit = item.limit + item.pageSize;
                break;
            case actionType.LoadAll:
                item.replies = appendResult(0, result.replies);
                item.limit = item.limit + item.pageSize;
                item.limit = item.totalReplies;
                break;
        }
    };

    $scope.shouldDisable = function (message) {
        return (!message || message.length === 0);
    };

    $scope.showReply = function (comment) {
        comment.isReply = true;
    };

    $scope.showEdit = function (comment) {
        comment.isEdited = true;
    };

    $scope.showLoadMore = function (item) {
        return (item.replies && item.replies.length < item.totalReplies);
    };

    $scope.showLess = function (item) {
        item.limit = 2;
    };

    $scope.showMore = function (item) {
        item.currentPage = item.currentPage + 1;
        $scope.loadComments(item, actionType.LoadMore, item.comment.id);
    };

    $scope.showAll = function (item) {
        item.currentPage = -1;
        item.limit = 1000;
        $scope.loadComments(item, actionType.LoadAll, item.comment.id);
    };


    $scope.addComment = function (newComment) {
        commentSvc.addReplyComment(newComment.message,"")
                .then(function (addedComment) {
                    newComment.message = '';
                    updateDOM($scope.baseCommentThread, addedComment, actionType.Add);
                },
                    function (errorMessage) {
                        console.warn(errorMessage);
                    });
    };

    $scope.replyComment = function (replyComment, parentComment) {
        commentSvc.addReplyComment(replyComment.message, parentComment.comment.id)
               .then(function (repliedComment) {
                   replyComment.message = '';
                   updateDOM(parentComment, repliedComment, actionType.Reply);
               },
                   function (errorMessage) {
                       console.warn(errorMessage);
                   });
    };

    $scope.editComment = function (comment) {
        commentSvc.editComment(comment)
               .then(function () {
                   comment.isEdited = false;
               },
                   function (errorMessage) {
                       console.warn(errorMessage);
                   });
    };


    $scope.deleteComment = function (item) {
        commentSvc.deleteComment(item.comment)
               .then(function () {
                   updateDOM(item);
               },
                   function (errorMessage) {
                       console.warn(errorMessage);
                   });
    };

    $scope.loadComments = function (item, action, parentId) {
        commentSvc.loadComments(item, parentId)
                .then(function (result) {
                    if (result)
                        updateDOM(item, result, action);
                },
                    function (errorMessage) {
                        console.warn(errorMessage);
                    });
    };

    $scope.init = function () {
        var thread = getCommentThread();
        $scope.loadComments(thread, actionType.Init, "-1");
        $scope.baseCommentThread = thread;
    };
    $scope.init();

}]);