operationApp.service("operationService", function ($http) {
     
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

});