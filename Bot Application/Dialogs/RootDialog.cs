using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Bot_Application.Services;
using Bot_Application.Serialization;

namespace Bot_Application.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync1(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
        //    private static async void Response(IDialogContext context, Activity message)
        //{
          //  Activity activity = new Activity();
            var response = await Luis.GetResponse(activity.Text);

            if (response != null)
            {
                var intent = new Intent();
                var entity = new Entity1();

                string productType = string.Empty;
                string product = string.Empty;
                string available = string.Empty;
                string agendaResult = string.Empty;

                foreach (var item in response.entities)
                {
                    switch (item.type)
                    {
                        case "phone":
							product = item.entity;
							productType = item.type;
							available = item.entity;
							break;
                        case "camera":
                            available = item.entity;
							product = item.entity;
							productType = item.type;
							break;
						case "books":
							product = item.entity;
							available = item.entity;
							productType = item.type;
							break;
						case "electronics":
							available = item.entity;
							product = item.entity;
							productType = item.type;
							break;
						case "emotion::happiness":
							available = item.entity;
							product = item.entity;
							productType = item.type;
							break;
						case "emotion::sadness":
							available = item.entity;
							product = item.entity;
							productType = item.type;
							break;
						case "emotion::fear":
							available = item.entity;
							product = item.entity;
							productType = item.type;
							break;
					}
                }

                if (!string.IsNullOrEmpty(product))
                {
                    if (!string.IsNullOrEmpty(available))
                        // return our reply to the user
                        await context.PostAsync($"Hello, {product} available in category type {productType}");
                   // resposta = message.CreateReplyMessage($"OK! Entendi, mostrando {agendaInf} de {pessoa}");
                    else
                        await context.PostAsync($"Hello, Its not available.");
                    //resposta = message.CreateReplyMessage("Not available for " + pessoa + ".");
                }
                else
                    await context.PostAsync($"No result found");
            }
            
        }
    }
}