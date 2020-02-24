﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Data.Models;
using Forum.Web.Models.Forum;
using Forum.Web.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class ForumController : Controller
    {
        #region "Fields"

        private readonly IForum _forumService;

        #endregion

        #region "Constructor"

        public ForumController(IForum forumService)
        {
            _forumService = forumService;
        }

        #endregion

        #region "Action Methods"

        /// <summary>
        /// Gets all forum topics.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var forums = _forumService.GetAll()
                .Select(forum => new ForumListingModel
                {
                    Id = forum.Id,
                    Name = forum.Title,
                    Description = forum.Description
                });

            var model = new ForumIndexModel
            {
                ForumList = forums
            };

            return View(model);
        }

        /// <summary>
        /// Gets a forum topic by id.
        /// </summary>
        /// <param name="id">The topic id.</param>
        /// <returns></returns>
        public IActionResult Topic(int id)
        {
            var forum = _forumService.GetById(id);
            var posts = forum.Posts;

            var postListings = posts.Select(post => new PostListingModel
            {
                Id = post.Id,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                AuthorName = post.User.UserName,
                Title = post.Title,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = BuildForumListing(post)
            });

            var model = new ForumTopicModel
            {
                Posts = postListings,
                Forum = BuildForumListing(forum)
            };

            return View(model);
        }

        #endregion

        #region "Helper Methods"

        /// <summary>
        /// Builds a forum listing model.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <returns></returns>
        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;
            return BuildForumListing(forum);
        }

        /// <summary>
        /// Builds a forum listing model.
        /// </summary>
        /// <param name="forum">The forum.</param>
        /// <returns></returns>
        private ForumListingModel BuildForumListing(ForumEntity forum)
        {
            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl
            };
        }

        #endregion
    }
}