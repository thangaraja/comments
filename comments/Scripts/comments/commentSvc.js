'use strict';
commentApp.service("commentSvc", function ($http, $q) {
    var urlBase = '/Comment/';

    return ({
        addReplyComment: addReplyComment,
        loadComments: loadComments,
        editComment: editComment,
        deleteComment: deleteComment
    });

    function loadComments(postId, _item, _parentId) {
        if (!_parentId)
            _parentId = -1;
        var deferred = $q.defer();
        var request = $http({
            method: "get",
            url: urlBase + "Get",
            params: {
                postId: postId,
                parentId: _parentId,
                page: _item.currentPage,
                pageSize: _item.pageSize
            }
        }).success(function (data) {
            deferred.resolve(data);
        });

        return deferred.promise;
    };

    function addReplyComment(message, parentId, postId) {
        var deferred = $q.defer();
        var request = $http({
            method: "post",
            url: urlBase + "AddOrReplyComment",
            data: {
                comment: {
                    message: message,
                    parentId: parentId,
                    postId: postId
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

    function deleteComment(id) {
        var deferred = $q.defer();
        var request = $http({
            method: "post",
            url: urlBase + "Delete",
            data: {
                commentId: id
            }
        }).success(function (result) {
            deferred.resolve(result);
        });

        return deferred.promise;
    };
});