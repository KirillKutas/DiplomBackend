using Newtonsoft.Json;

namespace DbProject.Helpers
{
    public static class ResponseCreator
    {
        public static string Create(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
