using System;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArch.Application.Members.Commands.Notifications;

public class MemberCreatedSMSHandler : INotificationHandler<MemberCreatedNotification>
{
    private readonly ILogger<MemberCreatedSMSHandler> _logger;

    public MemberCreatedSMSHandler(ILogger<MemberCreatedSMSHandler>? logger)
    {
        _logger = logger;
    }

    public Task Handle(MemberCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Confirmation SMS sent for: {notification.Member.LastName}");

        return Task.CompletedTask;
    }
}
