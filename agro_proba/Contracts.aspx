<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contracts.aspx.cs" Inherits="agro_proba.Contracts" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Агрохимик Online - Договоры</title>
    <link rel="icon" href="icon.jpeg" type="image/jpeg" />
    <script type="text/javascript" src="functions.js"></script>
    <script type="text/javascript" src="js/Contracts.js"></script>
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
        .red-row .x-grid-cell, .red-row .x-grid-rowwrap-div {
            background-color: red !important;
        }
        .green-row .x-grid-cell, .green-row .x-grid-rowwrap-div {
            background-color: green !important;
        }
        .yellow-row .x-grid-cell, .yellow-row .x-grid-rowwrap-div {
            background-color: yellow !important;
        }
        .white-row .x-grid-cell, .white-row .x-grid-rowwrap-div {
            background-color: white !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="MainSM" runat="server" AsyncPostBackTimeout="0"></asp:ScriptManager>
        <ext:ResourceManager ID="MainRM" runat="server" DisableViewState="true" AjaxTimeout="300000" />
        <ext:Viewport ID="IndexViewport" runat="server" Layout="BorderLayout" Hidden="false">
            <Items>
                <ext:Panel runat="server" Region="North" Height="26" ID="ToolP">
                   <TopBar>
                    <ext:Toolbar ID="IndexToolbar" runat="server">
                        <Items>
                            <ext:Button ID="ReportB" runat="server" Text="Отчёт" Icon="Report" />
                            <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                            <ext:Label ID="UserNameL" runat="server" Text="..." Hidden="true" />
                            <ext:Button ID="ExitB" runat="server" Text="Выход" Icon="DoorOut" Hidden="true" />
                        </Items>
                    </ext:Toolbar>
                   </TopBar>
                </ext:Panel>
                <ext:Panel ID="BottomP" runat="server" Height="36" Region="South" Layout="HBoxLayout">
                    <LayoutConfig><ext:HBoxLayoutConfig Align="Stretch"></ext:HBoxLayoutConfig></LayoutConfig>
                    <Items>
                        <ext:TextField ID="CountContractsTF" runat="server" FieldLabel="Кол-во договоров" LabelWidth="110" Width="300" ReadOnly="true" Margin="5" />
                        <ext:TextField ID="SumAreaTF" runat="server" FieldLabel="Площадь" LabelWidth="70" Width="250" ReadOnly="true" Margin="5" />
                        <ext:TextField ID="SumClientPriceTF" runat="server" FieldLabel="Сумма средств заказчиков" LabelWidth="180" Width="370" ReadOnly="true" Margin="5" />
                        <ext:TextField ID="SumBalancePriceTF" runat="server" FieldLabel="Остаток" LabelWidth="60" Width="240" ReadOnly="true" Margin="5" />
                    </Items>
                </ext:Panel>
                <ext:Panel ID="StatusP" runat="server" Height="26" Region="South">
                    <Items>
                        <ext:StatusBar ID="IndexSB" runat="server" Height="25" DefaultText="Готово" >
                            <Items>
                                <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" AutoDataBind="true" Text="Дата:" />
                                <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" AutoDataBind="true" Text="<%# DateTime.Now.Date %>" />
                            </Items>
                        </ext:StatusBar>
                    </Items>
                </ext:Panel>
                <ext:Panel ID="CentralP" runat="server" Region="Center" MinWidth="600" Split="true" Layout="FitLayout" AutoScroll="true">
                    <Items>
                        <ext:GridPanel ID="ContractsGP" runat="server" Layout="FitLayout" AutoScroll="true" MultiSelect="false" EnableColumnHide="false">
                            <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                            <TopBar>
                                <ext:Toolbar ID="ContractsTB" runat="server">
                                    <Items>
                                        <ext:Button ID="AddContractB" runat="server" Icon="Add" Text="Добавить" />
                                        <ext:Button ID="EditContractB" runat="server" Icon="Pencil" Text="Редактировать" />
                                        <ext:Button ID="ClearContractsFiltersB" runat="server" Icon="Reload" Text="Удалить фильтры" />
                                        <ext:ToolbarFill ID="FarmTFill" runat="server" />
                                        <ext:Button ID="DeleteContractB" runat="server" Icon="Delete" Text="Удалить" />
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Store>
                                <ext:Store ID="ContractsS" runat="server">
                                    <Model>
                                        <ext:Model ID="ContractsM" runat="server">
                                            <Fields>
                                                <ext:ModelField Name="id_contract" Type="Int" />
                                                <ext:ModelField Name="id_region" Type="Int" />
                                                <ext:ModelField Name="id_territory" Type="Int" />
                                                <ext:ModelField Name="code_region" Type="Int" />
                                                <ext:ModelField Name="title_region" Type="String" />
                                                <ext:ModelField Name="okato_region" Type="String" />
                                                <ext:ModelField Name="id_organization" Type="Int" />
                                                <ext:ModelField Name="code_organization" Type="Int" />
                                                <ext:ModelField Name="title_organization" Type="String" />
                                                <ext:ModelField Name="full_title_organization" Type="String" />
                                                <ext:ModelField Name="client_position" Type="String" />
                                                <ext:ModelField Name="client_leader" Type="String" />
                                                <ext:ModelField Name="basis_document" Type="String" />
                                                <ext:ModelField Name="legal_address" Type="String" />
                                                <ext:ModelField Name="mailing_address" Type="String" />
                                                <ext:ModelField Name="email_address" Type="String" />
                                                <ext:ModelField Name="inn_organization" Type="String" />
                                                <ext:ModelField Name="kpp_organization" Type="String" />
                                                <ext:ModelField Name="ogrn_organization" Type="String" />
                                                <ext:ModelField Name="okved_organization" Type="String" />
                                                <ext:ModelField Name="okpo_organization" Type="String" />
                                                <ext:ModelField Name="okato_organization" Type="String" />
                                                <ext:ModelField Name="pay_account" Type="String" />
                                                <ext:ModelField Name="full_bank_name" Type="String" />
                                                <ext:ModelField Name="bik" Type="String" />
                                                <ext:ModelField Name="bank_correspond_account" Type="String" />
                                                <ext:ModelField Name="id_contract_subject" Type="Int" />
                                                <ext:ModelField Name="code_contract_subject" Type="String" />
                                                <ext:ModelField Name="title_contract_subject" Type="String" />
                                                <ext:ModelField Name="index_number" Type="Int" />
                                                <ext:ModelField Name="number_contract" Type="String" />
                                                <ext:ModelField Name="id_signer" Type="Int" />
                                                <ext:ModelField Name="signer_full_name" Type="String" />
                                                <ext:ModelField Name="id_letter_attorney" Type="Int" />
                                                <ext:ModelField Name="letter_attorney" Type="String" />
                                                <ext:ModelField Name="contract_start_date" Type="Date" />
                                                <ext:ModelField Name="contract_end_date" Type="Date" />
                                                <ext:ModelField Name="area" Type="Float" />
                                                <ext:ModelField Name="payment_days" Type="Float" />
                                                <ext:ModelField Name="total_price" Type="Float" />
                                                <ext:ModelField Name="nds_price" Type="Float" />
                                                <ext:ModelField Name="client_price" Type="Float" />
                                                <ext:ModelField Name="federal_price" Type="Float" />
                                                <ext:ModelField Name="prepayment" Type="Float" />
                                                <ext:ModelField Name="balance" Type="Float" />
                                                <ext:ModelField Name="payment" Type="Float" />
                                                <ext:ModelField Name="date_finish" Type="Date" />
                                                <ext:ModelField Name="date_selecting" Type="Date" />
                                                <ext:ModelField Name="date_fulfilment" Type="Date" />
                                                <ext:ModelField Name="date_input" Type="Date" />
                                                <ext:ModelField Name="date_edit" Type="Date" />
                                                <ext:ModelField Name="id_user" Type="Int" />
                                                <ext:ModelField Name="surname" Type="String" />
                                                <ext:ModelField Name="name" Type="String" />
                                                <ext:ModelField Name="patronymic" Type="String" />
                                                <ext:ModelField Name="phone" Type="String" />
                                                <ext:ModelField Name="id_contract_status" Type="Int" />
                                                <ext:ModelField Name="title_contract_status" Type="String" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <ColumnModel>
                                <Columns>
                                    <ext:Column ID="Index_numberC" runat="server" DataIndex="index_number" Text="Порядковый №" Width="100" Hidden="true" />

                                    <ext:Column ID="Number_contractC" runat="server" DataIndex="number_contract" Text="№ договора" Width="100" />

                                    <ext:Column ID="Id_сontractC" runat="server" DataIndex="id_contract" Text="Id док." Width="50" Hidden="true" />
                                    <ext:Column ID="Id_organizationC" runat="server" DataIndex="id_organization" Text="Id орг." Width="50" Hidden="true" />
                                    <ext:Column ID="Code_organizationC" runat="server" DataIndex="code_organization" Text="Код орг." Width="50" Hidden="true" />

                                    <ext:Column ID="Title_organizationC" runat="server" DataIndex="title_organization" Text="Наименование орг." Width="150" />
                                    <ext:Column ID="Inn_organizationC" runat="server" DataIndex="inn_organization" Text="ИНН" Width="80" />
                                    <ext:Column ID="PhoneC" runat="server" DataIndex="phone" Text="Телефон" Width="100" />

                                    <ext:Column ID="Full_title_organizationC" runat="server" DataIndex="full_title_organization" Text="Полное наименование" Width="150" Hidden="true" />
                                    <ext:Column ID="Legal_addressC" runat="server" DataIndex="legal_address" Text="Юридический адрес" Width="200" Hidden="true" />
                                    <ext:Column ID="Mailing_addressC" runat="server" DataIndex="mailing_address" Text="Почтовый адрес" Width="200" Hidden="true" />
                                    <ext:Column ID="Email_addressC" runat="server" DataIndex="email_address" Text="E-mail" Width="100" />
                                    <ext:Column ID="Kpp_organizationC" runat="server" DataIndex="kpp_organization" Text="КПП" Width="50" Hidden="true" />
                                    <ext:Column ID="Ogrn_organizationC" runat="server" DataIndex="ogrn_organization" Text="ОГРН" Width="50" Hidden="true" />
                                    <ext:Column ID="Okved_organizationC" runat="server" DataIndex="okved_organization" Text="ОКВЕД" Width="50" Hidden="true" />
                                    <ext:Column ID="Okpo_organizationC" runat="server" DataIndex="okpo_organization" Text="ОКПО" Width="50" Hidden="true" />
                                    <ext:Column ID="Okato_organizationC" runat="server" DataIndex="okato_organization" Text="ОКАТО" Width="50" Hidden="true" />
                                    <ext:Column ID="Pay_accountC" runat="server" DataIndex="pay_account" Text="Расчётный счёт" Width="100" Hidden="true" />
                                    <ext:Column ID="Full_bank_nameC" runat="server" DataIndex="full_bank_name" Text="Наименование банка" Width="100" Hidden="true" />
                                    <ext:Column ID="BikC" runat="server" DataIndex="bik" Text="БИК банка" Width="50" Hidden="true" />
                                    <ext:Column ID="Bank_correspond_accountC" runat="server" DataIndex="bank_correspond_account" Text="Корресп. счёт" Width="100" Hidden="true" />
                                    <ext:Column ID="Id_regionC" runat="server" DataIndex="id_region" Text="Id района" Width="50" Hidden="true" />
                                    <ext:Column ID="Id_territoryC" runat="server" DataIndex="id_territory" Text="Id обл." Width="50" Hidden="true" />
                                    <ext:Column ID="Code_regionC" runat="server" DataIndex="code_region" Text="Код района" Width="50" Hidden="true" />

                                    <ext:Column ID="Title_regionC" runat="server" DataIndex="title_region" Text="Район" Width="100" />

                                    <ext:Column ID="Okato_regionC" runat="server" DataIndex="okato_region" Text="ОКАТО района" Width="50" Hidden="true" />
                                    <ext:Column ID="Id_contract_subjectC" runat="server" DataIndex="id_contract_subject" Text="Id предмета договора" Width="50" Hidden="true" />

                                    <ext:Column ID="Code_contract_subjectC" runat="server" DataIndex="code_contract_subject" Text="Код ПД" Width="50" />
                                    <ext:Column ID="Title_contract_subjectC" runat="server" DataIndex="title_contract_subject" Text="Наименование ПД" Width="100" />
                                    <ext:Column ID="AreaC" runat="server" DataIndex="area" Text="Площадь" Width="100" />

                                    <ext:Column ID="Id_contract_statusC" runat="server" DataIndex="id_contract_status" Text="Id статуса" Width="50" Hidden="true" />

                                    <ext:Column ID="Title_contract_statusC" runat="server" DataIndex="title_contract_status" Text="Статус" Width="100" />
                                    <ext:DateColumn ID="Contract_start_dateDC" runat="server" DataIndex="contract_start_date" Text="Дата заключения" Width="120" Align="Center" Format="dd.MM.yyyy" />
                                    <ext:Column ID="Total_priceC" runat="server" DataIndex="total_price" Text="Общая стоимость" Width="100" Align="Right" />
                                    <ext:Column ID="Nds_priceC" runat="server" DataIndex="nds_price" Text="НДС" Width="80" Hidden="true" Align="Right" />
                                    <ext:Column ID="Federal_priceC" runat="server" DataIndex="federal_price" Text="Федеральный бюджет" Width="100" Align="Right" />
                                    <ext:Column ID="Client_priceC" runat="server" DataIndex="client_price" Text="Бюджета клиента" Width="100" Align="Right" />
                                    <ext:Column ID="PrepaymentC" runat="server" DataIndex="prepayment" Text="Аванс" Width="80" Align="Right" />
                                    <ext:Column ID="PaymentC" runat="server" DataIndex="payment" Text="Оплата" Width="80" Align="Right" />
                                    <ext:Column ID="BalanceC" runat="server" DataIndex="balance" Text="Остаток" Width="80" Align="Right" />
                                    <ext:DateColumn ID="Date_finishC" runat="server" DataIndex="date_finish" Text="Дата завершения договора" Width="100" Align="Center" Format="dd.MM.yyyy" Hidden="true" />
                                    <ext:DateColumn ID="Contract_end_dateC" runat="server" DataIndex="contract_end_date" Text="Дата окончания" Width="120" Align="Center" Format="dd.MM.yyyy" Hidden="true" />

                                    <ext:Column ID="Client_positionC" runat="server" DataIndex="client_position" Text="Должность клиента" Hidden="true" Width="100" />
                                    <ext:Column ID="ClientLeaderC" runat="server" DataIndex="client_leader" Text="Руководитель" Width="150" Hidden="true" />
                                    <ext:Column ID="Basis_documentC" runat="server" DataIndex="basis_document" Text="Доверенность" Width="100" Hidden="true" />
                                    <ext:Column ID="Id_SignerC" runat="server" DataIndex="id_signer" Text="ID подписанта" Hidden="true" Width="50" />
                                    <ext:Column ID="Signer_full_nameC" runat="server" DataIndex="signer_full_name" Text="Ф.И.О. подписанта" Hidden="true" Width="100" />
                                    <ext:Column ID="Id_Letter_attorneyC" runat="server" DataIndex="id_letter_attorney" Text="ID дов." Hidden="true" Width="50" />
                                    <ext:Column ID="Letter_attorneyC" runat="server" DataIndex="letter_attorney" Text="Доверенность" Hidden="true" Width="100" />
                                    <ext:Column ID="Payment_daysC" runat="server" DataIndex="payment_days" Text="Срок оплаты" Width="80" Hidden="true" />
                                    <ext:DateColumn ID="Date_selectingС" runat="server" DataIndex="date_selecting" Text="Срок отбора" Width="100" Align="Center" Format="dd.MM.yyyy" />
                                    <ext:DateColumn ID="Date_fulfilmentC" runat="server" DataIndex="date_fulfilment" Text="Срок исполнения" Width="100" Align="Center" Format="dd.MM.yyyy" />
                                    <ext:Column ID="Date_inputC" runat="server" DataIndex="date_input" Text="Дата ввода" Width="50" Hidden="true" Align="Center" Format="dd.MM.yyyy" />
                                    <ext:Column ID="Date_editC" runat="server" DataIndex="date_edit" Text="Дата редактирования" Width="50" Hidden="true" Align="Center" Format="dd.MM.yyyy" />
                                    <ext:Column ID="Id_userC" runat="server" DataIndex="id_user" Text="Id пользователя" Width="50" Hidden="true" />
                                    <ext:Column ID="SurnameС" runat="server" DataIndex="surname" Text="Фамилия" Width="50" Hidden="true" />
                                    <ext:Column ID="NameС" runat="server" DataIndex="name" Text="Имя" Width="50" Hidden="true" />
                                    <ext:Column ID="PatronymicС" runat="server" DataIndex="patronymic" Text="Отчество" Width="50" Hidden="true" />
                                </Columns>        
                            </ColumnModel>
                            <Features>
                                <ext:GridFilters runat="server" ID="GridFilters1" Local="true">
                                    <Filters>
                                        <ext:NumericFilter DataIndex="id_contract" />
                                        <ext:NumericFilter DataIndex="id_region" />
                                        <ext:NumericFilter DataIndex="id_territory" />
                                        <ext:NumericFilter DataIndex="code_region" />
                                        <ext:StringFilter DataIndex="title_region" />
                                        <ext:StringFilter DataIndex="okato_region" />
                                        <ext:NumericFilter DataIndex="id_organization" />
                                        <ext:NumericFilter DataIndex="code_organization" />
                                        <ext:StringFilter DataIndex="title_organization" />
                                        <ext:StringFilter DataIndex="full_title_organization" />
                                        <ext:StringFilter DataIndex="client_leader" />
                                        <ext:StringFilter DataIndex="basis_document" />
                                        <ext:StringFilter DataIndex="legal_address" />
                                        <ext:StringFilter DataIndex="mailing_address" />
                                        <ext:StringFilter DataIndex="email_address" />
                                        <ext:StringFilter DataIndex="inn_organization" />
                                        <ext:StringFilter DataIndex="kpp_organization" />
                                        <ext:StringFilter DataIndex="ogrn_organization" />
                                        <ext:StringFilter DataIndex="okved_organization" />
                                        <ext:StringFilter DataIndex="okpo_organization" />
                                        <ext:StringFilter DataIndex="okato_organization" />
                                        <ext:StringFilter DataIndex="pay_account" />
                                        <ext:StringFilter DataIndex="full_bank_name" />
                                        <ext:StringFilter DataIndex="bik" />
                                        <ext:StringFilter DataIndex="bank_correspond_account" />
                                        <ext:NumericFilter DataIndex="id_contract_subject" />
                                        <ext:ListFilter DataIndex="code_contract_subject" Options="АХ,ПЭ,У,В,Уо,Ум,М,К,П,Пч,Э,И,СЗ" />
                                        <ext:StringFilter DataIndex="title_contract_subject" />
                                        <ext:NumericFilter DataIndex="index_number" />
                                        <ext:StringFilter DataIndex="number_contract" />
                                        <ext:NumericFilter DataIndex="id_signer" />
                                        <ext:StringFilter DataIndex="signer_full_name" />
                                        <ext:NumericFilter DataIndex="id_letter_attorney" />
                                        <ext:StringFilter DataIndex="letter_attorney" />
                                        <ext:DateFilter DataIndex="contract_start_date">
                                            <DatePickerOptions runat="server" TodayText="Сегодня" Format="dd.MM.yyyy" />
                                        </ext:DateFilter>
                                        <ext:DateFilter DataIndex="contract_end_date">
                                            <DatePickerOptions runat="server" TodayText="Сегодня" Format="dd.MM.yyyy" />
                                        </ext:DateFilter>
                                        <ext:NumericFilter DataIndex="area" />
                                        <ext:NumericFilter DataIndex="cost_of_work" />
                                        <ext:NumericFilter DataIndex="total_price" />
                                        <ext:NumericFilter DataIndex="nds_price" />
                                        <ext:NumericFilter DataIndex="client_price" />
                                        <ext:NumericFilter DataIndex="federal_price" />
                                        <ext:NumericFilter DataIndex="prepayment" />
                                        <ext:NumericFilter DataIndex="balance" />
                                        <ext:NumericFilter DataIndex="payment" />
                                        <ext:DateFilter DataIndex="date_finish">
                                            <DatePickerOptions runat="server" TodayText="Сегодня" Format="dd.MM.yyyy" />
                                        </ext:DateFilter>
                                        <ext:DateFilter DataIndex="date_selecting">
                                            <DatePickerOptions runat="server" TodayText="Сегодня" Format="dd.MM.yyyy" />
                                        </ext:DateFilter>
                                        <ext:DateFilter DataIndex="date_fulfilment">
                                            <DatePickerOptions runat="server" TodayText="Сегодня" Format="dd.MM.yyyy" />
                                        </ext:DateFilter>
                                        <ext:DateFilter DataIndex="date_input">
                                            <DatePickerOptions runat="server" TodayText="Сегодня" Format="dd.MM.yyyy" />
                                        </ext:DateFilter>
                                        <ext:DateFilter DataIndex="date_edit">
                                            <DatePickerOptions runat="server" TodayText="Сегодня" Format="dd.MM.yyyy" />
                                        </ext:DateFilter>
                                        <ext:NumericFilter DataIndex="id_user" />
                                        <ext:StringFilter DataIndex="surname" />
                                        <ext:StringFilter DataIndex="name" />
                                        <ext:StringFilter DataIndex="patronymic" />
                                        <ext:StringFilter DataIndex="phone" />
                                        <ext:NumericFilter DataIndex="id_contract_status" />
                                        <ext:ListFilter DataIndex="title_contract_status" Options="Оформление,На согласовании,Заключен,В работе,Исполнен" />
                                    </Filters>
                                </ext:GridFilters>
                            </Features>
                            <ViewConfig>
                                <GetRowClass />
                            </ViewConfig>
                        </ext:GridPanel>
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
    
        <ext:Window ID="AddEditContractW" runat="server" Closable="false" Width="500" Height="560"  BodyPadding="5" Modal="true" Hidden="true" ButtonAlign="Center" Resizable="false" Layout="VBoxLayout">
            <LayoutConfig><ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig></LayoutConfig>
            <Items>
                <ext:TextField ID="IdContractTF" runat="server" FieldLabel="Id договора:" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" Hidden="true" />
                <ext:FieldContainer ID="FieldContainer12" runat="server" Layout="HBoxLayout">
                    <Items>
                        <ext:NumberField ID="IndexNumberNF" runat="server" FieldLabel="Порядковый номер:" AllowBlank="true" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="120" MaxLength="80" Flex="7" />
                        <ext:Component ID="Component13" runat="server" Flex="1" />
                        <ext:TextField ID="NumberContractTF" runat="server" FieldLabel="Номер договора:" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" MaxLength="200" Flex="7" ReadOnly="true" Hidden="true" />
                    </Items>
                </ext:FieldContainer>
                <ext:ComboBox ID="StatusContractCB" runat="server" FieldLabel="Статус:" LabelCls="darkslateblue-note" AllowBlank="false" LabelWidth="70" Editable="false" DisplayField="title_contract_status" ValueField="id_contract_status" >
    		    	<Store>
    		    		<ext:Store ID="StatusContractS" runat="server">
    		    			<Model>
                                <ext:Model ID="StatusContractM" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="id_contract_status" />
                                        <ext:ModelField Name="title_contract_status" />
                                    </Fields>
                                </ext:Model>
                            </Model>
    		    		</ext:Store>
    		    	</Store>
    		    </ext:ComboBox>        
    		    <ext:ComboBox ID="RegionContractCB" runat="server" FieldLabel="Район:" LabelCls="darkslateblue-note" AllowBlank="true" LabelWidth="70" Editable="false" DisplayField="title_region" ValueField="id_region" EnableKeyEvents="true" >
    		    	<Store>
    		    		<ext:Store ID="RegionContractS" runat="server">
    		    			<Model>
                                <ext:Model ID="RegionContractM" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="id_region" />
                                        <ext:ModelField Name="title_region" />
                                    </Fields>
                                </ext:Model>
                            </Model>
    		    		</ext:Store>
    		    	</Store>
    		    </ext:ComboBox>
                <ext:ComboBox ID="ContractSubjectCB" runat="server" FieldLabel="Предмет договора:" LabelCls="darkslateblue-note" AllowBlank="false" LabelWidth="70" Editable="false" DisplayField="title_contract_subject" ValueField="id_contract_subject" >
    		    	<Store>
    		    		<ext:Store ID="ContractSubjectS" runat="server">
    		    			<Model>
                                <ext:Model ID="ContractSubjectM" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="id_contract_subject" />
                                        <ext:ModelField Name="title_contract_subject" />
                                    </Fields>
                                </ext:Model>
                            </Model>
    		    		</ext:Store>
    		    	</Store>
    		    </ext:ComboBox>
                <ext:ComboBox ID="SignerCB" runat="server" FieldLabel="Подписант:" LabelCls="darkslateblue-note" AllowBlank="false" LabelWidth="70" Editable="false" DisplayField="signer_full_name" ValueField="id_signer" >
    		    	<Store>
    		    		<ext:Store ID="SignerS" runat="server">
    		    			<Model>
                                <ext:Model ID="SignerM" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="id_signer" />
                                        <ext:ModelField Name="signer_full_name" />
                                    </Fields>
                                </ext:Model>
                            </Model>
    		    		</ext:Store>
    		    	</Store>
    		    </ext:ComboBox>
                <ext:ComboBox ID="LetterAttorneyCB" runat="server" FieldLabel="Доверенность:" LabelCls="darkslateblue-note" AllowBlank="false" LabelWidth="90" Editable="false" DisplayField="letter_attorney" ValueField="id_letter_attorney" >
    		    	<Store>
    		    		<ext:Store ID="LetterAttorneyS" runat="server">
    		    			<Model>
                                <ext:Model ID="LetterAttorneyM" runat="server">
                                    <Fields>
                                        <ext:ModelField Name="id_letter_attorney" />
                                        <ext:ModelField Name="letter_attorney" />
                                    </Fields>
                                </ext:Model>
                            </Model>
    		    		</ext:Store>
    		    	</Store>
    		    </ext:ComboBox>
                <ext:TextField ID="IdOrganizationContractTF" runat="server" FieldLabel="Id организации:" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" Hidden="true" /> 
                <ext:FieldContainer ID="FieldContainer4" runat="server" Layout="HBoxLayout">
                    <Items>
                        <ext:TextField ID="TitleOrganizationContractTF" runat="server" FieldLabel="Организация:" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="450" ReadOnly="true" /> 
                        <ext:Button ID="SelectOrganizationB" runat="server" Icon="Book"/>
                    </Items>
                </ext:FieldContainer>
                <ext:TextField ID="ClientPositionTF" runat="server" FieldLabel="Должность заказчика" AllowBlank="true" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="450" EnableKeyEvents="true" />
                <ext:TextField ID="ClientLeaderTF" runat="server" FieldLabel="ФИО заказчика" AllowBlank="true" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="450" />
                <ext:TextField ID="BasicDocumentTF" runat="server" FieldLabel="Доверенность" AllowBlank="true" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="450" EnableKeyEvents="true" />
                
                <ext:FieldContainer ID="FieldContainer10" runat="server" Layout="HBoxLayout">
                    <Items>
                        <ext:DateField ID="ContractStartDateDF" runat="server" FieldLabel="Дата заключения:" LabelWidth="110" LabelCls="darkslateblue-note" Format="d.MM.yyyy" TabIndex="-1" Editable="false" Flex="7" />
                        <ext:Component ID="Component6" runat="server" Flex="1" />
                        <ext:DateField ID="ContractEndDateDF" runat="server" FieldLabel="Дата окончания:" LabelWidth="110" LabelCls="darkslateblue-note" Format="d.MM.yyyy" TabIndex="-1" Editable="false" Flex="7" EnableKeyEvents="true" />
                    </Items>
                </ext:FieldContainer>

                <ext:FieldContainer ID="FieldContainer11" runat="server" Layout="HBoxLayout">
                    <Items>
                        <ext:DateField ID="DateFinishDF" runat="server" FieldLabel="Дата исполнения:" LabelWidth="110" LabelCls="darkslateblue-note" TabIndex="-1" Editable="false" Flex="7" EnableKeyEvents="true" />
                        <ext:Component ID="Component12" runat="server" Flex="1" />
                        <ext:DateField ID="DateFulfilmentDF" runat="server" FieldLabel="Срок исполнения:" LabelWidth="110" LabelCls="darkslateblue-note" TabIndex="-1" Editable="false" Flex="7" EnableKeyEvents="true" />
                    </Items>
                </ext:FieldContainer>

                <ext:FieldContainer ID="FieldContainer1" runat="server" Layout="HBoxLayout">
                    <Items>
                        <ext:NumberField ID="AreaNF" runat="server" FieldLabel="Площадь" AllowBlank="true" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" MaxLength="80" Flex="7" EnableKeyEvents="true" />
                        <ext:Component ID="Component1" runat="server" Flex="1" />
                        <ext:DateField ID="SelectingDateDF" runat="server" FieldLabel="Срок отбора:" LabelWidth="110" LabelCls="darkslateblue-note" TabIndex="-1" Editable="false" Flex="7" EnableKeyEvents="true" />
                    </Items>
                </ext:FieldContainer>

                <ext:FieldContainer ID="FieldContainer5" runat="server" Layout="HBoxLayout">
                    <Items>
                        <ext:NumberField ID="TotalPriceNF" runat="server" FieldLabel="Общая сумма:" AllowBlank="true" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" MaxLength="80" Flex="7" EnableKeyEvents="true" />
                        <ext:Component ID="Component7" runat="server" Flex="1" />
                        <ext:NumberField ID="FederalPriceNF" runat="server" FieldLabel="Федер. бюджет:" AllowBlank="true" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" MaxLength="80" Flex="7" EnableKeyEvents="true" />
                    </Items>
                </ext:FieldContainer>

                <ext:FieldContainer ID="FieldContainer2" runat="server" Layout="HBoxLayout">
                    <Items>
                        <ext:NumberField ID="ClientPriceNF" runat="server" FieldLabel="Средства заказчика:" AllowBlank="true" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" MaxLength="80" Flex="7" EnableKeyEvents="true" />
                        <ext:Component ID="Component2" runat="server" Flex="1" />
                        <ext:NumberField ID="PrepaymentNF" runat="server" FieldLabel="Аванс:" AllowBlank="true" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" MaxLength="80" Flex="7" EnableKeyEvents="true" />
                    </Items>
                </ext:FieldContainer>

                <ext:FieldContainer ID="FieldContainer3" runat="server" Layout="HBoxLayout">
                    <Items>
                        <ext:NumberField ID="PaymentDaysNF" runat="server" FieldLabel="Срок оплаты" AllowBlank="true" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" MaxLength="80" Flex="7" EnableKeyEvents="true" />
                        <ext:Component ID="Component3" runat="server" Flex="1" />
                        <ext:NumberField ID="NdsPriceNF" runat="server" FieldLabel="НДС:" AllowBlank="true" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" MaxLength="80" Flex="7" EnableKeyEvents="true" />                     
                    </Items>
                </ext:FieldContainer>

                <ext:FieldContainer ID="FieldContainer15" runat="server" Layout="HBoxLayout">
                    <Items>
                        <ext:NumberField ID="PaymentNF" runat="server" FieldLabel="Оплата:" AllowBlank="true" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" Flex="7" EnableKeyEvents="true" />
                        <ext:Component ID="Component14" runat="server" Flex="1" />
                        <ext:NumberField ID="BalanceNF" runat="server" FieldLabel="Остаток:" AllowBlank="true" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" MaxLength="80" Flex="7" EnableKeyEvents="true" />
                    </Items>
                </ext:FieldContainer>
            </Items>
            <Buttons>
                <ext:Button ID="AcceptAddContractB" runat="server" Text="Добавить" Icon="Disk" Hidden="true" /> 
                <ext:Button ID="AcceptEditContractB" runat="server" Text="Сохранить изменения" Icon="Disk" Hidden="true" />    
                <ext:Button ID="CancelAddEditContractB" runat="server" Text="Отменить" Icon="Cancel" />                     
            </Buttons>
        </ext:Window>

        <ext:Window ID="OrganizationW" runat="server" Closable="false" Width="720" Height="600"  BodyPadding="5" Modal="true" Hidden="true" ButtonAlign="Center" Resizable="true" Layout="FitLayout">   
            <Items>
                <ext:GridPanel ID="OrganizationGP" runat="server" Layout="FitLayout" AutoScroll="true" MultiSelect="false">
                    <SelectionModel><ext:RowSelectionModel></ext:RowSelectionModel></SelectionModel>
                    <TopBar>
                        <ext:Toolbar ID="OrganizationTB" runat="server">
                            <Items>
                                <ext:Button ID="AddOrganizationB" runat="server" Icon="Add" Text="Добавить" />
                                <ext:Button ID="EditOrganizationB" runat="server" Icon="Pencil" Text="Редактировать" />
                                <ext:ToolbarFill />
                                <ext:Button ID="DeleteOrganizationB" runat="server" Icon="Delete" Text="Удалить" />
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
                                        <ext:ModelField Name="title_region" />
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
                                    </Fields>
                                </ext:Model>
                            </Model>                                
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>            
                            <ext:Column ID="Id_organization" runat="server" DataIndex="id_organization" Hidden="true" Text="Id организации" Width="100" />
                            <ext:Column ID="Code_organization" runat="server" Text="Код организации" DataIndex="code_organization" Width="120">
                                <HeaderItems>
                                    <ext:Container runat="server" Layout="HBoxLayout" Margin="2">
                                        <Items>
                                            <ext:CycleButton runat="server" ShowText="true" Width="48" ForceIcon="#Magnifier">
                                                <Menu>
                                                    <ext:Menu runat="server">
                                                        <Items>
                                                            <ext:CheckMenuItem runat="server" Text="=" ToolTip="Равно" />
                                                            <ext:CheckMenuItem runat="server" Text="+" Checked="true" ToolTip="Начинается на" />
                                                            <ext:CheckMenuItem runat="server" Text="-" ToolTip="Заканчивается на" />
                                                            <ext:CheckMenuItem runat="server" Text="*" ToolTip="Содержит" />
                                                            <ext:CheckMenuItem runat="server" Text="!" ToolTip="Не содержит" />
                                                        </Items>
                                                    </ext:Menu>
                                                </Menu>
                                                <Listeners>
                                                    <Change Handler="this.up('grid').filterHeader.onFieldChange(this.up('container'));" />
                                                </Listeners>                                               
                                            </ext:CycleButton>
                                            <ext:TextField runat="server" Flex="1">
                                                <Plugins>
                                                    <ext:ClearButton runat="server" />
                                                </Plugins>
                                                <Listeners>
                                                    <Change Handler="this.up('grid').filterHeader.onFieldChange(this.up('container'));" />
                                                </Listeners>
                                            </ext:TextField>
                                        </Items>
                                        <CustomConfig>
                                            <ext:ConfigItem Name="getValue" Value="getTextValue" Mode="Raw" />
                                        </CustomConfig>
                                    </ext:Container>
                                </HeaderItems>
                            </ext:Column>
                            <ext:Column ID="Id_region_organization" runat="server" DataIndex="id_region" Hidden="true" Text="Id района" />
                            <ext:Column ID="Code_region_organization" runat="server" DataIndex="code_region" Hidden="true" Text="Код района" />
                            <ext:Column ID="Title_region_organization" runat="server" DataIndex="title_region" Text="Район" Width="150">
                                <HeaderItems>
                                    <ext:Container runat="server" Layout="HBoxLayout" Margin="2">
                                        <Items>
                                            <ext:CycleButton runat="server" ShowText="true" Width="48" ForceIcon="#Magnifier">
                                                <Menu>
                                                    <ext:Menu runat="server">
                                                        <Items>
                                                            <ext:CheckMenuItem runat="server" Text="=" ToolTip="Равно" />
                                                            <ext:CheckMenuItem runat="server" Text="+" Checked="true" ToolTip="Начинается на" />
                                                            <ext:CheckMenuItem runat="server" Text="-" ToolTip="Заканчивается на" />
                                                            <ext:CheckMenuItem runat="server" Text="*" ToolTip="Содержит" />
                                                            <ext:CheckMenuItem runat="server" Text="!" ToolTip="Не содержит" />
                                                        </Items>
                                                    </ext:Menu>
                                                </Menu>
                                                <Listeners>
                                                    <Change Handler="this.up('grid').filterHeader.onFieldChange(this.up('container'));" />
                                                </Listeners>                                               
                                            </ext:CycleButton>
                                            <ext:TextField runat="server" Flex="1">
                                                <Plugins>
                                                    <ext:ClearButton runat="server" />
                                                </Plugins>
                                                <Listeners>
                                                    <Change Handler="this.up('grid').filterHeader.onFieldChange(this.up('container'));" />
                                                </Listeners>
                                            </ext:TextField>
                                        </Items>
                                        <CustomConfig>
                                            <ext:ConfigItem Name="getValue" Value="getTextValue" Mode="Raw" />
                                        </CustomConfig>
                                    </ext:Container>
                                </HeaderItems>
                            </ext:Column>
                            <ext:Column ID="Title_organization" runat="server" Text="Наименование организации" DataIndex="title_organization" Width="250"> 
                                <HeaderItems>
                                    <ext:Container runat="server" Layout="HBoxLayout" Margin="2">
                                        <Items>
                                            <ext:CycleButton runat="server" ShowText="true" Width="48" ForceIcon="#Magnifier">
                                                <Menu>
                                                    <ext:Menu runat="server">
                                                        <Items>
                                                            <ext:CheckMenuItem runat="server" Text="=" ToolTip="Равно" />
                                                            <ext:CheckMenuItem runat="server" Text="+" ToolTip="Начинается на" />
                                                            <ext:CheckMenuItem runat="server" Text="-" ToolTip="Заканчивается на" />
                                                            <ext:CheckMenuItem runat="server" Text="*" Checked="true" ToolTip="Содержит" />
                                                            <ext:CheckMenuItem runat="server" Text="!" ToolTip="Не содержит" />
                                                        </Items>
                                                    </ext:Menu>
                                                </Menu>
                                                <Listeners>
                                                    <Change Handler="this.up('grid').filterHeader.onFieldChange(this.up('container'));" />
                                                </Listeners>                                               
                                            </ext:CycleButton>
                                            <ext:TextField runat="server" Flex="1">
                                                <Plugins>
                                                    <ext:ClearButton runat="server" />
                                                </Plugins>
                                                <Listeners>
                                                    <Change Handler="this.up('grid').filterHeader.onFieldChange(this.up('container'));" />
                                                </Listeners>
                                            </ext:TextField>
                                        </Items>
                                        <CustomConfig>
                                            <ext:ConfigItem Name="getValue" Value="getTextValue" Mode="Raw" />
                                        </CustomConfig>
                                    </ext:Container>
                                </HeaderItems>
                            </ext:Column>
                            <ext:Column ID="Full_title_organization" runat="server" Text="Полное наименование организации" DataIndex="full_title_organization" Hidden="true"/> 
                            <ext:Column ID="Leader_organization" runat="server" DataIndex="leader" Hidden="true" Text="Глава организации" />
                            <ext:Column ID="Basis_document" runat="server" DataIndex="basis_document" Hidden="true" Text="Действует на основании" />
                            <ext:Column ID="Chief_agronomist_org" runat="server" DataIndex="chief_agronomist" Hidden="true" Text="Гл. агроном организации" />
                            <ext:Column ID="Legal_address" runat="server" DataIndex="legal_address" Hidden="true" Text="Юридический адрес" />
                            <ext:Column ID="Mailing_address" runat="server" DataIndex="mailing_address" Hidden="true" Text="Почтовый адрес" />
                            <ext:Column ID="EMail_address" runat="server" DataIndex="email_organization" Hidden="true" Text="E-Mail" />
                            <ext:Column ID="Inn_organization" runat="server" Text="ИНН организации" DataIndex="inn_organization"  Width="150">
                                <HeaderItems>
                                    <ext:Container runat="server" Layout="HBoxLayout" Margin="2">
                                        <Items>
                                            <ext:CycleButton runat="server" ShowText="true" Width="48" ForceIcon="#Magnifier">
                                                <Menu>
                                                    <ext:Menu runat="server">
                                                        <Items>
                                                            <ext:CheckMenuItem runat="server" Text="=" ToolTip="Равно" />
                                                            <ext:CheckMenuItem runat="server" Text="+" Checked="true" ToolTip="Начинается на" />
                                                            <ext:CheckMenuItem runat="server" Text="-" ToolTip="Заканчивается на" />
                                                            <ext:CheckMenuItem runat="server" Text="*" ToolTip="Содержит" />
                                                            <ext:CheckMenuItem runat="server" Text="!" ToolTip="Не содержит" />
                                                        </Items>
                                                    </ext:Menu>
                                                </Menu>
                                                <Listeners>
                                                    <Change Handler="this.up('grid').filterHeader.onFieldChange(this.up('container'));" />
                                                </Listeners>                                               
                                            </ext:CycleButton>
                                            <ext:TextField runat="server" Flex="1">
                                                <Plugins>
                                                    <ext:ClearButton runat="server" />
                                                </Plugins>
                                                <Listeners>
                                                    <Change Handler="this.up('grid').filterHeader.onFieldChange(this.up('container'));" />
                                                </Listeners>
                                            </ext:TextField>
                                        </Items>
                                        <CustomConfig>
                                            <ext:ConfigItem Name="getValue" Value="getTextValue" Mode="Raw" />
                                        </CustomConfig>
                                    </ext:Container>
                                </HeaderItems>
                            </ext:Column>
                            <ext:Column ID="Okato_organization" runat="server" Text="ОКАТО" Hidden="true" DataIndex="okato_organization" Width="140" />
                            <ext:Column ID="Oktmo_organization" runat="server" DataIndex="oktmo_organization" Hidden="true" Text="ОКТМО" />                                
                            <ext:Column ID="Kpp_organization" runat="server" DataIndex="kpp_organization" Hidden="true" Text="КПП" />
                            <ext:Column ID="Orgn_organization" runat="server" DataIndex="ogrn_organization" Hidden="true" Text="ОГРН" />
                            <ext:Column ID="Okved_organization" runat="server" DataIndex="okved_organization" Hidden="true" Text="ОКВЭД" />
                            <ext:Column ID="Okpo_organization" runat="server" DataIndex="okpo_organization" Hidden="true" Text="ОКПО" />
                            <ext:Column ID="Pay_account" runat="server" DataIndex="pay_account" Hidden="true" Text="Расчетный счет" />
                            <ext:Column ID="Full_bank_name" runat="server" DataIndex="full_bank_name" Hidden="true" Text="Полное наименование банка" />
                            <ext:Column ID="Bik" runat="server" DataIndex="bik" Hidden="true" Text="БИК" />
                            <ext:Column ID="Bank_correspond_account" runat="server" DataIndex="bank_correspond_account" Hidden="true" Text="Корреспондентский счет банка" />
                        </Columns>            
                    </ColumnModel>
                    <Plugins>
                        <ext:FilterHeader runat="server" />
                    </Plugins>
                </ext:GridPanel>
            </Items>
            <Buttons>
                <ext:Button ID="CancelSelectOrganizationB" runat="server" Text="Отменить" Icon="Cancel" /> 
            </Buttons>       
        </ext:Window>
        <ext:Window ID="AddEditOrganizationW" runat="server" Closable="false" Width="465" Height="720"  BodyPadding="5" Modal="true" Hidden="true" ButtonAlign="Center" Resizable="false" Layout="FormLayout">
            <Items>
                <ext:FieldContainer ID="FieldContainer6" runat="server" Layout="HBoxLayout">
                    <Items>
                        <ext:TextField ID="CodeRegionOrganizationTF" runat="server" FieldLabel="Код района" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="170" ReadOnly="true" />
                        <ext:Component ID="Component4" runat="server" Width="72" />
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
                <ext:FieldContainer ID="FieldContainer7" runat="server" Layout="HBoxLayout">
                    <Items>
                        <ext:TextField ID="OKATOOrganizationTF" runat="server" FieldLabel="ОКАТО" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="215" MinLength="8" MaxLength="14" MaskRe="/[0-9]/" />
                        <ext:Component ID="Component5" runat="server" Width="28" />
                        <ext:TextField ID="INNOrganizationTF" runat="server" FieldLabel="ИНН" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="50"  Width="200" MinLength="10" MaxLength="12" MaskRe="/[0-9]/" />
                    </Items>
                </ext:FieldContainer>
                <ext:FieldContainer ID="FieldContainer8" runat="server" Layout="HBoxLayout">
                    <Items>
                        <ext:TextField ID="KPPOrganizationTF" runat="server" FieldLabel="КПП" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="215" MinLength="8" MaxLength="14" MaskRe="/[0-9]/" />
                        <ext:Component ID="Component8" runat="server" Width="28" />
                        <ext:TextField ID="OGRNOrganizationTF" runat="server" FieldLabel="ОГРН" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="50"  Width="200" MinLength="10" MaxLength="12" MaskRe="/[0-9]/" />
                    </Items>
                </ext:FieldContainer>
                <ext:FieldContainer ID="FieldContainer9" runat="server" Layout="HBoxLayout">
                    <Items>
                        <ext:TextField ID="OKVEDOrganizationTF" runat="server" FieldLabel="ОКВЭД" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="215" MinLength="8" MaxLength="14" MaskRe="/[0-9]/" />
                        <ext:Component ID="Component9" runat="server" Width="28" />
                        <ext:TextField ID="OKPOOrganizationTF" runat="server" FieldLabel="ОКПО" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="50"  Width="200" MinLength="10" MaxLength="12" MaskRe="/[0-9]/" />
                    </Items>
                </ext:FieldContainer>
                
                <ext:TextField ID="PayAccountTF" runat="server" FieldLabel="Расчетный счет" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                <ext:TextField ID="FullBankNameTF" runat="server" FieldLabel="Полное наименование банка" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                <ext:TextField ID="BIKTF" runat="server" FieldLabel="БИК" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                <ext:TextField ID="BankCorrespondingAccountTF" runat="server" FieldLabel="Корресп. счет банка" AllowBlank="false" LabelSeparator="" LabelCls="darkslateblue-note" LabelWidth="100" Width="443" MaxLength="80" />
                
                <ext:TextField ID="IdOrganizationTF" runat="server" Hidden="true" />
                <ext:Component ID="Component10" runat="server" Width="5" />
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
                        <ext:Component ID="Component11" runat="server" Width="5" />
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
                                <ext:CellEditing ID="CellEditing2" runat="server" ClicksToEdit="2" />
                            </Plugins>  
                        </ext:GridPanel>                               
                    </Items>
                </ext:Container>
            </Items>
            <Buttons>
                <ext:Button ID="AcceptAddOrganizationB" runat="server" Text="Добавить" Icon="Add" Hidden="true" /> 
                <ext:Button ID="AcceptEditOrganizationB" runat="server" Text="Сохранить изменения" Icon="Disk" Hidden="true" />    
                <ext:Button ID="CancelAddEditOrganizationB" runat="server" Text="Отменить" Icon="Cancel" />                     
            </Buttons>
        </ext:Window> 
        <ext:Window ID="ConfirmDeleteOrganizationW" runat="server" Title="Удаление организации" Closable="true" Resizable="false" Height="120" Icon="Delete" Width="360" Modal="true" BodyPadding="5" Layout="Form" ButtonAlign="Center" Hidden="true">
                <Items>
                    <ext:FieldContainer ID="FieldContainer13" runat="server" Layout="HBoxLayout">
                        <Items>
                            <ext:Component ID="Component69" runat="server" Width="10" />
                            <ext:Label ID="MessageL" runat="server" Text="Для подтверждения удаления введите слово: Удалить"/>
                        </Items>
                    </ext:FieldContainer>
                    <ext:FieldContainer ID="FieldContainer14" runat="server" Layout="HBoxLayout">
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
        
    </form>
</body>
</html>
