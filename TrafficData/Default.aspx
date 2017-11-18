<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TrafficData.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous"/>
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?key=AIzaSyBPQXaLdLvIUexUpzdzDgNcvwo2wc91jkk&sensor=true&libraries=visualization"></script>
    <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.2.1.min.js"></script>
    <script src="js/search.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <button type="button" onclick="javascript:querya()">click</button>
        <input type="search" name="query" id="search" placeholder="search" />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <div id="map" style="width:500px;height:500px;"></div>
    </div>
    </form>
</body>
</html>
