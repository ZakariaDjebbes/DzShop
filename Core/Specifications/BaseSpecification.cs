﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
	public class BaseSpecification<T> : ISpecification<T>
	{
		public BaseSpecification()
		{
		}

		public BaseSpecification(Expression<Func<T, bool>> criteria)
		{
			Criteria = criteria;
		}

		public Expression<Func<T, bool>> Criteria { get; }

		public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

		public Expression<Func<T, object>> OrderBy { get; private set; }

		public Expression<Func<T, object>> OrderByDescending { get; private set; }

		public int Take { get; private set; }

		public int Skip { get; private set; }

		public bool IsPagingEnabled { get; private set; }

		protected void AddInclude(Expression<Func<T, object>> expression)
		{
			Includes.Add(expression);
		}

		protected void SetOrderBy(Expression<Func<T, object>> expression)
		{
			OrderBy = expression;
		}

		protected void SetOrderByDescending(Expression<Func<T, object>> expression)
		{
			OrderByDescending = expression;
		}

		protected void ApplyPaging(int skip, int take)
		{
			Take = take;
			Skip = skip;
			IsPagingEnabled = true;
		}
	}
}