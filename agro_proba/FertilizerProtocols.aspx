<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FertilizerProtocols.aspx.cs" Inherits="agro_proba.FertilizerProtocols" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Агрохимик Online - Органические удобрения</title>
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
    <form id="form1" runat="server">
    <asp:ScriptManager ID="MainSM" runat="server" AsyncPostBackTimeout="0"></asp:ScriptManager>
    <ext:ResourceManager ID="MainRM" runat="server" DisableViewState="true" AjaxTimeout="300000" />
    <ext:Viewport ID="IndexViewport" runat="server" Layout="BorderLayout" Hidden="false">
        <Items>
            <ext:Panel runat="server" Region="North" Height="26" ID="IndexToolP">
               <TopBar>
                <ext:Toolbar ID="IndexToolbar" runat="server">
                    <Items>
                        <ext:Button ID="ReportProtocolsFertilizerB" runat="server" Text="Сводный отчёт по протоколам" Icon="Report" />
                        <ext:Button ID="ReportResultsProtocolsStatisticsB" runat="server" Text="Сводный отчёт по результатам протоколов" Icon="Report" />
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
                            <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" AutoDataBind="true" Text="<%# DateTime.Now.Date %>" />
                        </Items>
                    </ext:StatusBar>
                </Items>
            </ext:Panel>
            <ext:Panel ID="FarmOrganizationP" runat="server" Region="West" Title="Организации/поставщики удобрений" Width="325" MinWidth="150" MaxWidth="600" Split="true" Layout="FitLayout" AutoScroll="true">
                <Items>
                    <ext:GridPanel ID="FarmOrganizationGP" runat="server" Layout="FitLayout" AutoScroll="true" MultiSelect="false">
                        <SelectionModel><ext:RowSelectionModel ID="FarmOrganizationRSM"></ext:RowSelectionModel></SelectionModel>
                        <TopBar>
                            <ext:Toolbar ID="FarmOrganizationTB" runat="server">
                                <Items>
                                    <ext:Button ID="AddFarmOrganizationB" runat="server" Icon="Add" Text="Добавить" />
                                    <ext:ToolbarFill ID="FarmOrganizationTFill" runat="server" />
                                    <ext:Button ID="DeleteFarmOrganizationB" runat="server" Icon="Delete" Text="Удалить" />
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="FarmOrganizationS" runat="server">
                                <Model>
                                    <ext:Model ID="FarmOrganizationM" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="id_farm_organization"/>
                                            <ext:ModelField Name="title_farm_organization"/>
                                            <ext:ModelField Name="address"/>
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column ID="Id_farm_org" runat="server" DataIndex="id_farm_organization" Hidden="true" Text="Id организации" />
                                <ext:Column ID="Title_farm_org" runat="server" Text="Наименование" DataIndex="title_farm_organization" Width="150">
                                    <Editor>
                                        <ext:TextField ID="Title_farm_org_TF" runat="server" AllowBlank="false" />
                                    </Editor>
                                </ext:Column>
                                <ext:Column ID="Address" runat="server" Text="Адрес" DataIndex="address" Width="170">
                                    <Editor>
                                        <ext:TextField ID="Address_TF" runat="server" AllowBlank="true" />
                                    </Editor>
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <Plugins>
                            <ext:RowEditing ID="Farm_org_RE" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                        </Plugins>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
            <ext:Panel ID="CentralP" runat="server" Region="Center" MinWidth="600" Split="true" Layout="BorderLayout" AutoScroll="true">
                <Items>
                    <ext:Panel ID="FarmsLagoonsP" runat="server" Region="North" Split="true" Height="310" MinHeight="200" Layout="BorderLayout" AutoScroll="true">
                        <Items>
                            <ext:Panel ID="FarmP" runat="server" Region="Center" Title="Площадки" Layout="VBoxLayout" AutoScroll="true" Split="true">
                                <LayoutConfig>
                                    <ext:VBoxLayoutConfig Align="Stretch" />                    
                                </LayoutConfig>
                                <Items>
                                    <ext:FieldContainer runat="server" ID="RegionFC" Layout="HBoxLayout" >
                                        <LayoutConfig>
                                            <ext:HBoxLayoutConfig Align="Stretch" />
                                        </LayoutConfig>
                                        <Items>
                                            <ext:ComboBox ID="RegionCB" runat="server" FieldLabel="Район" LabelCls="darkslateblue-note" LabelWidth="40" DisplayField="title_region" 
                                                  ValueField="id_region" Editable="false" Padding="3" Width="400">
                                                <Store>
                                                    <ext:Store ID="RegionS" runat="server">
                                                        <Model>
                                                            <ext:Model ID="RegionM" runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="id_region" />
                                                                    <ext:ModelField Name="code_region" />
                                                                    <ext:ModelField Name="title_region" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>
                                            </ext:ComboBox>
                                            <ext:Button ID="ResetRegionB" runat="server" Icon="Reload" Text="Очистить" />
                                        </Items>
                                    </ext:FieldContainer>
                                    <ext:GridPanel ID="FarmGP" runat="server" Layout="FitLayout" AutoScroll="true" MultiSelect="false" Flex="1">
                                        <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                        <TopBar>
                                            <ext:Toolbar ID="FarmTB" runat="server">
                                                <Items>
                                                    <ext:Button ID="AddFarmB" runat="server" Icon="Add" Text="Добавить" />
                                                    <ext:ToolbarFill ID="FarmTFill" runat="server" />
                                                    <ext:Button ID="DeleteFarmB" runat="server" Icon="Delete" Text="Удалить" />
                                                </Items>
                                            </ext:Toolbar>
                                        </TopBar>
                                        <Store>
                                            <ext:Store ID="FarmS" runat="server">
                                                <Model>
                                                    <ext:Model ID="FarmM" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="id_farm" />
                                                            <ext:ModelField Name="id_region" />
                                                            <ext:ModelField Name="title_region" />
                                                            <ext:ModelField Name="id_type_farm" />
                                                            <ext:ModelField Name="code_type_farm" />
                                                            <ext:ModelField Name="title_type_farm" />
                                                            <ext:ModelField Name="number_farm" />
                                                            <ext:ModelField Name="title_farm" />
                                                            <ext:ModelField Name="location_farm" />
                                                            <ext:ModelField Name="animal_population" />
                                                            <ext:ModelField Name="lagoons_volume" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel>
                                            <Columns>            
                                                <ext:Column ID="Id_farm" runat="server" DataIndex="id_farm" Hidden="true" Text="Id комплекса" Width="50" />
                                                <ext:Column ID="Id_region" runat="server" DataIndex="id_region" Hidden="true" Text="Id района" Width="50" />
                                                <ext:Column ID="Title_region" runat="server" Text="Район" DataIndex="title_region" Width="150">
                                                    <Editor>
                                                        <ext:ComboBox ID="Title_region_CB" runat="server" DisplayField="title_region" ValueField="title_region" Editable="false" AllowBlank="false"> 
                                                            <Store>
                                                                <ext:Store ID="Title_region_S" runat="server">
                                                                    <Model>
                                                                        <ext:Model ID="Title_region_M" runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="title_region" />
                                                                                <ext:ModelField Name="title_region" />
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                </ext:Store>
                                                            </Store>
                                                        </ext:ComboBox> 
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="Id_type_farm" runat="server" DataIndex="id_type_farm" Hidden="true" Text="Id типа комплекса" Width="50" />
                                                <ext:Column ID="Code_type_farm" runat="server" Text="Код типа комплекса" DataIndex="code_type_farm" Width="100" Hidden="true" />
                                                <ext:Column ID="Title_type_farm" runat="server" Text="Тип комплекса" DataIndex="title_type_farm" Width="100">
                                                    <Editor>
                                                        <ext:ComboBox ID="Type_farm_CB" runat="server" DisplayField="title_type_farm" ValueField="title_type_farm" Editable="false" AllowBlank="false"> 
                                                            <Store>
                                                                <ext:Store ID="Type_farm_S" runat="server">
                                                                    <Model>
                                                                        <ext:Model ID="Type_farm_M" runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="id_type_farm" />
                                                                                <ext:ModelField Name="title_type_farm" />
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                </ext:Store>
                                                            </Store>
                                                        </ext:ComboBox> 
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="Number_farm" runat="server" Text="№ комплекса" DataIndex="number_farm"  Width="100">      
                                                    <Editor>
                                                        <ext:TextField ID="Number_farm_TF" runat="server" AllowBlank="false" />
                                                    </Editor>
                                                </ext:Column>                           
                                                <ext:Column ID="Title_farm" runat="server" Text="Наименование комплекса" DataIndex="title_farm" Width="250">
                                                    <Editor>
                                                        <ext:TextField ID="Title_farm_TF" runat="server" AllowBlank="true" />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="Location_farm" runat="server" DataIndex="location_farm" Text="Расположение комплекса" Width="400">
                                                    <Editor>
                                                        <ext:TextField ID="Location_farm_TF" runat="server" AllowBlank="true" />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="Animal_population" runat="server" DataIndex="animal_population" Text="Поголовье" Width="70">
                                                    <Editor>
                                                        <ext:NumberField ID="Animal_population_NF" runat="server" AllowBlank="true" />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="Lagoons_volume" runat="server" DataIndex="lagoons_volume" Text="Объём" Width="50" />
                                            </Columns>         
                                        </ColumnModel>
                                        <Plugins>
                                            <ext:RowEditing ID="Farm_RE" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                        </Plugins>
                                    </ext:GridPanel>
                                </Items>         
                            </ext:Panel>
                            <ext:Panel ID="LagoonsP" runat="server" Region="East" Title="Лагуны/Боксы" Width="420" MinWidth="280" Layout="VBoxLayout" AutoScroll="true" Split="true">
                                <LayoutConfig>
                                    <ext:VBoxLayoutConfig Align="Stretch" />                    
                                </LayoutConfig>
                                <Items>
                                    <ext:GridPanel ID="LagoonsGP" runat="server" Layout="FitLayout" AutoScroll="true" MultiSelect="false" Flex="1">
                                        <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                        <TopBar>
                                            <ext:Toolbar ID="LagoonsTB" runat="server">
                                                <Items>
                                                    <ext:Button ID="AddLagoonB" runat="server" Icon="Add" Text="Добавить" />
                                                    <ext:ToolbarFill ID="LagoonsTFill" runat="server" />
                                                    <ext:Button ID="DeleteLagoonB" runat="server" Icon="Delete" Text="Удалить" />
                                                </Items>
                                            </ext:Toolbar>
                                        </TopBar>
                                        <Store>
                                            <ext:Store ID="LagoonsS" runat="server">
                                                <Model>
                                                    <ext:Model ID="LagoonsM" runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="id_lagoon" />
                                                            <ext:ModelField Name="lagoon_number" />
                                                            <ext:ModelField Name="lagoon_name" />
                                                            <ext:ModelField Name="lagoon_volume" />
                                                            <ext:ModelField Name="id_type_lagoon" />
                                                            <ext:ModelField Name="code_type_lagoon" />
                                                            <ext:ModelField Name="title_type_lagoon" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>                                
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel>
                                            <Columns>            
                                                <ext:Column ID="Id_lagoon" runat="server" DataIndex="id_lagoon" Hidden="true" Text="Id лагуны" />
                                                <ext:Column ID="Lagoon_number" runat="server" DataIndex="lagoon_number" Text="№" Width="50">
                                                    <Editor>
                                                        <ext:TextField ID="Lagoon_number_TF" runat="server" AllowBlank="false" />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="Lagoon_name" runat="server" DataIndex="lagoon_name" Text="Название" Width="100">
                                                    <Editor>
                                                        <ext:TextField ID="Lagoon_name_TF" runat="server" AllowBlank="true" />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="Lagoon_volume" runat="server" Text="Объём" DataIndex="lagoon_volume" Width="100">
                                                    <Editor>
                                                        <ext:NumberField ID="Lagoon_volume_NF" runat="server" AllowBlank="false" />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column ID="TypeLagoon" runat="server" DataIndex="title_type_lagoon" Text="Тип" Width="150">
                                                    <Editor>
                                                        <ext:ComboBox ID="EditTypeLagoonCB" runat="server" DisplayField="title_type_lagoon" ValueField="title_type_lagoon" AllowBlank="false" Editable="false">
                                                            <Store>
                                                                <ext:Store ID="EditTypeLagoonS" runat="server">
                                                                    <Model>
                                                                        <ext:Model ID="EditTypeLagoonM" runat="server">
                                                                            <Fields>
                                                                                <ext:ModelField Name="id_type_lagoon" />
                                                                                <ext:ModelField Name="code_type_lagoon" />
                                                                                <ext:ModelField Name="title_type_lagoon" />
                                                                            </Fields>
                                                                        </ext:Model>
                                                                    </Model>
                                                                </ext:Store>
                                                            </Store>
                                                            <ListConfig  LoadingText="Загрузка..." MinWidth="100" Border="true">
                                                                <ItemTpl ID="ItemTpl5" runat="server">
                                                                    <Html><div class="search-item">{title_type_lagoon}</div></Html>
                                                                </ItemTpl>
                                                            </ListConfig>
                                                        </ext:ComboBox>
                                                    </Editor>
                                                </ext:Column>
                                            </Columns>            
                                        </ColumnModel>
                                        <Plugins>
                                            <ext:RowEditing ID="Lagoon_RE" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                                        </Plugins>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="ProtocolsP" runat="server" Region="Center" Title="Протоколы" Layout="FitLayout" AutoScroll="true" Split="true">
                        <Items>
                            <ext:GridPanel ID="ProtocolsGP" runat="server" Layout="FitLayout" AutoScroll="true" MultiSelect="false">
                                <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                                <TopBar>
                                    <ext:Toolbar ID="ProtocolsTB" runat="server">
                                        <Items>
                                            <ext:Button ID="AddProtocolB" runat="server" Icon="Add" Text="Добавить" />
                                            <ext:Button ID="EditProtocolB" runat="server" Icon="Pencil" Text="Редактировать" />
                                            <ext:Button ID="PrintProtocolB" runat="server" Icon="Printer" Text="Распечатать" />
                                            <ext:Button ID="CopyProtocolB" runat="server" Icon="ApplicationDouble" Text="Копировать" />
                                            <ext:Button ID="PasteProtocolB" runat="server" Icon="PastePlain" Text="Вставить" Hidden="true" />
                                            <ext:TextField ID="CopyIdProtocolTF" runat="server" Hidden="true" Text="0" />
                                            <ext:ToolbarFill ID="ProtocolsTFill" runat="server" />
                                            <ext:Button ID="DeleteProtocolB" runat="server" Icon="Delete" Text="Удалить"/>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="ProtocolsS" runat="server">
                                        <Model>
                                            <ext:Model ID="ProtocolsM" runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="id_protocol" />
                                                    <ext:ModelField Name="number_protocol" />
                                                    <ext:ModelField Name="id_fertilizer" />
                                                    <ext:ModelField Name="title_fertilizer" />
                                                    <ext:ModelField Name="organization_applicant_name" />
                                                    <ext:ModelField Name="batch_size" />
                                                    <ext:ModelField Name="sample_weight" />
                                                    <ext:ModelField Name="sample_sediment_weight" />
                                                    <ext:ModelField Name="accompanying_document" />
                                                    <ext:ModelField Name="sample_selecting_date" />
                                                    <ext:ModelField Name="sample_reception_date" />
                                                    <ext:ModelField Name="testing_time_begin" />
                                                    <ext:ModelField Name="testing_time_end" />
                                                    <ext:ModelField Name="normative_document_selection" />
                                                    <ext:ModelField Name="normative_document_testing" />
                                                    <ext:ModelField Name="date_input" />
                                                    <ext:ModelField Name="date_last_edit" />
                                                    <ext:ModelField Name="id_user" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <ColumnModel>
                                    <Columns>               
                                        <ext:Column ID="Id_protocol" runat="server" DataIndex="id_protocol" Hidden="true" Text="Id протокола" />   
                                        <ext:Column ID="Number_protocol" runat="server" DataIndex="number_protocol" Text="№ протокола" Width="100" />
                                        <ext:Column ID="Id_fertilizer" runat="server" DataIndex="id_fertilizer" Hidden="true" Text="Id удобрения" />
                                        <ext:Column ID="Title_fertilizer" runat="server" Text="Название удобрения" DataIndex="title_fertilizer"  Width="250" />
                                        <ext:Column ID="Organization_applicant" runat="server" Text="Организация заявитель" DataIndex="organization_applicant_name"  Width="250" />
                                        <ext:Column ID="Batch_size" runat="server" Text="Размер партии" DataIndex="batch_size"  Width="100" />
                                        <ext:Column ID="Sample_weight" runat="server" Text="Масса пробы" DataIndex="sample_weight"  Width="100" />
                                        <ext:Column ID="Sample_sediment_weight" runat="server" Text="Осадок" DataIndex="sample_sediment_weight"  Width="60" />
                                        <ext:Column ID="Accompanying_document" runat="server" Text="Сопроводительный документ" DataIndex="accompanying_document"  Width="170" />
                                        <ext:Column ID="Sample_selecting_date" runat="server" Text="Дата отбора пробы" DataIndex="sample_selecting_date"  Width="120" />
                                        <ext:Column ID="Sample_reception_date" runat="server" Text="Дата получения пробы" DataIndex="sample_reception_date"  Width="130" />
                                        <ext:Column ID="Testing_time_begin" runat="server" Text="Начало проведения испытаний" DataIndex="testing_time_begin"  Width="120" Hidden="true" />
                                        <ext:Column ID="Testing_time_end" runat="server" Text="Конец проведения испытаний" DataIndex="testing_time_end"  Width="120" Hidden="true" />
                                        <ext:Column ID="Normative_document_selection" runat="server" Text="НД отбора пробы" DataIndex="normative_document_selection"  Width="120" Hidden="true" />
                                        <ext:Column ID="Normative_document_testing" runat="server" Text="НД испытания пробы" DataIndex="normative_document_testing"  Width="120" Hidden="true" />
                                        <ext:Column ID="Date_input" runat="server" Text="Дата ввода" DataIndex="date_input"  Width="120" />
                                        <ext:Column ID="Date_last_edit" runat="server" Text="Дата изменения" DataIndex="date_last_edit"  Width="120" />
                                        <ext:Column ID="Id_user" runat="server" Text="Год" DataIndex="Id пользователя"  Width="50" Hidden="true" />
                                    </Columns>
                                </ColumnModel>
                            </ext:GridPanel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Panel>     
        </Items>
        </ext:Viewport>

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
                <ext:Button ID="AcceptLoginB" runat="server" Text="Вход" Icon="Accept" />
            </Buttons>
        </ext:Window>

        <ext:Window ID="FilterReportProtocolsW" runat="server" Title="Форма отчета по протоколам" Width="470" Height="265"  Modal="true" Hidden="true" Layout="VBoxLayout" ButtonAlign="Center" Resizable="false">                 
            <LayoutConfig>
                <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />
            </LayoutConfig>
            <Items>
                <ext:Panel ID="ReportProtocolsP1" runat="server" Frame="true" Layout="FormLayout">
                    <Items>
                        <ext:TextField ID="TypeReportTF" runat="server" AllowBlank="false" Hidden="true" />
                        <ext:ComboBox ID="ReportProtocolsFarmOrgCB" runat="server" FieldLabel="Организация" LabelCls="darkslateblue-note" LabelWidth="90" DisplayField="title_farm_organization" ValueField="id_farm_organization" Editable="false">
                             <Store>
                                 <ext:Store ID="ReportProtocolsFarmOrgS" runat="server">
                                      <Model>
                                         <ext:Model ID="ReportProtocolsFarmOrgM" runat="server">
                                             <Fields>
                                                 <ext:ModelField Name="id_farm_organization" />
                                                 <ext:ModelField Name="title_farm_organization" />
                                             </Fields>
                                         </ext:Model>
                                     </Model>
                                 </ext:Store>
                             </Store>
                        </ext:ComboBox>
                        <ext:ComboBox ID="ReportProtocolsRegionCB" runat="server" FieldLabel="Район" LabelCls="darkslateblue-note" LabelWidth="90" DisplayField="title_region" ValueField="id_region" Editable="false">
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
                        <ext:ComboBox ID="ReportProtocolsFarmCB" runat="server" FieldLabel="Площадка" LabelCls="darkslateblue-note" LabelWidth="90" DisplayField="title_farm" ValueField="id_farm" Editable="false">
                             <Store>
                                 <ext:Store ID="ReportProtocolsFarmS" runat="server">
                                     <Model>
                                         <ext:Model ID="ReportProtocolsFarmM" runat="server">
                                             <Fields>
                                                 <ext:ModelField Name="id_farm" />
                                                 <ext:ModelField Name="title_farm" />
                                             </Fields>
                                         </ext:Model>
                                     </Model>
                                 </ext:Store>
                             </Store>
                        </ext:ComboBox>
                        <ext:ComboBox ID="ReportProtocolsFertilizerCB" runat="server" FieldLabel="Орг. удобрение" LabelCls="darkslateblue-note" LabelWidth="90" DisplayField="title_fertilizer" ValueField="id_fertilizer" Editable="false">
                             <Store>
                                 <ext:Store ID="ReportProtocolsFertilizerS" runat="server">
                                     <Model>
                                         <ext:Model ID="ReportProtocolsFertilizerM" runat="server">
                                             <Fields>
                                                 <ext:ModelField Name="id_fertilizer" />
                                                 <ext:ModelField Name="title_fertilizer" />
                                             </Fields>
                                         </ext:Model>
                                     </Model>
                                 </ext:Store>
                             </Store>
                        </ext:ComboBox>                                   
                    </Items>                                          
                </ext:Panel>                    
                <ext:Panel ID="ReportProtocolsP2" runat="server" Frame="true" Layout="VBoxLayout">  
                    <LayoutConfig>
                        <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="2" />                    
                    </LayoutConfig>                     
                    <Items>                             
                        <ext:Label ID="ReportPlanL1" runat="server" Text="Даты" />               
                        <ext:Component ID="Component93" runat="server" Width="5" /> 
                        <ext:FieldContainer ID="FieldContainer12" runat="server" Layout="HBoxLayout">
                            <Items>                                      
                                <ext:DateField ID="ReportProtocolsDF1" runat="server" FieldLabel="с" LabelWidth="10" TabIndex="3" Width="190" Editable="false" />                
                                <ext:Component ID="Component94" runat="server" Width="10" />
                                <ext:DateField ID="ReportProtocolsDF2" runat="server" FieldLabel="по" LabelWidth="20" TabIndex="3" Width="200" Editable="false" />  
                            </Items>
                        </ext:FieldContainer>
                    </Items>                                          
                </ext:Panel>                                          
            </Items>
            <Buttons>
                <ext:Button ID="ResetReportProtocolsB" runat="server" Text="Сбросить" Icon="Reload" />
                <ext:Button ID="AcceptReportProtocolsB" runat="server" Text="Отчёт" Icon="Accept" />
                <ext:Button ID="CancelReportProtocolsB" runat="server" Text="Отменить" Icon="Cancel" />
            </Buttons>
        </ext:Window>
        <ext:Window ID="ProtocolEditW" runat="server" Title="Протоколы" Hidden="true" Layout="VBoxLayout" Width="1120" Height="700" Padding="3" ButtonAlign="Center" Maximizable="true">
            <LayoutConfig>
                <ext:VBoxLayoutConfig Align="Stretch" />
            </LayoutConfig>
            <Items>
                <ext:FieldContainer runat="server" ID="ProtocolEditFC1" Layout="HBoxLayout" >
                    <LayoutConfig>
                        <ext:HBoxLayoutConfig Align="Stretch" DefaultMargins="8" />
                    </LayoutConfig>
                    <Items>
                        <ext:FieldContainer runat="server" ID="ProtocolEditFC2" Flex="10" Layout="VBoxLayout" >
                            <LayoutConfig>
                                <ext:VBoxLayoutConfig Align="Stretch" />
                            </LayoutConfig>
                            <Items>
                                <ext:TextField ID="ProtocolEditTF" runat="server" FieldLabel="№ протокола" LabelWidth="180" AllowBlank="false" />
                                <ext:ComboBox ID="ProtocolEditFertilizerCB" runat="server" FieldLabel="Наименование объекта испытания" LabelWidth="180" DisplayField="title_fertilizer" 
                                              ValueField="id_fertilizer" Editable="false" AllowBlank="false" >
                                    <Store>
                                        <ext:Store ID="ProtocolEditFertilizerS" runat="server">
                                            <Model>
                                                <ext:Model ID="ProtocolEditFertilizerM" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="id_fertilizer" />
                                                        <ext:ModelField Name="title_fertilizer" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>
                                <ext:TextField ID="ProtocolEditOrganizationTF" FieldLabel="Организация и адрес заявителя" LabelWidth="180" runat="server" AllowBlank="false" />
                                <ext:FieldContainer runat="server" ID="ProtocolEditFC4" Layout="HBoxLayout" >
                                    <LayoutConfig>
                                        <ext:HBoxLayoutConfig Align="Stretch" />
                                    </LayoutConfig>
                                    <Items>
                                        <ext:NumberField ID="ProtocolEditSizeNF" FieldLabel="Размер партии" DecimalPrecision="3" LabelWidth="180" Flex="10" runat="server" AllowBlank="false" />
                                        <ext:Component ID="Component3" Flex="1" runat="server" />
                                        <ext:TextField ID="ProtocolEditSizeUnitTF" FieldLabel="Ед. измерения" LabelWidth="90" Flex="6" runat="server" AllowBlank="false" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" ID="ProtocolEditFC5" Layout="HBoxLayout" >
                                    <LayoutConfig>
                                        <ext:HBoxLayoutConfig Align="Stretch" />
                                    </LayoutConfig>
                                    <Items>
                                        <ext:NumberField ID="ProtocolEditValueNF" FieldLabel="Масса образца" DecimalPrecision="3" LabelWidth="180" Flex="10" runat="server" AllowBlank="false" />
                                        <ext:Component ID="Component1" Flex="1" runat="server" />
                                        <ext:TextField ID="ProtocolEditValueUnitTF" FieldLabel="Ед. измерения" LabelWidth="90" Flex="6" runat="server" AllowBlank="false" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:FieldContainer runat="server" ID="ProtocolEditFC6" Layout="HBoxLayout" >
                                    <LayoutConfig>
                                        <ext:HBoxLayoutConfig Align="Stretch" />
                                    </LayoutConfig>
                                    <Items>
                                        <ext:NumberField ID="ProtocolEditDeviationNF" FieldLabel="Масса осадка" DecimalPrecision="3" LabelWidth="180" Flex="10" runat="server" AllowBlank="true" />
                                        <ext:Component ID="Component5" Flex="1" runat="server" />
                                        <ext:TextField ID="ProtocolEditDeviationUnitTF" FieldLabel="Ед. измерения" LabelWidth="90" Flex="6" runat="server" AllowBlank="true" />
                                    </Items>
                                </ext:FieldContainer>
                                <ext:TextField ID="ProtocolEditOrganizationProbeTF" FieldLabel="Организация отбора пробы" LabelWidth="180" runat="server" AllowBlank="false" />
                                <ext:TextField ID="ProtocolEditDocumentTF" FieldLabel="Сопроводительный документ" LabelWidth="180" runat="server" AllowBlank="false" />
                            </Items>
                        </ext:FieldContainer>
                        <ext:Component ID="Component2" runat="server" Flex="1" />
                        <ext:FieldContainer runat="server" ID="ProtocolEditFC3" Flex="10" Layout="VBoxLayout" >
                            <LayoutConfig>
                                <ext:VBoxLayoutConfig Align="Stretch" />
                            </LayoutConfig>
                            <Items>
                                <ext:DateField ID="ProtocolEditSelectingDateDF" runat="server" FieldLabel="Дата отбора пробы" LabelWidth="180" AllowBlank="false" Editable="false" />
                                <ext:DateField ID="ProtocolEditReceptionDateDF" runat="server" FieldLabel="Дата получения пробы" LabelWidth="180" AllowBlank="false" Editable="false" />
                                <ext:DateField ID="ProtocolEditTestingTimeBeginDF" FieldLabel="Время начала проведения испытания" LabelWidth="180" runat="server" AllowBlank="false" Editable="false" />
                                <ext:DateField ID="ProtocolEditTestingTimeEndDF" FieldLabel="Время окончания проведения испытания" LabelWidth="180" runat="server" AllowBlank="false" Editable="false" />
                                <ext:TextField ID="ProtocolEditSelectingDocumentTF" FieldLabel="Нормативный документ отбора" LabelWidth="180" runat="server" AllowBlank="false" />
                                <ext:TextField ID="ProtocolEditTestingDocumentTF" FieldLabel="Нормативный документ пробы" LabelWidth="180" runat="server" AllowBlank="false" />
                                <ext:TextField ID="ProtocolEditHardenerPositionTF" FieldLabel="Должность подписчика" LabelWidth="180" runat="server" AllowBlank="false" />
                                <ext:TextField ID="ProtocolEditHardenerTF" FieldLabel="Ф.И.О. подписчика" LabelWidth="180" runat="server" AllowBlank="false" />
                            </Items>
                        </ext:FieldContainer>
                    </Items>
                </ext:FieldContainer>
                <ext:TextField ID="AdditionalTF" runat="server" FieldLabel="Дополнительно" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                <ext:Label ID="ProtocolEditCommentL" runat="server" Text="Комментарий" Padding="8" />
                <ext:TextArea ID="ProtocolEditCommentTA" runat="server" AllowBlank="true" Padding="8" />
                <ext:GridPanel ID="ProtocolEditResultsGP" Title="Результаты" runat="server" AutoScroll="true" Flex="1" MultiSelect="false">
                    <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                    <TopBar>
                        <ext:Toolbar ID="ProtocolEditResultsT" runat="server">
                            <Items>
                                <ext:Button ID="ProtocolEditAddResultB" runat="server" Icon="Add" Text="Добавить" />
                                <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                                <ext:Button ID="ProtocolEditDeleteResultB" runat="server" Icon="Delete" Text="Удалить"/>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                            <ext:Store ID="ProtocolEditGridS" runat="server">
                                <Model>
                                    <ext:Model ID="ProtocolEditGridM" runat="server">
                                        <Fields>
                                            <ext:ModelField Name="id_result" />
                                            <ext:ModelField Name="id_protocol" />
                                            <ext:ModelField Name="id_significative_fertilizer" />
                                            <ext:ModelField Name="title_significative_fertilizer" />
                                            <ext:ModelField Name="id_unit_s_f" />
                                            <ext:ModelField Name="unit_s_f" />
                                            <ext:ModelField Name="id_normative_document" />
                                            <ext:ModelField Name="title_normative_document" />
                                            <ext:ModelField Name="value" />
                                            <ext:ModelField Name="deviation" />
                                            <ext:ModelField Name="number_of_digits" />
                                            <ext:ModelField Name="minimum" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>            
                                <ext:Column ID="Id_resultC" runat="server" DataIndex="id_result" Hidden="true" Text="Id результата" />
                                <ext:Column ID="Id_significative_fertilizerC" runat="server" DataIndex="id_significative_fertilizer" Hidden="true" Text="Id показателя" />
                                <ext:Column ID="Title_significative_fertilizerC" runat="server" DataIndex="title_significative_fertilizer" Text="Наименование показателя" Width="300">
                                    <Editor>
                                        <ext:ComboBox ID="Title_significative_fertilizerCB" runat="server" LabelCls="darkslateblue-note" DisplayField="title_significative_fertilizer" 
                                          ValueField="title_significative_fertilizer" Editable="false" Padding="3">
                                            <Store>
                                                <ext:Store ID="Title_significative_fertilizerS" runat="server">
                                                    <Model>
                                                        <ext:Model ID="Title_significative_fertilizerM" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="id_significative_fertilizer" />
                                                                <ext:ModelField Name="title_significative_fertilizer" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Editor>
                                </ext:Column>
                                <ext:Column ID="Id_Units_s_fC" runat="server" DataIndex="id_unit_s_f" Hidden="true" Text="Id единицы измерения" />
                                <ext:Column ID="Unit_s_fC" runat="server" Text="Единицы измерения" DataIndex="unit_s_f" Width="150">
                                    <Editor>
                                        <ext:ComboBox ID="UnitsSignificativeFertilizerCB" runat="server" LabelCls="darkslateblue-note" DisplayField="unit_s_f" 
                                          ValueField="unit_s_f" Editable="false" Padding="3">
                                            <Store>
                                                <ext:Store ID="UnitsSignificativeFertilizerS" runat="server">
                                                    <Model>
                                                        <ext:Model ID="UnitsSignificativeFertilizerM" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="id_unit_s_f" />
                                                                <ext:ModelField Name="unit_s_f" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Editor>
                                </ext:Column>
                                <ext:Column ID="Id_DocumentC" runat="server" DataIndex="id_normative_document" Hidden="true" Text="Id документа" />
                                <ext:Column ID="DocumentC" runat="server" Text="Нормативный документ" DataIndex="title_normative_document" Width="250">
                                    <Editor>
                                        <ext:ComboBox ID="DocumentCB" runat="server" LabelCls="darkslateblue-note" DisplayField="title_normative_document" 
                                          ValueField="title_normative_document" Editable="false" Padding="3">
                                            <Store>
                                                <ext:Store ID="DocumentS" runat="server">
                                                    <Model>
                                                        <ext:Model ID="DocumentM" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="title_normative_document" />
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                    </Editor>
                                </ext:Column>
                                <ext:Column ID="ResultValueC" runat="server" Text="Фактическое<br />значение" DataIndex="value" Width="150">
                                    <Editor>
                                        <ext:NumberField ID="ResultValueNF" runat="server" AllowBlank="false" DecimalPrecision="5" />
                                    </Editor>
                                </ext:Column>
                                <ext:Column ID="ResultDeviationC" runat="server" Text="Погрешность" DataIndex="deviation" Width="100">
                                    <Editor>
                                        <ext:TextField ID="ResultDeviationTF" runat="server" AllowBlank="false" />
                                    </Editor>
                                </ext:Column>
                                <ext:Column ID="Column1" runat="server" Text="Кол-во<br />знаков" DataIndex="number_of_digits" Width="60">
                                    <Editor>
                                        <ext:NumberField ID="ResultNODNF" runat="server" AllowBlank="true" />
                                    </Editor>
                                </ext:Column>
                                <ext:Column ID="Column2" runat="server" Text="Минимум" DataIndex="minimum" Width="60">
                                    <Editor>
                                        <ext:TextField ID="ResultMinimumTF" runat="server" AllowBlank="true" />
                                    </Editor>
                                </ext:Column>
                            </Columns>            
                        </ColumnModel>
                        <Plugins>
                            <ext:RowEditing ID="ProtocolEditGPPlugin" runat="server" ClicksToMoveEditor ="1" AutoCancel="true" CancelBtnText="Отмена" SaveBtnText="Сохранить" />
                        </Plugins>
                </ext:GridPanel>
            </Items>
            <Buttons>
                <ext:Button ID="ProtocolEditAddB" runat="server" Icon="Add" Text="Добавить" Hidden="true" />
                <ext:Button ID="ProtocolEditSaveB" runat="server" Icon="Accept" Text="Сохранить" Hidden="true" />
                <ext:Button ID="ProtocolEditCancelB" runat="server" Icon="Cancel" Text="Отмена" />
            </Buttons>
        </ext:Window>
    </form>
</body>
</html>
