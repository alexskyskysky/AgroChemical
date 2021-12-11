<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MeasureUC.ascx.cs" Inherits="agro_proba.Measure" %>

<link type="text/css" href="css/jquery-ui.structure.min.css" rel="stylesheet" />
<link type="text/css" href="css/jquery-ui.theme.min.css" rel="stylesheet" />

<link type="text/css" href="css/Measure.css" rel="stylesheet" />
<script type="text/javascript" src="js/ol.js"></script>
<script type="text/javascript" src="js/Measure.js"></script>

<div id="MeasurePanel" class="ui-widget-content">
    <a href="#" id="ClearMeasure">
        <asp:Image ID="ClearMeasureI" ToolTip="Очистить" runat="server" ImageUrl="images/broom.png" />
    </a>
    <select id="typeMeasure">
          <option value="length" selected>Растояние</option>
          <option value="area">Площадь</option>
    </select>
    <label>
          <input type="checkbox" id="geodesicMeasure" checked="checked" />
          Геодезистские метрики
    </label>
    <a href="#" id="HideMeasure">
        <asp:Image ID="HideMeasureI" ToolTip="Закрыть" runat="server" ImageUrl="images/hide_measure.png" />
    </a>
</div>