namespace TransLight.Common
{
    public static class SessionExtensions
    {
        public static void SetCompanyId(
            this ISession session,
            Guid companyId)
        {
            session.SetString(
                "CompanyId",
                companyId.ToString());
        }

        public static Guid? GetCompanyId(
            this ISession session)
        {
            var id = session.GetString("CompanyId");

            return Guid.TryParse(id, out Guid companyId)
                ? companyId
                : null;
        }
    }
}
