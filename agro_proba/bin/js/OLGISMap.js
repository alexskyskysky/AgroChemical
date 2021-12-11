$(function () {
    $("#EditStyleW").dialog({
        autoOpen: false,
        height: 600,
        width: 860
    });

    $("#MenuPanel").draggable();
    $("#ControlMenu").dropDownPanels({
        speed: 100,
        resetTimer: 1000
    });
    $("#TerritoryMenu").dropDownPanels({
        speed: 100,
        resetTimer: 1000
    });
    $("#ThemeMenu").dropDownPanels({
        speed: 100,
        resetTimer: 1000
    });
    $("#TileMapMenu").dropDownPanels({
        speed: 100,
        resetTimer: 1000
    });

    $(".button_a").button();
    $(".img_button_a").button();
    $(".report_button_a").button().css("width", "100px").css("height", "100px");

    //$("#Legend").draggable({ handle: "#LegendHeader" }).resizable({ minHeight: 200, minWidth: 100 });
    $("#Legend").dialog({ autoOpen: false, minHeight: 200, minWidth: 100, width: 300, height: 500, position: { my: "left top", at: "left+10 top+400", of: "#map" } });

    $("#HistoryBook").dialog({
        autoOpen: false,
        height: 600,
        width: 730,
        minHeight: 400,
        minWidth: 400,
        maxHeight: 800,
        maxWidth: 850,
        buttons: {
            "Сохранить": saveData,
            "Закрыть": function () { $(this).dialog("close"); }
        }
    });

    /*$("#DosesCalculationsW").dialog({
        autoOpen: false,
        height: 600,
        width: 720,
        minHeight: 400,
        minWidth: 400,
        maxHeight: 800,
        maxWidth: 850,
        buttons: {
            "Закрыть": function () { $(this).dialog("close"); }
        }
    });*/

    $("#SoilSampleW").dialog({
        autoOpen: false,
        height: 400,
        width: 1000,
        minHeight: 200,
        minWidth: 800
    });

    $("#PopupW").dialog({
        autoOpen: false,
        height: 440,
        width: 580,
        minHeight: 440,
        minWidth: 580
    });

    $("#ReportsW").dialog({
        autoOpen: false,
        height: 600,
        width: 600,
        minHeight: 400,
        minWidth: 400,
        maxHeight: 1000,
        maxWidth: 1000
    });

    $("#TheReportW").dialog({
        autoOpen: false,
        height: 700,
        width: 1000,
        minHeight: 200,
        minWidth: 300
    });

    $('#SelectTourW').dialog({
        autoOpen: false,
        height: 130,
        width: 200,
        minHeight: 130,
        minWidth: 200,
        maxHeight: 130,
        maxWidth: 200,
        buttons: {
            "ОК": ShowSignificativeBySoilReport,
            "Отмена": function () { $(this).dialog("close"); }
        }
    });

    $('#SelectTourErosionChangeW').dialog({
        autoOpen: false,
        height: 130,
        width: 200,
        minHeight: 130,
        minWidth: 200,
        maxHeight: 130,
        maxWidth: 200,
        buttons: {
            "ОК": ShowErosionChangeReport,
            "Отмена": function () { $(this).dialog("close"); }
        }
    });

    $('#SelectUsingWateringTypeFarmlandTourW').dialog({
        autoOpen: false,
        height: 130,
        width: 200,
        minHeight: 130,
        minWidth: 200,
        maxHeight: 130,
        maxWidth: 200,
        buttons: {
            "ОК": ShowUsingWateringTypeFarmlandRegionReport,
            "Отмена": function () { $(this).dialog("close"); }
        }
    });

    $('#SelectYearW').dialog({
        autoOpen: false,
        height: 130,
        width: 200,
        minHeight: 130,
        minWidth: 200,
        maxHeight: 130,
        maxWidth: 200,
        buttons: {
            "ОК": ShowCulturesReport,
            "Отмена": function () { $(this).dialog("close"); }
        }
    });

    $('#SelectYearCultureW').dialog({
        autoOpen: false,
        height: 170,
        width: 200,
        minHeight: 170,
        minWidth: 200,
        maxHeight: 170,
        maxWidth: 200,
        buttons: {
            "ОК": ShowProductivityReport,
            "Отмена": function () { $(this).dialog("close"); }
        }
    });

    $('#SelectYearOrganicFertilizerW').dialog({
        autoOpen: false,
        height: 130,
        width: 200,
        minHeight: 130,
        minWidth: 200,
        maxHeight: 130,
        maxWidth: 200,
        buttons: {
            "ОК": ShowOrganicFertilizerReport,
            "Отмена": function () { $(this).dialog("close"); }
        }
    });

    $('#SelectYearPestsDiseasesWeedinessW').dialog({
        autoOpen: false,
        height: 130,
        width: 200,
        minHeight: 130,
        minWidth: 200,
        maxHeight: 130,
        maxWidth: 200,
        buttons: {
            "ОК": ShowPestsDiseasesWeedinessReport,
            "Отмена": function () { $(this).dialog("close"); }
        }
    });

    $("#GPSTrackersW").dialog({
        autoOpen: false,
        height: 300,
        width: 605,
        minHeight: 100,
        minWidth: 200,
        maxHeight: 600,
        maxWidth: 620,
        position: { my: "left top", at: "left+10 top+400", of: "#map" }
    });

    $("#PlantationsW").dialog({
        autoOpen: false,
        height: 600,
        width: 730,
        minHeight: 400,
        minWidth: 400
    });

    $("#HistoryBookTabs").tabs();
    $("#BasicTabs").tabs();
    $("#AgroParametersTabs").tabs();
    $("#CropRotationTabs").tabs();
    $("#DosesCalculationsW").tabs();

    $("#SowingDateTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#HarvestDateTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#DateBasicFertTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#DateSowingFertTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#DateDressingFertTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#OldDateBasicFertTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#OldDateSowingFertTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#OldDateDressingFertTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#DateOrganicFertTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#OldDateOrganicFertTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#DateTillageTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#DatePlantProtectionTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#DateAmeliorator1TB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#DateAmeliorator2TB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#DateTrackPointsFromTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#DateTrackPointsToTB").datepicker({ dateFormat: "dd.mm.yy" });

    $("#DatePestTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#DateDiseaseTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#DateWeedTB").datepicker({ dateFormat: "dd.mm.yy" });
    $("#DateWeedinessTB").datepicker({ dateFormat: "dd.mm.yy" });

    $("#SelectProtocolW").dialog({
        autoOpen: false,
        height: 240,
        width: 360,
       /* minHeight: 170,
        minWidth: 200,
        maxHeight: 170,
        maxWidth: 200,*/
        buttons: {
            "ОК": AcceptSelectProtocol,
            "Отмена": function () { $(this).dialog("close"); }
        }
    });

    $("#select_protocol_B").button().click(function (event) {
        CallServer('select_protocol_window:' + $("#TerritoryCB").val() + '|' + $("#RegionCB").val(), 'null');
        $("#SelectProtocolW").dialog("open");
    });

    $('#SelectProtocolRegionCB').on("change", function () {
        CallServer('select_protocol_farm_organization:' + $("#SelectProtocolFarmOrgCB").val() + '|' + $("#SelectProtocolRegionCB").val(), 'null');
    });

    $('#SelectProtocolFarmOrgCB').on("change", function () {
        CallServer('select_protocol_farm_organization:' + $("#SelectProtocolFarmOrgCB").val() + '|' + $("#SelectProtocolRegionCB").val(), 'null');
    });

    $('#SelectProtocolFarmCB').on("change", function () {
        CallServer('select_protocol_farm:' + $("#SelectProtocolFarmCB").val(), 'null');
    });

    $('#SelectProtocolLagoonCB').on("change", function () {
        CallServer('select_protocol_lagoon:' + $("#SelectProtocolLagoonCB").val(), 'null');
    });

    $("#show_protocol_B").button().click(function (event) {
        ShowFertilizerProtocolReport();
    });

    $("#delete_protocol_B").button().click(function (event) {
        DeleteFertilizerProtocol();
    });

    $(".sprav-combobox").prepend($('<option value="0"></option>'));
    $(".sprav-combobox option:nth-child(1)").attr('selected', 'selected');

    $("#SpravCropRotationB").button({ icons: { primary: "ui-icon-help", secondary: "ui-icon-help" } });
    $("#SpravCropRotationB").tooltip({ track: true });

    $("#add_tillage_B").button().click(function (event) {
        if ($("#TillageCB").val() != '' && $("#DateTillageTB").val() != '') {
            $("#TillageT").append('<tr><td hidden="hidden">' + $("#TillageCB").val() + '</td><td width="250">' + $("#TillageCB option:selected").html() +
                '</td><td width="100">' + $("#DepthTillageTB").val() +
                '</td><td width="100">' + $("#DateTillageTB").val() +
                '</td><td align="center" width="100"><a class="delete-tillage" href="#delete_tillage">Удалить</a></td></tr>');
            $(".delete-tillage").button().click(function (event) {
                $($(this).parents().get(1)).remove();
            });
        }
        else {
            alert('Заполнены не все поля!');
        }
    });

    $("#add_drug_B").button().click(function (event) {
        if ($("#DatePlantProtectionTB").val() != '' && $("#DrugCB").val() != '' && $("#DoseDrugTB").val() != '') {
            $("#PlantProtectionT").append('<tr><td hidden="hidden">' + $("#TypeDrugCB").val() + '</td><td width="100">' + $("#TypeDrugCB option:selected").html() + '</td><td hidden="hidden">' +
                $("#DrugCB").val() + '</td><td width="250">' + $("#DrugCB option:selected").html() + '</td><td width="50">' + $("#DoseDrugTB").val() + '</td><td width="100">' + $("#DatePlantProtectionTB").val() +
                '</td><td align="center" width="100"><a class="delete-plant-protection" href="#delete_plant_protection">Удалить</a></td></tr>');
            $(".delete-plant-protection").button().click(function (event) {
                $($(this).parents().get(1)).remove();
            });
        }
        else {
            alert('Заполнены не все поля!');
        }
    });

    $("#add_pest_B").button().click(function (event) {
        if ($("#CultureCB").val() != '') {
            if ($("#PhasePestsCB").val() != '' && $("#PestsCB").val() != '' && $("#CountPestTB").val() != '') {
                $("#PestsT").append('<tr><td hidden="hidden">' + $("#PhasePestsCB").val() + '</td><td width="150">' + $("#PhasePestsCB option:selected").html() + '</td><td hidden="hidden">' +
                $("#PestsCB").val() + '</td><td width="150">' + $("#PestsCB option:selected").html() + '</td><td width="50">' + $("#CountPestTB").val() + '</td><td width="100">' + $("#DatePestTB").val() +
                '</td><td align="center" width="100"><a class="delete-pest" href="#delete_pest">Удалить</a></td></tr>');
                $(".delete-pest").button().click(function (event) {
                    $($(this).parents().get(1)).remove();
                });
            }
            else {
                alert('Заполнены не все поля!');
            }
        }
        else {
            alert('Не выбрана культура!');
        }
    });

    $("#add_weed_B").button().click(function (event) {
        if ($("#CultureCB").val() != '') {
            if ($("#PhaseWeedsCB").val() != '' && $("#WeedsCB").val() != '' && $("#CountWeedTB").val() != '') {
                $("#WeedsT").append('<tr><td hidden="hidden">' + $("#PhaseWeedsCB").val() + '</td><td width="150">' + $("#PhaseWeedsCB option:selected").html() + '</td><td hidden="hidden">' +
                $("#WeedsCB").val() + '</td><td width="150">' + $("#WeedsCB option:selected").html() + '</td><td width="50">' + $("#CountWeedTB").val() + '</td><td width="100">' + $("#DateWeedTB").val() +
                '</td><td align="center" width="100"><a class="delete-weed" href="#delete_weed">Удалить</a></td></tr>');
                $(".delete-weed").button().click(function (event) {
                    $($(this).parents().get(1)).remove();
                });
            }
            else {
                alert('Заполнены не все поля!');
            }
        }
        else {
            alert('Не выбрана культура!');
        }
    });

    $("#add_disease_B").button().click(function (event) {
        if ($("#CultureCB").val() != '') {
            if ($("#PhaseDiseasesCB").val() != '' && $("#DiseasesCB").val() != '' && $("#PercentDiseaseTB").val() != '') {
                $("#DiseasesT").append('<tr><td hidden="hidden">' + $("#PhaseDiseasesCB").val() + '</td><td width="150">' + $("#PhaseDiseasesCB option:selected").html() + '</td><td hidden="hidden">' +
                $("#DiseasesCB").val() + '</td><td width="150">' + $("#DiseasesCB option:selected").html() + '</td><td width="50">' + $("#PercentDiseaseTB").val() + '</td><td width="100">' + $("#DateDiseaseTB").val() +
                '</td><td align="center" width="100"><a class="delete-disease" href="#delete_disease">Удалить</a></td></tr>');
                $(".delete-disease").button().click(function (event) {
                    $($(this).parents().get(1)).remove();
                });
            }
            else {
                alert('Заполнены не все поля!');
            }
        }
        else {
            alert('Не выбрана культура!');
        }
    });

    $("#add_weediness_B").button().click(function (event) {
        if ($("#CultureCB").val() != '') {
            if ($("#CountWeedinessTB").val() != '' || $("#PercentWeedinessTB").val() != '') {
                $("#WeedinessT").append('<tr><td width="100">' + $("#WeedinessTB").val() + '</td>' +
                '<td width="75">' + $("#CountWeedinessTB").val() + '</td><td width="75">' + $("#PercentWeedinessTB").val() + '</td><td width="100">' + $("#DateWeedinessTB").val() +
                '</td><td align="center" width="100"><a class="delete-weediness" href="#delete_weediness">Удалить</a></td></tr>');
                $(".delete-weediness").button().click(function (event) {
                    $($(this).parents().get(1)).remove();
                });
            }
            else {
                alert('Заполнены не все поля!');
            }
        }
        else {
            alert('Не выбрана культура!');
        }
    });

    $("#LoginW").dialog({
        autoOpen: true,
        height: 270,
        width: 380,
        modal: true,
        maxHeight: 270,
        maxWidth: 380,
        minHeight: 270,
        minWidth: 380,
        buttons: {
            "Вход": LoginToMap
        },
        open: function () { $(this).parents(".ui-dialog:first").find(".ui-dialog-titlebar-close").remove(); }
    });

    $("#LoginTB").keypress(function (event) { if (event.which == 13) { LoginToMap(); } });
    $("#PasswordTB").keypress(function (event) { if (event.which == 13) { LoginToMap(); } });

    /*$("#ShowPlotInfoI").button();
    $("#ShowLegendI").button();
    $("#ShowSoilSampleInfoI").button();
    $("#ShowReportsI").button();
    $("#CenterMapI").button();*/
});

function CenterMap() {
    if (map != null && current_extent != null) { map.getView().fit(current_extent, map.getSize()); };
}

function ShowLegend() {
    $("#Legend").dialog("open");
}

function ClearDosesCalculations() {
    /*$("#Loss_nTB").val('');
    $("#Loss_p2o5TB").val('');
    $("#Loss_k2oTB").val('');

    $("#K1_nTB").val('');
    $("#K1_p2o5TB").val('');
    $("#K1_k2oTB").val('');
    $("#K2_nTB").val('');
    $("#K3_nTB").val('');
    $("#K3_p2o5TB").val('');
    $("#K3_k2oTB").val('');
    $("#K4_nTB").val('');
    $("#K4_p2o5TB").val('');
    $("#K4_k2oTB").val('');
    $("#K5_nTB").val('');
    $("#K5_p2o5TB").val('');
    $("#K5_k2oTB").val('');
    $("#K6_nTB").val('');
    $("#K6_p2o5TB").val('');
    $("#K6_k2oTB").val('');
    $("#K7_p2o5TB").val('');
    $("#K7_k2oTB").val('');
    $("#K8_p2o5TB").val('');*/

    $("#D_NTB").val('');
    $("#D_P2O5TB").val('');
    $("#D_K2OTB").val('');

    $("#DNL").attr('title', '');
    $("#LossNL").attr('title', '');
    $("#ProductivityNL").attr('title', '');
    $("#K1NL").attr('title', '');
    $("#K2NL").attr('title', '');
    $("#K3NL").attr('title', '');
    $("#DoNL").attr('title', '');
    $("#K4NL").attr('title', '');
    $("#DopNL").attr('title', '');
    $("#K5NL").attr('title', '');
    
    $("#DP2O5L").attr('title', '');
    $("#LossP2O5L").attr('title', '');
    $("#ProductivityP2O5L").attr('title', '');
    $("#K1P2O5L").attr('title', '');
    $("#K3P2O5L").attr('title', '');
    $("#K7P2O5L").attr('title', '');
    $("#K8P2O5L").attr('title', '');
    $("#DoP2O5L").attr('title', '');
    $("#K4P2O5L").attr('title', '');
    $("#DopP2O5L").attr('title',''); 
    $("#K5P2O5L").attr('title', '');
    $("#DpP2O5L").attr('title', '');
    $("#K6P2O5L").attr('title', '');
    
    $("#DK2OL").attr('title', '');
    $("#LossK2OL").attr('title', '');
    $("#ProductivityK2OL").attr('title', '');
    $("#K1K2OL").attr('title', '');
    $("#K3K2OL").attr('title', '');
    $("#K7K2OL").attr('title', '');
    $("#DoK2OL").attr('title', '');
    $("#K4K2OL").attr('title', '');
    $("#DopK2OL").attr('title', '');
    $("#K5K2OL").attr('title', '');
    $("#DpK2OL").attr('title', '');
    $("#K6K2OL").attr('title', '');

    if ($("#PlannedProdTB").val() != '0' && $("#CultureCB").val() != '0' && $("#OldCultureCB").val() != '0' && $("#CultureZoneCB").val() != '0') {
        CallServer('doses_calculations_culture:' + $("#UniqNumberTB").val() + '|' + $("#CultureCB").val() + '|' + $("#CultureZoneCB").val() + '|' + $("#PlannedProdTB").val() + '|' + $("#OrganicTotalDoseNTB").val() + '|' + $("#OrganicTotalDosePTB").val() + '|' + $("#OrganicTotalDoseKTB").val() + '|' + $("#OldOrganicTotalDoseNTB").val() + '|' + $("#OldOrganicTotalDosePTB").val() + '|' + $("#OldOrganicTotalDoseKTB").val() + '|' + $("#OldTotalDoseNTB").val() + '|' + $("#OldTotalDosePTB").val() + '|' + $("#OldTotalDoseKTB").val() + '|' + $("#Year3CB").val() + '|' + $("#AreaTB").val(), 'null');
    }
}

function SetDateHistoryBook() {
    var date = new Date();
    var year = date.getFullYear();
    $("#Year3CB").empty();
    for (var i = -2; i < 8; i++) {
        var option_year = year + i;
        $("#Year3CB").append('<option value="' + option_year + '">' + option_year + '</option>');
    }
    $("#Year3CB option:nth-child(3)").attr('selected', 'selected');

    $("#Year4CB").empty();
    for (var i = -2; i < 8; i++) {
        var option_year = year + i;
        $("#Year4CB").append('<option value="' + option_year + '">' + option_year + '</option>');
    }
    $("#Year4CB option:nth-child(3)").attr('selected', 'selected');
}

function ShowHistoryBook() {
    SetDateHistoryBook();
    $("#HistoryBook").dialog("open");
    var currentTime = new Date();
    $("#DosesCT").css('display', 'none');
    if (currentTime.getFullYear() + 1 == $("#Year3CB").val())
        $("#DosesCT").css('display', 'inline');

}

function ShowSoilSampleW() {
    $("#SoilSampleW").dialog("open");
}

function ShowReports() {
    if ($('#OrganizationCB').val() == null) { $('#OrganizationCB').val('0'); };
    if ($('#RegionCB').val() == null) { $('#RegionCB').val('0'); };
    if ($('#TerritoryCB').val() == null) { $('#TerritoryCB').val('0'); };
    var values = get_cookie('Agrochim31_Map_User').split('&')[0].split('=')[1] + '|' + get_cookie('Agrochim31_Map_User').split('&')[4].split('=')[1] + '|'
                + $('#OrganizationCB').val() + '|'
                + $('#RegionCB').val() + '|'
                + $('#TerritoryCB').val();
    CallServer('getaccessreports:' + values, 'null');
    $("#ReportsW").dialog("open");
}

function ShowTrackers() {
    var values = get_cookie('Agrochim31_Map_User').split('&')[0].split('=')[1] + '|' + get_cookie('Agrochim31_Map_User').split('&')[4].split('=')[1];
    CallServer('gettrackers:' + $("#OrganizationCB").val() + '|' + values, 'null');
}

function GetFactCaCO3() {
    var v1 = 0, v2 = 0, v3 = 0;
    if ($("#PercentCaCO3_1TB").val() != '' && $("#DoseAmeliorator1TB").val() != '') {
        v1 = $("#PercentCaCO3_1TB").val() * $("#DoseAmeliorator1TB").val() / 100;
    }
    if ($("#PercentCaCO3_2TB").val() != '' && $("#DoseAmeliorator2TB").val() != '') {
        v2 = $("#PercentCaCO3_2TB").val() * $("#DoseAmeliorator2TB").val() / 100;
    }
    v3 = v1 + v2;
    return v3;
}

function saveData() {
    var str = '';
    str += ($("#UniqNumberTB").val() + '|');
    str += ($("#Year3CB").val() + '|');
    str += ($("#SortCRCB").val() + '|');

    str += ($("#TypePropertyCB").val() + '&');

    str += ($("#CultureCB").val() + '|');
    str += ($("#OldCultureCB").val() + '|');
    str += ($("#CrossCultureCB").val() + '|');
    str += ($("#ReproductionTB").val() + '|');
    str += ($("#SeedingRateTB").val() + '|');
    str += ($("#PlannedProdTB").val() + '|');
    str += ($("#SowingDateTB").val() + '|');
    str += ($("#ActualProdTB").val() + '|');
    str += ($("#HarvestDateTB").val() + '|');
    str += ($("#CultureZoneCB").val() + '&');

    str += ($("#BasicFertCB").val() + '|');
    str += ($("#DoseBasicFertTB").val() + '|');
    str += ($("#DateBasicFertTB").val() + '|');
    str += ($("#SowingFertCB").val() + '|');
    str += ($("#DoseSowingFertTB").val() + '|');
    str += ($("#DateSowingFertTB").val() + '|');
    str += ($("#DressingFertCB").val() + '|');
    str += ($("#DoseDressingFertTB").val() + '|');
    str += ($("#DateDressingFertTB").val() + '|');
    str += ($("#OldBasicFertCB").val() + '|');
    str += ($("#OldDoseBasicFertTB").val() + '|');
    str += ($("#OldDateBasicFertTB").val() + '|');
    str += ($("#OldSowingFertCB").val() + '|');
    str += ($("#OldDoseSowingFertTB").val() + '|');
    str += ($("#OldDateSowingFertTB").val() + '|');
    str += ($("#OldDressingFertCB").val() + '|');
    str += ($("#OldDoseDressingFertTB").val() + '|');
    str += ($("#OldDateDressingFertTB").val() + '&');

    str += ($("#OrganicFertCB").val() + '|');
    str += ($("#DoseOrganicFertTB").val() + '|');
    str += ($("#DateOrganicFertTB").val() + '|');
    str += ($("#OldOrganicFertCB").val() + '|');
    str += ($("#OldDoseOrganicFertTB").val() + '|');
    str += ($("#OldDateOrganicFertTB").val() + '|');
    str += ($("#IdProtocolTB").val() + '&');

    str += ($("#Ameliorator1CB").val() + '|');
    str += ($("#PercentCaCO3_1TB").val() + '|');
    str += ($("#DoseAmeliorator1TB").val() + '|');
    str += ($("#DateAmeliorator1TB").val() + '|');
    str += ($("#Ameliorator2CB").val() + '|');
    str += ($("#PercentCaCO3_2TB").val() + '|');
    str += ($("#DoseAmeliorator2TB").val() + '|');
    str += $("#DateAmeliorator2TB").val();
    str += '&';
    var tillage_rows_count = $('#TillageT tr').length;
    var tillage_rows_str = [];
    if (tillage_rows_count > 0) {
        for (var i = 0; i < tillage_rows_count; i++) {
            tillage_rows_str[i] = '';

            tillage_rows_str[i] += ($('#TillageT tr').eq(i).find('td').eq(0).text() + ';');
            tillage_rows_str[i] += ($('#TillageT tr').eq(i).find('td').eq(2).text() + ';');
            tillage_rows_str[i] += ($('#TillageT tr').eq(i).find('td').eq(3).text());

            if (i != (tillage_rows_count - 1)) {
                tillage_rows_str[i] += '|';
            }

            str += tillage_rows_str[i];
        }
    }
    str += '&';
    var drugs_rows_count = $('#PlantProtectionT tr').length;
    var drugs_rows_str = [];
    if (drugs_rows_count > 0) {
        for (var i = 0; i < drugs_rows_count; i++) {
            drugs_rows_str[i] = '';

            drugs_rows_str[i] += ($('#PlantProtectionT tr').eq(i).find('td').eq(0).text() + ';');
            drugs_rows_str[i] += ($('#PlantProtectionT tr').eq(i).find('td').eq(2).text() + ';');
            drugs_rows_str[i] += ($('#PlantProtectionT tr').eq(i).find('td').eq(4).text() + ';');
            drugs_rows_str[i] += ($('#PlantProtectionT tr').eq(i).find('td').eq(5).text());

            if (i != (drugs_rows_count - 1)) {
                drugs_rows_str[i] += '|';
            }

            str += drugs_rows_str[i];
        }
    }
    str += '&';
    var pests_rows_count = $('#PestsT tr').length;
    var pests_rows_str = [];
    if (pests_rows_count > 0) {
        for (var i = 0; i < pests_rows_count; i++) {
            pests_rows_str[i] = '';

            pests_rows_str[i] += ($('#PestsT tr').eq(i).find('td').eq(0).text() + ';');
            pests_rows_str[i] += ($('#PestsT tr').eq(i).find('td').eq(2).text() + ';');
            pests_rows_str[i] += ($('#PestsT tr').eq(i).find('td').eq(4).text() + ';');
            pests_rows_str[i] += ($('#PestsT tr').eq(i).find('td').eq(5).text());

            if (i != (pests_rows_count - 1)) {
                pests_rows_str[i] += '|';
            }

            str += pests_rows_str[i];
        }
    }
    str += '&';
    var diseases_rows_count = $('#DiseasesT tr').length;
    var diseases_rows_str = [];
    if (diseases_rows_count > 0) {
        for (var i = 0; i < diseases_rows_count; i++) {
            diseases_rows_str[i] = '';

            diseases_rows_str[i] += ($('#DiseasesT tr').eq(i).find('td').eq(0).text() + ';');
            diseases_rows_str[i] += ($('#DiseasesT tr').eq(i).find('td').eq(2).text() + ';');
            diseases_rows_str[i] += ($('#DiseasesT tr').eq(i).find('td').eq(4).text() + ';');
            diseases_rows_str[i] += ($('#DiseasesT tr').eq(i).find('td').eq(5).text());

            if (i != (diseases_rows_count - 1)) {
                diseases_rows_str[i] += '|';
            }

            str += diseases_rows_str[i];
        }
    }
    str += '&';
    var weeds_rows_count = $('#WeedsT tr').length;
    var weeds_rows_str = [];
    if (weeds_rows_count > 0) {
        for (var i = 0; i < weeds_rows_count; i++) {
            weeds_rows_str[i] = '';

            weeds_rows_str[i] += ($('#WeedsT tr').eq(i).find('td').eq(0).text() + ';');
            weeds_rows_str[i] += ($('#WeedsT tr').eq(i).find('td').eq(2).text() + ';');
            weeds_rows_str[i] += ($('#WeedsT tr').eq(i).find('td').eq(4).text() + ';');
            weeds_rows_str[i] += ($('#WeedsT tr').eq(i).find('td').eq(5).text());

            if (i != (weeds_rows_count - 1)) {
                weeds_rows_str[i] += '|';
            }

            str += weeds_rows_str[i];
        }
    }
    str += '&';
    var weediness_rows_count = $('#WeedinessT tr').length;
    var weediness_rows_str = [];
    if (weediness_rows_count > 0) {
        for (var i = 0; i < weediness_rows_count; i++) {
            weediness_rows_str[i] = '';

            weediness_rows_str[i] += ($('#WeedinessT tr').eq(i).find('td').eq(1).text() + ';');
            weediness_rows_str[i] += ($('#WeedinessT tr').eq(i).find('td').eq(2).text() + ';');
            weediness_rows_str[i] += ($('#WeedinessT tr').eq(i).find('td').eq(3).text());

            if (i != (weediness_rows_count - 1)) {
                weediness_rows_str[i] += '|';
            }

            str += weediness_rows_str[i];
        }
    }

    CallServer('savedata:' + str, 'null');
    //$(this).dialog("close");
}

function LoginToMap() {
    if ($("#LoginTB").val() == '' || $("#PasswordTB").val() == '') {
        alert('Не введён логин или пароль!');
    }
    else {
        CallServer('login:' + $("#LoginTB").val() + '|' + $("#PasswordTB").val(), 'null');
    }
}

function SetColors(array_colors, count_colors, opacity_colors) {
    var r = 255, g = 0, b = 0;
    var shag = Number(String(1280 / count_colors).split('.')[0]);
    var all_colors = [];
    all_colors[0] = 'rgba(' + r + ',' + g + ',' + b + ',' + opacity_colors + ')';

    for (var i = 1; i < 1280; i++) {
        if (i < 256) {
            if (g < 255)
                g++;
        }
        if (i >= 256 && i < 512) {
            if (r > 0)
                r--;
        }
        if (i >= 512 && i < 768) {
            if (b < 255)
                b++;
        }
        if (i >= 768 && i < 1024) {
            if (g > 0)
                g--;
        }
        if (i >= 1024) {
            if (r < 255)
                r++;
        }
        all_colors[i] = 'rgba(' + r + ',' + g + ',' + b + ',' + opacity_colors + ')';
    }
    for (var j = 0; j < count_colors; j++) {
        array_colors[j] = all_colors[j * shag];
    }
}

function get_cookie(cookie_name) {
    var results = document.cookie.match('(^|;) ?' + cookie_name + '=([^;]*)(;|$)');

    if (results)
        return (unescape(results[2]));
    else
        return null;
}

//выход пользователя
function UserExit() {
    $('#PasswordTB').val('');
    $('#TerritoryCB option:selected').each(function () { $(this).prop('selected', false); });
    $('#TerritoryCB option[value=0]').prop('selected', true);
    $('#TerritoryCB').empty();

    $('#RegionCB option:selected').each(function () { $(this).prop('selected', false); });
    $('#RegionCB option[value=0]').prop('selected', true);
    $('#RegionCB').empty();

    $('#OrganizationCB option:selected').each(function () { $(this).prop('selected', false); });
    $('#OrganizationCB option[value=0]').prop('selected', true);
    $('#OrganizationCB').empty();

    if (vector_regions != null) { map.removeLayer(vector_regions); }
    if (vector_organizations != null) { map.removeLayer(vector_organizations); }
    if (vector_plots != null) { map.removeLayer(vector_plots); }
    if (vector_soil != null) { map.removeLayer(vector_soil); }
    if (vector_soil_points != null) { map.removeLayer(vector_soil_points); }
    if (vector_erosion != null) { map.removeLayer(vector_erosion); }
    if (vector_slope != null) { map.removeLayer(vector_slope); }
    if (vector_exposure != null) { map.removeLayer(vector_exposure); }
    if (vector_typing != null) { map.removeLayer(vector_typing); }
    if (vector_project_plots != null) { map.removeLayer(vector_project_plots); }
    if (vector_grassing != null) { map.removeLayer(vector_grassing); }
    if (vector_water_objects != null) { map.removeLayer(vector_water_objects); }
    if (vector_woodland_belts != null) { map.removeLayer(vector_woodland_belts); }
    if (vector_farms != null) { map.removeLayer(vector_farms); }
    if (vector_lagoons != null) { map.removeLayer(vector_lagoons); }

    if (map != null && view != null) {
        view = new ol.View({ center: [11799431.182326088, 9558909.009231001], zoom: 4 });
        map.setView(view);
        if (vector_territory != null && zoom_to_extent != null && current_extent != null) {
            map.removeControl(zoom_to_extent);
            current_extent = vector_territory.getSource().getExtent();
            zoom_to_extent = new ol.control.ZoomToExtent({ extent: current_extent });
            map.addControl(zoom_to_extent);
        }
    }

    document.cookie = 'Agrochim31_Map_User=id_user=0&surname=0&name=0&patronymic=0&type_user=1';
    document.cookie = 'Agrochim31_For_Map=id_organization=0&year=0&tour=0&code_plot=0';

    $('#LoginW').dialog('open');
}

//переключение слоёв
function ChangeBaseMap() {
    if (osm_layer != null) {
        osm_layer.setVisible($('#OSMLayerRB').prop('checked'));
    }
    if (bing_road_layer != null) {
        bing_road_layer.setVisible($('#BingRoadLayerRB').prop('checked'));
    }
    if (bing_sat_layer != null) {
        bing_sat_layer.setVisible($('#BingSatLayerRB').prop('checked'));
    }
    if (bing_hyb_layer != null) {
        bing_hyb_layer.setVisible($('#BingHybLayerRB').prop('checked'));
    }
    /*if (yandex_sat_layer != null) {
        yandex_sat_layer.setVisible($('#YandexSatRB').prop('checked'));
    }
    if (current_projection != null){
        if ($('#YandexSatRB').prop('checked')) {
            current_projection = 'EPSG: 3395';
            ChangeProjection('EPSG:3857');
        }
        else {
            current_projection = 'EPSG: 3857';
            ChangeProjection('EPSG: 3395');
        }
    }*/
}

function ShowHideKadastr() {
    if (rr_kadastr_layer != null) {
        rr_kadastr_layer.setVisible($('#RRKadastrCB').prop('checked'));
        //rr_kadastr_layer.setVisible(checked);
    }
}
//09082016 отображение ферм
function ShowHideFarm() {
    if (vector_farms != null) {
        var checked = $('#FarmCB').prop('checked');
        //vector_farms.setVisible(checked);
        if (vector_farms != null) { map.removeLayer(vector_farms); }
        if (vector_lagoons != null) { map.removeLayer(vector_lagoons); }
        CallServer('legend:' + $("#MapThemeCB").val(), 'null');
        if (checked) {
            CallServer('showfarm:' + $('#TerritoryCB').val() + "|" + $('#RegionCB').val(), 'null');
        }
    }
}

function ShowHideDig(){
    if (vector_dig_drid != null) {
        var checked = $('#DigCB').prop('checked');
        if (checked) {
            CallServer('showdig:' + $('#TerritoryCB').val() + "|" + $('#RegionCB').val(), 'null');
        }
    }
}

/*function ChangeProjection(old_projection)
{
    if(vector_territory != null){
        vector_territory.getSource().forEachFeature(function (feature) {
            feature.getGeometry().transform(old_projection, current_projection);
        });
    }
    if(vector_regions != null){
        vector_regions.getSource().forEachFeature(function (feature) {
            feature.getGeometry().transform(old_projection, current_projection);
        });
    }
    if(vector_organizations != null){
        vector_organizations.getSource().forEachFeature(function (feature) {
            feature.getGeometry().transform(old_projection, current_projection);
        });
    }
    if(vector_plots != null){
        vector_plots.getSource().forEachFeature(function (feature) {
            feature.getGeometry().transform(old_projection, current_projection);
        });
    }
    if(vector_soil != null){
        vector_soil.getSource().forEachFeature(function (feature) {
            feature.getGeometry().transform(old_projection, current_projection);
        });
    }
    if(vector_soil_points != null){
        vector_soil_points.getSource().forEachFeature(function (feature) {
            feature.getGeometry().transform(old_projection, current_projection);
        });
    }
    if(vector_erosion != null){
        vector_erosion.getSource().forEachFeature(function (feature) {
            feature.getGeometry().transform(old_projection, current_projection);
        });
    }
    if(vector_slope != null){
        vector_slope.getSource().forEachFeature(function (feature) {
            feature.getGeometry().transform(old_projection, current_projection);
        });
    }
    if(vector_typing != null){
        vector_typing.getSource().forEachFeature(function (feature) {
            feature.getGeometry().transform(old_projection, current_projection);
        });
    }
    if(vector_project_plots != null){
        vector_project_plots.getSource().forEachFeature(function (feature) {
            feature.getGeometry().transform(old_projection, current_projection);
        });
    }
    if(vector_grassing != null){
        vector_grassing.getSource().forEachFeature(function (feature) {
            feature.getGeometry().transform(old_projection, current_projection);
        });
    }
    if(vector_water_objects != null){
        vector_water_objects.getSource().forEachFeature(function (feature) {
            feature.getGeometry().transform(old_projection, current_projection);
        });
    }
    if (vector_woodland_belts != null) {
        vector_woodland_belts.getSource().forEachFeature(function (feature) {
            feature.getGeometry().transform(old_projection, current_projection);
        });
    }
}*/

//работа с трекерами
function Tracker(id_tracker, checked) {
    this.id_tracker = id_tracker;
    this.checked = checked;
}

var tracker = {};
var trackers_count;
function SetTracks() {
    trackers_count = $('#GPSTrackersT tr').length;
    for (var i = 1; i < trackers_count; ++i) {
        tracker[$('#GPSTrackersT tr').eq(i).find('td').eq(5).find(':checkbox').attr('id')] = new Tracker($('#GPSTrackersT tr').eq(i).find('td').eq(1).text(), false);
    }
}
var dif = {};
var intervals = {};

function ShowHideTracks(id, checked, track_values) {
    clearInterval(days_timer);
    tracker[id].checked = checked;
    var id_tr = tracker[id].id_tracker;
    $('#Distance' + id_tr).val('');
    D[id_tr] = 0;
    realtime_tracking = false;
    track_point_source[id_tr].clear();
    track_point_ln_source[id_tr].clear();
    var date_from = $('#DateTrackPointsFromTB').val().toString();
    var date_to = $('#DateTrackPointsToTB').val().toString();
    realtime_tracking = IsTodayDate(date_to);
    if (checked && $('#DateTrackPointsFromTB').val().length != 0 && $('#DateTrackPointsToTB').val().length != 0 && $('#DateTrackPointsFromTB').val() && $('#DateTrackPointsToTB').val()) {
        SetTrackersActive(0);

        dif[id_tr] = DateDifferenceInDays(date_from, date_to);
        date_to = date_from;
        var is_end = (dif[id_tr] < 1) ? true : false;
        var timeout_value = 10000;
        track_values = id_tr + ";" + tracker[id].checked + "|" + date_to + ';' + date_to + ';' + is_end + ';' + timeout_value;
        SetTrackersActive(0);
        CallServer('SHtracks:' + track_values, 'null');
        intervals[id_tr] = 1;
        // for (var i = 1; i <= dif; ++i) {

        timeout_value = dif[id_tr] < 2 ? 10000 : (dif[id_tr] < 5 ? 15000 : 23000);
        days_timer[id_tr] = setInterval(function () {
            if (intervals[id_tr] <= dif[id_tr]) {
                is_end = (intervals[id_tr] == dif[id_tr]) ? true : false;
                intervals[id_tr]++;
                date_to = AddDaysToDate(date_to, 1);
                track_values = id_tr + ";" + tracker[id].checked + "|" + date_to + ';' + date_to + ';' + is_end + ';' + timeout_value;
                CallServer('SHtracks:' + track_values, 'null');
            }
            else {
                clearInterval(days_timer[id_tr]);
            }
        }, timeout_value);
        //}
    }
    else { track_last_point_source[id_tr].clear(); }
}
function UpdateTracks() {
    var id; var checked; var id_tr;
    for (var i = 1; i < trackers_count; ++i) {
        id = $('#GPSTrackersT tr').eq(i).find('td').eq(5).find(':checkbox').attr('id');
        id_tr = tracker[id].id_tracker;
        checked = tracker[id].checked;
        var track_values = id_tr + ";" + tracker[id].checked + "|" + (track_current_date_rt[id_tr] - 1) + ';' + (track_current_date_rt[id_tr] + 6000000000).toString() + ';0;500;false;0';
        //alert(track_values);
        if (checked) {
            CallServer('drawlastpoint:' + id_tr, 'null');
            if (realtime_tracking) CallServer('showhidetracks:' + track_values, 'null');
        }
        else {
            track_last_point_source.clear();
        }
        //ShowHideTracks(id, checked, track_values);
    }
}
function Refresh() {
    var id; var checked;
    for (var i = 1; i < trackers_count; ++i) {
        id = $('#GPSTrackersT tr').eq(i).find('td').eq(5).find(':checkbox').attr('id');
        checked = tracker[id].checked;
        ShowHideTracks(id, checked, 0);
    }
}
function SetTrackersActive(active_value) {
    if (active_value) {
        $('#GPSTrackersT').find(':checkbox').prop('disabled', false);
        $('#RefreshTrack').prop('disabled', false);
    }
    else {
        $('#GPSTrackersT').find(':checkbox').prop('disabled', true);
        $('#RefreshTrack').prop('disabled', true);
    }
}
function IsTodayDate(comparsion_date) {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    today = dd + '.' + mm + '.' + yyyy;
    return today == comparsion_date;
}
function StopUpdatingTracks() {
    clearInterval(timer_sht);
    realtime_tracking = false;
}
function SetUpdateTimer() {
    timer_sht = setInterval(UpdateTracks, 3000);
}
function IsChecked() {
    for (var i in tracker) {
        if (tracker[i].checked) return true;
    }
    return false;
}
function GenerateTrackersString() {
    var tr_str = "";
    for (var i in tracker) {
        tr_str += tracker[i].id_tracker + ";" + tracker[i].checked + "|";
    }
    tr_str += $('#DateTrackPointsFromTB').val() + ';';
    tr_str += $('#DateTrackPointsToTB').val();
    return tr_str;
}
function DrawTrackLines(layer, id_tracker, start, end) {
    /* track_point_ln_source[id_tracker] = vector_track_lines[id_tracker].getSource();
     var count = layer.getSource().getFeatures().length;
     var portion = end - start;
     var features = new ol.Collection();
     var feature_ln;
     var coords = [];
     features[0] = layer.getSource().getFeatureById(start);
     for (var i = start + 1; i < end; ++i) {
         features[i-start] = layer.getSource().getFeatureById(i);
         var point_prev = features[i - start - 1].getGeometry().getCoordinates();
         var point_curr = features[i - start].getGeometry().getCoordinates();
         coords = [point_prev, point_curr];
         feature_ln = new ol.Feature({ geometry: new ol.geom.LineString(coords) });
         feature_ln.name = 'track_line_' + String(i - 1);
         var layer_name = 'vector_track_lines[' + String(id_tracker) + ']';
         feature_ln.setProperties({ 'layer': layer_name, 'id_feature': i - start - 1, 'id_gps_tracker': features[i - start].getProperties().id_gps_tracker });
         track_point_ln_source[id_tracker].addFeature(feature_ln);
     }
    // var color_point = layer.getStyle().getFill().getColor();
     // var style_ln = new ol.style.Style({ stroke: new ol.style.Stroke({ color: color_point, width: 4 }) });
     //vector_track_lines[id_tracker] = new ol.layer.Vector({ source: track_point_ln_source, maxResolution: 10 });
     //map.addLayer(vector_track_lines[id_tracker]);
     vector_track_lines[id_tracker].setZIndex(3);*/
    CallServer('setlinesstyle:' + id_tracker, 'null');
}
function CountDistance(latitude1, longitude1, latitude2, longitude2) {
    latitude1 *= Math.PI / 180;
    latitude2 *= Math.PI / 180;
    longitude1 *= Math.PI / 180;
    longitude2 *= Math.PI / 180;
    var a = Math.pow(Math.cos(latitude2) * Math.sin(longitude2 - longitude1), 2);
    var b = Math.pow(Math.cos(latitude1) * Math.sin(latitude2) - Math.sin(latitude1) * Math.cos(latitude2) * Math.cos(longitude2 - longitude1), 2);
    var c = Math.sin(latitude1) * Math.sin(latitude2) + Math.cos(latitude1) * Math.cos(latitude2) * Math.cos(longitude2 - longitude1);
    var d = Math.atan2(Math.sqrt(a + b), c);
    var R = 6371200;
    d *= R;
    return d / 1000;
}
/*function CountDistance(latitude1, longitude1, altitude1, latitude2, longitude2, altitude2) {
    latitude1 *= Math.PI / 180;
    latitude2 *= Math.PI / 180;
    longitude1 *= Math.PI / 180;
    longitude2 *= Math.PI / 180;

    var x1 = altitude1 * Math.cos(latitude1) * Math.sin(longitude1);
    var y1 = altitude1 * Math.sin(latitude1);
    var z1 = altitude1 * Math.cos(latitude1) * Math.cos(longitude1);

    var x2 = altitude2 * Math.cos(latitude2) * Math.sin(longitude2);
    var y2 = altitude2 * Math.sin(latitude2);
    var z2 = altitude2 * Math.cos(latitude2) * Math.cos(longitude2);

    var d = Math.sqrt(Math.pow(x2 - x1, 2) + Math.pow(y2 - y1, 2) + Math.pow(z2 - z1, 2));
    return d;

}*/
function AddDaysToDate(date_string, days) {
    var day = date_string.split('.')[0];
    var month = date_string.split('.')[1] - 1;
    var year = date_string.split('.')[2];
    var tmp_Date = new Date(year, month, day);
    tmp_Date.setDate(tmp_Date.getDate() + days);
    var result_day = tmp_Date.getDate();
    if (result_day < 10) result_day = '0' + result_day;
    var result_month = tmp_Date.getMonth() + 1;
    if (result_month < 10) result_month = '0' + result_month;
    var result_year = tmp_Date.getFullYear();
    var result = result_day + "." + result_month + "." + result_year;
    return result;
}
function DateDifferenceInDays(date1, date2) {
    var day1 = date1.split('.')[0];
    var month1 = date1.split('.')[1];
    var year1 = date1.split('.')[2];
    var day2 = date2.split('.')[0];
    var month2 = date2.split('.')[1];
    var year2 = date2.split('.')[2];
    var D1 = new Date(year1, month1, day1);
    var D2 = new Date(year2, month2, day2);
    var result = Math.ceil((D2 - D1) / (1000 * 60 * 60 * 24));
    return result;
}

//работа с отчётами
var type_report_object = 0;
var id_report_object = 0;
var report_src = '';
function ShowSelectTourW(type_object) {
    type_report_object = type_object;
    //отчёт по элементам и типу почв
    switch (type_report_object) {
        //отделение
        case 0: {
            break;
        }
            //организация
        case 1: {
            if ($('#OrganizationCB').val() == null) { $('#OrganizationCB').val('0'); };
            id_report_object = $('#OrganizationCB').val();
            report_src = 'ReportSignificativeBySoilOrganization.aspx';
            break;
        }
            //район
        case 2: {
            if ($('#RegionCB').val() == null) { $('#RegionCB').val('0'); };
            id_report_object = $('#RegionCB').val();
            report_src = 'ReportSignificativeBySoilRegion.aspx';
            break;
        }
            //область
        case 3: {
            break;
        }
            //страна
        case 4: {
            break;
        }
    }
    CallServer('showselecttour:' + type_report_object + '|' + id_report_object, 'null');
    $('#SelectTourW').dialog('open');
}

function ShowSelectUsingWateringTypeFarmlandTourW(type_object) {
    type_report_object = type_object;
    //отчёт по элементам и типу почв
    switch (type_report_object) {
        //отделение
        case 0: {
            break;
        }
            //организация
        case 1: {
            break;
        }
            //район
        case 2: {
            if ($('#RegionCB').val() == null) { $('#RegionCB').val('0'); };
            id_report_object = $('#RegionCB').val();
            report_src = 'ReportUsingWateringTypeFarmlandRegion.aspx';
            break;
        }
            //область
        case 3: {
            break;
        }
            //страна
        case 4: {
            break;
        }
    }
    CallServer('showselect_u_w_f_tour:' + type_report_object + '|' + id_report_object, 'null');
    $('#SelectUsingWateringTypeFarmlandTourW').dialog('open');
}

function ShowSelectYearW(type_object) {
    type_report_object = type_object;
    switch (type_report_object) {
        //культуры
        case 1: {
            if ($('#OrganizationCB').val() == null) { $('#OrganizationCB').val('0'); };
            id_report_object = $('#OrganizationCB').val();
            report_src = 'ReportCulturesOrganization.aspx';
            break;
        }
        case 2: {
            if ($('#RegionCB').val() == null) { $('#RegionCB').val('0'); };
            id_report_object = $('#RegionCB').val();
            report_src = 'ReportCulturesRegion.aspx';
            break;
        }
    }
    CallServer('showselectyear:' + type_report_object + '|' + id_report_object, 'null');
    $('#SelectYearW').dialog('open');
}

function ShowSelectYearCultureW(type_object) {
    type_report_object = type_object;
    switch (type_report_object) {
        //урожайность
        case 1: {
            if ($('#OrganizationCB').val() == null) { $('#OrganizationCB').val('0'); };
            id_report_object = $('#OrganizationCB').val();
            report_src = 'ReportProductivityOrganization.aspx';
            break;
        }
        case 2: {
            if ($('#RegionCB').val() == null) { $('#RegionCB').val('0'); };
            id_report_object = $('#RegionCB').val();
            report_src = 'ReportProductivityRegion.aspx';
            break;
        }
    }
    $('#SelectYear1CB').on("change", function () {
        CallServer('select_culture_for_productivity:' + type_report_object + '|' + id_report_object + '|' + this.value, 'null');
    });
    CallServer('showselectyearculture:' + type_report_object + '|' + id_report_object, 'null');
    $('#SelectYearCultureW').dialog('open');
}

function ShowSignificativeBySoilReport() {
    if ($('#SelectTourCB').val() == null) { $('#SelectTourCB').val('0'); };
    document.cookie = 'Agrochim31_ReportTours=id=' + id_report_object + '&tour=' + $('#SelectTourCB').val();

    var iframe = $('<iframe />', {
        id: 'reportFrame',
        src: report_src,
        frameborder: 0,
        scrolling: 'yes'
    });
    document.getElementById('TheReportW').innerHTML = '';
    iframe.appendTo('#TheReportW');
    $('#TheReportW').LoadingMask('show', { duration: 300 });
    iframe.on("load", function () {
        $('#TheReportW').LoadingMask('hide');
    });
    $('#SelectTourW').dialog('close');
    $('#TheReportW').dialog('open');
}

function ShowErosionChangeReport() {
    if ($('#SelectTourErosionChangeCB').val() == null) { $('#SelectTourErosionChangeCB').val('0'); };
    document.cookie = 'Agrochim31_ReportTours=id=' + id_report_object + '&tour=' + $('#SelectTourErosionChangeCB').val();

    var iframe = $('<iframe />', {
        id: 'reportFrame',
        src: report_src,
        frameborder: 0,
        scrolling: 'yes'
    });
    document.getElementById('TheReportW').innerHTML = '';
    iframe.appendTo('#TheReportW');
    $('#TheReportW').LoadingMask('show', { duration: 300 });
    iframe.on("load", function () {
        $('#TheReportW').LoadingMask('hide');
    });
    $('#SelectTourErosionChangeW').dialog('close');
    $('#TheReportW').dialog('open');
}

function ShowCulturesReport() {
    if ($('#SelectYearCB').val() != null) {
        document.cookie = 'Agrochim31_ReportTours=id=' + id_report_object + '&year=' + $('#SelectYearCB').val();
        var iframe = $('<iframe />', {
            id: 'reportFrame',
            src: report_src,
            frameborder: 0,
            scrolling: 'yes'
        });
        document.getElementById('TheReportW').innerHTML = '';
        iframe.appendTo('#TheReportW');
        $('#TheReportW').LoadingMask('show', { duration: 300 });
        iframe.on("load", function () {
            $('#TheReportW').LoadingMask('hide');
        });

        $('#SelectYearW').dialog('close');

        $('#TheReportW').dialog('open');

        CallServer('cultures:' + id_report_object + '|' + $('#SelectYearCB').val() + '|' + type_report_object, 'null');
    }
}

function ShowProductivityReport() {
    if ($('#SelectYear1CB').val() != null && $('#SelectCultureCB').val() != null) {
        document.cookie = 'Agrochim31_ReportTours=id=' + id_report_object + '&year=' + $('#SelectYear1CB').val() + '&id_culture=' + $('#SelectCultureCB').val();
        var iframe = $('<iframe />', {
            id: 'reportFrame',
            src: report_src,
            frameborder: 0,
            scrolling: 'yes'
        });
        document.getElementById('TheReportW').innerHTML = '';
        iframe.appendTo('#TheReportW');
        $('#TheReportW').LoadingMask('show', { duration: 300 });
        iframe.on("load", function () {
            $('#TheReportW').LoadingMask('hide');
        });

        $('#SelectYearCultureW').dialog('close');

        $('#TheReportW').dialog('open');

        CallServer('productivity:' + id_report_object + '|' + $('#SelectYear1CB').val() + '|' + $('#SelectCultureCB').val() + '|' + type_report_object, 'null');
    }
}

function ShowUsingWateringTypeFarmlandRegionReport() {
    if ($('#SelectTour1CB').val() == null) { $('#SelectTour1CB').val('0'); };
    document.cookie = 'Agrochim31_ReportTours=id=' + id_report_object + '&tour=' + $('#SelectTour1CB').val();

    var iframe = $('<iframe />', {
        id: 'reportFrame',
        src: report_src,
        frameborder: 0,
        scrolling: 'yes'
    });
    document.getElementById('TheReportW').innerHTML = '';
    iframe.appendTo('#TheReportW');
    $('#TheReportW').LoadingMask('show', { duration: 300 });
    iframe.on("load", function () {
        $('#TheReportW').LoadingMask('hide');
    });
    $('#SelectUsingWateringTypeFarmlandTourW').dialog('close');
    $('#TheReportW').dialog('open');
}

function AcceptSelectProtocol()
{
    CallServer('selected_protocol_significatives:' + $("#SelectProtocolCB").val(), 'null');
}

function ShowFertilizerProtocolReport() {
    if ($('#IdProtocolTB').val() != "" && $('#IdProtocolTB').val() != null) {
        document.cookie = 'Agrochim31_Report_Protocol=id=' + $('#IdProtocolTB').val();
        var iframe = $('<iframe />', {
            id: 'reportFrame',
            src: 'ReportFertilizerProtocol.aspx',
            frameborder: 0,
            scrolling: 'yes'
        });
        document.getElementById('TheReportW').innerHTML = '';
        iframe.appendTo('#TheReportW');
        $('#TheReportW').LoadingMask('show', { duration: 300 });
        iframe.on("load", function () {
            $('#TheReportW').LoadingMask('hide');
        });
        $('#TheReportW').dialog('open');
    }
}

function ShowFertilizerProtocolReportById(id_protocol) {
    if (id_protocol != "" && id_protocol != null) {
        document.cookie = 'Agrochim31_Report_Protocol=id=' + id_protocol;
        var iframe = $('<iframe />', {
            id: 'reportFrame',
            src: 'ReportFertilizerProtocol.aspx',
            frameborder: 0,
            scrolling: 'yes'
        });
        document.getElementById('TheReportW').innerHTML = '';
        iframe.appendTo('#TheReportW');
        $('#TheReportW').LoadingMask('show', { duration: 300 });
        iframe.on("load", function () {
            $('#TheReportW').LoadingMask('hide');
        });
        $('#TheReportW').dialog('open');
    }
}

function DeleteFertilizerProtocol() {
    if ($('#IdProtocolTB').val() != "" && $('#IdProtocolTB').val() != null) {
        $('#IdProtocolTB').val('');
        $('#NumberProtocolTB').val('');

        $('#NContentTB').val('');
        $('#PContentTB').val('');
        $('#KContentTB').val('');

        $('#OrganicTotalDoseNTB').val('');
        $('#OrganicTotalDosePTB').val('');
        $('#OrganicTotalDoseKTB').val('');

        j_script += "\n$(\"#OrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});";
        j_script += "\n$(\"#OrganicFertCB option[value='0']\").prop('selected', true);";
    }
}

function ShowSelectYearOrganicFertilizerW(type_object) {
    type_report_object = type_object;
    switch (type_report_object) {
        //органические удобрения
        case 1: {
            if ($('#OrganizationCB').val() == null) { $('#OrganizationCB').val('0'); };
            id_report_object = $('#OrganizationCB').val();
            report_src = 'ReportFertilizerFromOrganization.aspx';
            break;
        }
    }
    CallServer('showselectyear_organic_fert:' + type_report_object + '|' + id_report_object, 'null');
    $('#SelectYearOrganicFertilizerW').dialog('open');
}

function ShowOrganicFertilizerReport()
{
    if ($('#SelectYear2CB').val() != null) {
        document.cookie = 'Agrochim31_ReportTours=id=' + id_report_object + '&year=' + $('#SelectYear2CB').val();
        var iframe = $('<iframe />', {
            id: 'reportFrame',
            src: report_src,
            frameborder: 0,
            scrolling: 'yes'
        });
        document.getElementById('TheReportW').innerHTML = '';
        iframe.appendTo('#TheReportW');
        $('#TheReportW').LoadingMask('show', { duration: 300 });
        iframe.on("load", function () {
            $('#TheReportW').LoadingMask('hide');
        });

        $('#SelectYearOrganicFertilizerW').dialog('close');

        $('#TheReportW').dialog('open');
    }
}

function ShowSelectYearPestsDiseasesWeedinessW(type_object, type) {
    type_report_object = type_object;
    switch (type_object) {
        case 1: {
            if ($('#OrganizationCB').val() == null) { $('#OrganizationCB').val('0'); };
            id_report_object = $('#OrganizationCB').val();
            switch (type) {
                case 0: {
                    report_src = 'ReportPests.aspx';
                    break;
                }
                case 1: {
                    report_src = 'ReportDiseases.aspx';
                    break;
                }
                case 2: {
                    report_src = 'ReportWeediness.aspx';
                    break;
                }
            }
            break;
        }
    }
    CallServer('showselectyear_pests_diseases_weediness:' + type_object + '|' + id_report_object, 'null');
    $('#SelectYearPestsDiseasesWeedinessW').dialog('open');
}

function ShowPestsDiseasesWeedinessReport() {
    if ($('#SelectYear3CB').val() != null) {
        document.cookie = 'Agrochim31_Report=id=' + id_report_object + '&year=' + $('#SelectYear3CB').val();
        var iframe = $('<iframe />', {
            id: 'reportFrame',
            src: report_src,
            frameborder: 0,
            scrolling: 'yes'
        });
        document.getElementById('TheReportW').innerHTML = '';
        iframe.appendTo('#TheReportW');
        $('#TheReportW').LoadingMask('show', { duration: 300 });
        iframe.on("load", function () {
            $('#TheReportW').LoadingMask('hide');
        });

        $('#SelectYearPestsDiseasesWeedinessW').dialog('close');

        $('#TheReportW').dialog('open');

        if (report_src == 'ReportWeediness.aspx') {
            CallServer('weediness:' + id_report_object + '|' + $('#SelectYear3CB').val() + '|' + type_report_object, 'null');
        }
    }
}

function ShowSelectTourErosionChangeW(type_object) {
    type_report_object = type_object;
    //отчёт по изменению эрозии почвы
    switch (type_report_object) {
        //отделение
        case 0: {
            break;
        }
            //организация
        case 1: {
            if ($('#OrganizationCB').val() == null) { $('#OrganizationCB').val('0'); };
            id_report_object = $('#OrganizationCB').val();
            report_src = 'ReportErosionChangeOrganization.aspx';
            break;
        }
            //район
        case 2: {

            break;
        }
            //область
        case 3: {
            break;
        }
            //страна
        case 4: {
            break;
        }
    }
    CallServer('showselecttourerosionchange:' + type_report_object + '|' + id_report_object, 'null');
    $('#SelectTourErosionChangeW').dialog('open');
}

function CreateFilter()
{
    var filter = "";
    if ($('#OrganizationCB').val() == null || $('#OrganizationCB').val() == "") {
        if ($('#RegionCB').val() == null || $('#RegionCB').val() == "") {
            if ($('#TerritoryCB').val() != null && $('#TerritoryCB').val() != "") {
                filter += ("id_territory=" + $('#TerritoryCB').val());
            }
        }
        else {
            filter += ("id_region=" + $('#RegionCB').val());
        }
    }
    else {
        filter += ("id_organization=" + $('#OrganizationCB').val());
    }

    if ($('#SurveyTourCB').val() != null && $('#SurveyTourCB').val() != "" && $('#SurveyTourCB').val() != "NULL") {
        if (filter != "") {
            filter += " AND ";
        }
        filter += ("tour=" + $('#SurveyTourCB').val());
    }
    else {
        if ($('#SurveyYearCB').val() != null && $('#SurveyYearCB').val() != "" && $('#SurveyYearCB').val() != "NULL") {
            if (filter != "") {
                filter += " AND ";
            }
            filter += ("year=" + $('#SurveyYearCB').val());
        }
    }

    if ($('#TypePlotCB').val() != null && $('#TypePlotCB').val() != "" && $('#TypePlotCB').val() != "NULL" && $('#TypePlotCB').val() != 0) {
        if (filter != "") {
            filter += " AND ";
        }
        if ($('#TypePlotCB').val() == 1) {
            filter += (" id_farmland >= 1");
            filter += (" AND id_farmland < 4");
        }
        else if ($('#TypePlotCB').val() == 2) {
            filter += (" id_farmland > 7");
            filter += (" AND id_farmland < 12");
        }
    }

    return filter;
}

function RegionStatisticsReport()
{
    if ($('#RegionCB').val() != null && $('#RegionCB').val() != "" && $('#RegionCB').val() != "0" && $('#RegionCB').val() != 0) {
        document.cookie = 'Agrochim31_Report=id_region=' + $('#RegionCB').val();
        var iframe = $('<iframe />', {
            id: 'reportFrame',
            src: 'ReportMacroStatisticsForRegion.aspx',
            frameborder: 0,
            scrolling: 'yes'
        });
        document.getElementById('TheReportW').innerHTML = '';
        iframe.appendTo('#TheReportW');
        $('#TheReportW').LoadingMask('show', { duration: 300 });
        iframe.on("load", function () {
            $('#TheReportW').LoadingMask('hide');
        });

        $('#TheReportW').dialog('open');
    }
    else {
        alert("Не выбран район!");
    }
}

function GetSelectedPlots()
{
    var plots_str = "";
    if (selectClick != null) {
        var selectedFeatures = selectClick.getFeatures();
        if (selectedFeatures.getLength() > 0) {
            selectedFeatures.forEach(function (feature) {
                if (feature.getProperties().layer == 'plots' && (feature.getGeometry().getType() == 'Polygon' || feature.getGeometry().getType() == 'MultiPolygon')) {
                    if (plots_str != "")
                    {
                        plots_str += ",";
                    }
                    plots_str += String(feature.getProperties().id_plot);
                }
            });
        }
    }
    return plots_str;
}

function ErosionChangeSelectedPlots() {
    if (GetSelectedPlots() != "") {
        document.cookie = 'Agrochim31_SelectedPlots=plots_str=' + GetSelectedPlots();
        var iframe = $('<iframe />', {
            id: 'reportFrame',
            src: 'ReportErosionChangeSelectedPlots.aspx',
            frameborder: 0,
            scrolling: 'yes'
        });
        document.getElementById('TheReportW').innerHTML = '';
        iframe.appendTo('#TheReportW');
        $('#TheReportW').LoadingMask('show', { duration: 300 });
        iframe.on("load", function () {
            $('#TheReportW').LoadingMask('hide');
        });

        $('#TheReportW').dialog('open');
    }
    else {
        alert("Нет выбранных участков!");
    }
}

function MacroStatisticsForSelectedPlots() {
    if (GetSelectedPlots() != "") {
        document.cookie = 'Agrochim31_SelectedPlots=plots_str=' + GetSelectedPlots();
        var iframe = $('<iframe />', {
            id: 'reportFrame',
            src: 'ReportMacroStatisticsForSelectedPlots.aspx',
            frameborder: 0,
            scrolling: 'yes'
        });
        document.getElementById('TheReportW').innerHTML = '';
        iframe.appendTo('#TheReportW');
        $('#TheReportW').LoadingMask('show', { duration: 300 });
        iframe.on("load", function () {
            $('#TheReportW').LoadingMask('hide');
        });

        $('#TheReportW').dialog('open');
    }
    else {
        alert("Нет выбранных участков!");
    }
}

function OrgaizationStatisticsReport() {
    if ($('#OrganizationCB').val() != null && $('#OrganizationCB').val() != "" && $('#OrganizationCB').val() != "0" && $('#OrganizationCB').val() != 0) {
        document.cookie = 'Agrochim31_Report=id_organization=' + $('#OrganizationCB').val();
        var iframe = $('<iframe />', {
            id: 'reportFrame',
            src: 'ReportMacroStatisticsForOrganization.aspx',
            frameborder: 0,
            scrolling: 'yes'
        });
        document.getElementById('TheReportW').innerHTML = '';
        iframe.appendTo('#TheReportW');
        $('#TheReportW').LoadingMask('show', { duration: 300 });
        iframe.on("load", function () {
            $('#TheReportW').LoadingMask('hide');
        });

        $('#TheReportW').dialog('open');
    }
    else {
        alert("Не выбрана организация!");
    }
}

function ShowReportOrganicFertilizerSurplus(id_protocol) {
    if (id_protocol != "" && id_protocol != null) {
        document.cookie = 'Agrochim31_Report=id_protocol=' + id_protocol;
        var iframe = $('<iframe />', {
            id: 'reportFrame',
            src: 'ReportOrganicFertilizerSurplus.aspx',
            frameborder: 0,
            scrolling: 'yes'
        });
        document.getElementById('TheReportW').innerHTML = '';
        iframe.appendTo('#TheReportW');
        $('#TheReportW').LoadingMask('show', { duration: 300 });
        iframe.on("load", function () {
            $('#TheReportW').LoadingMask('hide');
        });
        $('#TheReportW').dialog('open');

        CallServer('show_organic_fertilizer:' + id_protocol, 'null');
    }
}

function ShowPlantations() {
    $("#PlantationsW").html("<h1>Загрузка данных, пожалуйста подождите!</h1>");

    var filter = "";
    if ($('#OrganizationCB').val() == null || $('#OrganizationCB').val() == "" || $('#OrganizationCB').val() == 0) {
        if ($('#RegionCB').val() == null || $('#RegionCB').val() == "" || $('#RegionCB').val() == 0) {
            if ($('#TerritoryCB').val() != null && $('#TerritoryCB').val() != "" && $('#TerritoryCB').val() != 0) {
                filter += ("id_territory=" + $('#TerritoryCB').val());
            }
        }
        else {
            filter += ("id_region=" + $('#RegionCB').val());
        }
    }
    else {
        filter += ("id_organization=" + $('#OrganizationCB').val());
    }

    CallServer('show_plantations:' + filter, 'null');
    $("#PlantationsW").dialog('open');
}

function ShowPlantation(id_territory, id_region, id_organization, last_year) {
    var filter = "";
    filter += ("id_organization=" + id_organization);
    filter += (" AND [year]=" + last_year);
    filter += (" AND id_farmland > 7");
    filter += (" AND id_farmland < 12");
    var values = get_cookie('Agrochim31_Map_User').split('&')[0].split('=')[1] + '|' + get_cookie('Agrochim31_Map_User').split('&')[4].split('=')[1];

    /*$("#TerritoryCB option:selected").each(function(){$(this).prop('selected', false);});
    $("#TerritoryCB option[value='" + id_territory + "']").prop('selected', true);
    CallServer('select_territory:' + id_territory + '|' + values, 'null');

    $("#RegionCB option:selected").each(function(){$(this).prop('selected', false);});
    $("#RegionCB option[value='" + id_region + "']").prop('selected', true);
    CallServer('select_region:' + id_region + '|' + values, 'null');

    $("#OrganizationCB option:selected").each(function(){$(this).prop('selected', false);});
    $("#OrganizationCB option[value='" + id_organization + "']").prop('selected', true);
    CallServer('select_organization:' + id_organization + '|' + values, 'null');*/
    
    $("#TypePlotCB option:selected").each(function () { $(this).prop('selected', false); });
    $("#TypePlotCB option[value='2']").prop('selected', true);

    if (filter == "") {
        filter = "0";
    }

    CallServer('get_plots_by_filter:' + filter + '|' + id_organization + '|' + values, 'null');

    CallServer('load_region_organization:' + id_territory + '|' + id_region + '|' + id_organization + '|' + values, 'null');

    //$("#PlantationsW").dialog('close');
}

function ShowReportCultureHybridByPlantations() {
    var filter = "";
    if ($('#OrganizationCB').val() == null || $('#OrganizationCB').val() == "" || $('#OrganizationCB').val() == 0) {
        if ($('#RegionCB').val() == null || $('#RegionCB').val() == "" || $('#RegionCB').val() == 0) {
            if ($('#TerritoryCB').val() != null && $('#TerritoryCB').val() != "" && $('#TerritoryCB').val() != 0) {
                filter += ("id_territory|" + $('#TerritoryCB').val());
            }
        }
        else {
            filter += ("id_region|" + $('#RegionCB').val());
        }
    }
    else {
        filter += ("id_organization|" + $('#OrganizationCB').val());
    }

    if ($('#SurveyTourCB').val() != null && $('#SurveyTourCB').val() != "" && $('#SurveyTourCB').val() != "NULL") {
        if (filter != "") {
            filter += " AND ";
        }
        filter += ("tour|" + $('#SurveyTourCB').val());
    }
    else {
        if ($('#SurveyYearCB').val() != null && $('#SurveyYearCB').val() != "" && $('#SurveyYearCB').val() != "NULL") {
            if (filter != "") {
                filter += " AND ";
            }
            filter += ("year|" + $('#SurveyYearCB').val());
        }
    }

    if (filter == "") {
        filter = "0";
    }

    document.cookie = 'Agrochim31_Report=filter=' + filter;
    var iframe = $('<iframe />', {
        id: 'reportFrame',
        src: 'ReportCultureHybridByPlantations.aspx',
        frameborder: 0,
        scrolling: 'yes'
    });
    document.getElementById('TheReportW').innerHTML = '';
    iframe.appendTo('#TheReportW');
    $('#TheReportW').LoadingMask('show', { duration: 300 });
    iframe.on("load", function () {
        $('#TheReportW').LoadingMask('hide');
    });
    $('#TheReportW').dialog('open');
}