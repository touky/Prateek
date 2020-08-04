namespace Prateek.Runtime.Core.Extensions
{
    using System;
    using System.Reflection;
    using System.Text;

    public static class TypeExtensions
    {
        #region Class Methods
        public static string ToDebugString(this Type type)
        {
            var useSeparator = false;
            var builder      = new StringBuilder();
            builder.Append(type.Name);

            var typeInfo     = type.GetTypeInfo();
            if (typeInfo.GenericTypeArguments.Length > 0)
            {
                builder.Append("<");
                foreach (var argument in typeInfo.GenericTypeArguments)
                {
                    if (useSeparator)
                    {
                        builder.Append("/");
                    }

                    builder.Append(argument.Name);
                    useSeparator = true;
                }

                builder.Append(">");
            }

            return builder.ToString();
        }
        #endregion
    }
}
