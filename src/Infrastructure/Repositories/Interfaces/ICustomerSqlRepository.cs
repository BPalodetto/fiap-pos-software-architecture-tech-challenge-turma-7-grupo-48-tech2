﻿using System.Linq.Expressions;
using Infrastructure.SqlModels;

namespace Infrastructure.Repositories.Interfaces;

public interface ICustomerSqlRepository
{
	Task<CustomerSqlModel?> GetAsync(
		Expression<Func<CustomerSqlModel, bool>> expression,
		CancellationToken cancellationToken
	);
}