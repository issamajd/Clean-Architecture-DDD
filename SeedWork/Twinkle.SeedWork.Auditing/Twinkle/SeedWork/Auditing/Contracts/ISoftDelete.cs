namespace Twinkle.SeedWork.Auditing.Contracts;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}