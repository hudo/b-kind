using BKind.Web.Model;
using BKind.Web.ViewModels;

namespace BKind.Web.Features.Stories.Contracts
{
    public class ReadStoryViewModel : FormModelBase
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
