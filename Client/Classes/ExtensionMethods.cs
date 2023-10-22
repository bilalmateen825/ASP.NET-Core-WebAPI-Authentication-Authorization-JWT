using System.Security.Claims;

namespace Client.Classes
{
    public static class ExtensionMethods
    {
        public static bool HasEmployementDate(this ClaimsPrincipal User)
        {
            return !User.HasClaim(x => x.Type == Constants.EmployementDateClaimName) ? false : true;
        }

        public static bool IsAdminUser(this ClaimsPrincipal User)
        {
            return !User.HasClaim(x => x.Type == Constants.AdministrationUserClaimName) ? false : true;
        }

        public static bool IsEmployee(this ClaimsPrincipal User)
        {
            return !User.HasClaim(x => x.Type == Constants.EmployeeUserClaimName) ? false : true;
        }

        public static bool IsEligibleToViewPageEmpoyeePage(this ClaimsPrincipal User)
        {

            if (User.IsAdminUser() || User.IsEmployee())
                return true;

            return false;
        }

    }
}
