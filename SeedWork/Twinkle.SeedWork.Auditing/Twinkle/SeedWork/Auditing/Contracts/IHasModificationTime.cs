namespace Twinkle.SeedWork.Auditing.Contracts;

public interface IHasModificationTime
{
    DateTime? ModificationTime { get; set; }
}