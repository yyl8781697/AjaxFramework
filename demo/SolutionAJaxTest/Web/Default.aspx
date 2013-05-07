<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ajaxFramework Demo</title>
</head>
<body>
    <a href="data/add.ajax?a=3.5&b=8" target="_blank">Demo1</a><br />
    <a href="data/add.ajax?a=8&b=8.32" target="_blank">Demo2</a><br />
    <a href="data/get_pat.ajax" target="_blank">Demo3</a><br />
    <a href="data/get_pat2.ajax" target="_blank">Demo4</a><br />
    <a href="data/get_data.ajax" target="_blank">Demo5</a><br />
    <a href="data/insert_user.ajax?username=test&password=admin&age=23&birthday=2013-5-7" target="_blank">Demo6</a><br />
</body>
</html>
