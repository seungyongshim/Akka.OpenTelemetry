using Akka.Actor;
using Akka.Actor.Internal;
using Akka.Dispatch;
using Akka.OpenTelemetry.Cell;

namespace Akka.OpenTelemetry.Local.ActorRefs;

public class OpenTelemetryLocalActorRef : LocalActorRef
{
    private readonly OpenTelemetrySettings _settings;

    public OpenTelemetryLocalActorRef(OpenTelemetrySettings settings, ActorSystemImpl system, Props props, MessageDispatcher dispatcher, MailboxType mailboxType, IInternalActorRef supervisor, ActorPath path) : base(system, props, dispatcher, mailboxType, supervisor, path)
    {
        _settings = settings;
    }

    protected override void TellInternal(object message, IActorRef sender)
    {
        var envelope = TraceTell.TellInternal(message, Props);
        base.TellInternal(envelope, sender);
    }

    protected override ActorCell NewActorCell(ActorSystemImpl system, IInternalActorRef self, Props props, MessageDispatcher dispatcher,
        IInternalActorRef supervisor)
    {
        return new OpenTelemetryActorCell(_settings, system, self, props, dispatcher, supervisor);
    }
}