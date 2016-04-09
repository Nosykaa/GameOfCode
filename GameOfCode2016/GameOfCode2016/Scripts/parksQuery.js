// JavaScript source code
function getPark() {    
    var sliderVal = document.getElementById("ex6SliderVal").value;
    console.log(sliderVal);
    var address = document.getElementById("name").value;
    console.log(address);
    var resDiv = document.getElementById("resPark");
    var expectedHour = sliderVal.substring(0, sliderVal.length-2);
    var html = '<h2 align="center">Results</h2>';
    var contentHtml = "";
    $.post("/Home/GetParks", { address : address, expectedHour : expectedHour }, function (parkListJSON) {
        console.log(JSON.stringify(parkListJSON));
        for(var i=0; i<parkListJSON.length; i++){
            var park = parkListJSON[i];
            console.log(JSON.stringify(park));
            var divTop  = ' <div class="col-lg-4" align="center"><div class="panel panel-default"><div class="panel-heading"><h3 class="panel-title" align="center">' + park.Name +'</h3>';
            var divEnd = '</div><div class="panel-body"><i class="fa fa-male"></i>' + park.DistanceParkToMyAdress + ' &nbsp; <i class="fa fa-car"></i>' + park.FreeSlots + ' &nbsp; <i class="fa fa-credit-card"></i> ' + park.Cost + '</div></div></div>';
            contentHtml = contentHtml + divTop + divEnd;
        }
        
        resDiv.innerHTML = html + contentHtml;
    }, "json")
    .fail(function () {
      alert("error");
    });
    init();
}

function init() {
    //Create a new GeoAPI object, with an options object setting the language to french

    var mapElt = document.getElementById("map");
    mapElt.innerHTML = "";
    mapElt.style.height = "400px";
    mapElt.style.width = "100%";
    geo = new geoadmin.API({ lang: 'en' });

    lux = new OpenLayers.Projection("EPSG:2169"); // the luxembourg coordinate reference system 
    wgs = new OpenLayers.Projection("EPSG:4326"); // the WGS84 coordinate reference system, as used widely
    lonLat = new OpenLayers.LonLat(6.12459923192738, 49.6188206257115).transform(wgs, lux);

        geo.createMap({
            div: 'map',
            easting: lonLat.lon,
            northing: lonLat.lat,
            zoom: 5,
            bgLayer: 'pixelmaps-gray'
        });
    // Define context function for stations
    var context_stations = {
        getImagePath: function (feature) {
            var path = "/Content/images/Park.png";

            var r = feature.attributes["available_bikes"] / feature.attributes["bike_stands"] * 100;
            var a = feature.attributes["available_bikes"];
            var b = feature.attributes["bike_stands"];
            var name = feature.attributes["address"];
            //console.log(a + " / " + b + " * 100 = " + r + "(" + name + ")");
            if (r < 0.1) { path = "./Content/images/Park.png"; }
            else if ((r > 0) && (r <= 10)) { path = "./Content/images/Park.png"; }
            else if ((r > 10) && (r <= 20)) { path = "./Content/images/Park.png"; }
            else if ((r > 20) && (r <= 30)) { path = "./Content/images/Park.png"; }
            else if ((r > 30) && (r <= 40)) { path = "./Content/images/Park.png"; }
            else if ((r > 40) && (r <= 50)) { path = "./Content/images/Park.png"; }
            else if ((r > 50) && (r <= 60)) { path = "./Content/images/Park.png"; }
            else if ((r > 60) && (r <= 70)) { path = "./Content/images/Park.png"; }
            else if ((r > 70) && (r <= 80)) { path = "./Content/images/Park.png"; }
            else if ((r > 80) && (r <= 90)) { path = "./Content/images/Park.png"; }
            else if ((r > 90) && (r <= 100)) { path = "./Content/images/Park.png"; }
            else if ((r > 100)) { path = "./Content/images/Park.png"; }
            //console.log(path)
            return path;
        }

    };
    // Define symbol template for stations
    var template_stations = {
        externalGraphic: "${getImagePath}",
        graphicWidth: 40,
        graphicHeight: 55,
        graphicOpacity: 1,
        graphicXOffset: -20,
        graphicYOffset: -50,
        graphicZIndex: 0
    };

    // Define stations style with template and context combination
    var styleStations = new OpenLayers.Style(template_stations, { context: context_stations });

    var styleSelect_selected = new OpenLayers.Style({
        externalGraphic: "./Content/images/Park.png",
        graphicWidth: 80,
        graphicHeight: 110,
        graphicOpacity: 0.8,
        graphicXOffset: -42,
        graphicYOffset: -100,
        graphicZIndex: 100,
        label: "${available_bikes}" + "/" + "${bike_stands}",
        labelXOffset: 0,
        labelYOffset: 55,
        fontFamily: "arial",
        fontWeight: "bold",
        fontColor: "white",
        fontSize: 25,
        fillColor: "#3399FF",
        strokeWidth: 0, 'fillOpacity': 0.8
    });

    var styleMap_stations = new OpenLayers.StyleMap({
        "default": styleStations,
        //"select": styleSelect_selected
    });

    var renderer = OpenLayers.Util.getParameters(window.location.href).renderer;
    renderer = (renderer) ? [renderer] : OpenLayers.Layer.Vector.prototype.renderers;
    var myStrategy = new OpenLayers.Strategy.Refresh({ interval: 600000, force: true });
    var renderer = OpenLayers.Util.getParameters(window.location.href).renderer;
    renderer = (renderer) ? [renderer] : OpenLayers.Layer.Vector.prototype.renderers;
    var myStrategy = new OpenLayers.Strategy.Refresh({ interval: 60000, force: true });
    //cnew OpenLayers.Layer.Vector({
    //    title: 'added Layer',
    //    source: new OpenLayers.Source.GeoJSON({
    //        projection : 'EPSG:3857',
    //        url: "/Home/GetPOIFromAddress?address=Gare"
    //    })
    //});

    var veloh_stations = new OpenLayers.Layer.Vector("Bikes", {
        projection: "EPSG:4326",
        // Define when the features should be loaded
        strategies: [new OpenLayers.Strategy.Fixed(), myStrategy],
        //Launches the python script to transfrom JCDecaux to useful geoJSON
        protocol: new OpenLayers.Protocol.HTTP({
            url: "/Home/GetPOIFromAddress?address=" + document.getElementById("name").value,
            //url: "http://demo.geoportail.lu/veloh",
            format: new OpenLayers.Format.GeoJSON(),
            //callback: 'veloh',
            //callbackKey: 'cb'
        }),
        //Define the style that should be used to visualize the data
        styleMap: styleMap_stations,
        rendererOptions: { zIndexing: true },
        renderers: renderer
    });

	
    geo.map.addLayer(veloh_stations);


    function createUpdateTextLable(minutes) {
        // Check time since last update and generate a text label
        var label = '';
        if (minutes < 60) {
            if (minutes < 1) { label = "newly updated" }
            else if (minutes < 2) { label = "a minute ago" }
            else if (minutes >= 2) { label = Math.round(minutes) + " minutes ago" }
        } else {
            var hours = minutes / 60;
            if (hours == 1) { label = "an hours ago" }
            else if (hours < 24) { label = Math.floor(hours) + " h " + Math.round(minutes) % 60 + " min ago" }
            else { label = Math.round(hours / 24) + " days ago" }
        }
        return label;
    };

    function createPopup(feature) {
        //Compute time since last update
        var now = new Date();
        var last_update = feature.attributes.last_update;
        var min_since_last_update = (now - last_update) / 60000;
        update_label = createUpdateTextLable(min_since_last_update);

        popup = new GeoExt.Popup({
            title: 'Station vel\'oh',
            location: feature,
            //width:200,
            html: "<div style='font-family:arial'><b>Station:</b>   " + feature.attributes.address + "&nbsp;&nbsp;&nbsp;" +
                "<br><br><b>Status:</b> " + feature.attributes.status +
                "<br><b>Available bikes:</b> " + feature.attributes.available_bikes +
                "<br><b>Free stands:</b> " + feature.attributes.available_bike_stands +
                "<br><br><b>Last update:</b> " + update_label +

            "</div>",
            maximizable: false,
            collapsible: false,
            panIn: true,
            popupCls: "MyPopup"


        });
        // unselect feature when the popup
        // is closed
        popup.on({
            close: function () {
                selectCtrl.unselectAll();
                console.log("Popup on close");
            }
        });
        popup.show();
    }

    // Add a feature select controller to the bike layer
    //var selectCtrl = new OpenLayers.Control.SelectFeature(veloh_select);

    var highlightCtrl = new OpenLayers.Control.SelectFeature(veloh_stations, {
        hover: true,
        highlightOnly: true,
        selectType: styleSelect_selected
    });

    var selectCtrl = new OpenLayers.Control.SelectFeature(veloh_stations,
        { clickout: true, onSelect: createPopup }
    );

    //geo.map.addControl(highlightCtrl);
    //geo.map.addControl(selectCtrl);

    //highlightCtrl.activate();
    //selectCtrl.activate();


};