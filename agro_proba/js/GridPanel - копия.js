(function (window, $, undefined) {
    var GRID_PANEL = "GridPanel",
        GRID_PANEL_DATA_KEY = GRID_PANEL,
        GRID_PANEL_ROW_DATA_KEY = "GridPanelItem",
        GRID_PANEL_EDIT_ROW_DATA_KEY = "GridPanelEditRow";

    function Grid(element, config) {
        var $element = $(element);
        $element.data(GRID_PANEL_DATA_KEY, this);
        this._container = $element;
        this.data = [];
        this.fields = [];
        this.Field = Field;
        this.service = {
            name: "",
            command: ""
        };
        this._init(config);
    }

    Grid.prototype = {
        headerRowClass: "gridpanel-header-row",
        bodyRowClass: "gridpanel-body-row",
        selectedRowClass: "gridpanel-selected-row",
        rowClick: $.noop,
        rowDoubleClick: function (args) {
            /*if (this.editing) {
                this.editItem($(args.event.target).closest("tr"));
            }*/
        },

        _init: function (config) {
            $.extend(this, config);
            this.loadData();
            this._createTable();
        },

        destroy: function () {
            this._clear();
            this._container.removeData(GRID_PANEL_DATA_KEY);
        },
        //создание таблицы с данными
        //функция перечисления полей
        _eachField: function (callBack) {
            var self = this;
            $.each(this.fields, function (index, field) {
                if (field.visible) {
                    callBack.call(self, field, index);
                }
            });
        },
        //функция подготовки ячейки
        _prepareCell: function (cell, field) {
            return $(cell).css("width", field.width)
                .addClass(field.css)
                .addClass(field.align);
        },
        //функция создания строки заголовка
        _createHeaderRow: function () {
            var header_row = $("<tr></tr>").addClass(this.headerRowClass);
            this._eachField(function (field, index) {
                var header_row_cell = this._prepareCell("<td>", field)
                    .append(field.headerTemplate ? field.headerTemplate() : "")
                    .appendTo(header_row);
            });
            return header_row;
        },
        //функция создания заголовка
        _createHeader: function () {
            var header = $("<thead></thead>");
            header.append(this._createHeaderRow());
            return header;
        },
        //функция получения значения данныхх для ячейки
        _getItemFieldValue: function (item, field) {
            var props = field.name.split('.');
            var result = item[props.shift()];

            while (result && props.length) {
                result = result[props.shift()];
            }

            return result;
        },
        //функция создания ячеек в строке с данными
        _createCell: function (item, field) {
            var rowCell;
            var fieldValue = this._getItemFieldValue(item, field);
            rowCell = $("<td></td>").append(field.itemTemplate ? field.itemTemplate(fieldValue, item) : fieldValue);
            return this._prepareCell(rowCell, field);
        },
        //функция добавления ячеек в строку
        _renderCells: function ($row, item) {
            this._eachField(function (field) {
                $row.append(this._createCell(item, field));
            });
            return this;
        },
        //выделение строки
        _attachRowHover: function ($row) {
            var selectedRowClass = this.selectedRowClass;
            $row.hover(function () {
                $(this).addClass(selectedRowClass);
            },
                function () {
                    $(this).removeClass(selectedRowClass);
                }
            );
        },
        //функция создания строки с данными
        _createDataRow: function (item, itemIndex) {
            var dataRow = $("<tr></tr>");
            this._renderCells(dataRow, item);
            dataRow.data(GRID_PANEL_ROW_DATA_KEY, item);
            dataRow.on("click", $.proxy(function (e) {
                    this.rowClick({
                        item: item,
                        itemIndex: itemIndex,
                        event: e
                    });
                }, this))
                .on("dblclick", $.proxy(function (e) {
                    this.rowDoubleClick({
                        item: item,
                        itemIndex: itemIndex,
                        event: e
                    });
                }, this));

            if (this.selecting) {
                this._attachRowHover($result);
            }

            return dataRow;
        },
        //функция создания тела таблицы
        _createBody: function () {
            var body = $("<tbody></tbody>");
            body.empty();
            if (!this.data.length) {
                //$content.append(this._createNoDataRow());
                return this;
            }
            else {
                for (var itemIndex = 0; itemIndex < this.data.length; itemIndex++) {
                    var item = this.data[itemIndex];
                    body.append(this._createRow(item, itemIndex));
                }
            }

            return body;
        },
        //функция создания таблицы
        _createTable: function () {
            var table = $("<table></table>");
            table.append(this._createHeader());
            table.append(this._createBody());
            //table.appendTo(this._container);
            this._container.html(table);
        },
        //--------------------------
        loadData: function () {
            var table_data = [];
            $.ajax({
                url: this.service.name + "/" + this.service.command,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    table_data = JSON.parse(data.d);
                }
            });
            this.data = table_data;
        },
        addRow: function () {

        },
        editRow: function () {

        },
        deleteRow: function () {

        }
    };

    $.fn.GridPanel = function (config) {
        this.each(function () {
            var $element = $(this);
            new Grid($element, config);
            return this;
        });
    };
/*
    var fields = {};

    var setDefaults = function (config) {
        var componentPrototype;

        if ($.isPlainObject(config)) {
            componentPrototype = Grid.prototype;
        } else {
            componentPrototype = fields[config].prototype;
            config = arguments[1] || {};
        }

        $.extend(componentPrototype, config);
    };

    window.GridPanel = {
        Grid: Grid,
        fields: fields,
        setDefaults: setDefaults
    };*/

    function Field(config) {
        $.extend(this, config);
        //this.sortingFunc = this._getSortingFunc();
    }

    Field.prototype = {
        name: "",
        title: null,
        css: "",
        align: "",
        width: 100,

        visible: true,
        filtering: true,
        inserting: true,
        editing: true,
        sorting: true,
        sorter: "string", // name of SortStrategy or function to compare elements

        headerTemplate: function () {
            return (this.title === undefined || this.title === null) ? this.name : this.title;
        },

        itemTemplate: function (value, item) {
            return value;
        },

        filterTemplate: function () {
            return "";
        },

        insertTemplate: function () {
            return "";
        },

        editTemplate: function (value, item) {
            this._value = value;
            return this.itemTemplate(value, item);
        },

        filterValue: function () {
            return "";
        },

        insertValue: function () {
            return "";
        },

        editValue: function () {
            return this._value;
        }
    };
})(window, jQuery);
/*
(function (GridPanel, $, undefined) {

    function Field(config) {
        $.extend(this, config);
        //this.sortingFunc = this._getSortingFunc();
    }

    Field.prototype = {
        name: "",
        title: null,
        css: "",
        align: "",
        width: 100,

        visible: true,
        filtering: true,
        inserting: true,
        editing: true,
        sorting: true,
        sorter: "string", // name of SortStrategy or function to compare elements

        headerTemplate: function () {
            return (this.title === undefined || this.title === null) ? this.name : this.title;
        },

        itemTemplate: function (value, item) {
            return value;
        },

        filterTemplate: function () {
            return "";
        },

        insertTemplate: function () {
            return "";
        },

        editTemplate: function (value, item) {
            this._value = value;
            return this.itemTemplate(value, item);
        },

        filterValue: function () {
            return "";
        },

        insertValue: function () {
            return "";
        },

        editValue: function () {
            return this._value;
        }
    };

    GridPanel.Field = Field;

}(GridPanel, jQuery));*/