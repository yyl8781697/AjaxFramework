AjaxFramework
=============

#A Framework about ajax in donnet

*Do you hate so many *.ashx file in your web project.
*Do you like visit the general c# method by ajax but not like webservice?
Now AjaxFramewok can help you do it.you only need reference the AjaxFramework.dll and Wirte the Attribute before the 
head of method,the method is change to be a web method and you can visit it by url or by ajax.
*****
Example(these method is in class "data"):
```cs
[WebMethodAttr(RequestType.Get)]
public float Add(float a, float b)
{
     return a + b;
}
```
Now you can visit this method by url:http://domain.com/data/add.ajax?a=x&b=y

Also,this Framework has supplied other Attribute like WebParameterAttr,OutputCacheAttr and so on,
```cs
[WebMethodAttr(RequestType.Get)]
[WebParameterAttr("a", typeof(float), MinValue = 5)]
[WebParameterAttr("b",typeof(float),RegexText=@"^[0-9]{1,3}[\.][0-9]{1,3}$",ErrorMsg="b must be a decmail")]
public float Add(float a, float b)
{
     return a + b;
}

[WebMethodAttr(RequestType.Get)]
[OutputCacheAttr(20)]
public string Get_Pat2()
{
     return "pat" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
}
```
You can define your Attribute only inherit the abstract class of ValidateAttr.

Oh,It is no easy.
If the returnType of method is not a sample type.the program the Serializate it to json.
```cs 
[WebMethodAttr(RequestType.Get)]
        public DataTable Get_Data()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("id");
            dt.Columns.Add("name");

            DataRow row = dt.NewRow();
            row["id"] = 1;
            row["name"] = "tom";
            dt.Rows.Add(row);

            DataRow row2 = dt.NewRow();
            row2["id"] = 2;
            row2["name"] = "peter";
            dt.Rows.Add(row2);

            return dt;
        }
```
it will show in the page for json code.
*****
Now ,How use the framwork,it is easy.
you only config it in web.config and add the httphandler of AjaxHandlerFactory
IIS6.0
<add verb="*" path="*.ajax" validate="true" type="AjaxFramework.AjaxHandlerFactory,AjaxFramework" />
IIS7.0
<add name="ajaxhandler" verb="*" preCondition="integratedMode" path="*.ajax"  type="AjaxFramework.AjaxHandlerFactory,AjaxFramework" />
*****
Do you han any question that you can write in this project.
Wecome to develop this project together.
