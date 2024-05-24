using Microsoft.EntityFrameworkCore;

namespace Database;

public class BaseEntityContext : DbContext
{
    /// <summary>
    /// 
    /// </summary>
    protected bool IsUseLazyLoading { get; set; }

    protected string ConnectionString { get; set;}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isUseLazyLoading"></param>
    public BaseEntityContext(bool isUseLazyLoading)
    {
        IsUseLazyLoading = isUseLazyLoading;
    }

    public BaseEntityContext()
    {
    }
}