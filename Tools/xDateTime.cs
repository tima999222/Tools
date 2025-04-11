namespace Tools
{
    public static class xDateTime
    {
        public static string ToddMMyyyy(this DateTime dt)
        {
            return dt.ToString("ddMMyyyy");
        }

        public static string ToyyyyMMdd(this DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }
    }
}
