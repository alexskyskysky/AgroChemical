<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OLGISMap.aspx.cs" Inherits="agro_proba.OLGISMap" EnableEventValidation="false" %>
<%@ Implements Interface="System.Web.UI.ICallbackEventHandler" %>
<%@ Register Src="~/EditStyleUC.ascx" TagPrefix="ucES" TagName="EditStyleUC" %>
<%@ Register Src="~/EditPanelUC.ascx" TagPrefix="ucEP" TagName="EditPanelUC" %>
<%@ Register Src="~/MeasureUC.ascx" TagPrefix="ucDM" TagName="MeasureUC" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link type="text/css" href="css/ol.css" rel="stylesheet" />
    <link type="text/css" href="css/jquery-ui.structure.min.css" rel="stylesheet" />
    <link type="text/css" href="css/jquery-ui.theme.min.css" rel="stylesheet" />
    <link type="text/css" href="css/loadingmask.css" rel="stylesheet" />
    <link type="text/css" href="css/OLGISMap.css" rel="stylesheet" />
    
    <script type="text/javascript" src="js/ol.js"></script>
    <script type="text/javascript" src="js/jquery-3.1.1.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script type="text/javascript" src="js/dropdownpanel-1.0.js"></script>
    <script type="text/javascript" src="js/loadingmask.js"></script>
    <script type="text/javascript" src="js/JSCallBack.js"></script>
    <!--<script type="text/javascript" src="js/proj4.js"></script>-->
    <script type="text/javascript" src="js/OLGISMap.js"></script>
</head>
<body onload="CreateXMLHttpRequest();">
    <form id="theForm" runat="server">
        <div id="MenuPanel" class="ui-widget-content">
            <table>
                <tr valign="middle">
                    <td>
                        <ul id="ControlMenu" class="main_menu">
                            <li>
                                <a href="#" class="button_a">Панель управления</a>
                                <div class="submenu">
                                    <table>
                                        <tr valign="middle">
                                            <td align="center">
                                                <a href="#" class="img_button_a" onclick="ShowHistoryBook();">
                                                    <asp:Image ID="ShowPlotInfoI" ToolTip="Книга истории полей" runat="server" ImageUrl="images/icon_book_up.png" />
                                                </a>
                                            </td>
                                            <td align="center">
                                                <a href="#" class="img_button_a" onclick="ShowReports();">
                                                    <asp:Image ID="ShowReportsI" ToolTip="Отчёты" runat="server" ImageUrl="images/icon_report_up.png" />
                                                </a>
                                            </td>
                                        </tr> 
                                        <tr valign="middle">
                                            <td align="center">
                                                <a href="#" class="img_button_a" onclick="ShowLegend();">
                                                    <asp:Image ID="ShowLegendI" ToolTip="Легенда" runat="server" ImageUrl="images/icon_legend_up.png" />
                                                </a>
                                            </td>
                                            <td align="center">
                                                <a href="#" class="img_button_a" onclick="ShowSoilSampleW();">
                                                    <asp:Image ID="ShowSoilSampleInfoI" ToolTip="Данные почвенного образца" runat="server" ImageUrl="images/icon_point_up.png" />
                                                </a>
                                            </td>
                                        </tr>
                                        <tr valign="middle">
                                            <td align="center">
                                                <a href="#" class="img_button_a" onclick="ShowTrackers();">
                                                    <asp:Image ID="ShowTrackersI" ToolTip="Трекеры" runat="server" ImageUrl="images/icon_satellite_up.png" />
                                                </a>
                                            </td>
                                            <td align="center">
                                                <a href="#" class="img_button_a" onclick="CenterMap();">
                                                    <asp:Image ID="CenterMapI" ToolTip="Центровка карты" runat="server" ImageUrl="images/icon_center_up.png" />
                                                </a>
                                            </td>
                                        </tr>
                                        <tr valign="middle">
                                            <td align="center">
                                                <a href="#" class="img_button_a" onclick="ShowMeasurePanel();">
                                                    <asp:Image ID="MeasureI" ToolTip="Измерения" runat="server" ImageUrl="images/icon_measure.png" />
                                                </a>
                                            </td>
                                            <td align="center">
                                                <a href="#" class="img_button_a" onclick="ShowPlantations();">
                                                    <asp:Image ID="PlantationsI" ToolTip="Многолетние насаждения" runat="server" ImageUrl="images/tree_up.png" />
                                                </a>
                                            </td>
                                        </tr>
                                        <tr valign="middle">
                                            <td align="center">
                                                <a href="#" class="img_button_a" onclick="UserExit();">
                                                    <asp:Image ID="ExitI" ToolTip="Выход" runat="server" ImageUrl="images/exit_up.png" />
                                                </a>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </li>
                        </ul>
                    </td>
                    <td>
                        <ul id="TerritoryMenu" class="main_menu">
                            <li>
                                <a href="#" class="button_a">Выбор территории</a>
                                <div class="submenu">
                                    <table>
                                        <tr>
                                            <td valign="middle">
                                                <asp:Label ID="Label35" runat="server" Text="Область: "></asp:Label>
                                            </td>
                                            <td valign="middle">
                                                <asp:DropDownList ID="TerritoryCB" runat="server" DataValueField="id_territory" DataTextField="title_territory" Width="300px" Height="31px"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                <asp:Label ID="Label28" runat="server" Text="Район: "></asp:Label>
                                            </td>
                                            <td valign="middle">
                                                <asp:DropDownList ID="RegionCB" runat="server" DataValueField="id_region" DataTextField="title_region" Width="300px" Height="31px"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                <asp:Label ID="Label29" runat="server" Text="Организация: "></asp:Label>
                                            </td>
                                            <td valign="middle">
                                                <asp:DropDownList ID="OrganizationCB" runat="server" DataValueField="id_organization" DataTextField="title_organization" Width="300px" Height="31px"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                <asp:Label ID="Label84" runat="server" Text="Тур обследования: "></asp:Label>
                                            </td>
                                            <td valign="middle">
                                                <asp:DropDownList ID="SurveyTourCB" runat="server" DataValueField="tour" DataTextField="tour" Width="300px" Height="31px"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                <asp:Label ID="Label83" runat="server" Text="Год обследования: "></asp:Label>
                                            </td>
                                            <td valign="middle">
                                                <asp:DropDownList ID="SurveyYearCB" runat="server" DataValueField="year" DataTextField="year" Width="300px" Height="31px"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                <asp:Label ID="Label85" runat="server" Text="Тип участков: "></asp:Label>
                                            </td>
                                            <td valign="middle">
                                                <asp:DropDownList ID="TypePlotCB" runat="server" DataValueField="id_type_plot" DataTextField="title_type_plot" Width="300px" Height="31px"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                <asp:Label ID="Label61" runat="server" Text="Поиск по уникальному номеру участка: "></asp:Label>
                                            </td>
                                            <td valign="middle">
                                                <asp:TextBox ID="UniqueNumberSearchTB" Width="300px" runat="server"></asp:TextBox>
                                            </td>
                                            <td valign="middle">
                                                <input type="button" id="UniqueNumberSearchB" runat="server" value=">"/>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </li>
                        </ul>
                    </td>
                    <td>
                        <ul id="ThemeMenu" class="main_menu">
                            <li>
                                <a href="#" class="button_a">Тема карты</a>
                                <div class="submenu">
                                    <table>
                                        <tr valign="middle">
                                            <td>
                                                <asp:DropDownList ID="MapThemeCB" runat="server" DataValueField="name_theme" DataTextField="title_theme" Width="250px" Height="31px"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr valign="middle">
                                            <td>
                                                <asp:CheckBox ID="AgrochemicalPointsCB" runat="server" Text=" Показывать точки" Font-Size="12" />
                                            </td>  
                                        </tr> 
                                    </table>
                                </div>
                            </li>
                        </ul>
                    </td>
                    <td>
                        <ul id="TileMapMenu" class="main_menu">
                            <li>
                                <a href="#" class="button_a">Слои карты</a>
                                <div class="submenu">
                                    <table width="200">
                                        <tr valign="middle">
                                            <td align="left">
                                                <asp:RadioButton ID="OSMLayerRB" runat="server" GroupName="BaseLayers" Text="Open Street Map" Checked="true" onChange="ChangeBaseMap();"/>
                                            </td>
                                        </tr>
                                        <tr valign="middle">
                                            <td align="left">
                                                <asp:RadioButton ID="BingRoadLayerRB" runat="server" GroupName="BaseLayers" Text="Bing схема" onChange="ChangeBaseMap();" />
                                            </td>
                                        </tr>
                                        <tr valign="middle">
                                            <td align="left">
                                                <asp:RadioButton ID="BingSatLayerRB" runat="server" GroupName="BaseLayers" Text="Bing спутник" onChange="ChangeBaseMap();" />
                                            </td>
                                        </tr>
                                        <tr valign="middle">
                                            <td align="left">
                                                <asp:RadioButton ID="BingHybLayerRB" runat="server" GroupName="BaseLayers" Text="Bing гибрид" onChange="ChangeBaseMap();" />
                                            </td>
                                        </tr>
                                        <!--<tr valign="middle">
                                            <td align="left">
                                                <asp:RadioButton ID="YandexSatRB" runat="server" GroupName="BaseLayers" Text="Яндекс спутник" onChange="ChangeBaseMap();" />
                                            </td>
                                        </tr>-->
                                        <tr valign="middle">
                                            <td align="left">
                                                <asp:CheckBox ID="RRKadastrCB" runat="server" Text=" Кадастровые контура" Font-Size="12" onChange="ShowHideKadastr();" />
                                            </td>  
                                        </tr>
                                        <tr valign="middle">
                                            <td align="left">
                                                <asp:CheckBox ID="FarmCB" runat="server" Text=" Животноводческие комплексы" Font-Size="12" onChange="ShowHideFarm();" />
                                            </td>  
                                        </tr>
                                        <tr valign="middle">
                                            <td align="left">
                                                <asp:CheckBox ID="DigCB" runat="server" Text=" Точки копания" Font-Size="12" onChange="ShowHideDig();" />
                                            </td>  
                                        </tr>
                                    </table>
                                </div>
                            </li>
                        </ul>
                    </td>
                </tr> 
            </table>              
        </div>
        <div id="Legend" title="Легенда">
            <asp:Panel ID="LegendPanel" runat="server" Width="100%" Height="100%" ScrollBars="Auto">
                <asp:Table ID="LegendTable" runat="server" Width="100%" >
                </asp:Table>
            </asp:Panel>
        </div>
        <div id="PopupW" title=""></div>
        <div id="HistoryBook" title="Книга истории полей">
            <div id="HistoryBookTabs">
                <ul>
                    <li><a href="#tabs-1">Общие данные</a></li>
                    <li><a href="#tabs-2">Севооборот</a></li>
                    <li><a href="#tabs-3">Химическая мелиорация</a></li>
                    <li id="DosesCT" style="display:none;" onclick="ClearDosesCalculations();"><a href="#tabs-4">Расчет доз мин. удобрений</a></li>
                </ul>
                <div id="tabs-1">
                    <div id="BasicTabs">
                        <ul>
                            <li><a href="#tabs-11">Паспорт поля</a></li>
                            <li><a href="#tabs-12">Агрохимическая хар-ка</a></li>
                            <li><a href="#tabs-13">Почвенно-эрозион. хар-ка</a></li>
                        </ul>
                        <div id="tabs-11">
                            <asp:Table ID="BasicTable" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell Width="150"><asp:Label ID="OrgL" runat="server" Text="Организация"></asp:Label></asp:TableCell>
                                    <asp:TableCell><asp:TextBox ID="OrgTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="150"><asp:Label ID="DepL" runat="server" Text="Отделение"></asp:Label></asp:TableCell>
                                    <asp:TableCell><asp:TextBox ID="DepTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="150"><asp:Label ID="UniqNumberL" runat="server" Text="Уник. номер"></asp:Label></asp:TableCell>
                                    <asp:TableCell><asp:TextBox ID="UniqNumberTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="150"><asp:Label ID="NumberPlotL" runat="server" Text="Номер участка"></asp:Label></asp:TableCell>
                                    <asp:TableCell><asp:TextBox ID="NumberPlotTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="150"><asp:Label ID="AreaL" runat="server" Text="Площадь, га"></asp:Label></asp:TableCell>
                                    <asp:TableCell><asp:TextBox ID="AreaTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="150"><asp:Label ID="TypePropertyL" runat="server" Text="Вид собственности"></asp:Label></asp:TableCell>
                                    <asp:TableCell>
                                        <asp:DropDownList ID="TypePropertyCB" CssClass="sprav-combobox" runat="server" Width="100%" DataValueField="id_type_property" DataTextField="title_type_property"></asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                        <div id="tabs-12">
                            <p align="center">
                                <asp:Label ID="YearL" runat="server" Text="Год обследования"></asp:Label>
                                <asp:TextBox ID="YearTB" runat="server" ReadOnly="true" Width="100"></asp:TextBox>
                            </p>
                            <div id="AgroParametersTabs">
                                <ul>
                                    <li><a href="#tabs-121">Основные показатели</a></li>
                                    <li><a href="#tabs-122">Дополнительные показатели</a></li>
                                </ul>
                                <div id="tabs-121">
                                    <asp:Table ID="Table1" runat="server" Width="100%">
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"></asp:TableCell>
                                            <asp:TableCell Width="50"></asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Center"><asp:Label ID="Label2" runat="server" Text="Группа по содержанию"></asp:Label></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="NL" runat="server" Text="Гидролизуемый азот, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="NTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Right"><asp:TextBox ID="GroupNTB" runat="server" ReadOnly="true" Width="90%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="P2O5L" runat="server" Text="Подвижный фосфор, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="P2O5TB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Right"><asp:TextBox ID="GroupP2O5TB" runat="server" ReadOnly="true" Width="90%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="K2OL" runat="server" Text="Подвижный калий, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="K2OTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Right"><asp:TextBox ID="GroupK2OTB" runat="server" ReadOnly="true" Width="90%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="pHL" runat="server" Text="Степень кислотности, pH"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="pHTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Right"><asp:TextBox ID="GrouppHTB" runat="server" ReadOnly="true" Width="90%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="HumusL" runat="server" Text="Органическое вещество, %"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="HumusTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Right"><asp:TextBox ID="GroupHumusTB" runat="server" ReadOnly="true" Width="90%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="HAL" runat="server" Text="Гидролитическая кислотность, ммоль / 100 гр. почвы"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="HATB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Right"><asp:TextBox ID="GroupHATB" runat="server" ReadOnly="true" Width="90%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="ACL" runat="server" Text="Ёмкость катионного обмена, ммоль / 100 гр. почвы"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="ACTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Right"><asp:TextBox ID="GroupACTB" runat="server" ReadOnly="true" Width="90%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="TABL" runat="server" Text="Сумма поглощённых оснований, ммоль / 100 гр. почвы"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="TABTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Right"><asp:TextBox ID="GroupTABTB" runat="server" ReadOnly="true" Width="90%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="BSL" runat="server" Text="Степень насыщенн. основан., %"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="BSTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Right"><asp:TextBox ID="GroupBSTB" runat="server" ReadOnly="true" Width="90%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                                <div id="tabs-122">
                                    <asp:Table ID="Table2" runat="server" Width="100%">
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="MGL" runat="server" Text="Обменный магний, ммоль / 100 г. почвы"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="MGTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell Width="10"></asp:TableCell>
                                            <asp:TableCell ColumnSpan="2" HorizontalAlign="Center"><asp:Label ID="Label13" runat="server" Text="Тяжёлые металлы"></asp:Label></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="CAL" runat="server" Text="Обменный кальций, ммоль / 100 г. почвы"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="CATB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell Width="10"></asp:TableCell>
                                            <asp:TableCell Width="250"><asp:Label ID="CUHML" runat="server" Text="Валовая медь, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="CUHMTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="SL" runat="server" Text="Подвижная сера, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="STB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell Width="10"></asp:TableCell>
                                            <asp:TableCell Width="250"><asp:Label ID="ZNHML" runat="server" Text="Валовый цинк, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="ZNHMTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="BL" runat="server" Text="Подвижный бор, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="BTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell Width="10"></asp:TableCell>
                                            <asp:TableCell Width="250"><asp:Label ID="CDHML" runat="server" Text="Валовый  кадмий, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="CDHMTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="MOL" runat="server" Text="Подвижный молибден, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="MOTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell Width="10"></asp:TableCell>
                                            <asp:TableCell Width="250"><asp:Label ID="PBHML" runat="server" Text="Валовый свинец, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="PBHMTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="CUL" runat="server" Text="Подвижная медь, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="CUTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell Width="10"></asp:TableCell>
                                            <asp:TableCell Width="250"><asp:Label ID="NIHML" runat="server" Text="Валовый никель, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="NIHMTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="ZNL" runat="server" Text="Подвижный цинк, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="ZNTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell Width="10"></asp:TableCell>
                                            <asp:TableCell Width="250"><asp:Label ID="HGHML" runat="server" Text="Валовая ртуть, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="HGHMTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="NAL" runat="server" Text="Обменный натрий, ммоль / 100 г. почвы"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="NATB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell Width="10"></asp:TableCell>
                                            <asp:TableCell Width="250"><asp:Label ID="ASHML" runat="server" Text="Валовый  мышьяк, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="ASHMTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="COL" runat="server" Text="Подвижный кобальт, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="COTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell Width="10"></asp:TableCell>
                                            <asp:TableCell Width="250"><asp:Label ID="MGHML" runat="server" Text="Валовый марганец, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="MGHMTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="MNL" runat="server" Text="Подвижный марганец, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="MNTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell Width="10"></asp:TableCell>
                                            <asp:TableCell Width="250"><asp:Label ID="CRHML" runat="server" Text="Валовый хром, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="CRHMTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="FEL" runat="server" Text="Подвижное железо, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="FETB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell Width="10"></asp:TableCell>
                                            <asp:TableCell Width="250"><asp:Label ID="FHML" runat="server" Text="Валовый фтор, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="FHMTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell Width="250"><asp:Label ID="ALL" runat="server" Text="Подвижный алюминий, мг/кг"></asp:Label></asp:TableCell>
                                            <asp:TableCell Width="50"><asp:TextBox ID="ALTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                            <asp:TableCell Width="10"></asp:TableCell>
                                            <asp:TableCell Width="250"></asp:TableCell>
                                            <asp:TableCell Width="50"></asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                            </div>
                        </div>
                        <div id="tabs-13">
                            <p align="center">
                                <asp:Label ID="Year2L" runat="server" Text="Год обследования"></asp:Label>
                                <asp:TextBox ID="Year2TB" runat="server" ReadOnly="true" Width="100"></asp:TextBox>
                            </p>
                            <asp:Table ID="Table3" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell Width="350"></asp:TableCell>
                                    <asp:TableCell Width="60" HorizontalAlign="Center"><asp:Label ID="Slope1L" runat="server" Text="< 1°"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="60" HorizontalAlign="Center"><asp:Label ID="Slope2L" runat="server" Text="1-3°"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="60" HorizontalAlign="Center"><asp:Label ID="Slope3L" runat="server" Text="3-5°"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="60" HorizontalAlign="Center"><asp:Label ID="Slope4L" runat="server" Text="5-7°"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="60" HorizontalAlign="Center"><asp:Label ID="Slope5L" runat="server" Text="> 7°"></asp:Label></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="350"><asp:Label ID="Label24" runat="server" Text="Распределение крутизны склона, га"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="60"><asp:TextBox ID="Slope1TB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="60"><asp:TextBox ID="Slope2TB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="60"><asp:TextBox ID="Slope3TB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="60"><asp:TextBox ID="Slope4TB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="60"><asp:TextBox ID="Slope5TB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Height="25">
                                    <asp:TableCell ColumnSpan="6"></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="350"><asp:Label ID="ExposureL" runat="server" Text="Преобладающая экспозиция склона"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="5"><asp:TextBox ID="ExposureTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="350"><asp:Label ID="ErosionL" runat="server" Text="Преобладающая степень эродированности"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="5"><asp:TextBox ID="ErosionTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="350"><asp:Label ID="TypeSoilL" runat="server" Text="Преобладающий тип почвы"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="5"><asp:TextBox ID="TypeSoilTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="350"><asp:Label ID="GradingL" runat="server" Text="Гранулометрический состав"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="5"><asp:TextBox ID="GradingTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                    </div>
                </div>
                <div id="tabs-2">
                    <p align="center">
                        <asp:Label ID="Year3L" runat="server" Text="Год урожая"></asp:Label>
                        <asp:DropDownList ID="Year3CB" runat="server" Width="100" DataValueField="year" DataTextField="year"></asp:DropDownList>
                    </p>
                    <div id="CropRotationTabs">
                        <ul>
                            <li><a href="#tabs-21">Культуры</a></li>
                            <li><a href="#tabs-22">Мин. удобрения</a></li>
                            <li><a href="#tabs-23">Орг. удобрения</a></li>
                            <li><a href="#tabs-24">Обработка почвы</a></li>
                            <li><a href="#tabs-25">Защита растений</a></li>
                            <li><a href="#tabs-26">Вредители</a></li>
                            <li><a href="#tabs-27">Болезни</a></li>
                            <li><a href="#tabs-28">Сорняки</a></li>
                            <li hidden="hidden"><a href="#tabs-29">Засоренность</a></li>
                        </ul>
                        <div id="tabs-21">
                            <asp:Table ID="Table4" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="NumberFieldL" runat="server" Text="Номер поля"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="4"><asp:TextBox ID="NumberFieldTB" runat="server" ReadOnly="true" Width="100%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="SortCRL" runat="server" Text="Тип севооборота"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="4">
                                        <asp:DropDownList ID="SortCRCB" CssClass="sprav-combobox" runat="server" Width="80%" DataValueField="id_sort_crop_rotation" DataTextField="title_sort_crop_rotation"></asp:DropDownList>
                                        <a id="SpravCropRotationB" class="ui-button-icon-only" style="height:25px;" href="#?" title=""></a>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="CultureL" runat="server" Text="Культура"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="4"><asp:DropDownList ID="CultureCB" CssClass="sprav-combobox" runat="server" Width="100%" DataValueField="id_culture" DataTextField="title_culture"></asp:DropDownList></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="Label36" runat="server" Text="Район"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="4"><asp:DropDownList ID="CultureZoneCB" CssClass="sprav-combobox" runat="server" Width="100%" DataValueField="id_loss_zone" DataTextField="title_loss_zone"></asp:DropDownList></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="OldCultureL" runat="server" Text="Предшественник"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="4"><asp:DropDownList ID="OldCultureCB" CssClass="sprav-combobox" runat="server" Width="100%" DataValueField="id_culture" DataTextField="title_culture"></asp:DropDownList></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="CrossCultureL" runat="server" Text="Сорт / гибрид"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="4"><asp:DropDownList ID="CrossCultureCB" CssClass="sprav-combobox" runat="server" Width="100%" DataValueField="id_culture" DataTextField="title_cross_culture"></asp:DropDownList></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="ReproductionL" runat="server" Text="Репродукция"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="4"><asp:TextBox ID="ReproductionTB" runat="server" Width="100%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="SeedingRateL" runat="server" Text="Норма высева, кг/га"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="4"><asp:TextBox ID="SeedingRateTB" runat="server" Width="100%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="PlannedProdL" runat="server" Text="Планируемая урожайность, т/га"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="PlannedProdTB" runat="server" Width="100%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"></asp:TableCell>
                                    <asp:TableCell Width="200"><asp:Label ID="SowingDateL" runat="server" Text="Дата посева"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="150"><asp:TextBox ID="SowingDateTB" ReadOnly="true" runat="server" Width="100%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="ActualProdL" runat="server" Text="Фактическая урожайность, т/га"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="ActualProdTB" runat="server" Width="100%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"></asp:TableCell>
                                    <asp:TableCell Width="200"><asp:Label ID="HarvestDateL" runat="server" Text="Дата уборки"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="150"><asp:TextBox ID="HarvestDateTB" ReadOnly="true" runat="server" Width="100%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                        <div id="tabs-22">
                            <asp:Table ID="Table5" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="Label11" runat="server" Text="Под планируемую культуру"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label6" runat="server" Text="Удобрение"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label3" runat="server" Text="Доза, кг/га физ.вес"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label7" runat="server" Text="Дата применения"></asp:Label></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="BasicFertL" runat="server" Text="Основное внесение"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="250"><asp:DropDownList ID="BasicFertCB" CssClass="sprav-combobox" runat="server" Width="95%" DataValueField="id_fertilizer" DataTextField="title_fertilizer"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="DoseBasicFertTB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="DateBasicFertTB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="SowingFertL" runat="server" Text="Припосевное внесение"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="250"><asp:DropDownList ID="SowingFertCB" CssClass="sprav-combobox" runat="server" Width="95%" DataValueField="id_fertilizer" DataTextField="title_fertilizer"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="DoseSowingFertTB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="DateSowingFertTB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="DressingFertL" runat="server" Text="Подкормка"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="250"><asp:DropDownList ID="DressingFertCB" CssClass="sprav-combobox" runat="server" Width="95%" DataValueField="id_fertilizer" DataTextField="title_fertilizer"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="DoseDressingFertTB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="DateDressingFertTB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell><asp:Label ID="TotalDoseL" runat="server" Text="Итого внесено в д.в., кг/га"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
                                        <asp:Label ID="TotalDoseNL" runat="server" Text="  N (Азот): "></asp:Label>
                                        <asp:TextBox ID="TotalDoseNTB" ReadOnly="true" runat="server" Width="50"></asp:TextBox>
                                        <asp:Label ID="TotalDosePL" runat="server" Text="  P (Фосфор): "></asp:Label>
                                        <asp:TextBox ID="TotalDosePTB" ReadOnly="true" runat="server" Width="50"></asp:TextBox>
                                        <asp:Label ID="TotalDoseKL" runat="server" Text="  K (Калий): "></asp:Label>
                                        <asp:TextBox ID="TotalDoseKTB" ReadOnly="true" runat="server" Width="50"></asp:TextBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="100%" ColumnSpan="4" Height="25"></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="Label8" runat="server" Text="Под предшествующую культуру"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label9" runat="server" Text="Удобрение"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label10" runat="server" Text="Доза, кг/га физ.вес"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label12" runat="server" Text="Дата применения"></asp:Label></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="OldBasicFertL" runat="server" Text="Основное внесение"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="250"><asp:DropDownList ID="OldBasicFertCB" CssClass="sprav-combobox" runat="server" Width="95%" DataValueField="id_fertilizer" DataTextField="title_fertilizer"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="OldDoseBasicFertTB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="OldDateBasicFertTB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="OldSowingFertL" runat="server" Text="Припосевное внесение"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="250"><asp:DropDownList ID="OldSowingFertCB" CssClass="sprav-combobox" runat="server" Width="95%" DataValueField="id_fertilizer" DataTextField="title_fertilizer"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="OldDoseSowingFertTB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="OldDateSowingFertTB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="OldDressingFertL" runat="server" Text="Подкормка"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="250"><asp:DropDownList ID="OldDressingFertCB" CssClass="sprav-combobox" runat="server" Width="95%" DataValueField="id_fertilizer" DataTextField="title_fertilizer"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="OldDoseDressingFertTB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="OldDateDressingFertTB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell><asp:Label ID="OldTotalDoseL" runat="server" Text="Итого внесено в д.в., кг/га"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
                                        <asp:Label ID="OldTotalDoseNL" runat="server" Text="  N (Азот): "></asp:Label>
                                        <asp:TextBox ID="OldTotalDoseNTB" ReadOnly="true" runat="server" Width="50"></asp:TextBox>
                                        <asp:Label ID="OldTotalDosePL" runat="server" Text="  P (Фосфор): "></asp:Label>
                                        <asp:TextBox ID="OldTotalDosePTB" ReadOnly="true" runat="server" Width="50"></asp:TextBox>
                                        <asp:Label ID="OldTotalDoseKL" runat="server" Text="  K (Калий): "></asp:Label>
                                        <asp:TextBox ID="OldTotalDoseKTB" ReadOnly="true" runat="server" Width="50"></asp:TextBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                        <div id="tabs-23">
                            <asp:Table ID="Table6" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell Width="250"></asp:TableCell>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label4" runat="server" Text="Удобрение"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label5" runat="server" Text="Доза, т/га физ.вес"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label14" runat="server" Text="Дата применения"></asp:Label></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="OrganicFertL" runat="server" Text="Под планируемую культуру"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="250"><asp:DropDownList ID="OrganicFertCB" CssClass="sprav-combobox" runat="server" Width="95%" DataValueField="id_fertilizer" DataTextField="title_fertilizer"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="DoseOrganicFertTB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="DateOrganicFertTB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell><asp:Label ID="OrganicTotalDoseL" runat="server" Text="Итого внесено в д.в., кг/га"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
                                        <asp:Label ID="OrganicTotalDoseNL" runat="server" Text="  N (Азот): "></asp:Label>
                                        <asp:TextBox ID="OrganicTotalDoseNTB" ReadOnly="true" runat="server" Width="50"></asp:TextBox>
                                        <asp:Label ID="OrganicTotalDosePL" runat="server" Text="  P (Фосфор): "></asp:Label>
                                        <asp:TextBox ID="OrganicTotalDosePTB" ReadOnly="true" runat="server" Width="50"></asp:TextBox>
                                        <asp:Label ID="OrganicTotalDoseKL" runat="server" Text="  K (Калий): "></asp:Label>
                                        <asp:TextBox ID="OrganicTotalDoseKTB" ReadOnly="true" runat="server" Width="50"></asp:TextBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="100%" ColumnSpan="4" Height="25"></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="OldOrganicFertL" runat="server" Text="Под предшествующую культуру"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="250"><asp:DropDownList ID="OldOrganicFertCB" CssClass="sprav-combobox" runat="server" Width="95%" DataValueField="id_fertilizer" DataTextField="title_fertilizer"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="OldDoseOrganicFertTB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="OldDateOrganicFertTB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell><asp:Label ID="OldOrganicTotalDoseL" runat="server" Text="Итого внесено в д.в., кг/га"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
                                        <asp:Label ID="OldOrganicTotalDoseNL" runat="server" Text="  N (Азот): "></asp:Label>
                                        <asp:TextBox ID="OldOrganicTotalDoseNTB" ReadOnly="true" runat="server" Width="50"></asp:TextBox>
                                        <asp:Label ID="OldOrganicTotalDosePL" runat="server" Text="  P (Фосфор): "></asp:Label>
                                        <asp:TextBox ID="OldOrganicTotalDosePTB" ReadOnly="true" runat="server" Width="50"></asp:TextBox>
                                        <asp:Label ID="OldOrganicTotalDoseKL" runat="server" Text="  K (Калий): "></asp:Label>
                                        <asp:TextBox ID="OldOrganicTotalDoseKTB" ReadOnly="true" runat="server" Width="50"></asp:TextBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="100%" ColumnSpan="4" Height="25"></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="100%" ColumnSpan="4" HorizontalAlign="Center"><asp:Label ID="Label18" runat="server" Text="Для жидких навозных стоков необходимо указать:"></asp:Label></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:Label ID="NumberProtocolL" runat="server" Text="Номер протокола испытания"></asp:Label></asp:TableCell>
                                    <asp:TableCell ColumnSpan="2">
                                        <asp:TextBox ID="IdProtocolTB" runat="server" hidden=""></asp:TextBox>
                                        <asp:TextBox ID="NumberProtocolTB" runat="server" Width="100%"></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <a id="select_protocol_B" href="#select_protocol" style="width: 24px; height: 24px; margin: 3px; padding: 0px; float: left;">
                                            <div class="img-button img-button-select-protocol"></div>
                                        </a>
                                        <a id="show_protocol_B" href="#show_protocol" style="width: 24px; height: 24px; margin: 3px; padding: 0px; float: left;">
                                            <div class="img-button img-button-show-protocol"></div>
                                        </a>
                                        <a id="delete_protocol_B" href="#delete_protocol" style="width: 24px; height: 24px; margin: 3px; padding: 0px; float: left;">
                                            <div class="img-button img-button-delete-protocol"></div>
                                        </a>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="4" HorizontalAlign="Left">
                                        <asp:Label ID="NContentL" runat="server" Text="  N (Азот), %: "></asp:Label>
                                        <asp:TextBox ID="NContentTB" ReadOnly="true" runat="server" Width="50"></asp:TextBox>
                                        <asp:Label ID="PContentL" runat="server" Text="  P (Фосфор), %: "></asp:Label>
                                        <asp:TextBox ID="PContentTB" ReadOnly="true" runat="server" Width="50"></asp:TextBox>
                                        <asp:Label ID="KContentL" runat="server" Text="  K (Калий), %: "></asp:Label>
                                        <asp:TextBox ID="KContentTB" ReadOnly="true" runat="server" Width="50"></asp:TextBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                        <div id="tabs-24">
                            <asp:Table ID="HeaderTillageT" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label1" runat="server" Text="Тип обработки"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label30" runat="server" Text="Глубина, см"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label15" runat="server" Text="Дата"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250"><asp:DropDownList ID="TillageCB" runat="server" Width="95%" DataValueField="id_type_tillage" DataTextField="title_type_tillage"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="DepthTillageTB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="DateTillageTB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><a id="add_tillage_B" href="#add_tillage">Добавить</a></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Height="5">
                                    <asp:TableCell ColumnSpan="4"></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="TillageT" runat="server" Width="100%">
                            </asp:Table>
                        </div>
                        <div id="tabs-25">
                            <asp:Table ID="HeaderPlantProtectionT" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label16" runat="server" Text="Тип препарата"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label19" runat="server" Text="Препарат"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="50" HorizontalAlign="Center"><asp:Label ID="Label20" runat="server" Text="Доза"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label17" runat="server" Text="Дата"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="100"><asp:DropDownList ID="TypeDrugCB" runat="server" Width="95%" DataValueField="id_type_drug" DataTextField="title_type_drug"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="250"><asp:DropDownList ID="DrugCB" runat="server" Width="95%" DataValueField="id_drug" DataTextField="title_drug"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="50"><asp:TextBox ID="DoseDrugTB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="DatePlantProtectionTB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><a id="add_drug_B" href="#add_drug">Добавить</a></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Height="5">
                                    <asp:TableCell ColumnSpan="5"></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="PlantProtectionT" runat="server" Width="100%">
                            </asp:Table>
                        </div>
                        <div id="tabs-26">
                            <asp:Table ID="HeaderPestsT" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell Width="150" HorizontalAlign="Center"><asp:Label ID="Label67" runat="server" Text="Фаза"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="150" HorizontalAlign="Center"><asp:Label ID="Label68" runat="server" Text="Вредитель"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="50" HorizontalAlign="Center"><asp:Label ID="Label69" runat="server" Text="Кол-во,<br>шт/кв.м"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label70" runat="server" Text="Дата"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="150"><asp:DropDownList ID="PhasePestsCB" runat="server" Width="95%" DataValueField="id_phase" DataTextField="title_phase"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="150"><asp:DropDownList ID="PestsCB" runat="server" Width="95%" DataValueField="id_pest" DataTextField="title_pest"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="50"><asp:TextBox ID="CountPestTB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="DatePestTB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><a id="add_pest_B" href="#add_pest">Добавить</a></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Height="5">
                                    <asp:TableCell ColumnSpan="5"></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="PestsT" runat="server" Width="100%">
                            </asp:Table>
                        </div>
                        <div id="tabs-27">
                            <asp:Table ID="HeaderDiseasesT" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell Width="150" HorizontalAlign="Center"><asp:Label ID="Label72" runat="server" Text="Фаза"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="150" HorizontalAlign="Center"><asp:Label ID="Label73" runat="server" Text="Болезнь"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="50" HorizontalAlign="Center"><asp:Label ID="Label74" runat="server" Text="">%</asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label75" runat="server" Text="Дата"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="150"><asp:DropDownList ID="PhaseDiseasesCB" runat="server" Width="95%" DataValueField="id_phase" DataTextField="title_phase"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="150"><asp:DropDownList ID="DiseasesCB" runat="server" Width="95%" DataValueField="id_disease" DataTextField="title_disease"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="50"><asp:TextBox ID="PercentDiseaseTB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="DateDiseaseTB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><a id="add_disease_B" href="#add_disease">Добавить</a></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Height="5">
                                    <asp:TableCell ColumnSpan="5"></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="DiseasesT" runat="server" Width="100%">
                            </asp:Table>
                        </div>
                        <div id="tabs-28">
                            <asp:Table ID="HeaderWeedsT" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell Width="150" HorizontalAlign="Center"><asp:Label ID="Label76" runat="server" Text="Фаза"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="150" HorizontalAlign="Center"><asp:Label ID="Label77" runat="server" Text="Сорняк"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="50" HorizontalAlign="Center"><asp:Label ID="Label81" runat="server" Text="Кол-во,<br>шт/кв.м"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label82" runat="server" Text="Дата"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="150"><asp:DropDownList ID="PhaseWeedsCB" runat="server" Width="95%" DataValueField="id_phase" DataTextField="title_phase"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="150"><asp:DropDownList ID="WeedsCB" runat="server" Width="95%" DataValueField="id_weed" DataTextField="title_weed"></asp:DropDownList></asp:TableCell>
                                    <asp:TableCell Width="50"><asp:TextBox ID="CountWeedTB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="DateWeedTB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><a id="add_weed_B" href="#add_weed">Добавить</a></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Height="5">
                                    <asp:TableCell ColumnSpan="5"></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="WeedsT" runat="server" Width="100%">
                            </asp:Table>
                        </div>
                        <div id="tabs-29" hidden="hidden">
                            <asp:Table ID="HeaderWeedinessT" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label71" runat="server" Text="Засоренность" /></asp:TableCell>
                                    <asp:TableCell Width="75" HorizontalAlign="Center"><asp:Label ID="Label78" runat="server" Text="Кол-во/кв.м" /></asp:TableCell>
                                    <asp:TableCell Width="75" HorizontalAlign="Center"><asp:Label ID="Label79" runat="server" Text="% заполн." /></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label80" runat="server" Text="Дата" /></asp:TableCell>
                                    <asp:TableCell Width="100"></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="100"><asp:TextBox ID="WeedinessTB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="75"><asp:TextBox ID="CountWeedinessTB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="75"><asp:TextBox ID="PercentWeedinessTB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="DateWeedinessTB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100" HorizontalAlign="Center"><a id="add_weediness_B" href="#add_weediness">Добавить</a></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Height="5">
                                    <asp:TableCell ColumnSpan="5"></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="WeedinessT" runat="server" Width="100%">
                            </asp:Table>
                        </div>
                    </div>
                </div>
                <div id="tabs-3">
                    <p align="center">
                        <asp:Label ID="Year4L" runat="server" Text="Год урожая"></asp:Label>
                        <asp:DropDownList ID="Year4CB" runat="server" Width="100" DataValueField="year" DataTextField="year"></asp:DropDownList>
                    </p>
                    <asp:Table ID="Table8" runat="server" Width="100%">
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="3"><asp:Label ID="PriorityCalcifL" runat="server" Text="Нуждаемость в известковании: "></asp:Label></asp:TableCell>
                            <asp:TableCell><asp:TextBox ID="PriorityCalcifTB" ReadOnly="true" runat="server" Width="100%"></asp:TextBox></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="3"><asp:Label ID="pH1L" runat="server" Text="Степень кислотности, pH: "></asp:Label></asp:TableCell>
                            <asp:TableCell><asp:TextBox ID="pH1TB" ReadOnly="true" runat="server" Width="100%"></asp:TextBox></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="3"><asp:Label ID="HA1L" runat="server" Text="Гидролитическая кислотность, ммоль/100 г почвы: "></asp:Label></asp:TableCell>
                            <asp:TableCell><asp:TextBox ID="HA1TB" ReadOnly="true" runat="server" Width="100%"></asp:TextBox></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow Height="25">
                            <asp:TableCell ColumnSpan="4"></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="4" HorizontalAlign="Center"><asp:Label ID="Label42" runat="server" Text="Фактически произвестковано:"></asp:Label></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Width="200" HorizontalAlign="Center"><asp:Label ID="Label31" runat="server" Text="Вид мелиоранта"></asp:Label></asp:TableCell>
                            <asp:TableCell Width="150" HorizontalAlign="Center"><asp:Label ID="Label32" runat="server" Text="Содержание CaCO3, %"></asp:Label></asp:TableCell>
                            <asp:TableCell Width="100" HorizontalAlign="Center"><asp:Label ID="Label33" runat="server" Text="Доза, т/га"></asp:Label></asp:TableCell>
                            <asp:TableCell Width="150" HorizontalAlign="Center"><asp:Label ID="Label34" runat="server" Text="Дата"></asp:Label></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Width="200"><asp:DropDownList ID="Ameliorator1CB" CssClass="sprav-combobox" runat="server" Width="95%" DataValueField="id_ameliorator" DataTextField="title_ameliorator"></asp:DropDownList></asp:TableCell>
                            <asp:TableCell Width="150"><asp:TextBox ID="PercentCaCO3_1TB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                            <asp:TableCell Width="100"><asp:TextBox ID="DoseAmeliorator1TB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                            <asp:TableCell Width="150"><asp:TextBox ID="DateAmeliorator1TB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Width="200"><asp:DropDownList ID="Ameliorator2CB" CssClass="sprav-combobox" runat="server" Width="95%" DataValueField="id_ameliorator" DataTextField="title_ameliorator"></asp:DropDownList></asp:TableCell>
                            <asp:TableCell Width="150"><asp:TextBox ID="PercentCaCO3_2TB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                            <asp:TableCell Width="100"><asp:TextBox ID="DoseAmeliorator2TB" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                            <asp:TableCell Width="150"><asp:TextBox ID="DateAmeliorator2TB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell><asp:Label ID="FactCaCO3L" runat="server" Text="Фактическая доза CaCO3, т/га: "></asp:Label></asp:TableCell>
                            <asp:TableCell></asp:TableCell>
                            <asp:TableCell><asp:TextBox ID="FactCaCO3TB" ReadOnly="true" runat="server" Width="95%"></asp:TextBox></asp:TableCell>
                            <asp:TableCell></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div id="tabs-4">
                    <p align="left">
                        <div id="DosesCalculationsW">
                            <ul>
                                <li><a href="#tabs-41">Дозы минеральных уравнений</a></li>
                                <li><a href="#tabs-42">Значения коэффициентов</a></li>
                            </ul>
                            <asp:Table ID="DosesCalculationsNFT" runat="server">
                                <asp:TableRow>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="DNL" runat="server" Text="Д(N) =" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="LossNL" runat="server" Text="В(N) *" Font-Size="Small" ToolTip=""></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="ProductivityNL" runat="server" Text="У *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K1NL" runat="server" Text="K1(N) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K2NL" runat="server" Text="K2(N) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K3NL" runat="server" Text="K3(N) -" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="DoNL" runat="server" Text="До(N) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K4NL" runat="server" Text="K4(N) -" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="DopNL" runat="server" Text="Доп(N) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K5NL" runat="server" Text="K5(N)" Font-Size="Small"></asp:Label></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="DosesCalculationsP2O5FT" runat="server">
                                <asp:TableRow>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="DP2O5L" runat="server" Text="Д(P) =" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="LossP2O5L" runat="server" Text="В(P) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="ProductivityP2O5L" runat="server" Text="У *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K1P2O5L" runat="server" Text="K1(N) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K3P2O5L" runat="server" Text="K3(N) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K7P2O5L" runat="server" Text="K7(N) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K8P2O5L" runat="server" Text="K8(N) -" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="DoP2O5L" runat="server" Text="До(P) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K4P2O5L" runat="server" Text="K4(N) -" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="DopP2O5L" runat="server" Text="Доп(P) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K5P2O5L" runat="server" Text="K5(P) -" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="DpP2O5L" runat="server" Text="Дп(P) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K6P2O5L" runat="server" Text="K6(P)" Font-Size="Small"></asp:Label></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="DosesCalculationsK2OFT" runat="server">
                                <asp:TableRow>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="DK2OL" runat="server" Text="Д(K) =" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="LossK2OL" runat="server" Text="В(K) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="ProductivityK2OL" runat="server" Text="У *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K1K2OL" runat="server" Text="K1(K) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K3K2OL" runat="server" Text="K3(K) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K7K2OL" runat="server" Text="K7(K) -" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="DoK2OL" runat="server" Text="До(K) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K4K2OL" runat="server" Text="K4(N) -" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="DopK2OL" runat="server" Text="Доп(K) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K5K2OL" runat="server" Text="K5(K) -" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="DpK2OL" runat="server" Text="Дп(K) *" Font-Size="Small"></asp:Label></asp:TableCell>
                                     <asp:TableCell HorizontalAlign="Left"><asp:Label ID="K6K2OL" runat="server" Text="K6(K)" Font-Size="Small"></asp:Label></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <div id="tabs-41">
                            <asp:Table ID="DosesCaltulationsT" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label39" runat="server" Text="" Font-Bold="true"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label40" runat="server" Text="N (Азот)" Font-Bold="true"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label41" runat="server" Text="P2O5 (Фосфор)" Font-Bold="true"></asp:Label></asp:TableCell>
                                   <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label43" runat="server" Text="K2O (Калий)" Font-Bold="true"></asp:Label></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label57" runat="server" Text="Годовые дозы минеральных удобрений, кг/га" ToolTip="Рассчитанные годовые дозы азотных/фосфорных/калийных удобрений "></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="D_NTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="D_P2O5TB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="D_K2OTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Height="5">
                                    <asp:TableCell ColumnSpan="4"></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            </div>
                            <div id="tabs-42">
                            <asp:Table ID="DosesCalculationsTC" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label37" runat="server" Text="" Font-Bold="true"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label38" runat="server" Text="N (Азот)" Font-Bold="true"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label44" runat="server" Text="P2O5 (Фосфор)" Font-Bold="true"></asp:Label></asp:TableCell>
                                   <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label45" runat="server" Text="K2O (Калий)" Font-Bold="true"></asp:Label></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label46" runat="server" Text="В, кг/т" ToolTip="Вынос N/Р2О5/K2O культурой из расчета на 1 т основной продукции с учетом побочной"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="Loss_nTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="Loss_p2o5TB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="Loss_k2oTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label55" runat="server" Text="У, т/га" ToolTip="Планируемая урожайность культуры"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="300"><asp:TextBox ID="Planned_productivityTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label56" runat="server" Text="До, т/га" ToolTip="Планируемая годовая доза органического удобрения под культуру"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="Do_nTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="Do_p2o5TB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="Do_k2oTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label58" runat="server" Text="Доп, т/га" ToolTip="Фактически внесенная доза органического удобрения под предшествующую культуру"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="Dop_nTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="Dop_p2o5TB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="Dop_k2oTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label59" runat="server" Text="Дп, кг/га" ToolTip="Фактическая доза минеральных удобрений, внесенных под предшествующую культуру"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="Dp_nTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="Dp_p2o5TB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="Dp_k2oTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label47" runat="server" Text="K1" ToolTip="Поправочный коэффициент к годовым дозам удобрений в зависимости от гранулометрического состава почвы поля"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K1_nTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K1_p2o5TB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K1_k2oTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label48" runat="server" Text="K2" ToolTip="Поправочный коэффициент к годовым дозам азотных удобрений в зависимости от предшественников"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K2_nTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K2_p2o5TB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K2_k2oTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label49" runat="server" Text="K3" ToolTip="Поправочный коэффициент к годовым дозам удобрений в зависимости от степени эродированности почвы"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K3_nTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K3_p2o5TB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K3_k2oTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label50" runat="server" Text="K4" ToolTip="Коэффициент использования азота/фосфора/калия из органического удобрения в первый год действия"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K4_nTB"   runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K4_p2o5TB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K4_k2oTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label51" runat="server" Text="K5" ToolTip="Коэффициент использования азота/фосфора/калия из органических удобрений, внесенных под предшествующую культуру (второго года действия) "></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K5_nTB"    runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K5_p2o5TB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K5_k2oTB"  runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label52" runat="server" Text="K6" ToolTip="Коэффициент использования азота/фосфора/калия из минеральных удобрений, внесенных под предшествующую культуру (второго года действия)"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K6_nTB"    runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K6_p2o5TB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K6_k2oTB"  runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label53" runat="server" Text="K7" ToolTip="Поправочный коэффициент к дозам фосфорных/калийных удобрений в зависимости от содержания в почве подвижного фосфора/калия"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K7_nTB"    runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K7_p2o5TB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K7_k2oTB"  runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label54" runat="server" Text="K8" ToolTip="Поправочный коэффициент к годовым дозам фосфорных удобрений в зависимости от степени кислотности почв"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K8_nTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K8_p2o5TB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="K8_k2oTB" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="250" HorizontalAlign="Center"><asp:Label ID="Label60" runat="server" Text="Д, кг/га" Font-Bold="true" ToolTip="Расчетная годовая доза удобрений на намечаемую хозяйством урожайность культуры"></asp:Label></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="D_NTBc" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="D_P2O5TBc" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                    <asp:TableCell Width="100"><asp:TextBox ID="D_K2OTBc" runat="server" Width="95%" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Height="5">
                                    <asp:TableCell ColumnSpan="4"></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            </div>
                        </div>
                    </p>
                </div>
            </div>
        </div>

        <div id="LoginW" title="Вход в систему">
            <asp:Table ID="Table7" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="3" Width="100">
                        <asp:Image runat="server" ID="LogoIm" ImageUrl="logo_cas.png" Height="63" Width="54" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label ID="Label26" runat="server" Text="GIS Агрохимик Online"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label ID="Label23" runat="server" Text="Версия: 0.4"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label ID="Label25" runat="server" Text="© ФГБУ &quot;ЦАС &quot;Белгородский&quot;"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label21" runat="server" Text="Логин"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>           
                        <asp:TextBox ID="LoginTB" runat="server" Width="100%"></asp:TextBox>      
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label22" runat="server" Text="Пароль"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="PasswordTB" type="password" runat="server" Width="100%"></asp:TextBox>  
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>

        <div id="SoilSampleW" title="Результаты химического анализа образца почвы" class="ui-widget">
            <p align="center">
                <asp:Label ID="SoilSampleYearL" runat="server" Text="Год отбора"></asp:Label>
                <asp:TextBox ID="SoilSampleYearTB" runat="server" ReadOnly="true" Width="100"></asp:TextBox>
            </p>
            <asp:Table ID="HeaderSoilSampleT" runat="server" Width="1530" class="ui-widget ui-widget-content">
                <asp:TableHeaderRow class="ui-widget-header">
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Visible="false">Id точки<br />копания</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Visible="false">Id расположения<br />точки копания</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Width="70">№ <br />точки <br />копания</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Width="75">Горизонт</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Width="75">Глубина, <br />см</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Width="55">Гумус, <br />%</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Width="45">P<sub>2</sub>O<sub>5</sub>, <br />мг/кг</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Width="40">K<sub>2</sub>O, <br />мг/кг</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Width="50">pH<sub>H<sub>2</sub>O</sub></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Width="50">pH<sub>HCl</sub>*</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Width="90">Hг, <br />ммоль/100 г</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Width="90">СПО, <br />ммоль/100 г</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Width="90">Ca <br />обмен., <br />ммоль/100 г</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Width="90">Mg <br />обмен., <br />ммоль/100 г</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Width="90">Na <br />обмен., <br />ммоль/100 г</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" ColumnSpan="6">% от абсолютно сухой почвы</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Width="80">Содерж-е <br />физич. <br />глины <br />(частиц <br />< 0,01 мм)</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" RowSpan="2" Width="105">Грануло-<br />метрический<br /> состав</asp:TableCell>
                </asp:TableHeaderRow>
                <asp:TableHeaderRow class="ui-widget-header ">
                    <asp:TableCell HorizontalAlign="Center" Width="75">Крупный <br />песок <br />1-0,25 мм</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" Width="70">Мелкий <br />песок <br />0,25-0,05 мм</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" Width="70">Крупная <br />пыль <br />0,05-0,01 мм</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" Width="80">Средняя <br />пыль <br />0,01-0,005 мм</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" Width="90">Мелкая <br />пыль <br />0,005-0,001 мм</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" Width="50">Ил <br />< 0,001 мм</asp:TableCell>
                </asp:TableHeaderRow>
            </asp:Table>
            <asp:Table ID="SoilSampleT" runat="server" Width="1530" class="ui-widget ui-widget-content">
            </asp:Table>
            <asp:Label ID="Label27" runat="server" Text="* - при pH > 6.8 используется метод Мачигина"></asp:Label>
        </div>

        <div id="ReportsW" title="Отчёты" class="ui-widget">
            <asp:Table ID="ReportsT" runat="server" Width="100%" class="ui-widget ui-widget-content">
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
                        Отчёты по агрохимическим характеристикам
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <a id="RegionReportB" href="#" class="report_button_a" onclick="ShowSelectTourW(2); $('#ReportsW').dialog('close');">
                            По району
                        </a>
                    </asp:TableCell>
                    <asp:TableCell>
                        <a id="OrganizationReportB" href="#" class="report_button_a" onclick="ShowSelectTourW(1); $('#ReportsW').dialog('close');">
                            По организации
                        </a>
                    </asp:TableCell>
                    <asp:TableCell>
                        
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <a id="RegionStatisticsReportB" href="#" class="report_button_a" onclick="RegionStatisticsReport();">
                            Статистика<br /> по району
                        </a>
                    </asp:TableCell>
                    <asp:TableCell>
                        <a id="OrganizationStatisticsReportB" href="#" class="report_button_a" onclick="OrgaizationStatisticsReport();">
                            Статистика<br /> по организации
                        </a>
                    </asp:TableCell>
                    <asp:TableCell>

                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
                        Отчёты по книге истории полей
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <a id="CulturesReportB" href="#" class="report_button_a" onclick="ShowSelectYearW(1); $('#ReportsW').dialog('close');">
                            Структура<br />посевных<br />площадей
                        </a>
                    </asp:TableCell>
                    <asp:TableCell>
                        <a id="CulturesProductivityReportB" href="#" class="report_button_a" onclick="ShowSelectYearCultureW(1); $('#ReportsW').dialog('close');">
                            Урожайность
                        </a>
                    </asp:TableCell>
                    <asp:TableCell>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <a id="CulturesRegionReportB" href="#" class="report_button_a" onclick="ShowSelectYearW(2); $('#ReportsW').dialog('close');">
                            Структура<br />посевных<br />площадей<br />(по району)
                        </a>
                    </asp:TableCell>
                    <asp:TableCell>
                        <a id="CulturesProductivityRegionReportB" href="#" class="report_button_a" onclick="ShowSelectYearCultureW(2); $('#ReportsW').dialog('close');">
                            Урожайность<br />(по району)
                        </a>
                    </asp:TableCell>
                    <asp:TableCell>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
                        Отчёты по выделенным участкам
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <a id="SelectedPlotsErosionB" href="#" class="report_button_a" onclick="ErosionChangeSelectedPlots();">
                            Изменение<br />эрозии<br />почвы
                        </a>
                    </asp:TableCell>
                    <asp:TableCell>
                        <a id="SelectedPlotsStatisticsB" href="#" class="report_button_a" onclick="MacroStatisticsForSelectedPlots();">
                            Статистика
                        </a>
                    </asp:TableCell>
                    <asp:TableCell>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
                        Прочие отчёты
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <a id="UsingWateringTypeFarmlandRegionReportB" href="#" class="report_button_a" onclick="ShowSelectUsingWateringTypeFarmlandTourW(2); $('#ReportsW').dialog('close');">
                            Тип с/х земель<br />Использование<br />Орошаемость<br />(по району)
                        </a>
                    </asp:TableCell>
                    <asp:TableCell>
                        <a id="OrganicFertilizerReportB" href="#" class="report_button_a" onclick="ShowSelectYearOrganicFertilizerW(1); $('#ReportsW').dialog('close');">
                            Использование<br />органических<br />удобрений<br />(по организации)
                        </a>
                    </asp:TableCell>
                    <asp:TableCell>
                        <a id="ErosionChangeReportB" href="#" class="report_button_a" onclick="ShowSelectTourErosionChangeW(1); $('#ReportsW').dialog('close');">
                            Изменение<br />эрозии<br />почвы<br />(по организации)
                        </a>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <a id="ReportPestsB" href="#" class="report_button_a" onclick="ShowSelectYearPestsDiseasesWeedinessW(1,0); $('#ReportsW').dialog('close');">
                            Отчёт по<br />вредителям<br />(по организации)
                        </a>
                    </asp:TableCell>
                    <asp:TableCell>
                        <a id="ReportDiseasesB" href="#" class="report_button_a" onclick="ShowSelectYearPestsDiseasesWeedinessW(1,1); $('#ReportsW').dialog('close');">
                            Отчёт по<br />болезням<br />(по организации)
                        </a>
                    </asp:TableCell>
                    <asp:TableCell>
                        <a id="ReportWeedinessB" href="#" class="report_button_a" onclick="ShowSelectYearPestsDiseasesWeedinessW(1,2); $('#ReportsW').dialog('close');">
                            Отчёт по<br />засоренности<br />(по организации)
                        </a>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <a id="ReportPlantationsB" href="#" class="report_button_a" onclick="ShowReportCultureHybridByPlantations(); $('#ReportsW').dialog('close');">
                            Отчёт по<br />многолетним<br />насаждениям
                        </a>
                    </asp:TableCell>
                    <asp:TableCell>
                    </asp:TableCell>
                    <asp:TableCell>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>

        <div id="GPSTrackersW" title="Трекеры" class="ui-widget">
            <table>
                <tr>
                    <td>Дата: </td>
                    <td>с </td>
                    <td><asp:TextBox ID="DateTrackPointsFromTB" runat="server" ReadOnly="true" Width="100"></asp:TextBox></td>
                    <td>по </td>
                    <td><asp:TextBox ID="DateTrackPointsToTB" runat="server" ReadOnly="true" Width="100"></asp:TextBox></td>
                    <td><button ID="RefreshTrack" runat="server" onclick="Refresh()">Обновить</button></td>
                </tr>
            </table>
            <asp:Table ID="GPSTrackersT" runat="server" Width="570" class="ui-widget ui-widget-content">
            </asp:Table>
        </div>

        <div id="TheReportW" title="Отчёт" class="ui-widget">
        </div>

        <div id="SelectTourW" title="Выберите цикл обследования" class="ui-widget">
            <asp:DropDownList ID="SelectTourCB" runat="server" Width="95%" DataValueField="tour" DataTextField="tour"></asp:DropDownList>
        </div>

        <div id="SelectTourErosionChangeW" title="Выберите тур обследования" class="ui-widget">
            <asp:DropDownList ID="SelectTourErosionChangeCB" runat="server" Width="95%" DataValueField="tour" DataTextField="tour"></asp:DropDownList>
        </div>

        <div id="EditStyleW" title="Настройки" class="ui-widget">
            <ucES:EditStyleUC runat="server" id="EditStyleUC" />
        </div>

        <div id="SelectYearW" title="Выберите год" class="ui-widget">
            <asp:DropDownList ID="SelectYearCB" runat="server" Width="95%" DataValueField="year" DataTextField="year"></asp:DropDownList>
        </div>

        <div id="SelectYearCultureW" title="Выберите год и культуру" class="ui-widget">
            <asp:DropDownList ID="SelectYear1CB" runat="server" Width="95%" DataValueField="year" DataTextField="year"></asp:DropDownList>
            <asp:DropDownList ID="SelectCultureCB" runat="server" Width="95%" DataValueField="id_culture" DataTextField="title_culture"></asp:DropDownList>
        </div>

        <div id="SelectUsingWateringTypeFarmlandTourW" title="Выберите цикл обследования" class="ui-widget">
            <asp:DropDownList ID="SelectTour1CB" runat="server" Width="95%" DataValueField="tour" DataTextField="tour"></asp:DropDownList>
        </div>

        <div id="SelectYearOrganicFertilizerW" title="Выберите год" class="ui-widget">
            <asp:DropDownList ID="SelectYear2CB" runat="server" Width="95%" DataValueField="year" DataTextField="year"></asp:DropDownList>
        </div>

        <div id="SelectYearPestsDiseasesWeedinessW" title="Выберите год" class="ui-widget">
            <asp:DropDownList ID="SelectYear3CB" runat="server" Width="95%" DataValueField="year" DataTextField="year"></asp:DropDownList>
        </div>

        <div id="SelectProtocolW" title="Выбор поставщика и протокола" class="ui-widget">
            <asp:Table ID="ProtocolT" runat="server" Width="100%" class="ui-widget ui-widget-content">
                <asp:TableRow>
                    <asp:TableCell><asp:Label ID="Label62" runat="server" Text="Район: "></asp:Label></asp:TableCell>
                    <asp:TableCell><asp:DropDownList ID="SelectProtocolRegionCB" runat="server" Width="95%" DataValueField="id_region" DataTextField="title_region"></asp:DropDownList></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell><asp:Label ID="Label63" runat="server" Text="Организация: "></asp:Label></asp:TableCell>
                    <asp:TableCell><asp:DropDownList ID="SelectProtocolFarmOrgCB" runat="server" Width="95%" DataValueField="id_farm_organization" DataTextField="title_farm_organization"></asp:DropDownList></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell><asp:Label ID="Label64" runat="server" Text="Площадка: "></asp:Label></asp:TableCell>
                    <asp:TableCell><asp:DropDownList ID="SelectProtocolFarmCB" runat="server" Width="95%" DataValueField="id_farm" DataTextField="title_farm"></asp:DropDownList></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell><asp:Label ID="Label65" runat="server" Text="Лагуна: "></asp:Label></asp:TableCell>
                    <asp:TableCell><asp:DropDownList ID="SelectProtocolLagoonCB" runat="server" Width="95%" DataValueField="id_lagoon" DataTextField="lagoon_number"></asp:DropDownList></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell><asp:Label ID="Label66" runat="server" Text="Протокол: "></asp:Label></asp:TableCell>
                    <asp:TableCell><asp:DropDownList ID="SelectProtocolCB" runat="server" Width="95%" DataValueField="id_protocol" DataTextField="number_protocol"></asp:DropDownList></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>

        <div id="PlantationsW" title="Многолетние насаждения" class="ui-widget">
        </div>

        <ucEP:EditPanelUC runat="server" id="EditPanelUC" />
        <ucDM:MeasureUC runat="server" id="MeasureUC" />
        <div id="map" class="map"><div id="popup"></div></div>
    </form>
</body>
</html>
