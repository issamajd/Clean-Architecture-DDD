using Twinkle.Auditing.Abstractions;
using Twinkle.SeedWork;

namespace Twinkle.Auditing.AuditLogs;

public class AuditLogStore : IAuditLogStore
{
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly AuditLogDataToAuditLogMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AuditLogStore(IAuditLogRepository auditLogRepository,
        AuditLogDataToAuditLogMapper mapper, IUnitOfWork unitOfWork)
    {
        _auditLogRepository = auditLogRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task SaveAsync(AuditLogData auditLogData)
    {
        var saveHandle = await _unitOfWork.BeginAsync();
        await _auditLogRepository.AddAsync(_mapper.GetAuditLog(auditLogData));
        await saveHandle.CompleteAsync();
    }
}