using System.ComponentModel;

namespace Domain.Core.Extensions;

public static class EnumExtensions
{
    public static string GetEnumDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        if (field != null)
        {
            DescriptionAttribute? attribute = Attribute.GetCustomAttribute(
                field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attribute != null)
            {
                return attribute.Description;
            }
        }

        return value.ToString();
    }
}