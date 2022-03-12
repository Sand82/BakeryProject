﻿using Bakery.Models.Bakeries;

namespace Bakery.Service
{
    public interface IAuthorService
    {
        AuthorViewModel GetAuthorInfo();

        bool IsAuthor(string userId);
    }
}