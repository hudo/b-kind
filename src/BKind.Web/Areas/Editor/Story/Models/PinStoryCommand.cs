﻿using BKind.Web.Core;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Areas.Editor.Story.Models
{
    public class PinStoryCommand : IRequest<Response>
    {
        public PinStoryCommand(string slug, User userWithRoles)
        {
            Slug = slug;
            UserWithRoles = userWithRoles;
        }

        public string Slug { get; set; }
        public User UserWithRoles { get; set; }
    }
}