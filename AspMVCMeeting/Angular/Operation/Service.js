operationApp.service("operationService", function ($http) {
    //get All Titles by Column Name
    this.GetColumnValues = function (columnName, tableName) {
        var response = $http({
            method: "post",
            url: "/api/Operation/GetColumnValues",
            params: {
                columnName: JSON.stringify(columnName),
                tableName: JSON.stringify(tableName)
            }
        });
        return response;
    };

    //get All files
    this.GetAllFiles = function (id, type) {
        var response = $http({
            method: "post",
            url: "/api/MeetingMaster/GetAllFiles",
            params: {
                id: id,
                type: type
            }
        });
        return response;
    };

    //BEGIN change and Save

    //get All Line Files
    this.GetAllLineFiles = function (lineId) {
        return $http.get("/api/MeetingMaster/GetAllLineFiles/" + lineId);
    };

    //remove line files by id
    this.removeLineFileByID = function (fileId) {
        var response = $http({
            method: "post",
            url: "/api/MeetingMaster/removeLineFileByID",
            params: {
                fileId: JSON.stringify(fileId),
            }
        });
        return response;
    };

    //Delete Line
    this.DeleteLine = function (line) {
        var response = $http({
            method: "post",
            url: "/api/MeetingMaster/DeleteLine",
            data: JSON.stringify(line),
            dataType: "json"
        });
        return response;
    }

    // get Line By Id
    this.getLine = function (lineId) {
        var response = $http({
            method: "post",
            url: "/api/MeetingMaster/getLineById",
            params: {
                id: JSON.stringify(lineId)
            }
        });
        return response;
    }

    // Update Line 
    this.updateLine = function (line) {
        var response = $http({
            method: "post",
            url: "/api/MeetingMaster/UpdateLine",
            data: JSON.stringify(line),
            dataType: "json"
        });
        return response;
    }

    //END change and Save
     
    //get All Lines
    this.GetLinesAll = function (MT_Ref) {
        return $http.get("/api/Operation/GetLinesAll/" + MT_Ref);
    };

    //get All Approve
    this.GetApproveAll = function (MT_Ref) {
        return $http.get("/api/Operation/GetApproveAll/" + MT_Ref);
    };

    //get All Detail Files
    this.GetAllDetailFiles = function (detailId) {
        return $http.get("/api/Operation/GetAllDetailFiles/" + detailId);
    };

    //remove detail files by id
    this.removeDetailFileByID = function (fileId) {
        var response = $http({
            method: "post",
            url: "/api/Operation/removeDetailFileByID",
            params: {
                fileId: JSON.stringify(fileId),
            }
        });
        return response;
    };


    //get All Detail
    this.GetDetailAll = function (MLD_MTL_REF) {
        return $http.get("/api/Operation/GetDetailAll/" + MLD_MTL_REF);
    };

    // get Detail By Id
    this.getDetail = function (detailId) {
        var response = $http({
            method: "post",
            url: "/api/Operation/getDetailById",
            params: {
                id: JSON.stringify(detailId)
            }
        });
        return response;
    }

    // Update Detail 
    this.updateDetail = function (detail) {
        var response = $http({
            method: "post",
            url: "/api/Operation/UpdateDetail",
            data: JSON.stringify(detail),
            dataType: "json"
        });
        return response;
    }
    
    // Add Detail
    this.AddDetail = function (detail) {

        var response = $http({
            method: "post",
            url: "/api/Operation/AddDetail",
            data: JSON.stringify(detail),
            dataType: "json"
        });

        return response;
    }

    //Delete Detail
    this.DeleteDetail = function (detail) {
        var response = $http({
            method: "post",
            url: "/api/Operation/DeleteDetail",
            data: JSON.stringify(detail),
            dataType: "json"
        });
        return response;
    }
    
    // Update Status 
    this.updateStatus = function (line) {
        var response = $http({
            method: "post",
            url: "/api/Operation/UpdateStatus",
            data: JSON.stringify(line),
            dataType: "json"
        });
        return response;
    }

    // Update Selected Status 
    this.UpdateSelectedStatus = function (items, status) {
        var response = $http({
            method: "post",
            url: "/api/Operation/UpdateSelectedStatus",
            params: {
                items: JSON.stringify(items),
                status: status
            }
        });
        return response;
    }

    // Update Status Approve
    this.updateStatusApprove = function (line) {
        var response = $http({
            method: "post",
            url: "/api/Operation/UpdateStatusApprove",
            data: JSON.stringify(line),
            dataType: "json"
        });
        return response;
    }

    // Update Selected Status Approve
    this.UpdateSelectedStatusApprove = function (items, status, desc) {
        var response = $http({
            method: "post",
            url: "/api/Operation/UpdateSelectedStatusApprove",
            params: {
                items: JSON.stringify(items),
                status: status,
                desc: desc
            }
        });
        return response;
    }


    //BEGIN OFFER

    //get All Offer
    this.GetOfferAll = function () {
        return $http.get("/api/Operation/GetOfferAll");
    };

    //END OFFER

    //BEGIN OFFER

    //get All Follow
    this.GetFollowAll = function () {
        return $http.get("/api/Operation/GetFollowAll");
    };

    //END OFFER

    //BEGIN NOTIFICATION

     //get All Notifications
    this.GetNotificationAll = function () {
        return $http.get("/api/Operation/GetNotificationAll");
    };

    //get All Detail
    this.GetDetailAllByDetailId = function (id, NTF_REFID) {
        var response = $http({
            method: "post",
            url: "/api/Operation/GetDetailAllByDetailId",
            params: {
                id: id,
                NTF_REFID: NTF_REFID
            }
        });
        return response;
    };

    //get Line by Id
    this.getLineById = function (NTF_REFID) {
        var response = $http({
            method: "post",
            url: "/api/Operation/getLineById",
            params: {
                NTF_REFID: NTF_REFID
            }
        });
        return response;
    };

    //remove Notification
    this.removeNotification = function (id) {
        var response = $http({
            method: "post",
            url: "/api/Operation/removeNotification",
            params: {
                id: id
            }
        });
        return response;
    };

    //reply Notification
    this.replyNotification = function (id) {
        var response = $http({
            method: "post",
            url: "/api/Operation/replyNotification",
            params: {
                id: id
            }
        });
        return response;
    };

    //END NOTIFICATION
});