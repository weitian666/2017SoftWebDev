/*
 *  根据指定的地址直接链接跳转回退
 * @param {} urlString 
 * @returns {} 
 */
function lionGotoNewPage(urlString) {
    window.location.href = urlString;
}

/*
 *  根据指定的地址 urlString 访问控制器方法，然后根据返回的局部页刷新指定的 targetDiv 区域
 * @param {} urlString 
 * @param {} targetDivElelmentID 
 * @returns {} 
 */
function lionGotoNewPartial(urlString, targetDivElelmentID) {
    lionGotoNewPartialByJsonAndShowStatus(urlString, "", targetDivElelmentID, "", true);
}


/*
 * 根据指定的地址 urlString 访问控制器方法，
 * 执行访问时，在指定的位置呈现状态信息，
 * 然后根据返回的局部页刷新指定的 targetDiv 区域
 * @param {} urlString 
 * @param {} targetDivElelmentID 
 * @param {} statucMessage 
 * @returns {} 
 */
function lionGotoNewPartialAndShowStatus(urlString, targetDivElelmentID, statucMessage) {
    lionGotoNewPartialByJsonAndShowStatus(urlString, "", targetDivElelmentID, statucMessage, true);
}

/*
 * 根据指定的地址 urlString 和 jsonData 访问控制器方法，
 * 
 * @param {} urlString 
 * @param {} jsonData 
 * @param {} targetDivElelmentID 
 * @returns {} 
 */
function lionGotoNewPartialByJson(urlString, jsonData, targetDivElelmentID) {
    lionGotoNewPartialByJsonAndShowStatus(urlString, jsonData, targetDivElelmentID, "", true);
}

/*
 * 根据指定的地址 urlString 和 jsonData 访问控制器方法,
 * 执行访问时，在指定的位置呈现状态信息，
 * 然后根据返回的局部页刷新指定的 targetDivElelmentID 区域
 * 
 * @param {} urlString 
 * @param {} jsonData 
 * @param {} targetDivElelmentID 
 * @param {} statusMessage
 * * @param {} isAsync 
 * @returns {} 
 */
function lionGotoNewPartialByJsonAndShowStatus(urlString, jsonData, targetDivElelmentID, statusMessage, isAsync) {
    $.ajax({
        cache: false,
        type: "POST",
        async: isAsync,
        url: urlString,
        data: jsonData,
        beforeSend: function () {
            if (statusMessage !== "") {
                $("#" + targetDivElelmentID).html(statusMessage);
            }
        }
    }).done(function (data) {
        var reg = /^<script>.*<\/script>$/;
        if (reg.test(data)) {
            // 这句是为了响应后台返回的js
            $('body').append("<span id='responseJs'>" + data + "</span>").remove("#responseJs");
            return;
        } else {
            if (targetDivElelmentID !== '') {
                $("#" + targetDivElelmentID).html(data);
            }
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.error("调试错误:" + errorThrown);
    }).always(function () {
    });
}

/*
 * 创建新的 ListParaJson 对象
 * @param {} typeID 
 * @returns {} 
 */
function lionCreateListParaJson(typeID) {
    lionIntializationListPageParameter(typeID);
    var listParaJson = lionGetListParaJson();
    return listParaJson;
}

/*
 *  重新初始化页面规格参数
 * @param {} typeID 
 * @returns {} 
 */
function lionIntializationListPageParameter(typeID) {
    $("#lionTypeID").val(typeID);
    $("#lionPageIndex").val("1");
    $("#lionPageSize").val("18");
    $("#lionPageAmount").val("0");
    $("#lionObjectAmount").val("0");
    $("#lionKeyword").val("");
    $("#lionSortProperty").val("SortCode");
    $("#lionSortDesc").val("default");
    $("#lionSelectedObjectID").val("");
    $("#lionIsSearch").val("False");
}

/*
 * 提取页面分页规格数据,构建 ListParaJson 对象
 * @returns {} 
 */
function lionGetListParaJson() {
    // 提取缺省的页面规格参数
    var lionPageTypeID = $("#lionTypeID").val();
    var lionPagePageIndex = $("#lionPageIndex").val();
    var lionPagePageSize = $("#lionPageSize").val();
    var lionPagePageAmount = $("#lionPageAmount").val();
    var lionPageObjectAmount = $("#lionObjectAmount").val();
    var lionPageKeyword = $("#lionKeyword").val();
    var lionPageSortProperty = $("#lionSortProperty").val();
    var lionPageSortDesc = $("#lionSortDesc").val();
    var lionPageSelectedObjectID = $("#lionSelectedObjectID").val();
    var lionPageIsSearch = $("#lionIsSearch").val();
    // 创建前端 json 数据对象
    var listParaJson = "{" +
        "ObjectTypeID:\"" + lionPageTypeID + "\", " +
        "PageIndex:\"" + lionPagePageIndex + "\", " +
        "PageSize:\"" + lionPagePageSize + "\", " +
        "PageAmount:\"" + lionPagePageAmount + "\", " +
        "ObjectAmount:\"" + lionPageObjectAmount + "\", " +
        "Keyword:\"" + lionPageKeyword + "\", " +
        "SortProperty:\"" + lionPageSortProperty + "\", " +
        "SortDesc:\"" + lionPageSortDesc + "\", " +
        "IsSearch:\"" + lionPageIsSearch + "\", " +
        "SelectedObjectID:\"" + lionPageSelectedObjectID + "\"" +
        "}";

    return listParaJson;
}

