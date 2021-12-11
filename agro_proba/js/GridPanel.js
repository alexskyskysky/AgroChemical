(function (window, $, undefined) {
    var GRID_PANEL = "GridPanel",
        GRID_PANEL_DATA_KEY = GRID_PANEL,
        GRID_PANEL_ROW_DATA_KEY = "GridPanelItem",
        GRID_PANEL_EDIT_ROW_DATA_KEY = "GridPanelEditRow";

    var getOrApply = function (value, context) {
        if ($.isFunction(value)) {
            return value.apply(context, $.makeArray(arguments).slice(2));
        }
        return value;
    };

    function Grid(element, config) {
        var $element = $(element);
        $element.data(GRID_PANEL_DATA_KEY, this);
        this._container = $element;
        this.data = [];
        this.fields = [];
        this.service = {
            select_name: "",
            select_command: "",
            insert_name: "",
            insert_command: "",
            update_name: "",
            update_command: "",
            delete_name: "",
            delete_command: ""
        };
        this._filterRow = null;
        this._insertRow = null;
        this._editingRow = null;
        this._init(config);
    }

    Grid.prototype = {
        width: "700",
        headerRowClass: "gridpanel-header-row",
        filterRowClass: "gridpanel-filter-row",
        insertRowClass: "gridpanel-insert-row",
        bodyRowClass: "gridpanel-body-row",
        cellClass: "gridpanel-cell",
        headerCellClass: "gridpanel-header-cell",
        tableClass: "gridpanel-table",
        selectedRowClass: "gridpanel-selected-row",
        gridHeaderClass: "gridpanel-header",
        gridBodyClass: "gridpanel-body",
        gridClass: "gridpanel-grid",

        editing: true,
        confirmDeleting: true,
        deleteConfirm: "Подтвердите удаление!",
        rowClick: $.noop,
        rowDoubleClick: function (args) {
            if (this.editing) {
                this.editItem($(args.event.target).closest("tr"));
            }
        },
        //валидация значений
        validation: function (config) {
            return GridPanel.Validation && new GridPanel.Validation(config);
        },
        //функция создания валидатора
        _createValidation: function () {
            return getOrApply(this.validation, this);
        },

        invalidMessage: "Некорректные данные!",

        invalidNotify: function (args) {
            var messages = $.map(args.errors, function (error) {
                return error.message || null;
            });

            window.alert([this.invalidMessage].concat(messages).join("\n"));
        },

        _init: function (config) {
            $.extend(this, config);
            this._initFields();
            this.createGrid();
            this.loadData();

            var $this = this;
            $(window).resize(function () {
                $this._container.outerHeight($this._container.parent().height());
                var header_height = $this._container.find("#" + $this._container.prop("id") + "Header").outerHeight(true);
                var height = $this._container.height();
                $this._container.find("#" + $this._container.prop("id") + "Body").outerHeight(height - header_height);
            });
        },

        destroy: function () {
            this._clear();
            this._container.removeData(GRID_PANEL_DATA_KEY);
        },
        //создание таблицы с данными
        //создание полей
        _initFields: function () {
            var self = this;
            self.fields = $.map(self.fields, function (field) {
                if ($.isPlainObject(field)) {
                    var fieldConstructor = (field.type && GridPanel.fields[field.type]) || GridPanel.Field;
                    field = new fieldConstructor(field);
                }
                //field = GridPanel.Field();
                field._grid = self;
                return field;
            });
        },
        //функция перечисления полей
        _eachField: function (callBack) {
            var $self = this;
            $.each(this.fields, function (index, field) {
                if (field.visible) {
                    callBack.call($self, field, index);
                }
            });
        },
        //функция подготовки ячейки
        _prepareCell: function (isHeaderCell, cell, field) {
            var cell_class = this.cellClass;
            if (isHeaderCell) { cell_class = this.headerCellClass; }
            return $(cell).css("width", field.width)
                          .addClass(cell_class)
                          .addClass(field.css)
                          .addClass(field.align ? ("gridpanel-align-" + field.align) : "");
        },
        //функция создания строки заголовка
        _createHeaderRow: function () {
            var $header_row = $("<tr>").addClass(this.headerRowClass);
            this._eachField(function (field, index) {
                var header_row_cell = this._prepareCell(true, "<td>", field).append(field.headerTemplate ? field.headerTemplate() : "");
                header_row_cell.appendTo($header_row);
            });
            return $header_row;
        },
        //функция создани строки фильтрации
        _createFilterRow: function () {
            var $filter_row = $("<tr>").addClass(this.filterRowClass);
            this._eachField(function (field) {
                var filter_row_cell = this._prepareCell(false, "<td>", field).append(field.filterTemplate ? field.filterTemplate() : "");
                filter_row_cell.appendTo($filter_row);
            });
            return $filter_row;
        },
        //функция создани строки добавления
        _createInsertRow: function () {
            var $insert_row = $("<tr>").addClass(this.insertRowClass);
            this._eachField(function (field) {
                var insert_row_cell = this._prepareCell(false, "<td>", field).append(field.insertTemplate ? field.insertTemplate() : "");
                insert_row_cell.appendTo($insert_row);
            });
            return $insert_row;
        },
        //перерисовка данных в таблице
        rerenderData: function () {
            this._container.find("#" + this._container.prop("id") + "Body").remove();
            this._container.append(this._createBody());
            $(window).resize();
        },
        //функция создания заголовка
        _createHeader: function () {
            this._filterRow = this._createFilterRow();
            this._insertRow = this._createInsertRow();
            var thead = $("<thead>").append(this._createHeaderRow());
            var tbody = $("<tbody>").append(this._filterRow).append(this._insertRow);
            var headerTable = $("<table>").addClass(this.tableClass).css("width", this.width).append(thead).append(tbody);
            var header = $("<div>", { id: this._container.prop("id") + "Header" }).addClass(this.gridHeaderClass).css("min-width", this.width).append(headerTable);
            return header;
        },
        //функция получения значения данных для ячейки
        _getItemFieldValue: function (item, field) {
            var props = field.name.split('.');
            var result = item[props.shift()];

            while (result && props.length) {
                result = result[props.shift()];
            }

            return result;
        },
        //функция установка значения в ячейку
        _setItemFieldValue: function (item, field, value) {
            var props = field.name.split('.');
            var current = item;
            var prop = props[0];

            while (current && props.length) {
                item = current;
                prop = props.shift();
                current = item[prop];
            }

            if (!current) {
                while (props.length) {
                    item = item[prop] = {};
                    prop = props.shift();
                }
            }

            item[prop] = value;
        },
        //функция создания ячеек в строке с данными
        _createCell: function (item, field) {
            var rowCell;
            var fieldValue = this._getItemFieldValue(item, field);
            rowCell = $("<td>").append(field.itemTemplate ? field.itemTemplate(fieldValue, item) : fieldValue);
            return this._prepareCell(false, rowCell, field);
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
            var dataRow = $("<tr>").addClass(this.bodyRowClass);
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
            var tbody = $("<tbody>");
            tbody.empty();
            if (!this.data.length) {
                //$content.append(this._createNoDataRow());
                return this;
            }
            else {
                for (var itemIndex = 0; itemIndex < this.data.length; itemIndex++) {
                    var item = this.data[itemIndex];
                    tbody.append(this._createDataRow(item, itemIndex));
                }
            }

            var bodyTable = $("<table>").addClass(this.tableClass).css("width", this.width).append(tbody);
            var body = $("<div>", { id: this._container.prop("id") + "Body" }).addClass(this.gridBodyClass).css("min-width", this.width).append(bodyTable);
            return body;
        },
        //функция создания таблицы
        createGrid: function () {
            this._container.addClass(this.gridClass)
                .append(this._createHeader())
                .append(this._createBody());
            this._validation = this._createValidation();
        },
        //--------------------------
        loadData: function () {
            var $this = this;
            $.ajax({
                url: this.service.select_name + "/" + this.service.select_command,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $this.data = JSON.parse(data.d);
                    $this.rerenderData();
                    
                    //$this.createGrid();
                }
            });
        },
        //добавление элемента
        insertItem: function (item) {
            var insertingItem = item || this._getValidatedInsertItem();

            if (!insertingItem)
                return $.Deferred().reject().promise();

            return this._insertItemOnServer(insertingItem, function (insertedItem) {
                insertedItem = insertedItem || insertingItem;
                this._finishInsert(insertedItem);
            });
        },
        //проверка валидности добавляемого элемента
        _getValidatedInsertItem: function () {
            var item = this._getInsertItem();
            return this._validateItem(item, this._insertRow) ? item : null;
        },
        //получение добавляемого элемента
        _getInsertItem: function () {
            var result = {};
            this._eachField(function (field) {
                if (field.inserting) {
                    this._setItemFieldValue(result, field, field.insertValue());
                }
            });
            return result;
        },
        //завершение добавления данных
        _finishInsert: function (item) {
            //this.data.push(item);
            this.loadData();
        },
        //добавление данных на сервер
        _insertItemOnServer: function (item, doneCallback) {
            return $.ajax({
                url: this.service.insert_name + "/" + this.service.insert_command,
                data: item,
                type: "POST",
                success: $.proxy(doneCallback, this)
            });
        },
        //очистка полей добавления данных
        clearInsert: function () {
            var insertRow = this._createInsertRow();
            this._insertRow.replaceWith(insertRow);
            this._insertRow = insertRow;
        },
        //обновление элемента
        updateItem: function (item, editedItem) {
            if (arguments.length === 1) {
                editedItem = item;
            }

            var $row = item ? this.rowByItem(item) : this._editingRow;
            editedItem = editedItem || this._getValidatedEditedItem();

            if (!editedItem)
                return;

            //return
            this._updateRow($row, editedItem);
        },
        //проверка значений на валидность
        _getValidatedEditedItem: function () {
            var item = this._getEditedItem();
            return this._validateItem(item, this._getEditRow()) ? item : null;
        },
        //получение отредактированных значений
        _getEditedItem: function () {
            var result = {};
            this._eachField(function (field) {
                if (field.editing) {
                    this._setItemFieldValue(result, field, field.editValue());
                }
            });
            return result;
        },
        //обновление строки в таблице
        _updateRow: function ($updatingRow, editedItem) {
            var updatingItem = $updatingRow.data(GRID_PANEL_ROW_DATA_KEY),
                updatingItemIndex = this._itemIndex(updatingItem),
                previousItem = $.extend(true, {}, updatingItem);

            $.extend(true, updatingItem, editedItem);
            return this._updateItemOnServer(updatingItem, function (updatedItem) {
                updatedItem = updatedItem || updatingItem;
                var $updatedRow = this._finishUpdate($updatingRow, updatedItem, updatingItemIndex);
            });
        },
        //серверная функция на обновление
        _updateItemOnServer: function (item, doneCallback) {
            return $.ajax({
                url: this.service.update_name + "/" + this.service.update_command,
                data: item,
                type: "POST",
                success: $.proxy(doneCallback, this)
            });
        },
        //получение номера строки таблицы
        _rowIndex: function (row) {
            //return this._content.children().index($(row));
            return this._container.find("table").eq(1).children().index($(row));
        },
        //получение номера элемента в массиве данных
        _itemIndex: function (item) {
            return $.inArray(item, this.data);
        },
        //завершение редактирования
        _finishUpdate: function ($updatingRow, updatedItem, updatedItemIndex) {
            this.cancelEdit();
            this.data[updatedItemIndex] = updatedItem;

            var $updatedRow = this._createDataRow(updatedItem, updatedItemIndex);
            $updatingRow.replaceWith($updatedRow);
            return $updatedRow;
        },
        //редактирование строки таблицы
        editItem: function(item) {
            var $row = this.rowByItem(item);
            if($row.length)
                this._editRow($row);
        },
        //получение строки данных
        rowByItem: function(item) {
            if(item.jquery || item.nodeType)
                return $(item);
            return $("#" + this._container.id + "Body").find("tbody").eq(0).find("tr").filter(function () {
                return $.data($this, GRID_PANEL_ROW_DATA_KEY) === item;
            });
        },
        //редактирование строки
        _editRow: function($row) {
            if(!this.editing)
                return;

            var item = $row.data(GRID_PANEL_ROW_DATA_KEY);

            if (this._editingRow) {
                this.cancelEdit();
            }

            var $editRow = this._createEditRow(item);

            this._editingRow = $row;

            $row.hide();
            $editRow.insertBefore($row);
            $row.data(GRID_PANEL_EDIT_ROW_DATA_KEY, $editRow);
        },
        //создание строки редактирования
        _createEditRow: function(item) {
            var $result = $("<tr>").addClass(this.bodyRowClass);

            this._eachField(function(field) {
                var fieldValue = this._getItemFieldValue(item, field);

                this._prepareCell(false, "<td>", field).css("padding","0px")
                    .append(field.editTemplate ? field.editTemplate(fieldValue, item) : "")
                    .appendTo($result);
            });

            return $result;
        },
        //закрытие редактирования
        cancelEdit: function () {
            if (!this._editingRow)
                return;

            this._getEditRow().remove();
            this._editingRow.show();
            this._editingRow = null;
        },
        //получение редактируемой строки
        _getEditRow: function () {
            return this._editingRow.data(GRID_PANEL_EDIT_ROW_DATA_KEY);
        },
        //функция удаления элемента данных
        deleteItem: function(item) {
            var $row = this.rowByItem(item);

            if(!$row.length)
                return;

            if(this.confirmDeleting && !window.confirm(getOrApply(this.deleteConfirm, this, $row.data(GRID_PANEL_ROW_DATA_KEY))))
                return;

            return this._deleteRow($row);
        },
        //функция удаления строки
        _deleteRow: function($row) {
            var deletingItem = $row.data(GRID_PANEL_ROW_DATA_KEY),
                deletingItemIndex = this._itemIndex(deletingItem);
            return this._deleteItemOnServer(deletingItem, function () {
                this._finishDelete(deletingItem, deletingItemIndex);
            });
        },
        //завершение удаления
        _finishDelete: function(deletedItem, deletedItemIndex) {
            this.option("data").splice(deletedItemIndex, 1);
        },
        //удаление данных на сервере
        _deleteItemOnServer: function (item, doneCallback) {
            return $.ajax({
                url: this.service.delete_name + "/" + this.service.delete_command,
                data: item,
                type: "POST",
                success: $.proxy(doneCallback, this)
            });
        },
        //функция проверки валидности значения
        _validateItem: function (item, $row) {
            var validationErrors = [];

            var args = {
                item: item,
                itemIndex: this._rowIndex($row),
                row: $row
            };

            this._eachField(function (field, index) {
                if (!field.validate)
                    return;

                var errors = this._validation.validate($.extend({
                    value: this._getItemFieldValue(item, field),
                    rules: field.validate
                }, args));

                this._setCellValidity($row.children().eq(index), errors);

                if (!errors.length)
                    return;

                validationErrors.push.apply(validationErrors,
                    $.map(errors, function (message) {
                        return { field: field, message: message };
                    }));
            });

            if (!validationErrors.length)
                return true;

            var invalidArgs = $.extend({
                errors: validationErrors
            }, args);
            //this._callEventHandler(this.onItemInvalid, invalidArgs);
            $.extend(eventParams, {
                grid: this
            });
            this.invalidNotify(invalidArgs);

            return false;
        },

        _setCellValidity: function ($cell, errors) {
            $cell.toggleClass(this.invalidClass, !!errors.length)
                 .attr("title", errors.join("\n"));
        }
    };

    $.fn.GridPanel = function (config) {
        this.each(function () {
            var $element = $(this);
            new Grid($element, config);
            return this;
        });
    };

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
    };
})(window, jQuery);

(function (GridPanel, $, undefined) {

    function Validation(config) {
        this._init(config);
    }

    Validation.prototype = {

        _init: function (config) {
            $.extend(true, this, config);
        },

        validate: function (args) {
            var errors = [];

            $.each(this._normalizeRules(args.rules), function (_, rule) {
                if (rule.validator(args.value, args.item, rule.param))
                    return;

                var errorMessage = $.isFunction(rule.message) ? rule.message(args.value, args.item) : rule.message;
                errors.push(errorMessage);
            });

            return errors;
        },

        _normalizeRules: function (rules) {
            if (!$.isArray(rules))
                rules = [rules];

            return $.map(rules, $.proxy(function (rule) {
                return this._normalizeRule(rule);
            }, this));
        },

        _normalizeRule: function (rule) {
            if (typeof rule === "string")
                rule = { validator: rule };

            if ($.isFunction(rule))
                rule = { validator: rule };

            if ($.isPlainObject(rule))
                rule = $.extend({}, rule);
            else
                throw Error("Неправильная конфигурация валидатора");

            if ($.isFunction(rule.validator))
                return rule;

            return this._applyNamedValidator(rule, rule.validator);
        },

        _applyNamedValidator: function (rule, validatorName) {
            delete rule.validator;

            var validator = validators[validatorName];
            if (!validator)
                throw Error("Неизвестный валидатор \"" + validatorName + "\"");

            if ($.isFunction(validator)) {
                validator = { validator: validator };
            }

            return $.extend({}, validator, rule);
        }
    };

    GridPanel.Validation = Validation;


    var validators = {
        required: {
            message: "Поле обязательное для заполнения",
            validator: function (value) {
                return value !== undefined && value !== null && value !== "";
            }
        },

        rangeLength: {
            message: "Длина значения поля находится вне заданного диапазона",
            validator: function (value, _, param) {
                return value.length >= param[0] && value.length <= param[1];
            }
        },

        minLength: {
            message: "Значение поля слишком длинное",
            validator: function (value, _, param) {
                return value.length >= param;
            }
        },

        maxLength: {
            message: "Значение поля слишком короткое",
            validator: function (value, _, param) {
                return value.length <= param;
            }
        },

        pattern: {
            message: "Значение поля не соответствует заданному шаблону",
            validator: function (value, _, param) {
                if (typeof param === "string") {
                    param = new RegExp("^(?:" + param + ")$");
                }
                return param.test(value);
            }
        },

        range: {
            message: "Значение поля находится вне заданного диапазона",
            validator: function (value, _, param) {
                return value >= param[0] && value <= param[1];
            }
        },

        min: {
            message: "Значение поля слишком мало",
            validator: function (value, _, param) {
                return value >= param;
            }
        },

        max: {
            message: "Значение поля слишком велико",
            validator: function (value, _, param) {
                return value <= param;
            }
        }
    };

    GridPanel.validators = validators;

}(GridPanel, jQuery));

(function (GridPanel, $, undefined) {
    function Field(config) {
        $.extend(true, this, config);
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
}(GridPanel, jQuery));

(function (GridPanel, $, undefined) {

    var Field = GridPanel.Field;

    function TextField(config) {
        Field.call(this, config);
    }

    TextField.prototype = new Field({

        autosearch: true,
        readOnly: false,

        filterTemplate: function () {
            if (!this.filtering)
                return "";

            var grid = this._grid,
                $result = this.filterControl = this._createTextBox();

            if (this.autosearch) {
                $result.on("keypress", function (e) {
                    if (e.which === 13) {
                        grid.search();
                        e.preventDefault();
                    }
                });
            }

            return $result;
        },

        insertTemplate: function () {
            if (!this.inserting)
                return "";

            return this.insertControl = this._createTextBox();
        },

        editTemplate: function (value) {
            if (!this.editing)
                return this.itemTemplate(value);

            var $result = this.editControl = this._createTextBox();
            $result.val(value);
            return $result;
        },

        filterValue: function () {
            return this.filterControl.val();
        },

        insertValue: function () {
            return this.insertControl.val();
        },

        editValue: function () {
            return this.editControl.val();
        },

        _createTextBox: function () {
            return $("<input>").attr("type", "text").attr("align", this.align)
                .prop("readonly", !!this.readOnly);
        }
    });

    GridPanel.fields.text = GridPanel.TextField = TextField;

}(GridPanel, jQuery));

(function (GridPanel, $, undefined) {

    var TextField = GridPanel.TextField;

    function NumberField(config) {
        TextField.call(this, config);
    }

    NumberField.prototype = new TextField({

        sorter: "number",
        align: "right",
        readOnly: false,

        filterValue: function () {
            return parseInt(this.filterControl.val() || 0, 10);
        },

        insertValue: function () {
            return parseInt(this.insertControl.val() || 0, 10);
        },

        editValue: function () {
            return parseInt(this.editControl.val() || 0, 10);
        },

        _createTextBox: function () {
            return $("<input>").attr("type", "number").css("text-align", this.align).css("width", "100%")
                .prop("readonly", !!this.readOnly);
        }
    });

    GridPanel.fields.number = GridPanel.NumberField = NumberField;

}(GridPanel, jQuery));

(function (GridPanel, $, undefined) {

    var TextField = GridPanel.TextField;

    function TextAreaField(config) {
        TextField.call(this, config);
    }

    TextAreaField.prototype = new TextField({

        insertTemplate: function () {
            if (!this.inserting)
                return "";

            return this.insertControl = this._createTextArea();
        },

        editTemplate: function (value) {
            if (!this.editing)
                return this.itemTemplate(value);

            var $result = this.editControl = this._createTextArea();
            $result.val(value);
            return $result;
        },

        _createTextArea: function () {
            return $("<textarea>").css("text-align", this.align).css("width", "100%").css("height", "100px").prop("readonly", !!this.readOnly);
        }
    });

    GridPanel.fields.textarea = GridPanel.TextAreaField = TextAreaField;

}(GridPanel, jQuery));

(function (GridPanel, $, undefined) {

    var NumberField = GridPanel.NumberField;

    function ComboBoxField(config) {
        this.items = [];
        this.selectedIndex = -1;
        this.valueField = "";
        this.textField = "";
        this.service = {
            name: "",
            command: ""
        };

        if (config.valueField && config.items.length) {
            this.valueType = typeof config.items[0][config.valueField];
        }

        this.sorter = this.valueType;

        NumberField.call(this, config);
    }

    ComboBoxField.prototype = new NumberField({

        align: "center",
        valueType: "number",

        itemTemplate: function (value) {
            var items = this.items,
                valueField = this.valueField,
                textField = this.textField,
                resultItem;

            if (valueField) {
                resultItem = $.grep(items, function (item, index) {
                    return item[valueField] === value;
                })[0] || {};
            }
            else {
                resultItem = items[value];
            }

            var result = (textField ? resultItem[textField] : resultItem);

            return (result === undefined || result === null) ? "" : result;
        },

        filterTemplate: function () {
            if (!this.filtering)
                return "";

            var grid = this._grid,
                $result = this.filterControl = this._createSelect();

            if (this.autosearch) {
                $result.on("change", function (e) {
                    grid.search();
                });
            }

            return $result;
        },

        insertTemplate: function () {
            if (!this.inserting)
                return "";

            return this.insertControl = this._createSelect();
        },

        editTemplate: function (value) {
            if (!this.editing)
                return this.itemTemplate(value);

            var $result = this.editControl = this._createSelect();
            (value !== undefined) && $result.val(value);
            return $result;
        },

        filterValue: function () {
            var val = this.filterControl.val();
            return this.valueType === "number" ? parseInt(val || 0, 10) : val;
        },

        insertValue: function () {
            var val = this.insertControl.val();
            return this.valueType === "number" ? parseInt(val || 0, 10) : val;
        },

        editValue: function () {
            var val = this.editControl.val();
            return this.valueType === "number" ? parseInt(val || 0, 10) : val;
        },

        _init: function () {
            this._loadItems();
        },

        _loadItems: function () {
            var $this = this;
            $.ajax({
                url: this.service.name + "/" + this.service.command,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $this.items = JSON.parse(data.d);
                }
            });
        },

        _createSelect: function () {
            var $result = $("<select>").css("text-align", this.align).css("width", "100%"),
                valueField = this.valueField,
                textField = this.textField,
                selectedIndex = this.selectedIndex;
            this.
            $.each(this.items, function (index, item) {
                var value = valueField ? item[valueField] : index,
                    text = textField ? item[textField] : item;

                var $option = $("<option>")
                    .attr("value", value)
                    .text(text)
                    .appendTo($result);

                $option.prop("selected", (selectedIndex === index));
            });

            $result.prop("disabled", !!this.readOnly);

            return $result;
        }
    });

    GridPanel.fields.select = GridPanel.ComboBoxField = ComboBoxField;

}(GridPanel, jQuery));

(function (GridPanel, $, undefined) {

    var Field = GridPanel.Field;

    function CheckBoxField(config) {
        Field.call(this, config);
    }

    CheckBoxField.prototype = new Field({

        sorter: "number",
        align: "center",
        autosearch: true,

        itemTemplate: function (value) {
            return this._createCheckbox().prop({
                checked: value,
                disabled: true
            });
        },

        filterTemplate: function () {
            if (!this.filtering)
                return "";

            var grid = this._grid,
                $result = this.filterControl = this._createCheckbox();

            $result.prop({
                readOnly: true,
                indeterminate: true
            });

            $result.on("click", function () {
                var $cb = $(this);

                if ($cb.prop("readOnly")) {
                    $cb.prop({
                        checked: false,
                        readOnly: false
                    });
                }
                else if (!$cb.prop("checked")) {
                    $cb.prop({
                        readOnly: true,
                        indeterminate: true
                    });
                }
            });

            if (this.autosearch) {
                $result.on("click", function () {
                    grid.search();
                });
            }

            return $result;
        },

        insertTemplate: function () {
            if (!this.inserting)
                return "";

            return this.insertControl = this._createCheckbox();
        },

        editTemplate: function (value) {
            if (!this.editing)
                return this.itemTemplate(value);

            var $result = this.editControl = this._createCheckbox();
            $result.prop("checked", value);
            return $result;
        },

        filterValue: function () {
            return this.filterControl.get(0).indeterminate
                ? undefined
                : this.filterControl.is(":checked");
        },

        insertValue: function () {
            return this.insertControl.is(":checked");
        },

        editValue: function () {
            return this.editControl.is(":checked");
        },

        _createCheckbox: function () {
            return $("<input>").attr("type", "checkbox").css("text-align", this.align).css("width", "100%");
        }
    });

    GridPanel.fields.checkbox = GridPanel.CheckBoxField = CheckBoxField;

}(GridPanel, jQuery));

(function (GridPanel, $, undefined) {

    var Field = GridPanel.Field;

    function ControlField(config) {
        Field.call(this, config);
        this._configInitialized = false;
    }

    ControlField.prototype = new Field({
        css: "gridpanel-control-field",
        align: "center",
        width: 50,
        filtering: false,
        inserting: false,
        editing: false,
        sorting: false,

        buttonClass: "gridpanel-button",
        modeButtonClass: "gridpanel-mode-button",

        modeOnButtonClass: "gridpanel-mode-on-button",
        searchModeButtonClass: "gridpanel-search-mode-button",
        insertModeButtonClass: "gridpanel-insert-mode-button",
        editButtonClass: "gridpanel-edit-button",
        deleteButtonClass: "gridpanel-delete-button",
        searchButtonClass: "gridpanel-search-button",
        clearFilterButtonClass: "gridpanel-clear-filter-button",
        insertButtonClass: "gridpanel-insert-button",
        updateButtonClass: "gridpanel-update-button",
        cancelEditButtonClass: "gridpanel-cancel-edit-button",

        searchModeButtonTooltip: "Переключить на поиск",
        insertModeButtonTooltip: "Переключить на добавление",
        editButtonTooltip: "Редактировать",
        deleteButtonTooltip: "Удалить",
        searchButtonTooltip: "Поиск",
        clearFilterButtonTooltip: "Очистить фильтрыr",
        insertButtonTooltip: "Добавить",
        updateButtonTooltip: "Обновить",
        cancelEditButtonTooltip: "Отмена",

        editButton: true,
        deleteButton: true,
        clearFilterButton: true,
        modeSwitchButton: true,

        _initConfig: function () {
            this._hasFiltering = this._grid.filtering;
            this._hasInserting = this._grid.inserting;

            if (this._hasInserting && this.modeSwitchButton) {
                this._grid.inserting = false;
            }

            this._configInitialized = true;
        },

        headerTemplate: function () {
            if (!this._configInitialized) {
                this._initConfig();
            }

            var hasFiltering = this._hasFiltering;
            var hasInserting = this._hasInserting;

            if (!this.modeSwitchButton || (!hasFiltering && !hasInserting))
                return "";

            if (hasFiltering && !hasInserting)
                return this._createFilterSwitchButton();

            if (hasInserting && !hasFiltering)
                return this._createInsertSwitchButton();

            return this._createModeSwitchButton();
        },

        itemTemplate: function (value, item) {
            var $result = $([]);

            if (this.editButton) {
                $result = $result.add(this._createEditButton(item));
            }

            if (this.deleteButton) {
                $result = $result.add(this._createDeleteButton(item));
            }

            return $result;
        },

        filterTemplate: function () {
            var $result = this._createSearchButton();
            return this.clearFilterButton ? $result.add(this._createClearFilterButton()) : $result;
        },

        insertTemplate: function () {
            return this._createInsertButton();
        },

        editTemplate: function () {
            return this._createUpdateButton().add(this._createCancelEditButton());
        },

        _createFilterSwitchButton: function () {
            return this._createOnOffSwitchButton("filtering", this.searchModeButtonClass, true);
        },

        _createInsertSwitchButton: function () {
            return this._createOnOffSwitchButton("inserting", this.insertModeButtonClass, false);
        },

        _createOnOffSwitchButton: function (option, cssClass, isOnInitially) {
            var isOn = isOnInitially;

            var updateButtonState = $.proxy(function () {
                $button.toggleClass(this.modeOnButtonClass, isOn);
            }, this);

            var $button = this._createGridButton(this.modeButtonClass + " " + cssClass, "", function (grid) {
                isOn = !isOn;
                grid.option(option, isOn);
                updateButtonState();
            });

            updateButtonState();

            return $button;
        },

        _createModeSwitchButton: function () {
            var isInserting = false;

            var updateButtonState = $.proxy(function () {
                $button.attr("title", isInserting ? this.searchModeButtonTooltip : this.insertModeButtonTooltip)
                    .toggleClass(this.insertModeButtonClass, !isInserting)
                    .toggleClass(this.searchModeButtonClass, isInserting);
            }, this);

            var $button = this._createGridButton(this.modeButtonClass, "", function (grid) {
                isInserting = !isInserting;
                grid.option("inserting", isInserting);
                grid.option("filtering", !isInserting);
                updateButtonState();
            });

            updateButtonState();

            return $button;
        },

        _createEditButton: function (item) {
            return this._createGridButton(this.editButtonClass, this.editButtonTooltip, function (grid, e) {
                grid.editItem(item);
                e.stopPropagation();
            });
        },

        _createDeleteButton: function (item) {
            return this._createGridButton(this.deleteButtonClass, this.deleteButtonTooltip, function (grid, e) {
                grid.deleteItem(item);
                e.stopPropagation();
            });
        },

        _createSearchButton: function () {
            return this._createGridButton(this.searchButtonClass, this.searchButtonTooltip, function (grid) {
                grid.search();
            });
        },

        _createClearFilterButton: function () {
            return this._createGridButton(this.clearFilterButtonClass, this.clearFilterButtonTooltip, function (grid) {
                grid.clearFilter();
            });
        },

        _createInsertButton: function () {
            return this._createGridButton(this.insertButtonClass, this.insertButtonTooltip, function (grid) {
                grid.insertItem().done(function () {
                    grid.clearInsert();
                });
            });
        },

        _createUpdateButton: function () {
            return this._createGridButton(this.updateButtonClass, this.updateButtonTooltip, function (grid, e) {
                grid.updateItem();
                e.stopPropagation();
            });
        },

        _createCancelEditButton: function () {
            return this._createGridButton(this.cancelEditButtonClass, this.cancelEditButtonTooltip, function (grid, e) {
                grid.cancelEdit();
                e.stopPropagation();
            });
        },

        _createGridButton: function (cls, tooltip, clickHandler) {
            var grid = this._grid;

            return $("<input>").addClass(this.buttonClass)
                .addClass(cls)
                .attr({
                    type: "button",
                    title: tooltip
                })
                .on("click", function (e) {
                    clickHandler(grid, e);
                });
        },

        editValue: function () {
            return "";
        }

    });

    GridPanel.fields.control = GridPanel.ControlField = ControlField;

}(GridPanel, jQuery));