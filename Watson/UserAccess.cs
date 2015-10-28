using System;

namespace Watson
{
    public enum UserAccess
    {
        ANYONE,
        HALF_USER,
        FULL_USER
    }

    public class UserAccessAttr : Attribute
    {
        public static UserAccess GetByAccess(int access)
        {
            if (access == 2)
            {
                return UserAccess.FULL_USER;
            }
            else if (access == 1)
            {
                return UserAccess.HALF_USER;
            }
            else
            {
                return UserAccess.ANYONE;
            }
        }

        public static bool HasRequiredAccess(UserAccess userRights, UserAccess requiredAccess)
        {
            int userOrdinal = (int)userRights;
            int requiredOrdinal = (int)requiredAccess;
            return userOrdinal >= requiredOrdinal;
        }

        public static int GetByValue(UserAccess access)
        {
            if (access == UserAccess.FULL_USER)
            {
                return 2;
            }
            else if (access == UserAccess.HALF_USER)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
