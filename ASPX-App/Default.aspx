<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Forms.Default" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <title>Subir imágenes</title>
    <link rel="stylesheet" href="<%= ResolveClientUrl("~/css/index.css") %>" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-group">
            <h1>Redimensionamiento de imagen</h1>
            <%--<label>
                <asp:RadioButton ID="SaveDisk" runat="server" GroupName="save"
                    Checked="true" />
                Guardar imagen en disco
            </label>--%>
            <%--<label>
                <asp:RadioButton ID="SaveDb" runat="server" GroupName="save" />
                Guardar imagen en base de datos
           
            </label>--%>
        </div>
        <p></p>
        <div>
            <asp:Label AssociatedControlID="FileUploader" runat="server" Text="Selecciona una imagen jpg:" />

            <asp:FileUpload ID="FileUploader" runat="server" />
        </div>
        <p></p>
        <div>
            <asp:Button ID="btnCargar" runat="server"
                Text="Cargar" OnClick="btnCargar_Click" />
        </div>
        <p></p>
        <div class="message">
            <asp:Literal ID="Message" runat="server" />
        </div>

        <div class="message">
            <asp:Literal ID="msOrientacion" runat="server" />
        </div>




    </form>
</body>
</html>
