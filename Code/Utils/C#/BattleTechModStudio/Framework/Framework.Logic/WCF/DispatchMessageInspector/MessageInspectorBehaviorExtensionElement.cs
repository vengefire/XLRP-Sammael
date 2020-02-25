using System;
using System.ServiceModel.Configuration;

namespace Framework.Logic.WCF.DispatchMessageInspector
{
    public class MessageInspectorBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType => typeof(MessageInspectorBehavior);

        protected override object CreateBehavior()
        {
            return new MessageInspectorBehavior();
        }
    }
}