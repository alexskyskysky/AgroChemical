<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditPanelUC.ascx.cs" Inherits="agro_proba.EditPanelUC" %>

<link type="text/css" href="css/jquery-ui.structure.min.css" rel="stylesheet" />
<link type="text/css" href="css/jquery-ui.theme.min.css" rel="stylesheet" />

<link type="text/css" href="css/EditPanelUC.css" rel="stylesheet" />
<script type="text/javascript" src="js/ol.js"></script>
<script type="text/javascript" src="js/EditPanelUC.js"></script>

<div id="EditPanel" class="ui-widget-content" hidden="hidden">
    <label for="CursorRB" class="editpanelicons editpanelicons-cursor"></label>
    <input type="radio" name="operation" id="CursorRB">
    <label for="HandRB" class="editpanelicons editpanelicons-hand"></label>
    <input type="radio" name="operation" id="HandRB">
    <label for="EditingRB" class="editpanelicons editpanelicons-editing"></label>
    <input type="radio" name="operation" id="EditingRB">
    <label for="AddingRB" class="editpanelicons editpanelicons-additing"></label>
    <input type="radio" name="operation" id="AddingRB">
    <label for="PointRB" class="editpanelicons editpanelicons-point"></label>
    <input type="radio" name="object" id="PointRB">
    <label for="LineStringRB" class="editpanelicons editpanelicons-linestring"></label>
    <input type="radio" name="object" id="LineStringRB">
    <label for="PolygonRB" class="editpanelicons editpanelicons-polygon"></label>
    <input type="radio" name="object" id="PolygonRB">
    <span id="DeleteFeaturesB" class="ui-button ui-widget ui-corner-all ui-button-icon-only editpanelicons editpanelicons-delete"></span>
    <span id="SaveChangesB" class="ui-button ui-widget ui-corner-all ui-button-icon-only editpanelicons editpanelicons-save"></span>
    <span id="CancelChangesB" class="ui-button ui-widget ui-corner-all ui-button-icon-only editpanelicons editpanelicons-cancel"></span>
</div>