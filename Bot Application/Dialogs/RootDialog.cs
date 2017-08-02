using Bot_Application.Serialization;
using Bot_Application.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

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


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)

        {
            var activity = await result as Activity;
            var response = await Luis.GetResponse(activity.Text);
            if (response != null)
            {
                List<string> entities = new List<string>();
                foreach (var item in response.entities)
                {
                    entities.Add(item.type);
                }
                var matchedTag = FindXmlConfigurationMatchingNode(entities);
                var infoToUser = CreditCardDetails();
                // Later to remove if else else if
                if (matchedTag.Classic)
                {
                    if (matchedTag.Benefits || matchedTag.Rewards)
                    { await context.PostAsync($" {infoToUser.Classic.ClassicInformation.Rewards}"); }
                    else if (matchedTag.CreditLimit || matchedTag.CardLimit)
                    { await context.PostAsync($" {infoToUser.Classic.ClassicInformation.CardLimit} "); }
                    else if (matchedTag.Eligibility)
                    { await context.PostAsync($" {infoToUser.Classic.ClassicInformation.Eligibility}"); }
                    else if (matchedTag.InterestRate)
                    { await context.PostAsync($"{infoToUser.Classic.ClassicInformation.InterestRates}"); }
                    else if (matchedTag.Fee)
                    { await context.PostAsync($"{infoToUser.Classic.ClassicInformation.AnnualFee} for a classic card"); }
                    else if (matchedTag.Features)
                    { await context.PostAsync($"{infoToUser.Classic.ClassicInformation.Features} "); }
                    else
                    { await context.PostAsync($"Hello, Classic card has the following features - \n\nAnnual Fees - {infoToUser.Classic.ClassicInformation.AnnualFee} \n\n CardLimit is {infoToUser.Classic.ClassicInformation.CardLimit} AUD \n\n To be eligible {infoToUser.Classic.ClassicInformation.Eligibility} \n\n {infoToUser.Classic.ClassicInformation.InterestRates}"); }

                }
                else if (matchedTag.Platinum)
                {
                    if (matchedTag.Benefits || matchedTag.Rewards)
                    { await context.PostAsync($"{infoToUser.Platinum.PlatinumInformation.Rewards}"); }
                    else if (matchedTag.Eligibility)
                    { await context.PostAsync($"{infoToUser.Platinum.PlatinumInformation.Eligibility}"); }
                    else if (matchedTag.CreditLimit || matchedTag.CardLimit)
                    { await context.PostAsync($"{infoToUser.Platinum.PlatinumInformation.CardLimit}"); }
                    else if (matchedTag.InterestRate)
                    { await context.PostAsync($"{infoToUser.Platinum.PlatinumInformation.InterestRates}"); }
                    else if (matchedTag.Fee)
                    { await context.PostAsync($" {infoToUser.Platinum.PlatinumInformation.AnnualFee} "); }
                    else if (matchedTag.Features)
                    { await context.PostAsync($"{infoToUser.Platinum.PlatinumInformation.Features} "); }
                
                    else
                    { await context.PostAsync($"Hello, Platinum card has the following features - \n\nAnnual Fees - {infoToUser.Platinum.PlatinumInformation.AnnualFee} \n\n CardLimit is {infoToUser.Platinum.PlatinumInformation.CardLimit} AUD \n\n To be eligible {infoToUser.Platinum.PlatinumInformation.Eligibility} \n\n {infoToUser.Platinum.PlatinumInformation.InterestRates}"); }
               
                }
                else if ((matchedTag.Rewards && matchedTag.CreditCard))
                {
                    await context.PostAsync($"{infoToUser.Platinum.PlatinumInformation.Rewards}");
                }
                else if ((matchedTag.Benefits && matchedTag.CreditCard))
                {
                    await context.PostAsync($"{infoToUser.Benefits}");
                }
                else if ((matchedTag.Features && matchedTag.CreditCard))
                {
                    await context.PostAsync($"{infoToUser.Features}");
                }
                else if ((matchedTag.InterestRate && matchedTag.CreditCard))
                {
                    await context.PostAsync($"{infoToUser.InterestRates}");
                }
                else if ((matchedTag.AnnualFee && matchedTag.CreditCard))
                {
                    await context.PostAsync($"{infoToUser.AnnualFee}");
                }
                else if ((matchedTag.CardLimit && matchedTag.CreditCard))
                {
                    await context.PostAsync($"{infoToUser.CardLimit}");
                }
                else if ((matchedTag.Rewards && matchedTag.CreditCard))
                {
                    await context.PostAsync($"{infoToUser.Rewards}");
                }
                else if ((matchedTag.CreditCard && matchedTag.CCInformation) || matchedTag.CreditCard)
                {
                    await context.PostAsync($" {infoToUser.CardType} ");
                }
                else
                {
                    await context.PostAsync($"Help us to query your question, refine youe question.");
                }

            }

        }

        #region private methods
        private CreditCard CreditCardDetails(string entity = "")
        {
            var index = 0;
            var xdoc = LoadXmlConfiguration();
            var seriliazer = new XmlSerializer(typeof(SensitiveDataConfigCollection));
            SensitiveDataConfigCollection sensitiveDataConfigCollection;
            using (var reader = new StreamReader(ConfigurationManager.AppSettings["SensitiveData"]))
            {
                sensitiveDataConfigCollection = (SensitiveDataConfigCollection)seriliazer.Deserialize(reader);
            }
            return sensitiveDataConfigCollection.Items[index].CreditCard;
        }

        private XDocument LoadXmlConfiguration()
        {
            var xdoc = XDocument.Load(ConfigurationManager.AppSettings["SensitiveData"]);
            return xdoc;
        }

        private TagEntity FindXmlConfigurationMatchingNode(List<string> entities)
        {
            var tagEntity = new TagEntity();
            foreach (var entity in entities)
            {
                switch (entity)
                {
                    case "CCInformation":
                        tagEntity.CCInformation = true;
                        break;
                    case "CreditCard":
                    case "Credit Card":
                    case "credit card":
                    case "creditcard":
                        tagEntity.CreditCard = true;
                        break;
                    case "credit limit":
                        tagEntity.CardLimit = true;
                        break;
                    case "Classic":
                    case "classic":
                    case "Classic credit card":
                        tagEntity.Classic = true;
                        tagEntity.InfoOnCard = true;
                        break;
                    case "Platinum":
                    case "platinum":
                    case "Platinum credit card":
                        tagEntity.Platinum = true;
                        tagEntity.Features = true;
                        break;
                    case "rewards":
                        tagEntity.Rewards = true;
                        
                        break;
                    case "eligibility":
                        tagEntity.Eligibility = true;
                        break;
                    case "interestrate":
                    case "interest rate":
                    case "interest rates":
                        tagEntity.InterestRate = true;
                        break;
                    case "fee":
                        tagEntity.Fee = true;
                        break;
                    case "features":
                        tagEntity.Features = true;
                        break;
                    default:
                        break;
                }
            }
            return tagEntity;
        }
        #endregion
    }
}