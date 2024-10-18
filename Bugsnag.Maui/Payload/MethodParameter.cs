using System.Reflection;
using System.Text;

namespace Bugsnag.Maui.Payload
{
    public class MethodParameter(ParameterInfo parameterInfo)
    {
        public string DisplayName()
        {
            var builder = new StringBuilder();
            var type = parameterInfo.ParameterType;

            if (parameterInfo.IsOut)
            {
                builder.Append("out ");
            }
            else if (type.IsByRef)
            {
                builder.Append("ref ");
            }

            if (type.IsByRef)
            {
                type = type.GetElementType()!;
            }

            var parameterTypeString = TypeNameHelper.GetTypeDisplayName(
                type,
                fullName: false,
                includeGenericParameterNames: true
            );

            builder.Append(parameterTypeString).Append(' ').Append(parameterInfo.Name);

            return builder.ToString();
        }
    }
}
