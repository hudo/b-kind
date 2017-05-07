namespace BKind.Web.Features.Stories
{
    public class StoryButtonsViewModel
    {
        public int StoryId { get; set; }

        public bool CanVote { get; set; }

        public bool CanUnpublish { get; set; }

        public bool CanEdit { get; set; }
    }
}