using System.Reflection;
using System.Text;

namespace Bugsnag.Maui.Payload
{
    public class Method(MethodBase? methodBase)
    {
        public string? DisplayName()
        {
            if (methodBase == null)
            {
                return null;
            }

            var builder = new StringBuilder();

            var type = methodBase.DeclaringType;

            if (type != null)
            {
                var declaringTypeName = TypeNameHelper.GetTypeDisplayName(
                    type,
                    includeGenericParameterNames: true
                );
                if (!string.IsNullOrEmpty(declaringTypeName))
                {
                    builder.Append(declaringTypeName).Append('.');
                }
            }

            builder.Append(methodBase.Name);

            if (methodBase.IsGenericMethod)
            {
                var genericArguments = string.Join(
                    ", ",
                    methodBase
                        .GetGenericArguments()
                        .Select(arg =>
                            TypeNameHelper.GetTypeDisplayName(
                                arg,
                                fullName: false,
                                includeGenericParameterNames: true
                            )
                        )
                        .ToArray()
                );
                builder.Append('<').Append(genericArguments).Append('>');
            }

            var parameters = methodBase
                .GetParameters()
                .Select(p => new MethodParameter(p).DisplayName())
                .ToArray();
            builder.Append('(');
            builder.Append(string.Join(", ", parameters));
            builder.Append(')');

            return builder.ToString();
        }
    }
}
