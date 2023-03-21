namespace DDD.Core.Utils;

//TODO move it to another project
public interface IIdentityService<out TKey>
{
    /// <summary>
    /// Get the current user id
    /// </summary>
    /// <returns>User Id of the specified type or an exception if not possible</returns>
    public TKey GetUserId();
}