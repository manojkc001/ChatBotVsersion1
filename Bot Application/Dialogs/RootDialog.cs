using Bot_Application.Serialization;
using Bot_Application.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            var response = await Luis.GetResponse(activity.Text);
            if (response != null)
            {
                List<string> entities = new List<string>();
                foreach (var item in response.entities)
                {
                    entities.Add(item.type);
                }
                var matchedTag = FindXmlConfigurationMatchingNode(entities);

                // Later to remove if else else if
                if (matchedTag.Classic)
                {
                    var infoToUser = CreditCardDetails(Constant.Classic); 
                    if (matchedTag.Benefits || matchedTag.Rewards)
                    { await context.PostAsync($" {infoToUser.ClassicCard.ClassicInformation.Rewards}"); }
                    else if (matchedTag.CreditLimit || matchedTag.CardLimit)
                    { await context.PostAsync($" The Card limit for a Classic Card is  {infoToUser.ClassicCard.ClassicInformation.CardLimit} AUD"); }
                    else if (matchedTag.Eligibility)
                    { await context.PostAsync($"To be eligible for a Classic Card{infoToUser.ClassicCard.ClassicInformation.Eligibility}"); }
                    else if (matchedTag.InterestRate)
                    { await context.PostAsync($"{infoToUser.ClassicCard.ClassicInformation.InterestRatesForCards}"); }
                    else if (matchedTag.Fee)
                    { await context.PostAsync($"{infoToUser.ClassicCard.ClassicInformation.AnnualFees} for a classic card"); }
                    else if (matchedTag.Features || matchedTag.CreditCard || matchedTag.InfoOnCard)
                    { await context.PostAsync($"Hello, Classic card has the following features - \n\nAnnual Fees - {infoToUser.ClassicCard.ClassicInformation.AnnualFees} \n\n CardLimit is {infoToUser.ClassicCard.ClassicInformation.CardLimit} AUD \n\n To be eligible {infoToUser.ClassicCard.ClassicInformation.Eligibility} \n\n {infoToUser.ClassicCard.ClassicInformation.InterestRatesForCards}"); }
                    
                }
                else if (matchedTag.Platinum)
                {
                    var infoToUser = CreditCardDetails(Constant.Platinum); 
                    if (matchedTag.Benefits || matchedTag.Rewards)
                    { await context.PostAsync($"{infoToUser.PlatinumCard.PlatinumInformation.Rewards}"); }
                    else if (matchedTag.Eligibility)
                    { await context.PostAsync($"{infoToUser.PlatinumCard.PlatinumInformation.Eligibility}"); }
                    else if (matchedTag.CreditLimit || matchedTag.CardLimit)
                    { await context.PostAsync($"{infoToUser.PlatinumCard.PlatinumInformation.CardLimit}"); }
                    else if (matchedTag.InterestRate)
                    { await context.PostAsync($"{infoToUser.PlatinumCard.PlatinumInformation.InterestRatesForCards}"); }
                    else if (matchedTag.Fee)
                    { await context.PostAsync($"Platinum has an annual fee of {infoToUser.PlatinumCard.PlatinumInformation.AnnualFees} AUD"); }
                    else if (matchedTag.Features || matchedTag.Platinum)
                    { await context.PostAsync($"Hello, Platinum card has the following features - \n\nAnnual Fees - {infoToUser.PlatinumCard.PlatinumInformation.AnnualFees} \n\n CardLimit is {infoToUser.PlatinumCard.PlatinumInformation.CardLimit} AUD \n\n To be eligible {infoToUser.PlatinumCard.PlatinumInformation.Eligibility} \n\n {infoToUser.PlatinumCard.PlatinumInformation.InterestRatesForCards}"); }
                   
                }
                else if ((matchedTag.CreditCard && matchedTag.CCInformation) || matchedTag.CreditCard)
                {
                    var Creditcardsinfo = ReturnAvailableCards(Constant.CreditCard);
                    await context.PostAsync($"Hello,The follwing cards are offered \n\n{Creditcardsinfo} ");
                }
                else if ((matchedTag.Rewards && matchedTag.CreditCard) || (matchedTag.Benefits && matchedTag.CreditCard))
                {
                    var infoToUser = CreditCardDetails(Constant.Platinum);  
                    await context.PostAsync($"{infoToUser.PlatinumCard.PlatinumInformation.Rewards}");
                }

            }

        }

        // This needs to be taken out and use CardType from xml
        public string ReturnAvailableCards(string entity)
        {
            List<string> AvailableCards = new List<string>();
            var xdoc = LoadXmlConfiguration();
            var NodesCollection = xdoc.Descendants(entity).First().Elements().Descendants().ToList();
            foreach (var node in NodesCollection)
            {
                if (!AvailableCards.Contains(node.Parent.Name.LocalName))
                    AvailableCards.Add(node.Parent.Name.LocalName);

            }
            string combindedString = string.Join(",", AvailableCards);
            return combindedString;
        }
        public CreditCardTypes CreditCardDetails(string entity)
        {
            var index = 0;
            var xdoc = LoadXmlConfiguration();
            var cardInformation = (from elements in xdoc.Descendants(entity)
                                   select new CardInformation
                                   {
                                       AnnualFees = (string)elements.Element("AnnualFee"),
                                       Eligibility = (string)elements.Element("Eligibilty"),
                                       CardLimit = (string)elements.Element("CardLimit"),
                                       InterestRatesForCards = (string)elements.Element("InterestRates"),
                                       Rewards = (string)elements.Element("Rewards")
                                   }).ToList();
            var creditCard = new CreditCardTypes();
            if (entity.Equals(Constant.Classic))
                creditCard = new CreditCardTypes()
                {
                    ClassicCard = new Classic()
                    {
                        ClassicInformation = new CardInformation()
                        {
                            AnnualFees = cardInformation[index].AnnualFees.Trim(),
                            Eligibility = cardInformation[index].Eligibility.Trim(),
                            CardLimit = cardInformation[index].CardLimit.Trim(),
                            InterestRatesForCards = cardInformation[index].InterestRatesForCards.Trim(),
                            Rewards=cardInformation[index].Rewards.Trim()
                        }
                    }
                };

            if (entity.Equals(Constant.Platinum))
            {
                creditCard = new CreditCardTypes()
                {
                    PlatinumCard = new Platinum()
                    {
                        PlatinumInformation = new CardInformation()
                        {
                            AnnualFees = cardInformation[index].AnnualFees.Trim(),
                            Eligibility = cardInformation[index].Eligibility.Trim(),
                            CardLimit = cardInformation[index].CardLimit.Trim(),
                            InterestRatesForCards = cardInformation[index].InterestRatesForCards.Trim(),
                            Rewards = cardInformation[index].Rewards.Trim()
                        }
                    }
                };
            }
            return creditCard;
        }

        #region private methods
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