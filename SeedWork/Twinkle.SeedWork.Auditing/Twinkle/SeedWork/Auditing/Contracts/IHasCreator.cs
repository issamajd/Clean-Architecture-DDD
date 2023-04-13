namespace Twinkle.SeedWork.Auditing.Contracts;

public interface IHasCreator
{
    Guid CreatorId { get; set; }
}