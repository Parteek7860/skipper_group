
#nullable enable
namespace skipper_group_new.Models;

public class SubhomeModel
{
    public string? Name { get; set; }

    public string? linkname { get; set; }

    public string? rewriteurl { get; set; }

    public string? pageid { get; set; }

    public string? id { get; set; }

    public string ParentId { get; set; }
    public string uploadimage { get; set; }
    public string smalldesc { get; set; }

    public List<SubhomeModel2> SubMenus2 { get; set; } = new List<SubhomeModel2>();
}
public class SubhomeModel2
{
    public string? Name { get; set; }

    public string? linkname { get; set; }

    public string? rewriteurl { get; set; }

    public string? pageid { get; set; }

    public string? id { get; set; }

    public string ParentId { get; set; }
    public string uploadimage { get; set; }
    public string smalldesc { get; set; }
    public string uploadfile { get; set; }
}
