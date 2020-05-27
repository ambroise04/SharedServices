namespace SharedServices.UI.Models
{
    public class RequestFormViewModel
    {
        public int ServiceId { get; set; }
        public int OperationPoint { get; set; }
        public string TargetId { get; set; }
        public string TargetFullName { get; set; }
        public string ServiceTitle { get; set; }
        public string TargetUserPictureSource { get; set; }
    }
}