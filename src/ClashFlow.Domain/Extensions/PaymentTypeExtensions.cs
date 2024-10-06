using CashFlow.Domain.Enums;
using CashFlow.Domain.Reports.MessageResource;

namespace CashFlow.Domain.Extensions
{
    public static class PaymentTypeExtensions
    {
        public static string PaymentTypeToString(this PaymentTypeEnum paymentType)
        {
            return paymentType switch
            {
                PaymentTypeEnum.Cash => PaymentTypeMessageResource.CASH,
                PaymentTypeEnum.CreditCard => PaymentTypeMessageResource.CREDIT_CARD,
                PaymentTypeEnum.EletronicTransfer => PaymentTypeMessageResource.ELETRONIC_TRANSFER,
                PaymentTypeEnum.DebitCard => PaymentTypeMessageResource.DEBIT_CARD,
                _ => string.Empty
            };
        }

    }
}
