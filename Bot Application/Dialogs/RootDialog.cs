using Bot_Application.Serialization;
using Bot_Application.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                List<string> entities = new List<string>();

                foreach (var item in response.entities)
                {
                    entities.Add(item.type);
                }
                string combindedentity = string.Join(",", entities);
                if (combindedentity.Contains("CCInformation") || combindedentity.Contains("CreditCard"))
                {
                    var Creditcardsinfo = ReturnAvailableCards("CreditCard");
                    await context.PostAsync($"Hello,The follwing cards are offered \n\n{Creditcardsinfo} ");
                }
                if (combindedentity.Contains("Classic"))
                {
                    var InfoToUser = ReturnInfoForClassic("Classic");

                    if (combindedentity.Contains("rewards"))
                    {
                        await context.PostAsync($"Hello,Currently there are no rewards on Classic. Opt Platinum card for rewards.");
                    }
                    else if (combindedentity.Contains("eligibility"))
                    {
                        await context.PostAsync($"Hello,To be eligible for a Classic Card {InfoToUser.ClassicInformation.Eligibility} ");
                    }
                    else if (combindedentity.Contains("credit limit"))
                    {
                        await context.PostAsync($"Hello,The credit limit for a Classic Card is {InfoToUser.ClassicInformation.CardLimit} ");
                    }
                    else if (combindedentity.Contains("interestrate"))
                    {
                        await context.PostAsync($"Hello, {InfoToUser.ClassicInformation.InterestRatesForCards} ");
                    }
                    else if (combindedentity.Contains("fee"))
                    {
                        await context.PostAsync($"Hello,We are happy to tell you that  Classic card has {InfoToUser.ClassicInformation.AnnualFees} ");
                    }
                    else if (combindedentity.Contains("features") || combindedentity.Contains("credit card"))
                    {
                        await context.PostAsync($"Hello, Classic card has the following features - \n\nAnnual Fees - {InfoToUser.ClassicInformation.AnnualFees} \n\n CardLimit is {InfoToUser.ClassicInformation.CardLimit} AUD \n\n To be eligible {InfoToUser.ClassicInformation.Eligibility} \n\n {InfoToUser.ClassicInformation.InterestRatesForCards}");
                    }

                }
                if (combindedentity.Contains("Platinum"))
                {
                    var InfoToUser = ReturnInfoForPlatinum("Platinum");
                    if (combindedentity.Contains("rewards") || combindedentity.Contains("benefits"))
                    {
                        await context.PostAsync($"Hello, You can get upto 1% cashback maximum upto 30 AUD.");
                    }

                   else if (combindedentity.Contains("features")|| combindedentity.Contains("credit card"))
                    {
                        await context.PostAsync($"Hello, Platinum card has the following features - \n\nAnnual Fees - {InfoToUser.PlatinumInformation.AnnualFees} \n\n CardLimit is {InfoToUser.PlatinumInformation.CardLimit} AUD \n\n To be eligible {InfoToUser.PlatinumInformation.Eligibility} \n\n {InfoToUser.PlatinumInformation.InterestRatesForCards}");
                    }
                    else if (combindedentity.Contains("eligibility"))
                    {
                        await context.PostAsync($"Hello,To be eligible for a Platinum Card {InfoToUser.PlatinumInformation.Eligibility} ");
                    }
                    else if (combindedentity.Contains("credit limit"))
                    {
                        await context.PostAsync($"Hello,The credit limit for a Platinum Card is {InfoToUser.PlatinumInformation.CardLimit} AUD ");
                    }
                    else if (combindedentity.Contains("interestrate"))
                    {
                        await context.PostAsync($"Hello, {InfoToUser.PlatinumInformation.InterestRatesForCards} ");
                    }
                    else if (combindedentity.Contains("fee"))
                    {
                        await context.PostAsync($"Hello,Platinum card has an Annual fee of {InfoToUser.PlatinumInformation.AnnualFees} AUD");
                    }
                    
                }

                //switch (item.type)
                //{

                //    case "electronics":
                //        available = item.entity;
                //        product = item.entity;
                //        productType = item.type;
                //        break;
                //    case "classic features":
                //        productType = item.type;
                //        //("Classic");
                //        break;

                //    case "CCInformation":
                //        available = "CreditCard";
                //        List<string> CardsOffered = new List<string>();
                //        CardsOffered = ReturnAvailableCards(available);
                //        string combindedString = string.Join(",", CardsOffered.ToArray());

                //        //ReturnAvailbleCards()
                //        break;
                //}


                //if (!string.IsNullOrEmpty(product))
                //{
                //    if (!string.IsNullOrEmpty(available))
                //        // return our reply to the user
                //        await context.PostAsync($"Hello, {product} available in category type {productType}");
                //    // resposta = message.CreateReplyMessage($"OK! Entendi, mostrando {agendaInf} de {pessoa}");
                //    else
                //        await context.PostAsync($"Hello, Its not available.");
                //    //resposta = message.CreateReplyMessage("Not available for " + pessoa + ".");
                //}
                //else
                //    await context.PostAsync($"No result found");


            }

        }

        public string ReturnAvailableCards(string entity)
        {
            List<string> AvailableCards = new List<string>();
            var xdoc = XDocument.Load("C:\\Path\\SensitiveDataMapping.xml");
            var NodesCollection = xdoc.Descendants(entity).First().Elements().Descendants().ToList();
            foreach (var node in NodesCollection)
            {
                if (!AvailableCards.Contains(node.Parent.Name.LocalName))
                    AvailableCards.Add(node.Parent.Name.LocalName);
               
            }
            string combindedString = string.Join(",", AvailableCards);
            return combindedString;
        }

        public Platinum ReturnInfoForPlatinum(string entity, string compositeentity = "")
        {
            Platinum platinumcard = new Platinum();
            InfoForCards platinum = new InfoForCards();
            
            var xdoc = XDocument.Load("C:\\Path\\SensitiveDataMapping.xml");
            var platinumFeatures = from elements in xdoc.Descendants("Platinum")
                                   select new InfoForCards
                                   {
                                       AnnualFees = (string)elements.Element("AnnualFee"),
                                       Eligibility = (string)elements.Element("Eligibilty"),
                                       CardLimit = (string)elements.Element("CardLimit"),
                                       InterestRatesForCards = (string)elements.Element("InterestRates")
                                   };

            foreach (var unit in platinumFeatures)
            {
                platinum.AnnualFees = unit.AnnualFees.Trim();
                platinum.Eligibility = unit.Eligibility.Trim();
                platinum.CardLimit = unit.CardLimit.Trim();
                platinum.InterestRatesForCards = unit.InterestRatesForCards.Trim();
                platinumcard.PlatinumInformation = platinum;
            }
            return platinumcard;
        }
        public Classic ReturnInfoForClassic(string entity, string compositeentity = "")
        {
            Classic classiccard = new Classic();
            InfoForCards classic = new InfoForCards();

            var xdoc = XDocument.Load("C:\\Path\\SensitiveDataMapping.xml");
            var platinumFeatures = from elements in xdoc.Descendants("Classic")
                                   select new InfoForCards
                                   {
                                       AnnualFees = (string)elements.Element("AnnualFee"),
                                       Eligibility = (string)elements.Element("Eligibilty"),
                                       CardLimit = (string)elements.Element("CardLimit"),
                                       InterestRatesForCards = (string)elements.Element("InterestRates")
                                   };

            foreach (var unit in platinumFeatures)
            {
                classic.AnnualFees = unit.AnnualFees.Trim();
                classic.Eligibility = unit.Eligibility.Trim();
                classic.CardLimit = unit.CardLimit.Trim();
                classic.InterestRatesForCards = unit.InterestRatesForCards.Trim();
                classiccard.ClassicInformation = classic;
            }
            return classiccard;
        }
    }
}