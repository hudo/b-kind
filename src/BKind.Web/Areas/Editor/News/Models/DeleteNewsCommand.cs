﻿using BKind.Web.Core;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Areas.Editor.News.Models
{
    public class DeleteNewsCommand : IRequest<Response>
    {
        public DeleteNewsCommand(int id, User loggedUser)
        {
            Id = id;
            LoggedUser = loggedUser;
        }

        public int Id { get; set; }
        public User LoggedUser { get; set; }
    }
}