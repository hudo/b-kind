using BKind.Web.Model;
using BKind.Web.ViewModels;

namespace BKind.Web.Features.Stories.Contracts
{
    public class ReadStoryViewModel : FormModelBase
    {
        public ReadStoryViewModel(Story story)
        {
            Story = story;
        }

        public Story Story { get; set; }

        // comments, votes, etc
    }
}
