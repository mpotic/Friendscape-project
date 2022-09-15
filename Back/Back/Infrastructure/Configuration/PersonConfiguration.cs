using Back.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Infrastructure.Configuration
{
	public class PersonConfiguration : IEntityTypeConfiguration<Person>
	{
		public void Configure(EntityTypeBuilder<Person> builder)
		{
			builder.HasKey(x => x.Id);  //Setting the primary key
			builder.Property(x => x.Id).ValueGeneratedOnAdd();

			builder.HasMany(x => x.Friendships)
				.WithOne(x => x.User)
				.HasForeignKey(x => x.UserId);

			builder.HasMany(x => x.Friends)
				.WithOne(x => x.Friend)
				.HasForeignKey(x => x.FriendId)
				.OnDelete(DeleteBehavior.Restrict);


			builder.HasIndex(x => x.Email).IsUnique();
		}
	}
}
