using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.Enums
{
    public class DescriptionAttribute : Attribute
    {
        public string DisplayFr { get; set; }
        public DescriptionAttribute(string displayFr)
        {
            DisplayFr = displayFr;
        }

        public static string GetValue<T>(T genre)
        {
            Type enumType = typeof(T);
            MemberInfo[] memberInfos =
                enumType.GetMember(genre.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m =>
                m.DeclaringType == enumType);
            var valueAttributes =
                enumValueMemberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return ((DescriptionAttribute)valueAttributes[0]).DisplayFr;
        }
    }
}

