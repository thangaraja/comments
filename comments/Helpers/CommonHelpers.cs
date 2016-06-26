using System;

namespace CommentSystems.Helpers
{
    public static class CommonHelpers
    {
        public static bool IsNotEmptyGuid(string value)
        {
            Guid outId = Guid.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                return (Guid.TryParse(value, out outId) && (!outId.Equals(Guid.Empty)));
            }
            return false;
        }

        public static bool IsNotEmptyGuid(Guid? value)
        {
            return (value != null && !value.Value.Equals(Guid.Empty));
        }

        public static bool IsNotEmptyGuid(Guid value)
        {
            return (!value.Equals(Guid.Empty));
        }

        public static bool IsEmptyGuid(Guid value)
        {
            return (value.Equals(Guid.Empty));
        }
    }
}