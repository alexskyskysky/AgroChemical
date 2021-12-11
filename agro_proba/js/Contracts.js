Ext.net.FilterHeader.behaviour.addBehaviour("string", {
    name: "any",

    is: function (value) {
        return Ext.net.StringUtils.startsWith(value, "any ");
    },

    getValue: function (value) {
        var values = Ext.net.FilterHeader.behaviour.getStrValue(value).substring(4).split(","),
            tmp = [];

        Ext.each(values, function (v) {
            v = v.trim();
            if (!Ext.isEmpty(v)) {
                tmp.push(v);
            }
        });

        values = tmp;

        return { value: values, valid: values.length > 0 };
    },

    match: function (recordValue, matchValue) {
        for (var i = 0; i < matchValue.length; i++) {
            if (recordValue === matchValue[i]) {
                return true;
            }
        }

        return false;
    },

    isValid: function (value) {
        return this.getValue(value, field).valid;
    },

    serialize: function (value) {
        return {
            type: "string",
            op: "any",
            value: value
        };
    }
});

function getTextValue() {
    var value = this.getComponent(1).getValue();
    return (Ext.isEmpty(value) ? "" : this.getComponent(0).text) + value;
}

function getCheckedValue() {
    var text = [];

    this.menu.items.each(function (item) {
        if (item.checked) {
            text.push(item.text);
        }
    });

    if (text.length == 0) {
        return "";
    } else {
        return "any " + text.join(",");
    }
}

function onItemCheck(menuItem) {
    var checked = false,
        button = menuItem.up('button');

    menuItem.parentMenu.items.each(function (item) {
        if (item.checked) {
            checked = true;
            return false;
        }
    });

    if (checked) {
        button.setText("[Фильтрация]");
    } else {
        button.setText("[Без фильтра]");
    }

    menuItem.up('grid').filterHeader.onFieldChange(button);
}

var getRowClass = function (record) {
    var b = record.data.balance;
    var count_days;

    if (record.data.payment_days != "" && record.data.payment_days != "null" && record.data.payment_days != null) {
        if (record.data.payment_days == 0) { count_days = 10; }
        else { count_days = record.data.payment_days; }
    }
    else { count_days = 10; }

    var today_date = new Date();
    var dfinish = new Date();
    var dfulfilment = new Date();
    var t_days = 0;

    if (record.data.date_finish != "" && record.data.date_finish != "null" && record.data.date_finish != null)
    {
        dfinish = record.data.date_finish;
    }
    if (record.data.date_fulfilment != "" && record.data.date_fulfilment != "null" && record.data.date_fulfilment != null)
    {
        dfulfilment = record.data.date_fulfilment;
    }

    t_days = ((+today_date.valueOf()) / 86400000) - ((+dfinish.valueOf()) / 86400000);
    if (t_days > (count_days + 2) && b > 0) { return "red-row"; }
    if (t_days < (count_days + 2) && record.data.date_finish != "" && record.data.date_finish != "null" && record.data.date_finish != null) { return "green-row"; }

    t_days = ((+dfulfilment.valueOf()) / 86400000) - ((+today_date.valueOf()) / 86400000);
    if (t_days < 0 && (record.data.date_finish == "" || record.data.date_finish == "null" || record.data.date_finish == null)) { return "yellow-row"; }

    return "white-row";
}