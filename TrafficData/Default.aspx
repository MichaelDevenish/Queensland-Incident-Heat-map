<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TrafficData.Home" %>

    <!DOCTYPE html>

    <html xmlns="http://www.w3.org/1999/xhtml">

    <head runat="server">
        <title></title>
        <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.2.1.min.js"></script>
        <script type="text/javascript" src="http://maps.google.com/maps/api/js?key=AIzaSyBPQXaLdLvIUexUpzdzDgNcvwo2wc91jkk&libraries=visualization"></script>
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
        <link rel="stylesheet" href="css/StyleSheet.css" />
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
        <script src="js/search.js"></script>
        <script>
            $(document).ready(function() {
                drawMap();
            });
        </script>
    </head>

    <body>
        <div id="map"></div>
        <nav class="navbar navbar-default" role="navigation">
            <div class="container">
                <div class="navbar-header">
                    <a class="navbar-brand" href="#">Accident Hotspots</a>
                </div>
                <form class="navbar-form" onsubmit="return false;">
                    <div class="form-group" style="display:inline;">
                        <div class="input-group parentInput">
                            <select class="selectpicker form-control" id="BottomYear" onchange="minSelected()">
                                <option selected="selected">2001</option>
                                <option>2002</option>
                                <option>2003</option>
                                <option>2004</option>
                                <option>2005</option>
                                <option>2006</option>
                                <option>2007</option>
                                <option>2008</option>
                                <option>2009</option>
                                <option>2010</option>
                                <option>2011</option>
                                <option>2012</option>
                                <option>2013</option>
                                <option>2014</option>
                                <option>2015</option>
                                <option>2016</option>
                            </select>
                            <select class="selectpicker form-control" id="topYear" onchange="maxSelected()">
                                <option>2002</option>
                                <option>2003</option>
                                <option>2004</option>
                                <option>2005</option>
                                <option>2006</option>
                                <option>2007</option>
                                <option>2008</option>
                                <option>2009</option>
                                <option>2010</option>
                                <option>2011</option>
                                <option>2012</option>
                                <option>2013</option>
                                <option>2014</option>
                                <option>2015</option>
                                <option>2016</option>
                                <option selected="selected">2017</option>
                            </select>
                            <input type="text" class="form-control" id="search" placeholder="Postcode" onkeydown="if (event.keyCode == 13) queryDatabase()"/>
                            <div class="input-group-btn">
                                <button class="btn btn-default" type="button"  onclick="queryDatabase()" ><i class="glyphicon glyphicon-search"></i></button>
                            </div>
                        </div>
                    </div>
                </form>
                <p class="error text-center" id ="searchError">Must enter a valid Postcode</p>
            </div>
        </nav>

    </body>

    </html>