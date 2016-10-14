app.service("linesService", function ($http) {
    
    //get All files
    this.GetAllFiles = function (MT_Ref) {
        return $http.get("/tr/MeetingMaster/GetAllFiles/" + MT_Ref);
    };

    //get All Line Files
    this.GetAllLineFiles = function (lineId) {
        return $http.get("/tr/MeetingMaster/GetAllLineFiles/" + lineId);
    };

    //remove files by id
    this.removeFileByID = function (fileId) {
        var response = $http({
            method: "post",
            url: "/tr/MeetingMaster/removeFileByID",
            params: {
                fileId: JSON.stringify(fileId),
            }
        });
        return response;
    };

    //remove line files by id
    this.removeLineFileByID = function (fileId) {
        var response = $http({
            method: "post",
            url: "/tr/MeetingMaster/removeLineFileByID",
            params: {
                fileId: JSON.stringify(fileId),
            }
        });
        return response;
    };

    //get All Files
    this.GetLinesAll = function (MT_Ref) {
        return $http.get("/tr/MeetingMaster/GetLinesAll/" + MT_Ref);
    };

    // get Line By Id
    this.getLine = function (lineId) {
        var response = $http({
            method: "post",
            url: "/tr/MeetingMaster/getLineById",
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
            url: "/tr/MeetingMaster/UpdateLine",
            data: JSON.stringify(line),
            dataType: "json"
        });
        return response;
    }

    // Add Line
    this.AddLine = function (line) {

        var response = $http({
            method: "post",
            url: "/tr/MeetingMaster/AddLine",
            data: JSON.stringify(line),
            dataType: "json"
        });

        return response;
    }

    //Delete Line
    this.DeleteLine = function (line) {
        var response = $http({
            method: "post",
            url: "/tr/MeetingMaster/DeleteLine",
            data: JSON.stringify(line),
            dataType: "json"
        });
        return response;
    }
});