appIndep.service("IndepService", function ($http) {
       
    //get All Line Files
    this.GetAllLineFiles = function (lineId) {
        return $http.get("/tr/MeetingLineIndependent/GetAllLineFiles/" + lineId);
    };
    
    //remove line files by id
    this.removeLineFileByID = function (fileId) {
        var response = $http({
            method: "post",
            url: "/tr/MeetingLineIndependent/removeLineFileByID",
            params: {
                fileId: JSON.stringify(fileId),
            }
        });
        return response;
    };

    //get All Lines
    this.GetLinesAll = function (MT_Ref) {
        return $http.get("/tr/MeetingLineIndependent/GetLinesAll/" + MT_Ref);
    };

    // get Line By Id
    this.getLine = function (lineId) {
        var response = $http({
            method: "post",
            url: "/tr/MeetingLineIndependent/getLineById",
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
            url: "/tr/MeetingLineIndependent/UpdateLine",
            data: JSON.stringify(line),
            dataType: "json"
        });
        return response;
    }

    // Add Line
    this.AddLine = function (line) {

        var response = $http({
            method: "post",
            url: "/tr/MeetingLineIndependent/AddLine",
            data: JSON.stringify(line),
            dataType: "json"
        });

        return response;
    }

    //Delete Line
    this.DeleteLine = function (line) {
        var response = $http({
            method: "post",
            url: "/tr/MeetingLineIndependent/DeleteLine",
            data: JSON.stringify(line),
            dataType: "json"
        });
        return response;
    }
});