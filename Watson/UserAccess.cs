using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watson
{
   public  enum UserAccess
    {ANYONE,HALF_USER,
         FULL_USER
        
        
     
        
    }

    public class UserAccessAttr : Attribute
    {

        public static UserAccess GetByAccess(int access)
        {
            if(access == 2)
            {
                return UserAccess.FULL_USER;
            } else if(access == 1)
            {
                return UserAccess.HALF_USER;
            } else
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
    }


}
