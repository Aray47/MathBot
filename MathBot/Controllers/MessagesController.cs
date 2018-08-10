using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace dialogs_basic
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new MathsDialog());
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }
    }

    [Serializable]
    public class MathsDialog : IDialog<object>
    {
        // Bot Framework manages automatically persists per conversation data
        protected int number1 { get; set; }

        //the beginning
        public async Task StartAsync(IDialogContext context)
        {
            //context.Wait will wait until a message is recieved, once that happens -> MessageReceievedStart method will begin
            context.Wait(MessageReceivedStart);
        }
        public async Task MessageReceivedStart(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            //context.PostAsync posts to the user, asking them what operation to be played out - Output
            await context.PostAsync("Hi! Welcome to MathBot! \nDo you want to add, square root, or squared?");

            //context.Wait is waiting for the response from the previous question - Input
            context.Wait(MessageReceivedOperationChoice);
        }

        //decision time:
        public async Task MessageReceivedOperationChoice(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            //if the user chooses to add:
            if (message.Text.ToLower().Equals("add", StringComparison.InvariantCultureIgnoreCase))
            {
                //he is then prompted to provide number one
                await context.PostAsync("Provide number one:");
                //bot will wait for response and call new method that is prompted after first number selected
                context.Wait(MessageReceivedAddNumber1);
            }
            //if user chooses square root:
            else if (message.Text.ToLower().Equals("square root", StringComparison.InvariantCultureIgnoreCase))
            {
                //only provide one number
                await context.PostAsync("Provide one number:");
                //and sent it to square root method
                context.Wait(MessageReceivedSquareRoot);
            }
            //if user chose squared:
            else if (message.Text.ToLower().Equals("squared", StringComparison.InvariantCultureIgnoreCase))
            {
                //prompt user for one number
                await context.PostAsync("Provide one number:");
                //and send it to message receieved squared method
                context.Wait(MessageReceivedSquared);
            }
            else
            {
                //else if nothing else, start over to the beginning
                context.Wait(MessageReceivedStart);
            }
        }

        public async Task MessageReceivedAddNumber1(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var numbers = await argument;

            // number one is persisted between messages automatically by bot framework dialog
            //number1 is a protected int from above
            //parsed Text from previous message recieved into an integer
            this.number1 = int.Parse(numbers.Text);

            //after first number is receieved, the second one is required here:
            await context.PostAsync("Provide number two:");

            //and send it to MessageRecievedAddNumber2 method
            context.Wait(MessageReceivedAddNumber2);
        }

        //second method after user enters first number
        public async Task MessageReceivedAddNumber2(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var numbers = await argument;

            //parsing previous message to variable named number2, or - retrieving the value passed in
            var number2 = int.Parse(numbers.Text);

            await context.PostAsync($"{this.number1} + {number2} is = {this.number1 + number2}");

            context.Wait(MessageReceivedStart);
        }

        public async Task MessageReceivedSquareRoot(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var number = await argument;
            var num = double.Parse(number.Text);

            await context.PostAsync($"square root of {num} is {Math.Sqrt(num)}");

            context.Wait(MessageReceivedStart);
        }

        public async Task MessageReceivedSquared(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var activity = await argument;
            var num = double.Parse(activity.Text);

            await context.PostAsync($"{num} squared is {num * num}");

            context.Wait(MessageReceivedStart);
        }
    }
}