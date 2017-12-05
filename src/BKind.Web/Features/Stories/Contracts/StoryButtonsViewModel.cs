namespace BKind.Web.Features.Stories.Contracts
{
    public class StoryButtonsViewModel
    {
        public string Slug { get; set; }

        public bool CanVote { get; set; }

        public bool CanUnpublish { get; set; }

        public bool CanPublish { get; set; }

        public bool CanEdit { get; set; }
    }
}