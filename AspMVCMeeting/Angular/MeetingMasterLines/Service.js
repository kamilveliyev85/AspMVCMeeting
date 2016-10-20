﻿app.service("linesService", function ($http) {

    this.getMasterHistoryById = function (masterId) {
        var response = $http({
            method: "POST",
            url: "/tr/MeetingMaster/getMasterHistoryById",
            params: {
                id: masterId
            }
        });
        return response;
    }

    // get Master By Id
    this.getMasterById = function (masterId) {
        var response = $http({
            method: "POST",
            url: "/tr/MeetingMaster/getMasterById",
            params: {
                id: masterId
            }
        });
        return response;
    }

    // Update Master 
    this.updateMaster = function (master) {
        var response = $http({
            method: "post",
            url: "/tr/MeetingMaster/UpdateMaster",
            data: JSON.stringify(master),
            dataType: "json"
        });
        return response;
    }
    
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

    //Publish Line
    this.publishLine = function (line) {
        var response = $http({
            method: "post",
            url: "/tr/MeetingMaster/PublishLine",
            data: JSON.stringify(line),
            dataType: "json"
        });
        return response;
    }

});