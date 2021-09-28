

var holdconfirmObj = null;

/*----- basic scripts :: start -----*/
// Common controls
$(function () {
    $("[date='true']").datepicker().attr("maxlength", "10");
    //$("[datetime='true']").datetimepicker({
    //    timeFormat: "hh:mm tt"
    //});
});



// submit form with data
$.submitForm = function (url, data) {
    var frm = $("<form></form>").attr({ 'action': url, 'method': 'POST' });

    if (typeof (data) != "undefined") {
        // add hidden fields for form data
        for (var item in data) {
            if (typeof (data[item]) != "object") {
                frm.append(getElement(item, data[item]));
            }
            else if (data[item].constructor == Array)
                frm = appendArrayElements(item, frm, data[item]);
            else
                frm = appendElements(item, frm, data[item]);
        }
    }

    // frm = addFilingSession(frm);
    // submit from
    if (processDialog)
        processDialog.dialog("open");

    frm.appendTo(document.body).submit();
};
// Json Object
function appendElements(name, frm, data) {
    for (var item in data) {
        var subName = name + '.' + item;
        if (typeof (data[item]) != "object") {
            frm.append(getElement(subName, data[item]));
        }
        else if (data[item].constructor == Array)
            frm = appendArrayElements(subName, frm, data[item]);
        else
            frm = appendElements(subName, frm, data[item]);
    }
    return frm;
}

// Array Object
function appendArrayElements(name, frm, data) {
    for (var index in data) {
        var arrayName = name + "[" + index + "]";
        if (typeof (data[index]) != "object") {
            frm.append(getElement(arrayName, data[index]));
        }
        else if (data[index].constructor == Array)
            frm = appendArrayElements(arrayName, frm, data[index]);
        else
            frm = appendElements(arrayName, frm, data[index]);
    }
    return frm;
}

function getElement(name, data) {
    return $("<input type='hidden' />").attr({ 'name': name, 'value': typeof (data) == "function" ? data() : data });
}

$.datepicker.setDefaults({
    buttonImage: "../../Themes/Inhouse/nh/images/icon_calender.gif",
    buttonImageOnly: true,
    showOn: 'both',
    buttonText: 'Select Date',
    changeMonth: true,
    showAnim: 'slideDown',
    changeYear: true,
    maxDate: typeof (logindate) == 'undefined' ? '+0d' : logindate,
    defaultDate: typeof (logindate) == 'undefined' ? null : logindate
});

function getElement(name, data) {
    data = data == null ? "" : data;
    return $("<input type='hidden' />").attr({ 'name': name, 'value': typeof (data) == "function" ? data() : data });
}

function getKeyCode(e) {
    if (window.event)
        return window.event.keyCode;
    else if (e)
        return e.which;
    else
        return null;

}

// default button
function setDefaultButton(e, btnName, alternateBtn) {
    if (getKeyCode(e) == 13) {
        if (document.getElementById(btnName) != null) {
            document.getElementById(btnName).click();
        } else {
            document.getElementById(alternateBtn).click();
        }
        return false;
    }
    else {
        return true;
    }
}

// Web Arts integration
function onEnterGoPage(evt, gridObject) {

    var element = evt.target || evt.srcElement || window.event.srcElement;
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
    { return false; }
    if (charCode == 13) {
        if (element.value == '0') {
            alert('Enter valid page number.'); element.value = ''; element.focus(); return false;
        }
        if (element.value == '') {
            alert('Enter page number.');
            element.focus();
            return false;
        }
        if (parseInt(element.value) > parseInt($(element).closest("div").find("[name='hdnTotalPgCount']").val())) {
            alert('Enter valid page number.'); element.value = ''; element.focus(); return false;
        }
        else {

            var pageId = element.value;
            gridObject = gridObject.replace("{1}", pageId);
            eval(gridObject);
            //var ele = evt.targate || evt.srcElement;
            //ele.focus();
            return false;
        }
    }
}

function validatePages(obj) {

    if (obj.value == '0') {
        alert('Enter valid page number.'); obj.value = ''; obj.focus(); return false;
    }
    if (parseInt(obj.value) > parseInt(document.getElementById('hdnTotalPgCount').value)) {
        alert('Enter valid page number.'); obj.value = ''; obj.focus(); return false;
    }
}

function GotoPageNo(obj, gridObject) {
    var pageNum = $(obj).closest("td").find('#txtCommonPageNo').val();
    if (pageNum == '') {
        alert('Enter page number.'); document.getElementById('txtCommonPageNo').focus(); return false;
    }
    if (pageNum == '0') {
        alert('Enter valid page number.'); document.getElementById('txtCommonPageNo').value = ''; document.getElementById('txtCommonPageNo').focus(); return false;
    }
    if (parseInt(pageNum) > parseInt($(obj).closest("div").find("[name='hdnTotalPgCount']").val())) {
        alert('Enter valid page number.'); document.getElementById('txtCommonPageNo').value = ''; document.getElementById('txtCommonPageNo').focus(); return false;
    }
    else {
        var pageId = pageNum;
        gridObject = gridObject.replace("{1}", pageId);
        eval(gridObject);
        return false;
    }
}

var sessionCheck = function (xhr) {
    return true;
}

function isValidDates(fromDate, toDate, fromTime, toTime) {
    var fromDateTime = new Date(fromDateTime + ' ' + fromTime);
    var toDateTime = new Date(toDateTime + ' ' + toTime);
    if (fromDateTime == 'Invalid Date') {
        alert("Invalid from Date Time.");
        return false;
    }

    if (toDateTime == 'Invalid Date') {
        alert("Invalid to Date Time.");
        return false;
    }
    if (fromDateTime > toDateTime)
        return false;
    else
        return true;
}
/*----- basic scripts :: end -----*/

/*----- eZval :: start -----*/

function formSubmit(formId) {
    if ($v.validate(formId)) {
        $v.obj(formId).submit();
    }
}
//$v.messageCallback = function (obj) {
//    return "- " + obj.errMsg;
//}

/*----- eZval :: end -----*/

/*----- Accordion :: start -----*/
/*
 * Accordion 1.3 - jQuery menu widget
 *
 * Copyright (c) 2006 Jörn Zaefferer, Frank Marcia
 */

jQuery.fn.extend({
    // nextUntil is necessary, would be nice to have this in jQuery core
    nextUntil: function (expr) {
        var match = [];

        // We need to figure out which elements to push onto the array
        this.each(function () {
            // Traverse through the sibling nodes
            for (var i = this.nextSibling; i; i = i.nextSibling) {
                // Make sure that we're only dealing with elements
                if (i.nodeType != 1) continue;

                // If we find a match then we need to stop
                if (jQuery.filter(expr, [i]).r == 'undefined') {
                    if (jQuery.filter(expr, [i]).r.length) break;
                }

                // Otherwise, add it on to the stack
                match.push(i);
            }
        });

        return this.pushStack(match);
    },
    // the plugin method itself
    Accordion: function (settings) {
        // setup configuration
        settings = jQuery.extend({}, jQuery.Accordion.defaults, {
            // define context defaults
            header: jQuery(':first-child', this)[0] // take first child's tagName as header
        }, settings);

        // calculate active if not specified, using the first header
        var container = this,
			active = settings.active
				? jQuery(settings.active, this)
				: settings.active === false
					? jQuery("<div>")
					: jQuery(settings.header, this).eq(0),

       running = 0;

        container.find(settings.header)
			.not(active || "")
			.nextUntil(settings.header)
			.hide();
        active.addClass(settings.selectedClass);

        function clickHandler(event) {
            // get the click target
            var clicked;
            if (window.event) {
                clicked = $(window.event.srcElement);
            } else {
                clicked = $(event.target);
            }
            var clickedid = clicked.attr("id");
            if (clickedid != undefined) {
                clickedid = clickedid.substr(clickedid.indexOf("_") + 1, clickedid.length - 1);
            }
            //to allow navigations on no child items exist.
            if (clicked.parent().hasClass("nocontentexist")) {
                return true;
            }
            // due to the event delegation model, we have to check if one
            // of the parent elements is our actual header, and find that

            if (clicked.parents(settings.header).length) {
                while (!clicked.is(settings.header)) {
                    clicked = clicked.parent();
                }
            }

            var clickedActive = clicked[0] == active[0];

            // if animations are still active, or the active header is the target, ignore click
            if (running || (settings.alwaysOpen && clickedActive) || !clicked.is(settings.header)) {
                return;
            }

            // switch classes
            active.toggleClass(settings.selectedClass);
            if (!clickedActive) {
                clicked.addClass(settings.selectedClass);
            }

            // find elements to show and hide
            var toShow = clicked.nextUntil(settings.header),
				toHide = active.nextUntil(settings.header),
				data = [clicked, active, toShow, toHide];
            active = clickedActive ? jQuery([]) : clicked;
            // count elements to animate
            running = toHide.size() + toShow.size();
            var finished = function (cancel) {
                running = cancel ? 0 : --running;
                if (running)
                    return;

                // trigger custom change event
                container.trigger("change", data);
            };
            // TODO if hideSpeed is set to zero, animations are crappy
            // workaround: use hide instead
            // solution: animate should check for speed of 0 and do something about it
            if (settings.animated) {
                if (!settings.alwaysOpen && clickedActive) {
                    toShow.slideToggle(settings.showSpeed);
                    finished(true);
                } else {
                    toHide.filter(":hidden").each(finished).end().filter(":visible").slideUp(settings.hideSpeed, finished);
                    toShow.slideDown(settings.showSpeed, finished);
                }
            } else {
                if (!settings.alwaysOpen && clickedActive) {
                    toShow.toggle();
                } else {
                    toHide.hide();
                    toShow.show();
                }
                finished(true);
            }
            return false;
        };
        function activateHandlder(event, index) {
            // call clickHandler with custom event
            clickHandler({
                target: jQuery(settings.header, this)[index]
            });
        };

        return container
			.bind(settings.event, clickHandler)
			.bind("activate", activateHandlder);
    },
    // programmatic triggering
    activate: function (index) {
        return this.trigger('activate', [index || 0]);
    }
});

jQuery.Accordion = {};
jQuery.extend(jQuery.Accordion, {
    defaults: {
        selectedClass: "selected",
        showSpeed: 'slow',
        hideSpeed: 'fast',
        alwaysOpen: true,
        animated: true,
        event: "click"
    },
    setDefaults: function (settings) {
        jQuery.extend(jQuery.Accordion.defaults, settings);
    }
});

/*----- Accordion :: end -----*/


/* Ajax Loader */
var processDialog;
$(document).ready(function () {
    loadDialog();
    // Ajax loader for form submit
    $('form').submit(function () {
        var dflag = $(this).attr('dialog') != "true" ? true : false;
        if (dflag)
            processDialog.dialog("open");
    });

    // Align Center 
    //$(window).scroll(function () {
    //    $('.ui-dialog-content').dialog("option", "position", "center");
    //});

});

function YNconfirm() {
    if (window.confirm('Are you sure you want to proceed the payment')) {
        confirm("I agree to pay $102 of filing fee.")
        window.location.href = ('dgc_done.html');
    }
    else {
        window.location.href = ('dgc_payment.html');
    }
};

function errorDialog(msg, dmodal) {
    msg = msg.replace(/\n/g, "<br>");
    dmodal = typeof (dmodal) == 'undefined' ? true : false;
    if (msg != null || msg != undefined) {
        var errors = msg.split("<br>");
    }
    else
        var errors = msg;
    msg = '<ul class="errors" style="text-align:left;padding:0px 0px 0px 10px;">';
    for (var index in errors) {
        if (errors[index] != "" && index != "filter")
            msg += "<li class='error_message' style='padding:0px 0px 0px 22px'>" + errors[index].replace(/\n/g, "<br>") + "</li>";
    }
    msg += "</ul>";

    var errd = $("<div id='errorDialog' ></div>").dialog({
        autoOpen: false,
        title: "Alert",
        modal: dmodal,        
        resizable: false,
        width: 'auto',
        height: 'auto',
        minWidth: 300,
        position: 'center',
        open: function () {
            $(this).parent().find(".ui-dialog-titlebar-close").hide();
            $(this).parent().find(".ui-dialog, .ui-dialog-buttonpane, .ui-dialog-buttonset").css({
                "float": "none",
                "text-align": "center",
                width: 'auto'
            });
            $(this).parent().find(".ui-dialog, .ui-dialog-buttonpane").css("text-align", "center");
            $('.ui-dialog :button').focus();

        },
        buttons: {
            "OK": function (event, ui) {
                //$(".content_section").focus();
                $(this).dialog('destroy').remove();
            }
        }
    });

    errd.html(msg).dialog("open");
}

function loadDialog() {
    processDialog = $("<div id='process'></div>").css({ 'text-align': 'center', 'vertical-align': 'middle', 'padding-top': '5px' }).append($("<img />").attr("src", "../../Themes/app/ajax-loader.gif")).append("<br><span>Please wait...</span>").dialog({
        autoOpen: false,
        modal: true,
        width: 125,
        height: 100,
        closeOnEscape: false,
        resizable: false,
        position: 'center',
        open: function () {

            $(this).parent().find(".ui-dialog-titlebar").hide();
            $(this).parent().css("width", "125px");
        },
        close: function () {
            $(this).parent().find(".ui-dialog-titlebar").show();
        }
    });
}

var timer;
$(document).ajaxStart(function () {
    if (processDialog)
        timer = setTimeout(function () {
            processDialog.dialog("open");
            clearTimeout(timer);
        }, 0);

});

$(document).ajaxStop(function () {
    if (processDialog) {
        processDialog.dialog("close");
        clearTimeout(timer);
    }
});

function setDefaultButton(e, defaultbtn) {
    var keycode = (e.keyCode ? e.keyCode : e.which);
    var ele = e.target || e.srcElement;
    if (keycode == 13 && $(ele).is("input:text")) {
        $('#' + defaultbtn).trigger('click');
        return false;
    }
}

function isDecimal(evt) {
    
    var keyCode = (evt.which) ? evt.which : evt.keyCode;
    var ele = evt.target || evt.srcElement;
    if ((keyCode >= 48 && keyCode <= 57) || keyCode == 8 || keyCode == 46) {
        if (keyCode == 46 && ele.value.length == 0) return false;

        var dotindex = ele.value.indexOf('.');
        if (dotindex != -1) {
            if (keyCode == 46) return false;
            else {
                var textlength = ele.value.length - dotindex;
                if (textlength > 2) return false;
            }
        }
    }
    else {
        return false;
    }
}

function disableRule(tblObj, selecters) {
    $(selecters).each(function () {
        var attrVal = $(this).attr("rule");
        if (attrVal) {
            $(this).attr("oldrule", $(this).attr("rule")).removeAttr("rule");
        }
    });
}

function enableRule(tblObj, selecters) {
    $(selecters).each(function () {
        var attrVal = $(this).attr("oldrule");
        if (attrVal) {
            $(this).attr("rule", $(this).attr("oldrule")).removeAttr("oldrule");
        }
    });
}

function addLeadingZeros(n, length) {
    var str = (n > 0 ? n : -n) + "";
    var zeros = "";
    for (var i = length - str.length; i > 0; i--)
        zeros += "0";
    zeros += str;
    return n >= 0 ? zeros : "-" + zeros;
}

$.fn.disableRule = function () {

    var ruleAttr = $(this).attr("rule");
    if (ruleAttr) {
        $(this).attr("disabledRule", ruleAttr).removeAttr("rule");
    }

    $(this).find("[rule]").each(function () {
        var attr = $(this).attr("rule");
        $(this).attr("disabledRule", attr).removeAttr("rule");
    });
};

$.fn.enableRule = function () {
    var ruleAttr = $(this).attr("disabledRule");
    if (ruleAttr) {
        $(this).attr("rule", ruleAttr).removeAttr("disabledRule");
    }

    $(this).find("[disabledRule]").each(function () {
        var attr = $(this).attr("disabledRule");
        $(this).attr("rule", attr).removeAttr("disabledRule");
    });
};

jQuery(document).ready(function () {
    var offset = 220;
    var duration = 500;
    jQuery(window).scroll(function () {
        if (jQuery(this).scrollTop() > offset) {
            jQuery('.back-to-top').fadeIn(duration);
        } else {
            jQuery('.back-to-top').fadeOut(duration);
        }
    });

    jQuery('.back-to-top').click(function (event) {
        event.preventDefault();
        jQuery('html, body').animate({ scrollTop: 0 }, duration);
        return false;
    })
});

function AutoComplete(SelectorName, LookUpURL, extraData, selectCallBack) {

    $('#' + SelectorName).autocomplete({
        hint: true,
        highlight: true,
        minLength: 3,
        scroll: true,
        delay: 300,
        select: typeof selectCallBack == "function" ? selectCallBack : function (event, ui) { }
    },
        {
            name: 'states',
            displayKey: 'value',
            source: function (request, response) {

                console.log('auto Complete for ' + $('#' + SelectorName).val())
                var param = { Name: $('#' + SelectorName).val() };
                //Adding extra parameters.
                if (typeof extraData == "object")
                    $.extend(param, extraData);

                $.ajax({
                    url: LookUpURL,
                    type: "POST",
                    data: JSON.stringify(param),
                    dataType: "JSON",
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        //response(data);
                        response($.map(data, function (item) {

                            console.log(' Adding Item ' + item);
                            return {
                                label: item.label,
                                id: item.value
                            }
                        }))
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            }
        });
}

function confirmDialog(msg, obj, callback) {
    if (holdconfirmObj != null) {
        holdconfirmObj = null;
        return true;
    }

    holdconfirmObj = obj;
    $("<div id='confirmDialog' style='padding-left:20px;padding-right:20px;text-align:center'></br> " + msg + " </br></div>").dialog({
        resizable: false,
        height: 160,
        width: 500,
        title: "Confirm",
        modal: true,
        open: function () {
            $(this).parent().find(".ui-dialog, .ui-dialog-buttonpane, .ui-dialog-buttonset").css({
                "float": "none",
                "text-align": "center",
                width: 'auto'
            });
            $(this).parent().find(".ui-dialog, .ui-dialog-buttonpane").css("text-align", "center");
            $(this).parent().find(".ui-dialog-buttonset .ui-button-text").attr("class", "button");
        },
        buttons: {
            "Yes": function () {
                $(holdconfirmObj).click();
                $(this).dialog("close");
            },
            "No": function () {
                holdconfirmObj = null;
                $(this).dialog("close");
                return false;
            }
        }
    });
    return false;
}

function ConfirmDialog(msg, dmodal) {
    var errd = $("<div id='confirmDialog' style='padding-left:20px;padding-right:20px;'></div>").dialog({
        autoOpen: false,
        title: "Confirm",
        modal: dmodal,
        resizable: false,
        width: 'auto',
        height: 'auto',
        minWidth: 300,
        position: 'center',
        open: function () {
            $(this).parent().find(".ui-dialog-titlebar-close").hide();
            $(this).parent().find(".ui-dialog, .ui-dialog-buttonpane, .ui-dialog-buttonset").css({
                "float": "none",
                "text-align": "center",
                width: 'auto'
            });
            $(this).parent().find(".ui-dialog, .ui-dialog-buttonpane").css("text-align", "center");
            $('.ui-dialog :button').focus();

        },
        buttons: {
            "OK": function (event, ui) {
                $(".content_section").focus();
                $(this).dialog('destroy').remove();
                return true;
            },
            "Cancel": function (event, ui) {
                $(".content_section").focus();
                $(this).dialog('destroy').remove();
                return false;
            }
        }
    });

    errd.html(msg).dialog("open");
}