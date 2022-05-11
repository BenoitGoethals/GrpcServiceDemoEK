using ClientFactoryIntegration.ServerSide;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientFactoryIntegration.ClientSide.Pages
{
    public class GreetModel : PageModel
    {
        private readonly Greeter.GreeterClient client;
        [BindProperty]
        public string Name { get; set; }
        public string Message { get; set; }

        public GreetModel(Greeter.GreeterClient client) {
            this.client = client;
        }
        public void OnGet() {

        }
        public async Task OnPostAsync() {
            Message = (await client.SayHelloAsync(new HelloRequest() { Name = Name })).Message;
        }
    }
}
