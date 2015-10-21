namespace Zanshin.Domain.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether [is sub class of generic] [the specified child].
        /// </summary>
        /// <param name="child">The child.</param>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public static bool IsSubClassOfGeneric(this Type child, Type parent)
        {
            if (child == parent)
            {
                return false;
            }

            if (child.IsSubclassOf(parent))
            {
                return true;
            }

            var parameters = parent.GetGenericArguments();
            bool isParameterLessGeneric;

            try
            {
                isParameterLessGeneric =
                    !(parameters.Length > 0 &&
                        ((parameters[0].Attributes & TypeAttributes.BeforeFieldInit) == TypeAttributes.BeforeFieldInit));
            }
            catch (OverflowException)
            {
                isParameterLessGeneric = false;
            }
            
            while (child != null && child != typeof(object))
            {
                var cur = GetFullTypeDefinition(child);
                if (parent == cur || (isParameterLessGeneric &&
                                      cur.GetInterfaces().Select(GetFullTypeDefinition).Contains(GetFullTypeDefinition(parent))))
                {
                    return true;
                }
                if (!isParameterLessGeneric)
                {
                    if (GetFullTypeDefinition(parent) == cur && !cur.IsInterface)
                    {
                        if (VerifyGenericArguments(GetFullTypeDefinition(parent), cur))
                            return true;
                    }
                    else
                    {
                        foreach (var item in child.GetInterfaces().Where(i => GetFullTypeDefinition(parent) == GetFullTypeDefinition(i)))
                        {
                            if (VerifyGenericArguments(parent, item))
                            {
                                return true;
                            }
                        }
                    }
                }

                child = child.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Gets the full type definition.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Type GetFullTypeDefinition(this Type type)
        {
            return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        }

        /// <summary>
        /// Verifies the generic arguments.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="child">The child.</param>
        /// <returns></returns>
        public static bool VerifyGenericArguments(this Type parent, Type child)
        {
            Type[] childArguments = child.GetGenericArguments();
            Type[] parentArguments = parent.GetGenericArguments();

            if (childArguments.Length == parentArguments.Length)
            {
                for (int i = 0; i < childArguments.Length; i++)
                {
                    if ((childArguments[i].Assembly == parentArguments[i].Assembly)
                        && (childArguments[i].Name == parentArguments[i].Name)
                        && (childArguments[i].Namespace == parentArguments[i].Namespace))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}