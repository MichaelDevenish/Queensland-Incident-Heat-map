<%@ Page Title="Crash Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TrafficData.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">  
     <script src="js/search.js"></script>
    <script>
        $(document).ready(function() {
            drawMap();
        });
    </script>
</asp:Content>  

<asp:Content ID="Content2" ContentPlaceHolderID="Background" runat="server" >  
    <div id="map"></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Navbar" runat="server" >  
    <div class="form-group" style="display:inline;">
        <div class="input-group parentInput">
            <asp:DropdownList ID="BottomYear" class="selectpicker form-control" onchange="minSelected()" runat="server"></asp:DropdownList>
            <asp:DropdownList ID="topYear" class="selectpicker form-control" onchange="maxSelected()" runat="server"></asp:DropdownList> 
            <input type="text" class="form-control" id="search" placeholder="Postcode" onkeydown="if (event.keyCode == 13) queryDatabase()"/>
            <div class="input-group-btn">
                <button class="btn btn-default" type="button"  onclick="queryDatabase()" ><i class="glyphicon glyphicon-search"></i></button>
            </div>
        </div>
    </div>
    <p class="error text-center" id ="searchError">Must enter a valid Postcode</p>
</asp:Content>  
