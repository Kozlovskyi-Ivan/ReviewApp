﻿using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext context;

        public ReviewerRepository(DataContext context)
        {
            this.context = context;
        }
        public Reviewer GetReviewer(int reviewerId)
        {
            return context.Reviewers.Where(r=>r.Id==reviewerId).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return context.Reviewers.ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return context.Reviewers.Any(r => r.Id == reviewerId);
        }
    }
}
