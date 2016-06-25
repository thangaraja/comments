'use strict';
commentApp.service("commentSvc", function ($http, $q) {
    var urlBase = '/api/Comment/';

    return ({
        addReplyComment: addReplyComment,
        loadComments: loadComments,
        editComment: editComment,
        deleteComment: deleteComment
    });

    function loadComments(_item, _parentId) {
        if (!_parentId)
            _parentId = -1;
        var deferred = $q.defer();
        var request = $http({
            method: "get",
            url: urlBase + "Get",
            params: {
                parentId: _parentId,
                page: _item.currentPage,
                pageSize: _item.pageSize
            }
        }).success(function (data) {
            deferred.resolve(data);
        });

        return deferred.promise;
    };

    function addReplyComment(message, parentId) {
        var deferred = $q.defer();
        var request = $http({
            method: "post",
            url: urlBase + "AddOrReplyComment",
            data: {
                comment: {
                    message: message,
                    parentId: parentId
                }
            }
        }).success(function (result) {
            deferred.resolve(result);
        });

        return deferred.promise;
    };

    function editComment(_comment) {
        var deferred = $q.defer();
        var request = $http({
            method: "post",
            url: urlBase + "Edit",
            data: _comment,
        }).success(function (result) {
            deferred.resolve(result);
        });

        return deferred.promise;
    };

    function deleteComment(_comment) {
        var deferred = $q.defer();
        var request = $http({
            method: "post",
            url: urlBase + "DeleteComment",
            data: {
                commentId: _comment.id,
                parentId: _comment.inReplyToCommentId
            }
        }).success(function (result) {
            deferred.resolve(result);
        });

        return deferred.promise;
    };
});