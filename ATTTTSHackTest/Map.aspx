<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Map.aspx.cs" Inherits="ATTTTSHackTest.Map" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no"/>
 <link rel="stylesheet" href="http://js.arcgis.com/3.10/js/dojo/dijit/themes/claro/claro.css">    
  <link rel="stylesheet" href="http://js.arcgis.com/3.10/js/esri/css/esri.css">
  <style>
    html, body, #map{
      padding: 0;
      margin: 0;
      height: 100%;
    }
  </style>
  <script src="http://js.arcgis.com/3.10/"></script>
  <script>
      var map;

      require([
        "esri/map", "esri/layers/ArcGISDynamicMapServiceLayer", "esri/geometry/Extent", "esri/dijit/Scalebar",
        "esri/dijit/BasemapGallery", "esri/arcgis/utils", "dojo/parser", "dijit/layout/BorderContainer",
        "dijit/layout/ContentPane", "dijit/TitlePane", "esri/layers/LayerInfo", "esri/layers/FeatureLayer",
        "dojox/widget/Standby", "dojo/on", "esri/dijit/Legend", "dojo/_base/array", "esri/layers/ArcGISImageServiceLayer",
        "esri/layers/ImageServiceParameters", "esri/layers/GraphicsLayer", "esri/symbols/SimpleMarkerSymbol", "esri/graphic", "esri/geometry/Point", "esri/SpatialReference",
        "esri/toolbars/draw", "dijit/Dialog", "dojo/domReady!"
      ], function (Map, ArcGISDynamicMapServiceLayer, Extent, Scalebar, BasemapGallery, arcgisUtils,
          parser, BorderContainer, ContentPane, TitlePane, LayerInfo, FeatureLayer, Standby, dojoOn, Legend, arrayUtils, ArcGISImageServiceLayer,
          ImageServiceParameters, GraphicsLayer, SimpleMarkerSymbol, Graphic, Point, SpatialReference) {
          //parser.parse();

          map = new Map("map", {
              basemap: "topo",
              extent: new Extent(-115.27200000, 35.95, -115.002263, 36.404886)
          });

          map.on("load", function () {
              var pointGeometry = new esri.geometry.Point(<%= currentLocationLon %>, <%= currentLocationLat %>);
              var graphic = new esri.Graphic(pointGeometry, pictureMarkerSymbol2);
              map.graphics.add(graphic);

              var pointGeometry2 = new esri.geometry.Point(<%= meetingLocationLon %>, <%= meetingLocationLat %>);
              var graphic2 = new esri.Graphic(pointGeometry2, pictureMarkerSymbol);
              map.graphics.add(graphic2);

              //var pointGeometry3 = new esri.geometry.Point(-123.366, 48.461);
              //var graphic3 = new esri.Graphic(pointGeometry3, pictureMarkerSymbol);
              //map.graphics.add(graphic3);

              //var pointGeometry4 = new esri.geometry.Point(-123.482, 48.451);
              //var graphic4 = new esri.Graphic(pointGeometry4, pictureMarkerSymbol);
              //map.graphics.add(graphic4);

              //map.on("mouse-move", showCoordinates);
              //map.on("mouse-drag", showCoordinates);
          });
          setTimeout(function () {
              //document.getElementById("outerContainer").style.width = "100%";
              //document.getElementById("outerContainer").style.height = "100%";
              map.resize();
              map.reposition();
          }, 1000);
      }
      );

      var sfs;
      require([
        "esri/symbols/SimpleFillSymbol", "esri/symbols/SimpleLineSymbol", "esri/Color"], function (SimpleFillSymbol, SimpleLineSymbol, Color) {
            sfs = new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID,
              new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID,
              new Color([255, 0, 0]), 2), new Color([255, 255, 0, 0.25])
            );
        });
      var sms;
      require(["esri/symbols/SimpleMarkerSymbol", "esri/symbols/SimpleLineSymbol", "esri/Color"], function (SimpleMarkerSymbol, SimpleLineSymbol, Color) {
          sms = new SimpleMarkerSymbol(SimpleMarkerSymbol.STYLE_CIRCLE, 6,
            new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID,
            new Color([255, 0, 0]), 1),
            new Color([255, 0, 0, 1]));
      });
      var sls;
      require(["esri/symbols/SimpleLineSymbol", "esri/Color"], function (SimpleLineSymbol, Color) {
          sls = new SimpleLineSymbol(SimpleLineSymbol.STYLE_DASH, new Color([255, 0, 0]), 6);
      });

      var pictureMarkerSymbol;
      require([
        "esri/symbols/PictureMarkerSymbol"
      ], function (PictureMarkerSymbol) {
          pictureMarkerSymbol = new PictureMarkerSymbol('house3.png', 32, 32);
      });

      var pictureMarkerSymbol2;
      require([
        "esri/symbols/PictureMarkerSymbol"
      ], function (PictureMarkerSymbol) {
          pictureMarkerSymbol2 = new PictureMarkerSymbol('car3.png', 32, 32);
      });

  </script>

</head>
<body  class="claro" style="background-color:#E9E9E9">
    <form id="form1" runat="server">
    <div style="position:absolute;left:0px;top:0px;">
        <img src="logo.png" style="width:100%;" />
    </div>
    <div id="map" style="position:absolute;left:10px;top:100px;width:95%;height:300px;border-width:2px;border-style:solid;border-color:#666666;">
        <span style="position:absolute;top:10px; z-Index:999;width:100%;text-align:center;font-size:20px;font-family:Arial;font-weight:bold;"></span>
    </div>
    <div style="position:absolute;left:10px;top:430px;font-family:Arial;">
        Based on Bob's position. They will arrive in <%= duration %> minutes.<br /><br />
        Send Bob an audio message.<br /><br />
        <asp:TextBox runat="server" ID="txtMessageToSend" Rows="5" style="width:95%" TextMode="MultiLine" />
        <br /><br />
        <asp:Button runat="server" Text="Send" id="butSubmit" OnClick="butSubmit_Click" />
    </div>    
    </form>
</body>
</html>
