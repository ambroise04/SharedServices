using SharedServices.DAL;

namespace SharedServices.BL.Domain
{
    public class Feedback
    {
        public int Id { get; set; }
        public int Mark { get; set; }
        public string Comment { get; set; }
        public ApplicationUser User { get; set; }
        public string Advisor { get; set; }
        public bool DisplayAdvisorName { get; set; }
    }
}