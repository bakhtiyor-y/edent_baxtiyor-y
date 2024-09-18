namespace Edent.Api.Infrastructure
{
    public class Constants
    {
        public static class JwtClaimIdentifiers
        {
            public const string Rol = "rol", Id = "id", Roles = "roles", Claims = "claims";
        }

        public static class JwtClaims
        {
            public const string ApiAccess = "api_access";
        }

        public static class CustomClaimTypes
        {
            public const string Permission = "permission";
        }

        public static class PermissionConstants
        {
            public const string COMMON_USER_MANAGEMENT = "common.user_management";
            public const string COMMON_INVENTORY_MANAGEMENT = "common.inventory_management";
            public const string COMMON_REPORTS = "common.reports";
            public const string COMMON_DOCTOR = "common.doctor";
            public const string COMMON_MANUALS = "common.manuals";
            public const string COMMON_APP_SETTINGS = "common.app_settings";

            public const string COMMON_RECEPTION = "common.reception";
            public const string COMMON_CASHIER = "common.cashier";
            public const string COMMON_RENTGEN = "common.rentgen";


            public const string DOCTOR_VIEW_REPORTS = "doctor.view_reports";
            public const string DOCTOR_VIEW_CONTACTS = "doctor.view_contacts";
        }
    }
}
