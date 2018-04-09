using System.Collections.Generic;
using BKind.Web.Features.Shared;
using BKind.Web.Model;

namespace BKind.Web.Features.Stories.Contracts
{
    public class StoryListModel : ViewModelBase
    {
        private readonly string _photoDomain;

        public StoryListModel(IList<StoryProjection> stories, User userWithRoles, string photoDomain,  string title = null)
        {
            _photoDomain = photoDomain;
            Stories = stories;
            UserWithRoles = userWithRoles;
            Title = title;
        }

        public IList<StoryProjection> Stories { get; }
        public User UserWithRoles { get; }

        public string GenerateUrl(StoryProjection story) => $"{_photoDomain}/{story.Photo}";
    }
}