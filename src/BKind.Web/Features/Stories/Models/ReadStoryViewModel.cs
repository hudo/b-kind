using BKind.Web.Features.Shared;
using BKind.Web.Infrastructure;
using BKind.Web.Model;

namespace BKind.Web.Features.Stories.Contracts
{
    public class ReadStoryViewModel : ViewModelBase
    {
        public ReadStoryViewModel(StoryProjection story, User loggedUser)
        {
            Story = story;
            LoggedUser = loggedUser;
        }

        public StoryProjection Story { get; set; }

        public User LoggedUser { get; set; }

        // comments, votes, etc
    }
}
