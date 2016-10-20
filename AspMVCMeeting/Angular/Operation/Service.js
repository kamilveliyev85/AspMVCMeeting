operationApp.service("operationService", function ($http) {
     
    //get All Files
    this.GetLinesAll = function (MT_Ref) {
        return $http.get("/tr/Operation/GetLinesAll/" + MT_Ref);
    };

    //get All Detail Files
    this.GetAllDetailFiles = function (detailId) {
        return $http.get("/tr/Operation/GetAllDetailFiles/" + detailId);
    };

    //remove detail files by id
    this.removeDetailFileByID = function (fileId) {
        var response = $http({
            method: "post",
            url: "/tr/Operation/removeDetailFileByID",
            params: {
                fileId: JSON.stringify(fileId),
            }
        });
        return response;
    };


    //get All Detail
    this.GetDetailAll = function (MLD_MTL_REF) {
        return $http.get("/tr/Operation/GetDetailAll/" + MLD_MTL_REF);
    };

    // get Detail By Id
    this.getDetail = function (detailId) {
        var response = $http({
            method: "post",
            url: "/tr/Operation/getDetailById",
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
            url: "/tr/Operation/UpdateDetail",
            data: JSON.stringify(detail),
            dataType: "json"
        });
        return response;
    }
    
    // Add Detail
    this.AddDetail = function (detail) {

        var response = $http({
            method: "post",
            url: "/tr/Operation/AddDetail",
            data: JSON.stringify(detail),
            dataType: "json"
        });

        return response;
    }

    //Delete Detail
    this.DeleteDetail = function (detail) {
        var response = $http({
            method: "post",
            url: "/tr/Operation/DeleteDetail",
            data: JSON.stringify(detail),
            dataType: "json"
        });
        return response;
    }
    
});