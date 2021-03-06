'use strict';
commentApp.controller('commentCtrl', ['$scope', '$rootScope', 'commentSvc', 'UserService', function ($scope, $rootScope, commentSvc, userService) {
    $scope.id = $scope.value ? $scope.value.entityId : "";

    $scope.newComment = {
        message: '',
    };

    var postId = 'cnf4h5HT4dc';

    var commentLimit = {
        Firstlevel: 10,
        SecondLevel: 2,
        ThirdLevel: 0
    }

    $rootScope.$on('authorized', function () {
        $scope.currentUser = userService.getCurrentUser();
    });
    $rootScope.$on('unauthorized', function () {
        $scope.currentUser = userService.setCurrentUser(null);
    });

    $scope.baseCommentThread = {};

    var actionType = {
        Init: 1,
        Add: 2,
        Reply: 3,
        Edit : 4,
        Delete: 5,
        LoadMore: 6,
        LoadAll: 7
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
        comment.isEdit = true;
    };

    $scope.showLoadMore = function (item) {
        return (item.replies && item.replies.length < item.totalReplies);
    };

    $scope.showLess = function (item) {
        item.limit = 2;
    };

    $scope.showMore = function (item) {
        item.currentPage = item.currentPage + 1;
        loadComments(item, actionType.LoadMore, item.comment.id);
    };

    $scope.showAll = function (item) {
        item.currentPage = -1;
        item.limit = 1000;
        loadComments(item, actionType.LoadAll, item.comment.id);
    };

    $scope.addComment = function (newComment) {
        commentSvc.addReplyComment(newComment.message, "", postId)
                .then(function (addedComment) {
                    newComment.message = '';
                    updateDOM($scope.baseCommentThread, addedComment, actionType.Add);
                    addNewActivity(addedComment.comment, actionType.Reply);
                },
                    function (errorMessage) {
                        console.warn(errorMessage);
                    });
    };

    $scope.replyComment = function (replyComment, parentComment) {
        commentSvc.addReplyComment(replyComment.message, parentComment.comment.id, postId)
               .then(function (repliedComment) {
                   replyComment.message = '';
                   updateDOM(parentComment, repliedComment, actionType.Reply);
                   addNewActivity(repliedComment.comment, actionType.Reply);
               },
                   function (errorMessage) {
                       console.warn(errorMessage);
                   });
    };

    $scope.editComment = function (comment) {
        comment.postId = postId;
        commentSvc.editComment(comment)
               .then(function () {
                   comment.isEdit = false;
                   comment.isEdited = true;
                   addNewActivity(comment,actionType.Edit);
               },
                   function (errorMessage) {
                       console.warn(errorMessage);
                   });
    };

    var images = {};

    images["486e6f82-5863-6783-9f71-23ce80f83cde"] = "/Content/Images/Legolas.gif";
    images["486e6f82-1234-46fa-9f71-23ce80f83cde"] = "/Content/Images/gandalf.png";
    images["b196971f-9873-4ae2-9454-90f6ae2412c4"] = "/Content/Images/Aragorn.jpg";
    images["486e6f82-5678-46fa-9f71-23ce80f83cde"] = "/Content/Images/Frodo.png";
    images["486e6f82-5863-6537-9f71-23ce80f83cde"] = "/Content/Images/Merry.jpg";
    images["486e6f82-5863-9821-9f71-23ce80f83cde"] = "/Content/Images/Pippin.png";

    images["486e6f82-5863-3581-9f71-23ce80f83cde"] = "/Content/Images/Gimli.jpg";
    images["486e6f82-5863-3245-9f71-23ce80f83cde"] = "/Content/Images/Sam.png";
    images["486e6f82-5863-9471-9f71-23ce80f83cde"] = "/Content/Images/Boromir.jpg";

    $scope.getImage = function (userId) {
        if (images[userId])
            return images[userId];
        else "http://placehold.it/64x64";
    };

    $scope.deleteComment = function (item) {
        commentSvc.deleteComment(item.comment.id)
               .then(function () {
                   updateDOM(item, item.comment.id, actionType.Delete);
               },
                   function (errorMessage) {
                       console.warn(errorMessage);
                   });
    };

    var loadComments = function (item, action, parentId) {
        commentSvc.loadComments(postId, item, parentId)
                .then(function (result) {
                    if (result)
                        updateDOM(item, result, action);
                },
                    function (errorMessage) {
                        console.warn(errorMessage);
                    });
    };

    var getRecentActivities = function () {
        commentSvc.getRecentActivities()
                .then(function (result) {
                    $scope.recentActivities = result;
                },
                    function (errorMessage) {
                        console.warn(errorMessage);
                    });
    };

    var addNewActivity = function (comment,action) {

        var newcomment = {
            message: comment.message,
            changeDoneBy: comment.updatedByName == null ? comment.createdByName : comment.updatedByName,
            changeDoneOn: new Date(),
            action: action == actionType.Add ? "Added" : "Updated"
        };
        $scope.recentActivities = pushItemToArray($scope.recentActivities, newcomment);
    };

    $scope.init = function () {
        var thread = getCommentThread();
        loadComments(thread, actionType.Init, "-1");
        $scope.baseCommentThread = thread;
        getRecentActivities();
    };
    $scope.init();
}]);