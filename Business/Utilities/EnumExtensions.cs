using System;
using System.ComponentModel;
using System.Reflection;

namespace Business.Utilities
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public static class EnumExtensions
    {
        public static string GetDescription<TEnum>(this TEnum value) where TEnum : struct
        {
            FieldInfo field = typeof(TEnum).GetField(value.ToString());
            DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }

}
