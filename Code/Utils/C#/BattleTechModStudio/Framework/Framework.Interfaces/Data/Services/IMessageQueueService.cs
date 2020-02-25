﻿using Framework.Domain.Queue;

namespace Framework.Interfaces.Data.Services
{
    public interface IMessageQueueService
    {
        long CreateMessageAudit(MessageAudit messageAudit);

        void UpdateMessageProcessedStats(string messageId, MessageStatus messageStatus, long processingTime);

        void CreateMessageAuditException(MessageProcessingError processingError);

        MessageAudit GetMessageAuditById(long id);
    }
}