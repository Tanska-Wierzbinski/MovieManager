using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MovieManager.Interfaces;
using MovieManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Security
{
    public class CanEditOnlyOwnReview : AuthorizationHandler<ManageReviewAuthorNameRequirement>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public CanEditOnlyOwnReview(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageReviewAuthorNameRequirement requirement)
        {
            var authFilterContext = context.Resource as Endpoint;

            string loggedInUserName = context.User.Identity.Name;

            string reviewIdBeingEdited = _contextAccessor.HttpContext.Request.Path;

            var review = _unitOfWork.Review.Get(Int32.Parse(reviewIdBeingEdited.Split('/').Last()));

            if ((loggedInUserName == review.Result.Author) || (context.User.IsInRole("Admin")))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
