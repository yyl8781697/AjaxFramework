namespace AjaxFramework
{
    /// <summary>
    /// 这个仅仅用于标识是ajax类（非必须继承）  
    /// 继承该接口的类将会在编译时加载进程序集的缓存中 加速程序
    /// </summary>
    public interface IAjax:System.IDisposable
    {

    }
}
