<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="agro_proba.index" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Агрохимик Online</title>
    <link rel="icon" href="icon.jpeg" type="image/jpeg" />
    <script type="text/javascript" src="functions.js"></script>
    <style>
        .x-grid-row-over .x-grid-cell-inner {
            font-weight : bold;
        }
        .red-note {
            color: red;
        }
        .blue-note {
            color: blue;
        }
        .green-note {
            color: green;
        }
        .blueviolet-note {
            color: blueviolet;
        }
        .lightseagreen-note {
            color: lightseagreen;
        }
        .darkorange-note {
            color: darkorange;
        }
        .darkslateblue-note {
            color: darkslateblue;
        }
        .maroon-note {
            color: maroon;
        }
        .font-label {
            font-size: x-small;
            color: darkslateblue;
        }
        .verticalText {
	        -moz-transform: rotate(270deg);
	        -webkit-transform: rotate(270deg);
	        -o-transform: rotate(270deg);       
        }
        .x-green-livesearch-match
        {
            font-weight: bold;
            background-color: green;
            color:white;
        }
        .x-btn-focusClass {
            border: solid 1px red;
        }
        .x-btn-default {
            border: solid 1px silver;
        }
        #unlicensed {
            visibility: hidden;
        }
        .x-grid3-td {
            margin: 0px 0px 0px 0px;
            padding: 0px 0px 0px 0px;
            vertical-align:middle;
            display:block;
        }
    </style>
</head>
<body>
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="MainSM" runat="server" AsyncPostBackTimeout="0"></asp:ScriptManager>
    <ext:ResourceManager ID="MainRM" runat="server" DisableViewState="true" AjaxTimeout="300000" />
            <ext:Window ID="SettingsW" runat="server" Icon="Cog" Title="Настройки" Width="800" Height="460"  BodyPadding="5" Modal="true" Hidden="true" AutoRender="false" Resizable="false" Layout="FitLayout">
                <Items>
                    <ext:TabPanel ID="SettingsTP" runat="server" Plain="true" >
                        <Items>
                            <ext:Panel ID="BaseParamP" runat="server" Frame="true" Layout="FormLayout" Title="Базовые параметры">
                                <Items> 
                                    <ext:TextField ID="ShortTitleTF" runat="server" FieldLabel="Сокращенное наименование" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="220" Width="500" />
                                    <ext:TextField ID="FullTitleTF" runat="server" FieldLabel="Полное наименование" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="220" Width="500" />
                                    <ext:ComboBox ID="DirectorCB" runat="server" FieldLabel="Директор" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="220" Width="500" />
                                    <ext:ComboBox ID="ChiefSoilMonitoringCB" runat="server" FieldLabel="Начальник отдела мониторинга почв" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="220" Width="500" />
                                    <ext:ComboBox ID="ChiefAnalitycCB" runat="server" FieldLabel="Начальник аналитического отдела" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="220" Width="500" />
                                    <ext:ComboBox ID="AgrochemistCB" runat="server" FieldLabel="Агрохимик" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="220" Width="500" />
                                    <ext:ComboBox ID="MapmakerCB" runat="server" FieldLabel="Картограф" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="220" Width="500" />
                                    <ext:TextField ID="ThisYearTF" runat="server" FieldLabel="Текущий год" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="220" Width="265" />
                                    <ext:FieldContainer runat="server" ID="PathArchiveFC" Layout="HBoxLayout" >
                                        <Items>
                                            <ext:TextField ID="PathArchiveTF" runat="server" FieldLabel="Путь к архиватору" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="220" Width="470" />  
                                            <ext:Component runat="server" Width="5" /> 
                                            <ext:Button ID="PathArchiveB" runat="server" Text="..."/>                                       
                                        </Items>
                                    </ext:FieldContainer>
                                    <ext:FieldContainer runat="server" ID="FolderArchivesFC" Layout="HBoxLayout" >
                                        <Items>
                                            <ext:TextField ID="FolderArchivesTF" runat="server" FieldLabel="Папка архивов" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="220" Width="470" />  
                                            <ext:Component runat="server" Width="5" /> 
                                            <ext:Button ID="FolderArchivesB" runat="server" Text="..."/>                                       
                                        </Items>
                                    </ext:FieldContainer>
                                    <ext:FieldContainer runat="server" ID="FolterReportsFC" Layout="HBoxLayout" >
                                        <Items>
                                            <ext:TextField ID="FolterReportsTF" runat="server" FieldLabel="Папка отчетов" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="220" Width="470" />  
                                            <ext:Component runat="server" Width="5" /> 
                                            <ext:Button ID="FolterReportsB" runat="server" Text="..."/>                                       
                                        </Items>
                                    </ext:FieldContainer>
                                </Items>                                          
                            </ext:Panel>
                            <ext:Panel ID="IntervalsAndEditP" runat="server" Frame="true" Layout="HBoxLayout" Title="Интервалы и редактируемые поля">
                                <Items>
                                    <ext:FieldContainer runat="server" ID="IntervalsLeftFC" Layout="VBoxLayout"  >
                                        <Items> 
                                            <ext:FieldContainer runat="server" ID="LeftLabelFC" Layout="HBoxLayout" FieldLabel=" " LabelWidth="60" LabelSeparator="" >
                                                <Items>
                                                    <ext:Label ID="MinLeftL" runat="server" Width="50" Margins="0 3 0 0" Text="Минимум" Cls="font-label" />
                                                    <ext:Component runat="server" Width="3" />
                                                    <ext:Label ID="MaxLeftL" runat="server" Width="50" Margins="0 0 3 0" Text="Максимум" Cls="font-label" />
                                                    <ext:Component runat="server" Width="3" />
                                                    <ext:Label ID="SignsLeftL" runat="server" Width="50" Margins="0 0 0 3" Text="Знаков" Cls="font-label" />
                                                </Items>
                                            </ext:FieldContainer>                                            
                                            <ext:FieldContainer runat="server" ID="P2O5FC" Layout="HBoxLayout" FieldLabel="P<sub>2</sub>O<sub>5</sub>" LabelWidth="60" LabelSeparator="" LabelCls="green-note" >
                                                <Items>
                                                    <ext:TextField ID="P2O5minTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="P2O5maxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="P2O5signsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="P2O5CB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="K2OFC" Layout="HBoxLayout" FieldLabel="K<sub>2</sub>O" LabelWidth="60" LabelSeparator="" LabelCls="green-note" >
                                                <Items>
                                                    <ext:TextField ID="K2OminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="K2OmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="K2OsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="K2OCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="PhFC" Layout="HBoxLayout" FieldLabel="pH" LabelWidth="60" LabelSeparator="" LabelCls="green-note" >
                                                <Items>
                                                    <ext:TextField ID="PhminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="PhmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="PhsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="PhCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="HgFC" Layout="HBoxLayout" FieldLabel="Hг" LabelWidth="60" LabelSeparator="" LabelCls="green-note" >
                                                <Items>
                                                    <ext:TextField ID="HgminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="HgmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="HgsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="HgCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="NFC" Layout="HBoxLayout" FieldLabel="N" LabelWidth="60" LabelSeparator="" LabelCls="lightseagreen-note" >
                                                <Items>
                                                    <ext:TextField ID="NminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="NmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="NsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="NCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="NO3FC" Layout="HBoxLayout" FieldLabel="NO<sub>3</sub>" LabelWidth="60" LabelSeparator="" LabelCls="lightseagreen-note" >
                                                <Items>
                                                    <ext:TextField ID="NO3minTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="NO3maxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="NO3signsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="NO3CB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="HumusFC" Layout="HBoxLayout" FieldLabel="Гумус" LabelWidth="60" LabelSeparator="" LabelCls="lightseagreen-note" >
                                                <Items>
                                                    <ext:TextField ID="HumusminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="HumusmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="HumussignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="HumusCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="CapacityFC" Layout="HBoxLayout" FieldLabel="Емк. погл." LabelWidth="60" LabelSeparator="" LabelCls="lightseagreen-note" >
                                                <Items>
                                                    <ext:TextField ID="CapacityminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="CapacitymaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="CapacitysignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="CapacityCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="TotalAbsorbedBaseFC" Layout="HBoxLayout" FieldLabel="С. п. осн." LabelWidth="60" LabelSeparator="" LabelCls="lightseagreen-note" >
                                                <Items>
                                                    <ext:TextField ID="TotalAbsorbedBaseminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="TotalAbsorbedBasemaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="TotalAbsorbedBasesignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="TotalAbsorbedBaseCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="BaseSaturationFC" Layout="HBoxLayout" FieldLabel="С. н. осн." LabelWidth="60" LabelSeparator="" LabelCls="lightseagreen-note" >
                                                <Items>
                                                    <ext:TextField ID="BaseSaturationminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="BaseSaturationmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="BaseSaturationsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="BaseSaturationCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="Cs137FC" Layout="HBoxLayout" FieldLabel="Cs-137" LabelWidth="60" LabelSeparator="" LabelCls="blueviolet-note" >
                                                <Items>
                                                    <ext:TextField ID="Cs137minTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="Cs137maxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="Cs137signsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="Cs137CB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="Sr90FC" Layout="HBoxLayout" FieldLabel="Sr-90" LabelWidth="60" LabelSeparator=""  LabelCls="blueviolet-note" >
                                                <Items>
                                                    <ext:TextField ID="Sr90minFC" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="Sr90maxFC" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="Sr90signsFC" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="Sr90CB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                        </Items>
                                    </ext:FieldContainer>  
                                    <ext:Component runat="server" Width="40" /> 
                                    <ext:FieldContainer runat="server" ID="IntervalsCenterFC" Layout="VBoxLayout" >
                                        <Items> 
                                            <ext:FieldContainer runat="server" ID="CenterLabelFC" Layout="HBoxLayout" FieldLabel=" " LabelWidth="40" LabelSeparator="" >
                                                <Items>
                                                    <ext:Label ID="MinCenterL" runat="server" Width="50" Margins="0 3 0 0" Text="Минимум" Cls="font-label" />
                                                    <ext:Component runat="server" Width="3" />
                                                    <ext:Label ID="MaxCenterL" runat="server" Width="50" Margins="0 0 3 0" Text="Максимум" Cls="font-label" />
                                                    <ext:Component runat="server" Width="3" />
                                                    <ext:Label ID="SignsCenterL" runat="server" Width="50" Margins="0 0 0 3" Text="Знаков" Cls="font-label" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="MnFC" Layout="HBoxLayout" FieldLabel="Mn" LabelWidth="40" LabelSeparator="" LabelCls="blue-note" >
                                                <Items>
                                                    <ext:TextField ID="MnminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="MnmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="MnsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="MnCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="SFC" Layout="HBoxLayout" FieldLabel="S" LabelWidth="40" LabelSeparator="" LabelCls="blue-note" >
                                                <Items>
                                                    <ext:TextField ID="SminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="SmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="SsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="SCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="CuFC" Layout="HBoxLayout" FieldLabel="Cu" LabelWidth="40" LabelSeparator="" LabelCls="blue-note" >
                                                <Items>
                                                    <ext:TextField ID="CuminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="CumaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="CusignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="CuCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="ZnFC" Layout="HBoxLayout" FieldLabel="Zn" LabelWidth="40" LabelSeparator="" LabelCls="blue-note" >
                                                <Items>
                                                    <ext:TextField ID="ZnminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="ZnmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="ZnsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="ZnCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="CoFC" Layout="HBoxLayout" FieldLabel="Co" LabelWidth="40" LabelSeparator="" LabelCls="blue-note" >
                                                <Items>
                                                    <ext:TextField ID="CominTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="ComaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="CosignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="CoCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>                                            
                                            <ext:FieldContainer runat="server" ID="AlFC" Layout="HBoxLayout" FieldLabel="Al" LabelWidth="40" LabelSeparator="" LabelCls="blue-note" >
                                                <Items>
                                                    <ext:TextField ID="AlminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="AlmaxTF" runat="server" Width="50"  />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="AlsignsTF" runat="server" Width="50"  />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="AlCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>                                            
                                            <ext:FieldContainer runat="server" ID="CaFC" Layout="HBoxLayout" FieldLabel="Ca" LabelWidth="40" LabelSeparator="" LabelCls="blue-note" >
                                                <Items>
                                                    <ext:TextField ID="CaminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="CamaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="CasignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="CaCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="MoFC" Layout="HBoxLayout" FieldLabel="Mo" LabelWidth="40" LabelSeparator="" LabelCls="blue-note" >
                                                <Items>
                                                    <ext:TextField ID="MominTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="MomaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="MosignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="MoCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="BFC" Layout="HBoxLayout" FieldLabel="B" LabelWidth="40" LabelSeparator="" LabelCls="blue-note" >
                                                <Items>
                                                    <ext:TextField ID="BminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="BmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="BsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="BCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="MgFC" Layout="HBoxLayout" FieldLabel="Mg" LabelWidth="40" LabelSeparator="" LabelCls="blue-note" >
                                                <Items>
                                                    <ext:TextField ID="MgminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="MgmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="MgsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="MgCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="NaFC" Layout="HBoxLayout" FieldLabel="Na" LabelWidth="40" LabelSeparator="" LabelCls="blue-note" >
                                                <Items>
                                                    <ext:TextField ID="NaminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="NamaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="NasignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="NaCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>                                            
                                            <ext:FieldContainer runat="server" ID="ExposureFC" Layout="HBoxLayout" FieldLabel="Эксп-я" LabelWidth="40" LabelSeparator=""  LabelCls="maroon-note" >
                                                <Items>
                                                    <ext:TextField ID="ExposureminFC" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="ExposuremaxFC" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="ExposuresignsFC" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="ExposureCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                        </Items>
                                    </ext:FieldContainer>  
                                    <ext:Component runat="server" Width="40" />  
                                    <ext:FieldContainer runat="server" ID="IntervalsRightFC" Layout="VBoxLayout" >
                                        <Items> 
                                            <ext:FieldContainer runat="server" ID="RightLabelFC" Layout="HBoxLayout" FieldLabel=" " LabelWidth="30" LabelSeparator="" >
                                                <Items>
                                                    <ext:Label ID="MinRightL" runat="server" Width="50" Margins="0 3 0 0" Text="Минимум" Cls="font-label" />
                                                    <ext:Component runat="server" Width="3" />
                                                    <ext:Label ID="MaxRightL" runat="server" Width="50" Margins="0 0 3 0" Text="Максимум" Cls="font-label" />
                                                    <ext:Component runat="server" Width="3" />
                                                    <ext:Label ID="SignsRiightL" runat="server" Width="50" Margins="0 0 0 3" Text="Знаков" Cls="font-label" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="CuHmFC" Layout="HBoxLayout" FieldLabel="Cu" LabelWidth="30" LabelSeparator="" LabelCls="red-note" >
                                                <Items>
                                                    <ext:TextField ID="CuHmminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="CuHmmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="CuHmsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="CuHmCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="ZnHmFC" Layout="HBoxLayout" FieldLabel="Zn" LabelWidth="30" LabelSeparator="" LabelCls="red-note" >
                                                <Items>
                                                    <ext:TextField ID="ZnHmminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="ZnHmmaxFC" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="ZnHmsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="ZnHmCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="CdHmFC" Layout="HBoxLayout" FieldLabel="Cd" LabelWidth="30" LabelSeparator="" LabelCls="red-note" >
                                                <Items>
                                                    <ext:TextField ID="CdHmminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="CdHmmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="CdHmsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="CdHmCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="PbHmFC" Layout="HBoxLayout" FieldLabel="Pb" LabelWidth="30" LabelSeparator="" LabelCls="red-note" >
                                                <Items>
                                                    <ext:TextField ID="PbHmminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="PbHmmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="PbHmsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="PbHmCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="NiHmFC" Layout="HBoxLayout" FieldLabel="Ni" LabelWidth="30" LabelSeparator="" LabelCls="red-note" >
                                                <Items>
                                                    <ext:TextField ID="NiHmminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="NiHmmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="NiHmsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="NiHmCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="HgHmFC" Layout="HBoxLayout" FieldLabel="Hg" LabelWidth="30" LabelSeparator="" LabelCls="red-note" >
                                                <Items>
                                                    <ext:TextField ID="HgHmminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="HgHmmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="HgHmsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="HgHmCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="FHmFC" Layout="HBoxLayout" FieldLabel="F" LabelWidth="30" LabelSeparator="" LabelCls="red-note" >
                                                <Items>
                                                    <ext:TextField ID="FHmminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="FHmmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="FHmsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="FHmCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="AsHmFC" Layout="HBoxLayout" FieldLabel="As" LabelWidth="30" LabelSeparator="" LabelCls="red-note" >
                                                <Items>
                                                    <ext:TextField ID="AsHmminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="AsHmmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="AsHmsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="AsHmCb" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="MgHmFC" Layout="HBoxLayout" FieldLabel="Mg" LabelWidth="30" LabelSeparator="" LabelCls="red-note" >
                                                <Items>
                                                    <ext:TextField ID="MgHmmminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="MgHmmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="MgHmsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="MgHmCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="CrHmFC" Layout="HBoxLayout" FieldLabel="Cr" LabelWidth="30" LabelSeparator="" LabelCls="red-note" >
                                                <Items>
                                                    <ext:TextField ID="CrHmminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="CrHmmaxTF" runat="server" Width="50"  />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="CrHmsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="CrHmCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="FeHmFC" Layout="HBoxLayout" FieldLabel="Fe" LabelWidth="30" LabelSeparator="" LabelCls="red-note" >
                                                <Items>
                                                    <ext:TextField ID="FeHmminTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="FeHmmaxTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="FeHmsignsTF" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="FeHmCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:FieldContainer runat="server" ID="SlopeFC" Layout="HBoxLayout" FieldLabel="Укл." LabelWidth="30" LabelSeparator=""  LabelCls="maroon-note">
                                                <Items>
                                                    <ext:TextField ID="SlopeminFC" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="SlopemaxFC" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:TextField ID="SlopesignsFC" runat="server" Width="50" />
                                                    <ext:Component runat="server" Width="5" />
                                                    <ext:Checkbox ID="SlopeCB" runat="server" Checked="true" />
                                                </Items>
                                            </ext:FieldContainer>
                                        </Items>
                                    </ext:FieldContainer>                                                          
                                </Items>                                          
                            </ext:Panel>                            
                        </Items>
                    </ext:TabPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="AcceptSettingsB" runat="server" Text="Подтвердить" Icon="Accept" />
                    <ext:Button ID="CancelSettingsB" runat="server" Text="Отменить" Icon="Cancel" />
                </Buttons>
            </ext:Window>
            <ext:Window ID="LoginW" runat="server" Title="Авторизация" Closable="false" Resizable="false" Height="201" Icon="Lock" Width="260" Modal="true" BodyPadding="5" Layout="VBoxLayout" ActiveIndex="0" ButtonAlign="Center">
                <Items>
                    <ext:FieldContainer runat="server" ID="LoginFC1" Layout="HBoxLayout" >
                        <Items>
                            <ext:Image runat="server" ID="LogoI" ImageUrl="logo_cas.png" Height="63" Width="54" /> 
                            <ext:Component ID="Component97" runat="server" Width="7" />                                                             
                            <ext:FieldContainer runat="server" ID="LoginFC3" Layout="VBoxLayout">
                                <Items>
                                    <ext:FieldContainer runat="server" ID="FieldContainer16" Layout="HBoxLayout">
                                        <Items> 
                                            <ext:Component ID="Component98" runat="server" Width="35" />   
                                            <ext:Label ID="NameL" runat="server" Text="Агрохимик Online" Height="20" />  
                                        </Items>
                                    </ext:FieldContainer>
                                    <ext:FieldContainer runat="server" ID="FieldContainer17" Layout="HBoxLayout">
                                        <Items> 
                                            <ext:Component ID="Component101" runat="server" Width="50" />
                                            <ext:Label ID="VersionL" runat="server" Text="Версия: 1.0" Height="20" />
                                        </Items>
                                    </ext:FieldContainer>
                                    <ext:FieldContainer runat="server" ID="FieldContainer18" Layout="HBoxLayout">
                                        <Items>
                                            <ext:Label ID="AutorL" runat="server" Text='© ФГБУ "ЦАС "Белгородский"' Height="20" />
                                        </Items>
                                    </ext:FieldContainer>
                                </Items>
                            </ext:FieldContainer>
                        </Items>
                    </ext:FieldContainer>
                    <ext:TextField ID="UsernameTF" runat="server" FieldLabel="Логин" AllowBlank="false" EnableKeyEvents="true" LabelWidth="50" Width="237" />
                    <ext:TextField ID="UserPassword" runat="server" InputType="Password" FieldLabel="Пароль" AllowBlank="false" EnableKeyEvents="true" LabelWidth="50" Width="237" />
                </Items>
                <Buttons>
                    <ext:Button ID="ConnectionB" runat="server" MaxWidth="23" Icon="Connect" Flat="true" />
                    <ext:Button ID="AcceptLoginB" runat="server" Text="Вход" Icon="Accept" />
                </Buttons>
            </ext:Window>
            <ext:Window ID="AccessCodeW" runat="server" Title="Введите код доступа!" Resizable="false" Height="120" Icon="Lock" Width="275" Modal="true" BodyPadding="5" Layout="Form" ButtonAlign="Center" Hidden="true" Closable="False">
                <Items>
                    <ext:TextField ID="AccessCodeTF" runat="server" InputType="Password" FieldLabel="Код доступа" LabelSeparator=" " Width="250" LabelWidth="80" LabelCls="darkslateblue-note" />
                    <ext:Label ID="AccessCodeL" runat="server" Text="Введен неверный код!!!" Cls="red-note" Hidden="true" />
                </Items>
                <Buttons> 
                    <ext:Button ID="AcceptAccessCodeB" runat="server" Text="Подтвердить" Icon="Accept" />
                    <ext:Button ID="CancelAccessCodeB" runat="server" Text="Отмена" Icon="Cancel" />
                </Buttons>
            </ext:Window>
            <ext:Window ID="ConnectionW" runat="server" Title="Настройки подключения к БД" Closable="false" Resizable="false" Height="198" Icon="Connect" Width="275" Modal="true" BodyPadding="5" Layout="Form" ButtonAlign="Center" Hidden="true">
                <Items>
                    <ext:TextField ID="ServerdbTF" runat="server" FieldLabel="Сервер БД" LabelSeparator=" " Width="250" LabelWidth="80" LabelCls="darkslateblue-note" />
                    <ext:TextField ID="DbTF" runat="server" FieldLabel="База данных" LabelSeparator=" " Width="250" LabelWidth="80" LabelCls="darkslateblue-note" />
                    <ext:TextField ID="LoginTF" runat="server" FieldLabel="Логин" LabelSeparator=" " Width="250" LabelWidth="80" LabelCls="darkslateblue-note" />
                    <ext:TextField ID="PasswordTF" runat="server" InputType="Password" FieldLabel="Пароль" LabelSeparator=" " Width="250" LabelWidth="80" LabelCls="darkslateblue-note"/>
                    <ext:TextField ID="EditAccessCodeTF" runat="server" InputType="Password" FieldLabel="Код доступа" LabelSeparator=" " Width="250" LabelWidth="80" LabelCls="darkslateblue-note" />
                </Items>
                <Buttons> 
                    <ext:Button ID="AcceptConnectionB" runat="server" Text="Подтвердить" Icon="Accept" />
                    <ext:Button ID="CancelConnectionB" runat="server" Text="Отмена" Icon="Cancel" />
                </Buttons>
            </ext:Window>
            <ext:Window ID="InformationW" runat="server" Title="О программе" Closable="true" Resizable="false" Height="120" Icon="Information" Width="342" Modal="false" BodyPadding="5" Layout="VBoxLayout"  Hidden="true">
                    <Items>
                        <ext:FieldContainer runat="server" ID="FieldContainer19" Layout="HBoxLayout" >
                            <Items>
                                <ext:Image runat="server" ID="Image1" ImageUrl="logo_cas.png" Height="70" Width="60" /> 
                                <ext:Component ID="Component99" runat="server" Width="7" />                                                             
                                <ext:FieldContainer runat="server" ID="FieldContainer20" Layout="VBoxLayout">
                                    <Items>
                                        <ext:FieldContainer runat="server" ID="FieldContainer21" Layout="HBoxLayout">
                                            <Items> 
                                                <ext:Component ID="Component100" runat="server" Width="38" />   
                                                <ext:Label ID="Label1" runat="server" Text="Агрохимик Online, Версия: 1.0" Height="20" />  
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" ID="FieldContainer23" Layout="HBoxLayout">
                                            <Items>
                                                <ext:Component ID="Component102" runat="server" Width="1" /> 
                                                <ext:Label ID="Label3" runat="server" Text="© ФГБУ «ЦАС «Белгородский», 2013-2015" Height="20" />
                                            </Items>
                                        </ext:FieldContainer>
                                        <ext:FieldContainer runat="server" ID="FieldContainer24" Layout="HBoxLayout">
                                            <Items>
                                                <ext:Label ID="Label4" runat="server" Text="Разработчики: Костин И.Г., Чернявских Е.С." Height="20" />
                                            </Items>
                                        </ext:FieldContainer>
                                    </Items>
                                </ext:FieldContainer>
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:Window>

        <ext:Viewport ID="IndexViewport" runat="server" Layout="BorderLayout" Hidden="false">
        <Items>
            <ext:Panel runat="server" Region="North" Height="26" ID="IndexToolP">
               <TopBar>
                <ext:Toolbar ID="IndexToolbar" runat="server">
                    <Items>                        
                        <ext:Button ID="ReportsB" runat="server" Text="Отчеты" Icon="Report">
                            <Menu>
                                <ext:Menu ID="ReportsM" runat="server">
                                    <Items>
                                        <ext:MenuItem ID="ReportsM1" runat="server" Text="По отделению" >
                                            <Menu>
                                                <ext:Menu ID="ReportsM1Items" runat="server">
                                                    <Items>
                                                        <ext:MenuItem ID="ReportsM1Item1" runat="server" Text="Паспортная ведомость" />                                                        
                                                        <ext:MenuItem ID="ReportsM1Item2" runat="server" Text="Очередность известкования кислых почв" />
                                                        <ext:MenuItem ID="ReportsM1Item3" runat="server" Text="Группировка почв по типам сельхозугодий" />
                                                        <ext:MenuItem ID="ReportsM1Item4" runat="server" Text="Группировка групп обеспеченности элементами по уклонам" />
                                                        <ext:MenuItem ID="ReportsM1Item5" runat="server" Text="Группировка почв по типам сельхозугодий (с дополнительными группами)" />
                                                    </Items>
                                                </ext:Menu>
                                            </Menu>
                                        </ext:MenuItem>
                                        <ext:MenuItem ID="ReportsM2" runat="server" Text="По хозяйству">
                                            <Menu>
                                                <ext:Menu ID="ReportsM2Items" runat="server">
                                                    <Items>
                                                        <ext:MenuItem ID="ReportsM2Item1" runat="server" Text="Группировка почв по типам сельхозугодий" />
                                                        <ext:MenuItem ID="ReportsM2Item2" runat="server" Text="Группировка почв по типам сельхозугодий(ТМ)" />
                                                        <ext:MenuItem ID="ReportsM2Item3" runat="server" Text="Группировка групп обеспеченности элементами по уклонам" />
                                                        <ext:MenuItem ID="ReportsM2Item4" runat="server" Text="Очередность известкования кислых почв" />
                                                        <ext:MenuItem ID="ReportsM2Item5" runat="server" Text="Группировка почв по типам сельхозугодий (с дополнительными группами)" />
                                                    </Items>
                                                </ext:Menu>
                                            </Menu>
                                        </ext:MenuItem>
                                        <ext:MenuItem ID="ReportsM3" runat="server" Text="По району">
                                            <Menu>
                                                <ext:Menu ID="ReportsM3Items" runat="server">
                                                    <Items>
                                                        <ext:MenuItem ID="ReportsM3Item1" runat="server" Text="Группировка почв по типам сельхозугодий" />
                                                        <ext:MenuItem ID="ReportsM3Item2" runat="server" Text="Группировка почв по типам сельхозугодий(ТМ)" />
                                                        <ext:MenuItem ID="ReportsM3Item3" runat="server" Text="Группировка почв по типам сельхозугодий (с дополнительными группами)" />
                                                    </Items>
                                                </ext:Menu>
                                            </Menu>
                                        </ext:MenuItem>
                                        <ext:MenuItem ID="ReportsM4" runat="server" Text="По области">
                                            <Menu>
                                                <ext:Menu ID="ReportsM4Items" runat="server">
                                                    <Items>
                                                        <ext:MenuItem ID="ReportsM4Item1" runat="server" Text="Группировка почв по типам сельхозугодий" />
                                                        <ext:MenuItem ID="ReportsM4Item2" runat="server" Text="Группировка почв по типам сельхозугодий(ТМ)" />
                                                    </Items>
                                                </ext:Menu>
                                            </Menu>
                                        </ext:MenuItem>
                                        <ext:MenuItem ID="ReportsM5" runat="server" Text="По анализам">
                                            <Menu>
                                                <ext:Menu ID="ReportsM5Items" runat="server">
                                                    <Items>
                                                        <ext:MenuItem ID="ReportsM5Item1" runat="server" Text="По элементам" />
                                                        <ext:MenuItem ID="ReportsM5Item2" runat="server" Text="Общая ведомость" />
                                                    </Items>
                                                </ext:Menu>
                                            </Menu>
                                        </ext:MenuItem>
                                        <ext:MenuItem ID="ReportsM6" runat="server" Text="По планам">
                                            <Menu>
                                                <ext:Menu ID="ReportsM6Items" runat="server">
                                                    <Items>
                                                        <ext:MenuItem ID="ReportsM6Item1" runat="server" Text="План-задание" />
                                                    </Items>
                                                </ext:Menu>
                                            </Menu>
                                        </ext:MenuItem>
                                        <ext:MenuItem ID="ReportsM7" runat="server" Text="В Москву">
                                            <Menu>
                                                <ext:Menu ID="ReportsM7Items" runat="server">
                                                    <Items>
                                                        <ext:MenuItem ID="ReportsM7Item1" runat="server" Text="Отчёт с выбором циклов" />
                                                        <ext:MenuItem ID="ReportsM7Item2" runat="server" Text="Отчёт с выбором циклов (ТМ)" Hidden="true" />
                                                        <ext:MenuItem ID="ReportsM7Item3" runat="server" Text="Отчёт с выбором годов" />
                                                    </Items>
                                                </ext:Menu>
                                            </Menu>
                                        </ext:MenuItem>
                                    </Items>
                                </ext:Menu>
                            </Menu>
                        </ext:Button>                        
                        <ext:Button ID="GuidesB" runat="server" Text="Справочники" Icon="BookOpen">
                            <Menu>
                                <ext:Menu ID="GuidesM" runat="server">
                                    <Items>
                                        <ext:MenuItem ID="CommonGuidesM" runat="server" Text="Общие справочники">
                                            <Menu>
                                                <ext:Menu ID="CommonGuidesMI" runat="server">
                                                    <Items>
                                                        <ext:MenuItem ID="GuideSignificativeMI" runat="server" Text="Показатели" />
                                                        <ext:MenuItem ID="GuideSoilMI" runat="server" Text="Типы почв" />                                                               
                                                        <ext:MenuItem ID="GuideСultureMI" runat="server" Text="Культуры" />                                                                
                                                        <ext:MenuItem ID="GuideCropRotationMI" runat="server" Text="Типы севооборота" />
                                                        <ext:MenuItem ID="GuideFarmlandMI" runat="server" Text="Типы сельхозугодий" />
                                                        <ext:MenuItem ID="GuideErosionMI" runat="server" Text="Степени эродированности" />
                                                        <ext:MenuItem ID="GuideGradingMI" runat="server" Text="Механический состав почвы" />
                                                        <ext:MenuItem ID="GuideExposureMI" runat="server" Text="Экспозиция" />
                                                        <ext:MenuItem ID="GuideSlopeMI" runat="server" Text="Уклон" />
                                                    </Items>
                                                </ext:Menu>
                                            </Menu>
                                        </ext:MenuItem>                                                
                                        <ext:MenuItem ID="ExtraGuidesM" runat="server" Text="Дополнительные справочники">
                                            <Menu>
                                                <ext:Menu ID="ExtraGuidesMI" runat="server">
                                                    <Items>
                                                        <ext:MenuItem ID="GuideUserMI" runat="server" Text="Сотрудники" />
                                                        <ext:MenuItem ID="GuideJobTitleMI" runat="server" Text="Должности" />
                                                        <ext:MenuItem ID="GuideMissionMI" runat="server" Text="Задания" />
                                                        <ext:MenuItem ID="GuideTrackersMI" runat="server" Text="Трекеры" />
                                                        <ext:MenuItem ID="GuideCarsMI" runat="server" Text="Автомобили" />
                                                    </Items>
                                                </ext:Menu>
                                            </Menu>
                                        </ext:MenuItem> 
                                    </Items>
                                </ext:Menu>
                            </Menu>
                        </ext:Button>
                        <ext:Button ID="ImportDataB" runat="server" Text="Импорт данных" Icon="ArrowDown">
                            <Menu>
                                <ext:Menu ID="ImportDataM" runat="server">
                                    <Items>
                                        <ext:MenuItem ID="ImportSlopeExposureMI" runat="server" Text="Уклон и экспозиция"></ext:MenuItem>      
                                        <ext:MenuItem ID="ImportPointsMI" runat="server" Text="Точки"></ext:MenuItem>                              
                                    </Items>
                                </ext:Menu>
                            </Menu>
                        </ext:Button> 
                        <ext:Button ID="CopyMovePlotsB" runat="server" Text="Копирование / перемещение" Icon="ArrowSwitchBluegreen" />
                        <ext:Button ID="PlansB" runat="server" Text="План-задание" Icon="Book" />
                        <ext:Button ID="AnalysToPlotB" runat="server" Text="Синхронизация данных" Icon="ArrowJoin" />
                        <ext:Button ID="MapB" runat="server" Text="Карта" Icon="Map" HrefTarget="_blank" Href="OLGISMap.aspx" />
                        <ext:Button ID="AnalysisB" runat="server" Text="Анализы" Icon="AsteriskOrange">
                            <Menu>
                                <ext:Menu ID="AnalysisM" runat="server">
                                    <Items>
                                        <ext:MenuItem ID="AnalysisMI1" runat="server" Text="Категория1">
                                            <Menu>
                                                <ext:Menu ID="AnalysisMenu1" runat="server">
                                                    <Items>
                                                        <ext:MenuItem ID="AnalysPhMI" runat="server" Text="PH" /> 
                                                        <ext:MenuItem ID="AnalysPKMI" runat="server" Text="Фосфор, калий" />
                                                        <ext:MenuItem ID="AnalysHAMI" runat="server" Text="Hг" />
                                                    </Items>
                                                </ext:Menu>
                                            </Menu>
                                        </ext:MenuItem>                                                
                                        <ext:MenuItem ID="AnalysisMI2" runat="server" Text="Категория2">
                                            <Menu>
                                                <ext:Menu ID="Menu3" runat="server">
                                                    <Items>
                                                        <ext:MenuItem ID="AnalysHMMI" runat="server" Text="Тяжелые металлы" /> 
                                                        <ext:MenuItem ID="AnalysMicroMI" runat="server" Text="Микроэлементы" />
                                                    </Items>
                                                </ext:Menu>
                                            </Menu>
                                        </ext:MenuItem> 
                                    </Items>
                                </ext:Menu>
                            </Menu>
                        </ext:Button>
                        <ext:Button ID="ExecuteB" runat="server" Text="Выполнить" Icon="Accept">
                            <Menu>
                                <ext:Menu ID="ExecuteM" runat="server">
                                    <Items>
                                        <ext:MenuItem ID="ExecuteTeritoryMI" runat="server" Text="Добавление/обновление областей"></ext:MenuItem>
                                        <ext:MenuItem ID="ExecuteRegionsMI" runat="server" Text="Добавление/обновление районов"></ext:MenuItem>
                                        <ext:MenuItem ID="ExecuteOrganizationsMI" runat="server" Text="Добавление/обновление организаций"></ext:MenuItem>
                                        <ext:MenuItem ID="ExecutePlotsMI" runat="server" Text="Добавление/обновление участков"></ext:MenuItem>
                                        <ext:MenuItem ID="ExecuteUpdatePointsMI" runat="server" Text="Обновление а/х точек на карте"></ext:MenuItem>
                                        <ext:MenuItem ID="ExecuteSoilMI" runat="server" Text="Добавление/обновление почвенных карт"></ext:MenuItem>
                                        <ext:MenuItem ID="ExecuteAddSoilPointsMI" runat="server" Text="Добавление/обновление почвенных точек на карту"></ext:MenuItem>
                                        <ext:MenuItem ID="ExecuteSlopeMI" runat="server" Text="Добавление/обновление карт крутизны склонов"></ext:MenuItem>
                                        <ext:MenuItem ID="ExecuteExposureMI" runat="server" Text="Добавление/обновление карт экспозиции"></ext:MenuItem>
                                        <ext:MenuItem ID="ExecuteErosionMI" runat="server" Text="Добавление/обновление карт эрозии"></ext:MenuItem>
                                        <ext:MenuItem ID="ExecuteAddSoilSamplesMI" runat="server" Text="Добавление/обновление данных образцов почвы"></ext:MenuItem>
                                        <ext:MenuItem ID="ExecuteTypingMI" runat="server" Text="Добавление/обновление типизации почвы"></ext:MenuItem>
                                        <ext:MenuItem ID="ExecuteALSZMI" runat="server" Text="Добавление/обновление проекта АЛСЗ"></ext:MenuItem>
                                        <ext:MenuItem ID="ExecuteFarmsMI" runat="server" Text="Добавление/обновление животноводческих комплексов"></ext:MenuItem>
                                        <ext:MenuItem ID="ExecuteLagoonsMI" runat="server" Text="Добавление/обновление лагун/площадок/боксов"></ext:MenuItem>
                                    </Items>
                                </ext:Menu>
                            </Menu>
                        </ext:Button>
                        <ext:Button ID="SettingsB" runat="server" Icon="Cog" Text="Настройки" Hidden="true" />
                        <ext:Button ID="HelpB" runat="server" Text="Информация" Icon="Help" />
                        <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                        <ext:Label ID="UserNameL" runat="server" Text="..." Hidden="true" />
                        <ext:Button ID="ExitB" runat="server" Text="Выход" Icon="DoorOut" Hidden="true" />
                    </Items>
                </ext:Toolbar>
               </TopBar>                 
            </ext:Panel>
            <ext:Panel ID="IndexStatusP" runat="server" Height="26" Region="South">
                <Items>
                    <ext:StatusBar ID="IndexSB" runat="server" Height="25" DefaultText="Готово" >
                        <Items>
                            <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" AutoDataBind="true" Text="Дата:" />
                            <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" AutoDataBind="true" Text="<%# DateTime.Now %>" />
                        </Items>
                    </ext:StatusBar>
                </Items>
            </ext:Panel> 
            <ext:Panel ID="RegionP" runat="server" Region="West" Title="Районы" Width="325" MinWidth="175" MaxWidth="600" Split="true" Layout="FitLayout" AutoScroll="true">     
                <Items>
                    <ext:GridPanel ID="RegionGP" runat="server" Layout="FitLayout" AutoScroll="true" MultiSelect="false">
                        <SelectionModel><ext:RowSelectionModel ID="RegionRSM"></ext:RowSelectionModel></SelectionModel>
                        <TopBar>
                            <ext:Toolbar ID="RegionTB" runat="server">
                                <Items>
                                    <ext:Button ID="AddRegionB" runat="server" Icon="Add" Text="Добавить" /> 
                                    <ext:Button ID="EditRegionB" runat="server" Icon="Pencil" Text="Редактировать" />
                                    <ext:Button ID="StatisticsRegionB" runat="server" Icon="BookRed" Text="Статистика" />
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="RegionS" runat="server">
                                <Model>
                                    <ext:Model ID="RegionM" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="id_region"/>
                                            <ext:ModelField Name="code_region"/>
                                            <ext:ModelField Name="title_region"/>
                                            <ext:ModelField Name="okato_region"/>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column ID="Id_region" runat="server" DataIndex="id_region" Hidden="true" Text="Id района" />
                                <ext:Column ID="Code_region" runat="server" Text="Код района" DataIndex="code_region" Width="100" />
                                <ext:Column ID="Title_region" runat="server" Text="Наименование района" DataIndex="title_region" Width="140" />                                
                                <ext:Column ID="OKATO_region" runat="server" Text="ОКАТО" DataIndex="okato_region" Width="70" />
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>                    
                </Items>           
            </ext:Panel>
            <ext:Panel ID="OrganizationP" runat="server" Region="Center" Title="Хозяйства" MinWidth="175" Layout="FitLayout" AutoScroll="true">   
                <Items>
                    <ext:GridPanel ID="OrganizationGP" runat="server" Layout="FitLayout" AutoScroll="true" MultiSelect="false">
                        <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                        <TopBar>
                            <ext:Toolbar ID="OrganizationTB" runat="server">
                                <Items>
                                    <ext:Button ID="AddOrganizationB" runat="server" Icon="Add" Text="Добавить" />
                                    <ext:Button ID="EditOrganizationB" runat="server" Icon="Pencil" Text="Редактировать" />
                                    <ext:Button ID="DeleteOrganizationB" runat="server" Icon="Delete" Text="Удалить" />
                                    <ext:Button ID="EditdbB" runat="server" Icon="DatabaseEdit" Text="Ввод данных" />
                                    <ext:ToolbarFill />
                                    <ext:LiveSearchToolbar ID="OrganizationLSTB" runat="server" NextText=">>>" NextTooltipText="Следующая запись" PrevText="<<<" PrevTooltipText="Предыдущая запись"
                                        RegExpText="Регулярное выражение" CaseSensitiveText="Учитывать регистр" SearchText="Поиск:">
                                        <Items>
                                            <ext:Button ID="Button3" runat="server" Text="Обновить" Handler="var p = this.up('gridpanel').liveSearchPlugin; p.search(p.value);" />
                                        </Items>
                                    </ext:LiveSearchToolbar>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="OrganizationS" runat="server">
                                <Model>
                                    <ext:Model ID="OrganizationM" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="id_organization" />
                                            <ext:ModelField Name="id_region" />
                                            <ext:ModelField Name="code_region" />
                                            <ext:ModelField Name="code_organization" />
                                            <ext:ModelField Name="title_organization" />
                                            <ext:ModelField Name="full_title_organization" />
                                            <ext:ModelField Name="leader" />
                                            <ext:ModelField Name="basis_document" />
                                            <ext:ModelField Name="chief_agronomist" />
                                            <ext:ModelField Name="legal_address" />
                                            <ext:ModelField Name="mailing_address" />
                                            <ext:ModelField Name="email_organization" />
                                            <ext:ModelField Name="inn_organization" />
                                            <ext:ModelField Name="okato_organization" />                                     
                                            <ext:ModelField Name="kpp_organization" />
                                            <ext:ModelField Name="ogrn_organization" />
                                            <ext:ModelField Name="okved_organization" />
                                            <ext:ModelField Name="okpo_organization" />
                                            <ext:ModelField Name="oktmo_organization" />
                                            <ext:ModelField Name="pay_account" />
                                            <ext:ModelField Name="full_bank_name" />
                                            <ext:ModelField Name="bik" />
                                            <ext:ModelField Name="bank_correspond_account" />
                                            <ext:ModelField Name="id_old_title_organization" />
                                            <ext:ModelField Name="old_title_organization" />
                                            <ext:ModelField Name="id_phone" />
                                            <ext:ModelField Name="phone" />
                                            <ext:ModelField Name="max_tour" />
                                            <ext:ModelField Name="max_year" />
                                        </Fields>
                                    </ext:Model>
                                </Model>                                
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>            
                                <ext:Column ID="Id_organization" runat="server" DataIndex="id_organization" Hidden="true" Text="Id организации" />
                                <ext:Column ID="Id_region_organization" runat="server" DataIndex="id_region" Hidden="true" Text="Id района" />
                                <ext:Column ID="Code_region_organization" runat="server" DataIndex="code_region" Hidden="true" Text="Код района" />
                                <ext:Column ID="Code_organization" runat="server" Text="Код организации" DataIndex="code_organization" Width="120" />
                                <ext:Column ID="Title_organization" runat="server" Text="Наименование организации" DataIndex="title_organization" Width="200" /> 
                                <ext:Column ID="Full_title_organization" runat="server" Text="Полное наименование организации" DataIndex="full_title_organization" Hidden="true"/> 
                                <ext:Column ID="Leader_organization" runat="server" DataIndex="leader" Hidden="true" Text="Глава организации" />
                                <ext:Column ID="Basis_document" runat="server" DataIndex="basis_document" Hidden="true" Text="Действует на основании" />
                                <ext:Column ID="Chief_agronomist_org" runat="server" DataIndex="chief_agronomist" Hidden="true" Text="Гл. агроном организации" />
                                <ext:Column ID="Legal_address" runat="server" DataIndex="legal_address" Hidden="true" Text="Юридический адрес" />
                                <ext:Column ID="Mailing_address" runat="server" DataIndex="mailing_address" Hidden="true" Text="Почтовый адрес" />
                                <ext:Column ID="EMail_address" runat="server" DataIndex="email_organization" Hidden="true" Text="E-Mail" />
                                <ext:Column ID="Inn_organization" runat="server" Text="ИНН организации" DataIndex="inn_organization"  Width="120" />                                
                                <ext:Column ID="Okato_organization" runat="server" Text="ОКАТО" DataIndex="okato_organization" Width="140" />                                
                                <ext:Column ID="Kpp_organization" runat="server" DataIndex="kpp_organization" Hidden="true" Text="КПП" />
                                <ext:Column ID="Orgn_organization" runat="server" DataIndex="ogrn_organization" Hidden="true" Text="ОГРН" />
                                <ext:Column ID="Okved_organization" runat="server" DataIndex="okved_organization" Hidden="true" Text="ОКВЭД" />
                                <ext:Column ID="Okpo_organization" runat="server" DataIndex="okpo_organization" Hidden="true" Text="ОКПО" />
                                <ext:Column ID="Oktmo_organization" runat="server" DataIndex="oktmo_organization" Hidden="true" Text="ОКТМО" />
                                <ext:Column ID="Pay_account" runat="server" DataIndex="pay_account" Hidden="true" Text="Расчетный счет" />
                                <ext:Column ID="Full_bank_name" runat="server" DataIndex="full_bank_name" Hidden="true" Text="Полное наименование банка" />
                                <ext:Column ID="Bik" runat="server" DataIndex="bik" Hidden="true" Text="БИК" />
                                <ext:Column ID="Bank_correspond_account" runat="server" DataIndex="bank_correspond_account" Hidden="true" Text="Корреспондентский счет банка" />
                                
                                <ext:Column ID="Id_old_title_org" runat="server" DataIndex="id_old_title_organization" Hidden="true" Text="Id старого наименования" />
                                <ext:Column ID="Old_title_org" runat="server" DataIndex="old_title_organization" Text="Старое наименование организации" Width="200" />
                                <ext:Column ID="Id_phone_org" runat="server" DataIndex="id_phone" Hidden="true" Text="Id телефона" />
                                <ext:Column ID="Phone_org" runat="server" DataIndex="phone" Text="Телефон организации" Width="130" />
                                <ext:Column ID="Max_tour" runat="server" DataIndex="max_tour" Text="Цикл" Width="50" />
                                <ext:Column ID="Max_year" runat="server" DataIndex="max_year" Text="Год" Width="50" />
                            </Columns>            
                        </ColumnModel>
                        <Plugins>
                            <ext:LiveSearchGridPanel ID="OrganizationLS" runat="server" MatchCls="x-green-livesearch-match" />
                        </Plugins>
                    </ext:GridPanel>
                </Items>         
            </ext:Panel>
            <ext:Panel ID="DepartmentP" runat="server" Region="East" Split="true" Width="450" MinWidth="175" MaxWidth="700" Title="Отделения" Layout="VBoxLayout" AutoScroll="true">
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                </LayoutConfig>
                <Items>
                    <ext:GridPanel ID="DepartmentGP" runat="server" Layout="FitLayout" AutoScroll="true" MultiSelect="false" Flex="1">
                        <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                        <TopBar>
                            <ext:Toolbar ID="DepartmentTB" runat="server">
                                <Items>
                                    <ext:Button ID="AddDepartmentB" runat="server" Icon="Add" Text="Добавить" />
                                    <ext:Button ID="EditDepartmentB" runat="server" Icon="Pencil" Text="Редактировать" />
                                    <ext:Button ID="DeleteDepartmentB" runat="server" Icon="Delete" Text="Удалить"/>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="DepartmentS" runat="server">
                                <Model>
                                    <ext:Model ID="DepartmentM" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="id_department" />
                                            <ext:ModelField Name="id_organization" />
                                            <ext:ModelField Name="code_organization" />
                                            <ext:ModelField Name="code_department" />
                                            <ext:ModelField Name="title_department" />
                                            <ext:ModelField Name="tour" />
                                            <ext:ModelField Name="year" />
                                            <ext:ModelField Name="sum_area" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column ID="Id_department" runat="server" DataIndex="id_department" Hidden="true" Text="Id отделения" />   
                                <ext:Column ID="Id_organization_department" runat="server" DataIndex="id_organization" Hidden="true" Text="Id организации" />
                                <ext:Column ID="Code_organization_department" runat="server" DataIndex="code_organization" Hidden="true" Text="Код организации" />
                                <ext:Column ID="Code_department" runat="server" Text="Код отделения" DataIndex="code_department"  Width="100" />
                                <ext:Column ID="Title_department" runat="server" Text="Наименование отделения" DataIndex="title_department"  Width="160" />
                                <ext:Column ID="Tour_department" runat="server" Text="Цикл" DataIndex="tour"  Width="50" />
                                <ext:Column ID="Year_department" runat="server" Text="Год" DataIndex="year"  Width="50" />
                                <ext:Column ID="Area_department" runat="server" Text="Площадь" DataIndex="sum_area"  Width="70" />
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                    <ext:GridPanel ID="AboutOrgGP" runat="server" Layout="FitLayout" AutoScroll="true" MultiSelect="false" Flex="1" Title="По хозяйству">
                        <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                        <Store>
                            <ext:Store ID="AboutOrgS" runat="server">
                                <Model>
                                    <ext:Model ID="AboutOrgM" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="id_organization" />
                                            <ext:ModelField Name="tour" />
                                            <ext:ModelField Name="year" />
                                            <ext:ModelField Name="sum_area" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column ID="Id_organization_AboutOrg" runat="server" Text="ID организации" DataIndex="id_organization"  Width="50" Hidden="true" />
                                <ext:Column ID="Tour_AboutOrg" runat="server" Text="Цикл" DataIndex="tour"  Width="50" />
                                <ext:Column ID="Year_AboutOrg" runat="server" Text="Год" DataIndex="year"  Width="50" />
                                <ext:Column ID="Area_AboutOrg" runat="server" Text="Площадь" DataIndex="sum_area"  Width="70" />
                            </Columns>  
                        </ColumnModel>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
        </Items>
        </ext:Viewport>

            <ext:Window ID="AddEditRegionW" runat="server" Closable="false" Width="320" Height="146"  BodyPadding="5" Modal="true" Hidden="true" ButtonAlign="Center" Resizable="false">
                <Items>
                    <ext:TextField ID="CodeRegionTF" runat="server" FieldLabel="Код района" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="140" MaskRe="/[0-9]/" />
                    <ext:TextField ID="TitleRegionTF" runat="server" FieldLabel="Наименование района" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="140" />
                    <ext:TextField ID="OKATORegionTF" runat="server" FieldLabel="ОКАТО" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="140" MaskRe="/[0-9]/" />
                    <ext:TextField ID="IdRegionTF" runat="server" Hidden="true" />
                </Items>
                <Buttons>
                    <ext:Button ID="AcceptAddRegionB" runat="server" Text="Добавить" Icon="Disk" Hidden="true" />
                    <ext:Button ID="AcceptEditRegionB" runat="server" Text="Сохранить изменения" Icon="Disk" Hidden="true"/>
                    <ext:Button ID="CancelAddEditRegionB" runat="server" Text="Отменить" Icon="Cancel" />
                </Buttons>
            </ext:Window>
            <ext:Window ID="AddEditOrganizationW" runat="server" Closable="false" Width="465" Height="720"  BodyPadding="5" Modal="true" Hidden="true" ButtonAlign="Center" Resizable="false" Layout="FormLayout">
                <Items>
                    <ext:FieldContainer ID="FieldContainer22" runat="server" Layout="HBoxLayout">
                        <Items>
                            <ext:TextField ID="CodeRegionOrganizationTF" runat="server" FieldLabel="Код района" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="170" ReadOnly="true" />
                            <ext:Component ID="Component116" runat="server" Width="72" />
                            <ext:TextField ID="CodeOrganizationTF" runat="server" FieldLabel="Код организации" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="200" ReadOnly="true" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:TextField ID="TitleOrganizationTF" runat="server" FieldLabel="Наименование" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100"  Width="443" MaxLength="200" /> 
                    <ext:TextField ID="FullTitleOrganizationTF" runat="server" FieldLabel="Полное наименование" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100"  Width="443" MaxLength="200" /> 
                    <ext:TextField ID="LeaderTF" runat="server" FieldLabel="Руководитель" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                    <ext:TextField ID="BasisDocumentTF" runat="server" FieldLabel="Действует на основании" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                    <ext:TextField ID="ChiefAgronomistTF" runat="server" FieldLabel="Главный агроном" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                    <ext:TextField ID="LegalAddressTF" runat="server" FieldLabel="Юридический адрес" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                    <ext:TextField ID="MailingAddressTF" runat="server" FieldLabel="Почтовый адрес" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                    <ext:TextField ID="EMailOrganizationTF" runat="server" FieldLabel="E-Mail" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                    <ext:TextField ID="OKTMOOrganizationTF" runat="server" FieldLabel="ОКТМО" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                    <ext:FieldContainer ID="FieldContainer25" runat="server" Layout="HBoxLayout">
                        <Items>
                            <ext:TextField ID="OKATOOrganizationTF" runat="server" FieldLabel="ОКАТО" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="215" MinLength="8" MaxLength="14" MaskRe="/[0-9]/" />
                            <ext:Component ID="Component117" runat="server" Width="28" />
                            <ext:TextField ID="INNOrganizationTF" runat="server" FieldLabel="ИНН" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="50"  Width="200" MinLength="10" MaxLength="12" MaskRe="/[0-9]/" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer ID="FieldContainer26" runat="server" Layout="HBoxLayout">
                        <Items>
                            <ext:TextField ID="KPPOrganizationTF" runat="server" FieldLabel="КПП" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="215" MinLength="8" MaxLength="14" MaskRe="/[0-9]/" />
                            <ext:Component ID="Component118" runat="server" Width="28" />
                            <ext:TextField ID="OGRNOrganizationTF" runat="server" FieldLabel="ОГРН" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="50"  Width="200" MinLength="10" MaxLength="12" MaskRe="/[0-9]/" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer ID="FieldContainer27" runat="server" Layout="HBoxLayout">
                        <Items>
                            <ext:TextField ID="OKVEDOrganizationTF" runat="server" FieldLabel="ОКВЭД" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="215" MinLength="8" MaxLength="14" MaskRe="/[0-9]/" />
                            <ext:Component ID="Component119" runat="server" Width="28" />
                            <ext:TextField ID="OKPOOrganizationTF" runat="server" FieldLabel="ОКПО" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="50"  Width="200" MinLength="10" MaxLength="12" MaskRe="/[0-9]/" />
                        </Items>
                    </ext:FieldContainer>
                    
                    <ext:TextField ID="PayAccountTF" runat="server" FieldLabel="Расчетный счет" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                    <ext:TextField ID="FullBankNameTF" runat="server" FieldLabel="Полное наименование банка" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                    <ext:TextField ID="BIKTF" runat="server" FieldLabel="БИК" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                    <ext:TextField ID="BankCorrespondingAccountTF" runat="server" FieldLabel="Корресп. счет банка" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                    
                    <ext:TextField ID="IdOrganizationTF" runat="server" Hidden="true" />
                    <ext:Component ID="Component120" runat="server" Width="5" />
                    <ext:Container ID="Container1" runat="server" Layout="HBoxLayout">
                        <Items>
                            <ext:GridPanel ID="PhonesGP" runat="server" Flex="1" Layout="FitLayout" Height="120">
                                <TopBar>
                                    <ext:Toolbar ID="PhonesTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddPhoneB" runat="server" Icon="Add" Text="Добавить телефон"/>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="PhonesS" runat="server">
                                        <Model>
                                            <ext:Model ID="PhonesM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_phone" />
                                                    <ext:ModelField Name="id_organization" />
                                                    <ext:ModelField Name="phone" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="Id_phone" runat="server" DataIndex="id_phone" Hidden="true" />
                                        <ext:Column ID="Id_organization_phone" runat="server" DataIndex="id_organization" Hidden="true" />
                                        <ext:Column ID="Phone" runat="server" Text="Контактные телефоны" DataIndex="phone" Width="190">
                                            <Editor>
                                                <ext:TextField ID="PhoneTF" runat="server" MaskRe="/[0-9]/" />
                                            </Editor>
                                        </ext:Column>  
                                        <ext:ImageCommandColumn ID="DeletePhoneColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить телефон" CommandName="DeletePhone" />
                                            </Commands>
                                        </ext:ImageCommandColumn>                                 
                                    </Columns>            
                                </ColumnModel>  
                                <Plugins>
                                    <ext:CellEditing ID="CellEditing1" runat="server" ClicksToEdit="2" />
                                </Plugins>  
                            </ext:GridPanel>  
                            <ext:Component ID="Component121" runat="server" Width="5" />
                            <ext:GridPanel ID="OldOrganizationGP" runat="server" Flex="1" Layout="FitLayout" Height="120">
                                <TopBar>
                                    <ext:Toolbar ID="OldOrganizationTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddOldOrganizationB" runat="server" Icon="Add" Text="Добавить"/>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="OldOrganizationS" runat="server">
                                        <Model>
                                            <ext:Model ID="OldOrganizationM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_old_title_organization" />
                                                    <ext:ModelField Name="id_organization" />
                                                    <ext:ModelField Name="old_title_organization" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>  
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="Id_old_title_organization" runat="server" DataIndex="id_old_title_organization" Hidden="true" />
                                        <ext:Column ID="Id_old_organization" runat="server" DataIndex="id_organization" Hidden="true" />
                                        <ext:Column ID="OldTitleOrganization" runat="server" Text="Старые наименования" DataIndex="old_title_organization" Width="190">
                                            <Editor>
                                                <ext:TextField ID="OldOrganizationTF" runat="server" />
                                            </Editor>
                                        </ext:Column>  
                                        <ext:ImageCommandColumn ID="DeleteOldOrganizationColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить" CommandName="DeleteYear" />
                                            </Commands>
                                        </ext:ImageCommandColumn> 
                                    </Columns>            
                                </ColumnModel>  
                                <Plugins>
                                    <ext:CellEditing ID="CellEditing4" runat="server" ClicksToEdit="2" />
                                </Plugins>  
                            </ext:GridPanel>                               
                        </Items>
                    </ext:Container>
                </Items>
                <Buttons>
                    <ext:Button ID="AcceptAddOrganizationB" runat="server" Text="Добавить" Icon="Disk" Hidden="true" /> 
                    <ext:Button ID="AcceptEditOrganizationB" runat="server" Text="Сохранить изменения" Icon="Disk" Hidden="true" />    
                    <ext:Button ID="CancelAddEditOrganizationB" runat="server" Text="Отменить" Icon="Cancel" />                     
                </Buttons>
            </ext:Window>
            <ext:Window ID="AddEditDepartmentW" runat="server" Closable="false" Title="Новое отделение" Width="320" Height="150"  BodyPadding="5" Modal="true" Hidden="true" ButtonAlign="Center" Resizable="false">
                <Items>
                    <ext:TextField ID="CodeOrganizationDepartmentTF" runat="server" FieldLabel="Код организации" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="220" Width="300" ReadOnly="true" />
                    <ext:TextField ID="CodeDepartmentTF" runat="server" FieldLabel="Код отделения" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="220" Width="300" ReadOnly="true" />
                    <ext:TextField ID="TitleDepartmentTF" runat="server" FieldLabel="Наименование отделения" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="160" Width="300" />                
                    <ext:TextField ID="IdDepartmentTF" runat="server" Hidden="true" />
                </Items>
                <Buttons>
                    <ext:Button ID="AcceptAddDepartmentB" runat="server" Text="Добавить" Icon="Disk" Hidden="true" /> 
                    <ext:Button ID="AcceptEditDepartmentB" runat="server" Text="Сохранить изменения" Icon="Disk" Hidden="true" />
                    <ext:Button ID="CancelAddEditDepartmentB" runat="server" Text="Отменить" Icon="Cancel" />               
                </Buttons>
            </ext:Window>
            <ext:Window ID="EditdbW" runat="server" Icon="DatabaseEdit" Title="Ввод данных" Width="1000" Height="800" Modal="true" Hidden="true" Maximizable="true" Layout="BorderLayout" Resizable="false" ActiveIndex="0"> 
                <Items>   
                    <ext:Panel ID="EditdbTopP" runat="server" Header="false" Layout="HBoxLayout" Height="140" Frame="true" Region="North" Split="true">
                        <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                        </LayoutConfig>
                       <Items>
                            <ext:FieldSet ID="FieldSet1" runat="server" Flex="3" Layout="FormLayout">
                                <Items>
                                    <ext:TextField ID="EditdbRegionTF" runat="server" FieldLabel="Район" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="70" ReadOnly="true" />
                                    <ext:Component runat="server" Width="5" />
                                    <ext:TextField ID="EditdbOrganizationTF" runat="server" FieldLabel="Хозяйство" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="70" ReadOnly="true" />
                                    <ext:Component runat="server" Width="5" />
                                    <ext:ComboBox ID="EditdbDepartmentCB" runat="server" FieldLabel="Отделение" LabelCls="darkslateblue-note" LabelWidth="70" Editable="false" DisplayField="title_department" ValueField="id_department" >
                                        <Store>
                                            <ext:Store ID="EditdbDepartmentS" runat="server">
                                                <Model>
                                                    <ext:Model ID="EditdbDepartmentM" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="id_department" />
                                                            <ext:ModelField Name="id_organization" />
                                                            <ext:ModelField Name="code_organization" />
                                                            <ext:ModelField Name="code_department" />
                                                            <ext:ModelField Name="title_department" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                </Items>    
                            </ext:FieldSet>
                            <ext:Component ID="Component2" runat="server" Width="5"/>
                            <ext:FieldSet ID="FieldSet3" runat="server" Flex="1" Layout="HBoxLayout" Region="East">
                                <Items>
                                    <ext:GridPanel ID="ToursGP" runat="server" Flex="1" Layout="FitLayout" Height="110" AutoScroll="true" MultiSelect="false">
                                        <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                        <TopBar>
                                            <ext:Toolbar ID="ToursTB" runat="server">
                                                <Items>
                                                    <ext:Button ID="AddTourB" runat="server" Icon="Add" Text="Новый цикл"/>
                                                </Items>
                                            </ext:Toolbar>
                                        </TopBar>
                                        <Store>
                                            <ext:Store ID="ToursS" runat="server">
                                                <Model>
                                                    <ext:Model ID="ToursM" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="tour" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column ID="Tours" runat="server" Text="Циклы" DataIndex="tour" Width="60">
                                                    <Editor>
                                                        <ext:TextField ID="ToursTF" runat="server" MaskRe="/[0-9]/" EnableKeyEvents="true" />
                                                    </Editor>
                                                </ext:Column>                              
                                            </Columns>            
                                        </ColumnModel>  
                                        <Plugins>
                                            <ext:CellEditing ID="CellEditing3" runat="server" ClicksToEdit="2" />
                                        </Plugins>  
                                    </ext:GridPanel>                                      
                                    <ext:Component ID="Component3" runat="server" Width="5" />
                                    <ext:GridPanel ID="YearsGP" runat="server" Flex="1" Layout="FitLayout" Height="110" AutoScroll="true" MultiSelect="false">
                                        <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                        <TopBar>
                                            <ext:Toolbar ID="YearsTB" runat="server">
                                                <Items>
                                                    <ext:Button ID="AddYearB" runat="server" Icon="Add" Text="Новый год"/>
                                                </Items>
                                            </ext:Toolbar>
                                        </TopBar>
                                        <Store>
                                            <ext:Store ID="YearsS" runat="server">
                                                <Model>
                                                    <ext:Model ID="YearsM" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="year" />
                                                        </Fields>
                                                    </ext:Model> 
                                                </Model>
                                            </ext:Store>
                                        </Store>  
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column ID="Years" runat="server" Text="Года" DataIndex="year" Width="60">
                                                    <Editor>
                                                        <ext:TextField ID="YearsTF" runat="server" MaskRe="/[0-9]/" EnableKeyEvents="true" />
                                                    </Editor>
                                                </ext:Column>  
                                            </Columns>            
                                        </ColumnModel>  
                                        <Plugins>
                                            <ext:CellEditing ID="CellEditing10" runat="server" ClicksToEdit="2" />
                                        </Plugins>  
                                    </ext:GridPanel>                               
                                </Items>    
                            </ext:FieldSet>  
                       </Items>
                    </ext:Panel>
                    <ext:Panel ID="EditdbCenterP" runat="server" Header="false" Layout="FitLayout" Frame="true" Region="Center">
                       <Items>                     
                            <ext:GridPanel ID="PlotsGP" runat="server" Layout="FitLayout" Title="Участки" Width="970" AutoScroll="true" MultiSelect="false">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                <View>
                                     <ext:GridView ID="PlotsGV" runat="server" StripeRows="true" TrackOver="true" />
                                </View>
                                <TopBar>
                                    <ext:Toolbar ID="FieldsTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddPlotB" runat="server" Icon="Add" Text="Добавить" />
                                            <ext:Button ID="EditPlotB" runat="server" Icon="Pencil" Text="Редактировать" />
                                            <ext:Button ID="DeletePlotB" runat="server" Icon="Delete" Text="Удалить" />
                                            <ext:Component ID="Component4" runat="server" Width="300" />
                                            <ext:Checkbox ID="MicroElemCB" runat="server" BoxLabel="Микроэлементы" Checked="true"  />
                                            <ext:Component ID="Component5" runat="server" Width="10" />
                                            <ext:Checkbox ID="HeavyMetalCB" runat="server" BoxLabel="Тяжелые металлы" Checked="true"  />
                                            <ext:Component ID="Component6" runat="server" Width="10" />
                                            <ext:Checkbox ID="RadiologyCB" runat="server" BoxLabel="Радиология" Checked="true"  />
                                            <ext:Component ID="Component7" runat="server" Width="10" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>                                                
                                <Store>
                                    <ext:Store ID="PlotsS" runat="server">
                                        <Model>
                                            <ext:Model ID="PlotsM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_plot" />
                                                    <ext:ModelField Name="id_department" />
                                                    <ext:ModelField Name="code_plot" />
                                                    <ext:ModelField Name="number_plot" />
                                                    <ext:ModelField Name="area" />
                                                    <ext:ModelField Name="year" />
                                                    <ext:ModelField Name="tour" />
                                                    <ext:ModelField Name="number_field" />
                                                    <ext:ModelField Name="id_farmland" />
                                                    <ext:ModelField Name="code_farmland" />
                                                    <ext:ModelField Name="title_farmland" />
                                                    <ext:ModelField Name="id_culture" />
                                                    <ext:ModelField Name="code_culture" />
                                                    <ext:ModelField Name="title_culture" />
                                                    <ext:ModelField Name="id_old_culture" />
                                                    <ext:ModelField Name="code_old_culture" />
                                                    <ext:ModelField Name="title_old_culture" />
                                                    <ext:ModelField Name="id_crop_rotation" />
                                                    <ext:ModelField Name="code_crop_rotation" />
                                                    <ext:ModelField Name="title_crop_rotation" />
                                                    <ext:ModelField Name="number_crop_rotation" />
                                                    <ext:ModelField Name="id_type_soil" />
                                                    <ext:ModelField Name="code_type_soil" />
                                                    <ext:ModelField Name="title_type_soil" />
                                                    <ext:ModelField Name="id_grading" />
                                                    <ext:ModelField Name="code_grading" />
                                                    <ext:ModelField Name="title_grading" />
                                                    <ext:ModelField Name="id_erosion" />
                                                    <ext:ModelField Name="code_erosion" />
                                                    <ext:ModelField Name="title_erosion" />
                                                    <ext:ModelField Name="id_slope" />
                                                    <ext:ModelField Name="code_slope" />
                                                    <ext:ModelField Name="title_slope" />
                                                    <ext:ModelField Name="id_exposure" />
                                                    <ext:ModelField Name="code_exposure" /> 
                                                    <ext:ModelField Name="title_exposure" />         
                                                    <ext:ModelField Name="n" />
                                                    <ext:ModelField Name="no3" />
                                                    <ext:ModelField Name="no2" />
                                                    <ext:ModelField Name="p2o5" />
                                                    <ext:ModelField Name="k2o" />
                                                    <ext:ModelField Name="ph_s" />
                                                    <ext:ModelField Name="ph_w" />
                                                    <ext:ModelField Name="humus" />
                                                    <ext:ModelField Name="id_group_degree_humus" />
                                                    <ext:ModelField Name="id_degree_humus" />
                                                    <ext:ModelField Name="code_degree_humus" />
                                                    <ext:ModelField Name="title_degree_humus" />
                                                    <ext:ModelField Name="hydrolytic_acid" />
                                                    <ext:ModelField Name="absorbance_capacity" />
                                                    <ext:ModelField Name="total_absorbed_bases" />
                                                    <ext:ModelField Name="base_saturation" />
                                                    <ext:ModelField Name="id_priority_calcification" />
                                                    <ext:ModelField Name="number_pc_group" />
                                                    <ext:ModelField Name="title_pc_group" />
                                                    <ext:ModelField Name="id_method" />
                                                    <ext:ModelField Name="title_method" />
                                                    <ext:ModelField Name="dry_residue" />
                                                    <ext:ModelField Name="s" />
                                                    <ext:ModelField Name="ca" />        
                                                    <ext:ModelField Name="mn" />
                                                    <ext:ModelField Name="mo" />
                                                    <ext:ModelField Name="b" />
                                                    <ext:ModelField Name="cu" />
                                                    <ext:ModelField Name="mg" />
                                                    <ext:ModelField Name="zn" />
                                                    <ext:ModelField Name="na" />
                                                    <ext:ModelField Name="co" />
                                                    <ext:ModelField Name="al" />
                                                    <ext:ModelField Name="fe" />
                                                    <ext:ModelField Name="cu_hm" />
                                                    <ext:ModelField Name="zn_hm" />
                                                    <ext:ModelField Name="cd_hm" />
                                                    <ext:ModelField Name="pb_hm" />
                                                    <ext:ModelField Name="ni_hm" />
                                                    <ext:ModelField Name="hg_hm" />
                                                    <ext:ModelField Name="mg_hm" />
                                                    <ext:ModelField Name="cr_hm" />
                                                    <ext:ModelField Name="fe_hm" />
                                                    <ext:ModelField Name="f_hm" />
                                                    <ext:ModelField Name="as_hm" />
                                                    <ext:ModelField Name="cs137" />
                                                    <ext:ModelField Name="sr90" />
                                                    <ext:ModelField Name="date_input" />
                                                    <ext:ModelField Name="date_last_edit" />
                                                    <ext:ModelField Name="id_user" />
                                                    <ext:ModelField Name="surname" />
                                                    <ext:ModelField Name="name" />
                                                    <ext:ModelField Name="patronymic" />
                                                    <ext:ModelField Name="date_survey" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                                            
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="Id_type_soil" runat="server" DataIndex="id_type_soil" Hidden="true" Align="Right" Text="Id типа почв" />
                                        <ext:Column ID="Code_type_soil" runat="server" DataIndex="code_type_soil" Hidden="true" Align="Right" Text="Код типа почв" />
                                        <ext:Column ID="Title_type_soil" runat="server" Text="Тип почв" DataIndex="title_type_soil" Width="140" />
                                        <ext:Column ID="Id_grading" runat="server" DataIndex="id_grading" Hidden="true" Align="Right" Text="Id мех. состава" />
                                        <ext:Column ID="Code_grading" runat="server" DataIndex="code_grading" Hidden="true" Align="Right" Text="Код мех. состава" />
                                        <ext:Column ID="Title_grading" runat="server" Text="Механический <br>состав" DataIndex="title_grading" Width="120" />
                                        <ext:Column ID="Id_erosion" runat="server" DataIndex="id_erosion" Hidden="true" Align="Right" Text="Id степ. ерод." />
                                        <ext:Column ID="Code_erosion" runat="server" DataIndex="code_erosion" Hidden="true" Align="Right" Text="Код степ. ерод." />
                                        <ext:Column ID="Title_erosion" runat="server" Text="Степень <br>эродиров-ти" DataIndex="title_erosion" Width="110" />
                                        <ext:Column ID="Id_slope" runat="server" DataIndex="id_slope" Width="40" Hidden="true" Align="Right" Text="Id уклона" />
                                        <ext:Column ID="Code_slope" runat="server" DataIndex="code_slope" Width="40" Hidden="true" Align="Right" Text="Код уклона" />
                                        <ext:Column ID="Title_slope" runat="server" Text="Уклон" DataIndex="title_slope" Width="40" Align="Right" />
                                        <ext:Column ID="Id_exposure" runat="server" DataIndex="id_exposure" Hidden="true" Align="Right" Text="Id экспозиции" />
                                        <ext:Column ID="Code_exposure" runat="server" DataIndex="code_exposure" Hidden="true" Align="Right" Text="Код экспозиции" />
                                        <ext:Column ID="Title_exposure" runat="server" Text="Эксп-я" DataIndex="title_exposure" Width="45" />
                                        <ext:Column ID="Id_farmland" runat="server" DataIndex="id_farmland" Hidden="true" Align="Right" Text="Id типа с.х. <br>угодий" />
                                        <ext:Column ID="Code_farmland" runat="server" DataIndex="code_farmland" Hidden="true" Align="Right" Text="Код типа с.х. <br>угодий" />
                                        <ext:Column ID="Title_farmland" runat="server" Text="Тип с.х. <br>угодий" DataIndex="title_farmland" Width="70" />
                                        <ext:Column ID="Id_crop_rotation" runat="server" DataIndex="id_crop_rotation" Hidden="true" Align="Right" Text="Id типа севооборота" />
                                        <ext:Column ID="Code_crop_rotation" runat="server" DataIndex="code_crop_rotation" Hidden="true" Align="Right" Text="Код типа севооборота" />
                                        <ext:Column ID="Title_crop_rotation" runat="server" Text="Тип севооборота" DataIndex="title_crop_rotation" Width="180" />
                                        <ext:Column ID="Number_crop_rotation" runat="server" Text="<div class='verticalText'>№ сев-а</div>" DataIndex="number_crop_rotation" Width="40" Align="Right" />
                                        <ext:Column ID="Number_field" runat="server" Text="<div class='verticalText'>№ поля</div>" DataIndex="number_field" Width="40" Align="Right" />
                                        <ext:Column ID="Id_old_culture" runat="server" DataIndex="id_old_culture" Hidden="true" Align="Right" Text="Id пред. культуры" />
                                        <ext:Column ID="Code_old_culture" runat="server" DataIndex="code_old_culture" Hidden="true" Align="Right" Text="Код пред. культуры" />
                                        <ext:Column ID="Title_old_culture" runat="server" Text="Предшеств. <br />культуры" DataIndex="title_old_culture" Width="30" />
                                        <ext:Column ID="Id_culture" runat="server" DataIndex="id_culture" Hidden="true" Align="Right" Text="Id культуры" />
                                        <ext:Column ID="Code_culture" runat="server" DataIndex="code_culture" Hidden="true" Align="Right" Text="Код культуры" />
                                        <ext:Column ID="Title_culture" runat="server" Text="Культура" DataIndex="title_culture" Width="30" />
                                        <ext:Column ID="Id_plot" runat="server" DataIndex="id_plot" Hidden="true" Align="Right" Text="Id участка" />
                                        <ext:Column ID="Id_department_plot" runat="server" DataIndex="id_department" Hidden="true" Align="Right" Text="Id отделения" />
                                        <ext:Column ID="Code_plot" runat="server" DataIndex="code_plot" Hidden="true" Align="Right" Text="Код участка" />
                                        <ext:Column ID="Number_plot" runat="server" Text="<div class='verticalText'>№ уч-ка</div>" DataIndex="number_plot" Width="40" Align="Right" />
                                        <ext:Column ID="Area" runat="server" Text="<div class='verticalText'>Площадь</div>" DataIndex="area" Width="55" Align="Right" />
                                        <ext:Column ID="Year" runat="server" DataIndex="year" Hidden="true" Align="Right" Text="Год" />
                                        <ext:Column ID="Tour" runat="server" DataIndex="tour" Hidden="true" Align="Right" Text="Цикл" />
                                        <ext:Column ID="Macro" runat="server" Text="Макроэлементы">
                                            <Columns> 
                                                <ext:Column ID="N" runat="server" Text="N" DataIndex="n" Width="35" Align="Right" />
                                                <ext:Column ID="NO3" runat="server" Text="NO<sub>3</sub>" DataIndex="no3" Width="35" Align="Right" />
                                                <ext:Column ID="NO2" runat="server" Text="NO<sub>2</sub>" DataIndex="no2" Width="35" Align="Right" />
                                                <ext:Column ID="P2O5" runat="server" Text="P<sub>2</sub>O<sub>5</sub>" DataIndex="p2o5" Width="35" Align="Right" />
                                                <ext:Column ID="K2O" runat="server" Text="K<sub>2</sub>O" DataIndex="k2o" Width="35" Align="Right" />
                                                <ext:Column ID="Ph_s" runat="server" Text="pH<sub>HCl</sub>" DataIndex="ph_s" Width="40" Align="Right" />
                                                <ext:Column ID="Ph_w" runat="server" Text="pH<sub>H<sub>2</sub>O</sub>" DataIndex="ph_w" Width="40" Align="Right" />
                                                <ext:Column ID="S" runat="server" Text="S" DataIndex="s" Width="35" Align="Right" />
                                                <ext:Column ID="Humus" runat="server" Text="Гумус,<br>%" DataIndex="humus" Width="40" Align="Right" />
                                                <ext:Column ID="Degree_humus" runat="server" Text="Степ.гум.,<br>класс" DataIndex="title_degree_humus" Width="100" Align="Left" />
                                                <ext:Column ID="Hydrolytic_acid" runat="server" Text="Hг" DataIndex="hydrolytic_acid" Width="40" Align="Right" />
                                                <ext:Column ID="Absorbance_capacity" runat="server" Text="Емк. <br>погл-я" DataIndex="absorbance_capacity" Width="40" Align="Right" />
                                                <ext:Column ID="Total_absorbed_bases" runat="server" Text="Сумма <br>погл. осн." DataIndex="total_absorbed_bases" Width="60" Align="Right" />
                                                <ext:Column ID="Base_saturation" runat="server" Text="Степень <br> нас. осн." DataIndex="base_saturation" Width="60" Align="Right" />
                                            </Columns>
                                        </ext:Column>
                                        <ext:Column ID="Id_priority_calcification" runat="server" DataIndex="id_priority_calcification" Width="40" Align="Right" Hidden="true" Text="Id очер. извест." />
                                        <ext:Column ID="Priority_calcification_group" runat="server" Text="Очер.<br>изв-я" DataIndex="number_pc_group" Width="40" Align="Right" />
                                        <ext:Column ID="Priority_calcification_title" runat="server" DataIndex="title_pc_group" Width="40" Align="Right" Hidden="true" Text="Название очер. извест." />
                                        <ext:Column ID="Id_method" runat="server" DataIndex="id_method" Hidden="true" Align="Right" Text="Id метода" />
                                        <ext:Column ID="Title_method" runat="server" DataIndex="title_method" Hidden="true" Align="Right" Text="Название метода" />
                                        <ext:Column ID="Dry_residue" runat="server" Text="Сухой<br>(плотный)<br>остаток" DataIndex="dry_residue" Width="70" Align="Right" />
                                        <ext:Column ID="Micro" runat="server" Text="Микроэлементы">
                                            <Columns>
                                                <ext:Column ID="Mn" runat="server" Text="Mn" DataIndex="mn" Width="40" Align="Right" />
                                                <ext:Column ID="Cu" runat="server" Text="Cu" DataIndex="cu" Width="40" Align="Right" />
                                                <ext:Column ID="Zn" runat="server" Text="Zn" DataIndex="zn" Width="40" Align="Right" />
                                                <ext:Column ID="Co" runat="server" Text="Co" DataIndex="co" Width="40" Align="Right" />
                                                <ext:Column ID="Ca" runat="server" Text="Ca" DataIndex="ca" Width="40" Align="Right" />
                                                <ext:Column ID="Mo" runat="server" Text="Mo" DataIndex="mo" Width="40" Align="Right" />
                                                <ext:Column ID="B" runat="server" Text="B" DataIndex="b" Width="40" Align="Right" />
                                                <ext:Column ID="Mg" runat="server" Text="Mg" DataIndex="mg" Width="40" Align="Right" />
                                                <ext:Column ID="Na" runat="server" Text="Na" DataIndex="na" Width="40" Align="Right" />
                                                <ext:Column ID="Al" runat="server" Text="Al" DataIndex="al" Width="40" Align="Right" />
                                                <ext:Column ID="Fe" runat="server" Text="Fe" DataIndex="fe" Width="40" Align="Right" />
                                            </Columns>
                                        </ext:Column>
                                        <ext:Column ID="Heavy_metal" runat="server" Text="Тяжёлые металлы">
                                            <Columns>
                                                <ext:Column ID="Cu_hm" runat="server" Text="Cu" DataIndex="cu_hm" Width="40" Align="Right" />
                                                <ext:Column ID="Zn_hm" runat="server" Text="Zn" DataIndex="zn_hm" Width="40" Align="Right" />
                                                <ext:Column ID="Cd_hm" runat="server" Text="Cd" DataIndex="cd_hm" Width="40" Align="Right" />
                                                <ext:Column ID="Pb_hm" runat="server" Text="Pb" DataIndex="pb_hm" Width="40" Align="Right" />
                                                <ext:Column ID="Ni_hm" runat="server" Text="Ni" DataIndex="ni_hm" Width="40" Align="Right" />
                                                <ext:Column ID="Hg_hm" runat="server" Text="Hg" DataIndex="hg_hm" Width="40" Align="Right" />
                                                <ext:Column ID="Mg_hm" runat="server" Text="Mg" DataIndex="mg_hm" Width="40" Align="Right" />
                                                <ext:Column ID="Cr_hm" runat="server" Text="Cr" DataIndex="cr_hm" Width="40" Align="Right" />
                                                <ext:Column ID="Fe_hm" runat="server" Text="Fe" DataIndex="fe_hm" Width="40" Align="Right" />
                                                <ext:Column ID="F_hm" runat="server" Text="F" DataIndex="f_hm" Width="40" Align="Right" />
                                                <ext:Column ID="As_hm" runat="server" Text="As" DataIndex="as_hm" Width="40" Align="Right" />
                                            </Columns>
                                        </ext:Column>
                                        <ext:Column ID="Radiology" runat="server" Text="Радиология">
                                            <Columns>
                                                <ext:Column ID="Cs137" runat="server" Text="Cs-137" DataIndex="cs137" Width="42" Align="Right" />
                                                <ext:Column ID="Sr90" runat="server" Text="Sr-90" DataIndex="sr90" Width="40" Align="Right" />
                                            </Columns>
                                        </ext:Column>
                                        <ext:Column ID="Date_input" Text="Дата ввода" runat="server" DataIndex="date_input" Align="Center" Width="130" />
                                        <ext:Column ID="Date_last_edit" Text="Дата последнего<br>редактирования" runat="server" DataIndex="date_last_edit" Align="Center" Width="130" />
                                        <ext:Column ID="Id_User" runat="server" DataIndex="id_user" Hidden="true" Align="Right" Width="40" Text="Id пользователя" />
                                        <ext:Column ID="UserSurname" runat="server" DataIndex="surname" Hidden="true" Align="Left" Width="100" Text="Фамилия" />
                                        <ext:Column ID="UserName" runat="server" DataIndex="name" Hidden="true" Align="Left" Width="100" Text="Имя" />
                                        <ext:Column ID="UserPatronymic" runat="server" DataIndex="patronymic" Hidden="true" Align="Left" Width="100" Text="Отчество" />
                                        <ext:Column ID="Date_survey" Text="Дата обследования" runat="server" DataIndex="date_survey" Align="Center" Width="130" />
                                    </Columns>
                                </ColumnModel>
                            </ext:GridPanel>
                       </Items>
                    </ext:Panel>
                    <ext:Panel ID="EditdbEPlotsP" runat="server" Header="false" Layout="HBoxLayout" Height="250" Frame="true" Region="South" Split="true">
                       <Items>
                            <ext:GridPanel ID="EPlotsGP" runat="server" Flex="2" Region="Center" Layout="FitLayout" Title="Образцы" Height="190" MultiSelect="false" AutoScroll="true">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                <TopBar>
                                    <ext:Toolbar ID="EPlotsTB" runat="server">
                                        <Items>
                                            <ext:Button ID="CoordinatesB" runat="server" Icon="World" Text="Координаты" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                    <Store>
                                        <ext:Store ID="EPlotsS" runat="server">
                                            <Model>
                                                <ext:Model ID="EPlotsM" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="id_elementary_plot" />
                                                        <ext:ModelField Name="code_elementary_plot" />
                                                        <ext:ModelField Name="id_plot" />
                                                        <ext:ModelField Name="code_plot" />
                                                        <ext:ModelField Name="number_plot" />
                                                        <ext:ModelField Name="number_elementary_plot" />
                                                        <ext:ModelField Name="p2o5" />                                                         
                                                        <ext:ModelField Name="k2o" />
                                                        <ext:ModelField Name="ph_s" />
                                                        <ext:ModelField Name="ph_w" />
                                                        <ext:ModelField Name="hydrolytic_acid" />
                                                        <ext:ModelField Name="id_method" /> 
                                                        <ext:ModelField Name="title_method" />
                                                        <ext:ModelField Name="id_type_soil" />
                                                        <ext:ModelField Name="code_type_soil" />
                                                        <ext:ModelField Name="title_type_soil" />
                                                        <ext:ModelField Name="id_grading" />
                                                        <ext:ModelField Name="code_grading" />
                                                        <ext:ModelField Name="title_grading" />
                                                        <ext:ModelField Name="id_erosion" />
                                                        <ext:ModelField Name="code_erosion" />
                                                        <ext:ModelField Name="title_erosion" />                                                    
                                                        <ext:ModelField Name="id_slope" />
                                                        <ext:ModelField Name="code_slope" />
                                                        <ext:ModelField Name="title_slope" />
                                                        <ext:ModelField Name="id_exposure" />
                                                        <ext:ModelField Name="code_exposure" />
                                                        <ext:ModelField Name="title_exposure" />
                                                        <ext:ModelField Name="n" />
                                                        <ext:ModelField Name="no3" />
                                                        <ext:ModelField Name="no2" />                                                                                                            
                                                        <ext:ModelField Name="humus" />
                                                        <ext:ModelField Name="absorbance_capacity" />
                                                        <ext:ModelField Name="total_absorbed_bases" />
                                                        <ext:ModelField Name="base_saturation" />
                                                        <ext:ModelField Name="id_priority_calcification" />
                                                        <ext:ModelField Name="number_pc_group" />
                                                        <ext:ModelField Name="title_pc_group" />
                                                        <ext:ModelField Name="s" />
                                                        <ext:ModelField Name="ca" />        
                                                        <ext:ModelField Name="mn" />
                                                        <ext:ModelField Name="mo" />
                                                        <ext:ModelField Name="b" />
                                                        <ext:ModelField Name="cu" />
                                                        <ext:ModelField Name="mg" />
                                                        <ext:ModelField Name="zn" />
                                                        <ext:ModelField Name="na" />
                                                        <ext:ModelField Name="co" />
                                                        <ext:ModelField Name="al" />
                                                        <ext:ModelField Name="fe" />
                                                        <ext:ModelField Name="cu_hm" />
                                                        <ext:ModelField Name="zn_hm" />
                                                        <ext:ModelField Name="cd_hm" />
                                                        <ext:ModelField Name="pb_hm" />
                                                        <ext:ModelField Name="ni_hm" />
                                                        <ext:ModelField Name="hg_hm" />
                                                        <ext:ModelField Name="mg_hm" />
                                                        <ext:ModelField Name="cr_hm" />
                                                        <ext:ModelField Name="fe_hm" />
                                                        <ext:ModelField Name="f_hm" />
                                                        <ext:ModelField Name="as_hm" />
                                                        <ext:ModelField Name="cs137" />
                                                        <ext:ModelField Name="sr90" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column ID="Id_type_soil_eplot" runat="server" DataIndex="id_type_soil" Hidden="true" Align="Right" />
                                            <ext:Column ID="Code_type_soil_eplot" runat="server" DataIndex="code_type_soil" Hidden="true" Align="Right" />
                                            <ext:Column ID="Title_type_soil_eplot" runat="server" DataIndex="title_type_soil" Hidden="true" />
                                            <ext:Column ID="Id_grading_eplot" runat="server" DataIndex="id_grading" Hidden="true" Align="Right" />
                                            <ext:Column ID="Code_grading_eplot" runat="server" DataIndex="code_grading" Hidden="true" Align="Right" />
                                            <ext:Column ID="Title_grading_eplot" runat="server" DataIndex="title_grading" Hidden="true" />
                                            <ext:Column ID="Id_erosion_eplot" runat="server" DataIndex="id_erosion" Hidden="true" Align="Right" />
                                            <ext:Column ID="Code_erosion_eplot" runat="server" DataIndex="code_erosion" Hidden="true" Align="Right" />
                                            <ext:Column ID="Title_erosion_eplot" runat="server" DataIndex="title_erosion" Hidden="true" />
                                            <ext:Column ID="Id_slope_eplot" runat="server" DataIndex="id_slope" Hidden="true" Align="Right" />
                                            <ext:Column ID="Code_slope_eplot" runat="server" DataIndex="code_slope" Hidden="true" Align="Right" />
                                            <ext:Column ID="Title_slope_eplot" runat="server" DataIndex="title_slope" Hidden="true" />
                                            <ext:Column ID="Id_exposure_eplot" runat="server" DataIndex="id_exposure" Hidden="true" Align="Right" />
                                            <ext:Column ID="Code_exposure_eplot" runat="server" DataIndex="code_exposure" Hidden="true" Align="Right" />
                                            <ext:Column ID="Title_exposure_eplot" runat="server" DataIndex="title_exposure" Hidden="true" />
                                            <ext:Column ID="N_eplot" runat="server" DataIndex="n" Hidden="true" Align="Right" />
                                            <ext:Column ID="No3_eplot" runat="server" DataIndex="no3" Hidden="true" Align="Right" />
                                            <ext:Column ID="No2_eplot" runat="server" DataIndex="no2" Hidden="true" Align="Right" />
                                            <ext:Column ID="Humus_eplot" runat="server" DataIndex="humus" Hidden="true" />
                                            <ext:Column ID="Absorbance_capacity_eplot" runat="server" DataIndex="absorbance_capacity" Hidden="true" Align="Right" />
                                            <ext:Column ID="Total_absorbed_bases_eplot" runat="server" DataIndex="total_absorbed_bases" Hidden="true" Align="Right" />
                                            <ext:Column ID="Base_saturation_eplot" runat="server" DataIndex="base_saturation" Hidden="true" Align="Right" />
                                            <ext:Column ID="Id_priority_calcification_eplot" runat="server" DataIndex="id_priority_calcification" Hidden="true" Align="Right" />
                                            <ext:Column ID="Priority_calcification_eplot_group" runat="server" DataIndex="number_pc_group" Hidden="true" Align="Right" />
                                            <ext:Column ID="Priority_calcification_eplot_title" runat="server" DataIndex="title_pc_group" Hidden="true" Align="Right" />
                                            <ext:Column ID="S_eplot" runat="server" DataIndex="s" Hidden="true" Align="Right" />
                                            <ext:Column ID="Ca_eplot" runat="server" DataIndex="ca" Hidden="true" Align="Right" />
                                            <ext:Column ID="Mn_eplot" runat="server" DataIndex="mn" Hidden="true" Align="Right" />
                                            <ext:Column ID="Mo_eplot" runat="server" DataIndex="mo" Hidden="true" Align="Right" />
                                            <ext:Column ID="B_eplot" runat="server" DataIndex="b" Hidden="true" Align="Right" />
                                            <ext:Column ID="Cu_eplot" runat="server" DataIndex="cu" Hidden="true" Align="Right" />
                                            <ext:Column ID="Mg_eplot" runat="server" DataIndex="mg" Hidden="true" Align="Right" />
                                            <ext:Column ID="Zn_eplot" runat="server" DataIndex="zn" Hidden="true" Align="Right" />
                                            <ext:Column ID="Na_eplot" runat="server" DataIndex="na" Hidden="true" Align="Right" />
                                            <ext:Column ID="Co_eplot" runat="server" DataIndex="co" Hidden="true" Align="Right" />
                                            <ext:Column ID="Al_eplot" runat="server" DataIndex="al" Hidden="true" Align="Right" />
                                            <ext:Column ID="Fe_eplot" runat="server" DataIndex="fe" Hidden="true" Align="Right" />
                                            <ext:Column ID="Cu_hm_eplot" runat="server" DataIndex="cu_hm" Hidden="true" Align="Right" />
                                            <ext:Column ID="Zn_hm_eplot" runat="server" DataIndex="zn_hm" Hidden="true" Align="Right" />
                                            <ext:Column ID="Cd_hm_eplot" runat="server" DataIndex="cd_hm" Hidden="true" Align="Right" />
                                            <ext:Column ID="Pb_hm_eplot" runat="server" DataIndex="pb_hm" Hidden="true" Align="Right" />
                                            <ext:Column ID="Ni_hm_eplot" runat="server" DataIndex="ni_hm" Hidden="true" Align="Right" />
                                            <ext:Column ID="Hg_hm_eplot" runat="server" DataIndex="hg_hm" Hidden="true" Align="Right" />
                                            <ext:Column ID="Mg_hm_eplot" runat="server" DataIndex="mg_hm" Hidden="true" Align="Right" />
                                            <ext:Column ID="Cr_hm_eplot" runat="server" DataIndex="cr_hm" Hidden="true" Align="Right" />
                                            <ext:Column ID="Fe_hm_eplot" runat="server" DataIndex="fe_hm" Hidden="true" Align="Right" />
                                            <ext:Column ID="F_hm_eplot" runat="server" DataIndex="f_hm" Hidden="true" Align="Right" />
                                            <ext:Column ID="As_hm_eplot" runat="server" DataIndex="as_hm" Hidden="true" Align="Right" />
                                            <ext:Column ID="Cs137_eplot" runat="server" DataIndex="cs137" Hidden="true" Align="Right" />
                                            <ext:Column ID="Sr90_eplot" runat="server" DataIndex="sr90" Hidden="true" Align="Right" />
                                            <ext:Column ID="Id_method_eplot" runat="server" DataIndex="id_method" Hidden="true" Align="Right" />
                                            <ext:Column ID="Ph_w_eplot" runat="server" DataIndex="ph_w" Hidden="true" Align="Right" />
                                            <ext:Column ID="Number_plot_eplot" runat="server" DataIndex="number_plot" Hidden="true" Align="Right" />
                                            <ext:Column ID="Code_plot_eplot" runat="server" DataIndex="code_plot" Hidden="true" Align="Right" />
                                            <ext:Column ID="Id_eplot" runat="server" DataIndex="id_elementary_plot" Hidden="true" Align="Right" />
                                            <ext:Column ID="Code_eplot" runat="server" DataIndex="code_elementary_plot" Hidden="true" Align="Right" />
                                            <ext:Column ID="Id_plot_eplot" runat="server" DataIndex="id_plot" Hidden="true" Align="Right" />                                                
                                            <ext:Column ID="Number_eplot" runat="server" Text="№ образца" DataIndex="number_elementary_plot" Width="70" Align="Right" />        
                                            <ext:Column ID="Ph_s_eplot" runat="server" Text="pH<sub>HCl</sub>" DataIndex="ph_s" Width="40" Align="Right" />   
                                            <ext:Column ID="Hydrolytic_acid_eplot" runat="server" Text="Hг" DataIndex="hydrolytic_acid" Width="35" Align="Right" /> 
                                            <ext:Column ID="P2O5_eplot" runat="server" Text="P<sub>2</sub>O<sub>5</sub>" DataIndex="p2o5" Width="35" Align="Right" />   
                                            <ext:Column ID="K2O_eplot" runat="server" Text="K<sub>2</sub>O" DataIndex="k2o" Width="35" Align="Right" />
                                            <ext:Column ID="Title_method_eplot" runat="server" Text="Метод" DataIndex="title_method" Width="70" />                              
                                        </Columns>
                                    </ColumnModel>
                           </ext:GridPanel>
                           <ext:Component ID="Component14" runat="server" Width="10" />
                           <ext:GridPanel ID="SlopesGP" runat="server" Flex="1" Region="Center" Layout="FitLayout" Title="Уклоны" Height="190" MultiSelect="false" AutoScroll="true">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                <TopBar>
                                    <ext:Toolbar ID="SlopesTB" runat="server">
                                        <Items>
                                            <ext:Button ID="ImportSlopesB" runat="server" Icon="PageAdd" Text="Импорт из файла" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                    <Store>
                                        <ext:Store ID="SlopesS" runat="server">
                                            <Model>
                                                <ext:Model ID="SlopesM" runat="server">
                                                    <Fields>  
                                                        <ext:ModelField Name="id_plot" />
                                                        <ext:ModelField Name="id_geographic" /> 
                                                        <ext:ModelField Name="id_slope" />
                                                        <ext:ModelField Name="code_slope" /> 
                                                        <ext:ModelField Name="title_slope" />
                                                        <ext:ModelField Name="area" />  
                                                        <ext:ModelField Name="percentage" />                                                     
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel>
                                        <Columns>      
                                            <ext:Column ID="Id_plot_slope" runat="server" DataIndex="id_plot" Hidden="true" />
                                            <ext:Column ID="Id_geographic_slope" runat="server" DataIndex="id_geographic" Hidden="true" />
                                            <ext:Column ID="Id_slope_slope" runat="server" DataIndex="id_slope" Hidden="true" />
                                            <ext:Column ID="Code_slope_slope" runat="server" DataIndex="code_slope" Hidden="true" />  
                                            <ext:Column ID="Title_slope_slope" runat="server" DataIndex="title_slope" Hidden="false" Align="Center" Text="Уклон" />
                                            <ext:Column ID="Area_slope_slope" runat="server" DataIndex="area" Hidden="true" />
                                            <ext:Column ID="Area_percentage_slope" runat="server" DataIndex="percentage" Hidden="false" Align="Center" Text="Площадь, %" />                       
                                        </Columns>
                                    </ColumnModel>
                            </ext:GridPanel>
                            <ext:Component ID="Component8" runat="server" Width="10" />
                            <ext:FieldSet ID="StatisticsFS" runat="server" Flex="1" Region="East" Height="190" Layout="FormLayout" Title="Статистика по <br> хозяйству и циклу">
                                <Items>
                                    <ext:TextField ID="CountPlotTF" runat="server" FieldLabel="Количество участков" LabelCls="font-label" LabelWidth="110"/>
                                    <ext:Component ID="Component9" runat="server" Width="10" />
                                    <ext:TextField ID="CountEPlotTF" runat="server" FieldLabel="Количество образцов" LabelCls="font-label" LabelWidth="110"/>
                                    <ext:Component ID="Component10" runat="server" Width="10" />
                                    <ext:TextField ID="AreaFieldTF" runat="server" FieldLabel="Площадь участков" LabelCls="font-label" LabelWidth="110"/>
                                </Items>    
                            </ext:FieldSet>
                        </Items>
                        <Buttons>
                            <ext:Button ID="ShowMapB" runat="server" Icon="Map" Text="Карта" Height="40" Width="40" HrefTarget="_blank" Href="OLGISMap.aspx" />
                            <ext:Button ID="PaspVedB" runat="server" Text="Паспортная <br>ведомость" Height="40" />
                            <ext:Button ID="GroupByTFB" runat="server" Text="Групп-ка почв по <br>типам с/х угодий" Height="40" />
                            <ext:Button ID="OcherIzvB" runat="server" Text="Очер. изв-я <br>кислых почв" Height="40" />
                            <ext:Button ID="GroupByTFHMB" runat="server" Text="Групп-ка почв по <br>типам с/х уг. (ТМ)" Height="40" />
                        </Buttons>
                    </ext:Panel> 
                </Items> 
                <BottomBar>
                    <ext:StatusBar ID="EditdbSB" runat="server" DefaultText="Готово" />
                </BottomBar>
            </ext:Window> 
            <ext:Window ID="EditPlotW" runat="server" Title="Редактирование данных по образцам" Width="735" Height="740"  Modal="true" Hidden="true" Layout="VBoxLayout" Resizable="False" ButtonAlign="Center">
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                </LayoutConfig>
                <Items>
                    <ext:Panel ID="EditPlotTopP" runat="server" Frame="true" Layout="HBoxLayout">
                       <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                        </LayoutConfig>
                       <Items>
                            <ext:FieldContainer runat="server" Layout="FormLayout" Flex="1">
                                <Items>
                                    <ext:TextField ID="FarmlandTF" runat="server" FieldLabel="Тип угодий" LabelSeparator="" LabelWidth="100" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                                    <ext:TextField ID="OldCultureTF" runat="server" FieldLabel="Предшественник" LabelSeparator="" LabelWidth="100" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                                    <ext:TextField ID="CultureTF" runat="server" FieldLabel="Культура" LabelSeparator="" LabelWidth="100" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                                    <ext:TextField ID="CropRotationTF" runat="server" FieldLabel="Тип севооборота" LabelSeparator="" LabelWidth="100" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                                    <ext:TextField ID="SoilTF" runat="server" FieldLabel="Тип почв" LabelSeparator="" LabelWidth="100" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                                    <ext:TextField ID="GradingTF" runat="server" FieldLabel="Гран. состав" LabelSeparator="" LabelWidth="100" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                                    <ext:TextField ID="ErosionTF" runat="server" FieldLabel="Степень эродир." LabelSeparator="" LabelWidth="100" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                                    <ext:TextField ID="SlopeTF" runat="server" FieldLabel="Уклон" LabelSeparator="" LabelWidth="100" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                                    <ext:TextField ID="ExposureTF" runat="server" FieldLabel="Экспозиция" LabelSeparator="" LabelWidth="100" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                               </Items>
                            </ext:FieldContainer>
                            <ext:Component runat="server" Width="5" />  
                            <ext:FieldContainer runat="server" Layout="FormLayout" Flex="1">
                                <Items>                               
                                    <ext:TextField ID="IdPlotTF" runat="server" TabIndex="-1" />
                                    <ext:TextField ID="YearTF" runat="server" FieldLabel="Год обследования" LabelSeparator="" LabelWidth="130" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true"  />
                                    <ext:TextField ID="TourTF" runat="server" FieldLabel="Цикл обследования" LabelSeparator="" LabelWidth="130" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                                    <ext:TextField ID="DepartmentCodeTF" runat="server" FieldLabel="Код отделения" LabelSeparator="" LabelWidth="130" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                                    <ext:TextField ID="MethodTF" runat="server" FieldLabel="Метод расчета" LabelSeparator="" LabelWidth="130" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                                    <ext:Component ID="Component113" runat="server" Height="22" />
                                    <ext:DateField ID="DateSurvey" runat="server" FieldLabel="Дата обследования" LabelWidth="130" LabelCls="darkslateblue-note" TabIndex="-1" Editable="false" />
                                    <ext:Component ID="Component114" runat="server" Height="22" />
                                    <ext:TextField ID="DateInputTF" runat="server" FieldLabel="Дата ввода" LabelSeparator="" LabelWidth="130" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                                    <ext:TextField ID="DateEditTF" runat="server" FieldLabel="Дата редактирования" LabelSeparator="" LabelWidth="130" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                               </Items>
                            </ext:FieldContainer>                       
                        </Items>                                          
                    </ext:Panel>
                    <ext:Panel ID="EditPlotCenterP" runat="server" Frame="true" Layout="VBoxLayout" >
                        <Items>
                            <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout" >
                                <Items>
                                    <ext:Component ID="Component16" runat="server" Width="13" />
                                    <ext:TextField ID="EditFarmlandTF" runat="server" Note="Тип угод." NoteAlign="Top" Width="59" NoteCls="darkorange-note" TabIndex="19" EnableKeyEvents="true" />
                                    <ext:Component ID="Component17" runat="server" Width="4" />
                                    <ext:TextField ID="EditOldCultureTF" runat="server" Note="Предш-к" NoteAlign="Top" Width="58" NoteCls="darkorange-note" TabIndex="20" EnableKeyEvents="true" />
                                    <ext:Component ID="Component13" runat="server" Width="4" />
                                    <ext:TextField ID="EditCultureTF" runat="server" Note="Культура" NoteAlign="Top" Width="58" NoteCls="darkorange-note" TabIndex="21" EnableKeyEvents="true" />
                                    <ext:Component ID="Component18" runat="server" Width="4" />
                                    <ext:TextField ID="EditTypeCropRotationTF" runat="server" Note="Тип сев-а" NoteAlign="Top" Width="59" NoteCls="darkorange-note" TabIndex="22" EnableKeyEvents="true" />
                                    <ext:Component ID="Component19" runat="server" Width="4" />
                                    <ext:TextField ID="EditSoilTF" runat="server" Note="Тип почв" NoteAlign="Top" Width="59" NoteCls="darkorange-note" TabIndex="23" EnableKeyEvents="true" /> 
                                    <ext:Component ID="Component20" runat="server" Width="4" />
                                    <ext:TextField ID="EditGradingTF" runat="server" Note="Гран. сост" NoteAlign="Top" Width="59" NoteCls="darkorange-note" TabIndex="24" EnableKeyEvents="true" />
                                    <ext:Component ID="Component21" runat="server" Width="4" />
                                    <ext:TextField ID="EditErosionTF" runat="server" Note="Ст. эрод." NoteAlign="Top" Width="58" NoteCls="darkorange-note" TabIndex="25" EnableKeyEvents="true" />
                                    <ext:Component ID="Component22" runat="server" Width="4" />
                                    <ext:TextField ID="EditNumberCropRotationTF" runat="server" Note="№ сев-а" NoteAlign="Top" Width="58" NoteCls="darkorange-note" TabIndex="26" EnableKeyEvents="true" />
                                    <ext:Component ID="Component23" runat="server" Width="4" />
                                    <ext:TextField ID="EditFieldTF" runat="server" Note="№ поля" NoteAlign="Top" Width="58" NoteCls="darkorange-note" TabIndex="27" EnableKeyEvents="true" />
                                    <ext:Component ID="Component24" runat="server" Width="4" />
                                    <ext:TextField ID="NumberPlotTF" runat="server" Note="№ уч-ка" NoteAlign="Top" Width="58" NoteCls="darkorange-note" TabIndex="28" EnableKeyEvents="true" />
                                    <ext:Component ID="Component25" runat="server" Width="4" /> 
                                    <ext:TextField ID="EditAreaTF" runat="server" Note="Площадь" NoteAlign="Top" Width="59" NoteCls="darkorange-note" TabIndex="29" EnableKeyEvents="true" />                                            
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                                <Items> 
                                    <ext:Component ID="Component26" runat="server" Width="13" />
                                    <ext:TextField ID="HydrAcidTF" runat="server" Note="H<sub>г</sub>" NoteAlign="Top" Width="54" NoteCls="green-note" TabIndex="63" EnableKeyEvents="true" />
                                    <ext:Component ID="Component27" runat="server" Width="3" />
                                    <ext:TextField ID="PhSTF" runat="server" Note="pH<sub>HCl</sub>" NoteAlign="Top" Width="54" NoteCls="green-note" TabIndex="64" EnableKeyEvents="true" />
                                    <ext:Component ID="Component1" runat="server" Width="3" />
                                    <ext:TextField ID="PhWTF" runat="server" Note="pH<sub>H2O</sub>" NoteAlign="Top" Width="54" NoteCls="green-note" TabIndex="65" EnableKeyEvents="true" />
                                    <ext:Component ID="Component28" runat="server" Width="3" />
                                    <ext:TextField ID="P2O5TF" runat="server" Note="P<sub>2</sub>O<sub>5</sub>" NoteAlign="Top" Width="54" NoteCls="green-note" TabIndex="66" EnableKeyEvents="true" />                                    
                                    <ext:Component ID="Component29" runat="server" Width="3" />
                                    <ext:TextField ID="K2OTF" runat="server" Note="K<sub>2</sub>O" NoteAlign="Top" Width="54" NoteCls="green-note" TabIndex="67" EnableKeyEvents="true" />
                                    <ext:Component ID="Component34" runat="server" Width="3" />
                                    <ext:TextField ID="NO3TF" runat="server" Note="NO<sub>3</sub>" NoteAlign="Top" Width="54" NoteCls="lightseagreen-note" TabIndex="68" EnableKeyEvents="true" />
                                    <ext:Component ID="Component35" runat="server" Width="3" />
                                    <ext:TextField ID="NO2TF" runat="server" Note="NO<sub>2</sub>" NoteAlign="Top" Width="54" NoteCls="lightseagreen-note" TabIndex="69" EnableKeyEvents="true" />
                                    <ext:Component ID="Component33" runat="server" Width="3" />
                                    <ext:TextField ID="NTF" runat="server" Note="N<sub> </sub>" NoteAlign="Top" Width="54" NoteCls="lightseagreen-note" TabIndex="31" EnableKeyEvents="true" />
                                    <ext:Component ID="Component11" runat="server" Width="3" />
                                    <ext:TextField ID="HumusTF" runat="server" Note="Гумус<sub> </sub>" NoteAlign="Top" Width="54" NoteCls="lightseagreen-note" TabIndex="32" EnableKeyEvents="true" />
                                    <ext:Component ID="Component30" runat="server" Width="3" />
                                    <ext:TextField ID="CapacityTF" runat="server" Note="Емк.погл.<sub> </sub>" NoteAlign="Top" Width="56" NoteCls="lightseagreen-note" TabIndex="-1" ReadOnly="true" Enabled="false" />
                                    <ext:Component ID="Component31" runat="server" Width="3" />
                                    <ext:TextField ID="TotalAbsorbedBaseTF" runat="server" Note="С.п.осн.<sub> </sub>" NoteAlign="Top" Width="54" NoteCls="lightseagreen-note" TabIndex="33" EnableKeyEvents="true" /> 
                                    <ext:Component ID="Component32" runat="server" Width="3" />
                                    <ext:TextField ID="BaseSaturationTF" runat="server" Note="С.н.осн.<sub> </sub>" NoteAlign="Top" Width="54" NoteCls="lightseagreen-note" TabIndex="-1" ReadOnly="true" Enabled="false" EnableKeyEvents="true" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:Component ID="Component58" runat="server" Width="13" />
                                    <ext:TextField ID="MnTF" runat="server" Note="Mn" NoteAlign="Top" Width="54" NoteCls="blue-note" TabIndex="34" EnableKeyEvents="true" />
                                    <ext:Component ID="Component36" runat="server" Width="3" />
                                    <ext:TextField ID="STF" runat="server" Note="S" NoteAlign="Top" Width="54" NoteCls="blue-note" TabIndex="35" EnableKeyEvents="true" />
                                    <ext:Component ID="Component38" runat="server" Width="3" />
                                    <ext:TextField ID="CuTF" runat="server" Note="Cu" NoteAlign="Top" Width="54" NoteCls="blue-note" TabIndex="36" EnableKeyEvents="true" />
                                    <ext:Component ID="Component39" runat="server" Width="3" />
                                    <ext:TextField ID="ZnTF" runat="server" Note="Zn" NoteAlign="Top" Width="56" NoteCls="blue-note" TabIndex="37" EnableKeyEvents="true" />
                                    <ext:Component ID="Component40" runat="server" Width="3" />
                                    <ext:TextField ID="CoTF" runat="server" Note="Co" NoteAlign="Top" Width="54" NoteCls="blue-note" TabIndex="38" EnableKeyEvents="true" />
                                    <ext:Component ID="Component12" runat="server" Width="3" />
                                    <ext:TextField ID="FeTF" runat="server" Note="Fe" NoteAlign="Top" Width="54" NoteCls="blue-note" TabIndex="41" EnableKeyEvents="true" />
                                    <ext:Component ID="Component41" runat="server" Width="3" />
                                    <ext:TextField ID="AlTF" runat="server" Note="Al" NoteAlign="Top" Width="54" NoteCls="blue-note" TabIndex="42" EnableKeyEvents="true" />
                                    <ext:Component ID="Component42" runat="server" Width="3" />
                                    <ext:TextField ID="CaTF" runat="server" Note="Ca" NoteAlign="Top" Width="54" NoteCls="blue-note" TabIndex="43" EnableKeyEvents="true" />
                                    <ext:Component ID="Component43" runat="server" Width="3" />
                                    <ext:TextField ID="MoTF" runat="server" Note="Mo" NoteAlign="Top" Width="54" NoteCls="blue-note" TabIndex="44" EnableKeyEvents="true" /> 
                                    <ext:Component ID="Component44" runat="server" Width="3" />
                                    <ext:TextField ID="BTF" runat="server" Note="B" NoteAlign="Top" Width="54" NoteCls="blue-note" TabIndex="45" EnableKeyEvents="true" />
                                    <ext:Component ID="Component45" runat="server" Width="3" />
                                    <ext:TextField ID="MgTF" runat="server" Note="Mg" NoteAlign="Top" Width="54" NoteCls="blue-note" TabIndex="46" EnableKeyEvents="true" />
                                    <ext:Component ID="Component46" runat="server" Width="3" />
                                    <ext:TextField ID="NaTF" runat="server" Note="Na" NoteAlign="Top" Width="54" NoteCls="blue-note" TabIndex="47" EnableKeyEvents="true" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout" >
                                <Items> 
                                    <ext:Component ID="Component47" runat="server" Width="13" />
                                    <ext:TextField ID="CuhmTF" runat="server" Note="Cu" NoteAlign="Top" Width="58" NoteCls="red-note" TabIndex="48" EnableKeyEvents="true" />
                                    <ext:Component ID="Component48" runat="server" Width="5" />
                                    <ext:TextField ID="ZnhmTF" runat="server" Note="Zn" NoteAlign="Top" Width="58" NoteCls="red-note" TabIndex="49" EnableKeyEvents="true" />
                                    <ext:Component ID="Component49" runat="server" Width="5" />
                                    <ext:TextField ID="CdhmTF" runat="server" Note="Cd" NoteAlign="Top" Width="58" NoteCls="red-note" TabIndex="50" EnableKeyEvents="true" />
                                    <ext:Component ID="Component50" runat="server" Width="5" />
                                    <ext:TextField ID="PbhmTF" runat="server" Note="Pb" NoteAlign="Top" Width="58" NoteCls="red-note" TabIndex="51" EnableKeyEvents="true" />
                                    <ext:Component ID="Component51" runat="server" Width="5" />
                                    <ext:TextField ID="NihmTF" runat="server" Note="Ni" NoteAlign="Top" Width="58" NoteCls="red-note" TabIndex="52" EnableKeyEvents="true" /> 
                                    <ext:Component ID="Component52" runat="server" Width="5" />
                                    <ext:TextField ID="HghmTF" runat="server" Note="Hg" NoteAlign="Top" Width="58" NoteCls="red-note" TabIndex="53" EnableKeyEvents="true" />
                                    <ext:Component ID="Component53" runat="server" Width="5" />
                                    <ext:TextField ID="MghmTF" runat="server" Note="Mg" NoteAlign="Top" Width="57" NoteCls="red-note" TabIndex="54" EnableKeyEvents="true" />
                                    <ext:Component ID="Component54" runat="server" Width="5" />
                                    <ext:TextField ID="CrhmTF" runat="server" Note="Cr" NoteAlign="Top" Width="57" NoteCls="red-note" TabIndex="55" EnableKeyEvents="true" />
                                    <ext:Component ID="Component55" runat="server" Width="5" />
                                    <ext:TextField ID="FehmTF" runat="server" Note="Fe" NoteAlign="Top" Width="57" NoteCls="red-note" TabIndex="56" EnableKeyEvents="true" />
                                    <ext:Component ID="Component56" runat="server" Width="5" />
                                    <ext:TextField ID="FhmTF" runat="server" Note="F" NoteAlign="Top" Width="57" NoteCls="red-note" TabIndex="57" EnableKeyEvents="true" />
                                    <ext:Component ID="Component57" runat="server" Width="5" />
                                    <ext:TextField ID="AshmTF" runat="server" Note="As" NoteAlign="Top" Width="57" NoteCls="red-note" TabIndex="58" EnableKeyEvents="true" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer ID="FieldContainer5" runat="server" Layout="HBoxLayout">
                                <Items> 
                                    <ext:Component ID="Component37" runat="server" Width="13" />
                                    <ext:TextField ID="Cs137TF" runat="server" Note="Cs-137" NoteAlign="Top" Width="58" NoteCls="blueviolet-note" TabIndex="59" EnableKeyEvents="true" />
                                    <ext:Component ID="Component59" runat="server" Width="5" />
                                    <ext:TextField ID="Sr90TF" runat="server" Note="Sr-90" NoteAlign="Top" Width="58" NoteCls="blueviolet-note" TabIndex="60" EnableKeyEvents="true" />
                                    <ext:Component ID="Component60" runat="server" Width="5" />
                                    <ext:TextField ID="EditSlopeTF" runat="server" Note="Уклон" NoteAlign="Top" Width="58" NoteCls="maroon-note" TabIndex="61" EnableKeyEvents="true" />
                                    <ext:Component ID="Component61" runat="server" Width="5" />
                                    <ext:TextField ID="EditExposureTF" runat="server" Note="Экспоз." NoteAlign="Top" Width="58" NoteCls="maroon-note" TabIndex="62" EnableKeyEvents="true" />
                                    <ext:Component ID="Component115" runat="server" Width="5" />
                                    <ext:TextField ID="DryResidueTF" runat="server" Note="Сух. ост." NoteAlign="Top" Width="58" NoteCls="maroon-note" TabIndex="63" EnableKeyEvents="true" />
                                </Items>
                            </ext:FieldContainer>
                        </Items>                                          
                    </ext:Panel>
                    <ext:Panel ID="EditPlotBottomP" runat="server" Header="false" Frame="true" Layout="BorderLayout" Height="200" Region="South">
                       <Items>
                           <ext:GridPanel ID="EditEPlotGP" runat="server" Layout="FitLayout" Region="Center" Flex="1"  MultiSelect="false" AutoScroll="true">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                <TopBar>
                                    <ext:Toolbar ID="EditEPlotTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddEPlotB" runat="server" Icon="Add" Text="Новый образец" Cls="x-btn-default" Enabled="true" TabIndex="30" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="EditEPlotS" runat="server">
                                        <Model>
                                            <ext:Model ID="EditEPlotM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_elementary_plot" />
                                                    <ext:ModelField Name="id_plot" />
                                                    <ext:ModelField Name="code_plot" />
                                                    <ext:ModelField Name="number_plot" />
                                                    <ext:ModelField Name="number_elementary_plot" />
                                                    <ext:ModelField Name="p2o5" />                                                          
                                                    <ext:ModelField Name="k2o" />
                                                    <ext:ModelField Name="ph_s" />
                                                    <ext:ModelField Name="ph_w" />
                                                    <ext:ModelField Name="hydrolytic_acid" />
                                                    <ext:ModelField Name="id_method" /> 
                                                    <ext:ModelField Name="title_method" />
                                                    <ext:ModelField Name="id_type_soil" />
                                                    <ext:ModelField Name="code_type_soil" />
                                                    <ext:ModelField Name="title_type_soil" />
                                                    <ext:ModelField Name="id_grading" />
                                                    <ext:ModelField Name="code_grading" />
                                                    <ext:ModelField Name="title_grading" />
                                                    <ext:ModelField Name="id_erosion" />
                                                    <ext:ModelField Name="code_erosion" />
                                                    <ext:ModelField Name="title_erosion" />
                                                    <ext:ModelField Name="id_slope" />
                                                    <ext:ModelField Name="code_slope" />                                                   
                                                    <ext:ModelField Name="title_slope" />
                                                    <ext:ModelField Name="id_exposure" />
                                                    <ext:ModelField Name="code_exposure" />
                                                    <ext:ModelField Name="title_exposure" />
                                                    <ext:ModelField Name="n" />
                                                    <ext:ModelField Name="no3" />
                                                    <ext:ModelField Name="no2" />                                                                                                              
                                                    <ext:ModelField Name="humus" />
                                                    <ext:ModelField Name="absorbance_capacity" />
                                                    <ext:ModelField Name="total_absorbed_bases" />
                                                    <ext:ModelField Name="base_saturation" />
                                                    <ext:ModelField Name="id_priority_calcification" />
                                                    <ext:ModelField Name="s" />
                                                    <ext:ModelField Name="ca" />         
                                                    <ext:ModelField Name="mn" />
                                                    <ext:ModelField Name="mo" />
                                                    <ext:ModelField Name="b" />
                                                    <ext:ModelField Name="cu" />
                                                    <ext:ModelField Name="mg" />
                                                    <ext:ModelField Name="zn" />
                                                    <ext:ModelField Name="na" />
                                                    <ext:ModelField Name="co" />
                                                    <ext:ModelField Name="al" />
                                                    <ext:ModelField Name="fe" />
                                                    <ext:ModelField Name="cu_hm" />
                                                    <ext:ModelField Name="zn_hm" />
                                                    <ext:ModelField Name="cd_hm" />
                                                    <ext:ModelField Name="pb_hm" />
                                                    <ext:ModelField Name="ni_hm" />
                                                    <ext:ModelField Name="hg_hm" />
                                                    <ext:ModelField Name="mg_hm" />
                                                    <ext:ModelField Name="cr_hm" />
                                                    <ext:ModelField Name="fe_hm" />
                                                    <ext:ModelField Name="f_hm" />
                                                    <ext:ModelField Name="as_hm" />
                                                    <ext:ModelField Name="cs137" />
                                                    <ext:ModelField Name="sr90" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="Column3" runat="server" DataIndex="id_type_soil" Hidden="true" />
                                        <ext:Column ID="Column4" runat="server" DataIndex="code_type_soil" Hidden="true" />
                                        <ext:Column ID="Column5" runat="server" DataIndex="title_type_soil" Hidden="true" />
                                        <ext:Column ID="Column6" runat="server" DataIndex="id_grading" Hidden="true" />
                                        <ext:Column ID="Column7" runat="server" DataIndex="code_grading" Hidden="true" />
                                        <ext:Column ID="Column8" runat="server" DataIndex="title_grading" Hidden="true" />
                                        <ext:Column ID="Column9" runat="server" DataIndex="id_erosion" Hidden="true" />
                                        <ext:Column ID="Column10" runat="server" DataIndex="code_erosion" Hidden="true" />
                                        <ext:Column ID="Column11" runat="server" DataIndex="title_erosion" Hidden="true" />
                                        <ext:Column ID="Column1" runat="server" DataIndex="id_slope" Hidden="true" />
                                        <ext:Column ID="Column2" runat="server" DataIndex="code_slope" Hidden="true" />
                                        <ext:Column ID="Column12" runat="server" DataIndex="title_slope" Hidden="true" />
                                        <ext:Column ID="Column13" runat="server" DataIndex="id_exposure" Hidden="true" />
                                        <ext:Column ID="Column14" runat="server" DataIndex="code_exposure" Hidden="true" />
                                        <ext:Column ID="Column15" runat="server" DataIndex="title_exposure" Hidden="true" />
                                        <ext:Column ID="Column16" runat="server" DataIndex="n" Hidden="true" />
                                        <ext:Column ID="Column20" runat="server" DataIndex="no3" Hidden="true" />
                                        <ext:Column ID="Column24" runat="server" DataIndex="no2" Hidden="true" />
                                        <ext:Column ID="Column28" runat="server" DataIndex="humus" Hidden="true" />
                                        <ext:Column ID="Column32" runat="server" DataIndex="absorbance_capacity" Hidden="true" />
                                        <ext:Column ID="Column36" runat="server" DataIndex="total_absorbed_bases" Hidden="true" />
                                        <ext:Column ID="Column40" runat="server" DataIndex="base_saturation" Hidden="true" />
                                        <ext:Column ID="Column44" runat="server" DataIndex="id_priority_calcification" Hidden="true" />
                                        <ext:Column ID="Column45" runat="server" DataIndex="s" Hidden="true" />
                                        <ext:Column ID="Column49" runat="server" DataIndex="ca" Hidden="true" />
                                        <ext:Column ID="Column53" runat="server" DataIndex="mn" Hidden="true" />
                                        <ext:Column ID="Column57" runat="server" DataIndex="mo" Hidden="true" />
                                        <ext:Column ID="Column61" runat="server" DataIndex="b" Hidden="true" />
                                        <ext:Column ID="Column65" runat="server" DataIndex="cu" Hidden="true" />
                                        <ext:Column ID="Column69" runat="server" DataIndex="mg" Hidden="true" />
                                        <ext:Column ID="Column73" runat="server" DataIndex="zn" Hidden="true" />
                                        <ext:Column ID="Column77" runat="server" DataIndex="na" Hidden="true" />
                                        <ext:Column ID="Column81" runat="server" DataIndex="co" Hidden="true" />
                                        <ext:Column ID="Column85" runat="server" DataIndex="al" Hidden="true" />
                                        <ext:Column ID="Column89" runat="server" DataIndex="fe" Hidden="true" />
                                        <ext:Column ID="Column93" runat="server" DataIndex="cu_hm" Hidden="true" />
                                        <ext:Column ID="Column97" runat="server" DataIndex="zn_hm" Hidden="true" />
                                        <ext:Column ID="Column101" runat="server" DataIndex="cd_hm" Hidden="true" />
                                        <ext:Column ID="Column105" runat="server" DataIndex="pb_hm" Hidden="true" />
                                        <ext:Column ID="Column109" runat="server" DataIndex="ni_hm" Hidden="true" />
                                        <ext:Column ID="Column113" runat="server" DataIndex="hg_hm" Hidden="true" />
                                        <ext:Column ID="Column117" runat="server" DataIndex="mg_hm" Hidden="true" />
                                        <ext:Column ID="Column121" runat="server" DataIndex="cr_hm" Hidden="true" />
                                        <ext:Column ID="Column125" runat="server" DataIndex="fe_hm" Hidden="true" />
                                        <ext:Column ID="Column129" runat="server" DataIndex="f_hm" Hidden="true" />
                                        <ext:Column ID="Column133" runat="server" DataIndex="as_hm" Hidden="true" />
                                        <ext:Column ID="Column137" runat="server" DataIndex="cs137" Hidden="true" />
                                        <ext:Column ID="Column141" runat="server" DataIndex="sr90" Hidden="true" />
                                        <ext:Column ID="Column145" runat="server" DataIndex="id_method" Hidden="true" />
                                        <ext:Column ID="Column152" runat="server" DataIndex="ph_w" Hidden="true" />
                                        <ext:Column ID="Column162" runat="server" DataIndex="number_plot" Hidden="true" />
                                        <ext:Column ID="Column163" runat="server" DataIndex="code_plot" Hidden="true" />
                                        <ext:Column ID="Column164" runat="server" DataIndex="id_elementary_plot" Hidden="true" />
                                        <ext:Column ID="Column165" runat="server" DataIndex="id_plot" Hidden="true" />
                                        <ext:Column ID="Column166" runat="server" Text="№ образца" DataIndex="number_elementary_plot" Width="70">
                                            <Editor>
                                                <ext:TextField ID="NumberEplotTF" runat="server" EnableKeyEvents="true" SelectOnFocus="true" />
                                            </Editor>
                                        </ext:Column>        
                                        <ext:Column ID="Column167" runat="server" Text="Hг" DataIndex="hydrolytic_acid" Width="40">
                                            <Editor>
                                                <ext:TextField ID="HydrolyticAcidEPlotTF" runat="server" EnableKeyEvents="true" SelectOnFocus="true" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="Column168" runat="server" Text="pH<sub>HCl</sub>" DataIndex="ph_s" Width="40">
                                            <Editor>
                                                <ext:TextField ID="PhSEPlotTF" runat="server" EnableKeyEvents="true" SelectOnFocus="true" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="Column169" runat="server" Text="P<sub>2</sub>O<sub>5</sub>" DataIndex="p2o5" Width="40">
                                            <Editor>
                                                <ext:TextField ID="P2O5EPlotTF" runat="server" EnableKeyEvents="true" SelectOnFocus="true" />
                                            </Editor>
                                        </ext:Column>   
                                        <ext:Column ID="Column170" runat="server" Text="K<sub>2</sub>O" DataIndex="k2o" Width="40">
                                            <Editor>
                                                <ext:TextField ID="K2OEPlotTF" runat="server" EnableKeyEvents="true" SelectOnFocus="true" />
                                            </Editor>
                                        </ext:Column>   
                                        <ext:Column ID="Column171" runat="server" Text="Метод" DataIndex="title_method" Width="70" />
                                        <ext:ImageCommandColumn ID="DeleteEPlotColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить образец" CommandName="DeleteEPlot" />
                                            </Commands>
                                        </ext:ImageCommandColumn>
                                    </Columns>
                                </ColumnModel>
                                <Plugins>
                                    <ext:CellEditing ID="CellEditing2" runat="server" ClicksToEdit="2" />
                                </Plugins>
                            </ext:GridPanel>
                        </Items>
                    </ext:Panel>
                </Items>
                <Buttons>
                    <ext:Button ID="AcceptAddPlotB" runat="server" Text="Добавить" Icon="Accept" TabIndex="39" Hidden="true" />
                    <ext:Button ID="AcceptEditPlotB" runat="server" Text="Сохранить" Icon="Disk" TabIndex="39" Hidden="true" />
                    <ext:Button ID="CancelEditPlotB" runat="server" Text="Отменить" Icon="Cancel" TabIndex="40" />
                </Buttons>
            </ext:Window>

            <ext:Window ID="CoordinatesW" runat="server" Title="Координаты" Width="351" Height="300" Hidden="true" Layout="BorderLayout" Modal="true" Icon="World" Resizable="false" ButtonAlign="Center" >
                <Items>
                   <ext:Panel ID="CoordinatesP" runat="server" Region="Center" Layout="FormLayout" Frame="true">
                        <Items>
                            <ext:GridPanel ID="CoordinatesGP" runat="server" Layout="FitLayout" Height="225">  
                                <TopBar>
                                    <ext:Toolbar ID="CoordinatesTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddCoordinatesB" runat="server" Icon="Add" Text="Добавить вручную"/>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                            <ext:Button ID="ImportCoordinatesB" runat="server" Icon="PageAdd" Text="Импорт из файла"/>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="CoordinatesS" runat="server">
                                        <Model>
                                            <ext:Model ID="CoordinatesM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_soil" />
                                                    <ext:ModelField Name="latitude" />
                                                    <ext:ModelField Name="longitude" />
                                                    <ext:ModelField Name="date" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                                                      
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="Coordinates_dot" runat="server" Text="Точка" DataIndex="id_soil" Width="40">
                                            <Editor>
                                                <ext:TextField ID="DotTF" runat="server" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="Coordinates_latitude" runat="server" Text="Широта" DataIndex="latitude"  Width="70">
                                            <Editor>
                                                <ext:TextField ID="LatitudeTF" runat="server" />
                                            </Editor>
                                        </ext:Column> 
                                        <ext:Column ID="Coordinates_longitude" runat="server" Text="Долгота" DataIndex="longitude"  Width="70">
                                            <Editor>
                                                <ext:TextField ID="LongitudeTF" runat="server" />
                                            </Editor>
                                        </ext:Column>  
                                        <ext:Column ID="Coordinates_date" runat="server" Text="Дата" DataIndex="date"  Width="110">
                                            <Editor>
                                                <ext:TextField ID="DateTF" runat="server" />
                                            </Editor>
                                        </ext:Column> 
                                        <ext:ImageCommandColumn ID="DeleteDotColumn" runat="server" Width="20" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить точку" CommandName="DeleteDot" />
                                            </Commands>
                                        </ext:ImageCommandColumn>  
                                    </Columns>            
                                </ColumnModel> 
                                <Plugins>
                                    <ext:CellEditing ID="CellEditing5" runat="server" ClicksToEdit="2" />
                                </Plugins>    
                            </ext:GridPanel> 
                        </Items>                    
                    </ext:Panel> 
                </Items>
                <Buttons>
                    <ext:Button ID="SaveCoordinatesB" runat="server" Text="Сохранить изменения" Icon="Disk" OnClientClick="#{EditFieldW}.close();" />
                    <ext:Button ID="CancelCoordinatesB" runat="server" Text="Отменить" Icon="Cancel" />
                </Buttons>
            </ext:Window>            

            <ext:Window ID="SoilW" runat="server" Title="Типы почв" Width="350" Height="600" Hidden="true" Collapsible="true" Layout="BorderLayout" Modal="false" >
                <Items>
                   <ext:Panel ID="SoilP" runat="server" Region="Center" Layout="FitLayout" Frame="true">
                        <Items>
                            <ext:GridPanel ID="SoilGP" runat="server" Layout="FitLayout" MultiSelect="false">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                <TopBar>
                                    <ext:Toolbar ID="SoilTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddSoilB" runat="server" Icon="Add" Text="Добавить" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="SoilS" runat="server">
                                        <Model>
                                            <ext:Model ID="SoilM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_type_soil"/>
                                                    <ext:ModelField Name="code_type_soil"/>
                                                    <ext:ModelField Name="title_type_soil"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                                                       
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="IdSoil" runat="server" DataIndex="id_type_soil" Hidden="true" />
                                        <ext:Column ID="CodeTypeSoil" runat="server" Text="Код" DataIndex="code_type_soil" Width="40">
                                            <Editor>
                                                <ext:TextField ID="CodeTypeSoilTF" runat="server" MaskRe="/[0-9]/" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="TitleSoil" runat="server" Text="Наименование" DataIndex="title_type_soil"  Width="240">
                                            <Editor>
                                                <ext:TextField ID="TitleSoilTF" runat="server" />
                                            </Editor>
                                        </ext:Column> 
                                        <ext:ImageCommandColumn ID="DeleteSoilColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить тип почв" CommandName="DeleteSoil" />
                                            </Commands>
                                        </ext:ImageCommandColumn>                                         
                                    </Columns>            
                                </ColumnModel>   
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing10" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins> 
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel> 
                </Items>
            </ext:Window>
            <ext:Window ID="CultureW" runat="server" Title="Культуры" Width="350" Height="600" Hidden="true" Collapsible="true" Layout="BorderLayout" Modal="false">
                <Items>
                    <ext:Panel ID="CultureP" runat="server" Region="Center" Layout="FitLayout" Frame="true">
                        <Items>
                            <ext:GridPanel ID="CultureGP" runat="server" Layout="FitLayout"> 
                                <TopBar>
                                    <ext:Toolbar ID="CultureTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddCultureB" runat="server" Icon="Add" Text="Добавить" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="CultureS" runat="server">
                                        <Model>
                                            <ext:Model ID="CultureM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_culture"/>
                                                    <ext:ModelField Name="code_culture"/>
                                                    <ext:ModelField Name="title_culture"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                                                        
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="IdCulture" runat="server" DataIndex="id_culture" Hidden="true" />
                                        <ext:Column ID="CodeCulture" runat="server" Text="Код" DataIndex="code_culture" Width="40">
                                            <Editor>
                                                <ext:TextField ID="CodeCultureTF" runat="server" MaskRe="/[0-9]/" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="TitleCulture" runat="server" Text="Наименование" DataIndex="title_culture"  Width="240">
                                            <Editor>
                                                <ext:TextField ID="TitleCultureTF" runat="server" />
                                            </Editor>
                                        </ext:Column>  
                                        <ext:ImageCommandColumn ID="DeleteCultureColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить тип культур" CommandName="DeleteCulture" />
                                            </Commands>
                                        </ext:ImageCommandColumn>                                         
                                    </Columns>            
                                </ColumnModel>   
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing9" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins> 
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel>
                </Items>
            </ext:Window> 
            <ext:Window ID="CropRotationW" runat="server" Title="Типы севооборота" Width="350" Height="600" Hidden="true" Collapsible="true" Layout="BorderLayout" Modal="false">
                <Items>
                    <ext:Panel ID="CropRotationP" runat="server" Region="Center" Layout="FitLayout" Frame="true">
                        <Items>
                            <ext:GridPanel ID="CropRotationGP" runat="server" Layout="FitLayout">                                                        
                                <TopBar>
                                    <ext:Toolbar ID="CropRotationTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddCropRotationB" runat="server" Icon="Add" Text="Добавить" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="CropRotationS" runat="server">
                                        <Model>
                                            <ext:Model ID="CropRotationM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_crop_rotation"/>
                                                    <ext:ModelField Name="code_crop_rotation"/>
                                                    <ext:ModelField Name="title_crop_rotation"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>       
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="IdCropRotation" runat="server" DataIndex="id_crop_rotation" Hidden="true" />
                                        <ext:Column ID="CodeCropRotation" runat="server" Text="Код" DataIndex="code_crop_rotation" Width="40">
                                            <Editor>
                                                <ext:TextField ID="CodeCropRotationTF" runat="server" MaskRe="/[0-9]/" />
                                            </Editor>
                                        </ext:Column> 
                                        <ext:Column ID="TitleCropRotation" runat="server" Text="Наименование" DataIndex="title_crop_rotation"  Width="240">
                                            <Editor>
                                                <ext:TextField ID="TitleCropRotationTF" runat="server" />
                                            </Editor>
                                        </ext:Column>  
                                        <ext:ImageCommandColumn ID="DeleteCropRotationColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить тип сев-та" CommandName="DeleteCropRotation" />
                                            </Commands>
                                        </ext:ImageCommandColumn>                                          
                                    </Columns>            
                                </ColumnModel>  
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing8" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins>  
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel>
                </Items>
            </ext:Window> 
            <ext:Window ID="FarmlandW" runat="server" Title="Типы сельхозугодий" Width="350" Height="600" Hidden="true" Collapsible="true" Layout="BorderLayout" Modal="false">
                <Items>
                    <ext:Panel ID="FarmlandP" runat="server" Region="Center" Layout="FitLayout" Frame="true">
                        <Items>
                            <ext:GridPanel ID="FarmlandGP" runat="server" Layout="FitLayout">                                                        
                                <TopBar>
                                    <ext:Toolbar ID="FarmlandTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddFarmlandB" runat="server" Icon="Add" Text="Добавить" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="FarmlandS" runat="server">
                                        <Model>
                                            <ext:Model ID="FarmlandM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_farmland"/>
                                                    <ext:ModelField Name="code_farmland"/>
                                                    <ext:ModelField Name="title_farmland"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>       
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="IdFarmland" runat="server" DataIndex="id_farmland" Hidden="true" />
                                        <ext:Column ID="CodeFarmland" runat="server" Text="Код" DataIndex="code_farmland" Width="40">
                                            <Editor>
                                                <ext:TextField ID="CodeFarmlandTF" runat="server" MaskRe="/[0-9]/" />
                                            </Editor>
                                        </ext:Column> 
                                        <ext:Column ID="TitleFarmland" runat="server" Text="Наименование" DataIndex="title_farmland"  Width="240">
                                            <Editor>
                                                <ext:TextField ID="TitleFarmlandTF" runat="server" />
                                            </Editor>
                                        </ext:Column>     
                                        <ext:ImageCommandColumn ID="DeleteFarmlandColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить тип с/х угодий" CommandName="DeleteFarmland" />
                                            </Commands>
                                        </ext:ImageCommandColumn>                                     
                                    </Columns>            
                                </ColumnModel>   
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing7" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins>   
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel>
                </Items>
            </ext:Window>   
            <ext:Window ID="ErosionW" runat="server" Title="Степени эродированности" Width="350" Height="600" Hidden="true" Collapsible="true" Layout="BorderLayout" Modal="false">
                <Items>
                    <ext:Panel ID="ErosionP" runat="server" Region="Center" Layout="FitLayout" Frame="true">
                        <Items>
                            <ext:GridPanel ID="ErosionGP" runat="server" Layout="FitLayout">                                                        
                                <TopBar>
                                    <ext:Toolbar ID="ErosionTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddErosionB" runat="server" Icon="Add" Text="Добавить" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="ErosionS" runat="server">
                                        <Model>
                                            <ext:Model ID="ErosionM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_erosion"/>
                                                    <ext:ModelField Name="code_erosion"/>
                                                    <ext:ModelField Name="title_erosion"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>       
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="IdErosion" runat="server" DataIndex="id_erosion" Hidden="true" />
                                        <ext:Column ID="CodeErosion" runat="server" Text="Код" DataIndex="code_erosion" Width="40">
                                            <Editor>
                                                <ext:TextField ID="CodeErosionTF" runat="server" MaskRe="/[0-9]/" />
                                            </Editor>
                                        </ext:Column> 
                                        <ext:Column ID="TitleErosion" runat="server" Text="Наименование" DataIndex="title_erosion"  Width="240">
                                            <Editor>
                                                <ext:TextField ID="TitleErosionTF" runat="server" />
                                            </Editor>
                                        </ext:Column> 
                                        <ext:ImageCommandColumn ID="DeleteErosionColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить ст.эрод." CommandName="DeleteErosion" />
                                            </Commands>
                                        </ext:ImageCommandColumn>                                         
                                    </Columns>            
                                </ColumnModel>  
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing6" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins> 
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel>
                </Items>
            </ext:Window>   
            <ext:Window ID="GradingW" runat="server" Title="Механический состав почвы" Width="350" Height="600" Hidden="true" Collapsible="true" Layout="BorderLayout" Modal="false">
                <Items>
                    <ext:Panel ID="GradingP" runat="server" Region="Center" Layout="FitLayout" Frame="true">
                        <Items>
                            <ext:GridPanel ID="GradingGP" runat="server" Layout="FitLayout">                                                        
                                <TopBar>
                                    <ext:Toolbar ID="GradingTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddGradingB" runat="server" Icon="Add" Text="Добавить" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="GradingS" runat="server">
                                        <Model>
                                            <ext:Model ID="GradingM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_grading"/>
                                                    <ext:ModelField Name="code_grading"/>
                                                    <ext:ModelField Name="title_grading"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>       
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="IdGrading" runat="server" DataIndex="id_grading" Hidden="true" />
                                        <ext:Column ID="CodeGrading" runat="server" Text="Код" DataIndex="code_grading" Width="40">
                                            <Editor>
                                                <ext:TextField ID="CodeGradingTF" runat="server" MaskRe="/[0-9]/" />
                                            </Editor>
                                        </ext:Column> 
                                        <ext:Column ID="TitleGrading" runat="server" Text="Наименование" DataIndex="title_grading"  Width="240">
                                            <Editor>
                                                <ext:TextField ID="TitleGradingTF" runat="server" />
                                            </Editor>
                                        </ext:Column>  
                                        <ext:ImageCommandColumn ID="DeleteGradingColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить мех.состав" CommandName="DeleteGrading" />
                                            </Commands>
                                        </ext:ImageCommandColumn>                                       
                                    </Columns>            
                                </ColumnModel>   
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing5" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins>   
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel>
                </Items>
            </ext:Window>
            <ext:Window ID="SlopeW" runat="server" Title="Уклон" Width="350" Height="600" Hidden="true" Collapsible="true" Layout="BorderLayout" Modal="false">
                <Items>
                    <ext:Panel ID="SlopeP" runat="server" Region="Center" Layout="FitLayout" Frame="true">
                        <Items>
                            <ext:GridPanel ID="SlopeGP" runat="server" Layout="FitLayout">                                                        
                                <TopBar>
                                    <ext:Toolbar ID="SlopeTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddSlopeB" runat="server" Icon="Add" Text="Добавить" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="SlopeS" runat="server">
                                        <Model>
                                            <ext:Model ID="SlopeM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_slope"/>
                                                    <ext:ModelField Name="code_slope"/>
                                                    <ext:ModelField Name="title_slope"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>      
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="IdSlope" runat="server" DataIndex="id_slope" Hidden="true" />
                                        <ext:Column ID="CodeSlope" runat="server" Text="Код" DataIndex="code_slope" Width="40">
                                            <Editor>
                                                <ext:TextField ID="CodeSlopeTF" runat="server" MaskRe="/[0-9]/" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="TitleSlope" runat="server" Text="Наименование" DataIndex="title_slope"  Width="240">
                                            <Editor>
                                                <ext:TextField ID="TitleSlopeTF" runat="server" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:ImageCommandColumn ID="DeleteSlopeColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить уклон" CommandName="DeleteExposure" />
                                            </Commands>
                                        </ext:ImageCommandColumn>                                       
                                    </Columns>            
                                </ColumnModel>   
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing2" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins>   
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel>
                </Items>
            </ext:Window>
            <ext:Window ID="ExposureW" runat="server" Title="Экспозиция" Width="350" Height="600" Hidden="true" Collapsible="true" Layout="BorderLayout" Modal="false">
                <Items>
                    <ext:Panel ID="ExposureP" runat="server" Region="Center" Layout="FitLayout" Frame="true">
                        <Items>
                            <ext:GridPanel ID="ExposureGP" runat="server" Layout="FitLayout">                                                        
                                <TopBar>
                                    <ext:Toolbar ID="ExposureTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddExposureB" runat="server" Icon="Add" Text="Добавить" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="ExposureS" runat="server">
                                        <Model>
                                            <ext:Model ID="ExposureM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_exposure"/>
                                                    <ext:ModelField Name="code_exposure"/>
                                                    <ext:ModelField Name="title_exposure"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>      
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="IdExposure" runat="server" DataIndex="id_exposure" Hidden="true" />
                                        <ext:Column ID="CodeExposure" runat="server" Text="Код" DataIndex="code_exposure" Width="40">
                                            <Editor>
                                                <ext:TextField ID="CodeExposureTF" runat="server" MaskRe="/[0-9]/" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="TitleExposure" runat="server" Text="Наименование" DataIndex="title_exposure"  Width="240">
                                            <Editor>
                                                <ext:TextField ID="TitleExposureTF" runat="server" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:ImageCommandColumn ID="DeleteExposureColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить экспозицию" CommandName="DeleteExposure" />
                                            </Commands>
                                        </ext:ImageCommandColumn>                                       
                                    </Columns>            
                                </ColumnModel>   
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing1" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins>   
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel>
                </Items>
            </ext:Window>
            
            <ext:Window ID="SignificativeW" runat="server" Title="Показатели" Icon="ChartBar" Width="1100" Height="600" Hidden="true" Collapsible="true" Layout="BorderLayout">
                <Items>
                    <ext:Panel ID="SignificativeP" runat="server" Title="Показатель" Region="Center" Frame="true" Layout="FitLayout">
                        <Items>
                            <ext:GridPanel ID="SignificativeGP" runat="server" Layout="FitLayout" MultiSelect="false">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                <TopBar>
                                    <ext:Toolbar ID="SignificativeTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddSignificativeB" runat="server" Icon="Add" Text="Добавить показатель" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>  
                                <Store>
                                    <ext:Store ID="SignificativeS" runat="server">
                                         <Model>
                                            <ext:Model ID="SignificativeM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_significative" />
                                                    <ext:ModelField Name="title_significative" />
                                                    <ext:ModelField Name="unit_significative" />
                                                    <ext:ModelField Name="name_significative" />
                                                    <ext:ModelField Name="min_value" />
                                                    <ext:ModelField Name="max_value" />
                                                    <ext:ModelField Name="number_of_digits" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                      
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="IdSignificative" runat="server" DataIndex="id_significative" Hidden="true" />
                                        <ext:Column ID="TitleSignificative" runat="server" Text="Наименование" DataIndex="title_significative" Width="210">
                                            <Editor>
                                                <ext:TextField ID="TitleSignificativeTF" runat="server" AllowBlank="false" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="UnitSignificative" runat="server" Text="Ед. изм." DataIndex="unit_significative"  Width="90">
                                            <Editor>
                                                <ext:TextField ID="UnitSignificativeTF" runat="server" AllowBlank="false" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="NameSignificative" runat="server" Text="Имя поля" DataIndex="name_significative"  Width="100">
                                            <Editor>
                                                <ext:TextField ID="NameSignificativeTF" runat="server" AllowBlank="false" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="MinValue" runat="server" DataIndex="min_value" Text="Мин."  Width="40">
                                            <Editor>
                                                <ext:TextField ID="MinValueTF" runat="server" AllowBlank="false" MaskRe="/[0-9.,]/" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="MaxValue" runat="server" DataIndex="max_value" Text="Макс." Width="40">
                                            <Editor>
                                                <ext:TextField ID="MaxValueTF" runat="server" AllowBlank="false" MaskRe="/[0-9.,]/" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="NumberOfDigits" runat="server" DataIndex="number_of_digits" Text="Зн-в"  Width="40">
                                            <Editor>
                                                <ext:TextField ID="NumberOfDigitsTF" runat="server" AllowBlank="false" MaskRe="/[0-9]/" />
                                            </Editor>
                                        </ext:Column>                                       
                                        <ext:ImageCommandColumn ID="DeleteSignificativeColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить показатель" CommandName="DeleteSignificative" />
                                            </Commands>
                                        </ext:ImageCommandColumn> 
                                    </Columns>            
                                </ColumnModel>
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing3" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins>                                   
                            </ext:GridPanel>
                        </Items>
                    </ext:Panel>                                        
                    <ext:Panel ID="GroupsP" runat="server" Title="Группы" Region="East" Split="true" Width="500" Frame="true" Layout="FitLayout">
                        <Items>
                            <ext:GridPanel ID="GroupsGP" runat="server" Layout="FitLayout">
                                <TopBar>
                                    <ext:Toolbar ID="GroupsTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddGroupsB" runat="server" Icon="Add" Text="Добавить группу" Hidden="true" />
                                            <ext:Button ID="UpdateGroupsBySignificativeB" runat="server" Icon="Reload" Text="Пересчитать группы в БД" Hidden="true" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>      
                                <Store>
                                    <ext:Store ID="GroupsS" runat="server">
                                         <Model>
                                            <ext:Model ID="GroupsM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_group" />
                                                    <ext:ModelField Name="number_group" />
                                                    <ext:ModelField Name="id_significative" />
                                                    <ext:ModelField Name="title_method" />
                                                    <ext:ModelField Name="title_group" />
                                                    <ext:ModelField Name="from_group" />
                                                    <ext:ModelField Name="to_group" />
                                                    <ext:ModelField Name="coefficient" />
                                                    <ext:ModelField Name="color" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                                  
                                <ColumnModel>
                                    <Columns>                                        
                                        <ext:Column ID="IdGroup" runat="server" DataIndex="id_group" Hidden="true" />
                                        <ext:Column ID="IdSignificativeGroup" runat="server" DataIndex="id_significative" Hidden="true" />
                                        <ext:Column ID="NumberGroup" runat="server" Text="№ гр." DataIndex="number_group" Width="40">
                                            <Editor>
                                                <ext:TextField ID="NumberGroupTF" runat="server" AllowBlank="false" MaskRe="/[0-9]/" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="TitleGroup" runat="server" Text="Наименование" DataIndex="title_group"  Width="105">
                                            <Editor>
                                                <ext:TextField ID="TitleGroupTF" runat="server" AllowBlank="false" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="MethodGroup" runat="server" Text="Метод" DataIndex="title_method"  Width="120">
                                            <Editor>
                                                <ext:ComboBox ID="MethodGroupCB" runat="server" Editable="false" DisplayField="title_method" ValueField="title_method" >
                                                    <Store>
                                                        <ext:Store ID="MethodGroupS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="MethodGroupM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_method" />
                                                                        <ext:ModelField Name="title_method" />
                                                                        <ext:ModelField Name="from_pH" />
                                                                        <ext:ModelField Name="to_pH" />
                                                                        <ext:ModelField Name="condition" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="FromGroup" runat="server" Text="От" DataIndex="from_group"  Width="40">
                                            <Editor>
                                                <ext:TextField ID="FromGroupTF" runat="server" AllowBlank="false" MaskRe="/[0-9.,]/" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="ToGroup" runat="server" Text="До" DataIndex="to_group"  Width="40">
                                            <Editor>
                                                <ext:TextField ID="ToGroupTF" runat="server" AllowBlank="false" MaskRe="/[0-9.,]/" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="CoefficientGroup" runat="server" Text="Коэфф." DataIndex="coefficient"  Width="48">
                                            <Editor>
                                                <ext:TextField ID="CoefficientGroupTF" runat="server" AllowBlank="false" MaskRe="/[0-9.,]/" />
                                            </Editor>
                                        </ext:Column>  
                                        <ext:Column ID="ColorGroup" runat="server" Text="Цвет" DataIndex="color"  Width="50">
                                            <Editor>
                                                <ext:TextField ID="ColorGroupTF" runat="server" MinLength="6" MinLengthText="RGB код цвета должен быть 6 символов в 16-ой с.с." MaxLength="6" MaxLengthText="RGB код цвета должен быть 6 символов в 16-ой с.с." MaskRe="/[0-9A-F]/" />
                                            </Editor>
                                        </ext:Column>                                                                           
                                        <ext:ImageCommandColumn ID="DeleteGroupColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить группу" CommandName="DeleteGroup" />
                                            </Commands>
                                        </ext:ImageCommandColumn> 
                                    </Columns>            
                                </ColumnModel> 
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing4" runat="server" ClicksToMoveEditor ="1" AutoCancel="false" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins>    
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel>                    
                </Items>
            </ext:Window>
            <ext:Window ID="UserW" runat="server" Icon="User" Title="Пользователи" Width="1100" Height="400" Hidden="true" Collapsible="true" Layout="BorderLayout">
                <Items>
                    <ext:Panel ID="RoleP" runat="server" Title="Полномочия" Region="Center" Frame="true" Layout="FitLayout">
                        <Items>
                            <ext:GridPanel ID="RoleGP" runat="server" Layout="FitLayout" MultiSelect="false">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                <TopBar>
                                    <ext:Toolbar ID="RoleTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddRoleB" runat="server" Icon="Add" Text="Добавить роль" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar> 
                                <Store>
                                    <ext:Store ID="RoleS" runat="server">
                                        <Model>
                                            <ext:Model ID="RoleM" runat="server" Name="Employee">
                                                <Fields>
                                                    <ext:ModelField Name="id_role" />
                                                    <ext:ModelField Name="title_role" />
                                                    <ext:ModelField Name="read_role" />
                                                    <ext:ModelField Name="edit_role" />
                                                    <ext:ModelField Name="add_role" />
                                                    <ext:ModelField Name="delete_role" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                        
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="IdRole" runat="server" DataIndex="id_role" Hidden="true" />
                                        <ext:Column ID="TitleRole" runat="server" Text="Роль" DataIndex="title_role"  Width="110">
                                            <Editor>
                                                <ext:TextField ID="TitleRoleTF" runat="server" AllowBlank="false" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:CheckColumn ID="ReadRole" runat="server" Text="Чт." DataIndex="read_role" Width="40">
                                            <Editor>
                                                <ext:Checkbox ID="ReadRoleCB" runat="server" />
                                            </Editor>
                                        </ext:CheckColumn>
                                        <ext:CheckColumn ID="EditRole" runat="server" Text="Ред." DataIndex="edit_role"  Width="40">
                                            <Editor>
                                                <ext:Checkbox ID="EditRoleCB" runat="server" />
                                            </Editor>
                                        </ext:CheckColumn>
                                        <ext:CheckColumn ID="AddRole" runat="server" Text="Доб." DataIndex="add_role"  Width="40">
                                            <Editor>
                                                <ext:Checkbox ID="AddRoleCB" runat="server" />
                                            </Editor>
                                        </ext:CheckColumn>
                                        <ext:CheckColumn ID="DelRole" runat="server" Text="Уд."  DataIndex="delete_role" Width="40">
                                            <Editor>
                                                <ext:Checkbox ID="DelRoleCB" runat="server" />
                                            </Editor>
                                        </ext:CheckColumn>                                       
                                        <ext:ImageCommandColumn ID="DeleteRoleColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить роль" CommandName="DeleteRole" />
                                            </Commands>
                                        </ext:ImageCommandColumn> 
                                    </Columns>
                                </ColumnModel>
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing11" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins>
                            </ext:GridPanel>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="UserP" runat="server" Title="Сотрудники" Region="East" Width="760" Frame="true" Layout="FitLayout" Split="true">
                        <Items>
                            <ext:GridPanel ID="UserGP" runat="server" Layout="FitLayout" MultiSelect="false">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                <TopBar>
                                    <ext:Toolbar ID="UserTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddUserB" runat="server" Icon="UserAdd" Text="Добавить сотрудника" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="UserS" runat="server">
                                         <Model>
                                            <ext:Model ID="UserM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_user" />
                                                    <ext:ModelField Name="id_division" />
                                                    <ext:ModelField Name="title_division" />
                                                    <ext:ModelField Name="id_role" />
                                                    <ext:ModelField Name="surname" />
                                                    <ext:ModelField Name="name" />
                                                    <ext:ModelField Name="patronymic" />
                                                    <ext:ModelField Name="id_job_title" />
                                                    <ext:ModelField Name="title_job_title" />
                                                    <ext:ModelField Name="login" />
                                                    <ext:ModelField Name="password" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>         
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="IdUser" runat="server" DataIndex="id_user" Hidden="true" />
                                        <ext:Column ID="IdRoleUser" runat="server" DataIndex="id_role" Hidden="true" />
                                        <ext:Column ID="SurnameUser" runat="server" Text="Фамилия" DataIndex="surname"  Width="120">
                                            <Editor>
                                                <ext:TextField ID="SurnameUserTF" runat="server" AllowBlank="false" MaskRe="/[а-яА-Я]/" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="NameUser" runat="server" Text="Имя" DataIndex="name"  Width="90">
                                            <Editor>
                                                <ext:TextField ID="NameUserTF" runat="server" AllowBlank="false" MaskRe="/[а-яА-Я]/" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="PatronymicUser" runat="server" Text="Отчество" DataIndex="patronymic"  Width="100">
                                            <Editor>
                                                <ext:TextField ID="PatronymicUserTF" runat="server" AllowBlank="false" MaskRe="/[а-яА-Я]/" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="DivisionUser" runat="server" Text="Отдел" DataIndex="title_division"  Width="100">
                                            <Editor>
                                                <ext:ComboBox ID="DivisionUserCB" runat="server" DisplayField="title_division" ValueField="title_division" Editable="false"> 
                                                    <Store>
                                                        <ext:Store ID="DivisionUserS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="DivisionUserM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_division" />
                                                                        <ext:ModelField Name="title_division" />
                                                                        <ext:ModelField Name="from_division" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <ListConfig  LoadingText="Загрузка..." MinWidth="400" Border="true">
                                                        <ItemTpl ID="ItemTpl13" runat="server">
                                                            <Html><div class="search-item">{title_division}</div></Html>
                                                         </ItemTpl>
                                                    </ListConfig>
                                                </ext:ComboBox> 
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="TitleJobTitleUser" runat="server" Text="Должность" DataIndex="title_job_title"  Width="100">
                                            <Editor>
                                                <ext:ComboBox ID="JobTitleUserCB" runat="server" DisplayField="title_job_title" ValueField="title_job_title" Editable="false"> 
                                                    <Store>
                                                        <ext:Store ID="JobTitleUserS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="JobTitleUserM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_job_title" />
                                                                        <ext:ModelField Name="title_job_title" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <ListConfig  LoadingText="Загрузка..." MinWidth="300" Border="true">
                                                        <ItemTpl ID="ItemTpl2" runat="server">
                                                            <Html><div class="search-item">{title_job_title}</div></Html>
                                                         </ItemTpl>
                                                    </ListConfig>
                                                </ext:ComboBox> 
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="LoginUser" runat="server" Text="Логин" DataIndex="login"  Width="100">
                                            <Editor>
                                                <ext:TextField ID="LoginUserTF" runat="server" AllowBlank="false" MaskRe="/[a-zA-Z0-9_-]/" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="PasswordUser" runat="server" Text="Пароль" DataIndex="password" Width="100">
                                            <Editor>
                                                <ext:TextField ID="PasswordUserTF" runat="server" AllowBlank="false" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:ImageCommandColumn ID="DeleteUserColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить сотрудника" CommandName="DeleteUser" />
                                            </Commands>
                                        </ext:ImageCommandColumn>
                                    </Columns>
                                </ColumnModel>
                                <Plugins>
                                    <ext:RowEditing ID="RowEditingUser" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins>
                            </ext:GridPanel>
                        </Items>
                    </ext:Panel>  
                </Items>                
            </ext:Window>
            <ext:Window ID="JobTitleW" runat="server" Title="Должности" Width="700" Height="600" Hidden="true" Collapsible="true" Layout="BorderLayout" Modal="false">
                <Items>
                   <ext:Panel ID="JobTitleP" runat="server" Region="Center" Layout="FitLayout" Frame="true">
                        <Items>
                            <ext:GridPanel ID="JobTitleGP" runat="server" Layout="FitLayout" MultiSelect="false">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                <TopBar>
                                    <ext:Toolbar ID="JobTitleTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddJobTitleB" runat="server" Icon="Add" Text="Добавить" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="JobTitleS" runat="server">
                                        <Model>
                                            <ext:Model ID="JobTitleM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_job_title"/>
                                                    <ext:ModelField Name="title_job_title"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                                                       
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="IdJobTitle" runat="server" DataIndex="id_job_title" Hidden="true" />
                                        <ext:Column ID="TitleJobTitle" runat="server" Text="Наименование" DataIndex="title_job_title" Width="630">
                                            <Editor>
                                                <ext:TextField ID="TitleJobTitleTF" runat="server" />
                                            </Editor>
                                        </ext:Column>                                        
                                        <ext:ImageCommandColumn ID="DeleteJobTitleColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить должность" CommandName="DeleteJobTitle" />
                                            </Commands>
                                        </ext:ImageCommandColumn>                                         
                                    </Columns>            
                                </ColumnModel>   
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing13" runat="server" ClicksToMoveEditor="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins> 
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel> 
                </Items>
            </ext:Window>
            <ext:Window ID="MissionW" runat="server" Title="Задания" Width="700" Height="600" Hidden="true" Collapsible="true" Layout="BorderLayout" Modal="false">
                <Items>
                   <ext:Panel ID="MissionP" runat="server" Region="Center" Layout="FitLayout" Frame="true">
                        <Items>
                            <ext:GridPanel ID="MissionGP" runat="server" Layout="FitLayout" MultiSelect="false">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                <TopBar>
                                    <ext:Toolbar ID="MissionTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddMissionB" runat="server" Icon="Add" Text="Добавить" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="MissionS" runat="server">
                                        <Model>
                                            <ext:Model ID="MissionM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_mission"/>
                                                    <ext:ModelField Name="title_mission"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                                                       
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="IdMission" runat="server" DataIndex="id_mission" Hidden="true" />
                                        <ext:Column ID="TitleMission" runat="server" Text="Задание" DataIndex="title_mission" Width="630">
                                            <Editor>
                                                <ext:TextField ID="TitleMissionTF" runat="server" />
                                            </Editor>
                                        </ext:Column>                                         
                                        <ext:ImageCommandColumn ID="DeleteMissionColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить задание" CommandName="DeleteMission" />
                                            </Commands>
                                        </ext:ImageCommandColumn>                                         
                                    </Columns>            
                                </ColumnModel>   
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing14" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins> 
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel> 
                </Items>
            </ext:Window>
            <ext:Window ID="TrackersW" runat="server" Title="Трекеры" Width="200" Height="300" Hidden="true" Collapsible="true" Layout="BorderLayout" Modal="false">
                <Items>
                   <ext:Panel ID="TrackersP" runat="server" Region="Center" Layout="FitLayout" Frame="true">
                        <Items>
                            <ext:GridPanel ID="TrackersGP" runat="server" Layout="FitLayout" MultiSelect="false">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                <TopBar>
                                    <ext:Toolbar ID="TrackersTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddTrackerB" runat="server" Icon="Add" Text="Добавить" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="TrackersS" runat="server">
                                        <Model>
                                            <ext:Model ID="TrackersM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_gps_tracker"/>
                                                    <ext:ModelField Name="imei"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                                                       
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="IdGpsTrackerC" runat="server" DataIndex="id_gps_tracker" Hidden="true" />
                                        <ext:Column ID="IMEITrackerC" runat="server" Text="IMEI трекера" DataIndex="imei" Width="140">
                                            <Editor>
                                                <ext:TextField ID="EditIMEITrackerTF" runat="server" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:ImageCommandColumn ID="DeleteTrackerColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить трекер" CommandName="DeleteTracker" />
                                            </Commands>
                                        </ext:ImageCommandColumn>
                                    </Columns>            
                                </ColumnModel>   
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing12" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins> 
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel> 
                </Items>
            </ext:Window>

            <ext:Window ID="CarsW" runat="server" Title="Автомобили" Width="600" Height="500" Hidden="true" Collapsible="true" Layout="FitLayout" Modal="false">
                <Items>
                   <ext:Panel ID="Panel5" runat="server" Region="Center" Layout="VBoxLayout" Frame="true" Flex="1">
                       <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                        </LayoutConfig>
                        <Items>
                            <ext:ComboBox ID="CarsTerritoryCB" runat="server" FieldLabel="Область" LabelCls="darkslateblue-note" LabelWidth="70" DisplayField="title_territory" ValueField="id_territory" Editable="false">
                                 <Store>
                                     <ext:Store ID="CarsTerritoryS" runat="server">
                                         <Model>
                                             <ext:Model ID="CarsTerritoryM" runat="server">
                                                 <Fields>
                                                     <ext:ModelField Name="id_territory" />
                                                     <ext:ModelField Name="title_territory" />
                                                 </Fields>
                                             </ext:Model>
                                         </Model>
                                     </ext:Store>
                                 </Store>
                            </ext:ComboBox>
                            <ext:ComboBox ID="CarsRegionCB" runat="server" FieldLabel="Район" LabelCls="darkslateblue-note" LabelWidth="70" DisplayField="title_region" ValueField="id_region" Editable="false">
                                 <Store>
                                     <ext:Store ID="CarsRegionS" runat="server">
                                         <Model>
                                             <ext:Model ID="CarsRegionM" runat="server">
                                                 <Fields>
                                                     <ext:ModelField Name="id_region" />
                                                     <ext:ModelField Name="title_region" />
                                                 </Fields>
                                             </ext:Model>
                                         </Model>
                                     </ext:Store>
                                 </Store>
                            </ext:ComboBox>
                            <ext:GridPanel ID="CarsGP" runat="server" Layout="FitLayout" MultiSelect="false" Flex="1">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                <TopBar>
                                    <ext:Toolbar ID="CarsTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddCarB" runat="server" Icon="Add" Text="Добавить" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="CarsS" runat="server">
                                        <Model>
                                            <ext:Model ID="Model5" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_car"/>
                                                    <ext:ModelField Name="car_model"/>
                                                    <ext:ModelField Name="license_plate"/>
                                                    <ext:ModelField Name="imei"/>
                                                    <ext:ModelField Name="title_organization"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="IdCarsC" runat="server" DataIndex="id_car" Hidden="true" />
                                        <ext:Column ID="CarsOrganizationC" runat="server" Text="Организация" DataIndex="title_organization"  Width="100">
                                            <Editor>
                                                <ext:ComboBox ID="SelectCarsOrganizationCB" runat="server" DisplayField="title_organization" ValueField="title_organization" Editable="false"> 
                                                    <Store>
                                                        <ext:Store ID="CarsOrganizationS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="CarsOrganizationM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_organization" />
                                                                        <ext:ModelField Name="code_organization" />
                                                                        <ext:ModelField Name="title_organization" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <ListConfig  LoadingText="Загрузка..." MinWidth="300" Border="true">
                                                        <ItemTpl ID="CarsOrganizationIT" runat="server">
                                                            <Html><div class="search-item">{title_organization}</div></Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                </ext:ComboBox> 
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="CarsModelS" runat="server" Text="Марка автомобиля" DataIndex="car_model" Width="140">
                                            <Editor>
                                                <ext:TextField ID="EditCarsModeTF" runat="server" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="CarsLicensePlateC" runat="server" Text="Гос. номер" DataIndex="license_plate" Width="140">
                                            <Editor>
                                                <ext:TextField ID="EditCarsLicensePlateTF" runat="server" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="CarsIMEITrackerC" runat="server" Text="IMEI трекера" DataIndex="imei" Width="140">
                                            <Editor>
                                                <ext:ComboBox ID="EditCarsIMEITrackerCB" runat="server" DisplayField="imei" ValueField="imei" Editable="false"> 
                                                    <Store>
                                                        <ext:Store ID="CarsIMEITrackerS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="CarsIMEITrackerM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_gps_tracker" />
                                                                        <ext:ModelField Name="imei" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <ListConfig  LoadingText="Загрузка..." MinWidth="300" Border="true">
                                                        <ItemTpl ID="ItemTpl12" runat="server">
                                                            <Html><div class="search-item">{imei}</div></Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                </ext:ComboBox> 
                                            </Editor>
                                        </ext:Column>
                                        <ext:ImageCommandColumn ID="DeleteCarColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить автомобиль" CommandName="DeleteCar" />
                                            </Commands>
                                        </ext:ImageCommandColumn>
                                    </Columns>            
                                </ColumnModel>   
                                <Plugins>
                                    <ext:RowEditing ID="RowEditing15" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins> 
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel> 
                </Items>
            </ext:Window>

            <ext:Window ID="ReportToursW" runat="server" Title="Выбор цикла" Closable="true" Resizable="false" Height="100" Icon="Mouse" Width="200" Modal="true" BodyPadding="5" Layout="Form" ButtonAlign="Center" Hidden="true">
                <Items>
                    <ext:SelectBox
                        ID="ReportToursSB"
                        runat="server" 
                        DisplayField="tour"
                        ValueField="tour"
                        EmptyText="Выберите цикл!"
                        >
                        <Store>
                            <ext:Store ID="ReportToursS" runat="server">
                                <Model>
                                    <ext:Model ID="ReportToursM" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="tour" />
                                        </Fields>
                                    </ext:Model>
                                </Model>            
                            </ext:Store>    
                        </Store>  
                    </ext:SelectBox>
                </Items>
                <Buttons> 
                    <ext:Button ID="ReportToursAcceptB" runat="server" Text="Продолжить" Icon="Accept" Hidden="true" />
                    <ext:Button ID="ReportToursCancelB" runat="server" Text="Отмена" Icon="Cancel" />
                </Buttons>
            </ext:Window>

            <ext:Window ID="CopyMovePlotW" runat="server" Title="Копирование / перемещение участков" Closable="true" Resizable="false" Height="250" Icon="ArrowSwitchBluegreen" Width="340" Modal="true" BodyPadding="5" Layout="Form" ButtonAlign="Center" Hidden="true">
                <Items>
                    <ext:Label ID="FromL" runat="server" Text="Откуда:"/>
                    <ext:FieldContainer ID="FieldContainer6" runat="server" Layout="HBoxLayout" >
                        <Items>
                            <ext:Component ID="Component15" runat="server" Width="10" />
                            <ext:TextField ID="CodeDepartmentFromTF" runat="server" Note="Код отд." NoteAlign="Top" Width="200" TabIndex="1" MaskRe="/[0-9]/" />
                            <ext:Component ID="Component62" runat="server" Width="5" />
                            <ext:TextField ID="TourFromTF" runat="server" Note="Цикл" NoteAlign="Top" Width="40" TabIndex="2" MaskRe="/[0-9]/" />
                            <ext:Component ID="Component63" runat="server" Width="5" />
                            <ext:TextField ID="YearFromTF" runat="server" Note="Год" NoteAlign="Top" Width="50" TabIndex="3" MaskRe="/[0-9]/" />
                            <ext:Component ID="Component64" runat="server" Width="5" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:Label ID="ToL" runat="server" Text="Куда:"/>
                    <ext:FieldContainer ID="FieldContainer7" runat="server" Layout="HBoxLayout" >
                        <Items>
                            <ext:Component ID="Component66" runat="server" Width="10" />
                            <ext:TextField ID="CodeDepartmentToTF" runat="server" Note="Код отд." NoteAlign="Top" Width="200" TabIndex="4" MaskRe="/[0-9]/" />
                            <ext:Component ID="Component67" runat="server" Width="5" />
                            <ext:TextField ID="TourToTF" runat="server" Note="Цикл" NoteAlign="Top" Width="40" TabIndex="5" MaskRe="/[0-9]/" />
                            <ext:Component ID="Component68" runat="server" Width="5" />
                            <ext:TextField ID="YearToTF" runat="server" Note="Год" NoteAlign="Top" Width="50" TabIndex="6" MaskRe="/[0-9]/" />
                        </Items>
                    </ext:FieldContainer>
                    <ext:Label ID="CopyMovePlotsL" runat="server" Text="Участки:"/>
                    <ext:FieldContainer ID="FieldContainer8" runat="server" Layout="HBoxLayout">
                        <Items>
                            <ext:Component ID="Component65" runat="server" Width="10" />
                            <ext:TextField ID="CopyMovePlotsTF" runat="server" Width="300" TabIndex="7" MaskRe="/[0-9.,]/" />
                        </Items>
                    </ext:FieldContainer>
                </Items>
                <Buttons>
                    <ext:Button ID="MovePlotsB" runat="server" Text="Вырезать" Icon="Cut" TabIndex="8" />
                    <ext:Button ID="CopyPlotsB" runat="server" Text="Копировать" Icon="DatabaseCopy" TabIndex="9" />
                    <ext:Button ID="CancelCopyMovePlotsB" runat="server" Text="Отмена" Icon="Cancel" />
                </Buttons>
            </ext:Window>

            <ext:Window ID="ConfirmDeleteOrganizationW" runat="server" Title="Удаление организации" Closable="true" Resizable="false" Height="120" Icon="Delete" Width="360" Modal="true" BodyPadding="5" Layout="Form" ButtonAlign="Center" Hidden="true">
                <Items>
                    <ext:FieldContainer ID="FieldContainer9" runat="server" Layout="HBoxLayout">
                        <Items>
                            <ext:Component ID="Component69" runat="server" Width="10" />
                            <ext:Label ID="MessageL" runat="server" Text="Для подтверждения удаления введите слово: Удалить"/>
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer ID="FieldContainer11" runat="server" Layout="HBoxLayout">
                        <Items>
                            <ext:Component ID="Component76" runat="server" Width="10" />
                            <ext:TextField ID="ConfirmTF" runat="server" Width="320" />
                            <ext:TextField ID="IdDeleteOrganizationTF" runat="server" Width="50" Hidden="true" />
                        </Items>
                    </ext:FieldContainer>
                </Items>
                <Buttons>
                    <ext:Button ID="ConfirmDeleteOrganizationB" runat="server" Text="Удалить" Icon="Delete" />
                    <ext:Button ID="CancelDeleteOrganizationB" runat="server" Text="Отмена" Icon="Cancel" />
                </Buttons>
            </ext:Window>

            <ext:Window ID="AnalysPhSW" runat="server" Title="Степень кислотности" Width="770" Height="700"  Modal="true" Hidden="true" Layout="VBoxLayout" Resizable="False" ButtonAlign="Center">
                    <LayoutConfig>
                        <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                    </LayoutConfig>
                    <Items>
                        <ext:Panel ID="AnalysPhSP1" runat="server" Frame="true" Layout="FormLayout">
                           <Items>
                               <ext:TextField ID="AnalysPhSRegionTF" runat="server" FieldLabel="Район" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                               <ext:TextField ID="AnalysPhSOrganizationTF" runat="server" FieldLabel="Хозяйство" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                               <ext:TextField ID="AnalysPhSDepartmentTF" runat="server" FieldLabel="Отделение" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                           </Items>                                          
                        </ext:Panel>
                        <ext:Panel ID="AnalysPhSP2" runat="server" Frame="true" Layout="HBoxLayout">  
                            <LayoutConfig>
                                <ext:HBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                            </LayoutConfig>                     
                            <Items>
                                 <ext:ComboBox ID="AnalysPhSTourCB" runat="server" FieldLabel="Цикл" LabelCls="darkslateblue-note" LabelWidth="40" DisplayField="tour" ValueField="tour" TabIndex="1" Flex="2" MaskRe="/[0-9]/">
                                    <Store>
                                        <ext:Store ID="AnalysPhSTourS" runat="server">
                                            <Model>
                                                <ext:Model ID="AnalysPhSTourM" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="tour" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>
                                <ext:Component ID="Component70" runat="server" Width="5" />  
                                <ext:ComboBox ID="AnalysPhSYearCB" runat="server" FieldLabel="Год" LabelCls="darkslateblue-note" LabelWidth="40" DisplayField="year" ValueField="year" TabIndex="2" Flex="2" MaskRe="/[0-9]/">
                                    <Store>
                                        <ext:Store ID="AnalysPhSYearS" runat="server">
                                            <Model>
                                                <ext:Model ID="AnalysPhSYearM" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="year" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>        
                                <ext:TextField ID="AnalysPhSIdSampleTF" runat="server" Hidden="true" />                
                                <ext:Component ID="Component71" runat="server" Width="5" />                                                    
                                <ext:TextField ID="AnalysPhSNumberSampleTF" runat="server" FieldLabel="№ образца" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="3" Flex="2" EnableKeyEvents="true" />
                                <ext:Component ID="Component72" runat="server" Width="5" /> 
                                <ext:TextField ID="AnalysPhSValuePhSTF" runat="server" FieldLabel="PH" LabelSeparator="" LabelWidth="20" LabelCls="darkslateblue-note" TabIndex="4" Flex="1" EnableKeyEvents="true" MaskRe="/[0-9.,]/" />
                                <ext:Component ID="Component81" runat="server" Width="5" /> 
                                <ext:TextField ID="AnalysPhSControlTF" runat="server" FieldLabel="Контроль" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="4" Flex="2" EnableKeyEvents="true" MaskRe="/[0-9.,]/" />
                            </Items>                                          
                        </ext:Panel>
                        <ext:Panel ID="AnalysPhSP3" runat="server" Region="Center" Layout="FitLayout" Frame="true"  AutoScroll="true">
                            <Items>
                                <ext:GridPanel ID="AnalysPhSGP" runat="server" Layout="FitLayout" MultiSelect="false" AutoScroll="true" Height="523" InvalidateScrollerOnRefresh="False">
                                    <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>   
                                    <TopBar>
                                        <ext:Toolbar ID="AnalysPhSTB" runat="server">
                                            <Items>
                                                <ext:TextField ID="AnalysPhSFewTF" runat="server" EnableKeyEvents="true" Flex="1" />   
                                                <ext:Button ID="AnalysPhSAddFewB" runat="server" Icon="Add" Text="Добавить несколько" />
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>                            
                                    <Store>
                                        <ext:Store ID="AnalysPhSS" runat="server">
                                            <Model>
                                                <ext:Model ID="AnalysPhSM" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="id_sample"/>
                                                        <ext:ModelField Name="id_ph"/>
                                                        <ext:ModelField Name="number_sample"/>
                                                        <ext:ModelField Name="ph_s_value"/>
                                                        <ext:ModelField Name="control_value"/>
                                                        <ext:ModelField Name="date_input"/>
                                                        <ext:ModelField Name="date_last_edit"/>
                                                        <ext:ModelField Name="user_editor"/>
                                                        <ext:ModelField Name="tour"/>
                                                        <ext:ModelField Name="year"/>
                                                        <ext:ModelField Name="id_method"/>
                                                        <ext:ModelField Name="title_method"/>
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>                                                       
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column ID="AnalysPhSIdSample" runat="server" DataIndex="id_sample" Hidden="true" Text="IdSample" />
                                            <ext:Column ID="AnalysPhSIdMethod" runat="server" DataIndex="id_method" Hidden="true" Text="IdMethod" />
                                            <ext:Column ID="AnalysPhSIdPhS" runat="server" DataIndex="id_ph" Hidden="true" Text="IdPh" />
                                            <ext:Column ID="AnalysPhSTour" runat="server" DataIndex="tour" Hidden="true" Text="Цикл" />
                                            <ext:Column ID="AnalysPhSYear" runat="server" DataIndex="year" Hidden="true" Text="Год" />
                                            <ext:Column ID="AnalysPhSNumberSample" runat="server" Text="№ <br>обр." DataIndex="number_sample" Width="30" />
                                            <ext:Column ID="AnalysPhSValue" runat="server" Text="pH" DataIndex="ph_s_value"  Width="40" />
                                            <ext:Column ID="AnalysPhSControlValue" runat="server" Text="Контроль" DataIndex="control_value"  Width="70" />
                                            <ext:Column ID="AnalysPhSMethod" runat="server" Text="Метод" DataIndex="title_method"  Width="60" />
                                            <ext:Column ID="AnalysPhSDateInput" runat="server" Text="Дата ввода" DataIndex="date_input"  Width="150" />
                                            <ext:Column ID="AnalysPhSDateLastEdit" runat="server" Text="Дата редакт-я" DataIndex="date_last_edit"  Width="150" />
                                            <ext:Column ID="AnalysPhSUserEditor" runat="server" Text="Редактор" DataIndex="user_editor"  Width="200" />
                                            <ext:ImageCommandColumn ID="DeleteAnalysPhSColumn" runat="server" Width="25" Sortable="false">
                                                <Commands>
                                                    <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить образец" CommandName="DeleteAnalysPhS" />
                                                </Commands>
                                            </ext:ImageCommandColumn>                                         
                                        </Columns>            
                                    </ColumnModel>
                                </ext:GridPanel>
                            </Items>                    
                        </ext:Panel>              
                    </Items>
                </ext:Window>
            <ext:Window ID="AnalysPKW" runat="server" Title="Фосфор, калий" Width="880" Height="700"  Modal="true" Hidden="true" Layout="VBoxLayout" Resizable="False" ButtonAlign="Center">
                    <LayoutConfig>
                        <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                    </LayoutConfig>
                    <Items>
                        <ext:Panel ID="AnalysPKPanel1" runat="server" Frame="true" Layout="HBoxLayout">
                           <LayoutConfig>
                                <ext:HBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                            </LayoutConfig>
                           <Items>
                                <ext:FieldContainer ID="FieldContainer13" runat="server" Layout="FormLayout" Flex="4">
                                    <Items>
                                    <ext:TextField ID="AnalysPKRegionTF" runat="server" FieldLabel="Район" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                                    <ext:TextField ID="AnalysPKOrganizationTF" runat="server" FieldLabel="Хозяйство" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                                    <ext:TextField ID="AnalysPKDepartmentTF" runat="server" FieldLabel="Отделение" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                                    </Items>
                                </ext:FieldContainer> 
                                <ext:Component ID="Component73" runat="server" Width="20" />  
                                <ext:FieldContainer ID="FieldContainer14" runat="server" Layout="FormLayout" Flex="1">
                                    <Items>    
                                        <ext:ComboBox ID="AnalysPKTourCB" runat="server" FieldLabel="Цикл" LabelCls="darkslateblue-note" LabelWidth="30" DisplayField="tour" ValueField="tour" TabIndex="1" Flex="2">
                                            <Store>
                                                <ext:Store ID="AnalysPKTourS" runat="server">
                                                    <Model>
                                                        <ext:Model ID="AnalysPKTourM" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="tour" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                        <ext:Component ID="Component74" runat="server" Width="10" />
                                        <ext:ComboBox ID="AnalysPKYearCB" runat="server" FieldLabel="Год" LabelCls="darkslateblue-note" LabelWidth="30" DisplayField="year" ValueField="year" TabIndex="2" Flex="2">
                                            <Store>
                                                <ext:Store ID="AnalysPKYearS" runat="server">
                                                    <Model>
                                                        <ext:Model ID="AnalysPKYearM" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="year" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>                           
                                    </Items>
                                </ext:FieldContainer>                       
                            </Items>                                          
                        </ext:Panel>
                        <ext:Panel ID="AnalysPKPanel2" runat="server" Frame="true" Layout="HBoxLayout">  
                            <LayoutConfig>
                                <ext:HBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                            </LayoutConfig>                     
                            <Items>     
                                <ext:TextField ID="AnalysPKIdSampleTF" runat="server" Hidden="true" />                                
                                <ext:TextField ID="AnalysPKNumberSampleTF" runat="server" FieldLabel="№ образца" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="3" Flex="2" EnableKeyEvents="true" />
                                <ext:Component ID="Component88" runat="server" Width="15" /> 
                                <ext:TextField ID="AnalysPKValueP2O5TF" runat="server" FieldLabel="P<sub>2</sub>O<sub>5</sub>" LabelSeparator="" LabelWidth="28" LabelCls="darkslateblue-note" TabIndex="4" Flex="1" EnableKeyEvents="true" MaskRe="/[0-9]/" />
                                <ext:Component ID="Component89" runat="server" Width="15" /> 
                                <ext:TextField ID="AnalysPKValueK2OTF" runat="server" FieldLabel="K<sub>2</sub>O" LabelSeparator="" LabelWidth="25" LabelCls="darkslateblue-note" TabIndex="4" Flex="1" EnableKeyEvents="true" MaskRe="/[0-9]/" />
                                <ext:Component ID="Component90" runat="server" Width="15" /> 
                                <ext:TextField ID="AnalysPKControlP2O5TF" runat="server" FieldLabel="Контроль P<sub>2</sub>O<sub>5</sub>" LabelSeparator="" LabelWidth="85" LabelCls="darkslateblue-note" TabIndex="4" Flex="2" EnableKeyEvents="true" MaskRe="/[0-9]/" />
                                <ext:Component ID="Component91" runat="server" Width="15" /> 
                                <ext:TextField ID="AnalysPKControlK2OTF" runat="server" FieldLabel="Контроль K<sub>2</sub>O" LabelSeparator="" LabelWidth="80" LabelCls="darkslateblue-note" TabIndex="4" Flex="2" EnableKeyEvents="true" MaskRe="/[0-9]/" />
                            </Items>                                          
                        </ext:Panel>
                        <ext:Panel ID="AnalysPKPanel3" runat="server" Region="Center" Layout="FitLayout" Frame="true"  AutoScroll="true">
                            <Items>
                                <ext:GridPanel ID="AnalysPKGP" runat="server" Layout="FitLayout" MultiSelect="false" AutoScroll="true" Height="518" InvalidateScrollerOnRefresh="False">
                                    <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>   
                                    <Store>
                                        <ext:Store ID="AnalysPKS" runat="server">
                                            <Model>
                                                <ext:Model ID="AnalysPKM" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="id_sample"/>
                                                        <ext:ModelField Name="number_sample"/>
                                                        <ext:ModelField Name="id_ph"/>
                                                        <ext:ModelField Name="ph_s_value"/>
                                                        <ext:ModelField Name="id_p_k"/>
                                                        <ext:ModelField Name="p2o5_value"/>
                                                        <ext:ModelField Name="k2o_value"/>
                                                        <ext:ModelField Name="p2o5_control_value"/>
                                                        <ext:ModelField Name="k2o_control_value"/>
                                                        <ext:ModelField Name="date_input"/>
                                                        <ext:ModelField Name="date_last_edit"/>
                                                        <ext:ModelField Name="user_editor"/>
                                                        <ext:ModelField Name="tour"/>
                                                        <ext:ModelField Name="year"/>
                                                        <ext:ModelField Name="id_method"/>
                                                        <ext:ModelField Name="title_method"/>
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>                                                       
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column ID="AnalysPKIdSample" runat="server" DataIndex="id_sample" Hidden="true" Text="IdSample" />
                                            <ext:Column ID="AnalysPKIdMethod" runat="server" DataIndex="id_method" Hidden="true" Text="IdMethod" />
                                            <ext:Column ID="AnalysPKIdPh" runat="server" DataIndex="id_ph" Hidden="true" Text="IdPh" />
                                            <ext:Column ID="IdPK" runat="server" DataIndex="id_p_k" Hidden="true" Text="IdPK" />
                                            <ext:Column ID="AnalysPKTour" runat="server" DataIndex="tour" Hidden="true" Text="Цикл" />
                                            <ext:Column ID="AnalysPKYear" runat="server" DataIndex="year" Hidden="true" Text="Год" />
                                            <ext:Column ID="AnalysPKNumberSample" runat="server" Text="№ <br>обр." DataIndex="number_sample" Width="30" />
                                            <ext:Column ID="AnalysPKPhValue" runat="server" Text="Ph" DataIndex="ph_s_value"  Width="30" />
                                            <ext:Column ID="AnalysPKMethod" runat="server" Text="Метод" DataIndex="title_method"  Width="55" />
                                            <ext:Column ID="P2O5Value" runat="server" Text="P<sub>2</sub>O<sub>5</sub>" DataIndex="p2o5_value"  Width="40" />
                                            <ext:Column ID="K2OValue" runat="server" Text="K<sub>2</sub>O" DataIndex="k2o_value"  Width="40" />
                                            <ext:Column ID="ControlValueP2O5" runat="server" Text="Контроль<br>P<sub>2</sub>O<sub>5</sub>" DataIndex="p2o5_control_value"  Width="60" />
                                            <ext:Column ID="ControlValueK2O" runat="server" Text="Контроль<br>K<sub>2</sub>O" DataIndex="k2o_control_value"  Width="60" />
                                            <ext:Column ID="AnalysPKDateInput" runat="server" Text="Дата ввода" DataIndex="date_input"  Width="150" />
                                            <ext:Column ID="AnalysPKDateLastEdit" runat="server" Text="Дата редакт-я" DataIndex="date_last_edit"  Width="150" />
                                            <ext:Column ID="AnalysPKUserEditor" runat="server" Text="Редактор" DataIndex="user_editor"  Width="200" />
                                            <ext:ImageCommandColumn ID="DeleteAnalysPKColumn" runat="server" Width="20" Sortable="false">
                                                <Commands>
                                                    <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить образец" CommandName="DeleteAnalysPK" />
                                                </Commands>
                                            </ext:ImageCommandColumn>                                         
                                        </Columns>            
                                    </ColumnModel>
                                </ext:GridPanel>
                            </Items>                    
                        </ext:Panel>              
                    </Items>
                </ext:Window>
            <ext:Window ID="AnalysHAW" runat="server" Title="Гидролитическая кислотность" Width="770" Height="700"  Modal="true" Hidden="true" Layout="VBoxLayout" Resizable="False" ButtonAlign="Center">
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                </LayoutConfig>
                <Items>
                    <ext:Panel ID="AnalysHAP1" runat="server" Frame="true" Layout="FormLayout">
                       <Items>
                           <ext:TextField ID="AnalysHARegionTF" runat="server" FieldLabel="Район" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                           <ext:TextField ID="AnalysHAOrganizationTF" runat="server" FieldLabel="Хозяйство" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                           <ext:TextField ID="AnalysHADepartmentTF" runat="server" FieldLabel="Отделение" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                       </Items>                                          
                    </ext:Panel>
                    <ext:Panel ID="AnalysHAP2" runat="server" Frame="true" Layout="HBoxLayout">  
                        <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch" DefaultMargins="2" />
                        </LayoutConfig>
                        <Items>
                             <ext:ComboBox ID="AnalysHATourCB" runat="server" FieldLabel="Цикл" LabelCls="darkslateblue-note" LabelWidth="40" DisplayField="tour" ValueField="tour" TabIndex="1" Flex="2" MaskRe="/[0-9]/">
                                <Store>
                                    <ext:Store ID="AnalysHATourS" runat="server">
                                        <Model>
                                            <ext:Model ID="AnalysHATourM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="tour" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                            <ext:Component ID="Component82" runat="server" Width="5" />  
                            <ext:ComboBox ID="AnalysHAYearCB" runat="server" FieldLabel="Год" LabelCls="darkslateblue-note" LabelWidth="40" DisplayField="year" ValueField="year" TabIndex="2" Flex="2" MaskRe="/[0-9]/">
                                <Store>
                                    <ext:Store ID="AnalysHAYearS" runat="server">
                                        <Model>
                                            <ext:Model ID="AnalysHAYearM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="year" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>        
                            <ext:TextField ID="AnalysHAIdSampleTF" runat="server" Hidden="true" />                
                            <ext:Component ID="Component83" runat="server" Width="5" />                                                    
                            <ext:TextField ID="AnalysHANumberSampleTF" runat="server" FieldLabel="№ образца" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="3" Flex="2" EnableKeyEvents="true" />
                            <ext:Component ID="Component84" runat="server" Width="5" />
                            <ext:TextField ID="AnalysHAValuePhSTF" runat="server" FieldLabel="pH" LabelSeparator="" LabelWidth="20" LabelCls="darkslateblue-note" TabIndex="4" Flex="1" EnableKeyEvents="true" MaskRe="/[0-9.,]/" />
                            <ext:Component ID="Component85" runat="server" Width="5" /> 
                            <ext:TextField ID="AnalysHAValueHATF" runat="server" FieldLabel="Hг" LabelSeparator="" LabelWidth="20" LabelCls="darkslateblue-note" TabIndex="4" Flex="1" ReadOnly="true" EnableKeyEvents="true" MaskRe="/[0-9.,]/" />
                            <ext:Component ID="Component86" runat="server" Width="5" />
                            <ext:TextField ID="AnalysHAControlPhSTF" runat="server" FieldLabel="pHк" LabelSeparator="" LabelWidth="20" LabelCls="darkslateblue-note" TabIndex="4" Flex="1" EnableKeyEvents="true" MaskRe="/[0-9.,]/" />
                            <ext:Component ID="Component87" runat="server" Width="5" />
                            <ext:TextField ID="AnalysHAControlTF" runat="server" FieldLabel="Контроль" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="4" Flex="2" ReadOnly="true" EnableKeyEvents="true" MaskRe="/[0-9.,]/" />
                        </Items>                                          
                    </ext:Panel>
                    <ext:Panel ID="AnalysHAP3" runat="server" Region="Center" Layout="FitLayout" Frame="true"  AutoScroll="true">
                        <Items>
                            <ext:GridPanel ID="AnalysHAGP" runat="server" Layout="FitLayout" MultiSelect="false" AutoScroll="true" Height="523" InvalidateScrollerOnRefresh="False">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>   
                                <TopBar>
                                    <ext:Toolbar ID="AnalysHATB" runat="server" Hidden="true">
                                        <Items>
                                            <ext:TextField ID="AnalysHAFewTF" runat="server" EnableKeyEvents="true" Flex="1" />   
                                            <ext:Button ID="AnalysHAAddFewB" runat="server" Icon="Add" Text="Добавить несколько" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>                            
                                <Store>
                                    <ext:Store ID="AnalysHAS" runat="server">
                                        <Model>
                                            <ext:Model ID="AnalysHAM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_sample"/>
                                                    <ext:ModelField Name="id_ha"/>
                                                    <ext:ModelField Name="number_sample"/>
                                                    <ext:ModelField Name="ha_value"/>
                                                    <ext:ModelField Name="control_value"/>
                                                    <ext:ModelField Name="date_input"/>
                                                    <ext:ModelField Name="date_last_edit"/>
                                                    <ext:ModelField Name="user_editor"/>
                                                    <ext:ModelField Name="tour"/>
                                                    <ext:ModelField Name="year"/>
                                                    <ext:ModelField Name="id_method"/>
                                                    <ext:ModelField Name="title_method"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                                                       
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="AnalysHAIdSample" runat="server" DataIndex="id_sample" Hidden="true" Text="IdSample" />
                                        <ext:Column ID="AnalysHAIdMethod" runat="server" DataIndex="id_method" Hidden="true" Text="IdMethod" />
                                        <ext:Column ID="AnalysHAIdHA" runat="server" DataIndex="id_ha" Hidden="true" Text="IdPh" />
                                        <ext:Column ID="AnalysHATour" runat="server" DataIndex="tour" Hidden="true" Text="Цикл" />
                                        <ext:Column ID="AnalysHAYear" runat="server" DataIndex="year" Hidden="true" Text="Год" />
                                        <ext:Column ID="AnalysHANumberSample" runat="server" Text="№ <br>обр." DataIndex="number_sample" Width="30" />
                                        <ext:Column ID="AnalysHAMethod" runat="server" Text="Метод" DataIndex="title_method"  Width="60" />
                                        <ext:Column ID="AnalysHAValue" runat="server" Text="Hг" DataIndex="ha_value"  Width="40" />
                                        <ext:Column ID="AnalysHAControlValue" runat="server" Text="Контроль" DataIndex="control_value"  Width="70" />
                                        <ext:Column ID="AnalysHADateInput" runat="server" Text="Дата ввода" DataIndex="date_input"  Width="150" />
                                        <ext:Column ID="AnalysHADateLastEdit" runat="server" Text="Дата редакт-я" DataIndex="date_last_edit"  Width="150" />
                                        <ext:Column ID="AnalysHAUserEditor" runat="server" Text="Редактор" DataIndex="user_editor"  Width="200" />
                                        <ext:ImageCommandColumn ID="DeleteAnalysHAColumn" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить образец" CommandName="DeleteAnalysHA" />
                                            </Commands>
                                        </ext:ImageCommandColumn>                                         
                                    </Columns>            
                                </ColumnModel>
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel>              
                </Items>
            </ext:Window>
            <ext:Window ID="AnalysHMW" runat="server" Title="Тяжелые металлы" Width="770" Height="700"  Modal="true" Hidden="true" Layout="VBoxLayout" Resizable="False" ButtonAlign="Center">
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                </LayoutConfig>
                <Items>
                    <ext:Panel ID="AnalysHMP1" runat="server" Frame="true" Layout="FormLayout">
                       <Items>
                           <ext:TextField ID="AnalysHMRegionTF" runat="server" FieldLabel="Район" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                           <ext:TextField ID="AnalysHMOrganizationTF" runat="server" FieldLabel="Хозяйство" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                           <ext:TextField ID="AnalysHMDepartmentTF" runat="server" FieldLabel="Отделение" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                       </Items>                                          
                    </ext:Panel>
                    <ext:Panel ID="AnalysHMP2" runat="server" Frame="true" Layout="HBoxLayout">  
                        <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                        </LayoutConfig>                     
                        <Items>
                             <ext:ComboBox ID="AnalysHMTourCB" runat="server" FieldLabel="Цикл" LabelCls="darkslateblue-note" LabelWidth="40" DisplayField="tour" ValueField="tour" TabIndex="1" Flex="1" MaskRe="/[0-9]/">
                                <Store>
                                    <ext:Store ID="AnalysHMTourS" runat="server">
                                        <Model>
                                            <ext:Model ID="AnalysHMTourM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="tour" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                            <ext:Component ID="Component103" runat="server" Width="5" />  
                            <ext:ComboBox ID="AnalysHMYearCB" runat="server" FieldLabel="Год" LabelCls="darkslateblue-note" LabelWidth="40" DisplayField="year" ValueField="year" TabIndex="2" Flex="1" MaskRe="/[0-9]/">
                                <Store>
                                    <ext:Store ID="AnalysHMYearS" runat="server">
                                        <Model>
                                            <ext:Model ID="AnalysHMYearM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="year" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>                      
                            <ext:Component ID="Component104" runat="server" Width="5" />
                            <ext:ComboBox ID="AnalysHMElementCB" runat="server" FieldLabel="Элемент:" LabelCls="darkslateblue-note" LabelWidth="60" DisplayField="title_significative" ValueField="id_significative" TabIndex="1" Flex="1" Editable="false">
                                <Store>
                                    <ext:Store ID="AnalysHMElementS" runat="server">
                                        <Model>
                                            <ext:Model ID="AnalysHMElementM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_significative" />
                                                    <ext:ModelField Name="title_significative" />
                                                    <ext:ModelField Name="unit_significative" />
                                                    <ext:ModelField Name="name_significative" />
                                                    <ext:ModelField Name="min_value" />
                                                    <ext:ModelField Name="max_value" />
                                                    <ext:ModelField Name="number_of_digits" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                        </Items>                                          
                    </ext:Panel>
                    <ext:Panel ID="AnalysHMP3" runat="server" Frame="true" Layout="HBoxLayout">  
                        <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                        </LayoutConfig>                     
                        <Items>
                            <ext:TextField ID="AnalysHMIdHMTF" runat="server" Hidden="true" />                
                            <ext:Component ID="Component105" runat="server" Width="5" />  
                            <ext:TextField ID="AnalysHMNumberPlotTF" runat="server" FieldLabel="№ уч-ка" LabelSeparator="" LabelWidth="60" LabelCls="darkslateblue-note" TabIndex="4" Flex="1" EnableKeyEvents="true" MaskRe="/[0-9.,]/" />
                            <ext:Component ID="Component106" runat="server" Width="5" /> 
                            <ext:TextField ID="AnalysHMValueTF" runat="server" FieldLabel="Значение" LabelSeparator="" LabelWidth="60" LabelCls="darkslateblue-note" TabIndex="4" Flex="1" EnableKeyEvents="true" MaskRe="/[0-9.,]/" />
                            <ext:Component ID="Component107" runat="server" Width="5" /> 
                            <ext:TextField ID="AnalysHMControlTF" runat="server" FieldLabel="Контроль" LabelSeparator="" LabelWidth="60" LabelCls="darkslateblue-note" TabIndex="4" Flex="1" EnableKeyEvents="true" MaskRe="/[0-9.,]/" />
                        </Items>                                          
                    </ext:Panel>
                    <ext:Panel ID="AnalysHMP4" runat="server" Region="Center" Layout="FitLayout" Frame="true"  AutoScroll="true">
                        <Items>
                            <ext:GridPanel ID="AnalysHMGP" runat="server" Layout="FitLayout" MultiSelect="false" AutoScroll="true" Height="523" InvalidateScrollerOnRefresh="False">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                <TopBar>
                                    <ext:Toolbar ID="AnalysHMTB" runat="server">
                                        <Items>
                                            <ext:TextField ID="AnalysHMFewTF" runat="server" EnableKeyEvents="true" Flex="1" />   
                                            <ext:Button ID="AnalysHMAddFewB" runat="server" Icon="Add" Text="Добавить несколько" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>                      
                                <Store>
                                    <ext:Store ID="AnalysHMS" runat="server">
                                        <Model>
                                            <ext:Model ID="AnalysHMM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_hm"/>
                                                    <ext:ModelField Name="id_department"/>
                                                    <ext:ModelField Name="id_significative"/>
                                                    <ext:ModelField Name="tour"/>
                                                    <ext:ModelField Name="year"/>
                                                    <ext:ModelField Name="name_significative"/>
                                                    <ext:ModelField Name="title_significative"/>
                                                    <ext:ModelField Name="number_plot"/>
                                                    <ext:ModelField Name="value"/>
                                                    <ext:ModelField Name="control_value"/>
                                                    <ext:ModelField Name="date_input"/>
                                                    <ext:ModelField Name="date_last_edit"/>
                                                    <ext:ModelField Name="user_editor"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                                                       
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="AnalysHMIdHmC" runat="server" DataIndex="id_hm" Hidden="true" Text="IdHM" />
                                        <ext:Column ID="AnalysHMIdDepC" runat="server" DataIndex="id_department" Hidden="true" Text="IdDepartment" />
                                        <ext:Column ID="AnalysHMIdSignC" runat="server" DataIndex="id_significative" Hidden="true" Text="IdSign" />
                                        <ext:Column ID="AnalysHMTourC" runat="server" DataIndex="tour" Hidden="true" Text="Цикл" />
                                        <ext:Column ID="AnalysHMYearC" runat="server" DataIndex="year" Hidden="true" Text="Год" />
                                        <ext:Column ID="AnalysHMNameSignC" runat="server" DataIndex="name_significative" Text="Элемент" Width="50" />
                                        <ext:Column ID="AnalysHMTitleSignC" runat="server" DataIndex="title_significative" Hidden="true" Text="Название элемента" />
                                        <ext:Column ID="AnalysHMNumberPlotC" runat="server" Text="№ <br>уч-ка" DataIndex="number_plot" Width="40" />
                                        <ext:Column ID="AnalysHMValueC" runat="server" Text="Значение" DataIndex="value"  Width="60" />
                                        <ext:Column ID="AnalysHMControlValueC" runat="server" Text="Контроль" DataIndex="control_value"  Width="70" />
                                        <ext:Column ID="AnalysHMDateInputC" runat="server" Text="Дата ввода" DataIndex="date_input"  Width="150" />
                                        <ext:Column ID="AnalysHMDateeditC" runat="server" Text="Дата редакт-я" DataIndex="date_last_edit"  Width="150" />
                                        <ext:Column ID="AnalysHMEditorC" runat="server" Text="Редактор" DataIndex="user_editor"  Width="200" />
                                        <ext:ImageCommandColumn ID="AnalysHMDeleteIC" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить" CommandName="DeleteAnalysHM" />
                                            </Commands>
                                        </ext:ImageCommandColumn>                                         
                                    </Columns>            
                                </ColumnModel>
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel>              
                </Items>
            </ext:Window>
            <ext:Window ID="AnalysMicroW" runat="server" Title="Микроэлементы" Width="770" Height="700"  Modal="true" Hidden="true" Layout="VBoxLayout" Resizable="False" ButtonAlign="Center">
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                </LayoutConfig>
                <Items>
                    <ext:Panel ID="Panel1" runat="server" Frame="true" Layout="FormLayout">
                       <Items>
                           <ext:TextField ID="TextField1" runat="server" FieldLabel="Район" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                           <ext:TextField ID="TextField2" runat="server" FieldLabel="Хозяйство" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                           <ext:TextField ID="TextField3" runat="server" FieldLabel="Отделение" LabelSeparator="" LabelWidth="70" LabelCls="darkslateblue-note" TabIndex="-1" ReadOnly="true" />
                       </Items>                                          
                    </ext:Panel>
                    <ext:Panel ID="Panel2" runat="server" Frame="true" Layout="HBoxLayout">  
                        <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                        </LayoutConfig>                     
                        <Items>
                             <ext:ComboBox ID="ComboBox1" runat="server" FieldLabel="Цикл" LabelCls="darkslateblue-note" LabelWidth="40" DisplayField="tour" ValueField="tour" TabIndex="1" Flex="1" MaskRe="/[0-9]/">
                                <Store>
                                    <ext:Store ID="Store1" runat="server">
                                        <Model>
                                            <ext:Model ID="Model1" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="tour" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                            <ext:Component ID="Component108" runat="server" Width="5" />  
                            <ext:ComboBox ID="ComboBox2" runat="server" FieldLabel="Год" LabelCls="darkslateblue-note" LabelWidth="40" DisplayField="year" ValueField="year" TabIndex="2" Flex="1" MaskRe="/[0-9]/">
                                <Store>
                                    <ext:Store ID="Store2" runat="server">
                                        <Model>
                                            <ext:Model ID="Model2" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="year" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>                      
                            <ext:Component ID="Component109" runat="server" Width="5" />
                            <ext:ComboBox ID="ComboBox3" runat="server" FieldLabel="Элемент:" LabelCls="darkslateblue-note" LabelWidth="60" DisplayField="title_significative" ValueField="id_significative" TabIndex="1" Flex="1" Editable="false">
                                <Store>
                                    <ext:Store ID="Store3" runat="server">
                                        <Model>
                                            <ext:Model ID="Model3" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_significative" />
                                                    <ext:ModelField Name="title_significative" />
                                                    <ext:ModelField Name="unit_significative" />
                                                    <ext:ModelField Name="name_significative" />
                                                    <ext:ModelField Name="min_value" />
                                                    <ext:ModelField Name="max_value" />
                                                    <ext:ModelField Name="number_of_digits" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                        </Items>                                          
                    </ext:Panel>
                    <ext:Panel ID="Panel3" runat="server" Frame="true" Layout="HBoxLayout">  
                        <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                        </LayoutConfig>                     
                        <Items>
                            <ext:TextField ID="TextField4" runat="server" Hidden="true" />                
                            <ext:Component ID="Component110" runat="server" Width="5" />  
                            <ext:TextField ID="TextField5" runat="server" FieldLabel="№ уч-ка" LabelSeparator="" LabelWidth="60" LabelCls="darkslateblue-note" TabIndex="4" Flex="1" EnableKeyEvents="true" MaskRe="/[0-9.,]/" />
                            <ext:Component ID="Component111" runat="server" Width="5" /> 
                            <ext:TextField ID="TextField6" runat="server" FieldLabel="Значение" LabelSeparator="" LabelWidth="60" LabelCls="darkslateblue-note" TabIndex="4" Flex="1" EnableKeyEvents="true" MaskRe="/[0-9.,]/" />
                            <ext:Component ID="Component112" runat="server" Width="5" /> 
                            <ext:TextField ID="TextField7" runat="server" FieldLabel="Контроль" LabelSeparator="" LabelWidth="60" LabelCls="darkslateblue-note" TabIndex="4" Flex="1" EnableKeyEvents="true" MaskRe="/[0-9.,]/" />
                        </Items>                                          
                    </ext:Panel>
                    <ext:Panel ID="Panel4" runat="server" Region="Center" Layout="FitLayout" Frame="true"  AutoScroll="true">
                        <Items>
                            <ext:GridPanel ID="GridPanel1" runat="server" Layout="FitLayout" MultiSelect="false" AutoScroll="true" Height="523" InvalidateScrollerOnRefresh="False">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server">
                                        <Items>
                                            <ext:TextField ID="TextField8" runat="server" EnableKeyEvents="true" Flex="1" />   
                                            <ext:Button ID="Button1" runat="server" Icon="Add" Text="Добавить несколько" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>                      
                                <Store>
                                    <ext:Store ID="Store4" runat="server">
                                        <Model>
                                            <ext:Model ID="Model4" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_hm"/>
                                                    <ext:ModelField Name="id_department"/>
                                                    <ext:ModelField Name="id_significative"/>
                                                    <ext:ModelField Name="tour"/>
                                                    <ext:ModelField Name="year"/>
                                                    <ext:ModelField Name="name_significative"/>
                                                    <ext:ModelField Name="title_significative"/>
                                                    <ext:ModelField Name="number_plot"/>
                                                    <ext:ModelField Name="value"/>
                                                    <ext:ModelField Name="control_value"/>
                                                    <ext:ModelField Name="date_input"/>
                                                    <ext:ModelField Name="date_last_edit"/>
                                                    <ext:ModelField Name="user_editor"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                                                       
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="Column17" runat="server" DataIndex="id_hm" Hidden="true" Text="IdHM" />
                                        <ext:Column ID="Column18" runat="server" DataIndex="id_department" Hidden="true" Text="IdDepartment" />
                                        <ext:Column ID="Column19" runat="server" DataIndex="id_significative" Hidden="true" Text="IdSign" />
                                        <ext:Column ID="Column21" runat="server" DataIndex="tour" Hidden="true" Text="Цикл" />
                                        <ext:Column ID="Column22" runat="server" DataIndex="year" Hidden="true" Text="Год" />
                                        <ext:Column ID="Column23" runat="server" DataIndex="name_significative" Text="Элемент" Width="50" />
                                        <ext:Column ID="Column25" runat="server" DataIndex="title_significative" Hidden="true" Text="Название элемента" />
                                        <ext:Column ID="Column26" runat="server" Text="№ <br>уч-ка" DataIndex="number_plot" Width="40" />
                                        <ext:Column ID="Column27" runat="server" Text="Значение" DataIndex="value"  Width="60" />
                                        <ext:Column ID="Column29" runat="server" Text="Контроль" DataIndex="control_value"  Width="70" />
                                        <ext:Column ID="Column30" runat="server" Text="Дата ввода" DataIndex="date_input"  Width="150" />
                                        <ext:Column ID="Column31" runat="server" Text="Дата редакт-я" DataIndex="date_last_edit"  Width="150" />
                                        <ext:Column ID="Column33" runat="server" Text="Редактор" DataIndex="user_editor"  Width="200" />
                                        <ext:ImageCommandColumn ID="ImageCommandColumn1" runat="server" Width="25" Sortable="false">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить" CommandName="DeleteAnalysHM" />
                                            </Commands>
                                        </ext:ImageCommandColumn>                                         
                                    </Columns>            
                                </ColumnModel>
                            </ext:GridPanel>
                        </Items>                    
                    </ext:Panel>              
                </Items>
            </ext:Window>
            <ext:Window ID="PlansW" runat="server" Icon="Book" Title="Планы-задания"  MinWidth="1000" MinHeight="600" Width="1000" Height="700"  Modal="true" Maximizable="true" Hidden="true" Layout="BorderLayout" Resizable="true">              
                <Items>
                    <ext:Panel ID="PlansP1" runat="server" Frame="true" Layout="VBoxLayout" Flex="2" Region="Center">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                        </LayoutConfig>
                        <Items>                                                                     
                            <ext:ComboBox ID="PlansWorkerCB" runat="server" FieldLabel="Работник" LabelCls="darkslateblue-note" LabelWidth="70" DisplayField="title_worker" ValueField="id_worker" Editable="false">
                                 <Store>
                                     <ext:Store ID="PlansWorkerS" runat="server">
                                         <Model>
                                             <ext:Model ID="PlansWorkerM" runat="server">
                                                 <Fields>
                                                     <ext:ModelField Name="id_worker" />
                                                     <ext:ModelField Name="title_worker" />
                                                 </Fields>
                                             </ext:Model>
                                         </Model>
                                     </ext:Store>
                                 </Store>
                            </ext:ComboBox>
                            <ext:ComboBox ID="PlansRegionCB" runat="server" FieldLabel="Район" LabelCls="darkslateblue-note" LabelWidth="70" DisplayField="title_region" ValueField="id_region" Editable="false">
                                 <Store>
                                     <ext:Store ID="PlansRegionS" runat="server">
                                         <Model>
                                             <ext:Model ID="PlansRegionM" runat="server">
                                                 <Fields>
                                                     <ext:ModelField Name="id_region" />
                                                     <ext:ModelField Name="title_region" />
                                                 </Fields>
                                             </ext:Model>
                                         </Model>
                                     </ext:Store>
                                 </Store>
                            </ext:ComboBox>
                            <ext:ComboBox ID="PlansMissionCB" runat="server" FieldLabel="Задание" LabelCls="darkslateblue-note" LabelWidth="70" DisplayField="title_mission" ValueField="id_mission" Editable="false">
                                 <Store>
                                     <ext:Store ID="PlansMissionS" runat="server">
                                         <Model>
                                             <ext:Model ID="PlansMissionM" runat="server">
                                                 <Fields>
                                                     <ext:ModelField Name="id_mission" />
                                                     <ext:ModelField Name="title_mission" />
                                                 </Fields>
                                             </ext:Model>
                                         </Model>
                                     </ext:Store>
                                 </Store>
                            </ext:ComboBox>
                            <ext:FieldContainer ID="PlansDataFC" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:DateField ID="PlansFromDF" runat="server" LabelCls="darkslateblue-note" FieldLabel="Дата: с:" LabelWidth="70" Width="250" Editable="false" />                
                                    <ext:Component ID="Component75" runat="server" Width="10" />                                   
                                    <ext:DateField ID="PlansToDF" runat="server" Vtype="daterange" LabelCls="darkslateblue-note" FieldLabel="по" LabelWidth="20" Width="200" Editable="false" />
                                    <ext:Component ID="Component92" runat="server" Width="10" /> 
                                    <ext:Button ID="ResetPlansB" runat="server" Text="Сбросить"/>
                                </Items>
                            </ext:FieldContainer>
                            <ext:GridPanel ID="PlansGP" runat="server" Layout="FitLayout" Flex="3" MultiSelect="false" AutoScroll="true" Height="523">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>   
                                <TopBar>
                                    <ext:Toolbar ID="PlansTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddPlansB" runat="server" Icon="Add" Text="Добавить" />
                                            <ext:Button ID="CopyPlansB" runat="server" Icon="PageCopy" Text="Дублировать" />
                                            <ext:Button ID="DeletePlansB" runat="server" Icon="Delete" Text="Удалить" />
                                            <ext:Button ID="ReportPlansB" runat="server" Icon="Report" Text="Печатная форма" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>                            
                                <Store>
                                    <ext:Store ID="PlansS" runat="server">
                                        <Model>
                                            <ext:Model ID="PlansM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_plan"/>
                                                    <ext:ModelField Name="id_worker"/>
                                                    <ext:ModelField Name="title_worker"/>
                                                    <ext:ModelField Name="id_region"/>
                                                    <ext:ModelField Name="title_region"/>
                                                    <ext:ModelField Name="id_mission"/>
                                                    <ext:ModelField Name="title_mission"/>
                                                    <ext:ModelField Name="date_from"/>
                                                    <ext:ModelField Name="date_to"/>
                                                    <ext:ModelField Name="count_days"/>
                                                    <ext:ModelField Name="count_working_days"/>
                                                    <ext:ModelField Name="id_chief"/>
                                                    <ext:ModelField Name="title_chief"/>
                                                    <ext:ModelField Name="id_matcher"/>
                                                    <ext:ModelField Name="title_matcher"/>
                                                    <ext:ModelField Name="is_driver"/>
                                                    <ext:ModelField Name="id_check_points"/>
                                                    <ext:ModelField Name="title_check_points"/>
                                                    <ext:ModelField Name="id_check_act"/>
                                                    <ext:ModelField Name="title_check_act"/>
                                                    <ext:ModelField Name="id_check_probes"/>
                                                    <ext:ModelField Name="title_check_probes"/>
                                                    <ext:ModelField Name="id_check_maps"/>
                                                    <ext:ModelField Name="title_check_maps"/>
                                                    <ext:ModelField Name="date_check_points"/>
                                                    <ext:ModelField Name="date_check_act"/>
                                                    <ext:ModelField Name="date_check_probes"/>
                                                    <ext:ModelField Name="date_check_maps"/>
                                                    <ext:ModelField Name="plan_result"/>
                                                    <ext:ModelField Name="id_plan_closer"/>
                                                    <ext:ModelField Name="title_plan_closer"/>
                                                    <ext:ModelField Name="date_input"/>
                                                    <ext:ModelField Name="date_edit"/>
                                                    <ext:ModelField Name="id_user"/>
                                                    <ext:ModelField Name="title_user"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                                                       
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="PlansIdPlan" runat="server" DataIndex="id_plan" Hidden="true" Text="ID плана" Width="30" />
                                        <ext:Column ID="PlansIdWorker" runat="server" DataIndex="id_worker" Hidden="true" Text="ID работника" Width="30" />
                                        <ext:Column ID="PlansTitleWorker" runat="server" DataIndex="title_worker" Text="Работник" Width="150">
                                            <Editor>
                                                <ext:ComboBox ID="EditPlansWorkerCB" runat="server" DisplayField="title_worker" ValueField="title_worker" AllowBlank="false" Editable="false">
                                                    <Store>
                                                        <ext:Store ID="EditPlansWorkerS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="EditPlansWorkerM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_worker" />
                                                                        <ext:ModelField Name="title_worker" />
                                                                        <ext:ModelField Name="job_title_worker" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <ListConfig  LoadingText="Загрузка..." MinWidth="300" Border="true">
                                                        <ItemTpl ID="ItemTpl1" runat="server">
                                                            <Html><div class="search-item">{title_worker}</div></Html>
                                                         </ItemTpl>
                                                    </ListConfig>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="PlansIdRegion" runat="server" DataIndex="id_region" Hidden="true" Text="ID района" Width="30" />
                                        <ext:Column ID="PlansTitleRegion" runat="server" DataIndex="title_region" Text="Район" Width="150">
                                            <Editor>
                                                <ext:ComboBox ID="EditPlansRegionCB" runat="server" DisplayField="title_region" ValueField="title_region" AllowBlank="false" Editable="false">
                                                    <Store>
                                                        <ext:Store ID="EditPlansRegionS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="EditPlansRegionM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_region" />
                                                                        <ext:ModelField Name="title_region" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <ListConfig  LoadingText="Загрузка..." MinWidth="300" Border="true">
                                                        <ItemTpl ID="ItemTpl3" runat="server">
                                                            <Html><div class="search-item">{title_region}</div></Html>
                                                         </ItemTpl>
                                                    </ListConfig>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="PlansIdMission" runat="server" DataIndex="id_mission" Hidden="true" Text="ID задания" Width="30" />
                                        <ext:Column ID="PlansTitleMission" runat="server" DataIndex="title_mission" Text="Задание" Width="150">
                                            <Editor>
                                                <ext:ComboBox ID="EditPlansMissionCB" runat="server" DisplayField="title_mission" ValueField="title_mission" AllowBlank="false" Editable="false">
                                                    <Store>
                                                        <ext:Store ID="EditPlansMissionS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="EditPlansMissionM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_mission" />
                                                                        <ext:ModelField Name="title_mission" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <ListConfig  LoadingText="Загрузка..." MinWidth="300" Border="true">
                                                        <ItemTpl ID="ItemTpl4" runat="server">
                                                            <Html><div class="search-item">{title_mission}</div></Html>
                                                         </ItemTpl>
                                                    </ListConfig>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:DateColumn ID="PlansDateFromDC" runat="server" DataIndex="date_from" Text="С" Width="100" Format="dd.MM.yyyy">
                                            <Editor>
                                                <ext:DateField ID="EditPlansDateFromDF" runat="server" Editable="false" AllowBlank="false" Format="dd.MM.yyyy" />
                                            </Editor>
                                        </ext:DateColumn>
                                        <ext:DateColumn ID="PlansDateToDC" runat="server" DataIndex="date_to" Text="По" Width="100" Format="dd.MM.yyyy">
                                            <Editor>
                                                <ext:DateField ID="EditPlansDateToDF" runat="server" Editable="false" AllowBlank="false" Format="dd.MM.yyyy" />
                                            </Editor>
                                        </ext:DateColumn>
                                        <ext:Column ID="PlansCountDays" runat="server" DataIndex="count_days" Text="Кол-во дней" Width="60">
                                            <Editor>
                                                <ext:NumberField ID="EditPlansCountDaysNF" runat="server" Editable="false" MinValue="1" MaxValue="365" AllowBlank="false" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="PlansCountWorkingDays" runat="server" DataIndex="count_working_days" Text="Кол-во рабочих дней" Width="60">
                                            <Editor>
                                                <ext:NumberField ID="EditPlansCountWorkingDaysNF" runat="server" Editable="false" MinValue="1" MaxValue="365" AllowBlank="false" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="PlansIdChief" runat="server" DataIndex="id_chief" Hidden="true" Text="ID Составителя" Width="30" />
                                        <ext:Column ID="PlansTitleChief" runat="server" DataIndex="title_chief" Text="Составил" Width="150">
                                            <Editor>
                                                <ext:ComboBox ID="EditPlansChiefCB" runat="server" DisplayField="title_chief" ValueField="title_chief" AllowBlank="false" Editable="false">
                                                    <Store>
                                                        <ext:Store ID="EditPlansChiefS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="EditPlansChiefM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_chief" />
                                                                        <ext:ModelField Name="title_chief" />
                                                                        <ext:ModelField Name="job_title_chief" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <ListConfig  LoadingText="Загрузка..." MinWidth="300" Border="true">
                                                        <ItemTpl ID="ItemTpl5" runat="server">
                                                            <Html><div class="search-item">{title_chief}</div></Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="PlansIdMatcher" runat="server" DataIndex="id_matcher" Hidden="true" Text="ID Согласователя" Width="30" />
                                        <ext:Column ID="PlansTitleMatcher" runat="server" DataIndex="title_matcher" Text="Согласовал" Width="150">
                                            <Editor>
                                                <ext:ComboBox ID="EditPlansMatcherCB" runat="server" DisplayField="title_matcher" ValueField="title_matcher" AllowBlank="false" Editable="false">
                                                    <Store>
                                                        <ext:Store ID="EditPlansMatcherS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="EditPlansMatcherM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_matcher" />
                                                                        <ext:ModelField Name="title_matcher" />
                                                                        <ext:ModelField Name="job_title_matcher" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <ListConfig  LoadingText="Загрузка..." MinWidth="300" Border="true">
                                                        <ItemTpl ID="ItemTpl6" runat="server">
                                                            <Html><div class="search-item">{title_matcher}</div></Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:CheckColumn ID="PlansIsDriver" runat="server" Text="Водитель"  DataIndex="is_driver" Width="60">
                                            <Editor>
                                                <ext:Checkbox ID="EditPlansIsDriverCB" runat="server" />
                                            </Editor>
                                        </ext:CheckColumn>
                                        <ext:Column ID="PlansIdCheckProbes" runat="server" DataIndex="id_check_probes" Hidden="true" Text="ID Сдающего пробы" Width="30" />
                                        <ext:Column ID="PlansTitleCheckProbes" runat="server" DataIndex="title_check_probes" Text="Пробы сданы" Width="150">
                                            <Editor>
                                                <ext:ComboBox ID="EditPlansCheckProbesCB" runat="server" DisplayField="title_check_probes" ValueField="title_check_probes" AllowBlank="true" Editable="false">
                                                    <Store>
                                                        <ext:Store ID="EditPlansCheckProbesS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="EditPlansCheckProbesM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_check_probes" />
                                                                        <ext:ModelField Name="title_check_probes" />
                                                                        <ext:ModelField Name="job_title_check_probes" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <ListConfig  LoadingText="Загрузка..." MinWidth="300" Border="true">
                                                        <ItemTpl ID="ItemTpl7" runat="server">
                                                            <Html><div class="search-item">{title_check_probes}</div></Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:DateColumn ID="DateCheckProbes" runat="server" DataIndex="date_check_probes" Text="Дата сдачи проб" Width="100" Format="dd.MM.yyyy">
                                            <Editor>
                                                <ext:DateField ID="EditDateCheckProbesDF" runat="server" Editable="false" AllowBlank="true" Format="dd.MM.yyyy" />
                                            </Editor>
                                        </ext:DateColumn>
                                        <ext:Column ID="PlansIdCheckMaps" runat="server" DataIndex="id_check_maps" Hidden="true" Text="ID Принимающего карты" Width="30" />
                                        <ext:Column ID="PlansTitleCheckMaps" runat="server" DataIndex="title_check_maps" Text="Карты приняты" Width="150">
                                            <Editor>
                                                <ext:ComboBox ID="EditPlansCheckMapsCB" runat="server" DisplayField="title_check_maps" ValueField="title_check_maps" AllowBlank="true" Editable="false">
                                                    <Store>
                                                        <ext:Store ID="EditPlansCheckMapsS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="EditPlansCheckMapsM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_check_maps" />
                                                                        <ext:ModelField Name="title_check_maps" />
                                                                        <ext:ModelField Name="job_title_check_maps" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <ListConfig  LoadingText="Загрузка..." MinWidth="300" Border="true">
                                                        <ItemTpl ID="ItemTpl8" runat="server">
                                                            <Html><div class="search-item">{title_check_maps}</div></Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:DateColumn ID="DateCheckMaps" runat="server" DataIndex="date_check_maps" Text="Дата принятия карт" Width="100" Format="dd.MM.yyyy">
                                            <Editor>
                                                <ext:DateField ID="EditDateCheckMapsDF" runat="server" Editable="false" AllowBlank="true" Format="dd.MM.yyyy" />
                                            </Editor>
                                        </ext:DateColumn>
                                        <ext:Column ID="PlansIdCheckPoints" runat="server" DataIndex="id_check_points" Hidden="true" Text="ID Сохраняющего точки" Width="30" />
                                        <ext:Column ID="PlansTitleCheckPoints" runat="server" DataIndex="title_check_points" Text="Точки сохранены" Width="150">
                                            <Editor>
                                                <ext:ComboBox ID="EditPlansCheckPointsCB" runat="server" DisplayField="title_check_points" ValueField="title_check_points" AllowBlank="true" Editable="false">
                                                    <Store>
                                                        <ext:Store ID="EditPlansCheckPointsS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="EditPlansCheckPointsM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_check_points" />
                                                                        <ext:ModelField Name="title_check_points" />
                                                                        <ext:ModelField Name="job_title_check_points" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <ListConfig  LoadingText="Загрузка..." MinWidth="300" Border="true">
                                                        <ItemTpl ID="ItemTpl9" runat="server">
                                                            <Html><div class="search-item">{title_check_points}</div></Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:DateColumn ID="DateCheckPoints" runat="server" DataIndex="date_check_points" Text="Дата сохранения точек" Width="100" Format="dd.MM.yyyy">
                                            <Editor>
                                                <ext:DateField ID="EditDateCheckPointsDF" runat="server" Editable="false" AllowBlank="true" Format="dd.MM.yyyy" />
                                            </Editor>
                                        </ext:DateColumn>
                                        <ext:Column ID="PlansIdCheckAct" runat="server" DataIndex="id_check_act" Hidden="true" Text="ID Сдающего акт" Width="30" />
                                        <ext:Column ID="PlansTitleCheckAct" runat="server" DataIndex="title_check_act" Text="Акт сдан" Width="150">
                                            <Editor>
                                                <ext:ComboBox ID="EditPlansCheckActCB" runat="server" DisplayField="title_check_act" ValueField="title_check_act" AllowBlank="true" Editable="false">
                                                    <Store>
                                                        <ext:Store ID="EditPlansCheckActS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="EditPlansCheckActM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_check_act" />
                                                                        <ext:ModelField Name="title_check_act" />
                                                                        <ext:ModelField Name="job_title_check_act" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <ListConfig  LoadingText="Загрузка..." MinWidth="300" Border="true">
                                                        <ItemTpl ID="ItemTpl10" runat="server">
                                                            <Html><div class="search-item">{title_check_act}</div></Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:DateColumn ID="DateCheckAct" runat="server" DataIndex="date_check_act" Text="Дата сдачи акта" Width="100" Format="dd.MM.yyyy">
                                            <Editor>
                                                <ext:DateField ID="EditDateCheckActDF" runat="server" Editable="false" AllowBlank="true" Format="dd.MM.yyyy" />
                                            </Editor>
                                        </ext:DateColumn>
                                        <ext:Column ID="PlansPlanResult" runat="server" DataIndex="plan_result" Text="Заключение" Width="200">
                                            <Editor>
                                                <ext:TextField ID="EditPlanResultTF" runat="server" AllowBlank="true" />
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="PlansIdPlanCloser" runat="server" DataIndex="id_plan_closer" Hidden="true" Text="ID Принявшего работы" Width="30" />
                                        <ext:Column ID="PlansTitlePlanCloser" runat="server" DataIndex="title_plan_closer" Text="Принял работы" Width="150">
                                            <Editor>
                                                <ext:ComboBox ID="EditPlanCloserCB" runat="server" DisplayField="title_plan_closer" ValueField="title_plan_closer" AllowBlank="true" Editable="false">
                                                    <Store>
                                                        <ext:Store ID="EditPlanCloserS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="EditPlanCloserM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_plan_closer" />
                                                                        <ext:ModelField Name="title_plan_closer" />
                                                                        <ext:ModelField Name="job_title_plan_closer" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <ListConfig  LoadingText="Загрузка..." MinWidth="300" Border="true">
                                                        <ItemTpl ID="ItemTpl11" runat="server">
                                                            <Html><div class="search-item">{title_plan_closer}</div></Html>
                                                        </ItemTpl>
                                                    </ListConfig>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="PlansDateInput" runat="server" DataIndex="date_input" Text="Дата ввода" Width="100" />
                                        <ext:Column ID="PlansDateEdit" runat="server" DataIndex="date_edit" Text="Дата редактирования" Width="130" />
                                        <ext:Column ID="PlansIdUser" runat="server" DataIndex="id_user" Hidden="true" Text="ID пользователя" Width="30" />
                                        <ext:Column ID="PlansTitleUser" runat="server" DataIndex="title_user" Text="Пользователь" Width="150" />                                      
                                    </Columns>            
                                </ColumnModel>
                                <Plugins>
                                    <ext:RowEditing ID="RowEditingPlans" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins>
                            </ext:GridPanel>
                            <ext:GridPanel ID="SurveyListGP" runat="server" Layout="FitLayout" Flex="2" MultiSelect="false" AutoScroll="true" Height="523">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>   
                                <TopBar>
                                    <ext:Toolbar ID="SurveyListTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddSurveyListB" runat="server" Icon="Add" Text="Добавить" />
                                            <ext:Button ID="PrintStickersB" runat="server" Icon="Printer" Text="Этикетки" />
                                            <ext:Button ID="PrintForm1B" runat="server" Icon="Printer" Text="Форма 1" />
                                            <ext:Button ID="PrintForm2B" runat="server" Icon="Printer" Text="Форма 2" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>                            
                                <Store>
                                    <ext:Store ID="SurveyListS" runat="server">
                                        <Model>
                                            <ext:Model ID="SurveyListM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_survey_list"/>
                                                    <ext:ModelField Name="id_plan"/>
                                                    <ext:ModelField Name="id_organization"/>
                                                    <ext:ModelField Name="title_organization"/>
                                                    <ext:ModelField Name="planned_area"/>
                                                    <ext:ModelField Name="planned_probes"/>
                                                    <ext:ModelField Name="planned_cuts"/>
                                                    <ext:ModelField Name="planned_floor_pits"/>
                                                    <ext:ModelField Name="planned_excavation"/>
                                                    <ext:ModelField Name="actual_area"/>
                                                    <ext:ModelField Name="actual_probes"/>
                                                    <ext:ModelField Name="actual_cuts"/>
                                                    <ext:ModelField Name="actual_floor_pits"/>
                                                    <ext:ModelField Name="actual_excavation"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                                                       
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="SurveyListIdSL" runat="server" DataIndex="id_survey_list" Hidden="true" Text="ID обследования" Width="30" SummaryType="None" /> 
                                        <ext:Column ID="SurveyListIdPlan" runat="server" DataIndex="id_plan" Hidden="true" Text="ID плана" Width="30" SummaryType="None" />
                                        <ext:Column ID="SurveyListIdOrg" runat="server" DataIndex="id_organization" Hidden="true" Text="ID организации" Width="30" SummaryType="None" />
                                        <ext:Column ID="SurveyListTitleOrg" runat="server" DataIndex="title_organization" Text="Название организации" Width="200" SummaryType="Count">
                                            <Editor>
                                                <ext:ComboBox ID="CombSurveyListOrgCB" runat="server" DisplayField="title_organization" ValueField="title_organization" AllowBlank="false" Editable="false">
                                                    <Store>
                                                        <ext:Store ID="CombSurveyListOrgS" runat="server">
                                                            <Model>
                                                                <ext:Model ID="CombSurveyListOrgM" runat="server">
                                                                    <Fields>
                                                                        <ext:ModelField Name="id_organization" />
                                                                        <ext:ModelField Name="code_organization" />
                                                                        <ext:ModelField Name="title_organization" />
                                                                    </Fields>
                                                                </ext:Model>
                                                            </Model>
                                                        </ext:Store>
                                                    </Store>
                                                    <ListConfig  LoadingText="Загрузка..." MinWidth="400" Border="true">
                                                        <Tpl ID="Tpl1" runat="server">
                                                            <html>
					                                            <tpl for=".">
					                                        	    <table class="cbStates-list">
					                                        	        <tr class="x-boundlist-item">
					                                        		        <td>{code_organization}</td>
					                                        		        <td>{title_organization}</td>
					                                        	        </tr>
					                                        	    </table>
					                                            </tpl>
				                                            </html>
                                                        </Tpl> 
                                                    </ListConfig>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ID="SurveyListPlannedValues" runat="server" Text="Планируемые значения">
                                            <Columns>
                                                <ext:Column ID="SurveyListPlannedArea" runat="server" DataIndex="planned_area" Text="Площадь" Width="60" SummaryType="Sum">
                                                    <Editor>
                                                        <ext:TextField ID="EditSurveyListPlannedAreaTF" runat="server" MaskRe="/[0-9.,]/" />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="SurveyListPlannedProbes" runat="server" DataIndex="planned_probes" Text="Проб" Width="60" SummaryType="Sum">
                                                    <Editor>
                                                        <ext:TextField ID="EditSurveyListPlannedProbesTF" runat="server" MaskRe="/[0-9]/" />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="SurveyListPlannedCuts" runat="server" DataIndex="planned_cuts" Text="Разрезов" Width="60" SummaryType="Sum">
                                                    <Editor>
                                                        <ext:TextField ID="EditSurveyListPlannedCutsTF" runat="server" MaskRe="/[0-9]/" />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="SurveyListPlannedFloorPips" runat="server" DataIndex="planned_floor_pits" Text="Полуям" Width="60" SummaryType="Sum">
                                                    <Editor>
                                                        <ext:TextField ID="EditSurveyListPlannedFloorPipsTF" runat="server" MaskRe="/[0-9]/" />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="SurveyListPlannedExcavation" runat="server" DataIndex="planned_excavation" Text="Прикопок" Width="60" SummaryType="Sum">
                                                    <Editor>
                                                        <ext:TextField ID="EditSurveyListPlannedExcavationTF" runat="server" MaskRe="/[0-9]/" />
                                                    </Editor>
                                                </ext:Column>
                                            </Columns>
                                        </ext:Column>
                                        <ext:Column ID="SurveyListActualValues" runat="server" Text="Фактические значения">
                                            <Columns>
                                                <ext:Column ID="SurveyListActualArea" runat="server" DataIndex="actual_area" Text="Площадь" Width="60" SummaryType="Sum">
                                                    <Editor>
                                                        <ext:TextField ID="EditSurveyListActualAreaTF" runat="server" MaskRe="/[0-9.,]/" />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="SurveyListActualProbes" runat="server" DataIndex="actual_probes" Text="Проб" Width="60" SummaryType="Sum">
                                                    <Editor>
                                                        <ext:TextField ID="EditSurveyListActualProbesTF" runat="server" MaskRe="/[0-9]/" />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="SurveyListActualCuts" runat="server" DataIndex="actual_cuts" Text="Разрезов" Width="60" SummaryType="Sum">
                                                    <Editor>
                                                        <ext:TextField ID="EditSurveyListActualCutsTF" runat="server" MaskRe="/[0-9]/" />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="SurveyListActualFloorPips" runat="server" DataIndex="actual_floor_pits" Text="Полуям" Width="60" SummaryType="Sum">
                                                    <Editor>
                                                        <ext:TextField ID="EditSurveyListActualFloorPipsTF" runat="server" MaskRe="/[0-9]/" />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="SurveyListActualExcavation" runat="server" DataIndex="actual_excavation" Text="Прикопок" Width="60" SummaryType="Sum">
                                                    <Editor>
                                                        <ext:TextField ID="EditSurveyListActualExcavationTF" runat="server" MaskRe="/[0-9]/" />
                                                    </Editor>
                                                </ext:Column>
                                            </Columns>
                                        </ext:Column>
                                        <ext:ImageCommandColumn ID="SurveyListDeleteIC" runat="server" Width="25" Sortable="false" SummaryType="None">
                                            <Commands>
                                                <ext:ImageCommand Icon="Decline" ToolTip-Text="Удалить образец" CommandName="DeleteSurveyList" />
                                            </Commands>
                                        </ext:ImageCommandColumn>                                         
                                    </Columns>            
                                </ColumnModel>
                                <View>
                                    <ext:GridView ID="SurveyListGV" runat="server" StripeRows="true" MarkDirty="false" />
                                </View>
                                <Features>               
                                    <ext:GroupingSummary ID="SurveyListTotalGS" runat="server" GroupHeaderTplString="{id_plan}" HideGroupedHeader="true" EnableNoGroups="true" EnableGroupingMenu="true" ShowSummaryRow="true" />
                                    <ext:Summary ID="SurveyListSummary" runat="server" Dock="Bottom" />
                                </Features>
                                <Plugins>
                                    <ext:RowEditing ID="RowEditingSurveyList" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                </Plugins>
                            </ext:GridPanel>
                        </Items>                                          
                    </ext:Panel>
                </Items>  
            </ext:Window>
            <ext:Window ID="ReportAnalysW" runat="server" Title="Форма отчета по анализам" Width="430" Height="265"  Modal="true" Hidden="true" Layout="VBoxLayout" ButtonAlign="Center" Resizable="false">                 
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                </LayoutConfig>
                <Items>
                    <ext:Panel ID="ReportAnalysP1" runat="server" Frame="true" Layout="FormLayout">
                        <Items>                                   
                            <ext:ComboBox ID="ReportAnalysRegionCB" runat="server" FieldLabel="Район" LabelCls="darkslateblue-note" LabelWidth="70" DisplayField="title_region" ValueField="id_region" Editable="false">
                                 <Store>
                                     <ext:Store ID="ReportAnalysRegionS" runat="server">
                                         <Model>
                                             <ext:Model ID="ReportAnalysRegionM" runat="server">
                                                 <Fields>
                                                     <ext:ModelField Name="id_region" />
                                                     <ext:ModelField Name="title_region" />
                                                 </Fields>
                                             </ext:Model>
                                         </Model>
                                     </ext:Store>
                                 </Store>
                            </ext:ComboBox>
                            <ext:ComboBox ID="ReportAnalysOrganizationCB" runat="server" FieldLabel="Хозяйство" LabelCls="darkslateblue-note" LabelWidth="70" DisplayField="title_organization" ValueField="id_organization" Editable="false">
                                 <Store>
                                     <ext:Store ID="ReportAnalysOrganizationS" runat="server">
                                         <Model>
                                             <ext:Model ID="ReportAnalysOrganizationM" runat="server">
                                                 <Fields>
                                                     <ext:ModelField Name="id_organization" />
                                                     <ext:ModelField Name="title_organization" />
                                                 </Fields>
                                             </ext:Model>
                                         </Model>
                                     </ext:Store>
                                 </Store>
                             </ext:ComboBox>
                            <ext:ComboBox ID="ReportAnalysDepartmentCB" runat="server" FieldLabel="Отделение" LabelCls="darkslateblue-note" LabelWidth="70" DisplayField="title_department" ValueField="id_department" Editable="false">
                                 <Store>
                                     <ext:Store ID="ReportAnalysDepartmentS" runat="server">
                                         <Model>
                                             <ext:Model ID="ReportAnalysDepartmentM" runat="server">
                                                 <Fields>
                                                     <ext:ModelField Name="id_department" />
                                                     <ext:ModelField Name="title_department" />
                                                 </Fields>
                                             </ext:Model>
                                         </Model>
                                     </ext:Store>
                                 </Store>
                             </ext:ComboBox>
                        </Items>                              
                    </ext:Panel>
                    <ext:Panel ID="ReportAnalysP2" runat="server" Frame="true" Layout="HBoxLayout">  
                        <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                        </LayoutConfig>                     
                        <Items>
                            <ext:ComboBox ID="ReportAnalysTourCB" runat="server" FieldLabel="Цикл" LabelCls="darkslateblue-note" LabelWidth="30" DisplayField="tour" ValueField="tour" TabIndex="1" Width="80">
                                <Store>
                                    <ext:Store ID="ReportAnalysTourS" runat="server">
                                        <Model>
                                            <ext:Model ID="ReportAnalysTourM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="tour" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                            <ext:Component ID="Component77" runat="server" Width="5" />  
                            <ext:ComboBox ID="ReportAnalysYearCB" runat="server" FieldLabel="Год" LabelCls="darkslateblue-note" LabelWidth="30" DisplayField="year" ValueField="year" TabIndex="2" Width="95">
                                <Store>
                                    <ext:Store ID="ReportAnalysYearS" runat="server">
                                        <Model>
                                            <ext:Model ID="ReportAnalysYearM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="year" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>                       
                            <ext:Component ID="Component78" runat="server" Width="5" />                                                    
                            <ext:ComboBox ID="ReportAnalysElementCB" runat="server" FieldLabel="Элемент" LabelCls="darkslateblue-note" LabelWidth="60" DisplayField="element" ValueField="id_element" TabIndex="3" Width="200" Editable="false">
                                <Store>
                                    <ext:Store ID="ReportAnalysElementS" runat="server">
                                        <Model>
                                            <ext:Model ID="ReportAnalysElementM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_element" />
                                                    <ext:ModelField Name="element" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>                                                                                                                                                                    
                        </Items>                                          
                    </ext:Panel>
                    <ext:Panel ID="ReportAnalysP3" runat="server" Frame="true" Layout="VBoxLayout">  
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                        </LayoutConfig>                     
                        <Items>                             
                            <ext:Label ID="ReportAnalysL" runat="server" Text="Вывести отчет по дате" />               
                            <ext:Component ID="Component79" runat="server" Width="5" /> 
                            <ext:FieldContainer ID="FieldContainer10" runat="server" Layout="HBoxLayout">
                                <Items>                                      
                                    <ext:DateField ID="ReportAnalysDF1" runat="server" FieldLabel="с" LabelWidth="10" TabIndex="3" Width="190" Editable="false" />                
                                    <ext:Component ID="Component80" runat="server" Width="10" />                                   
                                    <ext:DateField ID="ReportAnalysDF2" runat="server" Vtype="daterange" FieldLabel="по" LabelWidth="20" TabIndex="3" Width="200" Editable="false" />  
                                </Items>
                            </ext:FieldContainer>
                    </Items>                                          
                    </ext:Panel>                                          
                </Items>
                <Buttons>
                    <ext:Button ID="ResetReportAnalysB" runat="server" Text="Сбросить" Icon="Reload" />
                    <ext:Button ID="AcceptReportAnalysB" runat="server" Text="Отчёт" Icon="Accept" />
                    <ext:Button ID="AcceptReportAnalysControlB" runat="server" Text="Констроль" Icon="Accept" />
                    <ext:Button ID="CancelReportAnalysB" runat="server" Text="Отменить" Icon="Cancel" />
                </Buttons>
            </ext:Window>
            <ext:Window ID="ReportPlanW" runat="server" Title="Форма отчета по планам" Width="470" Height="230"  Modal="true" Hidden="true" Layout="VBoxLayout" ButtonAlign="Center" Resizable="false">                 
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                </LayoutConfig>
                <Items>
                    <ext:Panel ID="ReportPlanP1" runat="server" Frame="true" Layout="FormLayout">
                               <Items>                                   
                                   <ext:ComboBox ID="ReportPlanRegionCB" runat="server" FieldLabel="Район" LabelCls="darkslateblue-note" LabelWidth="90" DisplayField="title_region" ValueField="id_region" Editable="false">
                                        <Store>
                                            <ext:Store ID="ReportPlanRegionS" runat="server">
                                                <Model>
                                                    <ext:Model ID="ReportPlanRegionM" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="id_region" />
                                                            <ext:ModelField Name="title_region" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                   <ext:ComboBox ID="ReportPlanWorkerCB" runat="server" FieldLabel="Работник" LabelCls="darkslateblue-note" LabelWidth="90" DisplayField="user_editor" ValueField="id_user" Editable="false">
                                        <Store>
                                            <ext:Store ID="ReportPlanWorkerS" runat="server">
                                                 <Model>
                                                    <ext:Model ID="ReportPlanWorkerM" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="id_user" />
                                                            <ext:ModelField Name="user_editor" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                   </ext:ComboBox>
                                   <ext:ComboBox ID="ReportPlanMissionsCB" runat="server" FieldLabel="Задание" LabelCls="darkslateblue-note" LabelWidth="90" DisplayField="title_mission" ValueField="id_mission" Editable="false">
                                        <Store>
                                            <ext:Store ID="ReportPlanMissionsS" runat="server">
                                                <Model>
                                                    <ext:Model ID="ReportPlanMissionsM" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="id_mission" />
                                                            <ext:ModelField Name="title_mission" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                   </ext:ComboBox>                                   
                               </Items>                                          
                            </ext:Panel>                    
                    <ext:Panel ID="ReportPlanP2" runat="server" Frame="true" Layout="VBoxLayout">  
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                        </LayoutConfig>                     
                        <Items>                             
                            <ext:Label ID="ReportPlanL1" runat="server" Text="Вывести отчет по дате" />               
                            <ext:Component ID="Component93" runat="server" Width="5" /> 
                            <ext:FieldContainer ID="FieldContainer12" runat="server" Layout="HBoxLayout">
                                <Items>                                      
                                    <ext:DateField ID="ReportPlanDF1" runat="server" FieldLabel="с" LabelWidth="10" TabIndex="3" Width="190" Editable="false" />                
                                    <ext:Component ID="Component94" runat="server" Width="10" />                                   
                                    <ext:DateField ID="ReportPlanDF2" runat="server" Vtype="daterange" FieldLabel="по" LabelWidth="20" TabIndex="3" Width="200" Editable="false" />  
                                </Items>
                            </ext:FieldContainer>
                    </Items>                                          
                    </ext:Panel>                                          
                </Items>
                <Buttons>
                    <ext:Button ID="ResetReportPlanB" runat="server" Text="Сбросить" Icon="Reload" />
                    <ext:Button ID="AcceptReportDriverB" runat="server" Text="Отчёт по водителям" Icon="Accept" />
                    <ext:Button ID="AcceptReportPlanB" runat="server" Text="Отчёт" Icon="Accept" />
                    <ext:Button ID="CancelReportPlanB" runat="server" Text="Отменить" Icon="Cancel" />
                </Buttons>
            </ext:Window>
            <ext:Window ID="RegionsTourForReportW" runat="server" Title="Выбор циклов обследования районов" Width="300" Height="600"  Modal="true" Hidden="true" Layout="VBoxLayout" ButtonAlign="Center" Resizable="false">                 
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                </LayoutConfig>
                <Items>
                    <ext:GridPanel ID="RegionsTourForReportGP" runat="server" Layout="FitLayout" Flex="1" MultiSelect="false" AutoScroll="true">
                        <SelectionModel><ext:RowSelectionModel runat="server"></ext:RowSelectionModel></SelectionModel>                              
                        <Store>
                            <ext:Store ID="RegionsTourForReportS" runat="server">
                                <Model>
                                    <ext:Model ID="RegionsTourForReportM" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="id_region"/>
                                            <ext:ModelField Name="title_region"/>
                                            <ext:ModelField Name="tour"/>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>                                                       
                        <ColumnModel>
                            <Columns>
                                <ext:Column ID="RegionsTourForReportIdRegion" runat="server" DataIndex="id_region" Hidden="true" Text="ID Района" Width="30" /> 
                                <ext:Column ID="RegionsTourForReportTitleRegion" runat="server" DataIndex="title_region" Text="Название района" Width="200" />
                                <ext:Column ID="RegionsTourForReportTour" runat="server" DataIndex="tour" Text="Цикл" Width="50">
                                    <Editor>
                                        <ext:ComboBox ID="EditRegionsTourForReportTourCB" runat="server" DisplayField="tour" ValueField="tour" AllowBlank="false" Editable="false">
                                            <Store>
                                                <ext:Store ID="EditRegionsTourForReportTourS" runat="server">
                                                    <Model>
                                                        <ext:Model ID="EditRegionsTourForReportM" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="tour" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Editor>
                                </ext:Column>                                 
                            </Columns>            
                        </ColumnModel>
                        <Plugins>
                            <ext:CellEditing ID="CellEditingRegionsTourForReport" runat="server" ClicksToEdit="2" />
                        </Plugins>
                    </ext:GridPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="GetRegionsTourForReportB" runat="server" Text="Отчёт" Icon="Accept" Hidden="true" />
                    <ext:Button ID="GetRegionsTourForReportHMB" runat="server" Text="Отчёт" Icon="Accept" Hidden="true" />
                    <ext:Button ID="CancelRegionsTourForReportB" runat="server" Text="Отмена" Icon="Cancel" />
                </Buttons>
            </ext:Window>
            <ext:Window ID="AnalysToPlotW" runat="server" Icon="ArrowJoin" Title="Синхронизация данных"  MinWidth="600" MinHeight="500" Width="600" Height="500"  Modal="true" Maximizable="true" Hidden="true" Layout="BorderLayout" Resizable="true" ButtonAlign="Center">              
                <Items>
                    <ext:Panel ID="AnalysToPlotP1" runat="server" Frame="true" Layout="VBoxLayout" Region="North">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                        </LayoutConfig>
                        <Items>
                            <ext:ComboBox ID="AnalysToPlotRegionCB" runat="server" FieldLabel="Район" LabelCls="darkslateblue-note" LabelWidth="80" DisplayField="title_region" ValueField="id_region" Editable="false">
                                 <Store>
                                     <ext:Store ID="AnalysToPlotRegionS" runat="server">
                                         <Model>
                                             <ext:Model ID="AnalysToPlotRegionM" runat="server">
                                                 <Fields>
                                                     <ext:ModelField Name="id_region" />
                                                     <ext:ModelField Name="title_region" />
                                                 </Fields>
                                             </ext:Model>
                                         </Model>
                                     </ext:Store>
                                 </Store>
                            </ext:ComboBox>
                            <ext:ComboBox ID="AnalysToPlotOrganizationCB" runat="server" FieldLabel="Организация" LabelCls="darkslateblue-note" LabelWidth="80" DisplayField="title_organization" ValueField="id_organization" Editable="false">
                                 <Store>
                                     <ext:Store ID="AnalysToPlotOrganizationS" runat="server">
                                         <Model>
                                             <ext:Model ID="AnalysToPlotOrganizationM" runat="server">
                                                 <Fields>
                                                     <ext:ModelField Name="id_organization" />
                                                     <ext:ModelField Name="id_region" />
                                                     <ext:ModelField Name="title_organization" />
                                                 </Fields>
                                             </ext:Model>
                                         </Model>
                                     </ext:Store>
                                 </Store>
                            </ext:ComboBox>
                            <ext:ComboBox ID="AnalysToPlotDepartmentCB" runat="server" FieldLabel="Отделение" LabelCls="darkslateblue-note" LabelWidth="80" DisplayField="title_department" ValueField="id_department" Editable="false">
                                 <Store>
                                     <ext:Store ID="AnalysToPlotDepartmentS" runat="server">
                                         <Model>
                                             <ext:Model ID="AnalysToPlotDepartmentM" runat="server">
                                                 <Fields>
                                                     <ext:ModelField Name="id_department" />
                                                     <ext:ModelField Name="id_organization" />
                                                     <ext:ModelField Name="title_department" />
                                                 </Fields>
                                             </ext:Model>
                                         </Model>
                                     </ext:Store>
                                 </Store>
                            </ext:ComboBox>
                            <ext:FieldContainer ID="FieldContainer15" runat="server" Layout="HBoxLayout">
                                <Items>
                                    <ext:ComboBox ID="AnalysToPlotTourCB" runat="server" FieldLabel="Цикл" LabelCls="darkslateblue-note" LabelWidth="80" DisplayField="tour" ValueField="tour" Editable="false">
                                        <Store>
                                            <ext:Store ID="AnalysToPlotTourS" runat="server">
                                                <Model>
                                                    <ext:Model ID="AnalysToPlotTourM" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="tour" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>            
                                    <ext:Component ID="Component95" runat="server" Width="28" />                                   
                                    <ext:ComboBox ID="AnalysToPlotYearCB" runat="server" FieldLabel="Год" LabelCls="darkslateblue-note" LabelWidth="80" DisplayField="year" ValueField="year" Editable="false">
                                        <Store>
                                            <ext:Store ID="AnalysToPlotYearS" runat="server">
                                                <Model>
                                                    <ext:Model ID="AnalysToPlotYearM" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="year" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                    <ext:Component ID="Component96" runat="server" Width="28" /> 
                                    <ext:Button ID="AnalysToPlotResetB" runat="server" Text="Сбросить"/>
                                </Items>
                            </ext:FieldContainer>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="AnalysToPlotP2" runat="server" Frame="true" Layout="HBoxLayout" Flex="1" Region="Center">
                        <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                        </LayoutConfig>
                        <Items>
                            <ext:GridPanel ID="AnalysToPlotPlotsGP" runat="server" Layout="FitLayout" Flex="1" MultiSelect="false" AutoScroll="true" InvalidateScrollerOnRefresh="False">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>                             
                                <Store>
                                    <ext:Store ID="AnalysToPlotPlotsS" runat="server">
                                        <Model>
                                            <ext:Model ID="AnalysToPlotPlotsM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_plot"/>
                                                    <ext:ModelField Name="id_department"/>
                                                    <ext:ModelField Name="number_plot"/>
                                                    <ext:ModelField Name="tour"/>
                                                    <ext:ModelField Name="year"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                                                       
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="AnalysToPlotPlotsIdPlotC" runat="server" DataIndex="id_plot" Hidden="true" Text="ID участка" Width="30" />
                                        <ext:Column ID="AnalysToPlotPlotsIdDepartmentC" runat="server" DataIndex="id_department" Hidden="true" Text="ID отделения" Width="30" />
                                        <ext:Column ID="AnalysToPlotPlotsNumberPlotC" runat="server" DataIndex="number_plot" Text="№ участка" Width="70" />
                                        <ext:Column ID="AnalysToPlotPlotsTourC" runat="server" DataIndex="tour" Text="Цикл" Width="50" />
                                        <ext:Column ID="AnalysToPlotPlotsYearC" runat="server" DataIndex="year" Text="Год" Width="50" />        
                                    </Columns>            
                                </ColumnModel>
                            </ext:GridPanel>
                            <ext:GridPanel ID="AnalysToPlotAnalysesGP" runat="server" Layout="FitLayout" Flex="1" MultiSelect="true" AutoScroll="true" InvalidateScrollerOnRefresh="False">
                                <SelectionModel><ext:RowSelectionModel runat="server" Mode="Multi" /></SelectionModel>                    
                                <Store>
                                    <ext:Store ID="AnalysToPlotAnalysesS" runat="server">
                                        <Model>
                                            <ext:Model ID="AnalysToPlotAnalysesM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_sample"/>
                                                    <ext:ModelField Name="id_department"/>
                                                    <ext:ModelField Name="number_plot"/>
                                                    <ext:ModelField Name="number_sample"/>
                                                    <ext:ModelField Name="tour"/>
                                                    <ext:ModelField Name="year"/>
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>                                                       
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column ID="AnalysToPlotAnalysesIdSampleC" runat="server" DataIndex="id_sample" Hidden="true" Text="ID образца" Width="30" />
                                        <ext:Column ID="AnalysToPlotAnalysesIdDepartmentC" runat="server" DataIndex="id_department" Hidden="true" Text="ID отделения" Width="30" />
                                        <ext:Column ID="AnalysToPlotAnalysesNumberPlotC" runat="server" DataIndex="number_plot" Text="№ участка" Width="70" />
                                        <ext:Column ID="AnalysToPlotAnalysesNumberSampleC" runat="server" DataIndex="number_sample" Text="№ образца" Width="70" />
                                        <ext:Column ID="AnalysToPlotAnalyseTourC" runat="server" DataIndex="tour" Text="Цикл" Width="50" />
                                        <ext:Column ID="AnalysToPlotAnalyseYearC" runat="server" DataIndex="year" Text="Год" Width="50" />           
                                    </Columns>            
                                </ColumnModel>
                            </ext:GridPanel>
                        </Items>                                          
                    </ext:Panel>
                </Items>
                <Buttons>
                    <ext:Button ID="AnalysToPlotSetNumberPlotB" runat="server" Text="Установить номер участка" Icon="ArrowRight" />
                    <ext:Button ID="AnalysToPlotAcceptB" runat="server" Text="Синхронизировать данные" Icon="Accept" />
                    <ext:Button ID="AnalysToPlotCancelB" runat="server" Text="Отменить" Icon="Cancel" />
                </Buttons>
            </ext:Window>
        <ext:Window ID="RegionsYearForReportW" runat="server" Title="Выбор годов обследования районов" Width="300" Height="600"  Modal="true" Hidden="true" Layout="VBoxLayout" ButtonAlign="Center" Resizable="false">                 
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />
                </LayoutConfig>
                <Items>
                    <ext:GridPanel ID="RegionsYearForReportGP" runat="server" Layout="FitLayout" Flex="1" MultiSelect="false" AutoScroll="true">
                        <SelectionModel><ext:RowSelectionModel runat="server"></ext:RowSelectionModel></SelectionModel>                              
                        <Store>
                            <ext:Store ID="RegionsYearForReportS" runat="server">
                                <Model>
                                    <ext:Model ID="RegionsYearForReportM" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="id_region"/>
                                            <ext:ModelField Name="title_region"/>
                                            <ext:ModelField Name="year"/>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>                                                       
                        <ColumnModel>
                            <Columns>
                                <ext:Column ID="RegionsYearForReportIdRegion" runat="server" DataIndex="id_region" Hidden="true" Text="ID Района" Width="30" /> 
                                <ext:Column ID="RegionsYearForReportTitleRegion" runat="server" DataIndex="title_region" Text="Название района" Width="200" />
                                <ext:Column ID="RegionsYearForReportYear" runat="server" DataIndex="year" Text="Год" Width="50">
                                    <Editor>
                                        <ext:ComboBox ID="EditRegionsYearForReportYearCB" runat="server" DisplayField="year" ValueField="year" AllowBlank="false" Editable="false">
                                            <Store>
                                                <ext:Store ID="EditRegionsYearForReportYearS" runat="server">
                                                    <Model>
                                                        <ext:Model ID="EditRegionsYearForReportYearM" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="year" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Editor>
                                </ext:Column>        
                            </Columns>            
                        </ColumnModel>
                        <Plugins>
                            <ext:CellEditing ID="CellEditingRegionsYearForReport" runat="server" ClicksToEdit="2" />
                        </Plugins>
                    </ext:GridPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="GetRegionsYearForReportB" runat="server" Text="Отчёт" Icon="Accept" Hidden="true" />
                    <ext:Button ID="CancelRegionsYearForReportB" runat="server" Text="Отмена" Icon="Cancel" />
                </Buttons>
            </ext:Window>
        <ext:Window ID="MapW" runat="server" Title="Карта" Width="1050" Height="850" Modal="true" Hidden="true" Layout="BorderLayout" Maximized="true" Header="false" Border="false" Shadow="false">
            <Loader ID="MapLoader" runat="server" Url="OLGISMap.aspx" Mode="Frame">
                <LoadMask ShowMask="true" />
            </Loader>
        </ext:Window>
        <ext:Window ID="StatisticsAreaRegionW" runat="server" Title="Статистика районов по площадям" Hidden="true" Modal="true" Layout="FitLayout" Width="570" Height="700" Padding="3" ButtonAlign="Center" Maximizable="false">
            <Items>
            <ext:GridPanel ID="StatisticsAreaRegionGP" runat="server" Layout="FitLayout" AutoScroll="true" MultiSelect="false" EnableColumnHide="false">
                            <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                    <TopBar>
                        <ext:Toolbar ID="StatisticsAreaRegionTB" runat="server">
                        </ext:Toolbar>
                    </TopBar>
                            <Store>
                                <ext:Store ID="StatisticsAreaRegionS" runat="server">
                                    <Model>
                                        <ext:Model ID="StatisticsAreaRegionM" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="id_statistic_region" Type="Int" />
                                                <ext:ModelField Name="id_region" Type="Int" />
                                                <ext:ModelField Name="title_region" Type="String" />
                                                <ext:ModelField Name="record_date" Type="Date" />
                                                <ext:ModelField Name="sum_area" Type="Float" />                                                
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <ColumnModel>
                                <Columns>
                                    <ext:Column ID="Id_statistic_regionC" runat="server" DataIndex="id_statistic_region" Text="№ записи статистики" Width="100" Hidden="true" />
                                    <ext:Column ID="Id_regionC" runat="server" DataIndex="id_region" Text="Id района" Width="100" Hidden="true" />
                                    <ext:Column ID="Title_regionC" runat="server" DataIndex="title_region" Text="Район" Width="300" />
                                    <ext:DateColumn ID="Statistics_area_region_record_dateDC" runat="server" DataIndex="record_date" Text="Дата записи" Width="150" Align="Center" Format="dd.MM.yyyy" />
                                    <ext:Column ID="AreaC" runat="server" DataIndex="sum_area" Text="Площадь" Width="120" />
                                </Columns>        
                            </ColumnModel>
                            <Features>
                                <ext:GridFilters runat="server" ID="StatisticsAreaRegionGF" Local="true">
                                    <Filters>
                                        <ext:NumericFilter DataIndex="id_statistic_region" />
                                        <ext:NumericFilter DataIndex="id_region" />
                                        <ext:StringFilter DataIndex="title_region" />
                                        <ext:DateFilter DataIndex="record_date">
                                            <DatePickerOptions runat="server" TodayText="Сегодня" Format="dd.MM.yyyy" />
                                        </ext:DateFilter>
                                        <ext:NumericFilter DataIndex="sum_area" />                                        
                                    </Filters>
                                </ext:GridFilters>
                            </Features> 
                        </ext:GridPanel>
                </Items>
        </ext:Window>
    </form>
</body>
</html>