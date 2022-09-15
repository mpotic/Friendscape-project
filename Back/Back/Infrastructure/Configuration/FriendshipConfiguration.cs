using Back.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Infrastructure.Configuration
{
	public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
	{
		public void Configure(EntityTypeBuilder<Friendship> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();
		}
	}
}
