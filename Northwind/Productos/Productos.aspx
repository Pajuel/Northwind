<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="Northwind.Productos.Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <div class="page-header text-info">
                <i class="glyphicon glyphicon-search green"></i>
                <h3>Seleccione uno o varios criterios de búsqueda</h3>
            </div>
            <div class="row">
                <div class="form-horizontal" role="form">
                    <div class="form-group">
                        <asp:Label ID="lblName" runat="server" CssClass="col-sm-3 control-label no-padding-right" AssociatedControlID="txtName">Nombre</asp:Label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblPrice" runat="server" CssClass="col-sm-3 control-label no-padding-right" AssociatedControlID="txtPrice">Precio Unitario</asp:Label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-actions align-center">
                    <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn btn-default" Text="<i class='glyphicon glyphicon-erase'></i> Limpiar" CausesValidation="False" formnovalidate="" OnClick="btnLimpiar_Click" />
                    <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-primary" Text="<i class='glyphicon glyphicon-search'></i> Buscar" CausesValidation="False" formnovalidate="" OnClick="btnBuscar_Click"  OnClientClick="ToggleLoading();"/>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <div class="table-responsive">
                        <asp:GridView ID="grdProducts" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" DataKeyNames="ProductID"
                            PageSize="10" EmptyDataText="No se encontraron registros" OnPreRender="grdProducts_PreRender">
                            <Columns>
                                <asp:BoundField DataField="ProductName" HeaderText="Nombre" />
                                <asp:BoundField DataField="Supplier.CompanyName" HeaderText="Proveedor" />
                                <asp:BoundField DataField="Category.CategoryName" HeaderText="Categoría" />
                                <asp:BoundField DataField="QuantityPerUnit" HeaderText="Cantidad por Unidad" />
                                <asp:BoundField DataField="UnitPrice" HeaderText="Precio Unitario" />
                                <asp:BoundField DataField="UnitsInStock" HeaderText="Unidades Disponibles" />
                                <asp:CheckBoxField DataField="Discontinued" HeaderText="Descontinuado" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
