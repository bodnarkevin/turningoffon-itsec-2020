﻿using CaffStore.Backend.Dal;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.RequestContext;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CaffStore.Backend.Test.Fixtures
{
	public class DatabaseFixture
	{
		public CaffStoreDbContext Context { get; }

		public DatabaseFixture(ITimeService timeService, IHttpRequestContext requestContext)
		{
			var options = new DbContextOptionsBuilder<CaffStoreDbContext>()
				.UseInMemoryDatabase("CaffStore")
				.Options;

			Context = new CaffStoreDbContext(options, timeService, requestContext);

			var user = new User
			{
				Id = 1,
				Email = "test@test",
				EmailConfirmed = false,
				PhoneNumberConfirmed = false,
				TwoFactorEnabled = false,
				LockoutEnabled = false,
				AccessFailedCount = 0,
				FirstName = "Test",
				LastName = "Name",
				IsDeleted = false
			};

			Context.Users.Add(user);

			var caffItem = new CaffItem
			{
				Id = 1,
				Title = "Test Title",
				Description = "Test Description",
				CaffData = new CaffData
				{
					Creation = DateTime.Now,
					Creator = "Test Creator",
					Animations = new List<CaffAnimationData>
					{
						new CaffAnimationData
						{
							Order = 0,
							Duration = 100,
							CiffData = new CiffData
							{
								Width = 300,
								Height = 500,
								Caption = "Test Caption",
								Tags = new List<CiffDataTag>
								{
									new CiffDataTag {Tag = new Tag {Text = "Test Tag"}}
								}
							}
						}
					}
				}
			};

			Context.CaffItems.Add(caffItem);
			Context.SaveChanges();
		}
	};
}