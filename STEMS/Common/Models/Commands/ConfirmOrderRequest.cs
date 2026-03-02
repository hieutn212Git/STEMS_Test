namespace Common.Models.Commands
{
    public class ConfirmOrderRequest : BaseCommand
    {
        public string Id { get; set; }

        public string Status { get; set; }

        public string PaymentMethod { get; set; }

        public string PaymentInfo { get; set; }
    }
}
