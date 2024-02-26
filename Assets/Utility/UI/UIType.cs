
public class UIType
{
    private string name;
    public string Name
    {
        get => name;
    }
    private string path;
    public string Path
    {
        get => path;
    }

    /// <summary>
    /// 初始化ui信息
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="uiPath"></param>
    public UIType(string uiName, string uiPath)
    {
        name = uiName;
        path = uiPath;
    }

}
