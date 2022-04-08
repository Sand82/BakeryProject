using Newtonsoft.Json;

namespace Bakery.Tests.GlobalMethods
{
    public static class TestService
    {
        public static string ConvertToJason(object obj)
        {
            var result = JsonConvert.SerializeObject(obj);

            return result;
        }
    }
}
