using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Dal.Extensions;
using CaffStore.Backend.Interface.Bll.RequestContext;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaffStore.Backend.Dal
{
	public class CaffStoreDbContext : IdentityDbContext<User, Role, int>
	{
		private readonly ITimeService _timeService;
		private readonly IHttpRequestContext _requestContext;

		public CaffStoreDbContext(DbContextOptions<CaffStoreDbContext> options, ITimeService timeService, IHttpRequestContext requestContext) : base(options)
		{
			_timeService = timeService;
			_requestContext = requestContext;
		}

		// Adding entities
		public DbSet<File> Files { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.RegisterSoftDeleteQueryFilter();
		}

		public override int SaveChanges()
		{
			SaveChangesCore();
			return base.SaveChanges();
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			SaveChangesCore();
			return base.SaveChangesAsync(cancellationToken);
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			SaveChangesCore();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			SaveChangesCore();
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		private void SaveChangesCore()
		{
			var utcNow = _timeService.UtcNow;

			var deletedEntries = ChangeTracker
				.Entries()
				.Where(e => e.Entity is ISoftDeletableEntity)
				.Where(e => e.State == EntityState.Deleted);

			UpdateDeletedEntries(deletedEntries);

			var modifiedEntries = ChangeTracker
				.Entries()
				.Where(e => e.Entity is IAuditableEntity)
				.Where(e => e.State == EntityState.Modified);

			UpdateModifiedEntries(modifiedEntries, utcNow);

			var addedEntries = ChangeTracker
				.Entries()
				.Where(e => e.Entity is IAuditableEntity)
				.Where(e => e.State == EntityState.Added);

			UpdateAddedEntries(addedEntries, utcNow);
		}

		private void UpdateDeletedEntries(IEnumerable<EntityEntry> deletedEntries)
		{
			foreach (var entry in deletedEntries)
			{
				var entity = (ISoftDeletableEntity)entry.Entity;

				entity.IsDeleted = true;

				entry.State = EntityState.Modified;
			}
		}

		private void UpdateModifiedEntries(IEnumerable<EntityEntry> modifiedEntries, DateTimeOffset utcNow)
		{
			foreach (var entry in modifiedEntries)
			{
				var entity = (IAuditableEntity)entry.Entity;

				entity.LastModifiedAt = utcNow;
				entity.LastModifiedById = _requestContext.CurrentUserId;
			}
		}

		private void UpdateAddedEntries(IEnumerable<EntityEntry> addedEntries, DateTimeOffset utcNow)
		{
			foreach (var entry in addedEntries)
			{
				var entity = (IAuditableEntity)entry.Entity;

				entity.CreatedAt = utcNow;
				entity.CreatedById = _requestContext.CurrentUserId;

				entity.LastModifiedAt = utcNow;
				entity.LastModifiedById = _requestContext.CurrentUserId;
			}
		}
	}
}
