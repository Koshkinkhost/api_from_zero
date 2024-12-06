namespace api_from_zero.ViewModels
{
    public class RegistrationResponse
    {
        public string Token {  get; set; }
        public bool Status { get; set; }
       
        public Dictionary<string, string[]> messages { get; set; }
    }
}
