<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImagePipe.aspx.cs" Inherits="UI_Admin_ImagePipe" %>

<%@ Register TagPrefix="igchart" Namespace="Infragistics.WebUI.UltraWebChart" Assembly="Infragistics2.WebUI.UltraWebChart.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
      <igchart:SecureImagePipe ID="SecureImagePipe1" runat="server">
      </igchart:SecureImagePipe>
    <div>
    
    </div>
    </form>
</body>
</html>
