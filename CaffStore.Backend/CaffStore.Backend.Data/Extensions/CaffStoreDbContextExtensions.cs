using CaffStore.Backend.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace CaffStore.Backend.Dal.Extensions
{
	public static class CaffStoreDbContextExtensions
	{
		public static void RegisterSoftDeleteQueryFilter(this ModelBuilder modelBuilder)
		{
			var entityTypes = modelBuilder
				.Model
				.GetEntityTypes()
				.Where(e => typeof(ISoftDeletableEntity).IsAssignableFrom(e.ClrType));

			foreach (var entityType in entityTypes)
			{
				var parameter = Expression.Parameter(entityType.ClrType);

				var property = Expression.Property(parameter, nameof(ISoftDeletableEntity.IsDeleted));
				var binaryExpression = Expression.MakeBinary(ExpressionType.Equal, property, Expression.Constant(false));
				var filter = Expression.Lambda(binaryExpression, parameter);

				modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
			}
		}
	}
}
